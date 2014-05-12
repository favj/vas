/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class ApprovalInputDetails
    {
        [DataMember(Name = "InvoiceId")]
        public string InvoiceId { get; set; }

        [DataMember(Name = "ForceApprove")]
        public bool ForceApprove { get; set; }
    }
}
