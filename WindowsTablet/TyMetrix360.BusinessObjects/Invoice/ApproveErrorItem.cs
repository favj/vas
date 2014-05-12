using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TyMetrix360.BusinessObjects.Invoice
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
