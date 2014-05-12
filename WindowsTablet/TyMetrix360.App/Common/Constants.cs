/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
namespace TyMetrix360.App.Common
{
    public static class Constants
    {

        //For Messenger
        public const string Confirm = "confirm";
        public const string Adjust = "adjust";
        public const string Reject = "Reject";
        public const string SetInvoice = "set";
        public const string Details = "details";
        public const string Expand = "expand";
        public const string SetLineDetail = "setline";
        public const string AdjustFees = "AdjustFees";
        public const string AdjustExpenses = "AdjustExpenses";
        public const string SetInvoiceSummary = "SetInvoiceSummary";
        public const string SendBackToInvoicePage = "SendBackToInvoicePage";
        public const string SetShellPage = "SetShellPage";
        public const string SendBackToLineItemDetailPage = "SendBackToLineItemDetailPage";
        public const string AdjustDefaultSettings = "AdjustDefaultSettings";
        public const string UnregisterInvoiceListEvents = "UnregisterInvoiceListEvents";
        public const string RegisterInvoiceListEvents = "RegisterInvoiceListEvents";
        public const string RefreshAppBar = "RefreshAppBar";
        public const string CancelMultiSelect = "CancelMultiSelect";
        public const string ApproveInvoice = "ApproveInvoice";
        public const string ShowConfirmationPopup = "ShowConfirmationPopup";
        public const string CloseConfirmationPopup = "CloseConfirmationPopup";
        public const string ToLineItemsFromInvoice = "ToLineItemsFromInvoice";
        public const string MultiSelect_SelectAll = "MultiSelect-SelectAll";
        public const string MultiSelect_ClearAll = "MultiSelect-ClearAll";
        public const string InvoiceErrorDetails = "InvoiceErrorDetails";
        public const string AddApproveItems = "AddApproveItems";
        public const string UnregisterFailureViewEvents = "UnregisterFailureViewEvents";
        public const string RefreshInvoiceList = "RefreshInvoiceList";
        public const string RemoveMultiSelect = "RemoveMultiSelect";
        public const string ShowPopupAfterConfirmation = "ShowPopupAfterConfirmation";

        //settings keys
        public const string UserNameKey = "username";
        public const string IntegratedLoginKey = "integratedlogin";
        public const string NotificationIdKey = "NotificationId";
        public const string DefaultsId = "DefaultsId";
        public const string Defaults = "Defaults";
        public const string PrivacyPolicyId = "PrivacyPolicyId";
        public const string PrivacyPolicy = "Privacy Policy";
        public const string LogOut = "LogOut";

        //error dialog title
        public const string ReasonsFailed = "Invoice Reasons Failed";
        public const string AdjustmentFailed = "Invoice Adjust Failed";
        public const string AdjustmentConfirmation = "Adjustment Confirmation";
        public const string DashboardFailed = "DashBoard Error";
        public const string FAQFailed = "FAQs Error";
        public const string SupportFailed = "Support Error";
        public const string LogoffFailed = "Logoff Error";
        public const string LogoffTitle = "Logoff Confirmation";
        public const string DisclaimerFailed = "Disclaimer Error";
        public const string LoginFailed = "Login Failed";
        public const string InvoiceLineItemDetailFailed = "Invoice Line Item Detail Failed";
        public const string InvoiceLineItemFailed = "Invoice Line Item Failed";
        public const string InvoiceListFailed = "Invoice list Failed";
        public const string InvoiceListSummaryFailed = "Invoice Summary Failed";
        public const string RejectionFailed = "Invoice Reject Failed";
        public const string RejectConfirmation = "Reject Confirmation";
        public const string PasswordError = "Password Reset";
        public const string SettingsFailed = "User Preference";
        public const string AutoLogOff = "Session Timed Out";
        public const string Selection = "Summary Failed";
        public const string ApproveTitle = "Approval Confirmation";
        public const string ApproveFailed = "Invoice Approve Failed";
        public const string AdjustNotAllowed = "Adjustment not Permitted";
        public const string NetworkError = "Network Error";
        public const string ApproveError = "Approve Failed";

