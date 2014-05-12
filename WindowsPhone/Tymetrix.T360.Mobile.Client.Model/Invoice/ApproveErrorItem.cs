/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */


using System.Collections.Generic;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class ApproveErrorItem
    {
        public string Message { get; set; }
        public List<InvoiceNumber> InvoiceNumbers { get; set; }
    }

    public class InvoiceNumber
    {
        public string InvoiceNo { get; set; }
    }
}
