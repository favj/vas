/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;
using System.Collections.ObjectModel;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.Core.Interfaces;

namespace TyMetrix360.BusinessObjects.Invoice
{
    public class InvoiceSummary : InvoiceCore, ISupportRowIndex
    {
        private string _billedExpenses;
        public string BilledExpenses
        {
            get { return _billedExpenses; }
            set { SetProperty(ref _billedExpenses, value); }
        }
        private string _originalBilledExpenses;
        public string OriginalBilledExpenses
        {
            get { return _originalBilledExpenses; }
            set { SetProperty(ref _originalBilledExpenses, value); }
        }
        private string _billedFees;
        public string BilledFees
        {
            get { return _billedFees; }
            set { SetProperty(ref _billedFees, value); }
        }
        private string _originalBilledFees;
        public string OriginalBilledFees
        {
            get { return _originalBilledFees; }
            set { SetProperty(ref _originalBilledFees, value); }
        }
        private string _billingPeriod;
        public string BillingPeriod
        {
            get { return _billingPeriod; }
            set { SetProperty(ref _billingPeriod, value); }
        }
        private ObservableCollection<BaseTyProperty> _commonProperties;
        public ObservableCollection<BaseTyProperty> CommonProperties
        {
            get { _commonProperties = _commonProperties ?? new ObservableCollection<BaseTyProperty>(); return _commonProperties; }
            set { SetProperty(ref _commonProperties, value); }
        }
        private string _credit;
        public string Credit
        {
            get { return _credit; }
            set { SetProperty(ref _credit, value); }
        }
        private string _currencySymbol;
        public string CurrencySymbol
        {
            get { return _currencySymbol; }
            set { SetProperty(ref _currencySymbol, value); }
        }
        private string _currencyType;
        public string CurrencyType
        {
            get { return _currencyType; }
            set { SetProperty(ref _currencyType, value); }
        }
        private ObservableCollection<FlagDetails> _flagsList;
        public ObservableCollection<FlagDetails> Flags
        {
            get { return (_flagsList == null) ? new ObservableCollection<FlagDetails>() : _flagsList; }
            set { SetProperty(ref _flagsList, value); }
        }
        private Dictionary<string, ObservableCollection<BaseTaxItem>> _taxList;
        public Dictionary<string, ObservableCollection<BaseTaxItem>> TaxList
        {
            get { return (_taxList == null) ? new Dictionary<string, ObservableCollection<BaseTaxItem>>() : _taxList; }
            set { SetProperty(ref _taxList, value); }
        }
        private ObservableCollection<Note> _notesList;
        public ObservableCollection<Note> NotesList
        {
            get { return (_notesList == null) ? new ObservableCollection<Note>() : _notesList; }
            set { SetProperty(ref _notesList, value); }
        }
        private string _grossAmount;
        public string GrossAmount
        {
            get { return _grossAmount; }
            set { SetProperty(ref _grossAmount, value); }
        }
        private string _itpAdjustment;
        public string ItpAdjustment
        {
            get { return _itpAdjustment; }
            set { SetProperty(ref _itpAdjustment, value); }
        }
        private int _index;
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }   

         private string _narrative;
        public string Narrative
        {
            get { return _narrative; }
            set { SetProperty(ref _narrative, value); }
        }
        private string _netTotal;
        public string NetTotal
        {
            get { return _netTotal; }
            set { SetProperty(ref _netTotal, value); }
        }
        private ObservableCollection<Note> _notes;
        public ObservableCollection<Note> Notes
        {
            get { return _notes = _notes ?? new ObservableCollection<Note>(); return _notes; }
            set { SetProperty(ref _notes, value); }
        }
        private Permissions _permissions;
        public Permissions Permissions
        {
            get { return _permissions; }
            set { SetProperty(ref _permissions, value); }
        }
        private string _processingDiscount;
        public string ProcessingDiscount
        {
            get { return _processingDiscount; }
            set { SetProperty(ref _processingDiscount, value); }
        }
        private ObservableCollection<BaseTyProperty> _properties;
        public ObservableCollection<BaseTyProperty> Properties
        {
            get { _properties = _properties ?? new ObservableCollection<BaseTyProperty>(); return _properties; }
            set { SetProperty(ref _properties, value); }
        }
        private ObservableCollection<ReviewerRouteItem> _reviewRouteList;
        public ObservableCollection<ReviewerRouteItem> ReviewRouteList
        {
            get { _reviewRouteList = _reviewRouteList ?? new ObservableCollection<ReviewerRouteItem>(); return _reviewRouteList; }
            set { SetProperty(ref _reviewRouteList, value); }
        }
        private string _reviewerAdjustment;
        public string ReviewerAdjustment
        {
            get { return _reviewerAdjustment; }
            set { SetProperty(ref _reviewerAdjustment, value); }
        }
        private string _status;
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }
        private string _subTotal;
        public string SubTotal
        {
            get { return _subTotal; }
            set { SetProperty(ref _subTotal, value); }
        }
        private string _tax;
        public string Tax
        {
            get { return _tax; }
            set { SetProperty(ref _tax, value); }
        }
        private string _taxCredit;
        public string TaxCredit
        {
            get { return _taxCredit; }
            set { SetProperty(ref _taxCredit, value); }
        }
        private string _totalBilledAmount;
        public string TotalBilledAmount
        {
            get { return _totalBilledAmount; }
            set { SetProperty(ref _totalBilledAmount, value); }
        }

        private string _totalWithCredit;
        public string TotalWithCredit
        {
            get { return _totalWithCredit; }
            set { SetProperty(ref _totalWithCredit, value); }
        }
        private string _vendorAdjustment;
        public string VendorAdjustment
        {
            get { return _vendorAdjustment; }
            set { SetProperty(ref _vendorAdjustment, value); }
        }
        private ObservableCollection<InvoiceSummaryViewItem> _invoiceSummaryViewItemList;
        public ObservableCollection<InvoiceSummaryViewItem> InvoiceSummaryViewItemList
        {
            get 
            {
                if (_invoiceSummaryViewItemList == null)
                {
                    _invoiceSummaryViewItemList = new ObservableCollection<InvoiceSummaryViewItem>();
                }
                return _invoiceSummaryViewItemList; 
            }
            set { SetProperty(ref _invoiceSummaryViewItemList, value); }
        }
    }
}
