/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;
using Tymetrix.T360.Mobile.Client.Core;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class InvoiceErrorDetails
    {
        public string PageType { get; set; }
        public List<Error> ErrorDetails { get; set; }
        public string Header { get; set; }
        public List<InvoiceModel> InvoiceBasicDetails { get; set; }
    }
}
