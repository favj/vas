/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;

using TyMetrix360.App.CommandParameters;
using TyMetrix360.App.Common;
using TyMetrix360.App.Navigation;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core;
using TyMetrix360.Core.Container;
using TyMetrix360.Core.Models;
using TyMetrix360.Core.ViewModelBase;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.BusinessObjects.Invoice;
using Windows.UI.ViewManagement;
using TyMetrix360.Dto.Invoice;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using TyMetrix360.App.View;

namespace TyMetrix360.App.ViewModel
{
    public class InvoiceListViewModel : ViewModelCore, IInvoiceListViewModel
    {
        private int previousSelectedIndex;

        private void ReloadListOnBack(ResetListParameter param)
        {
            ReloadList(param.InvoiceID);
        }

        private void ReloadListFromDashboard(InvoiceParameter param)
        {
            ReloadList(param.Invoice == null ? -1 : param.Invoice.InvoiceId);
        }

        public InvoiceListViewModel()
        {
            if (InvoiceDetail != null)
            {
                SetSummary(InvoiceDetail.InvoiceId);
            }
            Messenger.Default.Send<ReturnToPage>(new ReturnToPage() { PageItem = NavigationFactory.GetNavigationItem(Destination.InvoiceListView)});
            GoBackToDashboardCommand = new RelayCommand(e => CallGoBackToDashboard());
        }

        private async Task CallGoBackToDashboard()
        {
            try
            {
                if (!ServiceInvoker.Instance.IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);

                Messenger.Default.Send<string>(string.Empty, Constants.UnregisterInvoiceListEvents);
                IsBusy = true;
                Navigator.Navigate(Destination.DashboardView, OnUnloadView, parameters: new object[] { null });
                IsBusy = false;
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.InvoiceListFailed);
            }
        }

        private Action<ResetListParameter> _reloadInvoiceListOnBack;
        public Action<ResetListParameter> ReloadInvoiceListOnBack
        {
            get
            {
                if (_reloadInvoiceListOnBack == null) _reloadInvoiceListOnBack = new Action<ResetListParameter>(ReloadListOnBack);
                return _reloadInvoiceListOnBack;
            }
        }
        private Action<InvoiceParameter> _reloadInvoiceListFromDashboard;
        public Action<InvoiceParameter> ReloadInvoiceListFromDashboard
        {
            get
            {
                if (_reloadInvoiceListFromDashboard == null) _reloadInvoiceListFromDashboard = new Action<InvoiceParameter>(ReloadListFromDashboard);
                return _reloadInvoiceListFromDashboard;
            }
        }
        private Visibility _hasSelectedInvoice;
        public Visibility HasSelectedInvoice
        {
            get { return _hasSelectedInvoice; }
            set { SetProperty(ref _hasSelectedInvoice, value); }
        }
        private bool _isChildBusy;
        public bool IsChildBusy
        {
            get { return _isChildBusy; }
            set { SetProperty(ref _isChildBusy, value); }
        }       
        private int _selectedInvoice = -1;
        public int SelectedInvoice
        {
            get { return _selectedInvoice; }
            set
            {
                SetProperty(ref _selectedInvoice, value);
                if (!MultiSelect)
                {
                    previousSelectedIndex = value;
                }
            }
        }
        private int _selectedInvoiceId;
        public int SelectedInvoiceId
        {
            get { return _selectedInvoiceId; }
            set { SetProperty(ref _selectedInvoiceId, value); }
        }
        private Visibility _showList;
        public Visibility ShowList
        {
            get { return _showList; }
            set { SetProperty(ref _showList, value); }
        }
        private Visibility _showInvoice;
        public Visibility ShowInvoice
        {
            get { return _showInvoice; }
            set { SetProperty(ref _showInvoice, value); }
        }
        private Visibility _showText;
        public Visibility ShowText
        {
            get { return _showText; }
            set { SetProperty(ref _showText, value); }
        }
        private string _invoiceCount;
        public string InvoiceCount
        {
            get { return _invoiceCount; }
            set { SetProperty(ref _invoiceCount, value); }
        }
        private bool _multiSelect;
        public bool MultiSelect
        {
            get { return _multiSelect; }
            set
            {
                bool isChanged = SetProperty(ref _multiSelect, value);
                if (isChanged)
                {
                    ChangeSelectionMode();
                }
            }
        }
        private ListViewSelectionMode _invoiceListSelectionMode = ListViewSelectionMode.Single;
        public ListViewSelectionMode InvoiceListSelectionMode
        {
            get { return _invoiceListSelectionMode; }
            set { SetProperty(ref _invoiceListSelectionMode, value); }
        }
        private ObservableCollection<InvoiceListDisplayFields> _invoiceDetails;
        public ObservableCollection<InvoiceListDisplayFields> InvoiceDetails
        {
            get { return _invoiceDetails; }
            set
            {
                SetProperty(ref _invoiceDetails, value);
                if (value == null || value.Count == 0)
                {
                    InvoiceCount = "0";
                    ShowList = Visibility.Collapsed;
                    ShowText = Visibility.Visible;
                }
                else
                {
                    InvoiceCount = value.Count.ToString();
                    ShowList = Visibility.Visible;
                    ShowText = Visibility.Collapsed;
                }
                ShowInvoice = Visibility.Visible;
            }
        }
        private InvoiceSummary _invoiceDetail;
        public InvoiceSummary InvoiceDetail
        {
            get { return _invoiceDetail; }
            set
            {
                  SetProperty(ref _invoiceDetail, value);
                  Messenger.Default.Send<InvoiceParameter>(new InvoiceParameter() { Invoice = value }, Constants.SetInvoice);
                  Messenger.Default.Send<ReturnToPage>(new ReturnToPage() { PageItem = NavigationFactory.GetNavigationItem(Destination.InvoiceListView), Invoice = value });
            }
        }
        private IRelayCommand _goBackToDashboardCommand;
        public IRelayCommand GoBackToDashboardCommand
        {
            get { return _goBackToDashboardCommand; }
            set { SetProperty(ref _goBackToDashboardCommand, value); }
        }

