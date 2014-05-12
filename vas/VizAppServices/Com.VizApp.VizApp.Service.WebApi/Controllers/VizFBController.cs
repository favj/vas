/*
* @(#)VizFBController.cs
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
using Com.VizApp.Arch.Api;
using System.Web;

namespace Com.VizApp.VizApp.Service.WebApi.Controllers
{
    public class VizFBController : ApiController
    {
        [HttpPost]
        public Settings SaveFBDetails(FBData fbData)
        {
            IVizFBService service = ObjectFactory.Resolve<IVizFBService>();

            Settings settings = service.SaveFBDetails(fbData);
            var session = HttpContext.Current.Session;
            session["authinfo"] = fbData.User;
            return settings;
        }

        [HttpPost]
        public bool SaveFBFriends(FBFriends fbFriends)
        {
            IVizFBService service = ObjectFactory.Resolve<IVizFBService>();
            service.SaveFBFriends(fbFriends);
            return true;
        }
    }
}
