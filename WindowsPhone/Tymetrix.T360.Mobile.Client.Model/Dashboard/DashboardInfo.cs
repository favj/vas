/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;

namespace Tymetrix.T360.Mobile.Client.Model.Dashboard
{
    [DataContract]
    public class DashboardInfo
    {
        [DataMember(Name = "InvoiceCount")]
        public string InvoiceCount { get; set; }
    }
}
