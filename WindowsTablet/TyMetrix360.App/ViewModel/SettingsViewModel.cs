/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Threading.Tasks;

using TyMetrix360.App.Common;
using TyMetrix360.App.Navigation;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.BusinessObjects.Security;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core;
using TyMetrix360.Core.Container;
using TyMetrix360.Core.Models;
using TyMetrix360.Core.ViewModelBase;

namespace TyMetrix360.App.ViewModel
{
    public class SettingsViewModel : ViewModelCore, ISettingsViewModel
    {
        private LoginInfo _loginInfo;
        public LoginInfo LoginInfo
        {
              get { return _loginInfo; }
              set { SetProperty(ref _loginInfo, value); }
        }
        private UserSettings _userSettings;
        public UserSettings UserSettings
        {
            get { return _userSettings; }
            set { SetProperty(ref _userSettings, value); }
        }
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set 
            {
                SetProperty(ref _isChecked, value);
                UpdateCurrencySettings(value);
            }
        }

        private string _preferenceCurrencyCode;
        public string PreferenceCurrencyCode
        {
            get { return "(" + _preferenceCurrencyCode + ")"; }
            set { SetProperty(ref _preferenceCurrencyCode, value); }
        }
        public SettingsViewModel()
        {
            ChangedCommand = new RelayCommand((e) => { OnPreferenceCurrencyChanged(); });
            LoadData();
            GoBackToDashboardCommand = new RelayCommand(e => CallGoBackToDashboard());
        }

        private void OnPreferenceCurrencyChanged()
        {
            
        }

        private async Task CallGoBackToDashboard()
        {
            try
            {
                if (!ServiceInvoker.Instance.IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);

                IsBusy = true;
                Navigator.Navigate(Destination.DashboardView, parameters: new object[] { null });
                IsBusy = false;
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.SettingsFailed);
            }
        }

        private RelayCommand _changedCommand;
        public RelayCommand ChangedCommand
        {
            get { return _changedCommand; }
            set { SetProperty(ref _changedCommand, value); }
        }

        private IRelayCommand _goBackToDashboardCommand;
        public IRelayCommand GoBackToDashboardCommand
        {
            get { return _goBackToDashboardCommand; }
            set { SetProperty(ref _goBackToDashboardCommand, value); }
        }
        
        public async Task LoadData()
        {
            try
            {
                IsBusy = true;
                var response = await ServiceInvoker.Instance.InvokeServiceUsingGet<UserSettings>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.GetSettingsService));
                IsBusy = false;

                UserSettings = response;
                PreferenceCurrencyCode = UserSettings.PreferenceCurrencyCode;
                IsChecked = UserSettings.IsPreferenceCurrencyEnabled;
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.SettingsFailed);
            }
        }

        private async Task UpdateCurrencySettings(bool isPreferenceCurrencyEnabled)
        {
            try
            {
                IsBusy = true;
                UserSettings settings = new UserSettings() { IsPreferenceCurrencyEnabled = isPreferenceCurrencyEnabled };
                var response = await ServiceInvoker.Instance.InvokeServiceUsingPost<string>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.UpdateSettingsService), settings, true, false);
                IsBusy = false;
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.SettingsFailed);
            }
        }
    }
}
