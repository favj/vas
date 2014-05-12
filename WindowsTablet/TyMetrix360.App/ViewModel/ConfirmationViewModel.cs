using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TyMetrix360.App.Common;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core;
using TyMetrix360.Core.Models;
using TyMetrix360.Core.ViewModelBase;
using TyMetrix360.BusinessObjects.Invoice;

namespace TyMetrix360.App.ViewModel
{
    public class ConfirmationViewModel : ViewModelCore
    {
        private string _headerText;
        public string HeaderText
        {
            get { return _headerText; }
            set { SetProperty(ref _headerText, value); }
        }
        private string _totalAmount;
        public string TotalAmount
        {
            get { return _totalAmount; }
            set { SetProperty(ref _totalAmount, value); }
        }
        private List<InvoiceListDisplayFields> _confirmationList;
        public List<InvoiceListDisplayFields> ConfirmationList
        {
            get { return _confirmationList; }
            set { SetProperty(ref _confirmationList, value); }
        }
        private IRelayCommand _cancelCommand;
        public IRelayCommand CancelCommand
        {
            get { return _cancelCommand; }
            set { SetProperty(ref _cancelCommand, value); }
        }
        private IRelayCommand _proceedCommand;
        public IRelayCommand ProceedCommand
        {
            get { return _proceedCommand; }
            set { SetProperty(ref _proceedCommand, value); }
        }

        public void InitializeCommands(ConfirmationSource source)
        {
            CancelCommand = new RelayCommand(e => Close());
            ProceedCommand = new RelayCommand(e => Proceed(source));
        }

        private void Close()
        {
            Messenger.Default.Send<string>(string.Empty, Constants.CloseConfirmationPopup);
        }

        private void Proceed(ConfirmationSource source)
        {
            if (ConfirmationSource.Invoice_Approve == source)
            {
                ApproveInvoice();
            }
        }

        private async void ApproveInvoice()
        {
            try
            {
                bool isOk = await ShowConfirmationMessage(Constants.ApproveMultiConfirmationMsg, Constants.ApproveTitle);
                if (!isOk) return;

                Messenger.Default.Send<string>(string.Empty, Constants.ShowPopupAfterConfirmation);

                IsBusy = true;

                Dictionary<string, object> selectedInvoiceIds = new Dictionary<string, object>();
                selectedInvoiceIds.Add(Constants.SelectedInvoiceIds, GetSelectedIds(ConfirmationList));
                selectedInvoiceIds.Add(Constants.ForceApprove, false);

                await ServiceInvoker.Instance.InvokeServiceUsingPost<string>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.ApproveMultiInvoiceService) , selectedInvoiceIds, true, false);

                Messenger.Default.Send<string>(string.Empty, Constants.RefreshInvoiceList);
                Messenger.Default.Send<string>(string.Empty, Constants.CloseConfirmationPopup);
                Messenger.Default.Send<string>(string.Empty, Constants.RemoveMultiSelect);

                IsBusy = false;
            }
            catch (T360Exception te)
            {
                InvoiceErrorDetails InvoiceErrorDetails;
                List<Error> errors = te.ErrorCodes;
                Error error = errors[0];
                if (Constants.Disallow.ToUpper().Equals(error.Code.ToUpper()))
                {
                    InvoiceErrorDetails = new InvoiceErrorDetails()
                    {
                        PageType = Constants.Disallow,
                        ErrorDetails = errors,
                        Header = "Approve Invoices"
                    };
                }
                else if (Constants.Warning.ToUpper().Equals(error.Code.ToUpper()))
                {
                    InvoiceErrorDetails = new InvoiceErrorDetails()
                    {
                        PageType = Constants.Warning,
                        ErrorDetails = errors,
                        Header = "Approve Invoices",
                        InvoiceBasicDetails = ConfirmationList
                    };
                }
                else
                {
                    InvoiceErrorDetails = new InvoiceErrorDetails()
                    {
                        PageType = Constants.Failed,
                        ErrorDetails = errors,
                        Header = "Approve Invoices"
                    };
                }
                Messenger.Default.Send<InvoiceErrorDetails>(InvoiceErrorDetails, Constants.InvoiceErrorDetails);
            }
        }

        private List<string> GetSelectedIds(List<InvoiceListDisplayFields> selectedInvoices)
        {
            List<string> selectedIds = new List<string>();
            selectedIds.AddRange(selectedInvoices.Select(x => x.InvoiceId.ToString()));
            return selectedIds;
        }

        public enum ConfirmationSource
        {
            Invoice_Approve,
            Invoice_Reject,
            Lineitem_Adjust,
            Lineitem_Reject
        }
    }
}
