/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
namespace TyMetrix360.Core.Models
{
    public class UserPreference
    {
        private UserPreference() { }

        public int SelectedInvoiceIndex { get; set; }
        public int SelectedInvoiceId { get; set; }
        public bool IsClientOrTymetrixUser { get; set; }
        public bool HasInvoiceListAccess { get; set; }
        private string _currentUserName;
        public string CurrentUserName
        {
            get { return Vault.AES_Decrypt(_currentUserName); }
            set { _currentUserName = Vault.AES_Encrypt(value); }
        }
        public bool HasSaveUserName { get; set; }
        public bool HasIntegratedLogin { get; set; }
        public string UserName { get; set; }

        private static UserPreference instance;
        public static UserPreference Instance
        {
            get
            {
                if (instance == null) instance = new UserPreference();
                return instance;
            }
        }

        public void Clear()
        {
            instance = null;
        }
    }
}
