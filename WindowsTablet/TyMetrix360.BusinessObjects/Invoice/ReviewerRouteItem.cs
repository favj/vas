/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using TyMetrix360.BusinessObjects.Common;

namespace TyMetrix360.BusinessObjects.Invoice
{
    public class ReviewerRouteItem : BusinessObjectCore
    {
        private string _reviewStatus;
        public string ReviewStatus
        {
            get { return _reviewStatus; }
            set { SetProperty(ref _reviewStatus, value); }
        }
        private string _reviewerName;
        public string ReviewerName
        {
            get { return _reviewerName; }
            set { SetProperty(ref _reviewerName, value); }
        }
    }
}
