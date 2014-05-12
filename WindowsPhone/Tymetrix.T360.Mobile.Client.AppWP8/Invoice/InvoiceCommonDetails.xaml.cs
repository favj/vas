/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.Invoice;

namespace Tymetrix.T360.Mobile.Client.AppWP8.Invoice
{
    public partial class InvoiceCommonDetails : BasePage
    {
        private readonly Thickness ListWithKeyboardOpen = new Thickness(0, 0, 0, 350);
        private readonly Thickness ListWithKeyboardClose = new Thickness(0, 0, 0, 0);

        private readonly GridLength PlainViewColumn1Width = new GridLength(0, GridUnitType.Star);
        private readonly GridLength PlainViewColumn2Width = new GridLength(1, GridUnitType.Star);
        private readonly GridLength MultiViewColumn1WidthLandscape = new GridLength(1, GridUnitType.Star);
        private readonly GridLength MultiViewColumn2WidthLandscape = new GridLength(9, GridUnitType.Star);
        private readonly GridLength MultiViewColumn1WidthPortrait = new GridLength(1.25, GridUnitType.Star);
        private readonly GridLength MultiViewColumn2WidthPortrait = new GridLength(8.75, GridUnitType.Star);

        private LineItem selectedItemOnSearch;

        public InvoiceCommonDetails()
        {
            InitializeComponent();
            this.Loaded += InvoiceCommonDetails_Loaded;
            this.Unloaded += InvoiceCommonDetails_Unloaded;
        }

        void InvoiceCommonDetails_Unloaded(object sender, RoutedEventArgs e)
        {
            PageInProgress = false;
        }

        void InvoiceCommonDetails_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Source == Source.EXTERNAL)
            {
                this.Source = this.PreviousSource;
                return;
            }
            if (!HasPreparedAppBar)
            {
                PrepareApplicationBar();
                HasPreparedAppBar = true;
            }
            if (this.Source == Source.INVOICE_LIST)
            {
                PageInProgress = false;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if ((bool)PhoneApplicationService.Current.State.ContainsKey("isTombStoned"))
            {
                base.OnNavigatedTo(e);
                return;
            }
            if (this.Source == Source.EXTERNAL)
            {
                base.OnNavigatedTo(e);
                return;
            }
            if (this.Source == Source.INVOICE_LIST)
            {
                OnLoad = true;
                SummaryViewModel = new InvoiceSummaryViewModel();
                invoiceHeader.SetHeaderDetails(InvoiceSummaryDetails);
                PrepareSummaryViewModel();
                PrepareLineItemViewModel();
                this.DataContext = SummaryViewModel;
            }
            if (this.Source == Source.BACK_TO_LINE_ITEM_LIST)
            {
                InvoiceSummaryDetails = MultiConfirmation.InvoiceDetails;
                ShowLineItemList();
                if (IsInSearch)
                {
                    IsInSearch = false;
                    ManageSearch();
                }
                if (IsInLineItemMultiSelect)
                {
                    IsInLineItemMultiSelect = false;
                    ToggleLineItemMultiView();
                    ShowLineItemsAppBar();
                }
            }
            if (this.Source == Source.BACK_TO_INVOICE_SUMMARY)
            {
                IsInAction = false;
                AddPivotItem(PivotItem, 1);
                ShowSummary();
            }
            if (this.Source == Source.BACK_TO_INVOICE_SUMMARY_FROM_NOTES)
            {
                ShowSummary();
            }
            base.OnNavigatedTo(e);
        }

        private void RegisterEvents()
        {
            Messenger.Default.Register<LineItem>(this, Constants.LineItemMultiCheckChange, ManageCheckCount);
        }

        private void UnregisterEvents()
        {
            Messenger.Default.Unregister<LineItem>(this, Constants.LineItemMultiCheckChange, ManageCheckCount);
        }

        private void ManageCheckCount(LineItem param)
        {
            if (SummaryViewModel.LineItemList == null) return;

            int count = SummaryViewModel.LineItemList.Where(x => x.IsCheckboxChecked).ToList().Count;
            ((PivotItem)pivotControl.Items[0]).Header = "line items (" + count + ")";

            if (InvoiceSummaryDetails.Permissions.LineItemsMultipleAdjust) ManageAppBarButtonEnable(Constants.AdjustIconPath, count > 0);
            if (InvoiceSummaryDetails.Permissions.LineItemsMultipleReject) ManageAppBarButtonEnable(Constants.RejectIconPath, count > 0);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (Constants.ExternalURI.Equals(e.Uri.ToString()))
            {
                this.PreviousSource = this.Source;
                this.Source = Source.EXTERNAL;
                return;
            }
            RejectPage rejectPage = e.Content as RejectPage;
            if (rejectPage != null)
            {
                rejectPage.Source = IsInLineItem ? Source.LINE_ITEM_MULTI_REJECT : Source.INVOICE_SINGLE_REJECT;
                rejectPage.Reasons = RejectInput;
                rejectPage.InvoiceDetails = InvoiceSummaryDetails;
            }
            AdjustPage adjustPage = e.Content as AdjustPage;
            if (adjustPage != null)
            {
                adjustPage.Source = IsInLineItem ? Source.LINE_ITEM_MULTI_ADJUST : Source.INVOICE_SINGLE_ADJUST;
                adjustPage.Reasons = AdjustInput;
                adjustPage.InvoiceDetails = InvoiceSummaryDetails;
            }
            ConfirmationPage confirmationPage = e.Content as ConfirmationPage;
            if (confirmationPage != null)
            {
                confirmationPage.Source = IsInLineItemMultiAdjust ? Source.LINE_ITEM_MULTI_ADJUST : Source.LINE_ITEM_MULTI_REJECT;
                confirmationPage.LineItemInputDetails = MultiConfirmation;
            }
            LineItemSummary lineItemSummaryPage = e.Content as LineItemSummary;
            if (lineItemSummaryPage != null)
            {
                lineItemSummaryPage.Source = Source.LINE_ITEM_LIST;
                lineItemSummaryPage.LineItemSummaryDetails = LineItemSummaryInput;
                lineItemSummaryPage.HeaderDetails = InvoiceSummaryDetails;
                lineItemSummaryPage.LineItemList = SummaryViewModel.LineItemList;
                lineItemSummaryPage.CurrentLineItem = IsInSearch ? selectedItemOnSearch : (LineItem)listLineItem.SelectedItem;
            }
            NotesPage notesPage = e.Content as NotesPage;
            if (notesPage != null)
            {
                notesPage.Input = InvoiceSummaryDetails.NotesList;
                notesPage.IsInvoice = true;
            }
            ViewNotePage viewNote = e.Content as ViewNotePage;
            if (viewNote != null)
            {
                viewNote.Note = ViewNoteInput;
                viewNote.IsInvoice = true;
            }
            ManageAppBarVisible(false);
            base.OnNavigatedFrom(e);
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            base.OnOrientationChanged(e);
            if (IsInLineItem)
            {
                bool portrait = (e.Orientation == PageOrientation.Portrait ||
                                 e.Orientation == PageOrientation.PortraitDown ||
                                 e.Orientation == PageOrientation.PortraitUp);
                SetViewValues(portrait);
            }

            if (IsInSearch)
            {
                SetListMargin();
            }
        }

