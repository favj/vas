/*
* @(#)VizSecurityController.cs
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
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Com.VizApp.VizApp.Service.Api;
using Com.VizApp.Arch;
using System.Web;
using Com.VizApp.Arch.Api;
using Com.VizApp.VizApp.Arch;

namespace Com.VizApp.VizApp.Service.WebApi.Controllers
{
    public class VizSecurityController : ApiController
    {
        [HttpPost]
        public Settings Login(Credentials creds)
        {
            IVizSecurityService service = ObjectFactory.Resolve<IVizSecurityService>();

            Settings settings = service.Login(creds);
            var session = HttpContext.Current.Session;
            session["authinfo"] = new FBUser { Email = creds.Email };
            return settings;
        }

        [HttpGet]
        public bool Logout()
        {
            HttpContext.Current.Session.Abandon();
            return true;
        }
    }
}
