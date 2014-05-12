/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Tymetrix.T360.Mobile.Client.Common;

using Microsoft.Phone.Shell;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.ResetPassword;
using Tymetrix.T360.Mobile.Client.Core;

namespace Tymetrix.T360.Mobile.Client.AppWP8.Disclaimer
{
    public partial class Disclaimer : BasePage
    {
        public Disclaimer()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ProgressBar.Show();
            Uri u = new Uri("Resources/html/Disclaimer.html", UriKind.Relative);
            disclaimerBrowser.Navigate(u);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (PhoneApplicationService.Current.State.ContainsKey("sessionTimedout"))
            {
                PhoneApplicationService.Current.State.Remove("sessionTimedout");
                return;
            }
            if (PhoneApplicationService.Current.State.ContainsKey("isFromActivated") && PageInProgress)
            {
                PhoneApplicationService.Current.State.Remove("isFromActivated");
                PhoneApplicationPage_Loaded(null, null);
            }
            base.OnNavigatedTo(e);
        }

        private void acceptButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.ProgressBar.Show();
                PageInProgress = true;
                if (!ServiceInvoker.IsConnected)
                {
                    ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                    return;
                }

                ServiceInvoker.InvokeServiceUsingGet("api/t360/Security/AcceptDisclaimer", delegate(object a, ServiceEventArgs serviceEventArgs)
                {
                    ServiceResponse response = serviceEventArgs.Result;

                    if (!response.Status)
                    {
                        if (T360ErrorCodes.PasswordReset.Equals(response.ErrorDetails[0].Code) ||
                            T360ErrorCodes.RequestPasswordReset.Equals(response.ErrorDetails[0].Code) ||
                            T360ErrorCodes.TooSimplePassword.Equals(response.ErrorDetails[0].Code))
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                Credential credential = Credential.Instance;
                                credential.Rules = response.ErrorDetails[0].Data;
                                credential.ShowKeepCurrentPassword = T360ErrorCodes.RequestPasswordReset.Equals(response.ErrorDetails[0].Code);
                                Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/ResetPassword/ResetPassword.xaml", UriKind.Relative);
                                this.NavigationService.Navigate(uri);
                                this.ProgressBar.Hide();
                            });
                        }
                        else if (T360ErrorCodes.LicenseAgreement.Equals(response.ErrorDetails[0].Code))
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                ShowError(new AppException(T360ErrorCodes.LicenseAgreement), "Login Failed");
                                this.ProgressBar.Hide();
                                RedirectToLogin();
                            });
                        }
                        else
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                ShowError(new AppException(response.ErrorDetails), "Login Failed");
                                this.ProgressBar.Hide();
                            });
                        }
                        return;
                    }
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        ProcessUserData(response.Output, "/Tymetrix.T360.Mobile.Client.AppWP8");
                    });
                    
                },false);                
            }
            catch (AppException ex)
            {
                ShowError(ex, "Login Failed");
            }
        }

        private void declineButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ProgressBar.Show();
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }
            Dashboard.Dashboard.LogOff();
            NavigationService.GoBack();
            this.ProgressBar.Hide();
        }

        private void disclaimerBrowser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (e.Value.StartsWith("Completed"))
            {
                this.ProgressBar.Hide();
                PageInProgress = false;
            }
        }

        private void disclaimerBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var result = disclaimerBrowser.InvokeScript("constructDisclaimerWithCallBack", UserData.Instance.DisclaimerTitle, UserData.Instance.DisclaimerData);
            });
        }

        private void disclaimerBrowser_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            this.ProgressBar.Hide();
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
            Dashboard.Dashboard.LogOff();
            while (NavigationService.BackStack.Count() > 1)
            {
                NavigationService.RemoveBackEntry();
            }
            UserActivity.Instance.StopTimer();
            base.OnBackKeyPress(e);
        }
    }
}