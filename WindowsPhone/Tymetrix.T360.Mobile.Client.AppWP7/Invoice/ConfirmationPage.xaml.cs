/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;

using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.Invoice;

namespace Tymetrix.T360.Mobile.Client.AppWP7.Invoice
{
    public partial class ConfirmationPage : BasePage
    {
        private const string ConfirmationError = "Confirmation Failed";
        private const string InvoiceReasonsError = "Invoice Reasons Failed";

        public InvoiceConfirmationDetails InvoiceInputDetails { get; set; }
        private InvoiceErrorDetails InvoiceErrorDetails { get; set; }
        public LineItemConfirmationDetails LineItemInputDetails { get; set; }
        private bool HasPrepared { get; set; }
        
        public ConfirmationPage()
        {
            InitializeComponent();
            this.Loaded += ConfirmationPage_Loaded;
        }

        void ConfirmationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Source == Source.EXTERNAL)
            {
                this.Source = this.PreviousSource;
                return;
            }
            if (!HasPrepared)
            {
                PrepareApplicationBar();
                HasPrepared = true;
            }
            PageInProgress = false;
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
            if (Source == Model.Base.Source.LINE_ITEM_MULTI_ADJUST || Source == Model.Base.Source.LINE_ITEM_MULTI_REJECT)
            {
                InvoiceConfirmation invConfirmation = new InvoiceConfirmation();
                invConfirmation.ConfirmationItems = GetLineItemConfirmationItems(LineItemInputDetails.SelectedLineItems);
                invConfirmation.TotalNetAmount = LineItemInputDetails.NetAmount;
                invConfirmation.ConfirmationTitle = Source == Model.Base.Source.LINE_ITEM_MULTI_ADJUST
                    ? "Adjust (" + invConfirmation.ConfirmationItems.Count + ")"
                    : "Reject (" + invConfirmation.ConfirmationItems.Count + ")";

                this.DataContext = invConfirmation;
            }
            else if (Source == Model.Base.Source.INVOICE_MULTI_APPROVE || Source == Model.Base.Source.INVOICE_MULTI_REJECT)
            {
                InvoiceConfirmation invConfirmation = new InvoiceConfirmation();
                invConfirmation.ConfirmationItems = GetConfirmationItems(InvoiceInputDetails.SelectedInvoices);
                invConfirmation.TotalNetAmount = InvoiceInputDetails.NetTotal;
                invConfirmation.ConfirmationTitle = Source == Model.Base.Source.INVOICE_MULTI_APPROVE
                    ? "Approve (" + invConfirmation.ConfirmationItems.Count + ")"
                    : "Reject (" + invConfirmation.ConfirmationItems.Count + ")";

                this.DataContext = invConfirmation;
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
            RejectPage rejectPage = e.Content as RejectPage;
            if (Source == Model.Base.Source.INVOICE_MULTI_APPROVE)
            {
                InvoiceApproveError approveError = e.Content as InvoiceApproveError;
                if (approveError != null)
                {
                    approveError.InvoiceDetails = InvoiceErrorDetails;
                    NavigationService.RemoveBackEntry();
                }
            }
            else if (Source == Model.Base.Source.INVOICE_MULTI_REJECT)
            {
                if (rejectPage != null)
                {
                    rejectPage.Source = Model.Base.Source.INVOICE_MULTI_REJECT_CONFIRMATION;
                    rejectPage.InvoiceInputDetails = InvoiceInputDetails;
                    rejectPage.Reasons = InvoiceInputDetails.Reasons;
                    NavigationService.RemoveBackEntry();
                }
            }
            else if (Source == Model.Base.Source.LINE_ITEM_MULTI_ADJUST)
            {
                AdjustPage adjustPage = e.Content as AdjustPage;
                if (adjustPage != null)
                {
                    adjustPage.Source = Model.Base.Source.LINE_ITEM_MULTI_ADJUST_CONFIRMATION;
                    adjustPage.LineItemAdjustDetails = LineItemInputDetails;
                    adjustPage.Reasons = LineItemInputDetails.Reasons;
                    NavigationService.RemoveBackEntry();
                }
            }
            else if (Source == Model.Base.Source.LINE_ITEM_MULTI_REJECT)
            {
                if (rejectPage != null)
                {
                    rejectPage.Source = Model.Base.Source.LINE_ITEM_MULTI_REJECT_CONFIRMATION;
                    rejectPage.LineItemRejectDetails = LineItemInputDetails;
                    rejectPage.Reasons = LineItemInputDetails.Reasons;
                }
            }
            InvoiceCommonDetails commonDetailsPage = e.Content as InvoiceCommonDetails;
            if (commonDetailsPage != null)
            {
                if (Source == Model.Base.Source.LINE_ITEM_MULTI_ADJUST || Source == Model.Base.Source.LINE_ITEM_MULTI_REJECT)
                {
                    commonDetailsPage.Source = Model.Base.Source.BACK_TO_LINE_ITEM_LIST;
                    commonDetailsPage.MultiConfirmation = LineItemInputDetails;
                }

            }
            base.OnNavigatedFrom(e);
        }

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

