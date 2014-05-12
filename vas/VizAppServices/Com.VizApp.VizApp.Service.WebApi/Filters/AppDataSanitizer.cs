/*
* @(#)AppDataSanitizer.cs
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
using System.Web.Http;
using System.Web.Http.Filters;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.VizApp.VizApp.Service.WebApi.Filters
{
    public class AppDataSanitizer : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            
            /* //Commenting SQL Injection and JSInjection. TODO: Get the login parameters as JSON objects.
             * 
            var queryStringCollection = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
            foreach (string param in queryStringCollection.AllKeys)
            {
                //validateSQLInjection
                if (!IsSqlInjectionFree(param + queryStringCollection[param]))
                {
                    
                    throw new AppException(System.Net.HttpStatusCode.BadRequest.ToString());
                    //actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }

                //validateJSInjection
                if (!IsJsInjectionFree(param + queryStringCollection[param]))
                {
                    
                    throw new AppException(System.Net.HttpStatusCode.BadRequest.ToString());
                    //actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }
                
            }
           */     
           
        }

        private bool ParameterValidator(string parameter, string[] array_split_item, string validationtype)
        {
            bool functionReturnValue = true;
            int pos = 0;
           
            foreach (string strItem in array_split_item)
            {
                if (validationtype == "Regex")
                {
                    Regex regex = new Regex(strItem);

                    if (regex.IsMatch(parameter.ToLower(), 0))
                        return functionReturnValue = false;
                }
                else if (validationtype == "string")
                {
                    pos = parameter.ToLower().IndexOf(strItem.ToLower()) + 1;

                    if (pos > 0)
                    {
                        return functionReturnValue = false;

                    }
                }
            }
            return functionReturnValue;
        }
        private bool IsSqlInjectionFree(string parameter)
        {
            //Create an array of invalid characters and words
            string[] array_split_item = new string[] { ";", "/*", "*/", "@@", "char", "nchar", "varchar", "nvarchar", "alter", "begin", "cast", "create", "cursor", "declare", "delete", "drop", "end", "exec", "execute", "fetch", "insert", "kill", "open", "select", "sys", "sysobjects", "syscolumns", "table", "update", "<script", "</script>", "xp_", "--", "â€˜", "#", "%", "&", "'", "(", ")", "/", "\\", ":", ";", "<", ">", "=", "[", "]", "?", "`", "|" };
            return ParameterValidator(parameter, array_split_item, "string");
 
        }

        private bool IsJsInjectionFree(string parameter)
        {
            //Create an array of invalid characters and words
            string[] array_split_item = new string[] { "eval\\((.*)\\)","[\\s]*javascript[\\s]*:[\\s]*" ,"<[\\s]*[/]*((?i)script(.*))[\\s]*>"};
            return ParameterValidator(parameter, array_split_item, "Regex");
        }
    }
}