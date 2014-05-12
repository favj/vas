/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Threading.Tasks;
using Windows.UI.Popups;

using TyMetrix360.App.Common;
using TyMetrix360.App.Navigation;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core;
using TyMetrix360.Core.Container;
using TyMetrix360.Core.Models;
using TyMetrix360.Core.ViewModelBase;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.BusinessObjects.Security;
using TyMetrix360.App.Notification;
using Windows.Storage;

namespace TyMetrix360.App.ViewModel
{
    public class DashboardViewModel : ViewModelCore, IDashboardViewModel
    {
        private LoginInfo _loginInfo;
        public LoginInfo LoginInfo
        {
            get { return _loginInfo; }
            set { SetProperty(ref _loginInfo, value); }
        }
        private DashboardInfo _dashboardInfo;
        public DashboardInfo DashboardInfo
        {
            get { return _dashboardInfo; }
            set { SetProperty(ref _dashboardInfo, value); }
        }
        private string _invoiceCount;
        public string InvoiceCount
        {
            get { return _invoiceCount; }
            set { SetProperty(ref _invoiceCount, value); }
        }

        private string _memberName;
        public string MemberName
        {
            get { return Vault.AES_Decrypt(_memberName); }
            set { SetProperty(ref _memberName, Vault.AES_Encrypt(value)); }
        }

        private RelayCommand _invoiceCommand;
        public RelayCommand InvoiceListCommand
        {
            get { return _invoiceCommand; }
            set { SetProperty(ref _invoiceCommand, value); }
        }

        private RelayCommand _logoutCommand;
        public RelayCommand LogoutCommand
        {
            get { return _logoutCommand; }
            set { SetProperty(ref _logoutCommand, value); }
        }

        private RelayCommand _settingsCommand;
        public RelayCommand SettingsCommand
        {
            get { return _settingsCommand; }
            set { SetProperty(ref _settingsCommand, value); }
        }

        private RelayCommand _supportCommand;
        public RelayCommand SupportCommand
        {
            get { return _supportCommand; }
            set { SetProperty(ref _supportCommand, value); }
        }

        private RelayCommand _faqCommand;
        public RelayCommand FaqCommand
        {
            get { return _faqCommand; }
            set { SetProperty(ref _faqCommand, value); }
        }

        public override async Task LoadData(params object[] parameters)
        {
            if (parameters != null)
            {
                if (parameters[0] != null)
                {
                    LoginInfo = (LoginInfo)parameters[0];
                    MemberName = UserPreference.Instance.CurrentUserName = LoginInfo.MemberName;
                    PushNotification.UpdateTile(LoginInfo.InvoiceCount);
                    UserPreference.Instance.IsClientOrTymetrixUser = LoginInfo.IsClientOrTymetrixUser;
                    UserPreference.Instance.HasInvoiceListAccess = LoginInfo.HasInvoiceListAccess;
                    DashboardInfo = new DashboardInfo() { InvoiceCount = LoginInfo.InvoiceCount };
                    SaveUserDetails();
                }
                else
                {
                    try
                    {
                        IsBusy = true;
                        DashboardInfo = await ServiceInvoker.Instance.InvokeServiceUsingGet<DashboardInfo>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.GetDashboardInfoService));
                        MemberName = UserPreference.Instance.CurrentUserName;
                        IsBusy = false;
                    }
                    catch (T360Exception ex)
                    {
                        string message = getMessages(ex);
                        ShowErrorMessage(message, Constants.DashboardFailed);
                    }
                }
            }
        }

        public DashboardViewModel()
        {
            InvoiceListCommand = new RelayCommand((e) => { CallInvoiceList(); });
            SettingsCommand = new RelayCommand((e) => { OnSettings(); });
            LogoutCommand = new RelayCommand((e) => { LogOut(); });
            SupportCommand = new RelayCommand((e) => { OnSupport(); });
            FaqCommand = new RelayCommand((e) => { OnFAQ(); });
        }

        public void OnSupport()
        {
            Navigator.Navigate(Destination.SupportView, ExistingViewBehavior.Remove, new object[] { });
        }

        public void OnFAQ()
        {
            Navigator.Navigate(Destination.FaqView, ExistingViewBehavior.Remove, new object[] { });
        }

        public void OnSettings()
        {
            try
            {
                if (!ServiceInvoker.Instance.IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);

                if (!UserPreference.Instance.IsClientOrTymetrixUser)
                {
                    ShowErrorMessage(getMessages(new T360Exception(T360ErrorCodes.IsClientOrTymetrixUser)), Constants.DashboardFailed);
                    return;
                }
                Navigator.Navigate(Destination.SettingsView);
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.DashboardFailed);
            }
        }

        private async void LogOut()
        {
            var isOk = await ShowConfirmationMessage(Constants.LogoffConfirmationMsg, Constants.LogoffTitle);
            if (!isOk) return;
            try
            {
                if (LoginInfo != null)
                {
                    LoginInfo.LoginId = string.Empty;
                    LoginInfo.MemberName = string.Empty;
                }
                IsBusy = true;
                UserPreference.Instance.Clear();
                await ServiceInvoker.Instance.LogOut();
                IsBusy = false;
                Navigator.Navigate(Destination.LoginView);
            }
            catch (T360Exception ex)
            {
                string msg = getMessages(ex);
                ShowErrorMessage(msg, Constants.LogoffFailed);
            }
        }

        private void CommandInvokeHandler(IUICommand command)
        {
            throw new System.NotImplementedException();
        }

        private void CallInvoiceList()
        {
            try
            {
                if (!ServiceInvoker.Instance.IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);

                if (!UserPreference.Instance.IsClientOrTymetrixUser)
                {
                    ShowErrorMessage(getMessages(new T360Exception(T360ErrorCodes.IsClientOrTymetrixUser)), Constants.DashboardFailed);
                    return;
                }
                if (!UserPreference.Instance.HasInvoiceListAccess)
                {
                    ShowErrorMessage(getMessages(new T360Exception(T360ErrorCodes.HasInvoiceListAccess)), Constants.DashboardFailed);
                    return;
                }
                Navigator.Navigate(Destination.InvoiceListView);
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.DashboardFailed);
            }
        }

        private void SaveUserDetails()
        {
            if (UserPreference.Instance.HasSaveUserName)
            {
                ApplicationData.Current.LocalSettings.Values[Constants.UserNameKey] = UserPreference.Instance.UserName;
                UserPreference.Instance.HasSaveUserName = false;
                UserPreference.Instance.UserName = string.Empty;
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values[Constants.UserNameKey] = string.Empty;
            }

            ApplicationData.Current.LocalSettings.Values[Constants.IntegratedLoginKey] = UserPreference.Instance.HasIntegratedLogin;
            UserPreference.Instance.HasIntegratedLogin = false;
        }

        public override bool ShowAppBar { get { return true; } }
        public override bool ShowPrivacyPolicyButton { get { return true; } }
    }
}
