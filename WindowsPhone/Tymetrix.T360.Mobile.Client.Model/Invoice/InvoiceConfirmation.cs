/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class InvoiceConfirmation
    {
        public string ConfirmationTitle { get; set; }
        public string TotalNetAmount { get; set; }
        public List<ConfirmationItem> ConfirmationItems { get; set; }
    }
}
