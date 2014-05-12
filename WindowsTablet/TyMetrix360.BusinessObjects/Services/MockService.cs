/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.BusinessObjects.Invoice;
using TyMetrix360.BusinessObjects.LineItem;
using TyMetrix360.BusinessObjects.Security;

namespace TyMetrix360.BusinessObjects.Services
{
    public class MockService
    {

        private const string True = "true";
        private const string MemName = "John";
        private const string LoginID = "john";
        private const string NwName = "Some name";
        private const string Success = "Success";

        public Task<T> InvokeServiceUsingGet<T>(string serviceURL)
        {
            return null;
        }

        public Task<T> InvokeServiceUsingPost<T,U,V>(string serviceURL, U serializableData, bool asServiceResult, bool shouldReturnSuccessString)
        {
            return null;
        }

        public Task<InvoiceAccept> ApproveInvoice(InvoiceAccept param)
        {
            var mock = GetInvoiceSummary(param.InvoiceId).Result;
            var result = new Task<InvoiceAccept>(() =>
                    new InvoiceAccept() { Status = True, InvoiceId = param.InvoiceId, ForceApprove = param.ForceApprove });
            result.RunSynchronously(TaskScheduler.Default);
            return result;
        }
        public Task<LoginInfo> AcceptDisclaimer()
        {
            var mock = new LoginInfo
            {
                MemberName = MemName,
                LoginId = LoginID,
                NetworkName = NwName
            };
            var result = new Task<LoginInfo>(() => mock);
            result.RunSynchronously(TaskScheduler.Default);
            return result;
        }
        public bool IsNetworkConnected { get { return true; } }
        public Task<IServiceResult<DashboardInfo>> GetDashboardInfo()
        {
            var result = new Task<IServiceResult<DashboardInfo>>(() =>
                    new ServiceResult<DashboardInfo>(new DashboardInfo() { InvoiceCount = 2 }, string.Empty, true));
            result.RunSynchronously(TaskScheduler.Default);
            return result;
        }
        public Task<IServiceResult<UserSettings>> GetSettings()
        {   
            var result = new Task<IServiceResult<UserSettings>>(() =>
                    new ServiceResult<UserSettings>(new UserSettings(), string.Empty, true));
            result.RunSynchronously(TaskScheduler.Default);
            return result;
        }
        public Task<IServiceResult<string>> UpdateSettings(bool data)
        {
            var result = new Task<IServiceResult<string>>(() =>
                    new ServiceResult<string>(Success, string.Empty, true));
            result.RunSynchronously(TaskScheduler.Default);
            return result;
        }
        public Task<IServiceResult<string>> Adjust(InvoiceAdjustment param)
        {
            //Essentialy need to Send to the Adjustment Service and then re-fetch the summary again. 
            var mock =   GetInvoiceSummary(Int32.Parse(param.InvoiceId)).Result;
            var result = new Task<IServiceResult<string>>(() =>
                    new ServiceResult<string>(Success, string.Empty, true));
            result.RunSynchronously(TaskScheduler.Default);
            return result;
        }
        public Task<int> GetInvoiceCount(string userName, string password)
        {
            var result = new Task<int>(() => 99);
            result.RunSynchronously(TaskScheduler.Default);
            return result;
        }
        public Task<IEnumerable<LineItemDetail>> GetInvoiceLineItemDetails(int invoiceId)
        {
            var invoice = GetInvoiceSummary(invoiceId).Result.Result;
            var mock = new List<LineItemDetail>
            {
                new LineItemDetail
                    {
                        NetAmount = 210.00.ToString(), Date = DateTime.Now.ToString(), FlagCount= invoice.Flags.Count.ToString(), InvoiceId = invoiceId, LineItemId = 1, TimeKeeper="Bob", CompanyName=invoice.CompanyName, Narrative = invoice.Narrative
                    },
                new LineItemDetail
                    {
                        NetAmount = 310.00.ToString(), Date = DateTime.Now.ToString(), FlagCount= invoice.Flags.Count.ToString(), InvoiceId = invoiceId, LineItemId = 2, TimeKeeper="Mark", CompanyName=invoice.CompanyName, Narrative = invoice.Narrative
                    },
                new LineItemDetail
                    {
                        NetAmount = 410.00.ToString(), Date = DateTime.Now.ToString(), FlagCount= invoice.Flags.Count.ToString(), InvoiceId = invoiceId, LineItemId = 3, TimeKeeper="Jim", CompanyName=invoice.CompanyName, Narrative = invoice.Narrative
                    },
                new LineItemDetail
                    {
                        NetAmount = 510.00.ToString(), Date = DateTime.Now.ToString(), FlagCount= invoice.Flags.Count.ToString(), InvoiceId = invoiceId, LineItemId = 4, TimeKeeper="Sergey", CompanyName=invoice.CompanyName, Narrative = invoice.Narrative
                    }     
            };
            var result = new Task<IEnumerable<LineItemDetail>>(() => mock);
            result.RunSynchronously(TaskScheduler.Default);
            return result;
        }
        public Task<IServiceResult<LineItemDetail>> GetInvoiceLineItemDetail(int invoiceId, int lineItemId)
        {
            var mock = new LineItemDetail
            {
                NetAmount = 210.00.ToString(),
                Date = DateTime.Now.ToString(),
                FlagCount = 3.ToString(),
                InvoiceId = invoiceId,
                LineItemId = lineItemId,
                TimeKeeper = "Bob"
            };
            var result = new Task<IServiceResult<LineItemDetail>>(() =>
                   new ServiceResult<LineItemDetail>(mock, string.Empty, true));
            result.RunSynchronously(TaskScheduler.Default);
            return result;
        }
        public async Task LogOut()
        {
        }
        public Task<IServiceResult<InvoiceSummary>> GetInvoiceSummary(int param)
        {
            var mock = new InvoiceSummary
            {
                
                BilledAmount = "$10.00",
                BillingPeriod = "6/30/2012",
                CommonProperties = new ObservableCollection<BaseTyProperty>() {
                    new BaseTyProperty(){ LabelText="Some Common Property", ValueText="This went over by way too much."},
                    new BaseTyProperty(){ LabelText="Other Common Property", ValueText="10 Decrease."}
                },
                
                CompanyName = "Acme",
                Credit = 0.0m.ToString(),
                CurrencySymbol = "$", 
                CurrencyType = "USD",
                Flags = new ObservableCollection<FlagDetails>
                            {
                                new FlagDetails { WarningInfo = "Max Time duration has exceeded 3 days.", Priority = "Error" }, 
                                new FlagDetails { WarningInfo = "Max Time duration for one entry has exceeded 2 hours.", Priority = "Warning" }
                            },
                GrossAmount = 100.00m.ToString(),
                InvoiceDate = DateTime.Today.ToString(),
                InvoiceId = param,
                InvoiceNumber = param.ToString(),
                InvoiceSummaryViewItemList = new ObservableCollection<InvoiceSummaryViewItem>(),
                ItpAdjustment = 40.00m.ToString(),
                TaxList = new Dictionary<string, ObservableCollection<BaseTaxItem>>(),
                MatterName = "Rocket Co.",
                Narrative = "Here is a breakdown of the Summary Text you will see here. Need formatting information.",
                Notes = new ObservableCollection<Note>() {
                    new Note() { CreatedTime= DateTime.Now.ToString() ,  Description = "Here is a breakdown of the Note Text you will see here. Need formatting information.", Creator ="Tom"},
                    new Note() { CreatedTime= DateTime.Now.ToString(), Description = "Here is a breakdown of the Note Text you will see here. Another.", Creator = "Bob"}
                
                },
                NetAmount = "$106.02",
                NetTotal = 106.02m.ToString(),
                Permissions = new Permissions() { AdjustFee = true, Approve=true, NegativeAdjust=true, Reject=true},
                ProcessingDiscount = 10.02m.ToString(),
                Properties = new ObservableCollection<BaseTyProperty>() {
                    new BaseTyProperty(){ LabelText="Some Property", ValueText="This went over by way too much."},
                    new BaseTyProperty(){ LabelText="Other Property", ValueText="10 Decrease."}
                },
                ReviewerAdjustment = 0.0m.ToString(),
                ReviewRouteList = new ObservableCollection<ReviewerRouteItem>()
                {
                  new ReviewerRouteItem() {ReviewerName = "Bob", ReviewStatus="Approved"},
                  new ReviewerRouteItem() {ReviewerName = "Aaron", ReviewStatus="In Review"},
                },
                Status = "Away",
                SubTotal = 100.02m.ToString(),
                Tax = 6.00m.ToString(),
                TaxCredit = 0m.ToString(),
                TotalBilledAmount = 102320.02m.ToString(),
                TotalWithCredit = 100.02m.ToString(),   
                VendorAdjustment = 0m.ToString()
            };
            var result1 = new Task<InvoiceSummary>(() => mock);
            result1.RunSynchronously(TaskScheduler.Default);
            BaseTaxItem item1 = new BaseTaxItem() { Key = "Federal", TaxJurisdictionCode = "RJD2", TaxRate = "5%", TaxTypeCode = "Federal" }; 
            BaseTaxItem item2 = new BaseTaxItem() {Key="Local", TaxJurisdictionCode="C3PO", TaxRate="10%", TaxTypeCode="Local"};
            Dictionary<string, ObservableCollection<BaseTaxItem>> taxList = new Dictionary<string, ObservableCollection<BaseTaxItem>>();
            taxList.Add("Federal", new ObservableCollection<BaseTaxItem> { item1 });
            taxList.Add("Local", new ObservableCollection<BaseTaxItem> { item2 });
            var mock2 = new InvoiceSummary
            {
                BilledAmount = "$2310.00",
                BillingPeriod = "6/30/2012",
                CommonProperties = new ObservableCollection<BaseTyProperty>() {
                    new BaseTyProperty(){ LabelText="Some Common Property", ValueText="This went over by way too much."},
                    new BaseTyProperty(){ LabelText="Other Common Property", ValueText="10 Decrease."}
                },

                CompanyName = "Mock Company",
                Credit = 0.0m.ToString(),
                CurrencySymbol = "$",
                CurrencyType = "USD",          
                TaxList = taxList,
                Flags = new ObservableCollection<FlagDetails>
                            {
                                new FlagDetails { WarningInfo = "Max Time duration has exceeded 3 days.", Priority = "Error" }, 
                                new FlagDetails { WarningInfo = "Max Time duration for one entry has exceeded 2 hours.", Priority = "Warning" }
                            },
                GrossAmount = 100.00m.ToString(),
                InvoiceDate = DateTime.Today.ToString(),
                InvoiceId = param,
                InvoiceNumber = param.ToString(),
                InvoiceSummaryViewItemList = new ObservableCollection<InvoiceSummaryViewItem>(),
                ItpAdjustment = 40.00m.ToString(),
                MatterName = "Rocket Co.",
                Narrative = "Here is a breakdown of the Summary Text you will see here. Need formatting information.",
                Notes = new ObservableCollection<Note>() {
                                      new Note() { CreatedTime= DateTime.Now.ToString() ,  Description = "Here is a breakdown of the Note Text you will see here. Need formatting information.", Creator ="Tom"},
                                     new Note() { CreatedTime= DateTime.Now.ToString() ,  Description = "Here is a breakdown of the Note Text you will see here. Need formatting information. Another", Creator ="Bob"},
                
                },
                NetAmount = "$106.02",
                NetTotal = 106.02m.ToString(),
                Permissions = new Permissions() { AdjustFee = true, Approve = true, NegativeAdjust = true, Reject = true },
                ProcessingDiscount = 10.02m.ToString(),
                Properties = new ObservableCollection<BaseTyProperty>() {
                    new BaseTyProperty(){ LabelText="Some Property", ValueText="This went over by way too much."},
                    new BaseTyProperty(){ LabelText="Other Property", ValueText="10 Decrease."}
                },
                ReviewerAdjustment = 0.0m.ToString(),
                ReviewRouteList = new ObservableCollection<ReviewerRouteItem>()
                {
                  new ReviewerRouteItem() {ReviewerName = "Bob", ReviewStatus="Approved"},
                  new ReviewerRouteItem() {ReviewerName = "Aaron", ReviewStatus="In Review"},
                },
                Status = "Away",
                SubTotal = 100.02m.ToString(),
                Tax = 6.00m.ToString(),
                TaxCredit = 0m.ToString(),
                TotalBilledAmount = 102320.02m.ToString(),
                TotalWithCredit = 100.02m.ToString(),
                VendorAdjustment = 0m.ToString()
            };
            var resultSummary = new Task<InvoiceSummary>(() => mock2);
            resultSummary.RunSynchronously(TaskScheduler.Default);
            var r = new Random();
            var resultInvoice = new Task<IServiceResult<InvoiceSummary>>(() => new ServiceResult<InvoiceSummary>(new InvoiceSummary(), string.Empty, true));
            if (r.Next() % 2 == 0)
            {
                resultInvoice = new Task<IServiceResult<InvoiceSummary>>(() =>
                    new ServiceResult<InvoiceSummary>(result1.Result, string.Empty, true));
            }
            else
            {
                resultInvoice = new Task<IServiceResult<InvoiceSummary>>(() =>
                    new ServiceResult<InvoiceSummary>(resultSummary.Result, string.Empty, true));
            }
            resultInvoice.RunSynchronously(TaskScheduler.Default);
            return resultInvoice;
           
        }
        public Task<IServiceResult<IEnumerable<InvoiceListDisplayFields>>> GetInvoices()
        {
            var mock = new List<InvoiceListDisplayFields>
            {
                new InvoiceListDisplayFields
                    {
                        BilledAmount = "$510.00",  CompanyName="Acme", 
                        InvoiceDate= DateTime.Today.ToString(),InvoiceId= 2,InvoiceNumber= "1232",MatterName="Rocket Co",NetAmount="$13.00"
                    },
                new InvoiceListDisplayFields
                    {
                        BilledAmount = "$334.00", CompanyName="Beta", 
                        InvoiceDate= DateTime.Today.ToString(),InvoiceId= 3,InvoiceNumber= "12231122",MatterName="Giant Slingshot",NetAmount="$41.02"
                    },
                new InvoiceListDisplayFields
                    {
                        BilledAmount = "$120.00", CompanyName="Delta", 
                        InvoiceDate= DateTime.Today.ToString(),InvoiceId= 4,InvoiceNumber= "3242342342",MatterName="Water Wheels",NetAmount="$514.02"
                    },
                new InvoiceListDisplayFields
                    {
                        BilledAmount = "$131.00", CompanyName="Charlie", 
                        InvoiceDate= DateTime.Today.ToString(),InvoiceId= 4,InvoiceNumber= "000123",MatterName="Space Ship", NetAmount="$345.92"
                    },
                new InvoiceListDisplayFields
                    {
                        BilledAmount = "$131.00", CompanyName="Acme", 
                        InvoiceDate= DateTime.Today.ToString(),InvoiceId= 4,InvoiceNumber= "000123",MatterName="Space Ship", NetAmount="$345.92"
                    },
                new InvoiceListDisplayFields
                    {
                        BilledAmount = "$131.00", CompanyName="Matrix Moving", 
                        InvoiceDate= DateTime.Today.ToString(),InvoiceId= 4,InvoiceNumber= "000123",MatterName="Space Ship", NetAmount="$345.92"
                    },
                new InvoiceListDisplayFields
                    {
                        BilledAmount = "$131.00", CompanyName="Company X", 
                        InvoiceDate= DateTime.Today.ToString(),InvoiceId= 4,InvoiceNumber= "000123",MatterName="Space Ship", NetAmount="$345.92"
                    },
                new InvoiceListDisplayFields
                    {
                        BilledAmount = "$131.00", CompanyName="Full House Inc.", 
                        InvoiceDate= DateTime.Today.ToString(),InvoiceId= 4,InvoiceNumber= "000123",MatterName="Space Ship", NetAmount="$345.92"
                    },
                new InvoiceListDisplayFields
                    {
                        BilledAmount = "$131.00", CompanyName="Coca-Cola", 
                        InvoiceDate= DateTime.Today.ToString(),InvoiceId= 4,InvoiceNumber= "000123",MatterName="Space Ship", NetAmount="$345.92"
                    },
                new InvoiceListDisplayFields
                    {
                        BilledAmount = "$131.00", CompanyName="Pepsi-Cola", 
                        InvoiceDate= DateTime.Today.ToString(),InvoiceId= 4,InvoiceNumber= "000123",MatterName="Space Ship", NetAmount="$345.92"
                    }
            };
            var result = new Task<IServiceResult<IEnumerable<InvoiceListDisplayFields>>>(() => new ServiceResult<IEnumerable<InvoiceListDisplayFields>>(mock, string.Empty, true));
            result.RunSynchronously(TaskScheduler.Default);
            return result;
        }
        public Task<IEnumerable<ReasonCode>> GetReasonCodes(string s)
        {
            var mock = new List<ReasonCode>
            {
                new ReasonCode { Description =String.Format("{0} type Pulled",s), Id = 0},
                new ReasonCode { Description ="Account Locked", Id = 1},
                new ReasonCode { Description ="Default", Id = 2},
                new ReasonCode { Description ="Being Fussy", Id = 3}
            };
            var result = new Task<IEnumerable<ReasonCode>>(() => mock);
            result.RunSynchronously(TaskScheduler.Default);
            return result;
        }
        public Task<LoginInfo> Login(string userName, string password, bool isSSo, string integratedLoginId)
        {
            var mock = new LoginInfo
            {
                MemberName = MemName,
                LoginId = userName,
                NetworkName = NwName
            };
            var result = new Task<LoginInfo>(() => mock);
            result.RunSynchronously(TaskScheduler.Default);
            return result;
        }
        public Task<LoginInfo> ResetPassword(string password, bool UseLastPassword)
        {
            var mock = new LoginInfo
            {
                Password = password
            };
            var result = new Task<LoginInfo>(() => mock);
            result.RunSynchronously(TaskScheduler.Default);
            return result;
        }
        public Task<IServiceResult<string>> RejectInvoice(InvoiceReject param)
        {
            //Essentialy need to Send to the Adjustment Service and then re-fetch the summary again. 
            var mock = GetInvoiceSummary(param.InvoiceId).Result;
            var result = new Task<IServiceResult<string>>(() =>
                    new ServiceResult<string>(Success, string.Empty, true));
            result.RunSynchronously(TaskScheduler.Default);
            return result;
        }
      }
}
