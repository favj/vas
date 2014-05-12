/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    [DataContract]
    public class AdjustInputDetails
    {
        [DataMember(Name = "LineItemIds")]
        public List<string> LineItemIds { get; set; }

        [DataMember(Name = "InvoiceId")]
        public string InvoiceId { get; set; }

        [DataMember(Name = "ReasonId")]
        public string ReasonId { get; set; }

        [DataMember(Name = "NarrativeText")]
        public string NarrativeText { get; set; }

        [DataMember(Name = "AdjustmentMode")]
        public string AdjustmentMode { get; set; }

        [DataMember(Name = "AdjustmentAmount")]
        public string AdjustmentAmount { get; set; }

        [DataMember(Name = "AdjustmentType")]
        public string AdjustmentType { get; set; }

        [DataMember(Name = "AdjustmentStyle")]
        public string AdjustmentStyle { get; set; }

        [DataMember(Name = "NetTotal")]
        public string NetTotal { get; set; }

        [DataMember(Name = "IsMultiSelectEnabled")]
        public bool IsMultiSelectEnabled { get; set; }
    }
}
