/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using GalaSoft.MvvmLight.Messaging;
using TyMetrix360.App.CommandParameters;
using TyMetrix360.App.Common;
using TyMetrix360.App.Navigation;
using TyMetrix360.App.View;
using TyMetrix360.App.ViewModel;
using TyMetrix360.Core.Container;

using Windows.Data.Xml.Dom;
using Windows.UI.ApplicationSettings;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace TyMetrix360.App
{
    public sealed partial class ShellView : Page, IShellView
    {
        private Popup _childPopup;
        public Popup ChildPopup
        {
            get { return _childPopup; }
            set { _childPopup = value;}
        }

        public void UseSunGlasses(bool on)
        {
            SunGlasses.Visibility = on ? Visibility.Visible : Visibility.Collapsed;
        }

        //Live tile objects needd. 
        public TileTemplateType TileTemplate { get; set;}
        public XmlDocument TileXML { get; set; }
        public TileNotification TileNotifier { get; set; }
        public XmlNodeList TextNodes { get; set; }
        public XmlNodeList ImageNodes { get; set; }

        public ShellView()
        {
            this.InitializeComponent();
            this.ChildPopup = new Popup();
            var shellNavigationItem = NavigationFactory.GetNavigationItem(Destination.ShellView);
            this.DataContext = Container.ResolveViewModel(shellNavigationItem.ViewModelType);
            var resolution = Window.Current.Bounds;
            SunGlasses.Height = resolution.Height;
            SunGlasses.Width = resolution.Width;
            UseSunGlasses(false);
            SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;

            TileXML = TileUpdateManager.GetTemplateContent(TileTemplate);
            Window.Current.CoreWindow.KeyDown += KeyDowns;

        }

        private void KeyDowns(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            SendTimerReset();
        }

        void onCommandsRequested(SettingsPane settingsPane, SettingsPaneCommandsRequestedEventArgs eventArgs)
        {
            UICommandInvokedHandler handler = new UICommandInvokedHandler(onSettingsCommand);
            SettingsCommand privacyPolicyCommand = new SettingsCommand(Constants.PrivacyPolicyId, Constants.PrivacyPolicy, handler);
            eventArgs.Request.ApplicationCommands.Add(privacyPolicyCommand);
        }

        void onSettingsCommand(IUICommand command)
        {
            if (command.Id == Constants.PrivacyPolicyId)
            {
                ShellViewModel vm = (ShellViewModel)DataContext;
                vm.CallPrivacy();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested -= onCommandsRequested;
        }

       private void SendTimerReset()
       {
           Messenger.Default.Send<ResetTimer>(new ResetTimer());
       }

        public bool IsMain { get; set; }

        private void Page_KeyDown_1(object sender, KeyRoutedEventArgs e)
        {
            SendTimerReset();
        }

        private void Page_ManipulationStarted_1(object sender, ManipulationStartedRoutedEventArgs e)
        {
            SendTimerReset();
        }

        private void Page_PointerMoved_1(object sender, PointerRoutedEventArgs e)
        {
            SendTimerReset();
        }

        private void Page_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            SendTimerReset();
        }

        private void Page_GotFocus_1(object sender, RoutedEventArgs e)
        {
            SendTimerReset();
        }

        private void Page_LayoutUpdated_1(object sender, object e)
        {
            SendTimerReset();
        }

        private void Page_LostFocus_1(object sender, RoutedEventArgs e)
        {
            SendTimerReset();
        }

        private void Page_DragEnter_1(object sender, DragEventArgs e)
        {
            SendTimerReset();
        }

        private void Page_DragLeave_1(object sender, DragEventArgs e)
        {
            SendTimerReset();
        }

        private void Page_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {
            SendTimerReset();
        }

        private void Page_ManipulationStarting_1(object sender, ManipulationStartingRoutedEventArgs e)
        {
            SendTimerReset();
        }
    }
}
