/*
* @(#)FilterConfig.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using Com.VizApp.VizApp.Service.WebApi.Filters;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Com.VizApp.VizApp.Service.WebApi.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterHttpFilters(HttpFilterCollection filters)
        {
            filters.Add(new AppExceptionFilter());
            filters.Add(new AppAutheticationFilter());
            filters.Add(new AppAuthorizationFilter());
            filters.Add(new AppDataSanitizer());
        }
    }
}