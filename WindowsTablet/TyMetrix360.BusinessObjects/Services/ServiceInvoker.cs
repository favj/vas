/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;

using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.BusinessObjects.Invoice;
using TyMetrix360.BusinessObjects.LineItem;
using TyMetrix360.BusinessObjects.Security;
using TyMetrix360.Core;
using TyMetrix360.Core.Models;
using TyMetrix360.Dto.Common;
using TyMetrix360.Dto.Invoice;
using TyMetrix360.Dto.LineItem;
using System.Net;
using GalaSoft.MvvmLight.Messaging;

namespace TyMetrix360.BusinessObjects.Services
{
    public class ServiceInvoker
    {
        private static HttpClient _httpClient;

        private const string CultureName = "CultureName";
        private const string NotificationId = "NotificationId";
        private const string InstallationId = "InstallationId";
        private const string UniqueDeviceId = "UniqueDeviceId";
        private const string ContentType = "application/json";

        public const string Getconfig = "/api/t360/Security/GetConfig";
        public const string ApproveClient = "/api/t360/Security/ApproveClient";
        public const string DoLogin = "/api/t360/Security/DoLogin";
        public const string AcceptDisclaimerService = "/api/t360/Security/AcceptDisclaimer";
        public const string ResetPasswordService = "/api/t360/Security/ResetPassword";
        public const string GetDashboardInfoService = "/api/t360/Invoice/GetDashboardInfo";
        public const string GetAwaitingInvoiceList = "/api/t360/Invoice/GetAwaitingInvoiceList";
        public const string GetInvoiceSummaryService = "/api/t360/networks/invoice/";
        public const string CalculateInvoiceNetAmount = "/api/t360/Invoice/CalculateInvoiceNetAmount";
        public const string GetLineItemList = "/api/t360/networks/invoices/{0}/lineitem";
        public const string GetLineItemSummary = "/api/t360/networks/invoices/{0}/lineitem/{1}";
        public const string ApproveInvoiceService = "/api/t360/Invoice/ApproveInvoice";
        public const string ApproveMultiInvoiceService = "/api/t360/Invoice/ApproveMultipleInvoice";
        public const string GetReasonCodesService = "/api/t360/Invoice/GetReasonCodes";
        public const string AdjustInvoiceService = "/api/t360/Invoice/AdjustInvoice";
        public const string RejectInvoiceService = "/api/t360/Invoice/RejectInvoice";
        public const string GetSettingsService = "/api/t360/Settings/GetSettings";
        public const string UpdateSettingsService = "/api/t360/Settings/UpdateSettings";
        private const string DoLogoff = "/api/t360/Security/DoLogOff";

        public const string Success = "Success";

        private const string LoginPayload = "nvkejJ9dyJywyUG57jHHJyv5h7hTV/XesFSBLIT6ZQ4=";
        public const string ServiceUrl = "https://mobdev2.tymetrix.com";

        private ServiceInvoker() { }

        private static ServiceInvoker instance;
        public static ServiceInvoker Instance
        {
            get
            {
                if (instance == null) instance = new ServiceInvoker();
                return instance;
            }
        }

        public bool IsNetworkConnected
        {
            get { return NetworkInterface.GetIsNetworkAvailable(); }
        }

        private string CultureInformation
        {
            get
            {
                return Windows.Globalization.Language.CurrentInputMethodLanguageTag;
            }
        }

        public async Task LogOut()
        {
            if (!IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);
            try
            {
                var url = AppendUrl(DoLogoff);
                var data = await _httpClient.GetAsync(url);
                _httpClient = null;
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && T360ErrorCodes.ServerNotConnectedMsg.Equals(ex.InnerException.Message))
                {
                    throw new T360Exception(T360ErrorCodes.ServerNotFound);
                }
                else
                {
                    throw new T360Exception(T360ErrorCodes.RequestProcessingFailed);
                }
            }
        }

        public async Task<T> InvokeServiceUsingGet<T>(string serviceURL)
        {
            if (!IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);
            try
            {
                var data = await _httpClient.GetAsync(serviceURL);
                var output = data.Content.ReadAsStringAsync().Result;
                if ((int)data.StatusCode != 200)
                {
                    throw new T360Exception(JsonConvert.DeserializeObject<List<Error>>(output));
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(output);
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && T360ErrorCodes.ServerNotConnectedMsg.Equals(ex.InnerException.Message))
                {
                    throw new T360Exception(T360ErrorCodes.ServerNotFound);
                }
                else
                {
                    throw new T360Exception(T360ErrorCodes.RequestProcessingFailed);
                }
            }
        }

