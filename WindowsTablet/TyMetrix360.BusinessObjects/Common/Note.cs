/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
namespace TyMetrix360.BusinessObjects.Common
{
    public class Note : BusinessObjectCore
    {
        private string _createdTime;
        public string CreatedTime
        {
            get { return _createdTime; }
            set { SetProperty(ref _createdTime, value); }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
        private string _creator;
        public string Creator
        {
            get { return _creator; }
            set { SetProperty(ref _creator, value); }
        }
    }
}
