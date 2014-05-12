/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.ResetPassword;

namespace Tymetrix.T360.Mobile.Client.AppWP7.ResetPassword
{
    public partial class ResetPassword : BasePage
    {
        private const string PasswordError = "Password Reset";

        public ResetPassword()
        {
            InitializeComponent();
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            Prepopulate();
            PageInProgress = false;
            if (UserData.Instance.HasDisclaimer)
            {
                NavigationService.RemoveBackEntry();
            }
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
                BasePage_Loaded(null, null);
            }
            base.OnNavigatedTo(e);
        }

        private void Prepopulate()
        {
            Credential credential = Credential.Instance;
            if (credential.ShowKeepCurrentPassword)
            {
                ApplicationBar.IsVisible = true;
            }
            List<string> rules = credential.Rules;
            ClearLabels(rules.Count);
            if (rules.Count == 1)
            {
                ruleLabel1.Text = rules[0];
                bullet1.Text = "•";
            }
            else if (rules.Count == 2)
            {
                ruleLabel1.Text = rules[0];
                ruleLabel2.Text = rules[1];
                bullet1.Text = "•";
                bullet2.Text = "•";
            }
            else if (rules.Count == 3)
            {
                ruleLabel1.Text = rules[0];
                ruleLabel2.Text = rules[1];
                ruleLabel3.Text = rules[2];
                bullet1.Text = "•";
                bullet2.Text = "•";
                bullet3.Text = "•";
            }
        }

        private void ClearLabels(int totalRules)
        {
            if (totalRules == 0)
            {
                passwordRuleLabel.Text = "";
            }
            ruleLabel1.Text = "";
            ruleLabel2.Text = "";
            ruleLabel3.Text = "";
            bullet1.Text = "";
            bullet2.Text = "";
            bullet3.Text = "";
        }

        private void Save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ChangePassword();
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            Credential.Instance.UseLastPassword = true;
            ChangePassword();
        }

        private void ChangePassword()
        {
            PageInProgress = true;
            ProgressBar.Show();
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }
            
            BaseValidator validator = new BaseValidator();
            Credential credential = GetData();
            if (!validator.ResetPassword(credential) && !credential.UseLastPassword)
            {
                ShowError(validator.ClientException, PasswordError);
                FocusText();
                return;
            }

            string postData = JsonConvert.SerializeObject(credential);
            ServiceInvoker.InvokeServiceUsingPost("api/t360/Security/ResetPassword", postData, false, delegate(object obj, ServiceEventArgs arg)
            {
                ServiceResponse response = arg.Result;

                if (!response.Status)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        FocusText();
                    });
                    if (T360ErrorCodes.PasswordReset.Equals(response.ErrorDetails[0].Code) ||
                            T360ErrorCodes.RequestPasswordReset.Equals(response.ErrorDetails[0].Code) ||
                            T360ErrorCodes.TooSimplePassword.Equals(response.ErrorDetails[0].Code))
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            credential = Credential.Instance;
                            credential.Rules = response.ErrorDetails[0].Data;
                            credential.ShowKeepCurrentPassword = T360ErrorCodes.RequestPasswordReset.Equals(response.ErrorDetails[0].Code);
                        });
                    }
                    else if (T360ErrorCodes.Disclaimer.Equals(response.ErrorDetails[0].Code))
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            UserData.Instance.HasDisclaimer = true;
                            UserData.Instance.DisclaimerTitle = response.ErrorDetails[0].Data[0];
                            UserData.Instance.DisclaimerData = response.ErrorDetails[0].Data[1];
                            Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP7;component/Disclaimer/Disclaimer.xaml", UriKind.Relative);
                            this.NavigationService.Navigate(uri);
                            this.ProgressBar.Hide();
                        });
                    }
                    else if (T360ErrorCodes.LicenseAgreement.Equals(response.ErrorDetails[0].Code))
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            ShowError(new AppException(T360ErrorCodes.LicenseAgreement), "Login Failed");
                            RedirectToLogin();
                        });
                    }
                    else
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            ShowError(new AppException(response.ErrorDetails), PasswordError);
                        });
                    }
                    return;
                }
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ProcessUserData(response.Output, "/Tymetrix.T360.Mobile.Client.AppWP7");
                });
            });
        }

        private void passwordTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Save_Click(null, null);
            }
        }

        private void FocusText()
        {
            passwordTextBox.Password = string.Empty;
            ConfirmPasswordTextBox.Password = string.Empty;
            passwordTextBox.Focus();
            this.ProgressBar.Hide();
        }

        private Credential GetData()
        {
            Credential credential = Credential.Instance;
            credential.Password = passwordTextBox.Password;
            credential.ConfirmPassword = ConfirmPasswordTextBox.Password;
            return credential;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            this.ProgressBar.Show();
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }

            Dashboard.Dashboard.LogOff();
            this.ProgressBar.Hide();
            while (NavigationService.BackStack.Count() > 1)
            {
                NavigationService.RemoveBackEntry();
            }
            UserActivity.Instance.StopTimer();
            base.OnBackKeyPress(e);
        }
    }
}