            ApplicationBarIconButton cancelButton = new ApplicationBarIconButton(new Uri("Resources\\images\\Reject.png", UriKind.Relative));
            cancelButton.Text = Constants.Cancel;
            cancelButton.Click += CancelAppBar_Click;
            ApplicationBar.Buttons.Add(cancelButton);
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            DisableApplicationBar();
            try
            {
                if (!ServiceInvoker.IsConnected)
                {
                    ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                    EnableApplicationBar();
                    return;
                }
                if (Source == Model.Base.Source.LINE_ITEM_MULTI_ADJUST)
                {
                    GotoLineItemAdjustPage();
                }
                else if (Source == Model.Base.Source.LINE_ITEM_MULTI_REJECT)
                {
                    GotoLineItemRejectPage();
                }
                else if (Source == Model.Base.Source.INVOICE_MULTI_APPROVE)
                {
                    string message = InvoiceInputDetails.SelectedInvoices.Count == 1
                                        ? "Are you sure you want to approve this invoice?"
                                        :"Are you sure you want to approve these invoices?";

                    MessageBoxResult msgResult = MessageBox.Show(message, "Approve Confirmation", MessageBoxButton.OKCancel);
                    if (msgResult == MessageBoxResult.Cancel)
                    {
                        EnableApplicationBar();
                        return;
                    }

                    Dictionary<string, object> selectedInvoiceIds = new Dictionary<string, object>();
                    selectedInvoiceIds.Add(Constants.SelectedInvoiceIds, GetSelectedIds(InvoiceInputDetails.SelectedInvoices));
                    selectedInvoiceIds.Add(Constants.ForceApprove, false);

                    ServiceInvoker.InvokeServiceUsingPost("api/t360/Invoice/ApproveMultipleInvoice", JsonConvert.SerializeObject(selectedInvoiceIds), false, InvoiceMultiApproveHandler);
                }
                else if (Source == Model.Base.Source.INVOICE_MULTI_REJECT)
                {
                    InvoiceInputDetails.SelectedInvoiceIds = GetSelectedIds(InvoiceInputDetails.SelectedInvoices);
                    InvoiceInputDetails.NetTotal = InvoiceInputDetails.NetTotal;
                    UserData.Instance.IsMultipleInvoiceReject = true;

                    GotoRejectPage();
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
                EnableApplicationBar();
            }
        }

