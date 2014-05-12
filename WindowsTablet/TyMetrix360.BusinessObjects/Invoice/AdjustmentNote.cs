/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using TyMetrix360.BusinessObjects.Common;

namespace TyMetrix360.BusinessObjects.Invoice
{
    public class AdjustmentNote : BusinessObjectCore
    {
        private string _amount;
        public string Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }
        private string _description;
        public string Description 
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
        private string _groupDescription;
        public string GroupDescription
        {
            get { return _groupDescription; }
            set { SetProperty(ref _groupDescription, value); }
        }
        private string _owner;
        public string Owner 
        {
            get { return _owner; }
            set { SetProperty(ref _owner, value); }
        
        }
    }
}
