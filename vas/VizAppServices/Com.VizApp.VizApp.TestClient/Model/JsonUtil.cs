/*
* @(#)JsonUtil.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

namespace Com.VizApp.VizApp.TestClient
{
    public class JsonUtil
    {
        public static string Serialize(object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }

        public static object Deserialize(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<object>(json);
        }
    }
}