        public async Task<T> InvokeServiceUsingPost<T>(string serviceURL, object serializableData, bool shouldReturnSuccessString, bool headerRequired)
        {
            if (!IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);
            try
            {
                var serializedData = JsonConvert.SerializeObject(serializableData);
                HttpContent content = new StringContent(serializedData);
                content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
                if (headerRequired)
                {
                    ConstructHeaders(content);
                }
                var message = await _httpClient.PostAsync(serviceURL, content);

                string ResponseContent = await message.Content.ReadAsStringAsync();
                if ((int)message.StatusCode != 200)
                {
                    throw new T360Exception(JsonConvert.DeserializeObject<List<Error>>(ResponseContent));
                }
                else
                {
                    T result;
                    if (shouldReturnSuccessString)
                    {
                        result = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(Success));
                    }
                    else
                    {
                        result = JsonConvert.DeserializeObject<T>(ResponseContent);
                    }
                    return result;
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && T360ErrorCodes.ServerNotConnectedMsg.Equals(ex.InnerException.Message))
                {
                    throw new T360Exception(T360ErrorCodes.ServerNotFound);
                }
                else
                {
                    throw new T360Exception(T360ErrorCodes.RequestProcessingFailed);
                }
            }
        }

        public string AppendUrl(string uri)
        {
            return ServiceUrl + uri;
        }

        public async Task Initialize()
        {
            if (!IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);

            try
            {
                _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Add(CultureName, CultureInformation);
                var key = JsonConvert.DeserializeObject<string>(await _httpClient.GetStringAsync(AppendUrl(Getconfig)));
                var encrptedPayload = Vault.Encrypt(Vault.AES_Decrypt(LoginPayload), key);
                var data = new
                {
                    Content = encrptedPayload
                };
                var serializedData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(serializedData);
                content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
                HttpResponseMessage message = await _httpClient.PostAsync(AppendUrl(ApproveClient), content);
                string ResponseContent;
                if (message.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new T360Exception(T360ErrorCodes.InternalServerError);
                }
                else
                {
                    ResponseContent = await message.Content.ReadAsStringAsync();
                }
                var Response = JsonConvert.DeserializeObject<bool>(ResponseContent);
                Messenger.Default.Send<string>(key, "Encryption");
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && T360ErrorCodes.ServerNotConnectedMsg.Equals(ex.InnerException.Message))
                {
                    throw new T360Exception(T360ErrorCodes.ServerNotFound);
                }
                else
                {
                    throw new T360Exception(T360ErrorCodes.RequestProcessingFailed);
                }
            }
        }

        private void ConstructHeaders(HttpContent content)
        {
            EasClientDeviceInformation deviceInfo = new EasClientDeviceInformation();

            content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
            content.Headers.Add("DeviceId", GetDeviceId());
            content.Headers.Add("DeviceOS", "Windows Tablet : " + deviceInfo.SystemProductName);
            content.Headers.Add("DeviceType", deviceInfo.SystemManufacturer +  " : " + deviceInfo.OperatingSystem);
            content.Headers.Add("AppVersion", GetType().GetTypeInfo().Assembly.GetName().Version.Major.ToString());
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(InstallationId))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(ApplicationData.Current.LocalSettings.Values[InstallationId])))
                {
                    content.Headers.Add(InstallationId, (string)ApplicationData.Current.LocalSettings.Values[InstallationId]);
                }
            }
            else
            {
                content.Headers.Add(InstallationId, "");
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(NotificationId))
            {
                if (!string.IsNullOrEmpty((string)ApplicationData.Current.LocalSettings.Values[NotificationId]))
                {
                    content.Headers.Add(NotificationId, (string)ApplicationData.Current.LocalSettings.Values[NotificationId]);
                }
            }
            else
            {
                content.Headers.Add(NotificationId, Guid.NewGuid().ToString().Replace("-", string.Empty));
            }
        }

        private string GetDeviceId()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            object value = localSettings.Values[UniqueDeviceId];
            if (value == null)
            {
                value = Guid.NewGuid().ToString();
                localSettings.Values[UniqueDeviceId] = value;
            }
            return value.ToString();
        }
    }
}
