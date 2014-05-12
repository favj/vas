/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;

using TyMetrix360.BusinessObjects.Common;

namespace TyMetrix360.BusinessObjects.Security
{
    public class Credential : BusinessObjectCore
    {
        private string _password;
        public string Password 
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private bool _useLastPassword;
        public bool UseLastPassword
        {
            get { return _useLastPassword; }
            set { SetProperty(ref _useLastPassword, value); }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetProperty(ref _confirmPassword, value); }
        }

        private bool _showKeepCurrentPassword;
        public bool ShowKeepCurrentPassword
        {
            get { return _showKeepCurrentPassword; }
            set { SetProperty(ref _showKeepCurrentPassword, value); }
        }

        private List<string> _rules;
        public List<string> Rules
        {
            get { return _rules; }
            set { SetProperty(ref _rules, value); }
        }
    }
}
