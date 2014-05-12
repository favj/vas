/*
* @(#)AppAutheticationFilter.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using Com.VizApp.Arch;
using Com.VizApp.Arch.Util;
using Com.VizApp.VizApp.Arch;
using System.DirectoryServices.AccountManagement;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.SessionState;

namespace Com.VizApp.VizApp.Service.WebApi.Filters
{
    public class AppAutheticationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            var session = HttpContext.Current.Session;
            string ipaddress = string.Empty;
            if (actionContext.Request.Properties.ContainsKey("MS_HttpContext"))
            {
                ipaddress = ((HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (actionContext.Request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)actionContext.Request.Properties[RemoteEndpointMessageProperty.Name];
                ipaddress = prop.Address;
            }

            if (IsPreLoginRequest(actionContext))
            {
                return;
            }

            if (session["authinfo"] == null)
            {
                string localURL = actionContext.Request.RequestUri.LocalPath.ToLower();
                string method = RequestUtil.GetAction(localURL);
                if (!localURL.ToLower().EndsWith("SaveFBDetails".ToLower()) &&
                    !localURL.ToLower().EndsWith("Register".ToLower()) &&
                    !localURL.ToLower().EndsWith("Login".ToLower()))
                {
                    throw new AppException(ErrorCode.INVALID_SESSION);
                }
            }
            else
            {
                ResetAppRequestContext(session, ipaddress);
            }
        }

        private void ResetAppRequestContext(HttpSessionState session, string ipaddress)
        {
            FBUser User = (FBUser)session["authinfo"];
            AppRequestContext.FBUser = User;
            AppRequestContext.SessionID = session.SessionID;
            AppRequestContext.IPAddress = ipaddress;
        }

        private bool IsPreLoginRequest(HttpActionContext actionContext)
        {
            /*string localURL = actionContext.Request.RequestUri.LocalPath.ToLower();

            //TODO define the below strings as constants
            if (!localURL.Contains("b2bsecurity"))
            {
                return false;
            }
            
            string method = RequestUtil.GetAction(localURL);
            switch (method)
            {
                case "getconfig": return true;
                case "approveclient": return true;
            }*/

            return false;
        }
    }
}