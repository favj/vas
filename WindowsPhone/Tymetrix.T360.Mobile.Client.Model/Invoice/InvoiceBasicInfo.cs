/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;
using System.Runtime.Serialization;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;
using System.Windows;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{

    [DataContract]
    public class InvoiceBasicInfo
    {
        private byte[] companyName;
        private byte[] matterNumber;
        private byte[] matterName;
        private byte[] netAmount;
        private byte[] invoiceId;
        private byte[] invoiceNumber;
        private byte[] daysInQueue;
        private byte[] flagsCount;
        private bool isVisible;
        private bool isChecked;

        [DataMember(Name = "InvoiceId")]
        public string InvoiceId
        {
            get
            {
                return Vault.Decrypt(invoiceId);
            }
            set
            {
                invoiceId = Vault.Encrypt(value);
            }
        }

        [DataMember(Name = "InvoiceNumber")]
        public string InvoiceNumber
        {
            get
            {
                return Vault.Decrypt(invoiceNumber);
            }
            set
            {
                invoiceNumber = Vault.Encrypt(value);
            }
        }
        [DataMember(Name = "InvoiceDate")]
        public string InvoiceDate { get; set; }

        public string InvoiceNumberToDisplay
        {
            get
            {
                string formattedInvoiceNumber = InvoiceNumber.Length > 25 ? InvoiceNumber.Substring(0, 25) + "..." : InvoiceNumber;
                return "Inv #" + formattedInvoiceNumber;
            }
            set { }
        }

        [DataMember(Name = "CompanyName")]
        public string CompanyName
        {
            get
            {
                return Vault.Decrypt(companyName);
            }
            set
            {
                companyName = Vault.Encrypt(value);
            }
        }

        [DataMember(Name = "MatterNumber")]
        public string MatterNumber
        {
            get
            {
                return Vault.Decrypt(matterNumber);
            }
            set
            {
                matterNumber = Vault.Encrypt(value);
            }
        }

        [DataMember(Name = "MatterName")]
        public string MatterName
        {
            get
            {
                return Vault.Decrypt(matterName);
            }
            set
            {
                matterName = Vault.Encrypt(value);
            }
        }

        [DataMember(Name = "BilledAmount")]
        public string BilledAmount { get; set; }

        [DataMember(Name = "BilledAmountLabel")]
        public string BilledAmountLabel
        {
            get
            {
                string formattedBilledAmount = BilledAmount.Length > 7 ? BilledAmount.Substring(0, 7) + "..." : BilledAmount;
                return "Billed:" + formattedBilledAmount;
            }
            set { }
        }
        [DataMember(Name = "NetAmount")]
        public string NetAmount
        {
            get
            {
                return Vault.Decrypt(netAmount);
            }
            set
            {
                netAmount = Vault.Encrypt(value);
            }
        }

        [DataMember(Name = "NetAmountLabel")]
        public string NetAmountLabel
        {
            get
            {
                string formattedNetAmount = NetAmount.Length > 9 ? NetAmount.Substring(0, 9) + "..." : NetAmount;
                return "Net:" + formattedNetAmount;
            }
            set { }
        }

        [DataMember(Name = "NetTotal")]
        public string NetTotal { get; set; }

        [DataMember(Name = "BilledFees")]
        public string BilledFees { get; set; }

        [DataMember(Name = "BilledExpenses")]
        public string BilledExpenses { get; set; }

        [DataMember(Name = "CurrencySymbol")]
        public string CurrencySymbol { get; set; }

        [DataMember(Name = "LineItemId")]
        public string LineItemId { get; set; }

        [DataMember(Name = "Permissions")]
        public InvoicePermissions Permissions { get; set; }

        [DataMember(Name = "DaysInQueue")]
        public string DaysInQueue { get; set; }

        [DataMember(Name = "FlagVisible")]
        public Visibility FlagVisible
        {
            get { return !"0".Equals(FlagsCount) ? Visibility.Visible : Visibility.Collapsed; }
            set { }
        }

        [DataMember(Name = "DaysInQueueLabel")]
        public string DaysInQueueLabel
        {
            get
            {
                return "Days in Queue(" + DaysInQueue + ")";
            }
            set { }
        }

        [DataMember(Name = "FlagsCount")]
        public string FlagsCount { get; set; }

        [DataMember(Name = "FlagsCountLabel")]
        public string FlagsCountLabel
        {
            get
            {
                return "(" + FlagsCount + ")";
            }
            set { }
        }

        [DataMember(Name = "IsVisibility")]
        public bool IsVisibility
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        [DataMember(Name = "isCheckboxChecked")]
        public bool isCheckboxChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                Messenger.Default.Send<InvoiceBasicInfo>(this, Constants.InvoiceMultiCheckChange);
            }
        }
    }

    [DataContract]
    public class InvoiceLineItemsInfo
    {
        private bool isVisible;
        private bool isChecked;

        public InvoiceLineItemsInfo(string LineItemId)
        {
            this.LineItemId = LineItemId;
        }

        //Line Items
        [DataMember(Name = "TimeKeeper")]
        public string TimeKeeper { get; set; }

        [DataMember(Name = "NarrativeText")]
        public string NarrativeText { get; set; }

        [DataMember(Name = "Amount")]
        public string Amount { get; set; }

        [DataMember(Name = "Date")]
        public string Date { get; set; }

        [DataMember(Name = "Flags")]
        public string Flags { get; set; }

        [DataMember(Name = "LineItemId")]
        public string LineItemId { get; set; }

        public string ListItemFlag
        {
            get
            {
                return "(" + Flags + ")";
            }
            set { }
        }
        [DataMember(Name = "FlagVisible")]
        public Visibility FlagVisible
        {
            get { return !"0".Equals(Flags) ? Visibility.Visible : Visibility.Collapsed; }
            set { }
        }

        [DataMember(Name = "IsVisibility")]
        public bool IsVisibility
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        [DataMember(Name = "isCheckboxChecked")]
        public bool isCheckboxChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                Messenger.Default.Send<InvoiceLineItemsInfo>(this, Constants.LineItemMultiCheckChange);
            }
        }

        [DataMember(Name = "NetTotal")]
        public string NetTotal { get; set; }

        [DataMember(Name = "NetTotalLabel")]
        public string NetTotalLabel
        {
            get
            {
                string formattedNetTotal = NetTotal.Length > 15 ? NetTotal.Substring(0, 15) + "..." : NetTotal;
                return formattedNetTotal;
            }
            set { }
        }
    }
}
