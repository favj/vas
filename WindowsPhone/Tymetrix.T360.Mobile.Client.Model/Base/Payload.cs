/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;

namespace Tymetrix.T360.Mobile.Client.Model.Base
{
    [DataContract]
    public class Payload
    {
        [DataMember(Name = "Content")]
        public string Content { get; set; }
    }
}
