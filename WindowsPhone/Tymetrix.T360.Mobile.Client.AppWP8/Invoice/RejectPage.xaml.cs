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

namespace Tymetrix.T360.Mobile.Client.AppWP8.Invoice
{
    public partial class RejectPage : BasePage
    {
        public List<string> invoiceLineItems { get; set; }
        public object[] InvoiceApproveErrorInput { get; set; }
        public InvoiceErrorDetails InvoiceErrorDetails { get; set; }
        private const string InvoiceReasonsError = "Invoice Reasons Failed";
        private const string LineItemReasonError = "Line Item Reasons Failed";
        public List<ReasonCode> Reasons { get; set; }
        public InvoiceSummary InvoiceDetails { get; set; }
        private UserData userdata;
        public LineItemConfirmationDetails LineItemRejectDetails { get; set; }
        public InvoiceConfirmationDetails InvoiceInputDetails { get; set; }

        public RejectPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Source == Source.EXTERNAL)
            {
                this.Source = this.PreviousSource;
                return;
            }
            userdata = UserData.Instance;
            ApplicationBar = new ApplicationBar();
            PrePopulate();
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
            if (Source == Model.Base.Source.LINE_ITEM_MULTI_REJECT_CONFIRMATION && 
                            !PhoneApplicationService.Current.State.ContainsKey("isFromActivated"))
            {
                NavigationService.RemoveBackEntry();
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
            if (userdata.IsMultipleInvoiceReject) userdata.IsMultipleInvoiceReject = false;
            InvoiceApproveError approveError = e.Content as InvoiceApproveError;
            if (approveError != null)
            {
                approveError.Source = Source;
                approveError.InvoiceDetails = InvoiceErrorDetails;
                approveError.InvoiceInputDetails = InvoiceDetails;
                approveError.LineItemInputDetails = LineItemRejectDetails;
                NavigationService.RemoveBackEntry();
            }
            InvoiceCommonDetails commonDetailsPage = e.Content as InvoiceCommonDetails;
            if (commonDetailsPage != null)
            {
                if (Source == Model.Base.Source.INVOICE_SINGLE_REJECT)
                {
                    commonDetailsPage.Source = Model.Base.Source.BACK_TO_INVOICE_SUMMARY;
                    commonDetailsPage.InvoiceSummaryDetails = InvoiceDetails;
                }
                else if (Source == Model.Base.Source.LINE_ITEM_MULTI_REJECT_CONFIRMATION)
                {
                    commonDetailsPage.Source = Model.Base.Source.BACK_TO_LINE_ITEM_LIST;
                    commonDetailsPage.MultiConfirmation = LineItemRejectDetails;
                }
            }
            LineItemSummary lineItemSummary = e.Content as LineItemSummary;
            if (lineItemSummary != null)
            {
                lineItemSummary.Source = Source.BACK_TO_LINE_ITEM_SUMMARY;
                lineItemSummary.SelectedLineItemId = LineItemRejectDetails.SelectedLineItemIds[0];
            }
            base.OnNavigatedFrom(e);
        }
        private void PopulateHeaders()
        {
            if (Source == Model.Base.Source.INVOICE_SINGLE_REJECT)
            {
                invoiceHeader.SetHeaderDetails(InvoiceDetails);
                netAmountTextBlock.Text = InvoiceDetails.NetAmount;
            }
            else if (Source == Model.Base.Source.INVOICE_MULTI_REJECT_CONFIRMATION)
            {
                netAmountTextBlock.Text = InvoiceInputDetails.NetTotal;
                invoiceHeader.Visibility = Visibility.Collapsed;
            }
            else if (Source == Model.Base.Source.LINE_ITEM_SINGLE_REJECT)
            {
                invoiceHeader.SetHeaderDetails(LineItemRejectDetails.InvoiceDetails);
                netAmountTextBlock.Text = LineItemRejectDetails.NetAmount;
            }
            else if (Source == Model.Base.Source.LINE_ITEM_MULTI_REJECT_CONFIRMATION)
            {
                invoiceHeader.SetHeaderDetails(LineItemRejectDetails.InvoiceDetails);
                netAmountTextBlock.Text = LineItemRejectDetails.NetAmount;
            }
        }

