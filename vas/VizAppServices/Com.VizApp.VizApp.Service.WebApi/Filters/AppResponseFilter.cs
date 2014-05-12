/*
* @(#)AppResponseFilter.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using System.Web.Http.Filters;
using Com.VizApp.Arch;

namespace Com.VizApp.VizApp.Service.WebApi.Filters
{
    public class AppResponseFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            /*if (AppRequestContext.HasActivities())
            {
                AppRequestContext.SaveActivities();
            }*/
            AppRequestContext.ReleaseContext();
        }
    }
}