        private void SetViewValues(bool portrait)
        {
            foreach (LineItem lineItem in SummaryViewModel.LineItemList)
            {
                lineItem.NetTotalToDisplay = portrait ? DisplayString(lineItem.NetTotal, 10) : DisplayString(lineItem.NetTotal, 15);
                lineItem.FlagVisible = !"0".Equals(lineItem.Flags) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void PrepareLineItemViewModel()
        {
            SummaryViewModel.OriginalLineItemList = LineItemList;
            SummaryViewModel.LineItemList = new List<LineItem>(LineItemList);
        }

        private void PrepareSummaryViewModel()
        {
            List<SummaryView> summaryViews = new List<SummaryView>();

            //Invoice Summary Section
            summaryViews.Add(GetHeaderView(Constants.InvoiceSummaryHeader));
            summaryViews.Add(GetDetailView(Constants.InvoiceDateLabel, InvoiceSummaryDetails.InvoiceDate));
            summaryViews.Add(GetDetailView(Constants.BilledPeriodLabel, InvoiceSummaryDetails.BillingPeriod));
            summaryViews.Add(GetDetailView(Constants.TotalBilledAmountLabel, InvoiceSummaryDetails.TotalBilledAmount));
            summaryViews.Add(GetDetailView(Constants.NetFeesLabel, InvoiceSummaryDetails.BilledFees));
            summaryViews.Add(GetDetailView(Constants.NetExpensesLabel, InvoiceSummaryDetails.BilledExpenses));
            summaryViews.Add(GetDetailView(Constants.NetAmountLabel, InvoiceSummaryDetails.NetAmount));
            summaryViews.Add(GetDetailView(Constants.FlagsLabel, InvoiceSummaryDetails.Flags.Count.ToString()));
            summaryViews.Add(GetDetailView(Constants.StatusLabel, InvoiceSummaryDetails.Status));

            //Flags Section
            summaryViews.Add(GetHeaderView(Constants.FlagSectionHeader));
            if (InvoiceSummaryDetails.Flags.Count == 0)
            {
                summaryViews.Add(GetNoneView());
            }
            else
            {
                foreach (FlagDetails flag in InvoiceSummaryDetails.Flags)
                {
                    string imagePath = Constants.FlagInvoiceHighPriority.Equals(flag.Priority)
                        ? Constants.HighPriorityFlagIconPath
                        : Constants.FlagInvoiceMediumPriority.Equals(flag.Priority) ? Constants.MediumPriorityFlagIconPath
                                                                                    : Constants.LowPriorityFlagIconPath;
                    summaryViews.Add(GetFlagView(flag.DisplayName, imagePath));
                }
            }

            //Review Route Section
            summaryViews.Add(GetHeaderView(Constants.ReviewRouteHeader));
            if (InvoiceSummaryDetails.ReviewRouteList.Count == 0)
            {
                summaryViews.Add(GetNoneView());
            }
            else
            {
                foreach (ReviewRouteDetails rr in InvoiceSummaryDetails.ReviewRouteList)
                {
                    string imagePath = Constants.ReviewStatusReviewed.Equals(rr.ReviewStatus)
                        ? Constants.CheckedIconPath
                        : Constants.ReviewStatusYetToReview.Equals(rr.ReviewStatus) ? Constants.UncheckedIconPath
                                                                                    : string.Empty;
                    summaryViews.Add(GetReviewRouteView(rr.ReviewerName, imagePath, Constants.CurrentReviewerLabel));
                }
            }

            //Firm/Vendor Billing Section
            summaryViews.Add(GetHeaderView(Constants.FirmVendorBillingHeader));
            summaryViews.Add(GetDetailView(Constants.CurrencyTypeLabel, InvoiceSummaryDetails.CurrencyType));
            summaryViews.Add(GetDetailView(Constants.GrossAmountLabel, InvoiceSummaryDetails.GrossAmount));
            summaryViews.Add(GetDetailView(Constants.VendorAdjustmentLabel, InvoiceSummaryDetails.VendorAdjustment));
            summaryViews.Add(GetDetailView(Constants.BilledAmountLabel, InvoiceSummaryDetails.BilledAmount));

            //In-House Review Section
            summaryViews.Add(GetHeaderView(Constants.InHouseReviewHeader));
            summaryViews.Add(GetDetailView(Constants.ReviewerAdjustmentsLabel, InvoiceSummaryDetails.ReviewerAdjustment));
            summaryViews.Add(GetDetailView(Constants.ITPAdjustmentsLabel, InvoiceSummaryDetails.ItpAdjustment));
            summaryViews.Add(GetDetailView(Constants.SubtotalLabel, InvoiceSummaryDetails.SubTotal));
            summaryViews.Add(GetDetailView(Constants.TaxLabel, InvoiceSummaryDetails.Tax));
            summaryViews.Add(GetDetailView(Constants.NetTotalLabel, InvoiceSummaryDetails.NetTotal));

            //Discounts Section
            summaryViews.Add(GetHeaderView(Constants.DiscountsHeader));
            summaryViews.Add(GetDetailView(Constants.ProposedCreditLabel, InvoiceSummaryDetails.Credit));
            summaryViews.Add(GetDetailView(Constants.TotalWithCreditLabel, InvoiceSummaryDetails.TotalWithCredit));

            //Key-Fields Section
            summaryViews.Add(GetHeaderView(Constants.KeyFieldsHeader));
            if (InvoiceSummaryDetails.CommonProperties.Count == 0)
            {
                summaryViews.Add(GetNoneView());
            }
            else
            {
                foreach (KeyFields keyField in InvoiceSummaryDetails.CommonProperties)
                {
                    summaryViews.Add(GetDetailView(keyField.LabelText, keyField.ValueText));
                }
            }

            //Detail-Fields Section
            if (InvoiceSummaryDetails.Permissions.Properties)
            {
                summaryViews.Add(GetHeaderView(Constants.DetailFieldsHeader));
                if (InvoiceSummaryDetails.Properties.Count == 0)
                {
                    summaryViews.Add(GetNoneView());
                }
                else
                {
                    foreach (DetailField detailField in InvoiceSummaryDetails.Properties)
                    {
                        summaryViews.Add(GetDetailView(detailField.LabelText, detailField.ValueText));
                    }
                }
            }

            //Notes Section
            if (InvoiceSummaryDetails.Permissions.Notes)
            {
                summaryViews.Add(GetHeaderView(Constants.NotesHeader));
                if (InvoiceSummaryDetails.NotesList.Count == 0)
                {
                    summaryViews.Add(GetNoneView());
                }
                else
                {
                    NotesData note;
                    for (int i = 0; i < InvoiceSummaryDetails.NotesList.Count; i ++)
                    {
                        if (i > 1) break;
                        note = InvoiceSummaryDetails.NotesList[i];
                        summaryViews.Add(GetNoteView(note, false));
                    }
                    if (InvoiceSummaryDetails.NotesList.Count > 2)
                    {
                        summaryViews.Add(GetNoteView(null, true));
                    }
                }
            }

            //Taxes Section
            if (InvoiceSummaryDetails.TaxList.Count > 0)
            {
                List<ValueList> valueList;
                foreach (string key in InvoiceSummaryDetails.TaxList.Keys)
                {
                    valueList = InvoiceSummaryDetails.TaxList[key];
                    summaryViews.Add(GetHeaderView(key + " Taxes"));
                    foreach (ValueList value in valueList)
                    {
                        summaryViews.Add(GetDetailView(Constants.TaxJurisdictionCodeLabel, value.TaxJurisdictionCode));
                        summaryViews.Add(GetDetailView(Constants.TaxTypeCodeLabel, value.TaxTypeCode));
                        summaryViews.Add(GetDetailView(Constants.TaxRateLabel, value.TaxRate));
                        summaryViews.Add(GetDetailView(Constants.TaxableAmountLabel, value.TaxableAmount));
                        summaryViews.Add(GetDetailView(Constants.TaxableAmountLabel, value.TaxAmount));
                    }
                }
            }
            SummaryViewModel.DetailsList = summaryViews;
        }

        private SummaryView GetNoneView()
        {
            SummaryView view = new SummaryView();
            view.IsNoneView = Visibility.Visible;
            return view;
        }

        private SummaryView GetHeaderView(string title)
        {
            SummaryView view = new SummaryView();
            view.IsHeaderView = Visibility.Visible;
            view.HeaderText = title;
            return view;
        }

        private SummaryView GetDetailView(string label, string value)
        {
            SummaryView view = new SummaryView();
            view.IsDetailView = Visibility.Visible;
            view.DetailHeader = label;
            view.DetailValue = (value.Length > 540) ? value.Substring(0, 540) + "..." : value;
            return view;
        }

        private SummaryView GetFlagView(string label, string imagePath)
        {
            SummaryView view = new SummaryView();
            view.IsFlagView = Visibility.Visible;
            view.FlagName = label;
            view.ImagePath = new Uri(imagePath, UriKind.Relative);
            return view;
        }

        private SummaryView GetReviewRouteView(string label, string imagePath, string currentReviewer)
        {
            SummaryView view = new SummaryView();
            view.IsReviewRouteView = Visibility.Visible;
            view.ReviewerName = label;
            if (!string.Empty.Equals(imagePath))
            {
                view.ReviewRouteImageWidth = 30;
                view.ImageTitle = new Uri(imagePath, UriKind.Relative);
                view.ReviewRouteImageMargin = new Thickness(10, 0, 10, 0);
            }
            else
            {
                view.CurrentReviewerLabel = currentReviewer;
                view.CurrentReviewerVisible = Visibility.Visible;
            }
            return view;
        }

        private SummaryView GetNoteView(NotesData note, bool isButtonRow)
        {
            SummaryView view = new SummaryView();
            view.IsNotesView = Visibility.Visible;
            if (isButtonRow)
            {
                view.IsButtonRow = Visibility.Visible;
                view.ShowAllNotesCommand = new Command(OnNotesClick);
            }
            else
            {
                view.IsNotesRow = Visibility.Visible;
                view.Creator = note.Creator;
                view.CreatedTime = note.CreatedTime;
                view.Description = note.Description;
                view.Tag = note;
            }
            return view;
        }

        private void OnNotesClick(object parameter)
        {
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }
            this.ProgressBar.Show();
            PageInProgress = true;
            UserData.Instance.DetailsToNotes = true;
            Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Invoice/NotesPage.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
            this.ProgressBar.Hide();
            PageInProgress = false;
        }

        private void summaryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (summaryList.SelectedIndex < 0) return;

            SummaryView view = (SummaryView)e.AddedItems[0];
            if (view.IsNotesView == Visibility.Visible && view.IsNotesRow == Visibility.Visible)
            {
                if (!ServiceInvoker.IsConnected)
                {
                    ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                    return;
                }

                UserData.Instance.DetailsToViewNote = true;

                ViewNoteInput = (NotesData)(view.Tag);

                this.ProgressBar.Show();
                PageInProgress = true;

                Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Invoice/ViewNotePage.xaml", UriKind.Relative);
                NavigationService.Navigate(uri);
                this.ProgressBar.Hide();
                PageInProgress = false;
            }
            summaryList.SelectedIndex = -1;
        }

