/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Windows;
using System.Windows.Navigation;

using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.Invoice;

namespace Tymetrix.T360.Mobile.Client.AppWP7.Invoice
{
    public partial class InvoiceApproveError : BasePage
    {
        private const string ApproveError = "Approve Failed";
        public InvoiceErrorDetails InvoiceDetails { get; set; }
        public InvoiceSummary InvoiceInputDetails { get; set; }
        public LineItemConfirmationDetails LineItemInputDetails { get; set; }

        public InvoiceApproveError()
        {
            InitializeComponent();
            this.Loaded += InvoiceApproveError_Loaded;
        }

        void InvoiceApproveError_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Source == Source.EXTERNAL)
            {
                this.Source = this.PreviousSource;
                return;
            }
            PageInProgress = false;
        }

        protected override void OnOrientationChanged(Microsoft.Phone.Controls.OrientationChangedEventArgs e)
        {
            if (e.Orientation == PageOrientation.Landscape || e.Orientation == PageOrientation.LandscapeLeft || e.Orientation == PageOrientation.LandscapeRight)
            {
                LayoutRoot.RowDefinitions[0].Height = new GridLength(2.5, GridUnitType.Star);
                LayoutRoot.RowDefinitions[1].Height = new GridLength(7.5, GridUnitType.Star);
            }
            else
            {
                LayoutRoot.RowDefinitions[0].Height = new GridLength(2, GridUnitType.Star);
                LayoutRoot.RowDefinitions[1].Height = new GridLength(8, GridUnitType.Star);
            }
            base.OnOrientationChanged(e);
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
            ApproveError invConfirmation = new ApproveError();
            string str = InvoiceDetails.PageType;
            List<Error> resultError = InvoiceDetails.ErrorDetails;
            invConfirmation.ApproveErrorTitle = IsDisallow(str)
                ? Constants.DisallowHeader
                : IsWarning(str) ? Constants.WarningHeader
                                 : "Failed (" + GetFailedCount(resultError) + ")";
            invConfirmation.ApproveErrorItems = GetApproveErrorItems(InvoiceDetails.ErrorDetails);
            invConfirmation.Header = InvoiceDetails.Header;

            this.DataContext = invConfirmation;
            PrepareApplicationBar();
            base.OnNavigatedTo(e);
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (Constants.ExternalURI.Equals(e.Uri.ToString()))
            {
                this.PreviousSource = this.Source;
                this.Source = Source.EXTERNAL;
                return;
            }
            InvoiceCommonDetails commonPage = e.Content as InvoiceCommonDetails;
            if (commonPage != null)
            {
                if (Source == Source.LINE_ITEM_MULTI_REJECT_CONFIRMATION)
                {
                    commonPage.Source = Source.BACK_TO_LINE_ITEM_LIST;
                    commonPage.MultiConfirmation = LineItemInputDetails;
                }
                else if (Source == Source.INVOICE_SINGLE_REJECT)
                {
                    commonPage.Source = Source.BACK_TO_INVOICE_SUMMARY;
                    commonPage.InvoiceSummaryDetails = InvoiceInputDetails;
                }
            }
        }

        private bool IsDisallow(string input) { return Constants.Disallow.Equals(input); }
        private bool IsWarning(string input) { return Constants.Warning.Equals(input); }
        private bool IsFailure(string input) { return Constants.Failure.Equals(input); }

        private void PrepareApplicationBar()
        {
            if (ApplicationBar != null)
            {
                RemoveApplicationBarButtons();
            }
            else
            {
                ApplicationBar = new ApplicationBar();
                ApplicationBar.Opacity = 1;
            }

            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = false;

            ApplicationBarIconButton doneButton = new ApplicationBarIconButton(new Uri("Resources\\images\\RightArrow.png", UriKind.Relative));
            doneButton.Text = Constants.Proceed;
            doneButton.Click += doneButton_Click;
            ApplicationBar.Buttons.Add(doneButton);
            if (Constants.Warning.ToUpper().Equals((InvoiceDetails.PageType.ToUpper()))) // (string)Input[0]).ToUpper()))
            {
                ApplicationBarIconButton cancelButton = new ApplicationBarIconButton(new Uri("Resources\\images\\Reject.png", UriKind.Relative));
                cancelButton.Text = Constants.Cancel;
                cancelButton.Click += CancelAppBar_Click;
                ApplicationBar.Buttons.Add(cancelButton);
            }
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Constants.Warning.ToUpper().Equals((InvoiceDetails.PageType.ToUpper())))
                {
                    ReturnToInvoiceListPage();
                    return;
                }
                if (!ServiceInvoker.IsConnected)
                {
                    ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                    return;
                }
                List<InvoiceModel> selectedInvoices = InvoiceDetails.InvoiceBasicDetails;

                List<string> selectedIds = new List<string>();
                selectedInvoices.ForEach(x => { selectedIds.Add(x.InvoiceId.ToString()); });

                Dictionary<string, object> selectedInvoiceIds = new Dictionary<string, object>();
                selectedInvoiceIds.Add(Constants.SelectedInvoiceIds, selectedIds);
                selectedInvoiceIds.Add(Constants.ForceApprove, true);

                ServiceInvoker.InvokeServiceUsingPost("api/t360/Invoice/ApproveMultipleInvoice", JsonConvert.SerializeObject(selectedInvoiceIds), false, InvoiceMultiApproveHandler);
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

        private void InvoiceMultiApproveHandler(object sender, ServiceEventArgs se)
        {
            ServiceResponse result = se.Result;
            if (result.Status)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.GoBack();
                });
            }
            else
            {
                List<Error> resultError = result.ErrorDetails;
                ShowError(new AppException(resultError), ApproveError);
                if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        RedirectToInvoiceList();
                    });
                }
            }
        }

        private void ReturnToInvoiceListPage()
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
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
            base.OnBackKeyPress(e);
        }

        private void CancelAppBar_Click(object sender, EventArgs e)
        {
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }
            ReturnToInvoiceListPage();
        }

        private List<ApproveErrorItem> GetApproveErrorItems(List<Error> errors)
        {
            List<ApproveErrorItem> items = new List<ApproveErrorItem>();
            ApproveErrorItem item;
            bool isFailure = Constants.Failure.ToUpper().Equals(errors[0].Code.ToUpper());
            ResourceManager rm = CultureManager.Instance.GetCulture(CultureType.Message.ToString());
            for (int i = 1; i < errors.Count; i++)
            {
                item = new ApproveErrorItem();
                string msg = rm.GetString(errors[i].Code);
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
    }
}