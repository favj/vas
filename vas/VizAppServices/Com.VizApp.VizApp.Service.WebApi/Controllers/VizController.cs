/*
* @(#)VizController.cs
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
using Com.VizApp.Arch.Api;
using Com.VizApp.Arch;
using Com.VizApp.Arch.Logging;
using Com.VizApp.Arch.Util;
using Com.VizApp.VizApp.Arch;

namespace Com.VizApp.VizApp.Service.WebApi.Controllers
{
    public class VizController : ApiController
    {
        [HttpGet]
        public Settings GetSettings()
        {
            IVizService service = ObjectFactory.Resolve<IVizService>();
            return service.GetSettings();
        }

        [HttpPost]
        public Settings UpdateSettings(Settings setting)
        {
            Logger.Debug(JsonUtil.Serialize(setting));
            IVizService service = ObjectFactory.Resolve<IVizService>();
            return service.UpdateSettings(setting);
        }

        [HttpPost]
        public bool UpdateUserLocation(Location location)
        {
            IVizService service = ObjectFactory.Resolve<IVizService>();
            return service.UpdateUserLocation(location);
        }

        [HttpPost]
        public FBUser Register(FBUser user)
        {
            IVizService service = ObjectFactory.Resolve<IVizService>();
            return service.Register(user);
        }
    }
}
