/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    [DataContract]
    public class ReasonCodeInputDetails
    {
        [DataMember(Name = "Action")]
        public string Action { get; set; }
    }

    [DataContract]
    public class ReasonCodeMultipleInput : ReasonCodeInputDetails
    {
        [DataMember(Name = "InvoiceId")]
        public string InvoiceId { get; set; }

        [DataMember(Name = "SelectedLineItemIds")]
        public List<string> SelectedLineItemIds { get; set; }

        [DataMember(Name = "SelectedIds")]
        public List<string> SelectedIds { get; set; }
    }
}
