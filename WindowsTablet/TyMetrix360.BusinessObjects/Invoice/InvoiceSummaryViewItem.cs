/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.ObjectModel;

using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.Core.Interfaces;

namespace TyMetrix360.BusinessObjects.Invoice
{
    public class InvoiceSummaryViewItem : BusinessObjectCore, ISupportRowIndex
    {
        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set { SetProperty(ref _symbol, value); }
        }
        private string _header;
        public string Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }
        private int _index;
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }
        private string _value = string.Empty;
        public string Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        private ObservableCollection<SummaryViewSet> _SummaryViewSets;
        public ObservableCollection<SummaryViewSet> SummaryViewSets
        {
            get 
            {
                if (_SummaryViewSets == null)
                {
                    _SummaryViewSets = new ObservableCollection<SummaryViewSet>();
                }
                return _SummaryViewSets; 
            }
            set { SetProperty(ref _SummaryViewSets, value); }
        }
         
    }
}
