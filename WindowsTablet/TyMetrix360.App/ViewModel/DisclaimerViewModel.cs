/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

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
    public class DisclaimerViewModel : ViewModelCore, IDisclaimerViewModel
    {
        public DisclaimerViewModel()
        {
            AcceptCommand = new RelayCommand((e) => { OnAccept(); });
            DeclineCommand = new RelayCommand((e) => { OnDecline(); });
        }

        private LoginInfo _loginInfo;
        public LoginInfo LoginInfo
        {
            get { return _loginInfo; }
            set { SetProperty(ref _loginInfo, value); }
        }

        private RelayCommand _acceptCommand;
        public RelayCommand AcceptCommand
        {
            get { return _acceptCommand; }
            set { SetProperty(ref _acceptCommand, value); }
        }

        private RelayCommand _declineCommand;
        public RelayCommand DeclineCommand
        {
            get { return _declineCommand; }
            set { SetProperty(ref _declineCommand, value); }
        }

        public async void OnAccept()
        {
            try
            {
                IsBusy = true;
                LoginInfo = await ServiceInvoker.Instance.InvokeServiceUsingGet<LoginInfo>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.AcceptDisclaimerService));
                IsBusy = false;
                Navigator.Navigate(Destination.DashboardView, ExistingViewBehavior.Remove, LoginInfo);
            }
            catch (T360Exception ex)
            {
                if (T360ErrorCodes.PasswordReset.ToUpper().Equals(ex.ErrorCodes[0].Code.ToUpper()) ||
                    T360ErrorCodes.RequestPasswordReset.ToUpper().Equals(ex.ErrorCodes[0].Code.ToUpper()) ||
                    T360ErrorCodes.TooSimplePassword.Equals(ex.ErrorCodes[0].Code.ToUpper()))
                {
                    Credential credential = new Credential();
                    credential.Rules = ex.ErrorCodes[0].Data;
                    credential.ShowKeepCurrentPassword = T360ErrorCodes.RequestPasswordReset.ToUpper().Equals(ex.ErrorCodes[0].Code.ToUpper());
                    Navigator.Navigate(Destination.ResetPasswordView, ExistingViewBehavior.Remove, credential);
                }
                else if (T360ErrorCodes.LicenseAgreement.ToUpper().Equals(ex.ErrorCodes[0].Code.ToUpper()) ||
                    T360ErrorCodes.MobileAccess.ToUpper().Equals(ex.ErrorCodes[0].Code.ToUpper()))
                {
                    string msg = getMessages(ex);
                    ShowErrorMessage(msg, Constants.LoginFailed);
                    Navigator.Navigate(Destination.LoginView, ExistingViewBehavior.Remove, new object[] {});
                }
                else
                {
                    string msg = getMessages(ex);
                    ShowErrorMessage(msg, Constants.LoginFailed);
                }
            }
        }

        public async void OnDecline()
        {
            try
            {
                IsBusy = true;
                await ServiceInvoker.Instance.LogOut();
                IsBusy = false;
                Navigator.Navigate(Destination.LoginView, ExistingViewBehavior.Remove, new object[] {});
            }
            catch (T360Exception ex)
            {
                string msg = getMessages(ex);
                ShowErrorMessage(msg, Constants.DisclaimerFailed);
            }
        }

        public override async Task LoadData(params object[] parameters)
        {
            if (parameters != null)
            {
                if (parameters[0] != null)
                {
                    LoginInfo = (LoginInfo)parameters[0];
                }
            }
        }
    }
}