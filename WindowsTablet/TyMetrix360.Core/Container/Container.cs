/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TyMetrix360.Core.ViewBase;
using TyMetrix360.Core.ViewModelBase;

namespace TyMetrix360.Core.Container
{
    public static class Container
    {
        private static readonly Dictionary<Type, TypeInformation> TypeContainer = new Dictionary<Type, TypeInformation>();
        private static readonly Dictionary<string, object> ConfigurationContainer = new Dictionary<string, object>();

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        public static void Register<TInterface, TImplementation>(Lifetime lifetime = Lifetime.Transient)
            where TImplementation : class, TInterface, new()
        {
            TypeContainer[typeof(TInterface)] =
                new TypeInformation(
                    typeof(TImplementation).GetTypeInfo().DeclaredConstructors
                                      .Single(one => !one.GetParameters().Any()), lifetime);
        }

        /// <summary>
        /// Gets an instance of a class based on interface.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <returns></returns>
        public static TInterface Resolve<TInterface>()
        {
            return (TInterface) TypeContainer[typeof (TInterface)].GetInstance();
        }


        /// <summary>
        /// Creates an instance of a view model based on interface
        /// </summary>
        /// <param name="viewModelInterfaceType">The view model interface type.</param>
        /// <returns></returns>
        public static IViewModelCore ResolveViewModel(Type viewModelInterfaceType)
        {
            return (IViewModelCore) TypeContainer[viewModelInterfaceType].GetInstance();
        }

        /// <summary>
        /// Creates an instance of a view model based on interface
        /// </summary>
        /// <param name="viewInterfaceType">The view interface type.</param>
        /// <returns></returns>
        public static IViewCore ResolveView(Type viewInterfaceType)
        {
            return (IViewCore) TypeContainer[viewInterfaceType].GetInstance();
        }

        /// <summary>
        /// Adds the configuration entry
        /// </summary>
        /// <typeparam name="T">Type of entry</typeparam>
        /// <param name="key">The key of entry</param>
        /// <param name="value">The value of the configuration</param>
        public static void AddConfiguration<T>(string key, T value)
        {
            ConfigurationContainer[key] = value;
        }

        /// <summary>
        /// Gets the configuration entry
        /// </summary>
        /// <typeparam name="T">Type of entry</typeparam>
        /// <param name="key">The key of entry</param>
        /// <returns></returns>
        public static T GetConfiguration<T>(string key)
        {
            return (T)ConfigurationContainer[key];
        }

        public static void Clear()
        {
            TypeContainer.Clear();
            ConfigurationContainer.Clear();
        }
    }
}
