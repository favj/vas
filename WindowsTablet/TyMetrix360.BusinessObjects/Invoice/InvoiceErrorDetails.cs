using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TyMetrix360.Core.Models;
using TyMetrix360.BusinessObjects.Common;

namespace TyMetrix360.BusinessObjects.Invoice
{
    public class InvoiceErrorDetails
    {
        public string PageType { get; set; }
        public List<Error> ErrorDetails { get; set; }
        public string Header { get; set; }
        public List<InvoiceListDisplayFields> InvoiceBasicDetails { get; set; }
    }
}
