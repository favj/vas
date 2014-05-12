/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
namespace TyMetrix360.BusinessObjects.Common
{
    public class InvoiceCore : BusinessObjectCore
    {
        private bool _isConcealedMatter;
        public bool IsConcealedMatter
        {
            get { return _isConcealedMatter; }
            set { SetProperty(ref _isConcealedMatter, value); }
        }
        private int _flagsCount;
        public int FlagsCount
        {
            get { return _flagsCount; }
            set { SetProperty(ref _flagsCount, value); }
        }
        private string _daysInQueue;
        public string DaysInQueue
        {
            get { return _daysInQueue; }
            set { SetProperty(ref _daysInQueue, value); }
        }
        private string _billedAmount;
        public string BilledAmount
        {
            get { return _billedAmount; }
            set { SetProperty(ref _billedAmount, value); }
        }
        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set { SetProperty(ref _companyName, value); }
        }
        private string _invoiceDate;
        public string InvoiceDate
        {
            get { return _invoiceDate; }
            set { SetProperty(ref _invoiceDate, value); }
        }
        private int _invoiceId;
        public int InvoiceId
        {
            get {  return _invoiceId;  }
            set {  SetProperty(ref _invoiceId, value); }
        }
        private string _invoiceNumber;
        public string InvoiceNumber
        {
            get{ return _invoiceNumber; }
            set{ SetProperty(ref _invoiceNumber, value);}
        }
        private string _matterName;
        public string MatterName
        {
            get { return _matterName;}
            set {  SetProperty(ref _matterName, value); }
        }
        private string _matterNumber;
        public string MatterNumber
        {
            get { return _matterNumber; }
            set { SetProperty(ref _matterNumber, value); }
        }
        private string _netAmount;
        public string NetAmount
        {
            get { return _netAmount; }
            set { SetProperty(ref _netAmount, value); }
        }
    }
}
