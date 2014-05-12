/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class LineItemConfirmationDetails
    {
        public string NetAmount { get; set;}
        public List<LineItem> SelectedLineItems { get; set; }
        public List<string> SelectedLineItemIds { get; set; }
        public List<ReasonCode> Reasons { get; set; }
        public string CurrencySymbol { get; set; }
        public string InvoiceId { get; set; }
        public bool PositiveAdjustment { get; set; }
        public InvoiceSummary InvoiceDetails { get; set; }
    }
}
