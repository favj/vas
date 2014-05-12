using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using System.Linq;

using Newtonsoft.Json;
using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.Invoice;

namespace Tymetrix.T360.Mobile.Client.AppWP8.Invoice
{
    public partial class LineItemConfirmationPage : BasePage
    {
        private List<InvoiceLineItemsInfo> lstLineItems;
        private const string LineItemReasonError = "Line Item Reasons Failed";
        private const string InvoiceReasonsError = "Invoice Reasons Failed";
        private MultipleLineItemsInputDetails rejectMultiple;
        private UserData userdata;
        private List<ReasonCode> RejectInput { get; set; }
        private List<ReasonCode> AdjustInput { get; set; }

        public LineItemConfirmationPage()
        {
            InitializeComponent();
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            userdata = UserData.Instance;
            PrePopulate();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (PhoneApplicationService.Current.State.ContainsKey("isFromActivated") && PageInProgress)
            {
                PhoneApplicationService.Current.State.Remove("isFromActivated");
                PhoneApplicationPage_Loaded(null, null);
            }
            
            string lineitem = string.Empty;
            if (NavigationContext.QueryString.ContainsKey("lineitems"))
            {
                lineitem = NavigationContext.QueryString["lineitems"];
            }
            lstLineItems = JsonConvert.DeserializeObject<List<InvoiceLineItemsInfo>>(lineitem);
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            RejectPage rejectNote = e.Content as RejectPage;
            if (rejectNote != null)
            {
                rejectNote.Reasons = RejectInput;
                NavigationService.RemoveBackEntry();
            }
            AdjustLineItem adjustLineItem = e.Content as AdjustLineItem;
            if (adjustLineItem != null)
            {
                adjustLineItem.Reasons = AdjustInput;
                NavigationService.RemoveBackEntry();
            }
            base.OnNavigatedFrom(e);
        }

        private void PopulateHeaders()
        {
            InvoiceBasicInfo invoiceHeaderDetails = (InvoiceBasicInfo)PhoneApplicationService.Current.State[SelectedInvoice];
            invoiceHeader.HeaderInfo = invoiceHeaderDetails;
        }

