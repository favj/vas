/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.Invoice;

namespace Tymetrix.T360.Mobile.Client.AppWP7.Invoice
{
    public partial class NotesPage : BasePage
    {
        public NotesPage()
        {
            InitializeComponent();
            this.Loaded += NotesPage_Loaded;
        }

        void NotesPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Source == Source.EXTERNAL)
            {
                this.Source = this.PreviousSource;
                return;
            }
            PageInProgress = false;
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
            PrepareNotes();
            this.DataContext = new NotesList() { Notes = Input };
            base.OnNavigatedTo(e);
        }

        private void PrepareNotes()
        {
            if (IsInvoice)
            {
                foreach (NotesData note in Input)
                {
                    note.IsInvoice = Visibility.Visible;
                    note.IsLineItem = Visibility.Collapsed;
                }
            }
            else
            {
                foreach (NotesData note in Input)
                {
                    note.IsInvoice = Visibility.Collapsed;
                    note.IsLineItem = Visibility.Visible;
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (Constants.ExternalURI.Equals(e.Uri.ToString()))
            {
                this.PreviousSource = this.Source;
                this.Source = Source.EXTERNAL;
                return;
            }
            ViewNotePage viewNote = e.Content as ViewNotePage;
            if (viewNote != null)
            {
                viewNote.Note = ViewNoteInput;
                viewNote.IsInvoice = IsInvoice;
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

        private void lbNotes_SelectionChanged(object sender, SelectionChangedEventArgs se)
        {
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }

            ViewNoteInput = (NotesData)lbNotes.SelectedValue;

            Uri uri = new Uri("/Tymetrix.T360.Mobile.Client.AppWP7;component/Invoice/ViewNotePage.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }

        public NotesData ViewNoteInput { get; set; }
        public List<NotesData> Input { get; set; }
        public bool IsInvoice { get; set; }
    }
}