/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

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
    public partial class LineItemSummary : BasePage
    {
        private string currentLineItemId;

        public LineItemSummary()
        {
            InitializeComponent();
            this.Loaded += LineItemSummary_Loaded;
            this.Unloaded += LineItemSummary_Unloaded;
        }

        void LineItemSummary_Unloaded(object sender, RoutedEventArgs e)
        {
            PageInProgress = false;
        }

        void LineItemSummary_Loaded(object sender, RoutedEventArgs e)
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
            if (this.Source == Source.LINE_ITEM_LIST)
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
            if (this.Source == Source.LINE_ITEM_LIST)
            {
                SummaryViewModel = new InvoiceSummaryViewModel();
                invoiceHeader.SetHeaderDetails(HeaderDetails);
                PrepareViewModel();
                this.DataContext = SummaryViewModel;
                currentLineItemId = CurrentLineItem.LineItemId;
            }
            if (this.Source == Source.BACK_TO_LINE_ITEM_SUMMARY)
            {
                int index = LineItemList.IndexOf(LineItemList.First(x => x.LineItemId == SelectedLineItemId));
                if (IsInAction)
                {
                    IsInAction = false;
                    ManageActionItem();
                }
                RetrieveLineItemSummary(index);
            }
            if (this.Source == Source.BACK_TO_LINE_ITEM_SUMMARY_FROM_NOTES)
            {
                int index = LineItemList.IndexOf(LineItemList.First(x => x.LineItemId == CurrentLineItem.LineItemId));
                RetrieveLineItemSummary(index);
            }
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (Constants.ExternalURI.Equals(e.Uri.ToString()))
            {
                this.PreviousSource = this.Source;
                this.Source = Source.EXTERNAL;
                return;
            }
            InvoiceCommonDetails commonDetails = e.Content as InvoiceCommonDetails;
            if (commonDetails != null)
            {
                commonDetails.Source = Source.BACK_TO_LINE_ITEM_LIST;
                commonDetails.MultiConfirmation = new LineItemConfirmationDetails()
                {
                    InvoiceDetails = HeaderDetails
                };
            }
            RejectPage rejectPage = e.Content as RejectPage;
            if (rejectPage != null)
            {
                rejectPage.Source = Source.LINE_ITEM_SINGLE_REJECT;
                rejectPage.Reasons = RejectInput;
                List<string> lineItems = new List<string>();
                lineItems.Add(CurrentLineItem.LineItemId);
                rejectPage.LineItemRejectDetails = new LineItemConfirmationDetails()
                {
                    InvoiceDetails = HeaderDetails,
                    SelectedLineItemIds = lineItems,
                    NetAmount = LineItemSummaryDetails.NetTotal
                };
            }
            AdjustPage adjustPage = e.Content as AdjustPage;
            if (adjustPage != null)
            {
                adjustPage.Source = Source.LINE_ITEM_SINGLE_ADJUST;
                adjustPage.Reasons = AdjustInput;
                List<string> lineItems = new List<string>();
                lineItems.Add(CurrentLineItem.LineItemId);
                adjustPage.LineItemAdjustDetails = new LineItemConfirmationDetails()
                {
                    InvoiceDetails = HeaderDetails,
                    SelectedLineItemIds = lineItems,
                    CurrencySymbol = LineItemSummaryDetails.CurrencySymbol,
                    PositiveAdjustment = LineItemSummaryDetails.Permissions.PositiveAdjustment,
                    NetAmount = LineItemSummaryDetails.NetTotal
                };
            }
            NotesPage notesPage = e.Content as NotesPage;
            if (notesPage != null)
            {
                notesPage.Input = LineItemSummaryDetails.NotesList;
            }
            ViewNotePage viewNote = e.Content as ViewNotePage;
            if (viewNote != null)
            {
                viewNote.Note = ViewNoteInput;
            }
            base.OnNavigatedFrom(e);
        }

        protected override void PrepareAppBarMap()
        {
            AppBarModelList = new List<ApplicationBarModel>();

            ApplicationBarModel appBarModel;
            if (LineItemSummaryDetails.Permissions.Reject || LineItemSummaryDetails.Permissions.Adjust)
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

        private bool IsFirstItem()
        {
            return (GetIndex(CurrentLineItem.LineItemId.ToString()) == 0);
        }

        private bool IsLastItem()
        {
            return (GetIndex(CurrentLineItem.LineItemId.ToString()) == (LineItemList.Count - 1));
        }

        private int GetIndex(string lineItemId)
        {
            return LineItemList.IndexOf(LineItemList.First(x => x.LineItemId.ToString().Equals(lineItemId)));
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
                case Constants.RejectIconPath:
                    OnLineItemReject();
                    break;
                case Constants.AdjustIconPath:
                    OnLineItemAdjust();
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
                RemoveAppBarButton(Constants.ActionItemIconPath);
                RemoveAppBarButton(Constants.LeftArrowIconPath);
                RemoveAppBarButton(Constants.RightArrowIconPath);
                if (LineItemSummaryDetails.Permissions.Adjust) AddButtonInAppBar(Constants.AdjustIconPath, Constants.AdjustTitle, true);
                if (LineItemSummaryDetails.Permissions.Reject) AddButtonInAppBar(Constants.RejectIconPath, Constants.RejectTitle, true);
            }
            else
            {
                RemoveAppBarButton(Constants.RejectIconPath);
                RemoveAppBarButton(Constants.AdjustIconPath);
                AddButtonInAppBar(Constants.ActionItemIconPath, Constants.ActionItemTitle, true);
                AddButtonInAppBar(Constants.LeftArrowIconPath, Constants.LeftArrowTitle, !IsFirstItem());
                AddButtonInAppBar(Constants.RightArrowIconPath, Constants.RightArrowTitle, !IsLastItem());
            }
        }

        private void OnLeftArrow()
        {
            int index = GetIndex(CurrentLineItem.LineItemId.ToString()) - 1;
            currentLineItemId = LineItemList[ index ].LineItemId;
            ManagePreviousNext(index, Constants.LeftArrowIconPath, (index > 0));
        }

        private void OnRightArrow()
        {
            int index = GetIndex(CurrentLineItem.LineItemId.ToString()) + 1;
            currentLineItemId = LineItemList[ index ].LineItemId;
            ManagePreviousNext(index, Constants.RightArrowIconPath, (index < (LineItemList.Count - 1)));
        }

        private void ManagePreviousNext(int index, string key, bool enable)
        {
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }
            ManageAppBarButtonEnable(key, enable);
            RetrieveLineItemSummary(index);
        }

        private void RetrieveLineItemSummary(int index)
        {
            try
            {
                string lineItemId = LineItemList[index].LineItemId.ToString();
                PageInProgress = true;
                this.ProgressBar.Show();
                DisableApplicationBar();
                ServiceInvoker.InvokeServiceUsingGet("/api/t360/networks/invoices/" + HeaderDetails.InvoiceId + "/lineitem/" + lineItemId,
                                                     GetLineItemDetails,
                                                     false);
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
                    LineItemSummaryDetails = JsonConvert.DeserializeObject<InvoiceSummary>(result.Output);
                    CurrentLineItem = LineItemList.First(x => x.LineItemId == currentLineItemId);
                    PrepareViewModel();
                    if (Source != Source.BACK_TO_LINE_ITEM_SUMMARY_FROM_NOTES)
                    {
                        lineItemSummary.UpdateLayout();
                        lineItemSummary.ScrollIntoView(lineItemSummary.Items[0]);
                    }
                    this.ProgressBar.Hide();
                    PageInProgress = false;
                    ManageAppBarButtonEnable(Constants.ActionItemIconPath, true);
                    ManageAppBarButtonEnable(Constants.LeftArrowIconPath, !IsFirstItem());
                    ManageAppBarButtonEnable(Constants.RightArrowIconPath, !IsLastItem());
                });
            }
            else
            {
                currentLineItemId = CurrentLineItem.LineItemId;
                List<Error> resultError = result.ErrorDetails;
                ShowError(new AppException(resultError), Constants.LineItemSummaryError);
                if (resultError[ 0 ] != null && T360ErrorCodes.NotInReviewerQueue == resultError[ 0 ].Code)
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
            List<string> ids = new List<string>();
            ids.Add(CurrentLineItem.LineItemId);
            string postData = JsonConvert.SerializeObject(new ReasonCodeMultipleInput()
            {
                Action = Constants.RejectLineItemAction,
                InvoiceId = HeaderDetails.InvoiceId.ToString(),
                SelectedLineItemIds = ids
            });
            try
            {
                PageInProgress = true;
                this.ProgressBar.Show();
                ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/GetReasonCodes", postData, false, RejectLineItem);
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

        private void RejectLineItem(object sender, ServiceEventArgs args)
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
                ShowError(new AppException(resultError), Constants.InvoiceReasonsError);
                if (resultError[ 0 ] != null && T360ErrorCodes.NotInReviewerQueue == resultError[ 0 ].Code)
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
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                EnableApplicationBar();
                return;
            }

            if (LineItemSummaryDetails.Permissions != null)
            {
                if (!LineItemSummaryDetails.Permissions.AdjustAllowed)
                {
                    MessageBoxResult msgResult = MessageBox.Show(Constants.TaxedLineItemMsg, Constants.TaxedInvoiceTitle, MessageBoxButton.OK);
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
            List<string> ids = new List<string>();
            ids.Add(CurrentLineItem.LineItemId);
            string postData = JsonConvert.SerializeObject(new ReasonCodeMultipleInput() { Action = Constants.AdjustLineItemAction,
                                                                                          InvoiceId = HeaderDetails.InvoiceId.ToString(),
                                                                                          SelectedLineItemIds = ids });
            try
            {
                PageInProgress = true;
                this.ProgressBar.Show();
                ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/GetReasonCodes", postData, false, AdjustLineItem);
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

        private void AdjustLineItem(object sender, ServiceEventArgs args)
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
                ShowError(new AppException(resultError), Constants.InvoiceReasonsError);
                if (resultError[ 0 ] != null && T360ErrorCodes.NotInReviewerQueue == resultError[ 0 ].Code)
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
                list[ 0 ].IsEnabled = enable;
                appBarModel = list[ 0 ];
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

        private void PrepareViewModel()
        {
            List<SummaryView> summaryViews = new List<SummaryView>();

            //Line Item Summary Section
            summaryViews.Add(GetHeaderView(Constants.ItemsDetailsHeader));
            summaryViews.Add(GetDetailView(Constants.DateLabel, LineItemSummaryDetails.Date));

            if (LineItemSummaryDetails.TimeKeeper.Length > 0)
                summaryViews.Add(GetDetailView(Constants.TimeKeeperLabel, LineItemSummaryDetails.TimeKeeper));

            summaryViews.Add(GetDetailView(Constants.AmountLabel, LineItemSummaryDetails.NetAmount));

            //Narrative Section
            if (LineItemSummaryDetails.NarrativeText.Length == 0)
                summaryViews.Add(GetHeaderValueView(Constants.NarrativeHeader, "None"));
            else
                summaryViews.Add(GetHeaderValueView(Constants.NarrativeHeader, LineItemSummaryDetails.NarrativeText));

            //Firm/Vendor Billing Section
            summaryViews.Add(GetHeaderView(Constants.FirmVendorBillingHeader));
            summaryViews.Add(GetDetailView(Constants.TaskLabel, LineItemSummaryDetails.VendorTask));
            summaryViews.Add(GetDetailView(Constants.ActivityLabel, LineItemSummaryDetails.VendorActivity));
            summaryViews.Add(GetDetailView(Constants.UnitsLabel, LineItemSummaryDetails.VendorUnits));
            summaryViews.Add(GetDetailView(Constants.RateLabel, LineItemSummaryDetails.VendorRate));
            summaryViews.Add(GetDetailView(Constants.VendorAdjustmentLabel, LineItemSummaryDetails.VendorAdjustment));
            summaryViews.Add(GetDetailView(Constants.BilledTotalLabel, LineItemSummaryDetails.VendorBilledTotal));

            //Flags Section
            summaryViews.Add(GetHeaderView(Constants.FlagSectionHeader));
            if (LineItemSummaryDetails.Flagslist.Count == 0)
            {
                summaryViews.Add(GetNoneView());
            }
            else
            {
                foreach (FlagDetails flag in LineItemSummaryDetails.Flagslist)
                {
                    string imagePath = Constants.FlagLineItemHighPriority.Equals(flag.Priority)
                        ? Constants.HighPriorityFlagIconPath
                        : Constants.FlagLineItemMediumPriority.Equals(flag.Priority) ? Constants.MediumPriorityFlagIconPath
                                                                                    : Constants.LowPriorityFlagIconPath;
                    summaryViews.Add(GetFlagView(flag.WarningInfo, imagePath));
                }
            }

            //In-House Review Section
            summaryViews.Add(GetHeaderView(Constants.InHouseReviewHeader));
            summaryViews.Add(GetDetailView(Constants.ITPAdjustmentsLabel, LineItemSummaryDetails.ItpAdjustment));
            summaryViews.Add(GetDetailView(Constants.ReviewerAdjustmentsLabel, LineItemSummaryDetails.ReviewerAdjustment));
            summaryViews.Add(GetDetailView(Constants.NetTotalLabel, LineItemSummaryDetails.NetTotal));

            //Adjustment Section
            if (LineItemSummaryDetails.AdjustmentsList.Count == 0)
            {
                summaryViews.Add(GetHeaderView(Constants.AdjustmentHeader));
                summaryViews.Add(GetNoneView());
            }
            else
            {
                Dictionary<string, List<AdjustmentListDetails>> headers = new Dictionary<string, List<AdjustmentListDetails>>();
                AdjustmentListDetails adjustment;
                for (int i = 0; i < LineItemSummaryDetails.AdjustmentsList.Count; i++)
                {
                    List<AdjustmentListDetails> adjustmentList;
                    adjustment = LineItemSummaryDetails.AdjustmentsList[ i ];
                    if (!headers.ContainsKey(adjustment.GroupDescription))
                    {
                        adjustmentList = new List<AdjustmentListDetails>();
                        adjustmentList.Add(adjustment);
                        headers.Add(adjustment.GroupDescription, adjustmentList);
                    }
                    else
                    {
                        adjustmentList = headers[ adjustment.GroupDescription ];
                        adjustmentList.Add(adjustment);
                    }
                }
                foreach (string key in headers.Keys)
                {
                    summaryViews.Add(GetHeaderView(key));
                    foreach (AdjustmentListDetails adj in headers[ key ])
                    {
                        summaryViews.Add(GetAdjustmentView(adj.Owner, adj.Description, adj.Amount));
                    }
                }
            }
            //Notes Section
            if (LineItemSummaryDetails.Permissions.Notes)
            {
                summaryViews.Add(GetHeaderView(Constants.NotesHeader));
                if (LineItemSummaryDetails.NotesList.Count == 0)
                {
                    summaryViews.Add(GetNoneView());
                }
                else
                {
                    NotesData note;
                    for (int i = 0; i < LineItemSummaryDetails.NotesList.Count; i++)
                    {
                        if (i > 1) break;
                        note = LineItemSummaryDetails.NotesList[ i ];
                        summaryViews.Add(GetNoteView(note, false));
                    }
                    if (LineItemSummaryDetails.NotesList.Count > 2)
                    {
                        summaryViews.Add(GetNoteView(null, true));
                    }
                }
            }

            //Taxes Section
            if (LineItemSummaryDetails.TaxList.Count > 0)
            {
                List<ValueList> valueList;
                foreach (string key in LineItemSummaryDetails.TaxList.Keys)
                {
                    valueList = LineItemSummaryDetails.TaxList[ key ];
                    summaryViews.Add(GetHeaderView(key + " Taxes"));
                    foreach (ValueList value in valueList)
                    {
                        summaryViews.Add(GetDetailView(Constants.TaxTypeCodeLabel, value.TaxTypeCode));
                        summaryViews.Add(GetDetailView(Constants.TaxJurisdictionCodeLabel, value.TaxJurisdictionCode));
                        summaryViews.Add(GetDetailView(Constants.TaxRateLabel, value.TaxRate));
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

        private SummaryView GetHeaderValueView(string title, string value)
        {
            SummaryView view = new SummaryView();
            view.IsHeaderValueView = Visibility.Visible;
            view.HeaderText = title;
            view.ValueText = value;
            return view;
        }

        private SummaryView GetDetailView(string label, string value)
        {
            SummaryView view = new SummaryView();
            view.IsDetailView = Visibility.Visible;
            view.DetailHeader = label;
            view.DetailValue = value;
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

        private SummaryView GetAdjustmentView(string owner, string description, string amount)
        {
            SummaryView view = new SummaryView();
            view.IsAdjustmentsView = Visibility.Visible;
            view.Owner = owner;
            view.Description = description;
            view.Amount = amount;
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
                view.Creator = note.Owner;
                view.CreatedTime = note.Date;
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
            PageInProgress = true;
            this.ProgressBar.Show();
            UserData.Instance.DetailsToNotes = true;
            Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Invoice/NotesPage.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
            this.ProgressBar.Hide();
        }

        private void lineItemSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lineItemSummary.SelectedIndex < 0) return;

            SummaryView view = (SummaryView)e.AddedItems[0];
            if (view.IsNotesView == Visibility.Visible && view.IsNotesRow == Visibility.Visible)
            {
                if (!ServiceInvoker.IsConnected)
                {
                    ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                    return;
                }

                PageInProgress = true;
                this.ProgressBar.Show();

                UserData.Instance.DetailsToViewNote = true;

                ViewNoteInput = (NotesData)(view.Tag);

                Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Invoice/ViewNotePage.xaml", UriKind.Relative);
                NavigationService.Navigate(uri);

                this.ProgressBar.Hide();
            }
            lineItemSummary.SelectedIndex = -1;
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
            base.OnBackKeyPress(e);
        }

        private bool IsInAction { get; set; }
        private bool HasPreparedAppBar { get; set; }
        private InvoiceSummaryViewModel SummaryViewModel { get; set; }

        //line item summary
        private List<ReasonCode> RejectInput { get; set; }
        private List<ReasonCode> AdjustInput { get; set; }

        //note
        private NotesData ViewNoteInput { get; set; }

        public InvoiceSummary LineItemSummaryDetails { get; set; }
        public InvoiceSummary HeaderDetails { get; set; }
        public List<LineItem> LineItemList { get; set; }
        public LineItem CurrentLineItem { get; set; }
        public string SelectedLineItemId { get; set; }
    }
}