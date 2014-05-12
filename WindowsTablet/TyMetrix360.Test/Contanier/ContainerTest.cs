/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using TyMetrix360.Core.Container;

namespace TyMetrix360.Test.Contanier
{
    [TestClass]
    public class ContainerTest
    {
        [TestMethod]
        public void TestClassResolution()
        {
            Container.Register<IBaseInterface, BaseClass>();
            var instance = Container.Resolve<IBaseInterface>();
            Assert.IsNotNull(instance);
            Assert.AreEqual(typeof(BaseClass), instance.GetType());
        }

        [TestMethod]
        public void TestConfiguration()
        {
            Container.AddConfiguration("test", 72);
            var instance = Container.GetConfiguration<int>("test");
            Assert.AreEqual(72, instance);
        }
    }
}
