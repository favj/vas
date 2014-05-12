/*
* @(#)WebApiClassBase.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using System.Web;
using System.Security.Principal;
using System.IO;
using System.Configuration;
using NUnit.Framework;
using Com.VizApp.VizApp.Service.WebApi.Filters;
using Com.VizApp.Arch;

namespace Com.VizApp.VizApp.Tests.Base
{
    public abstract class WebApiClassBase : IDisposable
    {
        private readonly string baseAddress;
        private HttpSelfHostConfiguration configuration;
        private HttpSelfHostServer server;
        private readonly Type controllerType;

        protected WebApiClassBase(Type controllerType)
            : this("localhost", 8080, controllerType)
        {

        }

        protected WebApiClassBase(string host, int port, Type controllerType)
        {
            this.controllerType = controllerType;
            if (string.IsNullOrEmpty(host))
            {
                host = "localhost";
            }

            baseAddress = string.Format("http://{0}:{1}", host, port);
        }

        public virtual HttpSelfHostConfiguration Configuration
        {
            get
            {
                if (configuration == null)
                {
                    configuration = new HttpSelfHostConfiguration(baseAddress);
                    configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
                    configuration.Services.Replace(typeof(IAssembliesResolver), new WebApiClassBase.TestAssemblyResolver(this.controllerType));
                    configuration.Routes.MapHttpRoute(name: "Default", routeTemplate: "api/{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional });
                    configuration.Filters.Add(new AppExceptionFilter());
                    configuration.Filters.Add(new AppAuthorizationFilter());
                    configuration.Filters.Add(new AppDataSanitizer());
                    configuration.Filters.Add(new AppResponseFilter());
                    var appXmlType = configuration.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
                    configuration.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

                    //configuration.MessageHandlers.Add(new HttpContextHandler());
                }

                return configuration;
            }
        }

        public virtual HttpSelfHostServer Server
        {
            get { return server ?? (server = new HttpSelfHostServer(Configuration)); }
        }

        public string BaseAddress
        {
            get { return baseAddress; }
        }

        public void Start()
        {
            Server.OpenAsync().Wait();

            HttpContext.Current = new HttpContext(
            new HttpRequest("", "http://tempuri.org", ""),
            new HttpResponse(new StringWriter())
            );

            string user = ConfigurationManager.AppSettings["testuser"];
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(user), new string[0]);

        }

        public void Close()
        {
            Server.CloseAsync().Wait();
        }

        protected HttpResponseMessage CreateRequest(string url, HttpMethod method, string acceptedMediaType = null)
        {
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(baseAddress + url);

            if (acceptedMediaType != null)
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptedMediaType));

            request.Method = method;
            var client = new HttpClient(this.Server);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                return response;
            }
        }

        protected void AssertError(AppException e, List<Error> errCodes)
        {
            List<Error> found = e.Errors;
            List<Error> notFound = new List<Error>(errCodes);

            notFound.RemoveAll(delegate(Error err)
            {
                foreach (Error error in found)
                {
                    if (err.Code == error.Code) return true;
                }
                return false;
            });

            found.RemoveAll(delegate(Error err)
            {
                foreach (Error error in errCodes)
                {
                    if (err.Code == error.Code) return true;
                }
                return false;
            });

            // true iff, all are found and there's nothing extra
            bool result = ((notFound.Count == 0) && (found.Count == 0));

            if (!result)
            {
                String msg = (notFound.Count == 0) ? "Extra error codes found:" + found
                                                  : "Error codes not found:" + notFound;
                Assert.Fail(msg);
            }
        }

        protected string Serialize(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (configuration != null)
            {
                configuration.Dispose();
                configuration = null;
            }

            if (server != null)
            {
                server.Dispose();
                server = null;
            }
        }


        #endregion

        public class TestAssemblyResolver : IAssembliesResolver
        {
            private readonly Type controllerType;

            public TestAssemblyResolver(Type controllerType)
            {
                this.controllerType = controllerType;
            }

            public ICollection<Assembly> GetAssemblies()
            {
                List<Assembly> baseAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

                if (!baseAssemblies.Contains(controllerType.Assembly))
                    baseAssemblies.Add(controllerType.Assembly);

                return baseAssemblies;
            }
        }
    }
}