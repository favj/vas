/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Tymetrix.T360.Mobile.Client.Model.ResetPassword
{
    [DataContract]
    public class Credential
    {
        private static volatile Credential user;
        private static object syncRoot = new Object();

        [DataMember(Name = "Password")]
        public string Password { get; set; }

        [DataMember(Name = "UseLastPassword")]
        public bool UseLastPassword { get; set; }

        public string ConfirmPassword { get; set; }

        public bool ShowKeepCurrentPassword { get; set; }

        public List<string> Rules { get; set; }

        public static Credential Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (user == null)
                    {
                        user = new Credential();
                    }
                }
                return user;
            }
        }

        public void Clear()
        {
            user = null;
        }
    }
}
