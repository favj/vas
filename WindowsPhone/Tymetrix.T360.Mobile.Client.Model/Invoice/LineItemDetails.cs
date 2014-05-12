/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    [DataContract]
    public class LineItemDetails
    {
        [DataMember(Name = "Date")]
        public string Date { get; set; }

        [DataMember(Name = "TimeKeeper")]
        public string TimeKeeper { get; set; }

        [DataMember(Name = "NetAmount")]
        public string NetAmount { get; set; }

        [DataMember(Name = "NarrativeText")]
        public string NarrativeText { get; set; }

        [DataMember(Name = "VendorTask")]
        public string VendorTask { get; set; }

        [DataMember(Name = "VendorActivity")]
        public string VendorActivity { get; set; }

        [DataMember(Name = "VendorUnits")]
        public string VendorUnits { get; set; }

        [DataMember(Name = "VendorRate")]
        public string VendorRate { get; set; }

        [DataMember(Name = "VendorAdjustment")]
        public string VendorAdjustment { get; set; }

        [DataMember(Name = "VendorBilledTotal")]
        public string VendorBilledTotal { get; set; }

        [DataMember(Name = "ItpAdjustment")]
        public string ItpAdjustment { get; set; }

        [DataMember(Name = "ReviewerAdjustment")]
        public string ReviewerAdjustment { get; set; }

        [DataMember(Name = "NetTotal")]
        public string NetTotal { get; set; }

        [DataMember(Name = "AdjustmentList")]
        public List<LineItemAdjustmentDetails> AdjustmentsList { get; set; }

        [DataMember(Name = "NotesList")]
        public List<LineItemNotesDetails> NotesList { get; set; }

        [DataMember(Name = "Flagslist")]
        public List<LineItemFlagDetails> FlagsList { get; set; }


        [DataMember(Name = "FederalTaxList")]
        public List<LineItemTaxDetails> FederalTaxList { get; set; }

        [DataMember(Name = "LocalTaxList")]
        public List<LineItemTaxDetails> LocalTaxList { get; set; }
    }

    [DataContract]
    public class LineItemAdjustmentDetails
    {
        [DataMember(Name = "Owner")]
        public string Owner { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "Amount")]
        public string Amount { get; set; }
    }

    [DataContract]
    public class LineItemNotesDetails
    {
        [DataMember(Name = "Owner")]
        public string Owner { get; set; }

        [DataMember(Name = "Date")]
        public string Date { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }
    }

    [DataContract]
    public class LineItemFlagDetails
    {
        [DataMember(Name = "WarningInfo")]
        public string WarningInfo { get; set; }
    }
}
