/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using System.Linq;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.BusinessObjects.Invoice;
using TyMetrix360.BusinessObjects.Security;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Dto.Invoice;

namespace TyMetrix360.Test.Service
{
    [TestClass]
    public class ServiceTest
    {
        private const string Username = "2012Training5-class";
        private const string Password = "TyMetrix1!";

        [TestMethod]
        public void TestLogin()
        {
            var parameters = new
            {
                LoginId = Username,
                Password = Password,
                IsSSOEnabled = false,
                IntegratedLoginId = string.Empty
            };

            var loginTask = ServiceInvoker.Instance.InvokeServiceUsingPost<LoginInfo>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.DoLogin), parameters, false, true);
            Assert.IsTrue(loginTask != null);
        }

        [TestMethod]
        public async void TestInvoiceList()
        {
            var parameters = new
            {
                LoginId = Username,
                Password = Password,
                IsSSOEnabled = false,
                IntegratedLoginId = string.Empty
            };

            var loginTask = ServiceInvoker.Instance.InvokeServiceUsingPost<LoginInfo>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.DoLogin), parameters, false, true);
            Assert.IsTrue(loginTask != null);

            List<InvoiceListDisplayFields> displayFields = await ServiceInvoker.Instance.InvokeServiceUsingGet<List<InvoiceListDisplayFields>>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.GetAwaitingInvoiceList));
            Assert.IsTrue(displayFields != null);
        }

        private List<InvoiceListDisplayFields> GetDisplayFields(InvoiceSummaryDto[] summaryList)
        {
            List<InvoiceListDisplayFields> displayFields = new List<InvoiceListDisplayFields>();
            displayFields.AddRange(summaryList.Select(summary =>
                new InvoiceListDisplayFields()
                {
                    BilledAmount = summary.BilledAmount,
                    CompanyName = summary.CompanyName,
                    InvoiceDate = summary.InvoiceDate,
                    InvoiceId = summary.InvoiceId,
                    InvoiceNumber = summary.InvoiceNumber,
                    MatterName = summary.MatterName,
                    NetAmount = summary.NetAmount
                }));
            return displayFields;
        }

        [TestMethod]
        public async void TestInvoiceSummary()
        {
            var parameters = new
            {
                LoginId = Username,
                Password = Password,
                IsSSOEnabled = false,
                IntegratedLoginId = string.Empty
            };

            var loginTask = ServiceInvoker.Instance.InvokeServiceUsingPost<LoginInfo>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.DoLogin), parameters, false, false);
            Assert.IsTrue(loginTask != null);

            List<InvoiceListDisplayFields> displayFields = await ServiceInvoker.Instance.InvokeServiceUsingGet<List<InvoiceListDisplayFields>>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.GetAwaitingInvoiceList));
            Assert.IsTrue(displayFields != null);

            if(displayFields.Any())
            {
                var serializableData = new { InvoiceId = displayFields.First().InvoiceId };
                var data = ServiceInvoker.Instance.InvokeServiceUsingGet<InvoiceSummary>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.GetInvoiceSummaryService + displayFields.First().InvoiceId));
            }
        }
    }
}
