/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

using TyMetrix360.App.Common;
using TyMetrix360.App.Navigation;
using TyMetrix360.App.Notification;
using TyMetrix360.App.Validator;
using TyMetrix360.BusinessObjects.Security;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core;
using TyMetrix360.Core.Container;
using TyMetrix360.Core.Converters;
using TyMetrix360.Core.Models;
using TyMetrix360.Core.ViewModelBase;
using GalaSoft.MvvmLight.Messaging;

namespace TyMetrix360.App.ViewModel
{
    public class LoginViewModel : ViewModelCore, ILoginViewModel
    {
        private LoginInfo _loginInfo;
        public LoginInfo LoginInfo
        {
              get { return _loginInfo; }
              set { SetProperty(ref _loginInfo, value); }
        }
        private string _password;
        public string Password
        {
            get { return Vault.AES_Decrypt(_password); }
            set { SetProperty(ref _password, Vault.AES_Encrypt(value)); }
        }
        private Credential _credential;
        public Credential Credential
        {
            get { return _credential; }
            set { SetProperty(ref _credential, value); }
        }
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand((e) => { OnLogin(e); });
            LoadData();
        }
        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand
        {
            get { return _loginCommand; }
            set { SetProperty(ref _loginCommand, value); }
        }
        private string _userName;

        public string UserName
        {
            get { return Vault.AES_Decrypt(_userName); }
            set { SetProperty(ref _userName, Vault.AES_Encrypt(value)); }
        }
        private string _integratedLoginID;
        public string IntegratedLoginID
        {
            get { return _integratedLoginID; }
            set { SetProperty(ref _integratedLoginID, value); }
        }
        private bool _saveUserName;
        public bool SaveUserName
        {
            get { return _saveUserName; }
            set { SetProperty(ref _saveUserName, value); }
        }
        private bool _integratedLogin;
        public bool IntegratedLogin
        {
            get { return _integratedLogin; }
            set
            {
                SetProperty(ref _integratedLogin, value);
                ShowIntegratedLogin = (Visibility)new BooleanToVisibilityConverter().Convert(value, typeof(bool), null, null);
            }
        }
        private Visibility _showIntegratedLogin;
        public Visibility ShowIntegratedLogin
        {
            get { return _showIntegratedLogin; }
            set { SetProperty(ref _showIntegratedLogin, value); }
        }

        public async void OnLogin(object password)
        {
            try
            {
                Messenger.Default.Register<string>(this, "Encryption", EncryptPassword);
                string currencyCode = Windows.Globalization.Language.CurrentInputMethodLanguageTag;
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = currencyCode;
                string intLoginId = IntegratedLogin
                    ? (IntegratedLoginID == null) ? string.Empty : IntegratedLoginID
                    : null;
                T360Validator.ValidateLogin(UserName, (string)password, intLoginId);
                IsBusy = true;
                UserPreference.Instance.HasSaveUserName = SaveUserName;
                UserPreference.Instance.UserName = UserName;
                UserPreference.Instance.HasIntegratedLogin = IntegratedLogin;
                await ServiceInvoker.Instance.Initialize();
                var serializableData = GetLoginParameter(password);
                LoginInfo = await ServiceInvoker.Instance.InvokeServiceUsingPost<LoginInfo>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.DoLogin), serializableData, false, true);
                IsBusy = false;
                Messenger.Default.Unregister<string>(this, "Encryption", EncryptPassword);
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
                else
                {
                    Password = string.Empty;
                    IntegratedLoginID = string.Empty;
                    string message = getMessages(ex);
                    ShowErrorMessage(message, Constants.LoginFailed);
                }
            }
        }

        private string encryptedPassword;
        private void EncryptPassword(string key)
        {
            encryptedPassword = Vault.Encrypt(Password, key);
        }

        private object GetLoginParameter(object password)
        {
            return new
            {
                LoginId = UserName,
                Password = password == null ? string.Empty : encryptedPassword,
                IsSSOEnabled = IntegratedLogin,
                IntegratedLoginId = IntegratedLogin ? IntegratedLoginID : string.Empty
            };
        }

        public Task LoadData()
        {
            PushNotification.OpenPushChannel();
            ShowIntegratedLogin = Visibility.Collapsed;
            SaveUserName = true;
            SetUserDetails();
            return null;
        }

        private void SetUserDetails()
        {

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(Constants.UserNameKey))
            {
                UserName = (string)ApplicationData.Current.LocalSettings.Values[Constants.UserNameKey];
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(Constants.IntegratedLoginKey))
            {
                bool isIntegrated = (bool)ApplicationData.Current.LocalSettings.Values[Constants.IntegratedLoginKey];
                IntegratedLogin = isIntegrated;
                ShowIntegratedLogin = (Visibility)new BooleanToVisibilityConverter().Convert(isIntegrated, typeof(bool), null, null);
            }
        }
    }
}
