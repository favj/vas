/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using TyMetrix360.App.Navigation;
using TyMetrix360.BusinessObjects.Invoice;

namespace TyMetrix360.App.CommandParameters
{
    class ReturnToPage
    {
        public InvoiceSummary Invoice { get; set; }
        public int LineItemId { get; set; }
        public INavigationItem PageItem { get; set; }
    }
}
