/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using TyMetrix360.Core.ViewModelBase;

namespace TyMetrix360.App.ViewModel
{
    public interface IInvoiceListViewModel : IViewModelCore
    {
        int SelectedInvoice { get; set; }
    }
}
