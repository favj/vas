/*
* @(#)FBFriend.cs
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

namespace Com.VizApp.VizApp.Service.Api
{
    public class FBFriend
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string ProfileURL { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Selected { get; set; }
    }
}
