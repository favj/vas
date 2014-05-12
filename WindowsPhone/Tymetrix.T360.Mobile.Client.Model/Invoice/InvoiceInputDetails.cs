/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    [DataContract]
    public class InvoiceInputDetails
    {
        public InvoiceInputDetails(string selectedInvoiceId)
        {
            this.InvoiceId = selectedInvoiceId;
        }

        [DataMember(Name = "InvoiceId")]
        public string InvoiceId { get; set; }
    }
}
