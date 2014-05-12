using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TyMetrix360.Core;
using TyMetrix360.Core.ViewModelBase;
using TyMetrix360.App.Common;
using TyMetrix360.BusinessObjects.Invoice;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core.Models;

namespace TyMetrix360.App.ViewModel
{
    public class FailureViewModel : ViewModelCore
    {
        private bool isWarning;

        private string headerText;
        public string HeaderText
        {
            get { return headerText; }
            set { SetProperty(ref headerText, value); }
        }
        private string failureTitle;
        public string FailureTitle
        {
            get { return failureTitle; }
            set { SetProperty(ref failureTitle, value); }
        }
        public List<ApproveErrorItem> approveErrorItems;
        public List<ApproveErrorItem> ApproveErrorItems
        {
            get { return approveErrorItems; }
            set
            {
                SetProperty(ref approveErrorItems, value);
                Messenger.Default.Send<List<ApproveErrorItem>>(approveErrorItems, Constants.AddApproveItems);
            }
        }
        private List<InvoiceListDisplayFields> _invoiceList;
        public List<InvoiceListDisplayFields> InvoiceList
        {
            get { return _invoiceList; }
            set { SetProperty(ref _invoiceList, value); }
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

        public void InitializeCommands(bool IsWarning)
        {
            isWarning = IsWarning;

            CancelCommand = new RelayCommand(e => Close());
            ProceedCommand = new RelayCommand(e => Proceed());
        }

        private void Close()
        {
            Messenger.Default.Send<string>(string.Empty, Constants.CloseConfirmationPopup);
        }

        private void Proceed()
        {
            if (isWarning)
            {
                ApproveInvoiceWithWarning();
            }
            else
            {
                Messenger.Default.Send<string>(string.Empty, Constants.CloseConfirmationPopup);
            }
        }

        private async void ApproveInvoiceWithWarning()
        {
            try
            {
                Dictionary<string, object> selectedInvoiceIds = new Dictionary<string, object>();
                selectedInvoiceIds.Add(Constants.SelectedInvoiceIds, GetSelectedIds(InvoiceList));
                selectedInvoiceIds.Add(Constants.ForceApprove, true);

                await ServiceInvoker.Instance.InvokeServiceUsingPost<string>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.ApproveMultiInvoiceService), selectedInvoiceIds, true, false);
                Messenger.Default.Send<string>(string.Empty, Constants.RefreshInvoiceList);
                Messenger.Default.Send<string>(string.Empty, Constants.CloseConfirmationPopup);
                Messenger.Default.Send<string>(string.Empty, Constants.RemoveMultiSelect);
            }
            catch (T360Exception te)
            {
                List<Error> errors = te.ErrorCodes;
                Error error = errors[0];
                if (Constants.Failure.ToUpper().Equals(error.Code.ToUpper()))
                {
                    isWarning = false;
                    HeaderText = "Approve Invoices";
                    FailureTitle = "Failed (" + GetFailedCount(errors) + ")";
                    ApproveErrorItems = GetApproveErrorItems(errors);
                }
                else
                {
                    string msg = getMessages(te);
                    ShowErrorMessage(msg, Constants.ApproveError);
                }
            }
        }

        private List<ApproveErrorItem> GetApproveErrorItems(List<Error> errors)
        {
            List<ApproveErrorItem> items = new List<ApproveErrorItem>();
            ApproveErrorItem item;
            bool isFailure = Constants.Failure.ToUpper().Equals(errors[0].Code.ToUpper());
            for (int i = 1; i < errors.Count; i++)
            {
                item = new ApproveErrorItem();
                string msg = T360ErrorCodes.GetError(errors[i].Code);
                item.Message = string.IsNullOrEmpty(msg) ? errors[i].Code : msg;
                item.InvoiceNumbers = new List<InvoiceNumber>();
                foreach (string data in errors[i].Data)
                {
                    item.InvoiceNumbers.Add(new InvoiceNumber() { InvoiceNo = data });
                }
                items.Add(item);
            }
            return items;
        }

        private int GetFailedCount(List<Error> errors)
        {
            int count = 0;
            for (int i = 1; i < errors.Count; i++)
            {
                count += errors[i].Data.Count;
            }
            return count;
        }

        private List<string> GetSelectedIds(List<InvoiceListDisplayFields> selectedInvoices)
        {
            List<string> selectedIds = new List<string>();
            selectedIds.AddRange(selectedInvoices.Select(x => x.InvoiceId.ToString()));
            return selectedIds;
        }
    }
}
