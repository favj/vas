/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

namespace Tymetrix.T360.Mobile.Client.Model.Base
{
    public class Constants
    {
        //Event Tokens 
        public const string InvoiceMultiCheckChange = "InvoiceMultiCheckChange";
        public const string LineItemMultiCheckChange = "LineItemMultiCheckChange";

        //Constants
        public const string Warning = "Warning";
        public const string Failure = "Failure";
        public const string Disallow = "Disallow";
        public const string WarningHeader = "Multi Approve Warning";
        public const string DisallowHeader = "Multi Approve Disallow";
        public const string SelectedInvoiceIds = "SelectedInvoiceIds";
        public const string ForceApprove = "ForceApprove";
        public const string Proceed = "Proceed";
        public const string Cancel = "Cancel";
        public const string InvoiceApprovalStatus = "G";
        public const string LineItemPivotHeader = "line items";

        //icon path
        public const string MultiSelectIconPath = "Resources\\images\\MultiSelect.png";
        public const string SearchIconPath = "Resources\\images\\Search.png";
        public const string ApproveIconPath = "Resources\\images\\Approve.png";
        public const string RejectIconPath = "Resources\\images\\Reject.png";
        public const string AdjustIconPath = "Resources\\images\\adjust.png";
        public const string HighPriorityFlagIconPath = "/Resources/images/T360_Flags_High_Priority@2x.png";
        public const string MediumPriorityFlagIconPath = "/Resources/images/T360_Flags_Medium_Priority@2x.png";
        public const string LowPriorityFlagIconPath = "/Resources/images/T360_Flags_Low_Priority@2x.png";
        public const string CheckedIconPath = "/Resources/images/Checked.png";
        public const string UncheckedIconPath = "/Resources/images/Unchecked.png";
        public const string ActionItemIconPath = "Resources\\images\\ActionItem.png";
        public const string LeftArrowIconPath = "Resources\\images\\LeftArrow.png";
        public const string RightArrowIconPath = "Resources\\images\\RightArrow.png";

        //App bar title
        public const string MultiSelectTitle = "Multi-Select";
        public const string SearchTitle = "Search";
        public const string ApproveTitle = "Approve";
        public const string RejectTitle = "Reject";
        public const string AdjustTitle = "Adjust";
        public const string ActionItemTitle = "Action";
        public const string LeftArrowTitle = "Previous";
        public const string RightArrowTitle = "Next";

        //error titles
        public const string InvoiceSummaryError = "Invoice Summary Failed";
        public const string ApproveError = "Invoice Approve Failed";
        public const string InvoiceReasonsError = "Invoice Reasons Failed";
        public const string LineItemListError = "Line Item List Failed";
        public const string LineItemSummaryError = "Line Item Detail Failed";
        public const string LineItemReasonError = "Line Item Reasons Failed";
        public const string InvoiceListError = "Invoice list Failed";
        public const string RejectLineItemError = "Line Item Reject Failed";
        public const string RejectInvoiceError = "Invoice Reject Failed";

        //invoice summary headers and labels
        public const string InvoiceSummaryHeader = "Invoice Summary";
        public const string InvoiceDateLabel = "Invoice Date";
        public const string BilledPeriodLabel = "Billed Period";
        public const string TotalBilledAmountLabel = "Total Billed Amount";
        public const string NetFeesLabel = "Net Fees";
        public const string NetExpensesLabel = "Net Expenses";
        public const string NetAmountLabel = "Net Amount";
        public const string FlagsLabel = "Flags";
        public const string StatusLabel = "Status";

        //invoice summary flag section headers and labels
        public const string FlagSectionHeader = "Flags";
        public const string FlagInvoiceHighPriority = "FlagInvoiceHigh";
        public const string FlagInvoiceMediumPriority = "FlagInvoiceMedium";
        public const string FlagInvoiceLowPriority = "FlagInvoiceLow";

