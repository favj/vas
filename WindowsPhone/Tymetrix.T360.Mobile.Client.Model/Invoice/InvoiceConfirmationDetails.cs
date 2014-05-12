/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class InvoiceConfirmationDetails
    {
        public string NetTotal { get; set; }
        public List<InvoiceModel> SelectedInvoices { get; set; }
        public List<string> SelectedInvoiceIds { get; set; }
        public InvoicePermissions Permissions { get; set; }
        public List<ReasonCode> Reasons { get; set; }
        public string CurrencySymbol { get; set; }
    }
}
