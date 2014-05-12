/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using TyMetrix360.Core.ViewBase;

namespace TyMetrix360.App.View
{
    public sealed partial class SupportView : ViewCore, ISupportView
    {
        public SupportView()
        {
            this.InitializeComponent();
        }

        private async void Button_Click_1(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var mailto = new Uri("mailto:T360Support@wolterskluwer.com");
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }
    }
}
