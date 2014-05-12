/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using TyMetrix360.App.CommandParameters;
using TyMetrix360.App.Common;
using TyMetrix360.App.Navigation;
using TyMetrix360.App.Validator;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.BusinessObjects.Invoice;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core;
using TyMetrix360.Core.Container;
using TyMetrix360.Core.Models;
using TyMetrix360.Core.ViewModelBase;
using TyMetrix360.Dto.Invoice;

namespace TyMetrix360.App.ViewModel
{
    public class RejectionViewModel : ViewModelCore, IRejectionViewModel
    {
        public RejectionViewModel()
        {
            OKCommand = new RelayCommand(e => { SaveRejection(); });
            ClearCommand = new RelayCommand(e => { ClearAll(); });
            GoToInvoiceSummaryCommand = new RelayCommand(e => { GoToSummaryPage(); });
        }

        private IRelayCommand _okCommand;
        public IRelayCommand OKCommand
        {
            get { return _okCommand; }
            set { SetProperty(ref _okCommand, value); }
        }
        private IRelayCommand _goToInvoiceSummaryCommand;
        public IRelayCommand GoToInvoiceSummaryCommand
        {
            get { return _goToInvoiceSummaryCommand; }
            set { SetProperty(ref _goToInvoiceSummaryCommand, value); }
        }
        private IRelayCommand _clearCommand;
        public IRelayCommand ClearCommand
        {
            get { return _clearCommand; }
            set { SetProperty(ref _clearCommand, value); }
        }
        private ObservableCollection<InvoiceSummary> _rejectedItems;
        public ObservableCollection<InvoiceSummary> RejectedItems
        {
            get { return _rejectedItems; }
            set { SetProperty(ref _rejectedItems, value); }
        }
        private int _selectedInvoice;
        public int SelectedInvoice
        {
            get { return _selectedInvoice; }
            set { SetProperty(ref _selectedInvoice, value); }
        }
        private InvoiceSummary _invoiceSummary;
        public InvoiceSummary InvoiceSummary
        {
            get { return _invoiceSummary; }
            set { SetProperty(ref _invoiceSummary, value); }
        }
        private ObservableCollection<ReasonCode> _reasonCodes;
        public ObservableCollection<ReasonCode> ReasonCodes
        {
            get { return _reasonCodes; }
            set { SetProperty(ref _reasonCodes, value); }
        }
        private InvoiceReject _rejectionData;
        public InvoiceReject RejectionData
        {
            get { return _rejectionData; }
            set { SetProperty(ref _rejectionData, value); }
        }
        private int _selectedReasonId;
        public int SelectedReasonId
        {
            get { return _selectedReasonId; }
            set { SetProperty(ref _selectedReasonId, value); }
        }
        private string _narrativeText;
        public string NarrativeText
        {
            get { return _narrativeText; }
            set { SetProperty(ref _narrativeText, value); }
        }

        public override async Task LoadData(params object[] parameters)
        {
            try
            {
                IsBusy = true;
                InvoiceSummary = parameters[0] as InvoiceSummary;

                var items = new ObservableCollection<InvoiceSummary>();
                items.Add(InvoiceSummary);
                RejectedItems = items;
                SelectedInvoice = 0;

                await SetReasonCodes();
                IsBusy = false;
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.RejectionFailed);
                if (T360ErrorCodes.NotInReviewerQueue.Equals(ex.ErrorCodes[0].Code))
                {
                    Navigator.Navigate(Destination.InvoiceListView);
                }
            }
        }

        public async Task SetReasonCodes()
        {
            try
            {
                var serializableData = new { Action = Constants.Reject };
                List<ReasonCode> rejectReasons = await ServiceInvoker.Instance.InvokeServiceUsingPost<List<ReasonCode>>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.GetReasonCodesService),serializableData,false,false);
                this.ReasonCodes = new ObservableCollection<ReasonCode>(rejectReasons);
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.ReasonsFailed);
            }
        }

        private List<ReasonCode> GetReasons(ReasonCodeDto[] reasons)
        {
            List<ReasonCode> reasonList = new List<ReasonCode>();
            reasonList.AddRange(reasons.Select(reason =>
                new ReasonCode()
                {
                    Id = reason.Id,
                    Description = reason.Description
                }));
            return reasonList;
        }

        public void GoToSummaryPage()
        {
            try
            {
                if (!ServiceInvoker.Instance.IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);

                Navigator.Navigate(Destination.InvoiceListView, ExistingViewBehavior.Remove, new object[] { InvoiceSummary });
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.RejectionFailed);
            }
        }

        public async Task SaveRejection()
        {
            try
            {
                T360Validator.ValidateRejectInvoice(SelectedReasonId, NarrativeText);

                var isOk = await ShowConfirmationMessage(Constants.RejectConfirmationMsg, Constants.RejectConfirmation);
                if (!isOk) return;

                RejectionData = new InvoiceReject();
                RejectionData.ReasonId = SelectedReasonId;
                RejectionData.NarrativeText = NarrativeText;
                RejectionData.InvoiceId = InvoiceSummary.InvoiceId;
                IsBusy = true;
                var serializableData = GetRejectSerializationData(RejectionData);
                var data = await ServiceInvoker.Instance.InvokeServiceUsingPost<string>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.RejectInvoiceService), serializableData, true, false);
                IsBusy = false;

                Messenger.Default.Send<ResetListParameter>(new ResetListParameter() { });
                Messenger.Default.Send<InvoiceParameter>(new InvoiceParameter() { Invoice = null }, Constants.SetInvoice);

                if (ServiceInvoker.Success.Equals(data))
                {
                    Navigator.Navigate(Destination.InvoiceListView);
                }
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.RejectionFailed);
            }
        }

        private object GetRejectSerializationData(InvoiceReject param)
        {
            return new
            {
                InvoiceId = param.InvoiceId,
                ReasonId = param.ReasonId,
                NarrativeText = param.NarrativeText
            };
        }

        public async Task ClearAll()
        {
            SelectedReasonId = 0;
            NarrativeText = string.Empty;
        }

        public override bool ShowAppBar
        {
            get { return false; }
        }
    }
}
