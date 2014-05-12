/*
 * Copyright © 2004 - 2012 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Tymetrix.T360.Mobile.Client.Common;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Model.Invoice;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Core;

namespace Tymetrix.T360.Mobile.Client.AppWP7.Invoice
{
    public partial class LineItemListing : BasePage
    {
        private List<string> invoiceLineItems { get; set; }
        private const string LineItemListError = "Line ItemList Failed";

        public LineItemListing()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            PrePopulate();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (PhoneApplicationService.Current.State.ContainsKey("isFromActivated") && PageInProgress)
            {
                PhoneApplicationService.Current.State.Remove("isFromActivated");
                PhoneApplicationPage_Loaded(null, null);
            }
            base.OnNavigatedTo(e);
        }

        private void PopulateHeaders()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                InvoiceBasicInfo invoiceHeaderDetails = (InvoiceBasicInfo)PhoneApplicationService.Current.State[SelectedInvoice];
                invoiceHeader.HeaderInfo = invoiceHeaderDetails;
            });
        }

        private void PrePopulate()
        {
            //DisableApplicationBar();
            this.ProgressBar.Show();
            PopulateHeaders();
            LoadLineItems();
        }

        private void LoadLineItems()
        {
            Uri u = new Uri("Resources/html/LineItemList.html", UriKind.Relative);
            lineItemsBrowser.Navigate(u);
        }

        private void lineItemsBrowser_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {

        }

        private void lineItemsBrowser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (e.Value.StartsWith("Completed"))
            {
                PageInProgress = false;
                this.ProgressBar.Hide();
                //EnableApplicationBar();
            }
            else if (e.Value.StartsWith(SelectedLineItemId) && !PageInProgress)
            {
                PageInProgress = true;
                if (!ServiceInvoker.IsConnected)
                {
                    ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                    return;
                }
                char[] delimiter = new char[] { '=' };
                string[] param = e.Value.Split(delimiter);
                PhoneApplicationService.Current.State[SelectedLineItemId] = param[1];
                Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP7;component/Invoice/LineItemSummary.xaml", UriKind.Relative);
                this.NavigationService.Navigate(uri);
            }
        }

        private void lineItemsBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                InvoiceInputDetails selectedInvoice = new InvoiceInputDetails((string)PhoneApplicationService.Current.State[SelectedInvoiceId]);
                this.GetLineItemsList(selectedInvoice);
            }
            catch (AppException ex)
            {
                ShowError(ex, LineItemListError);
            }
        }

        private void ApplicationBarSummary_Click(object sender, EventArgs e)
        {
            try
            {
                PageInProgress = true;
                if (!ServiceInvoker.IsConnected)
                {
                    ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                    return;
                }                
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.GoBack();
                });
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void GetLineItemsList(InvoiceInputDetails selectedInvoice)
        {
            string postData = DataSerializer.JsonSerializer<InvoiceInputDetails>(selectedInvoice);
            try
            {
                ServiceInvoker.InvokeServiceUsingPost("Invoice/InvoiceService.svc/GetLineItemList", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
                {
                    ServiceResponse result = serviceEventArgs.Result;
                    if (result.Status)
                    {
                        List<InvoiceBasicInfo> lineItems = DataSerializer.JsonDeserialize<List<InvoiceBasicInfo>>(result.Output);
                        this.invoiceLineItems = (from l in lineItems select l.LineItemId).ToList<string>();
                        this.GetLineItemsListResponse(result.Output);
                    }
                    else
                    {
                        List<Error> resultError = result.ErrorDetails;
                        ShowError(new AppException(resultError), LineItemListError);
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
            catch (Exception ex)
            {
                ShowError((AppException)ex);
            }
        }

        private void GetLineItemsListResponse(string json)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var result = lineItemsBrowser.InvokeScript("constructLineItemListWithCallBack", json);
            });
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            LineItemSummary summary = e.Content as LineItemSummary;
            if (summary != null)
            {
                summary.invoiceLineItems = this.invoiceLineItems;
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (PageInProgress)
            {
                e.Cancel = true;
                return;
            }
            PageInProgress = true;
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                e.Cancel = true;
            }
            base.OnBackKeyPress(e);
        }
     }
}