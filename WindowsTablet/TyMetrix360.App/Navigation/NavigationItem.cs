/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;

namespace TyMetrix360.App.Navigation
{
    public class NavigationItem :INavigationItem
    {
        public NavigationItem(Type viewType, Type viewModelType, object[] parameters, string regionName)
        {
            ViewType = viewType;
            ViewModelType = viewModelType;
            Parameters = parameters;
            RegionName = regionName;
        }
        public Type ViewType { get; private set; }

        public Type ViewModelType { get; private set; }

        public object[] Parameters { get; set; }

        public string RegionName { get; private set; }
    }
}
