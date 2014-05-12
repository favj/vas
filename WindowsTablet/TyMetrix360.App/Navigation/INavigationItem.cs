/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;

namespace TyMetrix360.App.Navigation
{
    public interface INavigationItem
    {
        Type ViewType { get; }
        Type ViewModelType { get; }
        object[] Parameters { get; set; }
        string RegionName { get; }
    }
}
