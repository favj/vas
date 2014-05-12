/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    [DataContract]
    public class InvoiceNotes
    {
        [DataMember(Name = "Creator")]
        public string Creator { get; set; }

        [DataMember(Name = "CreatedTime")]
        public string CreatedTime { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }
    }
}
