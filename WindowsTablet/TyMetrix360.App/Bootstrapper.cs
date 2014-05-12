/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using TyMetrix360.App.Navigation;
using TyMetrix360.App.View;
using TyMetrix360.App.ViewModel;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core.Container;

namespace TyMetrix360.App
{
    public static class Bootstrapper
    {
        public static void Run(bool restoreState)
        {
            Init();
            Navigator.Navigate(Destination.LoginView);
        }

        public static void Init()
        {
            RegisterTypes();
        }

        private static void RegisterTypes()
        {
            Container.Clear();
            RegisterServices();
            RegisterViewModels();
            RegisterViews();
        }

        private static void RegisterServices()
        {
#if MOCK
            Container.Register<IService, MockService>(Lifetime.ContainerControlled);
#else
            //Container.Register<IService,Service>(Lifetime.ContainerControlled);
#endif
        }

        private static void RegisterViewModels()
        {
            Container.Register<IAdjustmentViewModel, AdjustmentViewModel>();
            Container.Register<IInvoiceLineItemsViewModel, InvoiceLineItemsViewModel>();
            Container.Register<IInvoiceLineItemViewModel, InvoiceLineItemViewModel>();
            Container.Register<IInvoiceListSummaryViewModel, InvoiceListSummaryViewModel>();
            Container.Register<IInvoiceListViewModel, InvoiceListViewModel>();
            Container.Register<ILoginViewModel, LoginViewModel>();
            Container.Register<IRejectionViewModel, RejectionViewModel>();          
            Container.Register<IShellViewModel, ShellViewModel>(Lifetime.ContainerControlled);
            Container.Register<ISupportViewModel, SupportViewModel>();
            Container.Register<IDashboardViewModel, DashboardViewModel>();
            Container.Register<ISettingsViewModel, SettingsViewModel>();
            Container.Register<IResetPasswordViewModel, ResetPasswordViewModel>();
            Container.Register<IDisclaimerViewModel, DisclaimerViewModel>();
            Container.Register<IFaqViewModel, FaqViewModel>();
        }

        private static void RegisterViews()
        {
            Container.Register<IAdjustmentView, AdjustmentView>();
            Container.Register<IInvoiceLineItemsView, InvoiceLineItemsView>();
            Container.Register<IInvoiceLineItemView, InvoiceLineItemView>();
            Container.Register<IInvoiceListSummaryView, InvoiceListSummaryView>();
            Container.Register<IInvoiceListView, InvoiceListView>();
            Container.Register<ILoginView, LoginView>();
            Container.Register<IRejectionView, RejectionView>();
            Container.Register<IShellView, ShellView>(Lifetime.ContainerControlled);
            Container.Register<ISupportView, SupportView>();
            Container.Register<IDashboardView, DashboardView>();
            Container.Register<ISettingsView, SettingsView>();
            Container.Register<IResetPasswordView, ResetPasswordView>();
            Container.Register<IDisclaimerView, DisclaimerView>();
            Container.Register<IFaqView, FaqView>();
        }
    }
}
