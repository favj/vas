/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    [DataContract]
    public class LineItemListDetails
    {
        [DataMember(Name = "LineItemId")]
        public string LineItemId { get; set; }

        [DataMember(Name = "Date")]
        public string Date { get; set; }

        [DataMember(Name = "Flags")]
        public string Flags { get; set; }

        [DataMember(Name = "TimeKeeper")]
        public string TimeKeeper { get; set; }

        [DataMember(Name = "Amount")]
        public string Amount { get; set; }

        [DataMember(Name = "NarrativeText")]
        public string NarrativeText { get; set; }
    }
}
