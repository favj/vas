/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;

using TyMetrix360.Dto.Common;

namespace TyMetrix360.Dto.Invoice
{
    public class InvoiceSummaryDto
    {
        public string BilledAmount { get; set; }
        public string BilledExpenses { get; set; }
        public string BilledFees { get; set; }
        public string BillingPeriod { get; set; }
        public List<TyPropertyBaseDto> CommonProperties { get; set; }
        public string CompanyName { get; set; }
        public string Credit { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyType { get; set; }
        public List<FlagDetailsDto> Flags { get; set; }
        public string GrossAmount { get; set; }
        public string InvoiceDate { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string ItpAdjustment { get; set; }
        public List<BaseTaxItemDto> TaxList { get; set; }
        public string MatterName { get; set; }
        public string MatterNumber { get; set; }
        public string NetAmount { get; set; }
        public string NetTotal { get; set; }
        public List<NoteDto> Notes { get; set; }
        public PermissionsDto Permissions { get; set; }
        public string ProcessingDiscount { get; set; }
        public List<TyPropertyBaseDto> Properties { get; set; }
        public List<ReviewRouteList> ReviewRouteList { get; set; }
        public string ReviewerAdjustment { get; set; }
        public string Status { get; set; }
        public string SubTotal { get; set; }
        public string Tax { get; set; }
        public string TaxCredit { get; set; }
        public string TotalBilledAmount { get; set; }
        public string TotalWithCredit { get; set; }
        public string VendorAdjustment { get; set; }
    }
}
