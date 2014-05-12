/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Newtonsoft.Json;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;

using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.Settings;

namespace Tymetrix.T360.Mobile.Client.AppWP8.Settings
{
    public partial class Settings : BasePage
    {
        private const string SettingsError = "User Preference";
        public Settings()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.ProgressBar.Show();
            LoadSettings();
            this.ProgressBar.Hide();
        }

        private void EnableEvents()
        {
            preferredCurrencyToggleSwitch.Checked += new EventHandler<RoutedEventArgs>(preferredCurrencyToggleSwitch_Checked);
            preferredCurrencyToggleSwitch.Unchecked += new EventHandler<RoutedEventArgs>(preferredCurrencyToggleSwitch_Unchecked);
        }

        private void DisableEvents()
        {
            preferredCurrencyToggleSwitch.Checked -= new EventHandler<RoutedEventArgs>(preferredCurrencyToggleSwitch_Checked);
            preferredCurrencyToggleSwitch.Unchecked -= new EventHandler<RoutedEventArgs>(preferredCurrencyToggleSwitch_Unchecked);
        }

        private void LoadSettings()
        {
            try
            {
                ServiceInvoker.InvokeServiceUsingGet("/api/t360/Settings/GetSettings", delegate(object a, ServiceEventArgs serviceEventArgs)
                {
                    ServiceResponse result = serviceEventArgs.Result;
                    if (result.Status)
                    {
                        UserSettings userSettings = JsonConvert.DeserializeObject<UserSettings>(result.Output);
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            DisableEvents();
                            preferredCurrencyToggleSwitch.IsChecked = userSettings.IsPreferenceCurrencyEnabled;
                            preferredCurrencyLabel.Text = string.Format("({0})", userSettings.PreferenceCurrencyCode);
                            EnableEvents();
                        });
                    }
                    else
                    {
                        ShowError(new AppException(result.ErrorDetails), SettingsError);
                    }
                    PageInProgress = false;
                }, false);
            }
            catch (Exception ex)
            {
                ShowError((AppException)ex);
            }
        }

        private void preferredCurrencyToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            if (!ServiceInvoker.IsConnected)
            {
                DisableEvents();
                preferredCurrencyToggleSwitch.IsChecked = false;
                EnableEvents();
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }
            string postData = JsonConvert.SerializeObject(new UserSettings() { IsPreferenceCurrencyEnabled = true });
            try
            {
                ServiceInvoker.InvokeServiceUsingPost("/api/t360/Settings/UpdateSettings", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
                {
                    ServiceResponse result = serviceEventArgs.Result;
                    if (!result.Status)
                    {
                        ShowError(new AppException(result.ErrorDetails), SettingsError);
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError((AppException)ex);
            }
        }

        private void preferredCurrencyToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!ServiceInvoker.IsConnected)
            {
                DisableEvents();
                preferredCurrencyToggleSwitch.IsChecked = true;
                EnableEvents();
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }
            string postData = JsonConvert.SerializeObject(new UserSettings() { IsPreferenceCurrencyEnabled = false });
            ServiceInvoker.InvokeServiceUsingPost("/api/t360/Settings/UpdateSettings", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
            {
                ServiceResponse result = serviceEventArgs.Result;
                if (!result.Status)
                {
                    ShowError(new AppException(result.ErrorDetails), SettingsError);
                }
            });
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

            while (NavigationService.BackStack.Count() > 2)
            {
                NavigationService.RemoveBackEntry();
            }
            base.OnBackKeyPress(e);
        }
    }
}