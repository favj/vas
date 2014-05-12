/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;

using Tymetrix.T360.Mobile.Client.Common;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.ResetPassword;
using Tymetrix.T360.Mobile.Client.Core;

namespace Tymetrix.T360.Mobile.Client.AppWP8.Base.View
{
    public partial class LogOn : BasePage
    {
        private const string LoginError = "Login Failed";

        public LogOn()
        {
            InitializeComponent();
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            userNameTextBox.Text = string.Empty;
            passwordTextBox.Password = string.Empty;
            PrePopulate();
            if (PhoneApplicationService.Current.State.ContainsKey("sessionTimedout"))
            {
                T360ViewUtlity.Handle(CultureManager.Instance, new AppException(T360ErrorCodes.SessionExpired), "Session Timed Out");
                PhoneApplicationService.Current.State.Remove("sessionTimedout");
            }
        }

        private void PrePopulate()
        {
            IntegratedToggleButton.IsChecked = false;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(ServiceInvoker.InstallationId))
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(ServiceInvoker.UserName))
                {
                    userNameTextBox.Text = (string)IsolatedStorageSettings.ApplicationSettings[ServiceInvoker.UserName];
                }
                else
                {
                    userNameTextBox.Text = string.Empty;
                    passwordTextBox.Password = string.Empty;
                }

                if (IsolatedStorageSettings.ApplicationSettings.Contains(ServiceInvoker.HasIntegratedLogin))
                {
                    integratedLoginTextBox.Text = string.Empty;
                    IntegratedToggleButton.IsChecked = true;
                }
            }
            saveUserToggleButton.IsChecked = true;
        }

        protected override void SetFocus()
        {
            base.SetFocus();
            userNameTextBox.Focus();
            loginButton.IsEnabled = true;
            passwordTextBox.Password = string.Empty;
            integratedLoginTextBox.Text = string.Empty;
            UserData.Instance.Clear();
        }
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            DismissKeyBoard();
            this.ProgressBar.Show();
            try
            {
                if (!ServiceInvoker.IsConnected)
                {
                    ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                    return;
                }
                this.DoLogin(GetData());
            }
            catch (AppException ex)
            {
                ShowError(ex, LoginError);
            }
        }

        private void DoLogin(UserData userInfo)
        {
            BaseValidator validator = new BaseValidator();

            if (!validator.ValidateLogOnFields(userInfo))
            {
                ShowError(validator.ClientException, LoginError);
                return;
            }

            UserData.Instance.HasSaveUserName = (bool)saveUserToggleButton.IsChecked;
            UserData.Instance.HasIntegratedLogin = (bool)IntegratedToggleButton.IsChecked;

            ServiceInvoker.Initialize(delegate(object a, ServiceEventArgs serviceEventArgs)
            {
                ServiceResponse result = serviceEventArgs.Result;
                if (!result.Status)
                {
                    ShowError(new AppException(result.ErrorDetails), LoginError);
                    return;
                }

                if (!Convert.ToBoolean(result.Output))
                {
                    ShowError(new AppException(result.ErrorDetails), LoginError);
                    return;
                }

                IsolatedStorageSettings.ApplicationSettings.Save();

                string postData = JsonConvert.SerializeObject(userInfo);
                ServiceInvoker.InvokeServiceUsingPost("api/t360/Security/DoLogin", postData, true, delegate(object obj, ServiceEventArgs arg)
                {
                    ServiceResponse response = arg.Result;

                    if (!response.Status)
                    {
                        if (T360ErrorCodes.Disclaimer.Equals(response.ErrorDetails[0].Code))
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                UserData.Instance.HasDisclaimer = true;
                                UserData.Instance.DisclaimerTitle = response.ErrorDetails[0].Data[0];
                                UserData.Instance.DisclaimerData = response.ErrorDetails[0].Data[1];
                                UserActivity.Instance.LoadTimer();
                                UserData.Instance.IsAuthenticated = true;
                                Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/Disclaimer/Disclaimer.xaml", UriKind.Relative);
                                this.NavigationService.Navigate(uri);
                                this.ProgressBar.Hide();
                            });
                        }
                        else if (T360ErrorCodes.PasswordReset.Equals(response.ErrorDetails[0].Code) ||
                                T360ErrorCodes.RequestPasswordReset.Equals(response.ErrorDetails[0].Code) ||
                                T360ErrorCodes.TooSimplePassword.Equals(response.ErrorDetails[0].Code))
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                Credential credential = Credential.Instance;
                                credential.Rules = response.ErrorDetails[0].Data;
                                credential.ShowKeepCurrentPassword = T360ErrorCodes.RequestPasswordReset.Equals(response.ErrorDetails[0].Code);
                                UserActivity.Instance.LoadTimer();
                                UserData.Instance.IsAuthenticated = true;
                                Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP8;component/ResetPassword/ResetPassword.xaml", UriKind.Relative);
                                this.NavigationService.Navigate(uri);
                                this.ProgressBar.Hide();
                            });
                        }
                        else
                        {
                            ShowError(new AppException(response.ErrorDetails), LoginError);
                        }
                        return;
                    }
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        ProcessUserData(response.Output, "/Tymetrix.T360.Mobile.Client.AppWP8");
                    });
                });
            });
        }

        private void DismissKeyBoard()
        {
            this.Focus();
        }

        private UserData GetData()
        {
            UserData userInfo = UserData.Instance;
            userInfo.UserName = userNameTextBox.Text;
            userInfo.Password = passwordTextBox.Password;
            userInfo.IntegratedLoginId = integratedLoginTextBox.Text;
            if (IntegratedToggleButton.IsChecked == true)
            {
                userInfo.IsSSOEnabled = true;
            }
            else
            {
                userInfo.IsSSOEnabled = false;
            }
            return userInfo;
        }

        private void userNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.loginButton_Click(null, null);
            }
        }

        private void IntegratedToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            integratedLoginTextBox.Text = string.Empty;
            integratedLoginlabel.Visibility = System.Windows.Visibility.Collapsed;
            integratedLoginTextBox.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void IntegratedToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            integratedLoginlabel.Visibility = System.Windows.Visibility.Visible;
            integratedLoginTextBox.Visibility = System.Windows.Visibility.Visible;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
        }
    }
}