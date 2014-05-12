/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    [DataContract]
    public class InvoiceSummaryDetails : InvoiceBasicInfo
    {
        [DataMember(Name = "BillingPeriod")]
        public string BillingPeriod { get; set; }

        [DataMember(Name = "TotalBilledAmount")]
        public string TotalBilledAmount { get; set; }

        [DataMember(Name = "Flags")]
        public List<FlagDetails> Flags { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "ProcessingDiscount")]
        public string ProcessingDiscount { get; set; }

        [DataMember(Name = "TaxCredit")]
        public string TaxCredit { get; set; }

        [DataMember(Name = "ReviewRouteList")]
        public List<ReviewRouteList> ReviewRouteList { get; set; }

        //Vendor Billing
        [DataMember(Name = "CurrencyType")]
        public string CurrencyType { get; set; }

        [DataMember(Name = "GrossAmount")]
        public string GrossAmount { get; set; }

        [DataMember(Name = "VendorAdjustment")]
        public string VendorAdjustment { get; set; }

        // In-House Review
        [DataMember(Name = "ReviewerAdjustment")]
        public string ReviewerAdjustment { get; set; }

        [DataMember(Name = "ItpAdjustment")]
        public string ItpAdjustment { get; set; }

        [DataMember(Name = "SubTotal")]
        public string SubTotal { get; set; }

        [DataMember(Name = "Tax")]
        public string Tax { get; set; }

        //Discounts
        [DataMember(Name = "Credit")]
        public string Credit { get; set; }

        [DataMember(Name = "TotalWithCredit")]
        public string TotalWithCredit { get; set; }

        //KeyFields
        [DataMember(Name = "CommonProperties")]
        public List<KeyFields> CommonProperties { get; set; }

        //Detail Fields
        [DataMember(Name = "Properties")]
        public List<DetailField> Properties { get; set; }

        //Notes
       // [DataMember(Name = "Notes")]
       // public List<NotesData> Notes { get; set; }

        //Taxes
        [DataMember(Name = "TaxList")]
        public Dictionary<string, List<ValueList>> TaxList { get; set; }

        //LineItems
        [DataMember(Name = "Date")]
        public string Date { get; set; }

        [DataMember(Name = "TimeKeeper")]
        public string TimeKeeper { get; set; }

        [DataMember(Name = "NarrativeText")]
        public string NarrativeText { get; set; }

        [DataMember(Name = "VendorTask")]
        public string VendorTask { get; set; }

        [DataMember(Name = "VendorActivity")]
        public string VendorActivity { get; set; }

        [DataMember(Name = "VendorUnits")]
        public string VendorUnits { get; set; }

        [DataMember(Name = "VendorRate")]
        public string VendorRate { get; set; }

        [DataMember(Name = "VendorBilledTotal")]
        public string VendorBilledTotal { get; set; }

        [DataMember(Name = "Flagslist")]
        public List<FlagDetails> Flagslist { get; set; }

        [DataMember(Name = "NotesAllowed")]
        public bool NotesAllowed { get; set; }

        //Notes
        [DataMember(Name = "NotesList")]
        public List<NotesData> NotesList { get; set; }

        [DataMember(Name = "AdjustmentsList")]
        public List<AdjustmentListDetails> AdjustmentsList { get; set; }

        //[DataMember(Name = "NetAmount")]
        //public string NetAmount { get; set; }
    }

     public class InvoicePermissions
    {
         [DataMember(Name = "Approve")]
         public bool Approve { get; set; }
         
         [DataMember(Name = "AdjustFee")]
         public bool AdjustFee { get; set; }
         
         [DataMember(Name = "AdjustExpense")]
         public bool AdjustExpense { get; set; }
         
         [DataMember(Name = "Reject")]
         public bool Reject { get; set; }
         
         [DataMember(Name = "AdjustInvoiceAllowed")]
         public bool AdjustInvoiceAllowed { get; set; }
  
         [DataMember(Name = "NegativeAdjust")]
         public string NegativeAdjust { get; set; }

         [DataMember(Name = "PositiveAdjustmentAllowed")]
         public bool PositiveAdjustmentAllowed { get; set; }

         [DataMember(Name = "PositiveAdjustment")]
         public bool PositiveAdjustment{ get; set; }

         [DataMember(Name = "UndoEnabled")]
         public bool UndoEnabled { get; set; }

         [DataMember(Name = "Undo")]
         public bool Undo { get; set; }

         [DataMember(Name = "AdjustAllowed")]
         public bool AdjustAllowed { get; set; }

         [DataMember(Name = "Adjust")]
         public bool Adjust { get; set; }

         [DataMember(Name = "Notes")]
         public bool Notes { get; set; }

         [DataMember(Name = "Properties")]
         public bool Properties { get; set; }

         [DataMember(Name = "LineItemsMultipleAdjust")]
         public bool LineItemsMultipleAdjust { get; set; }

         [DataMember(Name = "LineItemsMultipleAdjustAllowed")]
         public bool LineItemsMultipleAdjustAllowed { get; set; }

         [DataMember(Name = "LineItemsMultipleReject")]
         public bool LineItemsMultipleReject { get; set; }

         [DataMember(Name = "LineItemsMultipleUndo")]
         public bool LineItemsMultipleUndo { get; set; }
    }

     [DataContract]
     public class KeyFields
     {
         [DataMember(Name = "LabelText")]
         public string LabelText { get; set; }

         [DataMember(Name = "ValueText")]
         public string ValueText { get; set; }
     }

     [DataContract]
     public class DetailField
     {
         [DataMember(Name = "LabelText")]
         public string LabelText { get; set;  }

         [DataMember(Name = "ValueText")]
         public string ValueText { get; set; }
     }

     [DataContract]
     public class ReviewRouteList
     {
         [DataMember(Name = "ReviewStatus")]
         public string ReviewStatus { get; set; }

         [DataMember(Name = "ReviewerName")]
         public string ReviewerName { get; set; }

         public Uri ImageTitle
         {
             get;
             set;
         }
     }

     [DataContract]
     public class NotesData
     {
         [DataMember(Name = "Creator")]
         public string Creator { get; set; }

         [DataMember(Name = "CreatedTime")]
         public string CreatedTime { get; set; }

         [DataMember(Name = "Description")]
         public string Description { get; set; }

         [DataMember(Name = "Owner")]
         public string Owner { get; set; }

         [DataMember(Name = "Date")]
         public string Date { get; set; }

         private Visibility isButtonRow = Visibility.Collapsed;
         [DataMember(Name = "IsButtonRow")]
         public Visibility IsButtonRow
         {
             get { return isButtonRow; }
             set { isButtonRow = value; }
         }

         private Visibility isNotesRow = Visibility.Visible;
         [DataMember(Name = "IsNotesRow")]
         public Visibility IsNotesRow
         {
             get { return isNotesRow; }
             set { isNotesRow = value; }
         }

         private Visibility isInvoice = Visibility.Collapsed;
         [DataMember(Name = "IsInvoice")]
         public Visibility IsInvoice
         {
             get { return isInvoice; }
             set { isInvoice = value; }
         }

         private Visibility isLineItem = Visibility.Visible;
         [DataMember(Name = "IsLineItem")]
         public Visibility IsLineItem
         {
             get { return isLineItem; }
             set { isLineItem = value; }
         }
     }

    [DataContract]
    public class ValueList
    {
        [DataMember(Name = "TaxJurisdictionCode")]
        public string TaxJurisdictionCode { get; set; }

        [DataMember(Name = "TaxTypeCode")]
        public string TaxTypeCode { get; set; }

        [DataMember(Name = "TaxRate")]
        public string TaxRate { get; set; }

        [DataMember(Name = "TaxableAmount")]
        public string TaxableAmount { get; set; }

        [DataMember(Name = "TaxAmount")]
        public string TaxAmount { get; set; }

        public string Key { get; set; }
    }

    [DataContract]
    public class AdjustmentListDetails
    {
        [DataMember(Name = "Amount")]
        public string Amount { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "GroupDescription")]
        public string GroupDescription { get; set; }

        [DataMember(Name = "Owner")]
        public string Owner { get; set; }

        [DataMember(Name = "DescriptionVisible")]
        public Visibility DescriptionVisible { get; set; }

        [DataMember(Name = "OwnerVisible")]
        public Visibility OwnerVisible { get; set; }

        [DataMember(Name = "HeaderVisible")]
        public Visibility HeaderVisible
        {
            get { return !string.IsNullOrEmpty(GroupDescription) ? Visibility.Visible : Visibility.Collapsed; }
            set { }
        }
    }
}