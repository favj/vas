/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Microsoft.Phone.Shell;
using System.Windows;
using System.Windows.Navigation;

using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.Invoice;

namespace Tymetrix.T360.Mobile.Client.AppWP8.Invoice
{
    public partial class ViewNotePage : BasePage
    {
        public ViewNotePage()
        {
            InitializeComponent();
            this.Loaded += ViewNotePage_Loaded;
        }

        void ViewNotePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Source == Source.EXTERNAL)
            {
                this.Source = this.PreviousSource;
                return;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if ((bool)PhoneApplicationService.Current.State.ContainsKey("isTombStoned"))
            {
                base.OnNavigatedTo(e);
                return;
            }
            if (this.Source == Source.EXTERNAL)
            {
                base.OnNavigatedTo(e);
                return;
            }
            PrepareView();
            this.DataContext = Note;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (Constants.ExternalURI.Equals(e.Uri.ToString()))
            {
                this.PreviousSource = this.Source;
                this.Source = Source.EXTERNAL;
                return;
            }
            InvoiceCommonDetails commonDetails = e.Content as InvoiceCommonDetails;
            if (commonDetails != null)
            {
                commonDetails.Source = Source.BACK_TO_INVOICE_SUMMARY_FROM_NOTES;
            }
            LineItemSummary lineItemSummary = e.Content as LineItemSummary;
            if (lineItemSummary != null)
            {
                lineItemSummary.Source = Source.BACK_TO_LINE_ITEM_SUMMARY_FROM_NOTES;
            }
            base.OnNavigatedFrom(e);
        }

        private void PrepareView()
        {
            if (IsInvoice)
            {
                scrollableTextBlockInvoice.Text = Note.Description;
                invoiceGrid.Visibility = Visibility.Visible;
                lineitemGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                scrollableTextBlockLineitem.Text = Note.Description;
                invoiceGrid.Visibility = Visibility.Collapsed;
                lineitemGrid.Visibility = Visibility.Visible;
            }
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
            base.OnBackKeyPress(e);
        }

        public NotesData Note { get; set; }
        public bool IsInvoice { get; set; }
    }
}