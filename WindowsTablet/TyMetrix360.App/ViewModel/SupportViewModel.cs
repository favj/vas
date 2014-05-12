/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.ObjectModel;
using TyMetrix360.App.Common;
using TyMetrix360.App.Navigation;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.BusinessObjects.Invoice;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core;
using TyMetrix360.Core.Container;
using TyMetrix360.Core.Models;
using TyMetrix360.Core.ViewModelBase;
using Windows.UI.Xaml;

namespace TyMetrix360.App.ViewModel
{
    public class SupportViewModel : ViewModelCore, ISupportViewModel
    {
        private ObservableCollection<InvoiceSummaryViewItem> _invoiceSummaryViewItemList;
        public ObservableCollection<InvoiceSummaryViewItem> InvoiceSummaryViewItemList
        {
            get { return _invoiceSummaryViewItemList; }
            set { SetProperty(ref _invoiceSummaryViewItemList, value); }
        }
        private string _explanation;
        public string Explanation
        {
            get { return _explanation; }
            set { SetProperty(ref _explanation, value); }
        }
        private string _versionHeader;
        public string VersionHeader
        {
            get { return _versionHeader; }
            set { SetProperty(ref _versionHeader, value); }
        }
        private string _version;
        public string Version
        {
            get { return _version; }
            set { SetProperty(ref _version, value); }
        }

        public SupportViewModel()
        {
            InitializeSummary();
            GoBackCommand = new RelayCommand(e => GoBackToPage());
        }

        private void GoBackToPage()
        {
            try
            {
                if (!ServiceInvoker.Instance.IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);

                Navigator.Navigate(Destination.DashboardView, ExistingViewBehavior.Remove, new object[] { null });
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.SupportFailed);
            }
        }
        private IRelayCommand _goBackCommand;
        public IRelayCommand GoBackCommand
        {
            get { return _goBackCommand; }
            set { SetProperty(ref _goBackCommand, value); }
        }
        private string _loggedUserName;
        public string LoggedUserName
        {
            get { return Vault.AES_Decrypt(_loggedUserName); }
            set { SetProperty(ref _loggedUserName, Vault.AES_Encrypt(value)); }
        }
        private void InitializeSummary()
        {
            LoggedUserName = UserPreference.Instance.CurrentUserName;
            Explanation = "If the Client Network has the Global Service Coverage, Service begins at 8:00 PM ET Sunday through Friday 8:00 PM ET";
            VersionHeader = "App Version";
            Version = "1.1";
        }
    }
}
