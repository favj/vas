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

namespace Tymetrix.T360.Mobile.Client.AppWP7.Invoice
{
    public partial class InvoiceListing : BasePage
    {
        private readonly GridLength PlainSelectColumn1 = new GridLength(0, GridUnitType.Star);
        private readonly GridLength PlainSelectColumn2 = new GridLength(1, GridUnitType.Star);
        private readonly GridLength MultiSelectPortraitColumn1 = new GridLength(1.25, GridUnitType.Star);
        private readonly GridLength MultiSelectPortraitColumn2 = new GridLength(8.75, GridUnitType.Star);
        private readonly GridLength MultiSelectLandscapeColumn1 = new GridLength(1, GridUnitType.Star);
        private readonly GridLength MultiSelectLandscapeColumn2 = new GridLength(9, GridUnitType.Star);

        private readonly GridLength PlainDaysInQueueColumn = new GridLength(320, GridUnitType.Pixel);
        private readonly GridLength MultiDaysInQueueColumn = new GridLength(200, GridUnitType.Pixel);

        private readonly Thickness LabelPortraitPlainMargin = new Thickness(0, 0, 0, 0);
        private readonly Thickness LabelPortraitMultiMargin = new Thickness(40, 0, 0, 0);
        private readonly Thickness LabelLandscapePlainMargin = new Thickness(120, 0, 0, 0);
        private readonly Thickness LabelLandscapeMultiMargin = new Thickness(160, 0, 0, 0);
        private readonly Thickness BilledAmountPortraitPlainMargin = new Thickness(60, 0, 0, 0);
        private readonly Thickness BilledAmountPortraitMultiMargin = new Thickness(100, 0, 0, 0);
        private readonly Thickness BilledAmountLandscapePlainMargin = new Thickness(180, 0, 0, 0);
        private readonly Thickness BilledAmountLandscapeMultiMargin = new Thickness(220, 0, 0, 0);
        private readonly Thickness NetAmountPortraitPlainMargin = new Thickness(43, 0, 0, 0);
        private readonly Thickness NetAmountPortraitMultiMargin = new Thickness(83, 0, 0, 0);
        private readonly Thickness NetAmountLandscapePlainMargin = new Thickness(163, 0, 0, 0);
        private readonly Thickness NetAmountLandscapeMultiMargin = new Thickness(203, 0, 0, 0);

        private readonly Thickness ListWithKeyboardOpen = new Thickness(0, 0, 0, 350);
        private readonly Thickness ListWithKeyboardClose = new Thickness(0, 0, 0, 0);

        public InvoiceListing()
        {
            InitializeComponent();
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
                this.Source = this.PreviousSource;
                base.OnNavigatedTo(e);
                return;
            }
            InvoiceListModel = new InvoiceListingModel();
            this.DataContext = InvoiceListModel;
            LoadInvoiceList();
            base.OnNavigatedTo(e);
        }

