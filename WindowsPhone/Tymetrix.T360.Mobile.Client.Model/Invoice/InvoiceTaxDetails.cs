/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    /// <summary>
    /// InvoiceTaxDetails class contains the Tax Details Model for the Invoice
    /// </summary>
    [DataContract]
    public class InvoiceTaxDetails
    {
        [DataMember(Name = "TaxJurisdictionCode")]
        public string TaxJurisdictionCode { get; set; }

        [DataMember(Name = "TaxTypeCode")]
        public string TaxTypeCode { get; set; }

        [DataMember(Name = "TaxRate")]
        public string TaxRate { get; set; }

        [DataMember(Name = "TaxableAmount")]
        public string TaxableAmount { get; set; }

        [DataMember(Name = "TaxAmount")]
        public string TaxAmount { get; set; }
    }
}
