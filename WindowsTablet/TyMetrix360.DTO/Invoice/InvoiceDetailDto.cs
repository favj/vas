/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
namespace TyMetrix360.Dto.Invoice
{
    public class InvoiceDetailDto
    {
        public string Amount { get; set; }
        public string Date { get; set; }
        public string Flags { get; set; }
        public string LineItemId { get; set; }
        public string NarrativeText { get; set; }
        public string TimeKeeper { get; set; }
    }
}
