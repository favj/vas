/*
* @(#)WebApiConfig.cs
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
using System.Web.Http;

namespace Com.VizApp.VizApp.Service.WebApi.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            System.Web.Routing.RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "vizapp/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            ).RouteHandler = new SessionRouteHandler();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
