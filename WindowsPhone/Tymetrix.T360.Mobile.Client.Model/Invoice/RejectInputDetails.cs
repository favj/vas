/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class RejectInput { }

    [DataContract]
    public class RejectInputDetails : RejectInput
    {
        [DataMember(Name = "InvoiceId")]
        public string InvoiceId { get; set; }

        [DataMember(Name = "LineItemIDs")]
        public List<string> lstLineItemId { get; set; }

        [DataMember(Name = "ReasonId")]
        public string ReasonId { get; set; }

        [DataMember(Name = "NarrativeText")]
        public string NarrativeText { get; set; }

        [DataMember(Name = "IsMultiSelectEnabled")]
        public bool IsMultiSelectEnabled { get; set; }
    }

    [DataContract]
    public class RejectInputMultipleInvoice : RejectInput
    {
        [DataMember(Name = "NarrativeText")]
        public string NarrativeText { get; set; }

        [DataMember(Name = "SelectedInvoiceIds")]
        public List<string> SelectedInvoiceIds { get; set; }

        [DataMember(Name = "ReasonId")]
        public string ReasonId { get; set; }
    }

    [DataContract]
    public class MultipleLineItemsInputDetails
    {
        public MultipleLineItemsInputDetails(string invoiceId)
        {
            this.InvoiceId = invoiceId;
        }

        [DataMember(Name = "InvoiceId")]
        public string InvoiceId { get; set; }

        [DataMember(Name = "SelectedLineItemIds")]
        public List<string> SelectedLineItemIds { get; set; }

        [DataMember(Name = "NetAmount")]
        public string NetAmount { get; set; }

        [DataMember(Name = "PositiveAdjustment")]
        public bool PositiveAdjustment { get; set; }

        [DataMember(Name = "CurrencySymbol")]
        public string CurrencySymbol { get; set; }
    }

    [DataContract]
    public class RejectMultipleInvoices
    {
        [DataMember(Name = "SelectedInvoiceIds")]
        public List<string> SelectedInvoiceIds { get; set; }

        [DataMember(Name = "NetAmount")]
        public string NetAmount { get; set; }
    }
}
