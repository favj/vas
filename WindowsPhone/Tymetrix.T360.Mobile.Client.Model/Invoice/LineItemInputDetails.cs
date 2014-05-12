/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;
using System;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    [DataContract]
    public class LineItemInputDetails
    {
        public LineItemInputDetails(string invoiceId, string lineitemId)
        {
            this.InvoiceId = invoiceId;
            this.LineItemId = lineitemId;
        }

        [DataMember(Name = "InvoiceId")]
        public string InvoiceId { get; set; }

        [DataMember(Name = "LineItemId")]
        public string LineItemId { get; set; }
    }
}
