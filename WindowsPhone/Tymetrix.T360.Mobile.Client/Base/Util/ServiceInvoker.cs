/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Phone.Net.NetworkInformation;

using Tymetrix.T360.Mobile.Client.Common;
using Newtonsoft.Json;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Core;

namespace Tymetrix.T360.Mobile.Client.Common.Base.Util
{
    public class ServiceInvoker
    {   
        private static CookieContainer cookies;

        public const string UserName = "userName";
        public const string DeviceId = "DeviceId";
        public const string DeviceOS = "DeviceOS";
        public const string DeviceType = "DeviceType";
        public const string AppVersion = "AppVersion";
        public const string TypeClientIP = "Type_Client_IP";
        public const string InstallationId = "InstallationId";
        public const string NotificationId = "NotificationId";
        public const string HasIntegratedLogin = "hasIntegratedLogin";
        public const string CultureName = "CultureName";

        private class State
        {
            public HttpWebRequest Request { get; set; }
            public string PostData { get; set; }
            public EventHandler<ServiceEventArgs> callBackMethod { get; set; }
        }

        #region Public Properties
        
        public static string ServiceHost { get; set; }
        public static bool IsConnected
        {
            get
            {
                if (Reachability.NetworkInterfaceType != NetworkInterfaceType.None || DeviceNetworkInformation.IsCellularDataEnabled || DeviceNetworkInformation.IsWiFiEnabled)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion
        
        #region Public Methods

        public static void Initialize(EventHandler<ServiceEventArgs> callBack)
        {
            ServiceInvoker.ClearCookies();
            if (!IsConnected)
            {
                throw new AppException(T360ErrorCodes.UnableToConnectServer);
            }

            ServiceInvoker.InvokeServiceUsingGet("api/t360/Security/GetConfig", delegate(object a, ServiceEventArgs serviceEventArgs)
            {
                ServiceResponse serviceResponse = serviceEventArgs.Result;
                if (!serviceResponse.Status)
                {
                    ServiceInvokeCompleted(callBack, new ServiceEventArgs() { Result = serviceResponse });
                    return;
                }

                string encryptionKey = serviceResponse.Output;
                string payLoadZipPassword = @"5sPEWxlwA3Wq/soV1bUaVuO5OJ7mubnmGJOoDyAFl0E=";

                string payLoadPassword = Vault.Decrypt(Convert.FromBase64String(payLoadZipPassword));
                byte[] archive = UncompressSingleFile("Resources\\payload.zip", payLoadPassword , "payload.txt");
                string payloadData = Encoding.UTF8.GetString(archive, 0, archive.Length);
                payloadData = Vault.Decrypt(Convert.FromBase64String(payloadData));

                Payload payLoad = new Payload() { Content = Convert.ToBase64String(Vault.Encrypt(payloadData, encryptionKey)) };
                UserData userdata = UserData.Instance;
                string userpassword = userdata.Password;
                userdata.Password = Convert.ToBase64String(Vault.Encrypt(userpassword, encryptionKey));

                string postData = JsonConvert.SerializeObject(payLoad);

                ServiceInvoker.InvokeServiceUsingPost("api/t360/Security/ApproveClient", postData, false, delegate(object obj, ServiceEventArgs arg)
                {
                    ServiceResponse response = arg.Result;
                    ServiceInvokeCompleted(callBack, new ServiceEventArgs() { Result = response });
                });
            }, false);
        }

        public static void InvokeServiceUsingGet(string serviceName, EventHandler<ServiceEventArgs> method, bool isHeaderRequired)
        {
            if (!IsConnected)
            {
                throw new AppException(T360ErrorCodes.UnableToConnectServer);
            }

            Uri uri = ConstructUri(serviceName);
            HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create(uri);
            webRequest.Method = "GET";
            webRequest.Accept = "*/**";
            webRequest.CookieContainer = cookies;

            if (isHeaderRequired)
            {
                PrepareHeaders(webRequest);
            }
            webRequest.Headers[CultureName] = CultureInformation;

            State state = new State() { Request = webRequest , callBackMethod= method };

            webRequest.BeginGetResponse(GetResponseCallback, state);
        }

        public static void InvokeServiceUsingPost(string serviceName, string postJsonData, bool isHeaderRequired, EventHandler<ServiceEventArgs> method)
        {
            if (!IsConnected)
            {
                throw new AppException(T360ErrorCodes.UnableToConnectServer);
            }

            Uri uri = ConstructUri(serviceName);

            HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create(uri);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json; charset=utf-8";
            webRequest.Accept = "*/**";
            webRequest.CookieContainer = cookies;

            if (isHeaderRequired)
            {
                PrepareHeaders(webRequest);
            }
            webRequest.Headers[CultureName] = CultureInformation;

            State state = new State() { Request = webRequest, PostData = postJsonData , callBackMethod = method };

            webRequest.BeginGetRequestStream(LoadRequestBegin, state);

        }

        public static void ClearCookies()
        {
            cookies = null;
            cookies = new CookieContainer();
        }

        private static void LoadRequestBegin(IAsyncResult result)
        {
            try
            {
                var state = (State)result.AsyncState;
                HttpWebRequest request = state.Request;
                using (Stream stream = request.EndGetRequestStream(result))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(state.PostData);
                    }
                }
                request.BeginGetResponse(GetResponseCallback, state);
            }
            catch (Exception)
            {
                throw new AppException(T360ErrorCodes.InternalServerError);
            }
        }

