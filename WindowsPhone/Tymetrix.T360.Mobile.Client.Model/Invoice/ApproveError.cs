/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class ApproveError
    {
        public string Header { get; set; }
        public string ApproveErrorTitle { get; set; }
        public List<ApproveErrorItem> ApproveErrorItems { get; set; }
    }
}
