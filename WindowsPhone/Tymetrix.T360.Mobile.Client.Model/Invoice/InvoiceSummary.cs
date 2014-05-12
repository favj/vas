/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;
using Tymetrix.T360.Mobile.Client.Model.Base;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class InvoiceSummary : BaseModel
    {
        private int invoiceId;
        public int InvoiceId
        {
            get { return invoiceId; }
            set { SetProperty(ref invoiceId, value, "InvoiceId"); }
        }
        private string invoiceNumber;
        public string InvoiceNumber
        {
            get { return invoiceNumber; }
            set { SetProperty(ref invoiceNumber, value, "InvoiceNumber"); }
        }
        private string companyName;
        public string CompanyName
        {
            get { return companyName; }
            set { SetProperty(ref companyName, value, "CompanyName"); }
        }
        private string matterName;
        public string MatterName
        {
            get { return matterName; }
            set { SetProperty(ref matterName, value, "MatterName"); }
        }
        private string matterNumber;
        public string MatterNumber
        {
            get { return matterNumber; }
            set { SetProperty(ref matterNumber, value, "MatterNumber"); }
        }
        private string netAmount;
        public string NetAmount
        {
            get { return netAmount; }
            set { SetProperty(ref netAmount, value, "NetAmount"); }
        }
        private string invoiceDate;
        public string InvoiceDate
        {
            get { return invoiceDate; }
            set { SetProperty(ref invoiceDate, value, "InvoiceDate"); }
        }
        private string billedAmount;
        public string BilledAmount
        {
            get { return billedAmount; }
            set { SetProperty(ref billedAmount, value, "BilledAmount"); }
        }
        private string billedFees;
        public string BilledFees
        {
            get { return billedFees; }
            set { SetProperty(ref billedFees, value, "BilledFees"); }
        }
        private string originalBilledFees;
        public string OriginalBilledFees
        {
            get { return originalBilledFees; }
            set { SetProperty(ref originalBilledFees, value, "OriginalBilledFees"); }
        }
        private string billedExpenses;
        public string BilledExpenses
        {
            get { return billedExpenses; }
            set { SetProperty(ref billedExpenses, value, "BilledExpenses"); }
        }
        private string originalBilledExpenses;
        public string OriginalBilledExpenses
        {
            get { return originalBilledExpenses; }
            set { SetProperty(ref originalBilledExpenses, value, "OriginalBilledExpenses"); }
        }
        private string billingPeriod;
        public string BillingPeriod
        {
            get { return billingPeriod; }
            set { SetProperty(ref billingPeriod, value, "BillingPeriod"); }
        }
        private string credit;
        public string Credit
        {
            get { return credit; }
            set { SetProperty(ref credit, value, "Credit"); }
        }
        private string currencyType;
        public string CurrencyType
        {
            get { return currencyType; }
            set { SetProperty(ref currencyType, value, "CurrencyType"); }
        }
        private string currencySymbol;
        public string CurrencySymbol
        {
            get { return currencySymbol; }
            set { SetProperty(ref currencySymbol, value, "CurrencySymbol"); }
        }
        private string grossAmount;
        public string GrossAmount
        {
            get { return grossAmount; }
            set { SetProperty(ref grossAmount, value, "GrossAmount"); }
        }
        private string itpAdjustment;
        public string ItpAdjustment
        {
            get { return itpAdjustment; }
            set { SetProperty(ref itpAdjustment, value, "ItpAdjustment"); }
        }
        private string netTotal;
        public string NetTotal
        {
            get { return netTotal; }
            set { SetProperty(ref netTotal, value, "NetTotal"); }
        }
        private string processingDiscount;
        public string ProcessingDiscount
        {
            get { return processingDiscount; }
            set { SetProperty(ref processingDiscount, value, "ProcessingDiscount"); }
        }
        private string reviewerAdjustment;
        public string ReviewerAdjustment
        {
            get { return reviewerAdjustment; }
            set { SetProperty(ref reviewerAdjustment, value, "ReviewerAdjustment"); }
        }
        private string status;
        public string Status
        {
            get { return status; }
            set { SetProperty(ref status, value, "Status"); }
        }
        private string subTotal;
        public string SubTotal
        {
            get { return subTotal; }
            set { SetProperty(ref subTotal, value, "SubTotal"); }
        }
        private string tax;
        public string Tax
        {
            get { return tax; }
            set { SetProperty(ref tax, value, "Tax"); }
        }
        private string taxCredit;
        public string TaxCredit
        {
            get { return taxCredit; }
            set { SetProperty(ref taxCredit, value, "TaxCredit"); }
        }
        private string totalWithCredit;
        public string TotalWithCredit
        {
            get { return totalWithCredit; }
            set { SetProperty(ref totalWithCredit, value, "TotalWithCredit"); }
        }
        private string vendorAdjustment;
        public string VendorAdjustment
        {
            get { return vendorAdjustment; }
            set { SetProperty(ref vendorAdjustment, value, "VendorAdjustment"); }
        }
        private string totalBilledAmount;
        public string TotalBilledAmount
        {
            get { return totalBilledAmount; }
            set { SetProperty(ref totalBilledAmount, value, "TotalBilledAmount"); }
        }
        private List<FlagDetails> flags;
        public List<FlagDetails> Flags
        {
            get { return flags; }
            set { SetProperty(ref flags, value, "Flags"); }
        }
        private List<ReviewRouteDetails> reviewRouteList;
        public List<ReviewRouteDetails> ReviewRouteList
        {
            get { return reviewRouteList; }
            set { SetProperty(ref reviewRouteList, value, "ReviewRouteList"); }
        }
        private List<DetailField> properties;
        public List<DetailField> Properties
        {
            get { return properties; }
            set { SetProperty(ref properties, value, "Properties"); }
        }
        private List<KeyFields> commonProperties;
        public List<KeyFields> CommonProperties
        {
            get { return commonProperties; }
            set { SetProperty(ref commonProperties, value, "CommonProperties"); }
        }
        private List<NotesData> notesList;
        public List<NotesData> NotesList
        {
            get { return notesList; }
            set { SetProperty(ref notesList, value, "NotesList"); }
        }
        private Dictionary<string, List<ValueList>> taxList;
        public Dictionary<string, List<ValueList>> TaxList
        {
            get { return taxList; }
            set { SetProperty(ref taxList, value, "TaxList"); }
        }
        private InvoicePermissions permissions;
        public InvoicePermissions Permissions
        {
            get { return permissions; }
            set { SetProperty(ref permissions, value, "Permissions"); }
        }

        //Line Items
        private string date;
        public string Date
        {
            get { return date; }
            set { SetProperty(ref date, value, "Date"); }
        }
        private string timeKeeper;
        public string TimeKeeper
        {
            get { return timeKeeper; }
            set { SetProperty(ref timeKeeper, value, "TimeKeeper"); }
        }
        private string narrativeText;
        public string NarrativeText
        {
            get { return narrativeText; }
            set { SetProperty(ref narrativeText, value, "NarrativeText"); }
        }
        private string vendorTask;
        public string VendorTask
        {
            get { return vendorTask; }
            set { SetProperty(ref vendorTask, value, "VendorTask"); }
        }
        private string vendorActivity;
        public string VendorActivity
        {
            get { return vendorActivity; }
            set { SetProperty(ref vendorActivity, value, "VendorActivity"); }
        }
        
        private string vendorUnits;
        public string VendorUnits
        {
            get { return vendorUnits; }
            set { SetProperty(ref vendorUnits, value, "VendorUnits"); }
        }
        private string vendorRate;
        public string VendorRate
        {
            get { return vendorRate; }
            set { SetProperty(ref vendorRate, value, "VendorRate"); }
        }
        private string vendorBilledTotal;
        public string VendorBilledTotal
        {
            get { return vendorBilledTotal; }
            set { SetProperty(ref vendorBilledTotal, value, "VendorBilledTotal"); }
        }
        private string originalNetTotal;
        public string OriginalNetTotal
        {
            get { return originalNetTotal; }
            set { SetProperty(ref originalNetTotal, value, "OriginalNetTotal"); }
        }
        private List<FlagDetails> flagslist;
        public List<FlagDetails> Flagslist
        {
            get { return flagslist; }
            set { SetProperty(ref flagslist, value, "Flagslist"); }
        }
        private bool notesAllowed;
        public bool NotesAllowed
        {
            get { return notesAllowed; }
            set { SetProperty(ref notesAllowed, value, "NotesAllowed"); }
        }
        private List<AdjustmentListDetails> adjustmentsList;
        public List<AdjustmentListDetails> AdjustmentsList
        {
            get { return adjustmentsList; }
            set { SetProperty(ref adjustmentsList, value, "AdjustmentsList"); }
        }
    }
}
