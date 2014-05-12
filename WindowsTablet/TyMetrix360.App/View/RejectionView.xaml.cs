/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Windows.ApplicationModel.DataTransfer;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Navigation;

using TyMetrix360.App.Helper;
using TyMetrix360.App.Navigation;
using TyMetrix360.Core.ViewBase;

namespace TyMetrix360.App.View
{
    public sealed partial class RejectionView : ViewCore, IRejectionView
    {
        private InputPaneHelper inputPaneHelper;
        public RejectionView()
        {
            this.InitializeComponent();
            inputPaneHelper = new InputPaneHelper();
            inputPaneHelper.SubscribeToKeyboard(true);
            inputPaneHelper.AddShowingHandler(narrativeText, new InputPaneShowingHandler(CustomKeyboardHandler));
            inputPaneHelper.SetHidingHandler(new InputPaneHidingHandler(InputPaneHiding));
        }
        private void CustomKeyboardHandler(object sender, InputPaneVisibilityEventArgs e)
        {
            Navigator.KeyboardOpen();
        }
        private void InputPaneHiding(InputPane sender, InputPaneVisibilityEventArgs e)
        {
            Navigator.KeyboardClosed();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            inputPaneHelper.SubscribeToKeyboard(false);
            inputPaneHelper.RemoveShowingHandler(narrativeText);
            inputPaneHelper.SetHidingHandler(null);
        }

        public bool IsMain { get; set; }

        private void ListView_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (ApplicationView.Value == ApplicationViewState.Snapped)
            {
                ApplicationView.TryUnsnap();
            }
        }

        private void narrativeText_ContextMenuOpening(object sender, Windows.UI.Xaml.Controls.ContextMenuEventArgs e)
        {
            e.Handled = true;
        }

        private void narrativeText_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            Clipboard.Clear();
        }

        private void narrativeText_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            Clipboard.Clear();
        }
    }
}