        private void ChangeSelectionMode()
        {
            InvoiceListSelectionMode = MultiSelect ? ListViewSelectionMode.Multiple : ListViewSelectionMode.Single;
            HasSelectedInvoice = MultiSelect ? Visibility.Collapsed : Visibility.Visible;
            CurrentState = MultiSelect ? State.MULTI_SELECT : State.NONE;
            SetAppBar();
            if (!MultiSelect)
            {
                SelectedInvoice = previousSelectedIndex;
                InvoiceCount = InvoiceDetails.Count.ToString();
            }
            else
            {
                InvoiceCount = "1";
            }
        }

        private async void ReloadList(string emptyString)
        {
            ReloadList(-1);
        }

        int myNumber;
        public async void ReloadList(int InvoiceiD)
        {
            try
            {
                IsBusy = true;
                List<InvoiceListDisplayFields> displayFields = await ServiceInvoker.Instance.InvokeServiceUsingGet<List<InvoiceListDisplayFields>>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.GetAwaitingInvoiceList));
                var index = 0;
                foreach (var invoice in displayFields)
                {
                    invoice.Index = index;
                    index++;
                }

                InvoiceDetails = new ObservableCollection<InvoiceListDisplayFields>(displayFields);
                myNumber = InvoiceDetails.IndexOf(InvoiceDetails.Where(e => e.InvoiceId == InvoiceiD).FirstOrDefault());
                SelectedInvoice = (myNumber == -1 && InvoiceDetails.Count > 0) ? 0 : myNumber;

