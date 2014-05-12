/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Core;
using Microsoft.Phone.Shell;

namespace Tymetrix.T360.Mobile.Client.AppWP7.Faq
{
    public partial class Faq : BasePage
    {
        public Faq()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.ProgressBar.Show();
            Uri u = new Uri("Resources/html/Faqs.html", UriKind.Relative);
            faqBrowser.Navigate(u);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (PhoneApplicationService.Current.State.ContainsKey("sessionTimedout"))
            {
                PhoneApplicationService.Current.State.Remove("sessionTimedout");
                return;
            }
            base.OnNavigatedTo(e);
        }

        private void faqBrowser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (e.Value.StartsWith("Completed"))
            {
                this.ProgressBar.Hide();
            }
        }

        private void faqBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {
                var result = faqBrowser.InvokeScript("disableSelection");
                PageInProgress = false;
                this.ProgressBar.Hide();
            }
            catch (AppException ex)
            {
                ShowError(ex);
            }
        }

        private void faqBrowser_NavigationFailed(object sender, NavigationFailedEventArgs e)
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