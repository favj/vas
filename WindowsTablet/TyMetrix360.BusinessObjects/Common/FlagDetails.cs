/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
namespace TyMetrix360.BusinessObjects.Common
{
    public class FlagDetails : BusinessObjectCore
    {
        private string _displayName;
        public string DisplayName 
        {
            get { return _displayName; }
            set { SetProperty(ref _displayName, value); }
        }
        private string _warningInfo;
        public string WarningInfo
        {
            get { return _warningInfo; }
            set { SetProperty(ref _warningInfo, value); }
        }
        private string _priority;
        public string Priority
        {
            get { return _priority; }
            set { SetProperty(ref _priority, value); }
        }
    }
}
