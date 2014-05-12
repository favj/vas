/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Runtime.Serialization;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;

namespace Tymetrix.T360.Mobile.Client.Model.Base
{
    [DataContract]
    public class UserData
    {
        private static volatile UserData user;
        private static object syncRoot = new Object();

        private byte[] userName;
        private byte[] password;
        private byte[] integratedLoginId;
        private bool isAuthenticated;
        private byte[] networkName;
        private byte[] memberName;
        private bool bLineItemsBackKey;
        private bool bNavFromInvoiceSearch;
        private bool isLineItemsReject;
        private bool isMultipleLineItemReject;
        private bool isMultipleLineItemAdjust;
        
        #region Constructor
        
        public UserData()
        {
        }

        #endregion
        
        #region Properties
              
        [DataMember(Name = "LoginId")]
        public string UserName
        {
            get
            {
                if (userName == null) return string.Empty;
                return Vault.Decrypt(userName);
            }
            set
            {
                userName = Vault.Encrypt(value);
            }
        }

        [DataMember(Name = "NetworkName")]
        public string NetworkName
        {
            get
            {
                if (networkName == null) return string.Empty;
                return Vault.Decrypt(networkName);
            }
            set
            {
                networkName = Vault.Encrypt(value);
            }
        }

        /// <summary>
        /// UserName.
        /// </summary>
        [DataMember(Name = "MemberName")]
        public string MemberName
        {
            get
            {
                if (memberName == null) return string.Empty;
                return Vault.Decrypt(memberName);
            }
            set
            {
                memberName = Vault.Encrypt(value);
            }
        }

        [DataMember(Name = "InvoiceCount")]
        public string AwaitingInvoiceCount
        {
            get;
            set;
        }

        /// <summary>
        /// Password
        /// </summary>
        [DataMember(Name = "Password")]
        public string Password
        {
            get
            {
                if (password == null) return string.Empty;
                return Vault.Decrypt(password);
            }
            set
            {
                password = Vault.Encrypt(value);
            }
        }

        [DataMember(Name = "IntegratedLoginId")]
        public string IntegratedLoginId
        {
            get
            {
                if (integratedLoginId == null) return string.Empty;
                return Vault.Decrypt(integratedLoginId);
            }
            set
            {
                integratedLoginId = Vault.Encrypt(value);
            }
        }

        /// <summary>
        /// IsIntegratedLoginEnabled
        /// </summary>
        [DataMember(Name = "IsSSOEnabled")]
        public bool IsSSOEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// IsAuthenticated
        /// </summary>
        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
            set { isAuthenticated = value; }
        }

        [DataMember(Name = "InstallationId")]
        public string InstallationId
        {
            get;
            set;
        }

        [DataMember(Name = "HasInvoiceListAccess")]
        public bool HasInvoiceListAccess { get; set; }

        [DataMember(Name = "HasMatterListAccess")]
        public bool HasMatterListAccess { get; set; }

        [DataMember(Name = "IsClientOrTymetrixUser")]
        public bool IsClientOrTymetrixUser { get; set; }

        [DataMember(Name = "InvoiceQuickActionsAccess")]
        public bool InvoiceQuickActionsAccess { get; set; }

        public bool HasDisclaimer { get; set; }

        public string DisclaimerTitle { get; set; }

        public bool HasSaveUserName { get; set; }

        public string DisclaimerData { get; set; }

        public bool HasIntegratedLogin { get; set; }

        public bool DetailsToNotes { get; set; }

        public bool DetailsToViewNote { get; set; }

        public bool LineItemBackPress
        {
            get
            {
                return bLineItemsBackKey;
            }
            set
            {
                bLineItemsBackKey = value;
            }
        }

        public bool NavFromInvSearch
        {
            get
            {
                return bNavFromInvoiceSearch;
            }
            set
            {
                bNavFromInvoiceSearch = value;
            }
        }

        public bool IsMultipleInvoiceReject { get; set; }

        public bool IsLineItemsReject
        {
            get
            {
                return isLineItemsReject;
            }
            set
            {
                isLineItemsReject = value;
            }
        }

        public bool IsMultipleLineItemReject
        {
            get
            {
                return isMultipleLineItemReject;
            }
            set
            {
                isMultipleLineItemReject = value;
            }
        }

        public bool IsMultipleLineItemAdjust
        {
            get
            {
                return isMultipleLineItemAdjust;
            }
            set
            {
                isMultipleLineItemAdjust = value;
            }
        }

        public static UserData Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (user == null)
                    {
                        user = new UserData();
                    }
                }
                return user;
            }
        }

        #endregion

        #region Methods
        
        #endregion

        public void Clear()
        {
            user = null;
        }
    }
}
