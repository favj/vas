/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.Core;

namespace TyMetrix360.BusinessObjects.Security
{
    public class LoginInfo : BusinessObjectCore
    {
        private string _loginId; 
        public string LoginId 
        {
            get { return Vault.AES_Decrypt(_loginId); }
            set { SetProperty(ref _loginId, Vault.AES_Encrypt(value)); } 
        }
        private string _password;
        public string Password
        {
            get { return Vault.AES_Decrypt(_password); }
            set { SetProperty(ref _password, Vault.AES_Encrypt(value)); }
        }
        private string _integratedLoginId;
        public string IntegratedLoginId
        {
            get { return Vault.AES_Decrypt(_integratedLoginId); }
            set { SetProperty(ref _integratedLoginId, Vault.AES_Encrypt(value)); }
        }
        private bool _isAuthenticated;
        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
            set { SetProperty(ref _isAuthenticated, value); }
        }
        private string _networkName;
        public string NetworkName
        {
            get { return Vault.AES_Decrypt(_networkName); }
            set { SetProperty(ref _networkName, Vault.AES_Encrypt(value)); }
        }
        private string _memberName;
        public string MemberName
        {
            get { return Vault.AES_Decrypt(_memberName); }
            set { SetProperty(ref _memberName, Vault.AES_Encrypt(value)); }
        }
        private int _invoiceCount;
        public int InvoiceCount
        {
            get { return _invoiceCount; }
            set { SetProperty(ref _invoiceCount, value); }
        }
        private string _installationId;
        public string InstallationId
        {
            get { return _installationId; }
            set { SetProperty(ref _installationId, value); }
        }
        private bool _hasInvoiceListAccess;
        public bool HasInvoiceListAccess
        {
            get { return _hasInvoiceListAccess; }
            set { SetProperty(ref _hasInvoiceListAccess, value); }
        }
        private bool _hasMatterListAccess;
        public bool HasMatterListAccess
        {
            get { return _hasMatterListAccess; }
            set { SetProperty(ref _hasMatterListAccess, value); }
        }
        private bool _isClientOrTymetrixUser;
        public bool IsClientOrTymetrixUser
        {
            get { return _isClientOrTymetrixUser; }
            set { SetProperty(ref _isClientOrTymetrixUser, value); }
        }
        private bool _invoiceQuickActionsAccess;
        public bool InvoiceQuickActionsAccess
        {
            get { return _invoiceQuickActionsAccess; }
            set { SetProperty(ref _invoiceQuickActionsAccess, value); }
        }
        private bool _hasDisclaimer;
        public bool HasDisclaimer
        {
            get { return _hasDisclaimer; }
            set { SetProperty(ref _hasDisclaimer, value); }
        }
        private string _disclaimerTitle;
        public string DisclaimerTitle
        {
            get { return _disclaimerTitle; }
            set { SetProperty(ref _disclaimerTitle, value); }
        }
        private bool _hasSaveUserName;
        public bool HasSaveUserName
        {
            get { return _hasSaveUserName; }
            set { SetProperty(ref _hasSaveUserName, value); }
        }
        private string _disclaimerData;
        public string DisclaimerData
        {
            get { return _disclaimerData; }
            set { SetProperty(ref _disclaimerData, value); }
        }
        private bool _hasIntegratedLogin;
        public bool HasIntegratedLogin
        {
            get { return _hasIntegratedLogin; }
            set { SetProperty(ref _hasIntegratedLogin, value); }
        }
    }
}
