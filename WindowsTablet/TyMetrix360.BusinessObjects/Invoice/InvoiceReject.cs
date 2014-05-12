/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using TyMetrix360.BusinessObjects.Common;

namespace TyMetrix360.BusinessObjects.Invoice
{
    public class InvoiceReject :BusinessObjectCore
    {
        private int _invoiceId;
        public int InvoiceId
        {
            get { return _invoiceId; }
            set { SetProperty(ref _invoiceId, value); }
        }
        private string _narrativeText;
        public string NarrativeText
        {
            get { return _narrativeText; }
            set { SetProperty(ref _narrativeText, value); }
        }
        private int _reasonId;
        public int ReasonId
        {
            get { return _reasonId; }
            set { SetProperty(ref _reasonId, value); }
        }
    }
}