        //invoice summary review route section headers and labels
        public const string ReviewRouteHeader = "Review Route";
        public const string ReviewStatusReviewed = "Reviewed";
        public const string ReviewStatusInreview = "InReview";
        public const string ReviewStatusYetToReview = "YetToReview";
        public const string CurrentReviewerLabel = "Current Reviewer";

        //invoice summary firm vendor billing headers and billing
        public const string FirmVendorBillingHeader = "Firm/Vendor Billing";
        public const string CurrencyTypeLabel = "Currency Type";
        public const string GrossAmountLabel = "Gross Amount";
        public const string VendorAdjustmentLabel = "Vendor Adjustment";
        public const string BilledAmountLabel = "Billed Amount";

        //invoice summary in-house review headers and billing
        public const string InHouseReviewHeader = "In-House Review";
        public const string ReviewerAdjustmentsLabel = "Reviewer Adjustments";
        public const string ITPAdjustmentsLabel = "ITP Adjustments";
        public const string SubtotalLabel = "Subtotal";
        public const string TaxLabel = "Tax";
        public const string NetTotalLabel = "Net Total";

        //invoice summary discounts headers and billing
        public const string DiscountsHeader = "Discounts";
        public const string ProposedCreditLabel = "Proposed Credit";
        public const string TotalWithCreditLabel = "Total With Credit";

        //invoice summary key-fields headers and billing
        public const string KeyFieldsHeader = "Key Fields";

        //invoice summary detail-fields headers and billing
        public const string DetailFieldsHeader = "Detail Fields";

        //invoice summary notes headers and billing
        public const string NotesHeader = "Notes";

        //invoice summary tax headers and billing
        public const string TaxJurisdictionCodeLabel = "Tax Jurisdiction Code";
        public const string TaxTypeCodeLabel = "Tax Type Code";
        public const string TaxRateLabel = "Tax Rate";
        public const string TaxableAmountLabel = "Taxable Amount";
        public const string TaxAmountLabel = "Tax Amount";

        //Confirmation Messages
        public const string SingleApproveInvoiceMsg = "Are you sure you want to approve this invoice?";
        public const string NegativeInvoiceBalanceMsg = "Adjustments has resulted in a negative invoice balance";
        public const string TaxedInvoiceMsg = "This invoice contains taxes and cannot be adjusted.";
        public const string TaxedLineItemMsg = "This line item contains taxes and cannot be adjusted.";
        public const string TaxedMultiLineItemMsg = "These line items contains taxes and cannot be adjusted.";

        //confirmation dialog titles
        public const string ApprovalConfirmationTitle = "Approval Confirmation";
        public const string NegativeInvoiceBalanceTitle = "Approve With Warnings";
        public const string TaxedInvoiceTitle = "Adjustment not Permitted";

        //Reason code actions
        public const string RejectAction = "Reject";
        public const string RejectLineItemAction = "RejectLineItem";
        public const string AdjustFeesAction = "AdjustFees";
        public const string AdjustMultiLineItemAction = "AdjustSelectedLineItems";
        public const string AdjustLineItemAction = "AdjustLineItem";

        //Line item summary headers and labels
        public const string ItemsDetailsTitle = "Item Details";
        public const string ItemsDetailsHeader = "General";
        public const string DateLabel = "Date";
        public const string TimeKeeperLabel = "Time Keeper";
        public const string AmountLabel = "Amount";
        public const string NarrativeHeader = "Narrative";

        //Firm/Vendor Billing headers and labels
        public const string TaskLabel = "Task";
        public const string ActivityLabel = "Activity";
        public const string UnitsLabel = "Units/Hours";
        public const string RateLabel = "Rate";
        public const string BilledTotalLabel = "Billed Total";

        //Line Items summary flags labels
        public const string FlagLineItemHighPriority = "FlagLineItemHigh";
        public const string FlagLineItemMediumPriority = "FlagLineItemMedium";
        public const string FlagLineItemLowPriority = "FlagLineItemLow";

        //Line Items summary Adjustment headers
        public const string AdjustmentHeader = "Adjustments";
   
        //uri string
        public const string ExternalURI = "app://external/";
    }
}
