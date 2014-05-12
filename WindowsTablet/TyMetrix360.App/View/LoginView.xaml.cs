/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using TyMetrix360.App.Common;
using TyMetrix360.App.ViewModel;
using TyMetrix360.Core.ViewBase;

namespace TyMetrix360.App.View
{
    public sealed partial class LoginView : ViewCore, ILoginView
    {
        public LoginView()
        {
            this.InitializeComponent();

            Application.Current.Suspending += Current_Suspending;
            Application.Current.Resuming += Current_Resuming;
        }

        void Current_Resuming(object sender, object e)
        {
            GetUsernameTextBox().Text = (string)ApplicationData.Current.LocalSettings.Values[Constants.UserNameKey];
        }

        void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            GetPasswordTextBox().Password = string.Empty;
            integratedBox.Text = string.Empty;
            integratedBoxSnapped.Text = string.Empty;
        }

        private void integratedBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            Clipboard.Clear();
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                var d = DataContext as ILoginViewModel;
                d.IntegratedLoginID = d.IntegratedLogin ? GetIntegratedIdTextBox().Text : null;
                d.OnLogin(GetPasswordTextBox().Password);
            }
        }

        private void PasswordBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            Clipboard.Clear();
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                var d = DataContext as ILoginViewModel;
                d.IntegratedLoginID = d.IntegratedLogin ? GetIntegratedIdTextBox().Text : null;
                d.OnLogin(GetPasswordTextBox().Password);
            }
        }

        private TextBox GetIntegratedIdTextBox()
        {
            if (ApplicationView.Value == ApplicationViewState.Snapped)
                return integratedBoxSnapped;
            return integratedBox;
        }

        private PasswordBox GetPasswordTextBox()
        {
            if (ApplicationView.Value == ApplicationViewState.Snapped)
                return PasswordBoxSnapped;
            return PasswordBox;
        }

        private TextBox GetUsernameTextBox()
        {
            if (ApplicationView.Value == ApplicationViewState.Snapped)
                return UsernameBoxSnapped;
            return UsernameBox;
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch tSwitch = (ToggleSwitch)e.OriginalSource;
            if (tSwitch.IsOn)
            {
                //parentGrid.RowDefinitions[3].Height = new GridLength(240, GridUnitType.Pixel);
                viewGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                //parentGrid.RowDefinitions[3].Height = new GridLength(200, GridUnitType.Pixel);
                viewGrid.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Pixel);
            }
        }

        private void onContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }

        private void onKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            Clipboard.Clear();
        }

        private void onKeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            Clipboard.Clear();
        }
    }
}
