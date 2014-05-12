/*
* @(#)PolicyInjectionStrategy.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Com.VizApp.Arch
{
    public class UnityPolicyExtension : UnityContainerExtension
    {
        public static IUnityContainer CreateUnityContainer()
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<UnityPolicyExtension>();
            return container;
        }
        protected override void Initialize()
        {
            this.Context.Strategies
                 .AddNew<PolicyInjectionStrategy>(UnityBuildStage.PostInitialization);
        }
    }

    public class PolicyInjectionStrategy : IBuilderStrategy
    {
        private readonly IUnityContainer container = new UnityContainer();
        private static readonly TransparentProxyInterceptor injector =
                                               new TransparentProxyInterceptor();
        public PolicyInjectionStrategy()
        {
            container = new UnityContainer();
            container.AddNewExtension<Interception>();
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            PolicyInjectionSettings settings = (PolicyInjectionSettings)configurationSource
                                                  .GetSection(PolicyInjectionSettings.SectionName);
            if (settings != null)
            {
                settings.ConfigureContainer(container, configurationSource);
            }
        }
        public void PostBuildUp(IBuilderContext context)
        {
            var buildKey = (NamedTypeBuildKey)context.OriginalBuildKey;
            container.Configure<Interception>().SetDefaultInterceptorFor(buildKey.Type, injector);
            var policyInjectedInstance = container.BuildUp(buildKey.Type, context.Existing);
            context.Existing = policyInjectedInstance;
        }
        public void PostTearDown(IBuilderContext context)
        {
        }
        public void PreBuildUp(IBuilderContext context)
        {
        }
        public void PreTearDown(IBuilderContext context)
        {
        }
    }
}
