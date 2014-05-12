/*
* @(#)FBUser.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.VizApp.VizApp.TestClient.Model
{
    public class FBUser
    {
        public string id { get; set; }
        public List<FBEmployer> Work { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string TimeZone { get; set; }
        public bool Verified { get; set; }
        public string Locale { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string ProfileUrl { get; set; }
    }
}
