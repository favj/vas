﻿/*
* @(#)FBData.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using Com.VizApp.VizApp.Arch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.VizApp.VizApp.Service.Api
{
    public class FBData
    {
        public FBUser User { get; set; }
        public FBFriends Friends { get; set; }
    }
}