        //UI Label
        public const string AdjustBy = "Adjust By";
        public const string AdjustTo = "Adjust To";
        public const string LineItemLevelExpenseAdjustment = "Line Item Level Reviewer Expense Adjustment";
        public const string LineItemLevelFeeAdjustment = "Line Item Level Reviewer Fee Adjustment";
        public const string VendorFeeAdjustment = "Vendor Fee Adjustment";
        public const string VendorExpenseAdjustment = "Vendor Expense Adjustment";
        public const string ReviewerAdjustment = "Reviewer Adjustment";
        public const string ITPAdjustment = "ITP Adjustment";
        public const string None = "None";
        public const string Date = "Date";
        public const string Owner = "Owner";
        public const string TaxType = "Tax Type";
        public const string TaxJurisdiction = "Tax Jurisdiction";
        public const string TaxRate = "Tax Rate";
        public const string NetAmount = "Net Amount";
        public const string TimeKeeper = "TimeKeeper";
        public const string General = "General";
        public const string Narrative = "Narrative";
        public const string FirmVendorBilling = "Firm/Vendor Billing";
        public const string Task = "Task";
        public const string Activity = "Activity";
        public const string UnitHours = "Unit/Hours";
        public const string Rate = "Rate";
        public const string VendorAdjustment = "Vendor Adjustment";
        public const string BilledTotal = "Billed Total";
        public const string Flags = "Flags";
        public const string NetTotal = "Net Total";
        public const string InHouseReview = "In-House Review";
        public const string Adjustments = "Adjustments";
        public const string Notes = "Notes";
        public const string Taxes = "Taxes";
        public const string Description = "Description";
        public const string Amount = "Amount";
        public const string CurrentReviewer = "Current Reviewer";
        public const string TaxJurisdictionCode = "Tax Jurisdiction Code";
        public const string TaxTypeCode = "Tax Type Code";
        public const string TaxableAmount = "Taxable Amount";
        public const string TaxAmount = "Tax Amount";
        public const string InvoiceSummary = "Invoice Summary ";
        public const string InvoiceDate = "Invoice Date";
        public const string BillingPeriod = "Billing Period";
        public const string TotalBilledAmount = "Total Billed Amount";
        public const string NetFees = "Net Fees";
        public const string NetExpenses = "Net Expenses";
        public const string Status = "Status";
        public const string ReviewRoute = "Review Route";
        public const string CurrencyType = "Currency Type";
        public const string GrossAmount = "Gross Amount";
        public const string BilledAmount = "Billed Amount";
        public const string ReviewerAdjustments = "Reviewer Adjustments";
        public const string ITPAdjustments = "ITP Adjustments";
        public const string Subtotal = "Subtotal";
        public const string Tax = "Tax";
        public const string Discounts = "Discounts";
        public const string ProposedCredit = "Proposed Credit";
        public const string TotalwithCredit = "Total with Credit";
        public const string Dollar = "$";
        public const string KeyFields = "Key Fields";
        public const string DetailFields = "Detail Fields";

        //Flag Priority
        public const string High = "FlagInvoiceHigh";
        public const string Medium = "FlagInvoiceMedium";
        public const string Low = "FlagInvoiceLow";
        public const string LineHigh = "FlagLineItemHigh";
        public const string LineLow = "FlagLineItemLow";

        //Reviewer status
        public const string Reviewed = "Reviewed";
        public const string InReview = "InReview";
        public const string YetToReview = "YetToReview";

        //date format
        public const string DateFormat1 = "M/d/yyyy";
        public const string DateFormat2 = "MM/dd/yyyy";

        //Backgroumd brush keys
        public const string EvenBrush = "EvenBrush";
        public const string OddBrush = "OddBrush";
        public const string EvenBrushSummary = "EvenBrushSummary";
        public const string OddBrushSummary = "OddBrushSummary";

        //Page type
        public const string MainRegionName = "MainRegion";
        public const string SubRegionName = "SubRegion";
        public const string Popup = "Popup";
        public const string Tile = "Tile";

        //key code
        public const string KeyCode190 = "190";
        public const string KeyCode180 = "180";
        public const string KeyCode188 = "188";

        //property name
        public const string Warning = "warning";
        public const string Approved = "approved";
        public const string ShowAppBar = "ShowAppBar";
        public const string ShowSortListButton = "ShowSortListButton";
        public const string ShowCancelButton = "ShowCancelButton";
        public const string ShowApproveButton = "ShowApproveButton";
        public const string ShowAdjustButton = "ShowAdjustButton";
        public const string ShowRejectButton = "ShowRejectButton";
        public const string ShowUndoButton = "ShowUndoButton";
        public const string ShowAddNotesButton = "ShowAddNotesButton";
        public const string ShowDetailsButton = "ShowDetailsButton";
        public const string ShowPrivacyPolicyButton = "ShowPrivacyPolicyButton";
        public const string ShowSelectAllButton = "ShowSelectAllButton";
        public const string ShowDocumentsButton = "ShowDocumentsButton";
        public const string ShowClearButton = "ShowClearButton";
        public const string ShowExpandButton = "ShowExpandButton";

        //confirmation messages
        public const string NegativeInvoiceTitle = "Approve With Warnings";
        public const string AdjustConfirmationMsg = "Are you sure you want to adjust this invoice?";
        public const string ApproveConfirmationMsg = "Are you sure you want to approve this invoice?";
        public const string ApproveMultiConfirmationMsg = "Are you sure you want to approve these invoices?";
        public const string RejectConfirmationMsg = "Are you sure you want to reject  this invoice?";
        public const string LogoffConfirmationMsg = "Are you sure you want to logoff?";
        public const string NegativeInvoiceConfirmationMsg = "Adjustments has resulted in a negative invoice balance";
        public const string LogOutMsg = "Log Out of Tymetrix";

        //json property name
        public const string SelectedInvoiceIds = "SelectedInvoiceIds";
        public const string ForceApprove = "ForceApprove";
        public const string Disallow = "Disallow";
        public const string Failed = "Failed";
        public const string Failure = "Failure";
        public const string DisallowHeader = "Multi Approve Disallow";
        public const string WarningHeader = "Multi Approve Warning";
    }
}
