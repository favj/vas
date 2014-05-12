/*
* @(#)AppRequestContext.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using Com.VizApp.VizApp.Arch;
using System;
using System.Collections.Generic;

namespace Com.VizApp.Arch
{
    public class AppRequestContext
    {
        [ThreadStatic]
        private static AppRequestContext requestContext;

        public static AppRequestContext Instance
        {
            get
            {
                return requestContext = requestContext == null ? new AppRequestContext() : requestContext;
            }

            private set
            {
                requestContext = value;
            }
        }

        public static void ReleaseContext()
        {
            AppRequestContext.Instance = null;
        }

        public static void Init(FBUser FBUser, string ipAddress, string sessionId)
        {
            Instance.fbUser = FBUser;
            Instance.ipAddress = ipAddress;
            Instance.sessionId = sessionId;
        }

        private string ipAddress;
        private string sessionId;
        private FBUser fbUser;
        private string user;
        private bool isFBUser;

        public static string IPAddress
        {
            get
            {
                return Instance.ipAddress;
            }
            set
            {
                Instance.ipAddress = value;
            }
        }

        public static string SessionID
        {
            get
            {
                return Instance.sessionId;
            }
            set
            {
                Instance.sessionId = value;
            }
        }

        public static FBUser FBUser
        {
            get
            {
                return Instance.fbUser;
            }
            set
            {
                Instance.fbUser = value;
            }
        }

        public static string FileSeparator()
        {
            return "\\";
        }

        public static string ExtensionSeparator()
        {
            return ".";
        }
    }
}
