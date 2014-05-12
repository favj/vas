/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    [DataContract]
    public class ReviewRouteDetails
    {
        [DataMember(Name = "ReviewerName")]
        public string ReviewerName { get; set; }

        [DataMember(Name = "ReviewStatus")]
        public string ReviewStatus { get; set; }
    }
}
