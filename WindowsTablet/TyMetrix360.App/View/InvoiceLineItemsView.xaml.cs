/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using TyMetrix360.Core.ViewBase;
using TyMetrix360.App.ViewModel;

namespace TyMetrix360.App.View
{
    public sealed partial class InvoiceLineItemsView : ViewCore, IInvoiceLineItemsView
    {
        public InvoiceLineItemsView()
        {
            this.InitializeComponent();
        }

        private void SummaryStuff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = sender as ListView;
            lv.SelectedIndex = -1;
        }

        protected override void OnWindowSizeChanged()
        {
            InvoiceLineItemsViewModel vm = (InvoiceLineItemsViewModel)DataContext;
            vm.SetAppBar();
        }
    }
}
