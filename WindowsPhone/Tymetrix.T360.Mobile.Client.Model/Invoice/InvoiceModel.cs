/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using Tymetrix.T360.Mobile.Client.Model.Base;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class InvoiceModel : BaseModel
    {
        private int invoiceId;
        public int InvoiceId
        {
            get { return invoiceId; }
            set { SetProperty(ref invoiceId, value, "InvoiceId"); }
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
        private bool isConcealedMatter;
        public bool IsConcealedMatter
        {
            get { return isConcealedMatter; }
            set { SetProperty(ref isConcealedMatter, value, "IsConcealedMatter"); }
        }
        private string invoiceDate;
        public string InvoiceDate
        {
            get { return invoiceDate; }
            set { SetProperty(ref invoiceDate, value, "InvoiceDate"); }
        }
        private string invoiceNumber;
        public string InvoiceNumber
        {
            get { return invoiceNumber; }
            set { SetProperty(ref invoiceNumber, value, "InvoiceNumber"); }
        }
        private string billedAmount;
        public string BilledAmount
        {
            get { return billedAmount; }
            set { SetProperty(ref billedAmount, value, "BilledAmount"); }
        }
        private string netAmount;
        public string NetAmount
        {
            get { return netAmount; }
            set { SetProperty(ref netAmount, value, "NetAmount"); }
        }
        private string daysInQueue;
        public string DaysInQueue
        {
            get { return daysInQueue; }
            set { SetProperty(ref daysInQueue, value, "DaysInQueue"); }
        }
        private int flagsCount;
        public int FlagsCount
        {
            get { return flagsCount; }
            set { SetProperty(ref flagsCount, value, "FlagsCount"); }
        }

        //view specific
        private Thickness labelMargin;
        public Thickness LabelMargin
        {
            get { return labelMargin; }
            set { SetProperty(ref labelMargin, value, "LabelMargin"); }
        }
        private Thickness billedAmountMargin;
        public Thickness BilledAmountMargin
        {
            get { return billedAmountMargin; }
            set { SetProperty(ref billedAmountMargin, value, "BilledAmountMargin"); }
        }
        private Thickness netAmountMargin;
        public Thickness NetAmountMargin
        {
            get { return netAmountMargin; }
            set { SetProperty(ref netAmountMargin, value, "NetAmountMargin"); }
        }
        private Visibility checkVisible = Visibility.Collapsed;
        public Visibility CheckVisible
        {
            get { return checkVisible; }
            set { SetProperty(ref checkVisible, value, "CheckVisible"); }
        }
        private Visibility flagNotVisible;
        public Visibility FlagNotVisible
        {
            get { return flagNotVisible; }
            set { SetProperty(ref flagNotVisible, value, "FlagNotVisible"); }
        }
        private Visibility flagVisible;
        public Visibility FlagVisible
        {
            get { return flagVisible; }
            set { SetProperty(ref flagVisible, value, "FlagVisible"); }
        }
        private bool isCheckboxChecked = false;
        public bool IsCheckboxChecked
        {
            get { return isCheckboxChecked; }
            set
            {
                SetProperty(ref isCheckboxChecked, value, "IsCheckboxChecked");
                Messenger.Default.Send<InvoiceModel>(this, Constants.InvoiceMultiCheckChange);
            }
        }

        private string billedAmountToDisplay;
        public string BilledAmountToDisplay
        {
            get { return billedAmountToDisplay;  }
            set { SetProperty(ref billedAmountToDisplay, value, "BilledAmountToDisplay"); }
        }
        private string netAmountToDisplay;
        public string NetAmountToDisplay
        {
            get { return netAmountToDisplay; }
            set { SetProperty(ref netAmountToDisplay, value, "NetAmountToDisplay"); }
        }
        private GridLength daysInQueueWidth;
        public GridLength DaysInQueueWidth
        {
            get { return daysInQueueWidth; }
            set { SetProperty(ref daysInQueueWidth, value, "DaysInQueueWidth"); }
        }
        private TextTrimming amountTrim;
        public TextTrimming AmountTrim
        {
            get { return amountTrim; }
            set { SetProperty(ref amountTrim, value, "AmountTrim"); }
        }
        private string daysInQueueToDisplay;
        public string DaysInQueueToDisplay
        {
            get { return daysInQueueToDisplay; }
            set { SetProperty(ref daysInQueueToDisplay, value, "DaysInQueueToDisplay"); }
        }
        private string invoiceNumberToDisplay;
        public string InvoiceNumberToDisplay
        {
            get { return invoiceNumberToDisplay; }
            set { SetProperty(ref invoiceNumberToDisplay, value, "InvoiceNumberToDisplay"); }
        }
        public string FlagsCountToDisplay
        {
            get { return "(" + FlagsCount + ")"; }
            set { }
        }
    }
}
