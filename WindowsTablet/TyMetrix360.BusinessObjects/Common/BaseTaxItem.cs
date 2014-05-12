/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
namespace TyMetrix360.BusinessObjects.Common
{
    public class BaseTaxItem : BusinessObjectCore
    {
        private string _key;
        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }
        private string _taxableAmount;
        public string TaxableAmount
        {
            get { return _taxableAmount; }
            set { SetProperty(ref _taxableAmount, value); }
        }
        private string _taxAmount;
        public string TaxAmount
        {
            get { return _taxAmount; }
            set { SetProperty(ref _taxAmount, value); }
        }
        private string _taxJurisdictionCode;
        public string TaxJurisdictionCode
        {
            get { return _taxJurisdictionCode; }
            set { SetProperty(ref _taxJurisdictionCode, value); }
        }
        private string _taxTypeCode;
        public string TaxTypeCode
        {
            get { return _taxTypeCode; }
            set { SetProperty(ref _taxTypeCode, value); }
        }
        private string _taxRate;
        public string TaxRate
        {
            get { return _taxRate; }
            set { SetProperty(ref _taxRate, value); }
        }
    }
}
