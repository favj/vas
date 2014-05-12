/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Threading;

using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;

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
            if (idleTimer != null) return;
            idleTimer = new DispatcherTimer();
            idleTimer.Interval = TimeSpan.FromMinutes(10);
            idleTimer.Tick += OnTimerTick;
            idleTimer.Start();
            PhoneApplicationService.Current.State["lastActivityTime"] = DateTime.Now;
        }

        public void ResetTimer()
        {
            if (idleTimer == null) return;
            ValidateSession();
            idleTimer.Stop();
            idleTimer.Interval = TimeSpan.FromMinutes(10);
            idleTimer.Start();
            PhoneApplicationService.Current.State["lastActivityTime"] = DateTime.Now;
        }

        private void OnTimerTick(object sender, EventArgs args)
        {
            ValidateSession();
        }

        private void ValidateSession()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (UserData.Instance.IsAuthenticated)
                {
                    if (PhoneApplicationService.Current.State.ContainsKey("lastActivityTime") &&
                        !string.IsNullOrEmpty(PhoneApplicationService.Current.State["lastActivityTime"].ToString()))
                    {
                        TimeSpan idleTime = DateTime.Now.Subtract(Convert.ToDateTime((PhoneApplicationService.Current.State["lastActivityTime"])));
                        if (idleTime.TotalSeconds >= 600)
                        {
                            BasePage.LogOff();
                            PhoneApplicationService.Current.State.Remove("lastActivityTime");
                            this.StopTimer();
                            throw new QuitException();
                        }
                    }
                }
            });
        }

        public void StopTimer()
        {
            if (idleTimer == null) return;
            idleTimer.Stop();
            idleTimer.Tick -= OnTimerTick;
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
