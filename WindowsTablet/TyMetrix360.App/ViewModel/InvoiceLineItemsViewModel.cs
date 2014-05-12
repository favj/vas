/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

using TyMetrix360.App.CommandParameters;
using TyMetrix360.App.Common;
using TyMetrix360.App.Navigation;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.BusinessObjects.Invoice;
using TyMetrix360.BusinessObjects.LineItem;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core;
using TyMetrix360.Core.Container;
using TyMetrix360.Core.Models;
using TyMetrix360.Core.ViewModelBase;
using System.Collections.Generic;
using TyMetrix360.Dto.Invoice;
using TyMetrix360.Dto.LineItem;

namespace TyMetrix360.App.ViewModel
{
    public class InvoiceLineItemsViewModel : ViewModelCore, IInvoiceLineItemsViewModel
    {
        public bool IsSummarySelected { get { return SelectedDetail > 0; } }

        private IRelayCommand _goBackToInvoiceCommand;
        public IRelayCommand GoBackToInvoiceCommand
        {
            get { return _goBackToInvoiceCommand; }
            set { SetProperty(ref _goBackToInvoiceCommand, value); }
        }
        private ObservableCollection<LineItem> _invoiceDetails;
        public ObservableCollection<LineItem> LineItemList
        {
            get { return _invoiceDetails; }
            set { SetProperty(ref _invoiceDetails, value); }
        }
        private int _selectedDetail = -1;
        public int SelectedDetail
        {
            get { return _selectedDetail; }
            set
            {
                SetProperty(ref _selectedDetail, value);
                LoadDetail(Invoice.InvoiceId, LineItemList[SelectedDetail].LineItemId);
                Messenger.Default.Send<ReturnToPage>(new ReturnToPage() { PageItem = NavigationFactory.GetNavigationItem(Destination.InvoiceLineItemsView), LineItemId = Convert.ToInt32(LineItemList[SelectedDetail].LineItemId) });
            }
        }

        private InvoiceSummary _invoice;
        public InvoiceSummary Invoice
        {
            get { return _invoice; }
            set
            {
                SetProperty(ref _invoice, value);
            }
        }
        private LineItemDetail _lineItemDetail;
        public LineItemDetail LineItemDetails
        {
            get { return _lineItemDetail; }
            set { SetProperty(ref _lineItemDetail, value); }
        }
        public InvoiceLineItemsViewModel()
        {
            Messenger.Default.Register<InvoiceWithDetailParameter>(this, (s) =>
            {
                LoadDetail(s.InvoiceId, s.InvoiceSummaryDetailId.ToString());
            });
            GoBackToInvoiceCommand = new RelayCommand(e => CallGoBackToInvoice());
            Messenger.Default.Send<ReturnToPage>(new ReturnToPage() { PageItem = NavigationFactory.GetNavigationItem(Destination.InvoiceLineItemsView) });
        }