        private int GetIndex(string invoiceId)
        {
            return AwaitingInvoices.IndexOf(AwaitingInvoices.First(x => x.InvoiceId.ToString().Equals(invoiceId)));
        }

        private bool IsFirstItem()
        {
            return (GetIndex(InvoiceSummaryDetails.InvoiceId.ToString()) == 0);
        }

        private bool IsLastItem()
        {
            return (GetIndex(InvoiceSummaryDetails.InvoiceId.ToString()) == (AwaitingInvoices.Count - 1));
        }

        private bool HasPermission()
        {
            return (InvoiceSummaryDetails.Permissions.Approve || InvoiceSummaryDetails.Permissions.Reject ||
                    InvoiceSummaryDetails.Permissions.AdjustFee || InvoiceSummaryDetails.Permissions.AdjustExpense);
        }

        private void pivotControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OnLoad)
            {
                OnLoad = false;
                return;
            }
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                pivotControl.SelectionChanged -= pivotControl_SelectionChanged;
                pivotControl.SelectedIndex = (pivotControl.SelectedIndex == 0) ? 1 : 0;
                pivotControl.SelectionChanged += pivotControl_SelectionChanged;
                return;
            }
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    IsInLineItem = false;
                    ShowSummary();
                    break;
                case 1:
                    IsInLineItem = true;
                    ShowLineItemList();
                    break;
            }
        }

        private void ShowSummary()
        {
            PageInProgress = true;
            this.ProgressBar.Show();
            PopulateSummary();
            this.lineItemsSearch.SearchPage = null;
        }

        private void ShowLineItemList()
        {
            PageInProgress = true;
            this.ProgressBar.Show();
            PopulateLineItemList();
            this.lineItemsSearch.SearchPage = this;
        }

        private void PopulateSummary()
        {
            RetrieveInvoiceSummary(GetIndex(InvoiceSummaryDetails.InvoiceId.ToString()));
        }

        private void PopulateLineItemList()
        {
            RetrieveLineItemDetails(GetIndex(InvoiceSummaryDetails.InvoiceId.ToString()));
        }

        private bool ShowSummaryAppBar()
        {
            if (ContainsButton(Constants.LeftArrowIconPath)) return true;

            RemoveApplicationBarButtons();
            AddButtonInAppBar(Constants.ActionItemIconPath, Constants.ActionItemTitle, HasPermission());
            AddButtonInAppBar(Constants.LeftArrowIconPath, Constants.LeftArrowTitle, !IsFirstItem());
            AddButtonInAppBar(Constants.RightArrowIconPath, Constants.RightArrowTitle, !IsLastItem());

            return false;
        }

        private bool ShowLineItemsAppBar()
        {
            if (ContainsButton(Constants.SearchIconPath)) return true;

            RemoveApplicationBarButtons();
            if (InvoiceSummaryDetails.Permissions.LineItemsMultipleAdjust || InvoiceSummaryDetails.Permissions.LineItemsMultipleReject)
            {
                AddButtonInAppBar(Constants.MultiSelectIconPath, Constants.MultiSelectTitle, true);
            }
            AddButtonInAppBar(Constants.SearchIconPath, Constants.SearchTitle, true);

            return false;
        }

        protected override void PrepareAppBarMap()
        {
            AppBarModelList = new List<ApplicationBarModel>();

            ApplicationBarModel appBarModel;
            if (InvoiceSummaryDetails.Permissions.Approve || InvoiceSummaryDetails.Permissions.Reject ||
                InvoiceSummaryDetails.Permissions.AdjustFee || InvoiceSummaryDetails.Permissions.AdjustExpense)
            {
                appBarModel = new ApplicationBarModel();
                appBarModel.IconPath = Constants.ActionItemIconPath;
                appBarModel.IsEnabled = true;
                appBarModel.ButtonText = Constants.ActionItemTitle;
                AppBarModelList.Add(appBarModel);
            }

            appBarModel = new ApplicationBarModel();
            appBarModel.IconPath = Constants.LeftArrowIconPath;
            appBarModel.IsEnabled = !IsFirstItem();
            appBarModel.ButtonText = Constants.LeftArrowTitle;
            AppBarModelList.Add(appBarModel);

            appBarModel = new ApplicationBarModel();
            appBarModel.IconPath = Constants.RightArrowIconPath;
            appBarModel.IsEnabled = !IsLastItem();
            appBarModel.ButtonText = Constants.RightArrowTitle;
            AppBarModelList.Add(appBarModel);
        }

        protected override void OnAppBarButtonClick(ApplicationBarModel appBarModel)
        {
            if (PageInProgress) return;

            switch (appBarModel.IconPath)
            {
                case Constants.ActionItemIconPath:
                    OnActionItem();
                    break;
                case Constants.LeftArrowIconPath:
                    OnLeftArrow();
                    break;
                case Constants.RightArrowIconPath:
                    OnRightArrow();
                    break;
                case Constants.ApproveIconPath:
                    OnApprove();
                    break;
                case Constants.RejectIconPath:
                    if (!IsInLineItem)
                    {
                        OnSummaryReject();
                    }
                    else
                    {
                        OnLineItemReject();
                    }
                    break;
                case Constants.AdjustIconPath:
                    if (!IsInLineItem)
                    {
                        OnSummaryAdjust();
                    }
                    else
                    {
                        OnLineItemAdjust();
                    }
                    break;
                case Constants.MultiSelectIconPath:
                    OnMultiSelect();
                    break;
                case Constants.SearchIconPath:
                    OnSearch();
                    break;
                default: break;
            }
        }

        private void OnActionItem()
        {
            IsInAction = true;
            ManageActionItem();
        }

        private void ManageActionItem()
        {
            if (IsInAction)
            {
                RemovePivotItem(1);
                RemoveAppBarButton(Constants.ActionItemIconPath);
                RemoveAppBarButton(Constants.LeftArrowIconPath);
                RemoveAppBarButton(Constants.RightArrowIconPath);
                if (InvoiceSummaryDetails.Permissions.Approve) AddButtonInAppBar(Constants.ApproveIconPath, Constants.ApproveTitle, true);
                if (InvoiceSummaryDetails.Permissions.AdjustFee || InvoiceSummaryDetails.Permissions.AdjustExpense) AddButtonInAppBar(Constants.AdjustIconPath, Constants.AdjustTitle, true);
                if (InvoiceSummaryDetails.Permissions.Reject) AddButtonInAppBar(Constants.RejectIconPath, Constants.RejectTitle, true);
            }
            else
            {
                AddPivotItem(PivotItem, 1);
                RemoveAppBarButton(Constants.ApproveIconPath);
                RemoveAppBarButton(Constants.RejectIconPath);
                RemoveAppBarButton(Constants.AdjustIconPath);
                AddButtonInAppBar(Constants.ActionItemIconPath, Constants.ActionItemTitle, HasPermission());
                AddButtonInAppBar(Constants.LeftArrowIconPath, Constants.LeftArrowTitle, !IsFirstItem());
                AddButtonInAppBar(Constants.RightArrowIconPath, Constants.RightArrowTitle, !IsLastItem());
            }
        }

        private void AddButtonInAppBar(string path, string title, bool enable)
        {
            ApplicationBarModel appBarModel;
            List<ApplicationBarModel> list = AppBarModelList.Where(x => x.IconPath == (path)).ToList();
            if (list.Count == 0)
            {
                appBarModel = CreateAppBarIcon(path, title);
                appBarModel.IsEnabled = enable;
            }
            else
            {
                list[0].IsEnabled = enable;
                appBarModel = list[0];
            }
            AddAppBarButton(appBarModel);
        }

        private ApplicationBarModel CreateAppBarIcon(string path, string title)
        {
            ApplicationBarModel appBarModel = new ApplicationBarModel();
            appBarModel.IconPath = path;
            appBarModel.IsEnabled = true;
            appBarModel.ButtonText = title;

            return appBarModel;
        }

        private void AddPivotItem(PivotItem item, int position)
        {
            if (item == null) return;
            pivotControl.Items.Insert(position, item);
        }

        private void RemovePivotItem(int position)
        {
            PivotItem = pivotControl.Items[position] as PivotItem;
            pivotControl.Items.RemoveAt(position);
        }

        private void OnLeftArrow()
        {
            int index = GetIndex(InvoiceSummaryDetails.InvoiceId.ToString()) - 1;
            ManagePreviousNext(index, Constants.LeftArrowIconPath, (index > 0));
        }

        private void OnRightArrow()
        {
            int index = GetIndex(InvoiceSummaryDetails.InvoiceId.ToString()) + 1;
            ManagePreviousNext(index, Constants.RightArrowIconPath, (index < (AwaitingInvoices.Count - 1)));
        }

        private void ManagePreviousNext(int index, string key, bool enable)
        {
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }
            ManageAppBarButtonEnable(key, enable);
            RetrieveInvoiceSummary(index);
        }

        private void RetrieveLineItemDetails(int index)
        {
            string invoiceId = AwaitingInvoices[index].InvoiceId.ToString();
            try
            {
                ServiceInvoker.InvokeServiceUsingGet("/api/t360/networks/invoices/" + invoiceId + "/lineitem", GetLineItemList, false);
            }
            catch (Exception ex)
            {
                if (ex is AppException)
                {
                    ShowError((AppException)ex);
                }
                else
                {
                    ShowError(new AppException(T360ErrorCodes.UNKNOWN));
                }
            }
        }

        private void GetLineItemList(object sender, ServiceEventArgs args)
        {
            ServiceResponse result = args.Result;
            if (result.Status)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    LineItemList = JsonConvert.DeserializeObject<List<LineItem>>(result.Output);
                    PrepareLineItemViewModel();
                    SetViewValues(CurrentOrientation == PageOrientation.Portrait ||
                                  CurrentOrientation == PageOrientation.PortraitDown ||
                                  CurrentOrientation == PageOrientation.PortraitUp);
                    this.ProgressBar.Hide();
                    PageInProgress = false;
                    if (HasPreparedAppBar)
                    {
                        bool alreadyAdded = ShowLineItemsAppBar();
                        if (alreadyAdded)
                        {
                            if (InvoiceSummaryDetails.Permissions.LineItemsMultipleAdjust || InvoiceSummaryDetails.Permissions.LineItemsMultipleReject)
                            {
                                ManageAppBarButtonEnable(Constants.MultiSelectIconPath, true);
                            }
                            ManageAppBarButtonEnable(Constants.SearchIconPath, true);
                        }
                        ManageAppBarVisible(true);
                    }
                });
            }
            else
            {
                List<Error> resultError = result.ErrorDetails;
                ShowError(new AppException(resultError), Constants.LineItemListError);
                if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        RedirectToInvoiceList();
                    });
                }
            }
        }

        private void RetrieveInvoiceSummary(int index)
        {
            try
            {
                string invoiceId = AwaitingInvoices[index].InvoiceId.ToString();
                PageInProgress = true;
                this.ProgressBar.Show();
                DisableApplicationBar();
                ServiceInvoker.InvokeServiceUsingGet("/api/t360/networks/invoice/" + invoiceId, GetInvoiceSummary, false);
            }
            catch (Exception ex)
            {
                if (ex is AppException)
                {
                    ShowError((AppException)ex);
                }
                else
                {
                    ShowError(new AppException(T360ErrorCodes.UNKNOWN));
                }
            }
        }

        private void GetInvoiceSummary(object sender, ServiceEventArgs se)
        {
            ServiceResponse result = se.Result;
            if (result.Status)
            {
                InvoiceSummaryDetails = JsonConvert.DeserializeObject<InvoiceSummary>(result.Output);

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    invoiceHeader.SetHeaderDetails(InvoiceSummaryDetails);
                    PrepareSummaryViewModel();
                    if (Source != Source.BACK_TO_INVOICE_SUMMARY_FROM_NOTES)
                    {
                        summaryList.UpdateLayout();
                        summaryList.ScrollIntoView(summaryList.Items[0]);
                    }
                    this.ProgressBar.Hide();
                    PageInProgress = false;
                    if (HasPreparedAppBar)
                    {
                        bool alreadyAdded = ShowSummaryAppBar();
                        if (alreadyAdded)
                        {
                            ManageAppBarButtonEnable(Constants.ActionItemIconPath, true);
                            ManageAppBarButtonEnable(Constants.LeftArrowIconPath, !IsFirstItem());
                            ManageAppBarButtonEnable(Constants.RightArrowIconPath, !IsLastItem());
                        }
                        ManageAppBarVisible(true);
                    }
                });
            }
            else
            {
                List<Error> resultError = result.ErrorDetails;
                ShowError(new AppException(resultError), Constants.InvoiceSummaryError);

                if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        RedirectToInvoiceList();
                    });
                }
            }
        }

        private void OnApprove()
        {
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }

            MessageBoxResult msgResult1 = MessageBox.Show(Constants.SingleApproveInvoiceMsg, Constants.ApprovalConfirmationTitle, MessageBoxButton.OKCancel);
            if (msgResult1 == MessageBoxResult.Cancel) return;

            try
            {
                ApprovalInputDetails invoiceToApprove = new ApprovalInputDetails() { InvoiceId = InvoiceSummaryDetails.InvoiceId.ToString(), ForceApprove = false };
                string postData = JsonConvert.SerializeObject(invoiceToApprove);
                PageInProgress = true;
                this.ProgressBar.Show();
                ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/ApproveInvoice", postData, false, ApproveInvoice);
            }
            catch (Exception ex)
            {
                if (ex is AppException)
                {
                    ShowError((AppException)ex);
                }
                else
                {
                    ShowError(new AppException(T360ErrorCodes.UNKNOWN));
                }
            }
        }

        private void ApproveInvoice(object sender, ServiceEventArgs se)
        {
            try
            {
                ServiceResponse result = se.Result;
                if (result.Status)
                {
                    InvoiceApprovalDetails approveResponse = JsonConvert.DeserializeObject<InvoiceApprovalDetails>(result.Output);
                    if ((approveResponse.Status != InvoiceApprovalStatus.Approved.ToString(Constants.InvoiceApprovalStatus)) &&
                        (approveResponse.Status != InvoiceApprovalStatus.Warning.ToString(Constants.InvoiceApprovalStatus)))
                    {
                        ShowError(new AppException(result.ErrorDetails), Constants.ApproveError);
                    }
                    else if (approveResponse.Status == InvoiceApprovalStatus.Warning.ToString(Constants.InvoiceApprovalStatus))
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            MessageBoxResult msgResult = MessageBox.Show(Constants.NegativeInvoiceBalanceMsg, Constants.NegativeInvoiceBalanceTitle, MessageBoxButton.OKCancel);
                            if (msgResult == MessageBoxResult.Cancel) return;
                        });
                        ApprovalInputDetails invoiceToApprove = new ApprovalInputDetails() { InvoiceId = InvoiceSummaryDetails.InvoiceId.ToString(), ForceApprove = false };
                        string postData = JsonConvert.SerializeObject(invoiceToApprove);

                        ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/ApproveInvoice", postData, false, ApproveInvoiceWithWarning);
                    }
                    else
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            this.ProgressBar.Hide();
                            PageInProgress = false;
                            NavigationService.GoBack();
                        });
                    }
                }
                else
                {
                    List<Error> resultError = result.ErrorDetails;
                    ShowError(new AppException(resultError), Constants.ApproveError);
                    if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            this.ProgressBar.Hide();
                            PageInProgress = false;
                            RedirectToInvoiceList();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is AppException)
                {
                    ShowError((AppException)ex);
                }
                else
                {
                    ShowError(new AppException(T360ErrorCodes.UNKNOWN));
                }
            }
        }

        private void ApproveInvoiceWithWarning(object sender, ServiceEventArgs args)
        {
            ServiceResponse innerResult = args.Result;
            if (innerResult.Status)
            {
                InvoiceApprovalDetails finalApprove = JsonConvert.DeserializeObject<InvoiceApprovalDetails>(innerResult.Output);
                if (finalApprove.Status != InvoiceApprovalStatus.Approved.ToString(Constants.InvoiceApprovalStatus))
                {
                    ShowError(new AppException(innerResult.ErrorDetails), Constants.ApproveError);
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        this.ProgressBar.Hide();
                        PageInProgress = false;
                        NavigationService.GoBack();
                    });
                }
            }
            else
            {
                List<Error> resultError = innerResult.ErrorDetails;
                ShowError(new AppException(resultError), Constants.ApproveError);
                if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        this.ProgressBar.Hide();
                        PageInProgress = false;
                        RedirectToInvoiceList();
                    });
                }
            }
        }

        private void OnSummaryReject()
        {
            DisableApplicationBar();
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                EnableApplicationBar();
                return;
            }
            GotoRejectPage();
        }

        private void GotoRejectPage()
        {
            string postData = JsonConvert.SerializeObject(new ReasonCodeInputDetails() { Action = Constants.RejectAction });
            try
            {
                PageInProgress = true;
                this.ProgressBar.Show();
                ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/GetReasonCodes", postData, false, RejectInvoice);
            }
            catch (Exception ex)
            {
                if (ex is AppException)
                {
                    ShowError((AppException)ex);
                }
                else
                {
                    ShowError(new AppException(T360ErrorCodes.UNKNOWN));
                }
            }
        }

        private void RejectInvoice(object sender, ServiceEventArgs args)
        {
            ServiceResponse result = args.Result;
            if (result.Status)
            {
                RejectInput = JsonConvert.DeserializeObject<List<ReasonCode>>(result.Output);

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.ProgressBar.Show();
                    PageInProgress = true;
                    Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Invoice/RejectPage.xaml", UriKind.Relative);
                    this.NavigationService.Navigate(uri);
                    this.ProgressBar.Hide();
                });
            }
            else
            {
                List<Error> resultError = result.ErrorDetails;
                ShowError(new AppException(resultError), Constants.InvoiceSummaryError);
                if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        this.ProgressBar.Hide();
                        PageInProgress = false;
                        RedirectToInvoiceList();
                    });
                }
            }
        }

        private void OnLineItemReject()
        {
            DisableApplicationBar();
            CalculateLineItemNetAmount(false);
        }

        private void CalculateLineItemNetAmount(bool isAdjust)
        {
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                EnableApplicationBar();
                return;
            }
            IsInLineItemMultiAdjust = isAdjust;
            List<string> lineItemIds = new List<string>();
            List<LineItem> lineItems = SummaryViewModel.LineItemList.Where(x => x.IsCheckboxChecked).ToList();
            lineItems.ForEach(delegate(LineItem lt) { lineItemIds.Add(lt.LineItemId); });
            MultiConfirmation = new LineItemConfirmationDetails();
            MultiConfirmation.SelectedLineItemIds = lineItemIds;
            MultiConfirmation.InvoiceId = InvoiceSummaryDetails.InvoiceId.ToString();

            string postData = JsonConvert.SerializeObject(MultiConfirmation);
            try
            {
                PageInProgress = true;
                this.ProgressBar.Show();
                ServiceInvoker.InvokeServiceUsingPost("/api/t360/LineItem/CalculateLineItemNetAmount", postData, false, CalculateLineItemNetAmount);
            }
            catch (Exception ex)
            {
                if (ex is AppException)
                {
                    ShowError((AppException)ex);
                }
                else
                {
                    ShowError(new AppException(T360ErrorCodes.UNKNOWN));
                }
                EnableApplicationBar();
            }
        }

        private void CalculateLineItemNetAmount(object sender, ServiceEventArgs args)
        {
            ServiceResponse result = args.Result;
            if (result.Status)
            {
                MultipleLineItemsInputDetails rejectMultipleitem = JsonConvert.DeserializeObject<MultipleLineItemsInputDetails>(result.Output);
                
                MultiConfirmation.SelectedLineItems = SummaryViewModel.LineItemList.Where(x => x.IsCheckboxChecked).ToList();
                MultiConfirmation.NetAmount = rejectMultipleitem.NetAmount;
                MultiConfirmation.PositiveAdjustment = rejectMultipleitem.PositiveAdjustment;
                MultiConfirmation.CurrencySymbol = rejectMultipleitem.CurrencySymbol;
                MultiConfirmation.InvoiceDetails = InvoiceSummaryDetails;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.ProgressBar.Show();
                    PageInProgress = true;
                    Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Invoice/ConfirmationPage.xaml", UriKind.Relative);
                    this.NavigationService.Navigate(uri);
                    this.ProgressBar.Hide();
                });
            }
            else
            {
                List<Error> resultError = result.ErrorDetails;
                ShowError(new AppException(resultError), Constants.InvoiceSummaryError);
                if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        this.ProgressBar.Hide();
                        PageInProgress = false;
                        RedirectToInvoiceList();
                    });
                }
                EnableApplicationBar();
            }
        }

        private void OnSummaryAdjust()
        {
            DisableApplicationBar();
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                EnableApplicationBar();
                return;
            }

            if (InvoiceSummaryDetails.Permissions != null)
            {
                if (!InvoiceSummaryDetails.Permissions.AdjustInvoiceAllowed)
                {
                    MessageBoxResult msgResult = MessageBox.Show(Constants.TaxedInvoiceMsg, Constants.TaxedInvoiceTitle, MessageBoxButton.OK);
                    if (msgResult == MessageBoxResult.OK || msgResult == MessageBoxResult.None)
                    {
                        EnableApplicationBar();
                        return;
                    }
                }

                GotoAdjustPage();
            }
        }

        private void GotoAdjustPage()
        {
            string postData = JsonConvert.SerializeObject(new ReasonCodeInputDetails() { Action = Constants.AdjustFeesAction });
            try
            {
                PageInProgress = true;
                this.ProgressBar.Show();
                ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/GetReasonCodes", postData, false, AdjustInvoice);
            }
            catch (Exception ex)
            {
                if (ex is AppException)
                {
                    ShowError((AppException)ex);
                }
                else
                {
                    ShowError(new AppException(T360ErrorCodes.UNKNOWN));
                }
            }
        }

        private void AdjustInvoice(object sender, ServiceEventArgs args)
        {
            ServiceResponse result = args.Result;
            if (result.Status)
            {
                AdjustInput = JsonConvert.DeserializeObject<List<ReasonCode>>(result.Output);

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.ProgressBar.Show();
                    PageInProgress = true;
                    Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Invoice/AdjustPage.xaml", UriKind.Relative);
                    this.NavigationService.Navigate(uri);
                    this.ProgressBar.Hide();
                });
            }
            else
            {
                List<Error> resultError = result.ErrorDetails;
                ShowError(new AppException(resultError), Constants.InvoiceSummaryError);
                if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        this.ProgressBar.Hide();
                        PageInProgress = false;
                        RedirectToInvoiceList();
                    });
                }
            }
        }

        private void OnLineItemAdjust()
        {
            DisableApplicationBar();
            if (!InvoiceSummaryDetails.Permissions.LineItemsMultipleAdjustAllowed)
            {
                MessageBoxResult msgResult = MessageBox.Show(Constants.TaxedMultiLineItemMsg, Constants.TaxedInvoiceTitle, MessageBoxButton.OK);
                if (msgResult == MessageBoxResult.OK || msgResult == MessageBoxResult.None)
                {
                    EnableApplicationBar();
                    return;
                }
                return;
            }
            CalculateLineItemNetAmount(true);
        }

        private void OnMultiSelect()
        {
            IsInLineItemMultiSelect = true;
            ToggleLineItemMultiView();
            ShowLineItemMultiViewAppBar();
        }

        private void ToggleLineItemMultiView()
        {
            bool portrait = (CurrentOrientation == PageOrientation.Portrait ||
                             CurrentOrientation == PageOrientation.PortraitDown ||
                             CurrentOrientation == PageOrientation.PortraitUp);
            foreach (LineItem lineItem in SummaryViewModel.LineItemList)
            {
                lineItem.IsVisibility = !IsInLineItemMultiSelect ? Visibility.Collapsed : Visibility.Visible;
            }
            SetViewValues(portrait);
            if (IsInLineItemMultiSelect)
            {
                ((PivotItem)pivotControl.Items[1]).Header = "line items (0)";
                RemovePivotItem(0);
                RegisterEvents();
            }
            else
            {
                pivotControl.SelectionChanged -= pivotControl_SelectionChanged;
                AddPivotItem(PivotItem, 0);
                pivotControl.SelectedIndex = 1;
                ((PivotItem)pivotControl.Items[1]).Header = Constants.LineItemPivotHeader;
                pivotControl.SelectionChanged += pivotControl_SelectionChanged;
                UnregisterEvents();
            }
        }

        private string DisplayString(string str, int length)
        {
            if (str.Length > length)
                return str.Substring(0, length) + "...";
            return str;
        }

        private void ShowLineItemMultiViewAppBar()
        {
            RemoveApplicationBarButtons();
            if (InvoiceSummaryDetails.Permissions.LineItemsMultipleAdjust) AddButtonInAppBar(Constants.AdjustIconPath, Constants.AdjustTitle, false);
            if (InvoiceSummaryDetails.Permissions.LineItemsMultipleReject) AddButtonInAppBar(Constants.RejectIconPath, Constants.RejectTitle, false);
        }

        private void OnSearch()
        {
            IsInSearch = true;
            ManageSearch();
        }

        private void ManageSearch()
        {
            TextBox txtBox = lineItemsSearch.GetSearchTextBox();
            if (IsInSearch)
            {
                txtBox.TextChanged += OnLineItemSearch_TextChanged;
                this.lineItemsSearch.SearchPage = this;
                lineItemsSearch.Focus();
            }
            else
            {
                txtBox.TextChanged -= OnLineItemSearch_TextChanged;
                txtBox.Text = string.Empty;
                this.lineItemsSearch.SearchPage = null;
                SummaryViewModel.NoDataVisible = Visibility.Collapsed;
            }
            SummaryViewModel.PivotVisible = IsInSearch ? Visibility.Collapsed : Visibility.Visible;
            SummaryViewModel.SearchListVisible = IsInSearch ? Visibility.Visible : Visibility.Collapsed;
            SummaryViewModel.HeaderVisible = IsInSearch ? Visibility.Collapsed : Visibility.Visible;
            SummaryViewModel.SearchTextVisible = IsInSearch ? Visibility.Visible : Visibility.Collapsed;
            SummaryViewModel.SearchList = IsInSearch ? SummaryViewModel.OriginalLineItemList : null;
            SummaryViewModel.PivotOpacity = IsInSearch ? 0.25 : 1;
            SummaryViewModel.PivotEnable = !IsInSearch;
            ManageAppBarVisible(!IsInSearch);
        }

        private void OnLineItemSearch_TextChanged(object sender, EventArgs e)
        {
            List<LineItem> searchInvoice;
            string searchText = lineItemsSearch.GetSearchTextBox().Text;
            if (searchText.Length >= 3)
            {
                searchInvoice = new List<LineItem>(from inv in this.SummaryViewModel.OriginalLineItemList
                                                   where (inv.TimeKeeper.ToLower()).Contains(searchText.ToLower())
                                                   select inv).ToList();

                bool hasInvoice = (searchInvoice.Count > 0);
                SummaryViewModel.SearchListVisible = hasInvoice ? Visibility.Visible : Visibility.Collapsed;
                SummaryViewModel.NoDataVisible = hasInvoice ? Visibility.Collapsed : Visibility.Visible;
                if (hasInvoice)
                {
                    SummaryViewModel.PivotOpacity = 1;
                    SummaryViewModel.PivotEnable = true;
                    SummaryViewModel.SearchList = searchInvoice;
                    searchLineItem.UpdateLayout();
                    searchLineItem.ScrollIntoView(searchLineItem.Items[0]);
                    SetListMargin();
                }
            }
            else
            {
                SummaryViewModel.PivotOpacity = 0.25;
                SummaryViewModel.PivotEnable = false;
                SummaryViewModel.SearchListVisible = Visibility.Visible;
                SummaryViewModel.NoDataVisible = Visibility.Collapsed;
                SummaryViewModel.SearchList = SummaryViewModel.OriginalLineItemList;
            }
        }

        public override void OnSearchGotFocus()
        {
            SetListMargin();
        }

        public override void OnSearchLostFocus()
        {
            SummaryViewModel.ListMargin = ListWithKeyboardClose;
        }

        private void SetListMargin()
        {
            if (this.CurrentOrientation == PageOrientation.LandscapeRight ||
               this.CurrentOrientation == PageOrientation.LandscapeLeft ||
               this.CurrentOrientation == PageOrientation.Landscape)
                SummaryViewModel.ListMargin = ListWithKeyboardClose;
            else
                SummaryViewModel.ListMargin = ListWithKeyboardOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SummaryViewModel.SelectedIndex < 0) return;

            try
            {
                LineItem lineItemModel = (LineItem)listLineItem.SelectedItem;

                if (IsInLineItemMultiSelect)
                {
                    lineItemModel.IsCheckboxChecked = !lineItemModel.IsCheckboxChecked;
                    SummaryViewModel.SelectedIndex = -1;
                }
                else
                {
                    PageInProgress = true;
                    this.ProgressBar.Show();
                    DisableApplicationBar();
                    ServiceInvoker.InvokeServiceUsingGet("/api/t360/networks/invoices/" + InvoiceSummaryDetails.InvoiceId + "/lineitem/" + lineItemModel.LineItemId,
                                                         GetLineItemDetails,
                                                         false);
                }
            }
            catch (Exception ex)
            {
                if (ex is AppException)
                {
                    ShowError((AppException)ex);
                }
                else
                {
                    ShowError(new AppException(T360ErrorCodes.UNKNOWN));
                }
            }
        }

        private void SearchListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (searchLineItem.SelectedIndex < 0) return;

            try
            {
                selectedItemOnSearch = (LineItem)(e.AddedItems[0]);
                PageInProgress = true;
                this.ProgressBar.Show();
                ServiceInvoker.InvokeServiceUsingGet("/api/t360/networks/invoices/" + InvoiceSummaryDetails.InvoiceId + "/lineitem/" + selectedItemOnSearch.LineItemId,
                                                        GetLineItemDetails,
                                                        false);
                searchLineItem.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                if (ex is AppException)
                {
                    ShowError((AppException)ex);
                }
                else
                {
                    ShowError(new AppException(T360ErrorCodes.UNKNOWN));
                }
            }
        }

        private void GetLineItemDetails(object sender, ServiceEventArgs args)
        {
            ServiceResponse result = args.Result;
            if (result.Status)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    LineItemSummaryInput = JsonConvert.DeserializeObject<InvoiceSummary>(result.Output);
                    Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Invoice/LineItemSummary.xaml", UriKind.Relative);
                    this.NavigationService.Navigate(uri);
                    this.ProgressBar.Hide();
                    PageInProgress = false;
                });
            }
            else
            {
                List<Error> resultError = result.ErrorDetails;
                ShowError(new AppException(resultError), Constants.LineItemListError);
                if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        this.ProgressBar.Hide();
                        PageInProgress = false;
                        RedirectToInvoiceList();
                    });
                }
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (PageInProgress)
            {
                e.Cancel = true;
                return;
            }
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                e.Cancel = true;
                return;
            }
            if (IsInAction)
            {
                IsInAction = false;
                ManageActionItem();
                e.Cancel = true;
                return;
            }
            if (IsInLineItemMultiSelect)
            {
                IsInLineItemMultiSelect = false;
                ToggleLineItemMultiView();
                ShowLineItemsAppBar();
                UncheckAll();
                e.Cancel = true;
                return;
            }
            if (IsInSearch)
            {
                IsInSearch = false;
                ManageSearch();
                e.Cancel = true;
                return;
            }
            base.OnBackKeyPress(e);
        }

        private void UncheckAll()
        {
            foreach (LineItem item in SummaryViewModel.LineItemList)
            {
                item.IsCheckboxChecked = false;
            }
        }

        private bool IsInLineItem  { get; set; }
        private bool IsInAction { get; set; }
        private bool IsInLineItemMultiSelect { get; set; }
        private bool IsInLineItemMultiAdjust { get; set; }
        private bool IsInSearch { get; set; }
        private bool OnLoad { get; set; }
        private bool HasPreparedAppBar { get; set; }

        //invoice summary
        private List<ReasonCode> RejectInput { get; set; }
        private List<ReasonCode> AdjustInput { get; set; }

        //line item list
        public LineItemConfirmationDetails MultiConfirmation { get; set; }

        //line item summary
        private InvoiceSummary LineItemSummaryInput { get; set; }

        //note
        private NotesData ViewNoteInput { get; set; }

        private InvoiceSummaryViewModel SummaryViewModel { get; set; }
        private PivotItem PivotItem;

        public InvoiceSummary InvoiceSummaryDetails { get; set; }
        public List<LineItem> LineItemList { get; set; }
        public List<InvoiceModel> AwaitingInvoices { get; set; }
    }
}