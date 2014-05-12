/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Collections.Generic;
using System.Resources;

namespace Tymetrix.T360.Mobile.Client.Common.Base.Util
{
    public enum CultureType
    {
        UIText,
        Message
    }

    public class CultureManager
    {
        private static volatile CultureManager cm;
        private static object syncRoot = new Object();

        private Dictionary<string, ResourceManager> cultures;

        private void Init()
        {
            cultures = new  Dictionary<string, ResourceManager>();
        }

        public void AddCulture(string type, ResourceManager rm)
        {
            cultures.Add(type, rm);
        }

        public ResourceManager GetCulture(string type)
        {
            return (ResourceManager)cultures[type];
        }

        public string GetResource(string type, string key)
        {
            return GetCulture(type).GetString(key);
        }

        public static CultureManager Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (cm == null)
                    {
                        cm = new CultureManager();
                        cm.Init();
                    }
                }
                return cm;
            }
        }
    }
}