        private async Task CallGoBackToInvoice()
        {
            try
            {
                if (!ServiceInvoker.Instance.IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);

                IsBusy = true;
                object[] p = new object[1];
                p[0] = Invoice.InvoiceId;
                Navigator.Navigate(Destination.InvoiceListView, parameters: p);
                IsBusy = false;
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.InvoiceLineItemFailed);
                if (T360ErrorCodes.NotInReviewerQueue.Equals(ex.ErrorCodes[0].Code))
                {
                    Navigator.Navigate(Destination.InvoiceListView);
                }
            }
        }
        private bool _isChildBusy;
        public bool IsChildBusy
        {
            get { return _isChildBusy; }
            set { SetProperty(ref _isChildBusy, value); }
        }
        public async Task LoadDetail(int invoiceid, string lineItemId)
        {
            try
            {
                if (ApplicationView.Value == ApplicationViewState.Snapped)
                {
                    ApplicationView.TryUnsnap();
                }
                IsBusy = true;
                IsChildBusy = true;
                var serializableData = new { InvoiceId = invoiceid, LineItemId = lineItemId };
                this.LineItemDetails = await ServiceInvoker.Instance.InvokeServiceUsingGet<LineItemDetail>(ServiceInvoker.Instance.AppendUrl(string.Format(ServiceInvoker.GetLineItemSummary, invoiceid, lineItemId)));
                SetSummaryListsFromInvoiceDetail(this.LineItemDetails, this.Invoice);
                var a = this.LineItemDetails.LineItemId;
                IsChildBusy = false;
                IsBusy = false;
                Messenger.Default.Send<ReturnToPage>(new ReturnToPage() { PageItem = NavigationFactory.GetNavigationItem(Destination.InvoiceLineItemsView), Invoice = this.Invoice, LineItemId = this.LineItemDetails.LineItemId });
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.InvoiceLineItemDetailFailed);
                if (T360ErrorCodes.NotInReviewerQueue.Equals(ex.ErrorCodes[0].Code))
                {
                    Navigator.Navigate(Destination.InvoiceListView);
                }
            }
        }

        public override async Task LoadData(params object[] parameters)
        {
            try
            {
                var nullableInvoiceId = parameters[0] as int?;
                var invoiceId = nullableInvoiceId.HasValue ? nullableInvoiceId.Value : 0;
                var lineItemId = 0;
                if (parameters.Length > 1)
                {
                    var nullableLineItemId = parameters[1] as int?;
                    lineItemId = nullableLineItemId.HasValue ? nullableLineItemId.Value : 0;
                }
                IsBusy = true;
                List<LineItem> invoiceLineItemList = await ServiceInvoker.Instance.InvokeServiceUsingGet<List<LineItem>>(ServiceInvoker.Instance.AppendUrl(string.Format(ServiceInvoker.GetLineItemList, invoiceId)));
                this.LineItemList = new ObservableCollection<LineItem>(invoiceLineItemList);
                var index = 0;
                foreach (var lineItem in LineItemList)
                {
                    lineItem.Index = index;
                    lineItem.FlagVisible = "0".Equals(lineItem.Flags) ? Visibility.Collapsed : Visibility.Visible;
                    index++;
                }
                var serializableData = new { InvoiceId = invoiceId };
                Invoice = await ServiceInvoker.Instance.InvokeServiceUsingGet<InvoiceSummary>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.GetInvoiceSummaryService + invoiceId));
                IsBusy = false;

                if (LineItemList.Count > 0)
                {
                    var serializableLineItemData = new { InvoiceId = invoiceId, LineItemId = LineItemList[0].LineItemId };
                    this.LineItemDetails = await ServiceInvoker.Instance.InvokeServiceUsingGet<LineItemDetail>(ServiceInvoker.Instance.AppendUrl(string.Format(ServiceInvoker.GetLineItemSummary, invoiceId, LineItemList[0].LineItemId)));
                    int setIndex = this.LineItemList.IndexOf(this.LineItemList.Where(e => e.LineItemId.Equals(LineItemList[0].LineItemId.ToString())).FirstOrDefault());
                    SelectedDetail = (setIndex == -1) ? 0 : setIndex;
                }
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.InvoiceLineItemDetailFailed);
                if (T360ErrorCodes.NotInReviewerQueue.Equals(ex.ErrorCodes[0].Code))
                {
                    Navigator.Navigate(Destination.InvoiceListView);
                }
            }
        }

        public void SetSummaryListsFromInvoiceDetail(LineItemDetail lineItemDetail, InvoiceSummary invoice)
        {
            try
            {
                var flags = new ObservableCollection<SummaryViewSet>();
                foreach (var eachFlag in lineItemDetail.Flagslist)
                {
                    flags.Add(new SummaryViewSet()
                    {
                        Key = eachFlag.WarningInfo,
                        SourceRight = (Constants.LineHigh.Equals(eachFlag.Priority) ? "ms-appx:///Assets/T360_Flags_High_Priority@2x.png"
                        : (Constants.LineLow.Equals(eachFlag.Priority)) ? "ms-appx:///Assets/T360_Flags_Low_Priority@2x.png" :
                        "ms-appx:///Assets/T360_Flags_Medium_Priority@2x.png"),
                        CellType = SummaryCellType.TwoColumnImageRight
                    });
                }
                if (flags.Count == 0)
                {
                    flags.Add(new SummaryViewSet() { Key = string.Empty, Value2 = Constants.None, CellType = SummaryCellType.OneColumn });
                }

                var notes = new ObservableCollection<SummaryViewSet>();
                foreach (var a in lineItemDetail.NotesList)
                {
                    notes.Add(new SummaryViewSet() { Key = Constants.Date, Value = a.CreatedTime, CellType = SummaryCellType.TwoColumn });
                    notes.Add(new SummaryViewSet() { Key = string.Empty, Value2 = a.Description, CellType = SummaryCellType.TwoColumn });
                    notes.Add(new SummaryViewSet() { Key = Constants.Owner, Value = a.Creator, CellType = SummaryCellType.TwoColumn });
                }
                if (notes.Count == 0)
                {
                    notes.Add(new SummaryViewSet() { Key = string.Empty, Value2 = Constants.None, CellType = SummaryCellType.OneColumn });
                }

                var Taxes = new ObservableCollection<SummaryViewSet>();
                foreach (var taxInfoKey in lineItemDetail.Taxlist.Keys)
                {
                    var taxInfoList = lineItemDetail.Taxlist[taxInfoKey];
                    foreach (var taxInfo in taxInfoList)
                    {
                        Taxes.Add(new SummaryViewSet() { Key = string.Empty, Value2 = taxInfoKey, CellType = SummaryCellType.OneColumn });
                        Taxes.Add(new SummaryViewSet() { Key = Constants.TaxType, Value = taxInfo.TaxJurisdictionCode, CellType = SummaryCellType.TwoColumn });
                        Taxes.Add(new SummaryViewSet() { Key = Constants.TaxJurisdiction, Value = taxInfo.TaxTypeCode, CellType = SummaryCellType.TwoColumn });
                        Taxes.Add(new SummaryViewSet() { Key = Constants.TaxRate, Value = taxInfo.TaxRate, CellType = SummaryCellType.TwoColumn });
                    }
                }

                SummaryViewSet viewSet4 = new SummaryViewSet() { Key = Constants.Date, Value = lineItemDetail.Date, CellType = SummaryCellType.TwoColumn };
                SummaryViewSet viewSet5 = new SummaryViewSet() { Key = Constants.Amount, Value = lineItemDetail.NetAmount, CellType = SummaryCellType.TwoColumn };
                SummaryViewSet viewSet6 = new SummaryViewSet() { Key = Constants.TimeKeeper, Value = lineItemDetail.TimeKeeper, CellType = SummaryCellType.TwoColumn };

                var summaryViewItems = new ObservableCollection<SummaryViewSet>();
                summaryViewItems.Add(viewSet4);
                if (!string.IsNullOrWhiteSpace(lineItemDetail.TimeKeeper)) summaryViewItems.Add(viewSet6);
                summaryViewItems.Add(viewSet5);

                InvoiceSummaryViewItem item = new InvoiceSummaryViewItem();
                item.Header = Constants.General;
                item.Symbol = "";
                item.SummaryViewSets = summaryViewItems;

                var items = new ObservableCollection<InvoiceSummaryViewItem>();

                items.Add(item);

                items.Add(
                    new InvoiceSummaryViewItem()
                    {
                        Header = Constants.Narrative,
                        Symbol = "",
                        SummaryViewSets = new ObservableCollection<SummaryViewSet>() { 
                        new SummaryViewSet() {Key= string.Empty, Value2 = lineItemDetail.Narrative, CellType=SummaryCellType.OneColumn} 
                        }
                    });

                items.Add(
                    new InvoiceSummaryViewItem()
                    {
                        Header = Constants.FirmVendorBilling,
                        Symbol = "",
                        SummaryViewSets = new ObservableCollection<SummaryViewSet>() { 
                            new SummaryViewSet() { Key=Constants.Task, Value = lineItemDetail.VendorTask, CellType=SummaryCellType.TwoColumn}, 
                            new SummaryViewSet() { Key=Constants.Activity, Value = lineItemDetail.VendorActivity, CellType=SummaryCellType.TwoColumn}, 
                            new SummaryViewSet() { Key=Constants.UnitHours, Value = lineItemDetail.VendorUnits, CellType=SummaryCellType.TwoColumn}, 
                            new SummaryViewSet() { Key=Constants.Rate, Value = lineItemDetail.VendorRate, CellType=SummaryCellType.TwoColumn}, 
                            new SummaryViewSet() { Key=Constants.VendorAdjustment, Value = lineItemDetail.VendorAdjustment, CellType=SummaryCellType.TwoColumn}, 
                            new SummaryViewSet() { Key=Constants.BilledTotal, Value = lineItemDetail.VendorBilledTotal, CellType=SummaryCellType.TwoColumn}
                        }
                    });

                items.Add(
                    new InvoiceSummaryViewItem()
                    {
                        Header = Constants.Flags,
                        Symbol = "",
                        SummaryViewSets = flags

                    });

                SummaryViewSet viewSet1 = new SummaryViewSet() { Key = Constants.ITPAdjustment, Value = lineItemDetail.ItpAdjustment, CellType = SummaryCellType.TwoColumn };
                SummaryViewSet viewSet2 = new SummaryViewSet() { Key = Constants.ReviewerAdjustment, Value = lineItemDetail.ReviewerAdjustment, CellType = SummaryCellType.TwoColumn };
                SummaryViewSet viewSet3 = new SummaryViewSet() { Key = Constants.NetTotal, Value = lineItemDetail.NetTotal, CellType = SummaryCellType.TwoColumn };

                var summaryViewSets = new ObservableCollection<SummaryViewSet>();
                summaryViewSets.Add(viewSet1);
                summaryViewSets.Add(viewSet2);
                summaryViewSets.Add(viewSet3);

                items.Add(
                  new InvoiceSummaryViewItem()
                  {
                      Header = Constants.InHouseReview,
                      Symbol = "",
                      SummaryViewSets = summaryViewSets
                  });

                var adjustmentData = lineItemDetail.AdjustmentsList.GroupBy(adjustment => adjustment.GroupDescription);

                foreach (var adjustment in adjustmentData)
                {
                    var adjustmentSet = new ObservableCollection<SummaryViewSet>();
                    foreach (var eachItem in adjustment)
                    {
                        adjustmentSet = AddSummaryViewData(eachItem, adjustmentSet);
                    }
                    items.Add(
                        new InvoiceSummaryViewItem()
                        {
                            Header = adjustment.Key,
                            Symbol = "",
                            SummaryViewSets = adjustmentSet
                        });
                }

                if (lineItemDetail.AdjustmentsList.Count == 0)
                {
                    var adjustments = new ObservableCollection<SummaryViewSet>();
                    adjustments.Add(new SummaryViewSet() { Key = string.Empty, Value2 = Constants.None, CellType = SummaryCellType.OneColumn });
                    items.Add(
                        new InvoiceSummaryViewItem()
                        {
                            Header = Constants.Adjustments,
                            Symbol = "",
                            SummaryViewSets = adjustments
                        });
                }

                if (lineItemDetail.Permissions.Notes)
                {
                    items.Add(
                        new InvoiceSummaryViewItem()
                        {
                            Header = Constants.Notes,
                            Symbol = "",
                            SummaryViewSets = notes
                        });
                }

                if (Taxes.Count > 0)
                {
                    items.Add(
                        new InvoiceSummaryViewItem()
                        {
                            Header = Constants.Taxes,
                            Symbol = "",
                            SummaryViewSets = Taxes

                        });
                }

                invoice.InvoiceSummaryViewItemList = items;

                var index = 0;
                foreach (var summary in invoice.InvoiceSummaryViewItemList)
                {
                    foreach (var viewSet in summary.SummaryViewSets)
                    {
                        viewSet.Index = index;
                        index++;
                    }
                }

                this.Invoice = invoice;
            }
            catch (T360Exception e)
            {
                string message = getMessages(e);
                ShowErrorMessage(message, Constants.InvoiceLineItemDetailFailed);
                if (T360ErrorCodes.NotInReviewerQueue.Equals(e.ErrorCodes[0].Code))
                {
                    Navigator.Navigate(Destination.InvoiceListView);
                }
            }
            catch (Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.InvoiceLineItemDetailFailed);
            }
         }

        private ObservableCollection<SummaryViewSet> AddSummaryViewData(AdjustmentNote adjustment, ObservableCollection<SummaryViewSet> adjustmentSet)
        {
            adjustmentSet.Add(new SummaryViewSet() { Key = Constants.Owner, Value = adjustment.Owner, CellType = SummaryCellType.TwoColumn});
            adjustmentSet.Add(new SummaryViewSet() { Key = Constants.Description, Value = adjustment.Description, CellType = SummaryCellType.TwoColumn});
            adjustmentSet.Add(new SummaryViewSet() { Key = Constants.Amount, Value = adjustment.Amount, CellType = SummaryCellType.TwoColumn,
                                    DarkLine=Visibility.Visible, ThinLine=Visibility.Collapsed});
            return adjustmentSet;
        }

        public override bool ShowApproveButton
        {
            get { return false; }
        }
        public override bool ShowAdjustButton
        {
            get { return false; }
        }
        public override bool ShowRejectButton
        {
            get { return false; }
        }
        public override bool ShowPrivacyPolicyButton
        {
            get { return true && (ApplicationView.Value != ApplicationViewState.Snapped); }
        }
        public override bool ShowExpandButton
        {
            get { return (ApplicationView.Value == ApplicationViewState.Snapped) || false; }
        }
        public override bool ShowDetailsButton
        {
            get { return false; }
        }

        public void SetAppBar()
        {
            Messenger.Default.Send<IViewModelCore>(this, Constants.RefreshAppBar);
        }
    }
}
