/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public enum InvoiceApprovalStatus
    {
        Warning,
        Approved,
        Rejected
    }

    public class InvoiceApprovalDetails
    {
        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }
    }
}
