/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    [DataContract]
    public class ReasonCode
    {
        [DataMember(Name = "Id")]
        public string Id { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }
    }
}