        private void GotoLineItemRejectPage()
        {
            string postData = JsonConvert.SerializeObject(new ReasonCodeMultipleInput()
            {
                Action = "RejectLineItem",
                InvoiceId = LineItemInputDetails.InvoiceId,
                SelectedLineItemIds = LineItemInputDetails.SelectedLineItemIds
            });
            try
            {
                ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/GetReasonCodes", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
                {
                    ServiceResponse result = serviceEventArgs.Result;
                    if (result.Status)
                    {
                        LineItemInputDetails.Reasons = JsonConvert.DeserializeObject<List<ReasonCode>>(result.Output);
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            this.ProgressBar.Show();
                            PageInProgress = true;
                            string url = string.Format("/Tymetrix.T360.Mobile.Client.AppWP7;component/Invoice/RejectPage.xaml"); //?MultipleItems={0}", rejectMultipleitems);
                            NavigationService.Navigate(new Uri(url, UriKind.Relative));
                            this.ProgressBar.Hide();
                        });
                    }
                    else
                    {
                        List<Error> resultError = result.ErrorDetails;
                        ShowError(new AppException(resultError), ConfirmationError);
                        if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                this.ProgressBar.Hide();
                                PageInProgress = false;
                                RedirectToInvoiceList();
                            });
                        }
                        else
                        {
                            EnableApplicationBar();
                        }
                    }
                    PageInProgress = false;
                });
            }
            catch (Exception ex)
            {
                ShowError((AppException)ex);
                EnableApplicationBar();
            }
        }

        private void GotoLineItemAdjustPage()
        {
            string postData = JsonConvert.SerializeObject(new ReasonCodeMultipleInput()
            {
                Action = "AdjustSelectedLineItems",
                InvoiceId = LineItemInputDetails.InvoiceId,
                SelectedLineItemIds = LineItemInputDetails.SelectedLineItemIds
            });
            try
            {
                ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/GetReasonCodes", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
                {
                    ServiceResponse result = serviceEventArgs.Result;
                    if (result.Status)
                    {
                        LineItemInputDetails.Reasons = JsonConvert.DeserializeObject<List<ReasonCode>>(result.Output);
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            this.ProgressBar.Show();
                            PageInProgress = true;
                            string url = string.Format("/Tymetrix.T360.Mobile.Client.AppWP7;component/Invoice/AdjustPage.xaml");
                            NavigationService.Navigate(new Uri(url, UriKind.Relative));
                            this.ProgressBar.Hide();
                        });
                    }
                    else
                    {
                        List<Error> resultError = result.ErrorDetails;
                        ShowError(new AppException(resultError), ConfirmationError);
                        if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                this.ProgressBar.Hide();
                                PageInProgress = false;
                                RedirectToInvoiceList();
                            });
                        }
                        else
                        {
                            EnableApplicationBar();
                        }
                    }
                    PageInProgress = false;
                });
            }
            catch (Exception ex)
            {
                ShowError((AppException)ex);
                EnableApplicationBar();
            }
        }

        private void GotoRejectPage()
        {
            string postData = JsonConvert.SerializeObject(new ReasonCodeInputDetails() { Action = "Reject" });
            try
            {
                ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/GetReasonCodes", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
                {
                    ServiceResponse result = serviceEventArgs.Result;
                    if (result.Status)
                    {
                        InvoiceInputDetails.Reasons = JsonConvert.DeserializeObject<List<ReasonCode>>(result.Output);
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            this.ProgressBar.Show();
                            PageInProgress = true;
                            Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP7;component/Invoice/RejectPage.xaml", UriKind.Relative);
                            this.NavigationService.Navigate(uri);
                            this.ProgressBar.Hide();
                        });
                    }
                    else
                    {
                        List<Error> resultError = result.ErrorDetails;
                        ShowError(new AppException(resultError), ConfirmationError);
                        if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                this.ProgressBar.Hide();
                                PageInProgress = false;
                                RedirectToInvoiceList();
                            });
                        }
                        else
                        {
                            EnableApplicationBar();
                        }
                    }
                    PageInProgress = false;
                });
            }
            catch (Exception ex)
            {
                ShowError((AppException)ex);
                EnableApplicationBar();
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

        private List<string> GetSelectedIds(List<InvoiceModel> selectedInvoices)
        {
            List<string> selectedIds = new List<string>();
            selectedInvoices.ForEach(x => { selectedIds.Add(x.InvoiceId.ToString()); });
            return selectedIds;
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
                Error error = resultError[0];
                if (Constants.Disallow.ToUpper().Equals(error.Code.ToUpper()))
                {
                    InvoiceErrorDetails = new Model.Invoice.InvoiceErrorDetails()
                    {
                        PageType = Constants.Disallow,
                        ErrorDetails = resultError,
                        Header = Source == Model.Base.Source.INVOICE_MULTI_APPROVE ? "Approve Invoices" : string.Empty
                    };
                }
                else if (Constants.Warning.ToUpper().Equals(error.Code.ToUpper()))
                {
                    InvoiceErrorDetails = new Model.Invoice.InvoiceErrorDetails()
                    {
                        PageType = Constants.Warning,
                        ErrorDetails = resultError,
                        Header = Source == Model.Base.Source.INVOICE_MULTI_APPROVE ? "Approve Invoices" : string.Empty,
                        InvoiceBasicDetails = InvoiceInputDetails.SelectedInvoices
                    };
                }
                else
                {
                    InvoiceErrorDetails = new Model.Invoice.InvoiceErrorDetails()
                    {
                        PageType = "Failed (" + resultError.Count + ")",
                        ErrorDetails = resultError,
                        Header = Source == Model.Base.Source.INVOICE_MULTI_APPROVE ? "Approve Invoices" : string.Empty
                    };
                }
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.ProgressBar.Show();
                    PageInProgress = true;
                    Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP7;component/Invoice/InvoiceApproveError.xaml", UriKind.Relative);
                    NavigationService.Navigate(uri);
                    this.ProgressBar.Hide();
                });
            }
        }

        private void CancelAppBar_Click(object sender, EventArgs e)
        {
            ManageAppBarVisible(false);
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                ManageAppBarVisible(true);
                return;
            }
            NavigationService.GoBack();
            //this.ProgressBar.Show();
        }

        private List<ConfirmationItem> GetConfirmationItems(List<InvoiceModel> input)
        {
            List<ConfirmationItem> items = new List<ConfirmationItem>();
            ConfirmationItem item;
            foreach (InvoiceModel info in input)
            {
                item = new ConfirmationItem();
                item.LeftText = "Inv #" + info.InvoiceNumber;
                item.RightText = info.NetAmount;
                items.Add(item);
            }
            return items;
        }

        private List<ConfirmationItem> GetLineItemConfirmationItems(List<LineItem> input)
        {
            List<ConfirmationItem> items = new List<ConfirmationItem>();
            ConfirmationItem item;
            foreach (LineItem info in input)
            {
                item = new ConfirmationItem();
                item.LeftText = info.TimeKeeper;
                item.RightText = info.NetTotal;
                items.Add(item);
            }
            return items;
        }
    }
}