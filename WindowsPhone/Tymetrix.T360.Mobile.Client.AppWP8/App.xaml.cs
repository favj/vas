/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Xml.Linq;

using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Core;

namespace Tymetrix.T360.Mobile.Client.AppWP8
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }
        private bool reset;

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = false;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            //Uri nUri = new Uri("/Base/View/LogOn.xaml", UriKind.Relative);
            //((App)Application.Current).RootFrame.Navigate(nUri);
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (!e.IsApplicationInstancePreserved)
            {
                // being activated after TombStoned
                // Do nothing
                // BasePage.OnNavigatedTo will redirect to login screen
                PhoneApplicationService.Current.State["isTombStoned"] = true;
                return;
            }

            PhoneApplicationService.Current.State["isFromActivated"] = true;

            // activate from Dormant - validate for idle duration
            if (PhoneApplicationService.Current.State.ContainsKey("lastActivityTime") && !string.IsNullOrEmpty(PhoneApplicationService.Current.State["lastActivityTime"].ToString()))
            {
                TimeSpan idleTime = DateTime.Now.Subtract(Convert.ToDateTime((PhoneApplicationService.Current.State["lastActivityTime"])));
                if (idleTime.TotalSeconds >= 600)
                {
                    BasePage.LogOff();
                    throw new QuitException();
                }
            }
            if (UserData.Instance.IsAuthenticated)
            {
                UserActivity.Instance.ResetTimer();
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            //UserActivity.Instance.StopTimer();
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is QuitException)
            {
                PhoneApplicationService.Current.State["sessionTimedout"] = true;
                e.Handled = true;
                int count = ((App)Application.Current).RootFrame.BackStack.Reverse().Count();
                for (int i = 0; i < count - 1; i++)
                {
                    ((App)Application.Current).RootFrame.RemoveBackEntry();
                }
                //if (((App)Application.Current).RootFrame.CanGoBack)
                //{
                //    ((App)Application.Current).RootFrame.GoBack();
                //}
                return;
            }

            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
            e.Handled = true;
            ErrorPage.Exception = e.ExceptionObject;
            (RootVisual as Microsoft.Phone.Controls.PhoneApplicationFrame).Source = new Uri("/ErrorPage.xaml", UriKind.Relative);
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            //RootFrame = new PhoneApplicationFrame();
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            RootFrame.Navigating += RootFrame_Navigating;
            RootFrame.Navigated += RootFrame_Navigated;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;

            EnablePushNotification();
            InitCulture();
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(ServiceInvoker.InstallationId))
            {
                CopyFiles();
            }

            XDocument loadedData = XDocument.Load("config.xml");
            ServiceInvoker.ServiceHost = loadedData.Root.Value.Trim();

            Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);
        }

        void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (reset && e.IsCancelable && e.Uri.OriginalString == "/Base/View/LogOn.xaml")
            {
                e.Cancel = true;
                reset = false;
            }
        }

        void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            reset = e.NavigationMode == NavigationMode.Reset;
        }


        private void EnablePushNotification()
        {
            NotificationService notificationService = new NotificationService("T360MobileClient");
            notificationService.Subscribe();
            notificationService.RawNotificationReceived += new EventHandler<RawNotificationRecievedEventArgs>(notificationService_RawNotificationReceived);
        }

        void notificationService_RawNotificationReceived(object sender, RawNotificationRecievedEventArgs e)
        {
            ShellTile firstTile = ShellTile.ActiveTiles.First();
            var newData = new StandardTileData()
            {
                Title = "TyMetrix 360",
                BackgroundImage = new Uri("Background.png", UriKind.Relative),
                Count = Convert.ToInt16(e.Message),
                BackBackgroundImage = new Uri("Background.png", UriKind.Relative),
            };

            // Update the default tile
            firstTile.Update(newData);
        }

        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            if (!UserData.Instance.IsAuthenticated) return;
            UserActivity.Instance.ResetTimer();
        }

        private void InitCulture()
        {
            CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            ResourceManager rm = new ResourceManager("Tymetrix.T360.Mobile.Client.AppWP8.Resources.Culture.Messages", Assembly.GetExecutingAssembly());
            CultureManager.Instance.AddCulture(CultureType.Message.ToString(), rm);
        }

        private void CopyFiles()
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                //-----------------------------------------------------------
                // Must copy all html, css and js files in the ISO 
                //-----------------------------------------------------------
                if (!store.DirectoryExists("Resources")) store.CreateDirectory("Resources");
                if (!store.DirectoryExists("Resources\\html")) store.CreateDirectory("Resources\\html");
                if (!store.DirectoryExists("Resources\\js")) store.CreateDirectory("Resources\\js");
                if (!store.DirectoryExists("Resources\\css")) store.CreateDirectory("Resources\\css");
                if (!store.DirectoryExists("Resources\\images")) store.CreateDirectory("Resources\\images");

                CopyToIsolatedStorage("Resources\\html\\Support.html", store);
                UpdateResourcePath("Resources\\html\\Support.html", store);

                CopyToIsolatedStorage("Resources\\html\\Faqs.html", store);
                UpdateResourcePath("Resources\\html\\Faqs.html", store);

                CopyToIsolatedStorage("Resources\\html\\Disclaimer.html", store);
                UpdateResourcePath("Resources\\html\\Disclaimer.html", store);

                CopyToIsolatedStorage("Resources\\css\\windows.css", store);

                CopyToIsolatedStorage("Resources\\js\\common.js", store);
                CopyToIsolatedStorage("Resources\\js\\windows.js", store);
                CopyToIsolatedStorage("Resources\\js\\accordion.js", store);

                CopyToIsolatedStorage("Resources\\Images\\appbackground.png", store);
                CopyToIsolatedStorage("Resources\\Images\\background.png", store);
                CopyToIsolatedStorage("Resources\\Images\\Dark_background.png", store);

                CopyToIsolatedStorage("Resources\\Images\\ActionItem.png", store);
                CopyToIsolatedStorage("Resources\\Images\\adjust.png", store);
                CopyToIsolatedStorage("Resources\\Images\\Approve.png", store);

                CopyToIsolatedStorage("Resources\\Images\\arrow_left.png", store);
                CopyToIsolatedStorage("Resources\\Images\\arrow_right.png", store);
                CopyToIsolatedStorage("Resources\\Images\\arrowclose.png", store);
                CopyToIsolatedStorage("Resources\\Images\\arrowopen.png", store);

                CopyToIsolatedStorage("Resources\\Images\\Checked.png", store);
                CopyToIsolatedStorage("Resources\\Images\\faq_icon.png", store);

                CopyToIsolatedStorage("Resources\\Images\\flag-icon-red.png", store);
                CopyToIsolatedStorage("Resources\\Images\\Flag.png", store);
                CopyToIsolatedStorage("Resources\\Images\\invoice_icon.png", store);
                CopyToIsolatedStorage("Resources\\Images\\LeftArrow.png", store);

                CopyToIsolatedStorage("Resources\\Images\\lineitem_icon.png", store);

                CopyToIsolatedStorage("Resources\\Images\\login_button.png", store);

                CopyToIsolatedStorage("Resources\\Images\\logo.png", store);
                CopyToIsolatedStorage("Resources\\Images\\logoSupport.png", store);
                CopyToIsolatedStorage("Resources\\Images\\logout_icon.png", store);

                CopyToIsolatedStorage("Resources\\Images\\MultiSelect.png", store);
                CopyToIsolatedStorage("Resources\\Images\\Reject.png", store);

                CopyToIsolatedStorage("Resources\\Images\\RightArrow.png", store);
                CopyToIsolatedStorage("Resources\\Images\\Search.png", store);

                CopyToIsolatedStorage("Resources\\Images\\settings_icon.png", store);
                CopyToIsolatedStorage("Resources\\Images\\skip.png", store);

                CopyToIsolatedStorage("Resources\\Images\\summary_icon.png", store);
                CopyToIsolatedStorage("Resources\\Images\\support_icon.png", store);

                CopyToIsolatedStorage("Resources\\Images\\T360_Flags_High_Priority@2x.png", store);
                CopyToIsolatedStorage("Resources\\Images\\T360_Flags_Low_Priority@2x.png", store);
                CopyToIsolatedStorage("Resources\\Images\\T360_Flags_Medium_Priority@2x.png", store);
                CopyToIsolatedStorage("Resources\\Images\\T360_greenflag@2x.png", store);

                CopyToIsolatedStorage("Resources\\Images\\Unchecked.png", store);

                CopyToIsolatedStorage("Resources\\Payload.zip", store);
            }
        }

        private void UpdateResourcePath(string file, IsolatedStorageFile store)
        {
            string contents = "";
            using (IsolatedStorageFileStream ifs = store.OpenFile(file, FileMode.OpenOrCreate))
            {
                StreamReader reader = new StreamReader(ifs);
                contents = reader.ReadToEnd();
                contents = Regex.Replace(contents, "#T360_CSS_PATH#", "/Resources/css");
                contents = Regex.Replace(contents, "#T360_JS_PATH#", "/Resources/js");

                using (StreamWriter sw = new StreamWriter(ifs))
                {
                    sw.Write(contents);
                    sw.Close();
                }
            }

            using (IsolatedStorageFileStream ifs = store.OpenFile(file, FileMode.Truncate))
            {
                using (StreamWriter sw = new StreamWriter(ifs))
                {
                    sw.Write(contents);
                    sw.Close();
                }
            }

        }

        private void CopyToIsolatedStorage(string file, IsolatedStorageFile store, bool overwrite = true)
        {
            if (store.FileExists(file) && !overwrite)
                return;

            using (var resourceStream = Application.GetResourceStream(new Uri(file, UriKind.Relative)).Stream)
            using (var fileStream = store.OpenFile(file, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                int bytesRead;
                var buffer = new byte[resourceStream.Length];
                while ((bytesRead = resourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    fileStream.Write(buffer, 0, bytesRead);
            }
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}