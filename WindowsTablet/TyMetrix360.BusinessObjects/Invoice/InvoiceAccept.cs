/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using TyMetrix360.BusinessObjects.Common;

namespace TyMetrix360.BusinessObjects.Invoice
{
    public class InvoiceAccept : BusinessObjectCore
    {
        private int _invoiceId;
        public int InvoiceId
        {
            get { return _invoiceId; }
            set { SetProperty(ref _invoiceId, value); }
        }

        private bool _forceApprove;
        public bool ForceApprove
        {
            get { return _forceApprove; }
            set { SetProperty(ref _forceApprove, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
        private string _status;
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }
    }
}
