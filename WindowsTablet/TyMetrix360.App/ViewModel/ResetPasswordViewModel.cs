/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;
using System.Threading.Tasks;

using TyMetrix360.App.Common;
using TyMetrix360.App.Navigation;
using TyMetrix360.BusinessObjects.Security;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core;
using TyMetrix360.Core.Container;
using TyMetrix360.Core.Models;
using TyMetrix360.Core.ViewModelBase;

namespace TyMetrix360.App.ViewModel
{
    public class ResetPasswordViewModel : ViewModelCore, IResetPasswordViewModel
    {
        private LoginInfo _loginInfo;
        public LoginInfo LoginInfo
        {
            get { return _loginInfo; }
            set { SetProperty(ref _loginInfo, value); }
        }

        private Credential _credential;
        public Credential Credential
        {
            get { return _credential; }
            set { SetProperty(ref _credential, value); }
        }

        private bool _showKeepCurrentPassword;
        public bool ShowKeepCurrentPassword
        {
            get { return _showKeepCurrentPassword; }
            set { SetProperty(ref _showKeepCurrentPassword, value); }
        }

        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set { SetProperty(ref _newPassword, value); }
        }

        private string _confirmNewPassword;
        public string ConfirmNewPassword
        {
            get { return _confirmNewPassword; }
            set { SetProperty(ref _confirmNewPassword, value); }
        }

        private bool _rule1Exists;
        public bool Rule1Exists
        {
            get { return _rule1Exists; }
            set { SetProperty(ref _rule1Exists, value); }
        }

        private bool _rule2Exists;
        public bool Rule2Exists
        {
            get { return _rule2Exists; }
            set
            {
                SetProperty(ref _rule2Exists, value);
                if (value)
                {
                    Rule1Exists = true;
                }
            }
        }

        private bool _rule3Exists;
        public bool Rule3Exists
        {
            get { return _rule3Exists; }
            set
            {
                SetProperty(ref _rule3Exists, value);
                if (value)
                {
                    Rule2Exists = true;
                    Rule1Exists = true;
                }
            }
        }

        private string _ruleA;
        public string RuleA
        {
            get { return _ruleA; }
            set
            {
                SetProperty(ref _ruleA, value);
            }
        }
        private string _ruleB;
        public string RuleB
        {
            get { return _ruleB; }
            set
            {
                SetProperty(ref _ruleB, value);
            }
        }
        private string _ruleC;
        public string RuleC
        {
            get { return _ruleC; }
            set
            {
                SetProperty(ref _ruleC, value);
            }
        }

        private RelayCommand _changePasswordCommand;
        public RelayCommand ChangePasswordCommand
        {
            get { return _changePasswordCommand; }
            set { SetProperty(ref _changePasswordCommand, value); }
        }

        private RelayCommand _keepCurrentPasswordCommand;
        public RelayCommand KeepCurrentPasswordCommand
        {
            get { return _keepCurrentPasswordCommand; }
            set { SetProperty(ref _keepCurrentPasswordCommand, value); }
        }

        private RelayCommand _goBackToLoginCommand;
        public RelayCommand GoBackToLoginCommand
        {
            get { return _goBackToLoginCommand; }
            set { SetProperty(ref _goBackToLoginCommand, value); }
        }

        public ResetPasswordViewModel()
        {
            ChangePasswordCommand = new RelayCommand((e) => { OnChangePassword(); });
            KeepCurrentPasswordCommand = new RelayCommand((e) => { OnKeepCurrentPassword(); });
            GoBackToLoginCommand = new RelayCommand((e) => { GoBackToLogin(); });
        }

        private async void GoBackToLogin()
        {
            IsBusy = true;
            await ServiceInvoker.Instance.LogOut();
            Navigator.Navigate(Destination.LoginView);
            IsBusy = false;
        }

        private void OnKeepCurrentPassword()
        {
            Credential.UseLastPassword = true;
            OnChangePassword();
        }

        private async void OnChangePassword()
        {
            try
            {
                if (!Credential.UseLastPassword)
                {
                    Validator.T360Validator.ValidateResetPassword(NewPassword, ConfirmNewPassword);
                }
                IsBusy = true;
                Credential credential = new Credential() { Password = NewPassword, ConfirmPassword = ConfirmNewPassword };
                LoginInfo = await ServiceInvoker.Instance.InvokeServiceUsingPost<LoginInfo>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.ResetPasswordService),credential,false,false);
                IsBusy = false;
                Navigator.Navigate(Destination.DashboardView, ExistingViewBehavior.Remove, LoginInfo);
            }
            catch (T360Exception ex)
            {
                if (T360ErrorCodes.DisclaimerError.Equals(ex.ErrorCodes[0].Code))
                {
                    LoginInfo = new LoginInfo();
                    LoginInfo.DisclaimerTitle = ex.ErrorCodes[0].Data[0];
                    LoginInfo.DisclaimerData = ex.ErrorCodes[0].Data[1];
                    LoginInfo.HasDisclaimer = true;
                    LoginInfo.IsAuthenticated = true;
                    Navigator.Navigate(Destination.DisclaimerView, ExistingViewBehavior.Remove, LoginInfo);
                }
                else if (T360ErrorCodes.PasswordReset.Equals(ex.ErrorCodes[0].Code) ||
                    T360ErrorCodes.RequestPasswordReset.Equals(ex.ErrorCodes[0].Code) ||
                    T360ErrorCodes.TooSimplePassword.Equals(ex.ErrorCodes[0].Code))
                {
                    Credential = new Credential();
                    Credential.Rules = ex.ErrorCodes[0].Data;
                    Credential.ShowKeepCurrentPassword = T360ErrorCodes.RequestPasswordReset.Equals(ex.ErrorCodes[0].Code);
                    Navigator.Navigate(Destination.ResetPasswordView, ExistingViewBehavior.Remove, Credential);
                }
                else if (T360ErrorCodes.LicenseAgreement.Equals(ex.ErrorCodes[0].Code) ||
                    T360ErrorCodes.MobileAccess.ToUpper().Equals(ex.ErrorCodes[0].Code.ToUpper()))
                {
                    string msg = getMessages(ex);
                    ShowErrorMessage(msg, Constants.LoginFailed);
                    Navigator.Navigate(Destination.LoginView, ExistingViewBehavior.Remove, new object[] { });
                }
                else
                {
                    ConfirmNewPassword = string.Empty;
                    NewPassword = string.Empty;
                    string message = getMessages(ex);
                    ShowErrorMessage(message, Constants.PasswordError);
                }
            }
        }

        public override async Task LoadData(params object[] parameters)
        {
            if (parameters != null)
            {
                if (parameters[0] != null)
                {
                    Credential = parameters[0] as Credential;
                    ShowKeepCurrentPassword = Credential.ShowKeepCurrentPassword;

                    List<string> passwordRules = Credential.Rules;
                    int rulesCount = passwordRules.Count;
                    if (rulesCount == 1)
                    {
                        Rule1Exists = true;
                        RuleA = passwordRules[0];
                    }
                    else if (rulesCount == 2)
                    {
                        Rule2Exists = true;
                        RuleA = passwordRules[0];
                        RuleB = passwordRules[1];
                    }
                    else if (rulesCount == 3)
                    {
                        Rule3Exists = true;
                        RuleA = passwordRules[0];
                        RuleB = passwordRules[1];
                        RuleC = passwordRules[2];
                    }
                }
            }
        }
    }
}
