/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
namespace TyMetrix360.BusinessObjects.Common
{
    public class DashboardInfo : BusinessObjectCore
    {
        private int _invoiceCount;
        public int InvoiceCount 
        {
            get { return _invoiceCount; }
            set { SetProperty(ref _invoiceCount, value); }
        }
    }
}
