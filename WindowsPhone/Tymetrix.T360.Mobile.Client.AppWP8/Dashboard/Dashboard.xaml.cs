/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;

using Tymetrix.T360.Mobile.Client.Common;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Model.ResetPassword;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.Dashboard;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.AppWP8.Invoice;
using Tymetrix.T360.Mobile.Client.Model.Invoice;
using System.Collections.Generic;

namespace Tymetrix.T360.Mobile.Client.AppWP8.Dashboard
{
    public partial class Dashboard : BasePage
    {
        private const string DashboardError = "DashBoard Error";

        public Dashboard()
        {
            InitializeComponent();
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            PrePopulate();
            PageInProgress = false;
            while (NavigationService.BackStack.Count() > 1)
            {
                NavigationService.RemoveBackEntry();
            }
        }

        private void PrePopulate()
        {
            string invoiceCount = UserData.Instance.AwaitingInvoiceCount;
            invoiceCountLabel.FontSize = 72;
            if (invoiceCount.Length > 2)
            {
                invoiceCountLabel.FontSize = 48;
            }
            invoiceCountLabel.Text = invoiceCount;
            if (UserData.Instance.HasDisclaimer)
            {
                NavigationService.RemoveBackEntry();
                UserData.Instance.HasDisclaimer = false;
                UserData.Instance.DisclaimerTitle = null;
            }
        }

        private void invoiceButton_Click(object sender, RoutedEventArgs e)
        {
            PageInProgress = true;
            ProgressBar.Show();
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }
            if (!UserData.Instance.IsClientOrTymetrixUser)
            {
                ShowError(new AppException(T360ErrorCodes.IsClientOrTymetrixUser), DashboardError);
                return;
            }
            if (!UserData.Instance.HasInvoiceListAccess)
            {
                ShowError(new AppException(T360ErrorCodes.HasInvoiceListAccess), DashboardError);
                return;
            }
            Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Invoice/InvoiceListing.xaml", UriKind.Relative);
            this.NavigationService.Navigate(uri);
            ProgressBar.Hide();
            PageInProgress = false;
        }

        private void DoLogout()
        {
            try
            {
                RedirectToLogin();
            }
            catch (AppException ex)
            {
                ShowError(ex, DashboardError);
            }
        }

        private void logOutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msgResult = MessageBox.Show("Are you sure you want to logoff?", "Logoff Confirmation", MessageBoxButton.OKCancel);
            if (msgResult == MessageBoxResult.Cancel) return;
            
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }
            
            DoLogout();

            UserActivity.Instance.StopTimer();
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Base/View/LogOn.xaml", UriKind.Relative);
                this.NavigationService.Navigate(uri);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            InvoiceListing invoiceListing = e.Content as InvoiceListing;
            if (invoiceListing != null)
            {
                invoiceListing.Source = Source.DASHBOARD;
            }
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.Back && PhoneApplicationService.Current.State.ContainsKey("sessionTimedout"))
            {
                return;
            }
            if ((e.NavigationMode == NavigationMode.Back) && (e.Content.GetType() == typeof(Dashboard)))
            {
                try
                {
                    ServiceInvoker.InvokeServiceUsingGet("/api/t360/Invoice/GetDashboardInfo", delegate(object a, ServiceEventArgs serviceEventArgs)
                    {
                        ServiceResponse result = serviceEventArgs.Result;
                        DashboardInfo dashBoardInfo = JsonConvert.DeserializeObject<DashboardInfo>(result.Output);
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            string invoiceCount = UserData.Instance.AwaitingInvoiceCount = dashBoardInfo.InvoiceCount;
                            invoiceCountLabel.FontSize = 72;
                            if (invoiceCount.Length > 2)
                            {
                                invoiceCountLabel.FontSize = 48;
                            }
                            invoiceCountLabel.Text = UserData.Instance.AwaitingInvoiceCount;
                        });
                    }, false);
                }
                catch (Exception ex)
                {
                    ShowError((AppException)ex);
                }
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                e.Cancel = true;
                return;
            }
            
            if (NavigationService.CanGoBack)
            {
                DoLogout();
                NavigationService.RemoveBackEntry();
            }
            this.ProgressBar.Hide();
            base.OnBackKeyPress(e);
        }

        private void supportButton_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Support/Support.xaml", UriKind.Relative);
            this.NavigationService.Navigate(uri);
        }

        private void faqButton_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Faq/Faq.xaml", UriKind.Relative);
            this.NavigationService.Navigate(uri);
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }
            if (!UserData.Instance.IsClientOrTymetrixUser)
            {
                ShowError(new AppException(T360ErrorCodes.IsClientOrTymetrixUser), DashboardError);
                return;
            }

            Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Settings/Settings.xaml", UriKind.Relative);
            this.NavigationService.Navigate(uri);
        }
    }
}