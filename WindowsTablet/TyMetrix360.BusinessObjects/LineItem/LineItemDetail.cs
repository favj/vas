/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;
using System.Collections.ObjectModel;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.BusinessObjects.Invoice;
using TyMetrix360.Core.Interfaces;

namespace TyMetrix360.BusinessObjects.LineItem
{
    public class LineItemDetail : BusinessObjectCore,  ISupportRowIndex
    {
        private ObservableCollection<AdjustmentNote> _adjustmentsList;
        public ObservableCollection<AdjustmentNote> AdjustmentsList
        {
            get { return _adjustmentsList; }
            set { SetProperty(ref _adjustmentsList, value); }
        }
        private string _netAmount;
        public string NetAmount
        {
            get { return _netAmount; }
            set { SetProperty(ref _netAmount, value); }
        }
        private string _originalNetTotal;
        public string OriginalNetTotal
        {
            get { return _originalNetTotal; }
            set { SetProperty(ref _originalNetTotal, value); }
        }
        private string _narrativeText;
        public string NarrativeText
        {
            get { return _narrativeText; }
            set { SetProperty(ref _narrativeText, value); }
        }
        private string _currencySymbol;
        public string CurrencySymbol
        {
            get { return _currencySymbol; }
            set { SetProperty(ref _currencySymbol, value); }
        }
        private string _itpAdjustment;
        public string ItpAdjustment
        {
            get { return _itpAdjustment; }
            set { SetProperty(ref _itpAdjustment, value); }
        }
        private string _reviewerAdjustment;
        public string ReviewerAdjustment
        {
            get { return _reviewerAdjustment; }
            set { SetProperty(ref _reviewerAdjustment, value); }
        }
        private string _netTotal;
        public string NetTotal
        {
            get { return _netTotal; }
            set { SetProperty(ref _netTotal, value); }
        }
        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set { SetProperty(ref _companyName, value); }
        }
        private string _date;
        public string Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }
        private string _flagCount;
        public string FlagCount
        {
            get { return _flagCount; }
            set { SetProperty(ref _flagCount, value); }
        }
        private ObservableCollection<FlagDetails> _flagsList;
        public ObservableCollection<FlagDetails> Flagslist
        {
            get { return _flagsList; }
            set { SetProperty(ref _flagsList, value); }
        }
        private int _index;
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }
        private int _invoiceId;
        public int InvoiceId
        {
            get { return _invoiceId; }
            set { SetProperty(ref _invoiceId, value); }
        }
        private int _lineItemId;
        public int LineItemId
        {
            get { return _lineItemId; }
            set { SetProperty(ref _lineItemId, value); }
        }
        private string _narrative;
        public string Narrative
        {
            get { return _narrative; }
            set { SetProperty(ref _narrative, value); }
        }
        private ObservableCollection<Note> _notes;
        public ObservableCollection<Note> NotesList
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }
        private Dictionary<string, ObservableCollection<BaseTaxItem>> _taxList;
        public Dictionary<string, ObservableCollection<BaseTaxItem>> Taxlist
        {
            get { return _taxList; }
            set { SetProperty(ref _taxList, value); }
        }
        private string _timeKeeper;
        public string TimeKeeper
        {
            get { return _timeKeeper; }
            set { SetProperty(ref _timeKeeper, value); }
        }
        private string _vendorActivity;
        public string VendorActivity 
        {
            get { return _vendorActivity; }
            set { SetProperty(ref _vendorActivity, value); }
        }
        private string _vendorAdjustment;
        public string VendorAdjustment 
        {
            get { return _vendorAdjustment; }
            set { SetProperty(ref _vendorAdjustment, value); }
        }
        private string _vendorBilledTotal;
        public string VendorBilledTotal
        {
            get { return _vendorBilledTotal; }
            set { SetProperty(ref _vendorBilledTotal, value); }
        }
        private string _vendorRate;
        public string VendorRate
        {
            get { return _vendorRate; }
            set { SetProperty(ref _vendorRate, value); }
        }
        private string _vendorTask;
        public string VendorTask
        {
            get { return _vendorTask; }
            set { SetProperty(ref _vendorTask, value); }
        }
        private string _vendorUnits;
        public string VendorUnits   
        {
            get { return _vendorUnits; }
            set { SetProperty(ref _vendorUnits, value); }
        }
        private Permissions _permissions;
        public Permissions Permissions   
        {
            get { return _permissions; }
            set { SetProperty(ref _permissions, value); }
        }
    }
}
