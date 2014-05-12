/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Windows.UI.Xaml.Controls;

using TyMetrix360.App.ViewModel;

namespace TyMetrix360.App.View
{
    public sealed partial class TYAppBarView : UserControl, ITYAppBarView
    {
        public TYAppBarView()
        {
            DataContext = new TYAppBarViewModel();
        }
        public Button AdjBut 
        {
            get { return AdjustButton; }
            set { AdjustButton = value; }
        }
        private bool _isMain;
        public bool IsMain
        {
            get { return _isMain; }
            set { _isMain = value; }
        }
    }
}
