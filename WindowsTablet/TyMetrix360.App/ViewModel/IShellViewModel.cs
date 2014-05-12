/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Windows.UI.Xaml;

using TyMetrix360.Core.ViewModelBase;
using TyMetrix360.BusinessObjects.Invoice;

namespace TyMetrix360.App.ViewModel
{
    public interface IShellViewModel : IViewModelCore
    {
        Visibility IsMain { get; set; }
        InvoiceSummary Invoice { get; set; }
        void RefreshAppBar(IViewModelCore viewModelCore);
    }
}
