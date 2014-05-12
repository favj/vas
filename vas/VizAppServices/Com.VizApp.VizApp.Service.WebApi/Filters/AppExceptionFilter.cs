/*
* @(#)AppExceptionFilter.cs
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
using System.Net.Http;
using System.Net;
using Com.VizApp.Arch;
using Com.VizApp.Arch.Logging;

namespace Com.VizApp.VizApp.Service.WebApi.Filters
{
    public class AppExceptionFilter : ExceptionFilterAttribute
    {
        public const string Unhandled_Error = "UNHANDLED_ERROR";
        public override void OnException(HttpActionExecutedContext context)
        {
            AppException hbiEx = null;

            if (context.Exception is AppException)
            {
                hbiEx = context.Exception as AppException;
            }
            else
            {
                hbiEx = new AppException(Unhandled_Error, context.Exception);
            }
            Logger.Error(hbiEx.InnerException != null ? hbiEx.InnerException.ToString() : hbiEx.ToString());
            context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, hbiEx.Errors);
        }
    }
}