                IsBusy = false;
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.InvoiceListFailed);
            }
        }

        public string getErrorMessages(Exception ex)
        {
            return getMessages(ex);
        }

        public void showErrorMessages(string msg)
        {
            ShowErrorMessage(msg, Constants.InvoiceListFailed);
        }

        public override async Task LoadData(params object[] parameters)
        {
            RegisterEvents();
            Messenger.Default.Send<InvoiceParameter>(new InvoiceParameter() { Invoice = this.InvoiceDetail }, Constants.SetInvoiceSummary);
            InvoiceCount = string.Empty;
            ShowText = Visibility.Collapsed;
            ShowList = Visibility.Collapsed;
            ShowInvoice = Visibility.Collapsed;
            if (SelectedInvoice < 0)
            {
                HasSelectedInvoice = Visibility.Collapsed;
            }
        }

        public void OnUnloadView(object sender, EventArgs e)
        {
            UnregisterEvents();
        }

        public void OnUnloadView(string emptyString)
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            Messenger.Default.Register<ResetListParameter>(this, ReloadInvoiceListOnBack);
            Messenger.Default.Register<InvoiceParameter>(this, Constants.SendBackToInvoicePage, ReloadInvoiceListFromDashboard);
            Messenger.Default.Register<string>(this, Constants.Expand, ExpandButton);
            Messenger.Default.Register<string>(this, Constants.ToLineItemsFromInvoice, OnUnloadView);
            Messenger.Default.Register<string>(this, Constants.CancelMultiSelect, CancelMultiSelect);
            Messenger.Default.Register<string>(this, Constants.ApproveInvoice, ApproveInvoice);
            Messenger.Default.Register<string>(this, Constants.RefreshInvoiceList, ReloadList);
            Messenger.Default.Register<string>(this, Constants.RemoveMultiSelect, RemoveMultiSelect);
        }

        private void UnregisterEvents()
        {
            Messenger.Default.Unregister<ResetListParameter>(this, ReloadInvoiceListOnBack);
            Messenger.Default.Unregister<InvoiceParameter>(this, Constants.SendBackToInvoicePage, ReloadInvoiceListFromDashboard);
            Messenger.Default.Unregister<string>(this, Constants.Expand, ExpandButton);
            Messenger.Default.Unregister<string>(this, Constants.ToLineItemsFromInvoice, OnUnloadView);
            Messenger.Default.Unregister<string>(this, Constants.CancelMultiSelect, CancelMultiSelect);
            Messenger.Default.Unregister<string>(this, Constants.ApproveInvoice, ApproveInvoice);
            Messenger.Default.Unregister<string>(this, Constants.RefreshInvoiceList, ReloadList);
            Messenger.Default.Unregister<string>(this, Constants.RemoveMultiSelect, RemoveMultiSelect);
        }

        private void RemoveMultiSelect(string emptyString)
        {
            Messenger.Default.Send<string>(string.Empty, Constants.UnregisterInvoiceListEvents);
            MultiSelect = false;
            Messenger.Default.Send<string>(string.Empty, Constants.RegisterInvoiceListEvents);
        }

        private void ApproveInvoice(string emptyString)
        {
            if (MultiSelect)
            {
                CurrentState = State.CONFIRMATION_DIALOG;
                IsBusy = true;
                Messenger.Default.Send<string>(emptyString, Constants.ShowConfirmationPopup);
                IsBusy = false;
            }
            else
            {
                ApproveSingleInvoice();
            }
        }

        private async void ApproveSingleInvoice()
        {
            bool isOk = await ShowConfirmationMessage(Constants.ApproveConfirmationMsg, Constants.ApproveTitle);
            if (!isOk) return;
            try
            {
                IsBusy = true;
                InvoiceAccept invoiceAccept = new InvoiceAccept() { InvoiceId = UserPreference.Instance.SelectedInvoiceId, ForceApprove = false };
                InvoiceAccept invoiceApprove = await ServiceInvoker.Instance.InvokeServiceUsingPost<InvoiceAccept>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.ApproveInvoiceService), invoiceAccept, false, false);
                if (invoiceApprove.Status.ToLower().Equals(Constants.Warning.ToLower()))
                {
                    isOk = false;
                    isOk = await ShowConfirmationMessage(Constants.NegativeInvoiceConfirmationMsg, Constants.NegativeInvoiceTitle);
                    if (!isOk)
                    {
                        IsBusy = false;
                        return;
                    }
                    invoiceAccept = new InvoiceAccept() { InvoiceId = UserPreference.Instance.SelectedInvoiceId, ForceApprove = true };
                    InvoiceAccept finalResult = await ServiceInvoker.Instance.InvokeServiceUsingPost<InvoiceAccept>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.ApproveInvoiceService), invoiceAccept, false, false);
                    if (!finalResult.Status.ToLower().Equals(Constants.Approved))
                    {
                        ShowErrorMessage(T360ErrorCodes.UnknownErrorMsg, Constants.ApproveFailed);
                    }
                }
                IsBusy = false;
                Messenger.Default.Send<ResetListParameter>(new ResetListParameter() { });
                Messenger.Default.Send<InvoiceParameter>(new InvoiceParameter() { Invoice = null }, Constants.SetInvoice);
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.ApproveFailed);
                if (T360ErrorCodes.NotInReviewerQueue.Equals(ex.ErrorCodes[0].Code))
                {
                    Navigator.Navigate(Destination.InvoiceListView);
                }
            }
        }

        private void CancelMultiSelect(string emptyString)
        {
            MultiSelect = false;
            ChangeSelectionMode();
        }

        public void ExpandButton(string expandString)
        {
            if (ApplicationView.Value == ApplicationViewState.Snapped)
            {
                ApplicationView.TryUnsnap();
            }
        }

        public async Task<bool> SetSummary(int invoiceId)
        {
            try
            {
                IsBusy = true;
                IsChildBusy = true;
                SetAppBar();
                var serializableData = new { InvoiceId = invoiceId };
                this.InvoiceDetail = await ServiceInvoker.Instance.InvokeServiceUsingGet<InvoiceSummary>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.GetInvoiceSummaryService + invoiceId));
                SetSummaryListsFromInvoiceDetail(this.InvoiceDetail);
                IsChildBusy = false;
                IsBusy = false;
                SetAppBar();
                return true;
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.InvoiceListSummaryFailed);
                if (T360ErrorCodes.NotInReviewerQueue.Equals(ex.ErrorCodes[0].Code))
                {
                    ReloadList(-1);
                }
                return false;
            }
        }

        public void SetSummaryListsFromInvoiceDetail(InvoiceSummary invoiceDetail)
        {
            var notes = new ObservableCollection<SummaryViewSet>();
            foreach (var a in invoiceDetail.Notes)
            {
                notes.Add(new SummaryViewSet() { Key = Constants.Date, Value = a.CreatedTime, CellType = SummaryCellType.TwoColumn });
                notes.Add(new SummaryViewSet() { Key = string.Empty, Value2 = a.Description, CellType = SummaryCellType.OneColumn });
                notes.Add(new SummaryViewSet() { Key = Constants.Owner, Value = a.Creator, CellType = SummaryCellType.TwoColumn });
            }
            if (notes.Count == 0)
            {
                notes.Add(new SummaryViewSet() { Key = string.Empty, Value2 = Constants.None, CellType = SummaryCellType.OneColumn });
            }
            var commonProperties = new ObservableCollection<SummaryViewSet>();
            foreach (var a in invoiceDetail.CommonProperties)
            {
                commonProperties.Add(new SummaryViewSet() { Key = a.LabelText, Value = a.ValueText, CellType = SummaryCellType.TwoColumn, Valign = VerticalAlignment.Top, Margin = new Thickness(0, 10, 0, 0) });
            }
            if (commonProperties.Count == 0)
            {
                commonProperties.Add(new SummaryViewSet() { Key = string.Empty, Value2 = Constants.None, CellType = SummaryCellType.OneColumn });
            }
            var properties = new ObservableCollection<SummaryViewSet>();
            foreach (var a in invoiceDetail.Properties)
            {
                properties.Add(new SummaryViewSet() { Key = a.LabelText, Value = a.ValueText, CellType = SummaryCellType.TwoColumn, Valign = VerticalAlignment.Top, Margin = new Thickness(0, 10, 0, 0) });
            }
            invoiceDetail.InvoiceSummaryViewItemList = new ObservableCollection<InvoiceSummaryViewItem>();
            var reviewers = new ObservableCollection<SummaryViewSet>();
            foreach (var a in invoiceDetail.ReviewRouteList)
            {
                if (Constants.Reviewed.Equals(a.ReviewStatus) || Constants.YetToReview.Equals(a.ReviewStatus))
                {
                    reviewers.Add(new SummaryViewSet()
                    {
                        Key = a.ReviewerName,
                        SourceLeft = (Constants.Reviewed.Equals(a.ReviewStatus)) ? "ms-appx:///Assets/Checked.png"
                        : "ms-appx:///Assets/Unchecked.png",
                        CellType = SummaryCellType.TwoColumnImageLeft,
                        Margin = new Thickness(35, 0, 0, 0)
                    });
                }
                else
                {
                    reviewers.Add(new SummaryViewSet()
                    {
                        Key = a.ReviewerName,
                        TextLeft = Constants.CurrentReviewer,
                        CellType = SummaryCellType.TwoColumnTextLeft,
                        Margin = new Thickness(125, 0, 0, 0),
                        ColumnWidth = new GridLength(400, GridUnitType.Pixel)
                    });
                }
            }
            var Taxes = new ObservableCollection<SummaryViewSet>();
            foreach (var taxInfoKey in invoiceDetail.TaxList.Keys)
            {
                var taxInfoList = invoiceDetail.TaxList[taxInfoKey];
                foreach (var taxInfo in taxInfoList)
                {
                    Taxes.Add(new SummaryViewSet() { Key = string.Empty, Value2 = taxInfoKey, CellType = SummaryCellType.OneColumn });
                    Taxes.Add(new SummaryViewSet() { Key = Constants.TaxJurisdictionCode, Value = taxInfo.TaxJurisdictionCode, CellType = SummaryCellType.TwoColumn });
                    Taxes.Add(new SummaryViewSet() { Key = Constants.TaxTypeCode, Value = taxInfo.TaxTypeCode, CellType = SummaryCellType.TwoColumn });
                    Taxes.Add(new SummaryViewSet() { Key = Constants.TaxRate, Value = taxInfo.TaxRate, CellType = SummaryCellType.TwoColumn });
                    Taxes.Add(new SummaryViewSet() { Key = Constants.TaxableAmount, Value = taxInfo.TaxableAmount, CellType = SummaryCellType.TwoColumn });
                    Taxes.Add(new SummaryViewSet() { Key = Constants.TaxAmount, Value = taxInfo.TaxAmount, CellType = SummaryCellType.TwoColumn });
                }
            }
            var Flags = new ObservableCollection<SummaryViewSet>();
            foreach (var flag in invoiceDetail.Flags)
            {
                Flags.Add(new SummaryViewSet() { Key = flag.DisplayName,
                                                 SourceRight = (Constants.High.Equals(flag.Priority) ? "ms-appx:///Assets/T360_Flags_High_Priority@2x.png"
                                                 : (Constants.Low.Equals(flag.Priority)) ? "ms-appx:///Assets/T360_Flags_Low_Priority@2x.png" :
                                                 "ms-appx:///Assets/T360_Flags_Medium_Priority@2x.png"),
                                                 CellType = SummaryCellType.TwoColumnImageRight
                                                 });
            }
            if (Flags.Count == 0)
            {
                Flags.Add(new SummaryViewSet() { Key = string.Empty, Value2 = Constants.None, CellType = SummaryCellType.OneColumn });
            }


            invoiceDetail.InvoiceSummaryViewItemList.Add(
                new InvoiceSummaryViewItem()
                {
                    Header = Constants.InvoiceSummary,
                    Symbol = "",
                    SummaryViewSets = new ObservableCollection<SummaryViewSet>() {
                        new SummaryViewSet() { Key=Constants.InvoiceDate, Value = invoiceDetail.InvoiceDate, CellType = SummaryCellType.TwoColumn},
                        new SummaryViewSet() { Key=Constants.BillingPeriod, Value = invoiceDetail.BillingPeriod, CellType = SummaryCellType.TwoColumn},
                        new SummaryViewSet() { Key=Constants.TotalBilledAmount, Value = invoiceDetail.BilledAmount, CellType = SummaryCellType.TwoColumn},
                        new SummaryViewSet() { Key=Constants.NetFees, Value = invoiceDetail.BilledFees, CellType = SummaryCellType.TwoColumn},
                        new SummaryViewSet() { Key=Constants.NetExpenses, Value = invoiceDetail.BilledExpenses, CellType = SummaryCellType.TwoColumn},
                        new SummaryViewSet() { Key=Constants.NetAmount, Value = invoiceDetail.NetAmount, CellType = SummaryCellType.TwoColumn},
                        new SummaryViewSet() { Key=Constants.Flags, Value = invoiceDetail.Flags.Count.ToString(), CellType = SummaryCellType.TwoColumn},
                        new SummaryViewSet() { Key=Constants.Status, Value = invoiceDetail.Status, CellType = SummaryCellType.TwoColumn}
                    }
                });

            invoiceDetail.InvoiceSummaryViewItemList.Add(
                new InvoiceSummaryViewItem()
                {
                    Header = Constants.Flags,
                    Symbol = "",
                    SummaryViewSets = Flags
                });

            invoiceDetail.InvoiceSummaryViewItemList.Add(
                new InvoiceSummaryViewItem()
                {
                    Header = Constants.ReviewRoute,
                    Symbol = "",
                    SummaryViewSets = reviewers
                });
            invoiceDetail.InvoiceSummaryViewItemList.Add(
                new InvoiceSummaryViewItem()
                {
                    Header = Constants.FirmVendorBilling,
                    Symbol = "",
                    SummaryViewSets = new ObservableCollection<SummaryViewSet>() { 
                        new SummaryViewSet() { Key=Constants.CurrencyType, Value = invoiceDetail.CurrencyType, CellType = SummaryCellType.TwoColumn}, 
                        new SummaryViewSet() { Key=Constants.GrossAmount, Value = invoiceDetail.GrossAmount, CellType = SummaryCellType.TwoColumn}, 
                        new SummaryViewSet() { Key=Constants.VendorAdjustment, Value = invoiceDetail.VendorAdjustment, CellType = SummaryCellType.TwoColumn}, 
                        new SummaryViewSet() { Key=Constants.BilledAmount, Value = invoiceDetail.BilledAmount, CellType = SummaryCellType.TwoColumn}
                    }
                });
            invoiceDetail.InvoiceSummaryViewItemList.Add(
                new InvoiceSummaryViewItem()
                {
                    Header = Constants.InHouseReview,
                    Symbol = "",
                    SummaryViewSets = new ObservableCollection<SummaryViewSet>() { 
                        new SummaryViewSet() { Key=Constants.ReviewerAdjustments, Value = invoiceDetail.ReviewerAdjustment, CellType = SummaryCellType.TwoColumn}, 
                        new SummaryViewSet() { Key=Constants.ITPAdjustments, Value = invoiceDetail.ItpAdjustment, CellType = SummaryCellType.TwoColumn}, 
                        new SummaryViewSet() { Key=Constants.Subtotal, Value = invoiceDetail.SubTotal, CellType = SummaryCellType.TwoColumn}, 
                        new SummaryViewSet() { Key=Constants.Tax, Value = invoiceDetail.Tax, CellType = SummaryCellType.TwoColumn},
                        new SummaryViewSet() { Key=Constants.NetTotal, Value = invoiceDetail.NetTotal, CellType = SummaryCellType.TwoColumn}
                    }
                });
            invoiceDetail.InvoiceSummaryViewItemList.Add(
                new InvoiceSummaryViewItem()
                {
                    Header = Constants.Discounts,
                    Symbol = Constants.Dollar,
                    SummaryViewSets = new ObservableCollection<SummaryViewSet>() { 
                        new SummaryViewSet() { Key=Constants.ProposedCredit, Value=invoiceDetail.Credit, CellType=SummaryCellType.TwoColumn},
                        new SummaryViewSet() { Key=Constants.TotalwithCredit, Value=invoiceDetail.TotalWithCredit, CellType=SummaryCellType.TwoColumn}
                       }
                });
            
            invoiceDetail.InvoiceSummaryViewItemList.Add(
                new InvoiceSummaryViewItem()
                {
                    Header = Constants.KeyFields,
                    Symbol = "",
                    SummaryViewSets = commonProperties
                });

            if (invoiceDetail.Permissions.Properties)
            {
                invoiceDetail.InvoiceSummaryViewItemList.Add(
                    new InvoiceSummaryViewItem()
                    {
                        Header = Constants.DetailFields,
                        Symbol = "",
                        SummaryViewSets = properties
                    });
            }

            if (invoiceDetail.Permissions.Notes)
            {
                invoiceDetail.InvoiceSummaryViewItemList.Add(
                   new InvoiceSummaryViewItem()
                   {
                       Header = Constants.Notes,
                       Symbol = "",
                       SummaryViewSets = notes
                   });
            }

            if (Taxes.Count > 0)
            {
                invoiceDetail.InvoiceSummaryViewItemList.Add(
                 new InvoiceSummaryViewItem()
                 {
                     Header = Constants.Taxes,
                     Symbol = "",
                     SummaryViewSets = Taxes
                 });
            }

            var index = 0;
            foreach (var summary in InvoiceDetail.InvoiceSummaryViewItemList)
            {
                foreach (var viewSet in summary.SummaryViewSets)
                {
                    viewSet.Index = index;
                    index++;
                }
            }
            this.InvoiceDetail = invoiceDetail;
        }

        public void SetAppBar()
        {
            Messenger.Default.Send<IViewModelCore>(this, Constants.RefreshAppBar);
        }

        private bool IsSnappedView { get { return ApplicationView.Value == ApplicationViewState.Snapped; } }

        public override bool ShowAppBar
        {
            get { return !IsBusy; }
        }
        public override bool ShowSortListButton
        {
            get { return false; } //V2.1 !IsSnappedView && !MultiSelect; }
        }
        public override bool ShowCancelButton
        {
            get { return !IsSnappedView && MultiSelect; }
        }
        public override bool ShowApproveButton
        {
            get { return ((this.InvoiceDetail == null) ? true : this.InvoiceDetail.Permissions.Approve) && !IsSnappedView; }
        }
        public override bool ShowAdjustButton
        {
            get { return ((this.InvoiceDetail == null) ? true : (this.InvoiceDetail.Permissions.AdjustExpense || this.InvoiceDetail.Permissions.AdjustFee)) && !IsSnappedView; }
        }
        public override bool ShowRejectButton
        {
            get { return ((this.InvoiceDetail == null) ? true : this.InvoiceDetail.Permissions.Reject) && !IsSnappedView; }
        }
        public override bool ShowUndoButton
        {
            get { return false; }
        }
        public override bool ShowAddNotesButton
        {
            get { return false; }
        }
        public override bool ShowDetailsButton
        {
            get { return !IsSnappedView && !MultiSelect; }
        }
        public override bool ShowSelectAllButton
        {
            get { return !IsSnappedView && MultiSelect; }
        }
        public override bool ShowClearButton
        {
            get { return !IsSnappedView && MultiSelect; }
        }
        public override bool ShowDocumentsButton
        {
            get { return false; } //V2.1 !IsSnappedView && !MultiSelect; }
        }
        public override bool ShowExpandButton
        {
            get { return IsSnappedView; }
        }

        private State CurrentState { get; set; }

        private enum State
        {
            NONE,
            MULTI_SELECT,
            CONFIRMATION_DIALOG,
            SEARCH
        }
    } 
}