        private static void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                var state = (State)asynchronousResult.AsyncState;
                HttpWebRequest wr = state.Request;
                HttpWebResponse response = (HttpWebResponse)wr.EndGetResponse(asynchronousResult);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream streamResponse = response.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(streamResponse))
                        {
                            serviceResponse.Output = JsonConvert.DeserializeObject(streamReader.ReadToEnd()).ToString();
                            serviceResponse.Status = true;
                        }
                    }
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    serviceResponse.Status = false;
                    serviceResponse.ErrorDetails = new List<Error>() { new Error() { Code = "APP901" } };
                }
                else
                {
                    serviceResponse.Status = false;
                    serviceResponse.ErrorDetails = new List<Error>() { new Error() { Code = "APP200" } };
                }
                response.Close();
            }
            catch (WebException e)
            {
                serviceResponse.Status = false;
                if (e.Response != null)
                {
                    var errorResponse = (HttpWebResponse)e.Response;
                    if (errorResponse.StatusCode == HttpStatusCode.NotFound && errorResponse.StatusDescription != string.Empty &&
                        errorResponse.ResponseUri != null && !string.Empty.Equals(errorResponse.ResponseUri))
                    {
                        serviceResponse.ErrorDetails = new List<Error>() { new Error() { Code = T360ErrorCodes.ServerNotFound } };
                    }
                    else if (errorResponse.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        using (Stream streamResponse = errorResponse.GetResponseStream())
                        {
                            using (StreamReader streamReader = new StreamReader(streamResponse))
                            {
                                serviceResponse.ErrorDetails = JsonConvert.DeserializeObject<List<Error>>(JsonConvert.DeserializeObject(streamReader.ReadToEnd()).ToString());
                            }
                        }
                    }
                    else
                    {
                        serviceResponse.ErrorDetails = new List<Error>() { new Error() { Code = T360ErrorCodes.ServerError } };
                    }
                }
                else
                {
                    serviceResponse.ErrorDetails = new List<Error>() { new Error() { Code = T360ErrorCodes.ServerError } };
                }
            }
            finally
            {
                var state = (State)asynchronousResult.AsyncState;
                ServiceInvokeCompleted(state.callBackMethod, new ServiceEventArgs() { Result = serviceResponse });
            }
        }

        private static void ServiceInvokeCompleted(EventHandler<ServiceEventArgs> callback, ServiceEventArgs e)
        {
            if (callback != null)
            {
                callback(null, e);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static void PrepareHeaders(HttpWebRequest webRequest)
        {
            WebHeaderCollection header = new WebHeaderCollection();
            byte[] myDeviceID = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
            string idAsString = Convert.ToBase64String(myDeviceID);
            webRequest.Headers[DeviceId] = idAsString;
            webRequest.Headers[DeviceOS] = "Windows Phone:" + Environment.OSVersion.Version.ToString();
            webRequest.Headers[DeviceType] = (string)Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceName");
            webRequest.Headers[AppVersion] = GetVersionNumber();
            webRequest.Headers[TypeClientIP] = "Mobile App";
            if (IsolatedStorageSettings.ApplicationSettings.Contains(InstallationId))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(IsolatedStorageSettings.ApplicationSettings[InstallationId])))
                {
                    webRequest.Headers[InstallationId] = (string)IsolatedStorageSettings.ApplicationSettings[InstallationId];
                }
            }
            if (!string.IsNullOrEmpty(NotificationService.ChannelUri))
            {
                webRequest.Headers[NotificationId] = NotificationService.ChannelUri;
            }
            else
            {
                webRequest.Headers[NotificationId] = idAsString;
            }
        }

        private static Uri ConstructUri(string serviceName)
        {
            //return new Uri(ServiceHost + @"/" + serviceName + @"?t=" + DateTime.Now.ToString(), UriKind.Absolute);
            return new Uri(ServiceHost + @"/" + serviceName + @"", UriKind.Absolute);
        }

        public static string GetVersionNumber()
        {
            var asm = Assembly.GetExecutingAssembly();
            var parts = asm.FullName.Split(',');
            string fullVersion = parts[1].Split('=')[1];
            if (fullVersion != null && fullVersion.Length > 3)
            {
                fullVersion = fullVersion.Remove(3);
            }
            return fullVersion;
        }

        private static byte[] UncompressSingleFile(string compressedFileName, string compressedFilePassword, string fileNameEntryToUncompress)
        {
            using (Stream stream = Application.GetResourceStream(new Uri(compressedFileName, UriKind.Relative)).Stream)
            {
                using (var zipFile = new ZipFile(stream))
                {
                    zipFile.Password = compressedFilePassword;
                    var zipEntry = zipFile.GetEntry(fileNameEntryToUncompress);
                    using (var zipStream = zipFile.GetInputStream(zipEntry))
                    {
                        byte[] byt = new byte[16 * 1024];
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            int read;
                            while ((read = zipStream.Read(byt, 0, byt.Length)) > 0)
                            {
                                memoryStream.Write(byt, 0, read);
                            }
                            return memoryStream.ToArray();
                        }
                    }
                }
            }
        }


        private static string CultureInformation
        {
            get { return CultureInfo.CurrentCulture.ToString(); }
        }
        #endregion Private Methods

    }
}