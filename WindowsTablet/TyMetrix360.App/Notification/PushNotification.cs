/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using Windows.Foundation;
using Windows.Networking.PushNotifications;
using Windows.Storage;

using TyMetrix360.App.Common;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace TyMetrix360.App.Notification
{
    class PushNotification
    {
        private static PushNotificationChannel pushNotificationChannel;
        public static void OpenPushChannel()
        {
            try
            {
                var operation = PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                operation.Completed = OnChannelCreationCompleted;
            }
            catch (Exception)
            {
            }
        }

        private static void OnChannelCreationCompleted(IAsyncOperation<PushNotificationChannel> operation, AsyncStatus asyncStatus)
        {
            try
            {
                if (operation.Status == AsyncStatus.Completed)
                {
                    pushNotificationChannel = operation.GetResults();
                    ApplicationData.Current.LocalSettings.Values[Constants.NotificationIdKey] = pushNotificationChannel.Uri;
                }
            }
            catch (Exception)
            {
            }
        }

        public static void UpdateTile(int invoiceCount)
        {
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(CreateNotification(invoiceCount));
        }

        private static BadgeNotification CreateNotification(int invoiceCount)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(GetContent(invoiceCount));
            return new BadgeNotification(xmlDoc);
        }

        private static string GetContent(int count)
        {
            return String.Format("<badge version='{0}' value='{1}'/>", 1, count);
        }
    }
}
