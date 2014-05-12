/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;

using TyMetrix360.App.Common;
using TyMetrix360.App.View;
using TyMetrix360.App.ViewModel;

namespace TyMetrix360.App.Navigation
{
    public static class NavigationFactory
    {
        private static readonly Dictionary<Destination, NavigationItem> Destinations = new Dictionary<Destination, NavigationItem>
        {
            { Destination.AdjustmentView, new NavigationItem(typeof(IAdjustmentView), typeof(IAdjustmentViewModel), null, Constants.MainRegionName) },
            { Destination.InvoiceListView, new NavigationItem(typeof(IInvoiceListView), typeof(IInvoiceListViewModel), null, Constants.MainRegionName) },
            { Destination.InvoiceLineItemView, new NavigationItem(typeof(IInvoiceLineItemView), typeof(IInvoiceLineItemsViewModel), null, Constants.MainRegionName) },
            { Destination.InvoiceLineItemsView, new NavigationItem(typeof(IInvoiceLineItemsView), typeof(IInvoiceLineItemsViewModel), null , Constants.MainRegionName) },
            { Destination.InvoiceSummaryView, new NavigationItem(typeof(IInvoiceListSummaryView), typeof(IInvoiceListViewModel), null, Constants.MainRegionName) },      
            { Destination.LoginView, new NavigationItem(typeof(ILoginView), typeof(ILoginViewModel), null, Constants.MainRegionName)  },
            { Destination.RejectionView, new NavigationItem(typeof(IRejectionView), typeof(IRejectionViewModel), null, Constants.MainRegionName) },
            { Destination.ShellView, new NavigationItem(typeof(IShellView), typeof(IShellViewModel),null, Constants.MainRegionName) },
            { Destination.SupportView, new NavigationItem(typeof(ISupportView), typeof(ISupportViewModel),null, Constants.MainRegionName) },
            { Destination.DashboardView, new NavigationItem(typeof(IDashboardView), typeof(IDashboardViewModel), null, Constants.MainRegionName) },
            { Destination.SettingsView, new NavigationItem(typeof(ISettingsView), typeof(ISettingsViewModel), null, Constants.MainRegionName) },
            { Destination.ResetPasswordView, new NavigationItem(typeof(IResetPasswordView), typeof(IResetPasswordViewModel), null, Constants.MainRegionName) },
            { Destination.DisclaimerView, new NavigationItem(typeof(IDisclaimerView), typeof(IDisclaimerViewModel), null, Constants.MainRegionName) },
            { Destination.FaqView, new NavigationItem(typeof(IFaqView), typeof(IFaqViewModel), null, Constants.MainRegionName) }
        };

        public static INavigationItem GetNavigationItem(Destination destination)
        {
            return Destinations[destination];
        }
    }
}
