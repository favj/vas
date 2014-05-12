/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
namespace TyMetrix360.BusinessObjects.Common
{
    public class Permissions : BusinessObjectCore
    {
        private bool _adjustExpense;
        public bool AdjustExpense
        {
            get { return _adjustExpense; }
            set { SetProperty(ref _adjustExpense, value); }
        }
        private bool _adjustFee;
        public bool AdjustFee
        {
            get { return _adjustFee; }
            set { SetProperty(ref _adjustFee, value); }
        }
        private bool _adjustInvoiceAllowed;
        public bool AdjustInvoiceAllowed
        {
            get { return _adjustInvoiceAllowed; }
            set { SetProperty(ref _adjustInvoiceAllowed, value); }
        }
        private bool _approve;
        public bool Approve
        {
            get { return _approve; }
            set { SetProperty(ref _approve, value); }
        }
        private bool? _negativeAdjust;
        public bool? NegativeAdjust
        {
            get { return _negativeAdjust.Value; }
            set
            {
                if (value == null || !value.HasValue)
                {
                    _negativeAdjust = false;
                }
                SetProperty(ref _negativeAdjust, value);
            }
        }
        private bool _notes;
        public bool Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }
        private bool _positiveAdjustmentAllowed;
        public bool PositiveAdjustmentAllowed
        {
            get { return _positiveAdjustmentAllowed; }
            set { SetProperty(ref _positiveAdjustmentAllowed, value); }
        }
        private bool _properties;
        public bool Properties
        {
            get { return _properties; }
            set { SetProperty(ref _properties, value); }
        }
        private bool _reject;
        public bool Reject
        {
            get { return _reject; }
            set { SetProperty(ref _reject, value); }
        }
        private bool _lineItemsMultipleAdjust;
        public bool LineItemsMultipleAdjust
        {
            get { return _lineItemsMultipleAdjust; }
            set { SetProperty(ref _lineItemsMultipleAdjust, value); }
        }
        private bool _lineItemsMultipleAdjustAllowed;
        public bool LineItemsMultipleAdjustAllowed
        {
            get { return _lineItemsMultipleAdjustAllowed; }
            set { SetProperty(ref _lineItemsMultipleAdjustAllowed, value); }
        }
        private bool _lineItemsMultipleReject;
        public bool LineItemsMultipleReject
        {
            get { return _lineItemsMultipleReject; }
            set { SetProperty(ref _lineItemsMultipleReject, value); }
        }
        private bool _lineItemsMultipleUndo;
        public bool LineItemsMultipleUndo
        {
            get { return _lineItemsMultipleUndo; }
            set { SetProperty(ref _lineItemsMultipleUndo, value); }
        }
    }
}
