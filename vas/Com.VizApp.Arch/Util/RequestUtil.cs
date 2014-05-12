/*
* @(#)RequestUtil.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

namespace Com.VizApp.Arch.Util
{
    public class RequestUtil
    {
        public static string GetAction(string localURL)
        {
            string contrlName = localURL.Contains("b2bsecurity") ? "b2bsecurity" : "b2b";

            string rem = localURL.Substring(localURL.IndexOf(contrlName) + contrlName.Length + 1);
            return rem.Contains("?") ? rem.Substring(0, rem.IndexOf("?")) : rem;
        }
    }
}
