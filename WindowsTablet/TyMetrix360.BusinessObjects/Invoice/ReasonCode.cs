/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using TyMetrix360.BusinessObjects.Common;

namespace TyMetrix360.BusinessObjects.Invoice
{
    public class ReasonCode : BusinessObjectCore
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
    }
}