        private void PrePopulate()
        {
            this.ProgressBar.Show();
            PageInProgress = true;
            DisableApplicationBar();
            ShowRejectAppBar();
            PopulateHeaders();
            LoadReasons();
            this.ProgressBar.Hide();
            PageInProgress = false;
            EnableApplicationBar();
        }

        private void ShowRejectAppBar()
        {
            RemoveApplicationBarButtons();
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = false;

            ApplicationBarIconButton submitButton = new ApplicationBarIconButton(new Uri("Resources\\images\\Approve.png", UriKind.Relative));
            submitButton.Text = "Submit";
            submitButton.Click += submitAppBar_Click;
            ApplicationBar.Buttons.Add(submitButton);

            ApplicationBarIconButton cancelButton = new ApplicationBarIconButton(new Uri("Resources\\images\\Reject.png", UriKind.Relative));
            cancelButton.Text = "Cancel";
            cancelButton.Click += cancelAppBar_Click;
            ApplicationBar.Buttons.Add(cancelButton);
        }

        private void LoadReasons()
        {
            PopulateReasons();
        }

        private void PopulateReasons()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                this.reasonPicker.ItemsSource = Reasons;
            });
        }

        private RejectInput GetData()
        {
            ReasonCode reason;
            InvoiceBasicInfo invoiceSummary = null;
            RejectInputDetails rejectInputDetails = new RejectInputDetails();
            if (PhoneApplicationService.Current.State.ContainsKey(SelectedInvoice))
            {
                invoiceSummary = (InvoiceBasicInfo)PhoneApplicationService.Current.State[SelectedInvoice];
            }
            string invoiceId = string.Empty;

            if (Source == Model.Base.Source.INVOICE_SINGLE_REJECT)
            {
                invoiceId = InvoiceDetails.InvoiceId.ToString();
            }

            if (Source == Model.Base.Source.LINE_ITEM_SINGLE_REJECT)
            {
                reason = reasonPicker.SelectedItem as ReasonCode;
                //LineItemInputDetails selectedLineItem = new LineItemInputDetails((string)PhoneApplicationService.Current.State[SelectedInvoiceId], (string)PhoneApplicationService.Current.State[SelectedLineItemId]);
                //invoiceSummary.LineItemId = selectedLineItem.LineItemId;
                rejectInputDetails.InvoiceId = LineItemRejectDetails.InvoiceDetails.InvoiceId.ToString();
                rejectInputDetails.lstLineItemId = new List<string>();
                rejectInputDetails.lstLineItemId = LineItemRejectDetails.SelectedLineItemIds;
                rejectInputDetails.ReasonId = reason.Id.ToString();
                rejectInputDetails.NarrativeText = narrativeTextBox.Text.Trim();
                rejectInputDetails.IsMultiSelectEnabled = false;
                
            }
            else if(Source == Model.Base.Source.LINE_ITEM_MULTI_REJECT_CONFIRMATION)
            {
                reason = reasonPicker.SelectedItem as ReasonCode;
                rejectInputDetails.InvoiceId = LineItemRejectDetails.InvoiceDetails.InvoiceId.ToString();
                rejectInputDetails.lstLineItemId = new List<string>();
                rejectInputDetails.lstLineItemId = LineItemRejectDetails.SelectedLineItemIds;
                rejectInputDetails.ReasonId = reason.Id.ToString();
                rejectInputDetails.NarrativeText = narrativeTextBox.Text.Trim();
                rejectInputDetails.IsMultiSelectEnabled = true;
            }
            else if (Source == Model.Base.Source.INVOICE_MULTI_REJECT_CONFIRMATION)
            {
                RejectInputMultipleInvoice rejectInputMultiInvoice;
                if (reasonPicker.Items.Count == 0)
                {
                    rejectInputMultiInvoice = new RejectInputMultipleInvoice() { SelectedInvoiceIds = InvoiceInputDetails.SelectedInvoiceIds, ReasonId = string.Empty, NarrativeText = narrativeTextBox.Text.Trim() };
                }
                else
                {
                    reason = reasonPicker.SelectedItem as ReasonCode;
                    rejectInputMultiInvoice = new RejectInputMultipleInvoice() { SelectedInvoiceIds = InvoiceInputDetails.SelectedInvoiceIds, ReasonId = reason.Id.ToString(), NarrativeText = narrativeTextBox.Text.Trim() };
                }
                return rejectInputMultiInvoice;
            }
            else
            {
                if (reasonPicker.Items.Count == 0)
                {
                    rejectInputDetails = new RejectInputDetails() { InvoiceId = InvoiceDetails.InvoiceId.ToString(), ReasonId = string.Empty, NarrativeText = narrativeTextBox.Text.Trim() };
                }
                else
                {
                    reason = reasonPicker.SelectedItem as ReasonCode;
                    rejectInputDetails = new RejectInputDetails() { InvoiceId = InvoiceDetails.InvoiceId.ToString(), ReasonId = reason.Id.ToString(), NarrativeText = narrativeTextBox.Text.Trim() };
                }
            }
            return rejectInputDetails;
        }

        private void RejectSelectedItem(RejectInput rejectInputDetails)
        {
            BaseValidator validator = new BaseValidator();
            MessageBoxResult msgResult;
            bool isValid = Source == Model.Base.Source.INVOICE_MULTI_REJECT_CONFIRMATION
                ? validator.RejectInvoice((RejectInputMultipleInvoice)rejectInputDetails)
                : validator.RejectInvoice((RejectInputDetails)rejectInputDetails);

            if (!isValid)
            {
                if (Source == Model.Base.Source.LINE_ITEM_SINGLE_REJECT || Source == Model.Base.Source.LINE_ITEM_MULTI_REJECT_CONFIRMATION)
                {
                    ShowError(validator.ClientException, Constants.RejectLineItemError);
                }
                else
                {
                    ShowError(validator.ClientException, Constants.RejectInvoiceError);
                }
                return;
            }
            if (Source == Model.Base.Source.LINE_ITEM_SINGLE_REJECT || Source == Model.Base.Source.LINE_ITEM_MULTI_REJECT_CONFIRMATION)
            {
                string message = LineItemRejectDetails.SelectedLineItemIds.Count == 1
                                   ? "Are you sure you want to reject this line item?"
                                   : "Are you sure you want to reject these line items?";

                msgResult = MessageBox.Show(message, "Rejection Confirmation", MessageBoxButton.OKCancel);
            }
            else if (Source == Model.Base.Source.INVOICE_MULTI_REJECT_CONFIRMATION)
            {
                string message = InvoiceInputDetails.SelectedInvoiceIds.Count == 1
                                    ? "Are you sure you want to reject this invoice?"
                                    : "Are you sure you want to reject these invoices?";

                msgResult = MessageBox.Show(message, "Rejection Confirmation", MessageBoxButton.OKCancel);
            }
            else
            {
                msgResult = MessageBox.Show("Are you sure you want to reject this invoice?", "Rejection Confirmation", MessageBoxButton.OKCancel);
            }

            if (msgResult == MessageBoxResult.Cancel) return;

            string postData = JsonConvert.SerializeObject(rejectInputDetails);
            if (Source == Source.LINE_ITEM_MULTI_REJECT_CONFIRMATION || Source == Source.LINE_ITEM_SINGLE_REJECT)
            {
                RejectSelectedLineItem(rejectInputDetails, postData);
            }
            try
            {
                if (Source == Source.INVOICE_MULTI_REJECT_CONFIRMATION || Source == Source.INVOICE_SINGLE_REJECT)
                {
                    RejectSelectedInvoice(rejectInputDetails, postData);
                }
            }
            catch (Exception ex)
            {
                ShowError((AppException)ex);
            }
        }

        private void RejectSelectedInvoice(RejectInput rejectInputDetails, string postData)
        {
            try
            {
                if (Source == Model.Base.Source.INVOICE_MULTI_REJECT_CONFIRMATION)
                {
                    ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/RejectMultipleInvoice", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
                    {
                        ServiceResponse result = serviceEventArgs.Result;
                        if (result.Status)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                RedirectToInvoiceList();
                            });
                        }
                        else
                        {
                            handleRejectError(result.ErrorDetails, Constants.RejectInvoiceError);
                        }
                    });
                }
                else if (Source == Model.Base.Source.INVOICE_SINGLE_REJECT)
                {
                    ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/RejectInvoice", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
                    {
                        ServiceResponse result = serviceEventArgs.Result;
                        if (result.Status)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                NavigationService.RemoveBackEntry();
                                NavigationService.GoBack();
                            });
                        }
                        else
                        {
                            List<Error> resultError = result.ErrorDetails;
                            ShowError(new AppException(resultError), Constants.RejectInvoiceError);
                            if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                            {
                                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    RedirectToInvoiceList();
                                });
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                ShowError((AppException)ex);
            }
        }

        private void RejectSelectedLineItem(RejectInput rejectInputDetails, string postData)
        {
            try
            {
                if (Source == Model.Base.Source.LINE_ITEM_SINGLE_REJECT || Source == Model.Base.Source.LINE_ITEM_MULTI_REJECT_CONFIRMATION)
                {
                    ServiceInvoker.InvokeServiceUsingPost("api/t360/LineItem/RejectLineItems", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
                    {
                        ServiceResponse result = serviceEventArgs.Result;
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
                            ShowError(new AppException(resultError), Constants.RejectLineItemError);
                            if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                            {
                                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    RedirectToInvoiceList();
                                });
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                ShowError((AppException)ex);
            }
        }

        private void handleRejectError(List<Error> errorList, string title)
        {
            if (Constants.Failure.ToUpper().Equals(errorList[0].Code.ToUpper()))
            {
                InvoiceErrorDetails = new Model.Invoice.InvoiceErrorDetails()
                {
                    PageType = Constants.Failure,
                    ErrorDetails = errorList,
                    Header = "Reject Invoices"
                };
                //InvoiceApproveErrorInput = new object[3];
                //InvoiceApproveErrorInput[0] = Constants.Failure;
                //InvoiceApproveErrorInput[1] = errorList;
                //InvoiceApproveErrorInput[2] = "Reject Invoices";
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.ProgressBar.Show();
                    PageInProgress = true;
                    Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Invoice/InvoiceApproveError.xaml", UriKind.Relative);
                    NavigationService.Navigate(uri);
                    this.ProgressBar.Hide();
                });
            }
            else
            {
                ShowError(new AppException(errorList), title);
            }
            if (errorList[0] != null && T360ErrorCodes.NotInReviewerQueue == errorList[0].Code)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.ProgressBar.Hide();
                    PageInProgress = false;
                    RedirectToInvoiceList();
                });
            }
        }

        private void GoBack()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                NavigationService.GoBack();
            });
        }

        private void submitAppBar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ServiceInvoker.IsConnected)
                {
                    throw new AppException(T360ErrorCodes.UnableToConnectServer);
                }
                this.RejectSelectedItem(GetData());
            }
            catch (Exception ex)
            {
                ShowError((AppException)ex);
            }
        }

        private void cancelAppBar_Click(object sender, EventArgs e)
        {
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }
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
     }
}