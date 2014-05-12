/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;

using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;

namespace Tymetrix.T360.Mobile.Client.AppWP8.Support
{
    public partial class Support : BasePage
    {
        public Support()
        {
            InitializeComponent();
        }
        
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.ProgressBar.Show();
            appVersionLabel.Text = "App Version " + ServiceInvoker.GetVersionNumber();
            Uri u = new Uri("Resources\\html\\Support.html", UriKind.Relative);
            supportBrowser.Navigate(u);
        }

        private void supportBrowser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                if (e.Value.StartsWith("Completed"))
                {
                    this.ProgressBar.Hide();
                    PageInProgress = false;
                }
                if (e.Value.StartsWith("mailto:"))
                {
                    new EmailComposeTask
                    {
                        To = e.Value.ToLower().Replace("mailto:", string.Empty)
                    }.Show();
                }
                if (e.Value.StartsWith("tel:"))
                {
                    new PhoneCallTask
                    {
                        PhoneNumber = e.Value.ToLower().Replace("tel:", string.Empty)
                    }.Show();
                }
            }
        }

        private void supportBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {
                var result = supportBrowser.InvokeScript("constructSupport", UserData.Instance.MemberName);
            }
            catch (AppException ex)
            {
                ShowError(ex);
            }
        }

        private void supportBrowser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Debug.WriteLine(e.Exception);
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