        private void LoadInvoiceList()
        {
            try
            {
                PageInProgress = true;
                ProgressBar.Show();
                ServiceInvoker.InvokeServiceUsingGet("/api/t360/Invoice/GetAwaitingInvoiceList", GetAwaitingInvoiceList, false);
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

        private void GetAwaitingInvoiceList(object sender, ServiceEventArgs serviceEventArgs)
        {
            ServiceResponse result = serviceEventArgs.Result;
            if (result.Status)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    InvoiceListModel.OriginalInvoiceList = JsonConvert.DeserializeObject<List<InvoiceModel>>(result.Output);

                    InvoiceListModel.HeaderTitle = "Invoice List (" + InvoiceListModel.OriginalInvoiceList.Count + ")";
                    InvoiceListModel.SearchTextVisible = Visibility.Collapsed;
                    InvoiceListModel.InvoiceList = InvoiceListModel.OriginalInvoiceList;
                    InvoiceListModel.DataFound = (InvoiceListModel.OriginalInvoiceList.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
                    InvoiceListModel.NoDataFound = (InvoiceListModel.OriginalInvoiceList.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
                    if (InvoiceListModel.OriginalInvoiceList.Count > 0) CanPrepareAppBar = true;

                    ManageMultiSelect();
                    foreach (InvoiceModel model in InvoiceListModel.InvoiceList)
                    {
                        model.FlagNotVisible = (model.FlagsCount == 0) ? Visibility.Visible : Visibility.Collapsed;
                        model.FlagVisible = (model.FlagsCount == 0) ? Visibility.Collapsed : Visibility.Visible;
                    }
                    SetPageHeader(InvoiceListModel.HeaderTitle);
                    ProgressBar.Hide();
                    PageInProgress = false;
                    ShowAppBar();
                });
            }
            else
            {
                List<Error> resultError = result.ErrorDetails;
                ShowError(new AppException(resultError), Constants.InvoiceListError);
            }
        }

        private void ShowAppBar()
        {
            if (CanPrepareAppBar)
            {
                if (!HasPreparedAppBar)
                {
                    PrepareApplicationBar();
                    HasPreparedAppBar = true;
                }
                else
                {
                    ManageAppBarVisible(true);
                    EnableApplicationBar();
                }
            }
        }

        private void RegisterEvents()
        {
            Messenger.Default.Register<InvoiceModel>(this, Constants.InvoiceMultiCheckChange, ManageCheckCount);
        }

        private void UnregisterEvents()
        {
            Messenger.Default.Unregister<InvoiceModel>(this, Constants.InvoiceMultiCheckChange, ManageCheckCount);
        }

        private void ManageCheckCount(InvoiceModel param)
        {
            if (InvoiceListModel.InvoiceList == null) return;

            int count = InvoiceListModel.InvoiceList.Where(x => x.IsCheckboxChecked).ToList().Count;
            pageHeader.HeaderTitle = "Invoice List (" + count + ")";

            ManageAppBarButtonEnable(Constants.ApproveIconPath, count > 0);
            ManageAppBarButtonEnable(Constants.RejectIconPath, count > 0);
        }

        private void SetPageHeader(string title)
        {
            pageHeader.HeaderTitle = title;
        }

        protected override void PrepareAppBarMap()
        {
            AppBarModelList = new List<ApplicationBarModel>();

            ApplicationBarModel appBarModel;
            if (UserData.Instance.InvoiceQuickActionsAccess)
            {
                appBarModel = new ApplicationBarModel();
                appBarModel.IconPath = Constants.MultiSelectIconPath;
                appBarModel.IsEnabled = true;
                appBarModel.ButtonText = Constants.MultiSelectTitle;
                AppBarModelList.Add(appBarModel);
            }

            appBarModel = new ApplicationBarModel();
            appBarModel.IconPath = Constants.SearchIconPath;
            appBarModel.IsEnabled = true;
            appBarModel.ButtonText = Constants.SearchTitle;
            AppBarModelList.Add(appBarModel);
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            base.OnOrientationChanged(e);

            if (InvoiceListModel == null || InvoiceListModel.InvoiceList == null) return;

            ManageMultiSelect();
            if (IsInSearch)
            {
                SetListMargin();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (Constants.ExternalURI.Equals(e.Uri.ToString()))
            {
                this.PreviousSource = this.Source;
                this.Source = Source.EXTERNAL;
                return;
            }
            ConfirmationPage confirmation = e.Content as ConfirmationPage;
            if (confirmation != null)
            {
                confirmation.Source = IsApprove ? Source.INVOICE_MULTI_APPROVE : Source.INVOICE_MULTI_REJECT;
                confirmation.InvoiceInputDetails = ApproveConfirmation;
                if (IsInMultiSelect)
                {
                    IsInMultiSelect = false;
                    ManageAppBarVisible(false);
                    ManageAppBar();
                }
            }
            InvoiceCommonDetails commonDetails = e.Content as InvoiceCommonDetails;
            if (commonDetails != null)
            {
                commonDetails.Source = Source.INVOICE_LIST;
                commonDetails.InvoiceSummaryDetails = InvoiceSummaryDetails;
                commonDetails.AwaitingInvoices = InvoiceListModel.InvoiceList;
            }
            if (CanPrepareAppBar) ManageAppBarVisible(false);
            SetPageHeader(string.Empty);
            base.OnNavigatedFrom(e);
        }

        protected override void OnAppBarButtonClick(ApplicationBarModel appBarModel)
        {
            if (PageInProgress) return;

            switch (appBarModel.IconPath)
            {
                case Constants.MultiSelectIconPath:
                    OnMultiSelect();
                    break;
                case Constants.SearchIconPath:
                    OnSearch();
                    break;
                case Constants.ApproveIconPath:
                    OnApprove();
                    break;
                case Constants.RejectIconPath:
                    OnReject();
                    break;
                default: break;
            }
        }

        private void OnMultiSelect()
        {
            IsInMultiSelect = true;
            ManageMultiSelect();
            ManageAppBar();
            ManageTitle();
        }

        private void OnSearch()
        {
            IsInSearch = true;
            ManageSearch();
        }

        private void ManageSearch()
        {
            TextBox txtBox = invoiceSearch.GetSearchTextBox();
            if (IsInSearch)
            {
                txtBox.TextChanged += OnInvoiceSearch_TextChanged;
                this.invoiceSearch.SearchPage = this;
                invoiceSearch.Focus();
            }
            else
            {
                txtBox.TextChanged -= OnInvoiceSearch_TextChanged;
                this.invoiceSearch.SearchPage = null;
                InvoiceListModel.InvoiceList = InvoiceListModel.OriginalInvoiceList;
                InvoiceListModel.DataFound = Visibility.Visible;
                InvoiceListModel.NoDataFound = Visibility.Collapsed;
            }
            txtBox.Text = string.Empty;
            InvoiceListModel.HeaderVisible = IsInSearch ? Visibility.Collapsed : Visibility.Visible;
            InvoiceListModel.SearchTextVisible = IsInSearch ? Visibility.Visible : Visibility.Collapsed;
            InvoiceListModel.InvoiceList = InvoiceListModel.OriginalInvoiceList;
            InvoiceListModel.ListOpacity = IsInSearch ? 0.25 : 1;
            InvoiceListModel.ListEnable = !IsInSearch;
            ManageAppBarVisible(!IsInSearch);
        }

        //Search Implementation for Invoice List
        private void OnInvoiceSearch_TextChanged(object sender, EventArgs e)
        {
            if (!IsInSearch) return;

            List<InvoiceModel> searchInvoice;
            string searchText = invoiceSearch.GetSearchTextBox().Text;
            if (searchText.Length >= 3)
            {
                searchInvoice = new List<InvoiceModel>(from inv in InvoiceListModel.OriginalInvoiceList
                                                       where ((!inv.IsConcealedMatter && inv.MatterName.ToLower().Contains(searchText.ToLower())) ||
                                                              inv.InvoiceNumber.ToLower().Contains(searchText.ToLower()) ||
                                                              inv.CompanyName.ToLower().Contains(searchText.ToLower()))
                                                       select inv).ToList();

                bool hasInvoice = (searchInvoice.Count > 0);
                InvoiceListModel.DataFound = hasInvoice ? Visibility.Visible : Visibility.Collapsed;
                InvoiceListModel.NoDataFound = hasInvoice ? Visibility.Collapsed : Visibility.Visible;
                if (hasInvoice)
                {
                    InvoiceListModel.ListOpacity = 1;
                    InvoiceListModel.ListEnable = true;
                    InvoiceListModel.InvoiceList = searchInvoice;
                    lbInvoice.UpdateLayout();
                    lbInvoice.ScrollIntoView(lbInvoice.Items[0]);
                    SetListMargin();
                }
            }
            else
            {
                InvoiceListModel.ListOpacity = 0.25;
                InvoiceListModel.ListEnable = false;
                InvoiceListModel.DataFound = Visibility.Visible;
                InvoiceListModel.NoDataFound = Visibility.Collapsed;
                InvoiceListModel.InvoiceList = InvoiceListModel.OriginalInvoiceList;
            }
        }

        public override void OnSearchGotFocus()
        {
            SetListMargin();
        }

        public override void OnSearchLostFocus()
        {
            InvoiceListModel.ListMargin = ListWithKeyboardClose;
        }

        private void SetListMargin()
        {
            if (this.CurrentOrientation == PageOrientation.LandscapeRight ||
               this.CurrentOrientation == PageOrientation.LandscapeLeft ||
               this.CurrentOrientation == PageOrientation.Landscape)
                InvoiceListModel.ListMargin = ListWithKeyboardClose;
            else
                InvoiceListModel.ListMargin = ListWithKeyboardOpen;
        }

        private void OnApprove()
        {
            PageInProgress = true;
            ManageAppBarVisible(false);
            CalculateInvoiceNetAmount(true);
        }

        private void OnReject()
        {
            PageInProgress = true;
            ManageAppBarVisible(false);
            CalculateInvoiceNetAmount(false);
        }

        private void CalculateInvoiceNetAmount(bool isApprove)
        {
            try
            {
                if (!ServiceInvoker.IsConnected)
                {
                    ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                    ManageAppBarVisible(true);
                    return;
                }
                IsApprove = isApprove;
                List<InvoiceModel> selectedInvoices = InvoiceListModel.InvoiceList.Where(x => x.IsCheckboxChecked).ToList();

                List<string> selectedIds = new List<string>();
                selectedInvoices.ForEach(x => { selectedIds.Add(x.InvoiceId.ToString()); });

                Dictionary<string, List<string>> selectedInvoiceIds = new Dictionary<string, List<string>>();
                selectedInvoiceIds.Add("SelectedInvoiceIds", selectedIds);

                ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/CalculateInvoiceNetAmount", JsonConvert.SerializeObject(selectedInvoiceIds), false, TotalNetAmountHandler);
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
                ManageAppBarVisible(true);
            }
        }

        private void TotalNetAmountHandler(object sender, ServiceEventArgs se)
        {
            ServiceResponse result = se.Result;
            if (result.Status)
            {
                string netTotal = (string)result.Output;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ApproveConfirmation = new InvoiceConfirmationDetails();
                    ApproveConfirmation.NetTotal = netTotal;
                    ApproveConfirmation.SelectedInvoices = InvoiceListModel.InvoiceList.Where(x => x.IsCheckboxChecked).ToList();

                    Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP7;component/Invoice/ConfirmationPage.xaml", UriKind.Relative);
                    this.NavigationService.Navigate(uri);
                });
            }
            else
            {
                List<Error> resultError = result.ErrorDetails;
                ShowError(new AppException(resultError));
                if (resultError[ 0 ] != null && T360ErrorCodes.NotInReviewerQueue == resultError[ 0 ].Code)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        RedirectToInvoiceList();
                    });
                }
                else
                {
                    ManageAppBarVisible(true);
                }
            }
        }

        private void ManageTitle()
        {
            SetPageHeader(IsInMultiSelect ? "Invoice List (0)" : InvoiceListModel.HeaderTitle);
        }

        private void ManageAppBar()
        {
            if (IsInMultiSelect)
            {
                RemoveAppBarButton(Constants.MultiSelectIconPath);
                RemoveAppBarButton(Constants.SearchIconPath);
                AddButtonInAppBar(Constants.ApproveIconPath, Constants.ApproveTitle);
                AddButtonInAppBar(Constants.RejectIconPath, Constants.RejectTitle);
            }
            else
            {
                RemoveAppBarButton(Constants.ApproveIconPath);
                RemoveAppBarButton(Constants.RejectIconPath);
                AddAppBarButton(AppBarModelList.Where(x => x.IconPath == Constants.MultiSelectIconPath).ToList()[ 0 ]);
                AddAppBarButton(AppBarModelList.Where(x => x.IconPath == Constants.SearchIconPath).ToList()[ 0 ]);
            }
        }

        private void AddButtonInAppBar(string path, string title)
        {
            ApplicationBarModel appBarModel;
            List<ApplicationBarModel> list = AppBarModelList.Where(x => x.IconPath == path).ToList();
            if (list.Count == 0)
            {
                appBarModel = CreateAppBarIcon(path, title);
            }
            else
            {
                list[ 0 ].IsEnabled = false;
                appBarModel = list[ 0 ];
            }
            AddAppBarButton(appBarModel);
        }

        private ApplicationBarModel CreateAppBarIcon(string path, string title)
        {
            ApplicationBarModel appBarModel = new ApplicationBarModel();
            appBarModel.IconPath = path;
            appBarModel.IsEnabled = false;
            appBarModel.ButtonText = title;
            return appBarModel;
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
            if (IsInMultiSelect)
            {
                IsInMultiSelect = false;
                ManageMultiSelect();
                UncheckAll();
                ManageAppBar();
                ManageTitle();
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

        private void ManageMultiSelect()
        {
            bool isPortrait = (CurrentOrientation == PageOrientation.Portrait ||
                               CurrentOrientation == PageOrientation.PortraitDown ||
                               CurrentOrientation == PageOrientation.PortraitUp);
            foreach (InvoiceModel model in InvoiceListModel.InvoiceList)
            {
                model.CheckVisible = IsInMultiSelect ? Visibility.Visible : Visibility.Collapsed;
                model.LabelMargin = isPortrait ? (IsInMultiSelect ? LabelPortraitMultiMargin: LabelPortraitPlainMargin)
                                               : (IsInMultiSelect ? LabelLandscapeMultiMargin : LabelLandscapePlainMargin);
                model.BilledAmountMargin = isPortrait ? (IsInMultiSelect ? BilledAmountPortraitMultiMargin : BilledAmountPortraitPlainMargin)
                                                      : (IsInMultiSelect ? BilledAmountLandscapeMultiMargin : BilledAmountLandscapePlainMargin);
                model.NetAmountMargin = isPortrait ? (IsInMultiSelect ? NetAmountPortraitMultiMargin : NetAmountPortraitPlainMargin)
                                                   : (IsInMultiSelect ? NetAmountLandscapeMultiMargin : NetAmountLandscapePlainMargin);
                model.DaysInQueueWidth = !IsInMultiSelect ? PlainDaysInQueueColumn : MultiDaysInQueueColumn;
                model.DaysInQueueToDisplay = !IsInMultiSelect ? "Days in Queue (" + model.DaysInQueue.ToString() + ")" : "(" + model.DaysInQueue.ToString() + ")";

                model.BilledAmountToDisplay = (model.BilledAmount.Length > 9 ? model.BilledAmount.Substring(0, 9) + "..." : model.BilledAmount);
                model.NetAmountToDisplay = (model.NetAmount.Length > 11 ? model.NetAmount.Substring(0, 11) + "..." : model.NetAmount);
            }
            if (IsInMultiSelect)
            {
                RegisterEvents();
            }
            else
            {
                UnregisterEvents();
            }
        }

        private void UncheckAll()
        {
            foreach (InvoiceModel model in InvoiceListModel.InvoiceList)
            {
                model.IsCheckboxChecked = false;
            }
        }

        private void lbInvoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InvoiceListModel.SelectedIndex < 0) return;

            try
            {
                InvoiceModel invoiceModel = (InvoiceModel)lbInvoice.SelectedItem;
                if (IsInMultiSelect)
                {
                    invoiceModel.IsCheckboxChecked = !invoiceModel.IsCheckboxChecked;
                    InvoiceListModel.SelectedIndex = -1;
                }
                else
                {
                    PageInProgress = true;
                    this.ProgressBar.Show();
                    DisableApplicationBar();
                    ServiceInvoker.InvokeServiceUsingGet("/api/t360/networks/invoice/" + invoiceModel.InvoiceId, GetInvoiceSummary, false);
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

        private void GetInvoiceSummary(object sender, ServiceEventArgs se)
        {
            ServiceResponse result = se.Result;
            if (result.Status)
            {
                InvoiceSummaryDetails = JsonConvert.DeserializeObject<InvoiceSummary>(result.Output);

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP7;component/Invoice/InvoiceCommonDetails.xaml", UriKind.Relative);
                    this.NavigationService.Navigate(uri);
                    this.ProgressBar.Hide();
                    PageInProgress = false;
                });
            }
            else
            {
                List<Error> resultError = result.ErrorDetails;
                ShowError(new AppException(resultError), Constants.InvoiceListError);

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    lbInvoice.SelectionChanged -= lbInvoice_SelectionChanged;
                    lbInvoice.SelectedIndex = -1;
                    lbInvoice.SelectionChanged += lbInvoice_SelectionChanged;
                });
                if (resultError[ 0 ] != null && T360ErrorCodes.NotInReviewerQueue == resultError[ 0 ].Code)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        LoadInvoiceList();
                    });
                }
            }
        }

        private bool IsInMultiSelect { get; set; }
        private bool IsInSearch { get; set; }
        private bool IsApprove { get; set; }

        private InvoiceConfirmationDetails ApproveConfirmation { get; set; }
        private InvoiceSummary InvoiceSummaryDetails { get; set; }

        private bool HasPreparedAppBar { get; set; }
        private bool CanPrepareAppBar { get; set; }

        public InvoiceListingModel InvoiceListModel { get; set; }
    }
}