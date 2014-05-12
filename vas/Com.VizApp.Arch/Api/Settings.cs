/*
* @(#)Settings.cs
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

namespace Com.VizApp.Arch.Api
{
    public class Settings
    {
        public bool Vibrate { get; set; }
        public bool Sound { get; set; }
        public int LocationUpdate { get; set; }
        public bool AutoLogin { get; set; }
    }
}
