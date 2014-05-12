/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;
using Tymetrix.T360.Mobile.Client.Model.Base;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class InvoiceMultiConfirmation : BaseModel
    {
        private List<InvoiceModel> selectedInvoiceList;
        public List<InvoiceModel> SelectedInvoiceList
        {
            get { return selectedInvoiceList; }
            set { SetProperty(ref selectedInvoiceList, value, "SelectedInvoiceList"); }
        }

        private string netAmount;
        public string NetAmount
        {
            get { return netAmount; }
            set { SetProperty(ref netAmount, value, "NetAmount"); }
        }
    }
}
