/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Phone.Notification;

namespace Tymetrix.T360.Mobile.Client.Common.Base.Util
{
    public class NotificationService
    {
        string channelName;
        HttpNotificationChannel channel;

        public NotificationService(string channelName)
        {
            this.channelName = channelName;
        }

        /// <summary>
        /// Subscribes to the notification events on the channel.
        /// If the channel doesn't already exist, it will be created
        /// and bound to shell tile and toasts.
        /// </summary>
        public void Subscribe()
        {
            if (channel == null) BindChannel();
        }

        /// <summary>
        /// Unubscribes from the notification events on the channel.
        /// </summary>
        public void Unsubscribe()
        {
            if (channel != null) UnsubscribeFromChannelEvents();
        }

        /// <summary>
        /// Finds or creates the notification channel and binds the shell tile
        /// and toast notifications as well as events.
        /// </summary>
        private void BindChannel()
        {
            channel = HttpNotificationChannel.Find(channelName);

            if (channel == null || channel.ChannelUri == null)
            {
                if (channel != null) DisposeChannel();

                channel = new HttpNotificationChannel(channelName);
                channel.ChannelUriUpdated += channel_ChannelUriUpdated;
                channel.Open();
            }
            else
            {
                ChannelUri = channel.ChannelUri.AbsoluteUri;
                System.Diagnostics.Debug.WriteLine(channel.ChannelUri.AbsoluteUri);
            }

            SubscribeToChannelEvents();

            if (!channel.IsShellTileBound) channel.BindToShellTile();
            if (!channel.IsShellToastBound) channel.BindToShellToast();
        }

        /// <summary>
        /// Subscribes to the channel's events.
        /// </summary>
        private void SubscribeToChannelEvents()
        {
            channel.ShellToastNotificationReceived += channel_ShellToastNotificationReceived;
            channel.HttpNotificationReceived += channel_HttpNotificationReceived;
            channel.ErrorOccurred += channel_ErrorOccurred;
        }

        /// <summary>
        /// Unsubscribes from the channel's events
        /// </summary>
        private void UnsubscribeFromChannelEvents()
        {
            channel.ShellToastNotificationReceived -= channel_ShellToastNotificationReceived;
            channel.HttpNotificationReceived -= channel_HttpNotificationReceived;
            channel.ErrorOccurred -= channel_ErrorOccurred;
        }

        /// <summary>
        /// Closes the channel and disposes it.
        /// </summary>
        private void DisposeChannel()
        {
            channel.Close();
            channel.Dispose();
            channel = null;
        }

        /// <summary>
        /// Event handler for the ChannelUriUpdate event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void channel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            channel.ChannelUriUpdated -= channel_ChannelUriUpdated;
            //System.Diagnostics.Debug.WriteLine(e.ChannelUri.AbsoluteUri);
            ChannelUri = e.ChannelUri.AbsoluteUri;
            OnChannelUriUpdated(e);
        }

        /// <summary>
        /// Raised when the notification channel is given a URI.
        /// </summary>
        /// <remarks>
        /// This is when you would call a web service to tell it that a client is
        /// registered and what the notification URI is.
        /// </remarks>
        public event EventHandler<NotificationChannelUriEventArgs> ChannelUriUpdated;

        /// <summary>
        /// Raises the ChannelUriUpdated event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChannelUriUpdated(NotificationChannelUriEventArgs e)
        {
            if (ChannelUriUpdated != null) ChannelUriUpdated(this, e);
        }

        /// <summary>
        /// Event handler for the HtppNotificationReceived event.
        /// This is called when a raw notification is received.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void channel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {
            byte[] bytes;
            using (var stream = e.Notification.Body)
            {
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
            }
            var message = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            OnRawNotificationReceived(message);
        }

        /// <summary>
        /// Occurs when a raw notification is received.
        /// </summary>
        public event EventHandler<RawNotificationRecievedEventArgs> RawNotificationReceived;

        /// <summary>
        /// Raises the RawNotificationReceived event on the UI thread.
        /// </summary>
        /// <param name="message"></param>
        protected virtual void OnRawNotificationReceived(string message)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (RawNotificationReceived != null)
                    RawNotificationReceived(
                        this,
                        new RawNotificationRecievedEventArgs(message)
                        );
            });
        }

        /// <summary>
        /// Event handler for the ShellToastNotificationReceived event.
        /// This occurs when a toast notification is received on the channel.
        /// </summary>
        /// <remarks>
        /// This must be handled by the application if it is running when a toast is received.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void channel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            var title = e.Collection.Values.First();
            var message = e.Collection.Values.Skip(1).FirstOrDefault() ?? string.Empty;
            OnToastNotificationReceived(title, message);
        }

        /// <summary>
        /// Occurs when a toast notification is received.
        /// </summary>
        /// <remarks>
        /// This must be handled by the application if it is running when a toast is received.
        /// </remarks>
        public event EventHandler<ToastNotificationReceivedEventArgs> ToastNotificationReceived;

        /// <summary>
        /// Raises the ToastNotificationReceived event on the UI thread.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        protected virtual void OnToastNotificationReceived(string title, string message)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (ToastNotificationReceived != null)
                    ToastNotificationReceived(
                        this,
                        new ToastNotificationReceivedEventArgs(title, message)
                        );
            });
        }

        /// <summary>
        /// Event handler for the ErrorOccurred event.
        /// Handles different events according to ErrorType.
        /// </summary>
        /// <remarks>
        /// Needs more work... ;-(
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void channel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            switch (e.ErrorType)
            {
                // something went severely wrong. lets wait a while before trying again.
                case ChannelErrorType.ChannelOpenFailed:
                    DisposeChannel();
                    System.Threading.Thread.Sleep(60000);
                    BindChannel();
                    break;
                // an image uri has been referenced in a notification that was
                // not bound to the shell tile.
                case ChannelErrorType.MessageBadContent:
                    break;
                // too many notifications have been received in too short a time span.
                case ChannelErrorType.NotificationRateTooHigh:
                    break;
                // a bad payload was received. re-establish the connection to overcome this.
                case ChannelErrorType.PayloadFormatError:
                    DisposeChannel();
                    BindChannel();
                    break;
                // the type notifications we're receiving is going to change.
                case ChannelErrorType.PowerLevelChanged:
                    break;
                default:
                    break;
            }
        }

        public static string ChannelUri { get; set; }
    }

    public class RawNotificationRecievedEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public RawNotificationRecievedEventArgs(string message)
        {
            Message = message;
        }
    }

    public class ToastNotificationReceivedEventArgs : EventArgs
    {
        public string Title { get; private set; }
        public string Message { get; private set; }
        public ToastNotificationReceivedEventArgs(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
