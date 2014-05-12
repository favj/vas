/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;

using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Common;
using System.Collections.Generic;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.ResetPassword;
using Tymetrix.T360.Mobile.Client.Core;

namespace Tymetrix.T360.Mobile.Client.Common.Base.View
{
    public partial class BasePage : PhoneApplicationPage
    {
        public const string SelectedInvoice = "selectedInvoice";
        public const string SelectedInvoiceId = "selectedInvoiceId";
        public const string SelectedLineItemId = "selectedLineItemId";
        protected static bool PageInProgress = false;

        public BasePage()
        {
            InitializeComponent();
            PageInProgress = true;
        }

        private static OverlayProgressBar _progress;
        protected internal OverlayProgressBar ProgressBar
        {
            get
            {
                if (_progress == null)
                {
                    _progress = new OverlayProgressBar();
                    _progress.ParentPage = this;
                }
                return _progress;
            }
        }

        protected void ShowError(AppException ex)
        {
            if (ex.ErrorCodes.Count == 1 && T360ErrorCodes.UnableToConnectServer.Equals(ex.ErrorCodes[0].Code))
            {
                ShowError(ex, "Network Error");
                return;
            }
            if (ex.ErrorCodes.Count == 1 && T360ErrorCodes.AppSessionExpired.Equals(ex.ErrorCodes[0].Code))
            {
                ShowError(ex, "Session Expired");
                return;
            }
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBoxResult result = T360ViewUtlity.Handle(CultureManager.Instance, ex);
                if (result == MessageBoxResult.OK || result == MessageBoxResult.None)
                {
                    SetFocus();
                }
            });
        }

        protected void ShowError(AppException ex, string title)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBoxResult result = T360ViewUtlity.Handle(CultureManager.Instance, ex, title);
                if (result == MessageBoxResult.OK || result == MessageBoxResult.None)
                {
                    SetFocus();
                }
                if (T360ErrorCodes.AppSessionExpired.Equals(ex.ErrorCodes[0].Code))
                {
                    RedirectToLogin();
                }
            });
        }

        protected void RedirectToLogin()
        {
            LogOff();
            while (NavigationService.BackStack.Count() > 1)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.GoBack();
            this.ProgressBar.Hide();
        }

        public static void LogOff()
        {
            ServiceInvoker.InvokeServiceUsingGet("api/t360/Security/DoLogOff", delegate(object a, ServiceEventArgs serviceEventArgs)
            {
                ServiceResponse result = serviceEventArgs.Result;
            }, false);

            PhoneApplicationService.Current.State.Clear();
            UserData.Instance.Clear();
            Credential.Instance.Clear();
            UserActivity.Instance.StopTimer();
            ServiceInvoker.ClearCookies();
        }

        protected void ProcessUserData(string response, string assembly)
        {
            UserData s = JsonConvert.DeserializeObject<UserData>(response);
            UserData.Instance.AwaitingInvoiceCount = s.AwaitingInvoiceCount;
            UserData.Instance.MemberName = s.MemberName;
            UserData.Instance.NetworkName = s.NetworkName;
            UserData.Instance.IsAuthenticated = true;
            UserData.Instance.HasInvoiceListAccess = s.HasInvoiceListAccess;
            UserData.Instance.HasMatterListAccess = s.HasMatterListAccess;
            UserData.Instance.IsClientOrTymetrixUser = s.IsClientOrTymetrixUser;
            UserData.Instance.InstallationId = s.InstallationId;
            UserData.Instance.InvoiceQuickActionsAccess = s.InvoiceQuickActionsAccess;
            UserData.Instance.DisclaimerTitle = s.DisclaimerTitle;

            if (UserData.Instance.IsAuthenticated)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    if (UserData.Instance.HasSaveUserName)
                    {
                        IsolatedStorageSettings.ApplicationSettings[ServiceInvoker.UserName] = UserData.Instance.UserName;
                    }
                    else
                    {
                        IsolatedStorageSettings.ApplicationSettings.Remove(ServiceInvoker.UserName);
                    }

                    if (UserData.Instance.HasIntegratedLogin)
                    {
                        IsolatedStorageSettings.ApplicationSettings[ServiceInvoker.HasIntegratedLogin] = true;
                    }
                    else
                    {
                        IsolatedStorageSettings.ApplicationSettings.Remove(ServiceInvoker.HasIntegratedLogin);
                    }

                    if (!string.IsNullOrEmpty(s.InstallationId))
                    {
                        IsolatedStorageSettings.ApplicationSettings[ServiceInvoker.InstallationId] = s.InstallationId;
                    }

                    IsolatedStorageSettings.ApplicationSettings.Save();

                    this.ProgressBar.Hide();
                    UserActivity.Instance.LoadTimer();
                    Uri uri = new Uri(assembly + ";component/Dashboard/Dashboard.xaml", UriKind.Relative);
                    NavigationService.Navigate(uri);
                });
            }
        }

        protected virtual void SetFocus()
        {
            this.ProgressBar.Hide();
            this.Focus();
            PageInProgress = false;
        }

        protected void DisableApplicationBar()
        {
            ApplicationBar.IsMenuEnabled = false;
            foreach (ApplicationBarIconButton btn in ApplicationBar.Buttons)
            {
                if (btn != null)
                {
                    btn.IsEnabled = false;
                }
            }
        }

        protected void EnableApplicationBar()
        {
            ApplicationBar.IsMenuEnabled = true;
            foreach (ApplicationBarIconButton btn in ApplicationBar.Buttons)
            {
                if (btn != null)
                {
                    btn.IsEnabled = true;
                }
            }
        }

        protected void RemoveApplicationBarButtons()
        {
            int count = ApplicationBar.Buttons.Count;
            for (int i = count; i > 0 ; i--)
            {
                if (ApplicationBar.Buttons[i - 1] != null)
                {
                    ApplicationBar.Buttons.RemoveAt(i-1);
                }
            }
        }

        //protected void AddButtonsToApplicationBar(ApplicationBarIconButton appBarIconButton, string iconText)
        //{
        //    if(ApplicationBar == null)
        //    ApplicationBar = new ApplicationBar();
        //    //appBarIconButton = new ApplicationBarIconButton();

        //    ApplicationBar.Mode = ApplicationBarMode.Default;
        //    ApplicationBar.Opacity = 1.0;
        //    ApplicationBar.IsVisible = true;
        //    ApplicationBar.IsMenuEnabled = false;
        //    appBarIconButton.Text = iconText;
        //    ApplicationBar.Buttons.Add(appBarIconButton);
        //}

        protected void RemoveButtonsFromApplicationBar()
        {

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //long deviceTotalMemory = (long)Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceTotalMemory");

            //long applicationCurrentMemoryUsage = (long)Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("ApplicationCurrentMemoryUsage");
            //MessageBox.Show("Application Memory : " + applicationCurrentMemoryUsage);
            
            bool isTombStoned = (bool)PhoneApplicationService.Current.State.ContainsKey("isTombStoned");
            if (!isTombStoned || e.IsNavigationInitiator) 
            {
                base.OnNavigatedTo(e);
                return;
            }

            PhoneApplicationService.Current.State["sessionTimedout"] = true;
            // page being restored after TombStoned - redirect to login

            // clear the TombStoned flag, so as not to affect further navigation
            PhoneApplicationService.Current.State.Remove("isTombStoned");

            Uri nUri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP7;component/Base/View/LogOn.xaml", UriKind.Relative);
            ((TransitionFrame)Application.Current.RootVisual).Navigate(nUri);
        }
    }
}