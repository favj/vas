/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    [DataContract]
    public class LineItemTaxDetails
    {
        [DataMember(Name = "TaxJurisdictionCode")]
        public string TaxJurisdictionCode { get; set; }

        [DataMember(Name = "TaxTypeCode")]
        public string TaxTypeCode { get; set; }

        [DataMember(Name = "TaxRate")]
        public string TaxRate { get; set; }
    }
}
