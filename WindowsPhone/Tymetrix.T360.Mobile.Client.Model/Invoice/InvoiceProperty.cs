/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    [DataContract]
    public class InvoiceProperty
    {
        [DataMember(Name = "LabelText")]
        public string LabelText { get; set; }

        [DataMember(Name = "ValueText")]
        public string ValueText { get; set; }
    }
}
