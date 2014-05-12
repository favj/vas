/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Windows.UI.Xaml.Controls.Primitives;

using TyMetrix360.Core.ViewBase;

namespace TyMetrix360.App.View
{
    public interface IShellView : IViewCore
    {
        Popup ChildPopup {get;set;}
        void UseSunGlasses(bool on);
    }
}
