﻿/*
* @(#)HomeController.cs
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
using System.Web;
using System.Web.Mvc;

namespace Com.VizApp.VizApp.Service.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
