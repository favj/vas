/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Phone.Shell;

using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Core;

namespace Tymetrix.T360.Mobile.Client.Common.Base.Util
{
    public class UserActivity
    {
        private static volatile UserActivity userActivity;
        private static object syncRoot = new Object();

        private DispatcherTimer idleTimer;

        private UserActivity()
        { 
        }

        public void LoadTimer()
        {
            idleTimer = new DispatcherTimer();
            idleTimer.Interval = TimeSpan.FromMinutes(10);
            idleTimer.Tick += new EventHandler(OnTimerTick);
            idleTimer.Start();
            PhoneApplicationService.Current.State["lastActivityTime"] = DateTime.Now;
        }

        public void ResetTimer()
        {
            if (idleTimer == null) return;
            idleTimer.Stop();
            idleTimer.Interval = TimeSpan.FromMinutes(10);
            idleTimer.Start();
            PhoneApplicationService.Current.State["lastActivityTime"] = DateTime.Now;
        }

        private void OnTimerTick(object sender, EventArgs args)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (UserData.Instance.IsAuthenticated)
                {
                    BasePage.LogOff();
                    PhoneApplicationService.Current.State.Remove("lastActivityTime");
                    this.StopTimer();
                    throw new QuitException();
                }
            });
        }

        public void StopTimer()
        {
            if (idleTimer == null) return;
            idleTimer.Stop();
            idleTimer.Tick -= new EventHandler(OnTimerTick);
        }

        public static UserActivity Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (userActivity == null)
                    {
                        userActivity = new UserActivity();
                    }
                }
                return userActivity;
            }
        }
    }
}
