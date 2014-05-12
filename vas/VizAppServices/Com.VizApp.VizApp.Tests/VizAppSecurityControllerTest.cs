/*
* @(#)VizAppSecurityControllerTest.cs
*
* Copyright (c) 2013, Liberty Tax Service.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using System.Collections.Generic;
using NUnit.Framework;
using System.Net.Http;

namespace Com.LibertyTax.B2B.Tests
{
    [TestFixture]
    class B2BSecurityControllerTest //: WebApiClassBase
    {
        //public B2BSecurityControllerTest()
        //    : base("localhost", 63801, typeof(B2BSecurityController))
        //{

        //}

        //[TestFixtureSetUp]
        //public void Init()
        //{
        //    base.Start();
        //}

        //[Test]
        //public void Test01_GetAuthenticated()
        //{
        //    var response = base.CreateRequest("/api/B2Bsecurity/GetAuthenticated/", HttpMethod.Post);


        //    List<UserGroup> groups = new List<UserGroup>();
        //    groups.Add(new UserGroup() { CustomerId = 1, FirstName = "Elango", LastName = "Dhandapani" });
        //    groups.Add(new UserGroup() { CustomerId = 2, FirstName = "Babu", LastName = "Palani" });
        //    string expectedJson = Serialize(groups);
        //    IList<UserGroup> recievedRegions = response.Content.ReadAsAsync<IList<UserGroup>>().Result;
        //    string recievedJson = Serialize(recievedRegions);
        //    Assert.AreEqual(expectedJson, recievedJson);
        //}
    }
}