        private void PopulateRejectionData(MultipleLineItemsInputDetails rejectMultipleitems)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                lineitemMultipleReject.ItemsSource = lstLineItems;
                textNetAmount.Text = rejectMultipleitems.NetAmount;
                rejectMultiple = rejectMultipleitems;
            });
        }

        private void PrePopulate()
        {
            this.ProgressBar.Show();
            ApplicationBar = new ApplicationBar();
            rejectMultiple = new MultipleLineItemsInputDetails((string)PhoneApplicationService.Current.State[SelectedInvoiceId]);
            if (userdata.IsMultipleLineItemReject)
            {
                textConfirmation.Text = "Reject (" + lstLineItems.Count + ")";
            }
            else if(userdata.IsMultipleLineItemAdjust)
            {
                textConfirmation.Text = "Adjust (" + lstLineItems.Count + ")";
            }
            PopulateHeaders();
            ShowConfirmationAppBar();
            LoadTotalNetAmount();
            this.ProgressBar.Hide();
        }

        private void ShowConfirmationAppBar()
        {
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = false;

            ApplicationBarIconButton actionButton = new ApplicationBarIconButton(new Uri("Resources\\images\\RightArrow.png", UriKind.Relative));
            actionButton.Text = "Proceed";
            actionButton.Click += ProccedAppBar_Click;
            ApplicationBar.Buttons.Add(actionButton);

            ApplicationBarIconButton cancelButton = new ApplicationBarIconButton(new Uri("Resources\\images\\Reject.png", UriKind.Relative));
            cancelButton.Text = "Cancel";
            cancelButton.Click += cancelAppBar_Click;
            ApplicationBar.Buttons.Add(cancelButton);
        }

        private void ProccedAppBar_Click(object sender, EventArgs e)
        {
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }
            if (userdata.IsMultipleLineItemReject)
            {
                GotoRejectPage();
            }
            else if (userdata.IsMultipleLineItemAdjust)
            {
                GotoAdjustPage();
            }
        }

        private void GotoRejectPage()
        {
            string postData = JsonConvert.SerializeObject(new ReasonCodeMultipleInput()
                                                            {
                                                                Action = "RejectLineItem",
                                                                InvoiceId = rejectMultiple.InvoiceId,
                                                                SelectedLineItemIds = rejectMultiple.SelectedLineItemIds
                                                            });
            try
            {
                ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/GetReasonCodes", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
                {
                    ServiceResponse result = serviceEventArgs.Result;
                    if (result.Status)
                    {
                        RejectInput = JsonConvert.DeserializeObject<List<ReasonCode>>(result.Output);
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            string rejectMultipleitems = JsonConvert.SerializeObject(rejectMultiple);
                            string url = string.Format("/Tymetrix.T360.Mobile.Client.AppWP8;component/Invoice/RejectPage.xaml?MultipleItems={0}", rejectMultipleitems);
                            NavigationService.Navigate(new Uri(url, UriKind.Relative));
                        });
                    }
                    else
                    {
                        List<Error> resultError = result.ErrorDetails;
                        ShowError(new AppException(resultError), InvoiceReasonsError);
                        if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                RedirectToInvoiceList();
                            });
                        }
                    }
                    PageInProgress = false;
                });
            }
            catch (Exception ex)
            {
                ShowError((AppException)ex);
            }
        }

        private void GotoAdjustPage()
        {
            string postData = JsonConvert.SerializeObject(new ReasonCodeMultipleInput()
            {
                Action = "AdjustSelectedLineItems",
                InvoiceId = rejectMultiple.InvoiceId,
                SelectedLineItemIds = lstLineItems.Select<InvoiceLineItemsInfo, string>(delegate (InvoiceLineItemsInfo info) { return info.LineItemId; }).ToList()
            });
            try
            {
                ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/GetReasonCodes", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
                {
                    ServiceResponse result = serviceEventArgs.Result;
                    if (result.Status)
                    {
                        AdjustInput = JsonConvert.DeserializeObject<List<ReasonCode>>(result.Output);
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            string adjustMultipleLineitems = JsonConvert.SerializeObject(rejectMultiple);
                            string url = string.Format("/Tymetrix.T360.Mobile.Client.AppWP8;component/Invoice/AdjustLineItem.xaml?MultipleItems={0}", adjustMultipleLineitems);
                            NavigationService.Navigate(new Uri(url, UriKind.Relative));
                        });
                    }
                    else
                    {
                        List<Error> resultError = result.ErrorDetails;
                        ShowError(new AppException(resultError), InvoiceReasonsError);
                        if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                RedirectToInvoiceList();
                            });
                        }
                    }
                    PageInProgress = false;
                });
            }
            catch (Exception ex)
            {
                ShowError((AppException)ex);
            }
        }

        private void LoadTotalNetAmount()
        {
            InvoiceBasicInfo invoiceSummary = (InvoiceBasicInfo)PhoneApplicationService.Current.State[SelectedInvoice];
            rejectMultiple.InvoiceId = invoiceSummary.InvoiceId;
            rejectMultiple.SelectedLineItemIds = new List<string>();
            for (int i = 0; i < lstLineItems.Count; i++)
            {
                rejectMultiple.SelectedLineItemIds.Add(lstLineItems[i].LineItemId);
            }
            string postData = JsonConvert.SerializeObject(rejectMultiple);
            try
            {
                ServiceInvoker.InvokeServiceUsingPost("/api/t360/LineItem/CalculateLineItemNetAmount", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
                {
                    ServiceResponse result = serviceEventArgs.Result;
                    if (result.Status)
                    {
                        MultipleLineItemsInputDetails rejectMultipleitem = JsonConvert.DeserializeObject<MultipleLineItemsInputDetails>(result.Output);
                        rejectMultipleitem.InvoiceId = rejectMultiple.InvoiceId;
                        rejectMultipleitem.SelectedLineItemIds = rejectMultiple.SelectedLineItemIds;
                        PopulateRejectionData(rejectMultipleitem);
                    }
                    else
                    {
                        List<Error> resultError = result.ErrorDetails;
                        ShowError(new AppException(resultError), LineItemReasonError);
                        if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                RedirectToInvoiceList();
                            });
                        }
                    }
                    PageInProgress = false;
                });
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
    }
}