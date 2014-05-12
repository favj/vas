/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using TyMetrix360.BusinessObjects.Common;

namespace TyMetrix360.BusinessObjects.Invoice
{
    public class InvoiceAdjustment : BusinessObjectCore
    {
        private const string Expenses = "Expenses";
        private const string Fees = "Fees";
        private const string AdjustBy = "Adjust By";
        private const string AdjustTo = "Adjust To";
        private const string Dollar = "$";
        private const string Percentage = "%";

        private string _adjustmentAmount;
        public string AdjustmentAmount
        {
            get { return _adjustmentAmount; }
            set { SetProperty(ref _adjustmentAmount, value); }
        }
        private AdjustmentToggleType _adjustmentMode;
        public AdjustmentToggleType AdjustmentMode
        {
            get { return _adjustmentMode; }
            set { SetProperty(ref _adjustmentMode, value); }
        }
        private double _adjustmentNumber;
        public double  AdjustmentNumber
        {
            get { return _adjustmentNumber; }
            set { SetProperty(ref _adjustmentNumber, value); }
        }
        public string AdjustmentModeText
        {
            get { return (this.AdjustmentMode == AdjustmentToggleType.Expenses ? Expenses : Fees); }
        }
        private AdjustmentSign _positiveOrNegative;
        public AdjustmentSign PositiveOrNegative
        {
            get { return _positiveOrNegative; }
            set { SetProperty(ref _positiveOrNegative, value); }
        }
        private AdjustmentByTo _adjustmentStyle;
        public AdjustmentByTo AdjustmentStyle
        {
            get { return _adjustmentStyle; }
            set { SetProperty(ref _adjustmentStyle, value); }
        }
        public string AdjustmentStyleText
        {
            get { return (this.AdjustmentStyle == AdjustmentByTo.AdjustBy ? AdjustBy : AdjustTo); }
        }
        private AdjustmentType _adjustmentType;
        public AdjustmentType AdjustmentType
        {
            get { return _adjustmentType; }
            set { SetProperty(ref _adjustmentType, value);}
        }
        private string _invoiceId;
        public string InvoiceId
        {
            get { return _invoiceId; }
            set { SetProperty(ref _invoiceId, value); }
        }
        private string _netTotal;
        public string NetTotal
        {
            get { return _netTotal; }
            set { SetProperty(ref _netTotal, value); }
        }
        public string AdjustmentTypeText
        {
            get { return (AdjustmentType == AdjustmentType.Percentage ? Percentage : Dollar); }
        }
        private string _narrativeText;
        public string NarrativeText
        {
            get { return _narrativeText; }
            set { SetProperty(ref _narrativeText, value); }
        }
        private int _reasonId;
        public int ReasonId
        {
            get { return _reasonId; }
            set { SetProperty(ref _reasonId, value); }
        }
        private string _currencySymbol;
        public string CurrencySymbol
        {
            get { return _currencySymbol; }
            set { SetProperty(ref _currencySymbol, value); }
        }
    }
}
