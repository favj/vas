/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using TyMetrix360.Core.Interfaces;

namespace TyMetrix360.BusinessObjects.Common
{
    public class InvoiceListDisplayFields : InvoiceCore, ISupportRowIndex
    {
        private int _index;
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }   
        private string _currencyType;
        public string CurrencyType
        {
            get { return _currencyType; }
            set { SetProperty(ref _currencyType, value); }
        }
    }
}
