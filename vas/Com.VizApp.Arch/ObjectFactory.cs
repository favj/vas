/*
* @(#)ObjectFactory.cs
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
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Com.VizApp.Arch.Logging;

namespace Com.VizApp.Arch
{
    /// <summary>
    /// AppContext it's a easy-simple-helper class that implement a singleton,
    /// and help to get the objects for the application making IoC with Spring.Net
    /// The methods wrapps the singleton.
    /// </summary>
    public class ObjectFactory
    {
        /// <summary>
        /// Needed recursive declaration to implement a singleton
        /// </summary>
        private static ObjectFactory objectFactory;

        /// <summary>
        /// Object that gonna content the context of the app
        /// </summary>
        private IUnityContainer container = null;

        private ObjectFactory()
        {
            try
            {
                container = new UnityContainer();
                UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                section.Configure(container);
                container.AddNewExtension<UnityPolicyExtension>();

                switch (ConfigurationManager.AppSettings["logLevel"].ToLower())
                {
                    case "off":
                        Logger.SetLevel(Logger.Level.OFF);
                        break;
                    case "error":
                        Logger.SetLevel(Logger.Level.ERROR);
                        break;
                    case "info":
                        Logger.SetLevel(Logger.Level.INFO);
                        break;
                    case "debug":
                        Logger.SetLevel(Logger.Level.DEBUG);
                        break;
                }

                //LogWriterFactory factory = new LogWriterFactory(new SystemConfigurationSource());
                //LogWriter logWriter = factory.Create();
                //Logger.SetLogger(logWriter);

            }
            catch (Exception ex)
            {
                throw new AppException("Can't get the context of the Application", ex);
            }
        }

        /// <summary>
        /// Provide a Unique instance of AppContext
        /// </summary>
        private static ObjectFactory Instance
        {
            get
            {
                if (objectFactory == null)
                {
                    objectFactory = new ObjectFactory();
                }

                return objectFactory;
            }
        }

        public static T Resolve<T>()
        {
            return (T)Instance.container.Resolve<T>();
        }
    }
}