/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Runtime.Serialization;
using Tymetrix.T360.Mobile.Client.Core;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    [DataContract]
    public class InvoiceDetails
    {
        private byte[] invoiceNumber;
        private byte[] companyName;
        private byte[] matterName;
        private byte[] invoiceId;

        [DataMember(Name = "InvoiceId")]
        public string InvoiceId
        {
            get
            {
                if (invoiceId == null) return string.Empty;
                return Vault.Decrypt(invoiceId);
            }
            set
            {
                invoiceId = Vault.Encrypt(value);
            }
        }

        [DataMember(Name = "InvoiceNumber")]
        public string InvoiceNumber
        {
            get
            {
                if (invoiceNumber == null) return string.Empty;
                return Vault.Decrypt(invoiceNumber);
            }
            set
            {
                invoiceNumber = Vault.Encrypt(value);
            }
        }

        [DataMember(Name = "CompanyName")]
        public string CompanyName
        {
            get
            {
                if (companyName == null) return string.Empty;
                return Vault.Decrypt(companyName);
            }
            set
            {
                companyName = Vault.Encrypt(value);
            }
        }

        [DataMember(Name = "MatterName")]
        public string MatterName
        {
            get
            {
                if (matterName == null) return string.Empty;
                return Vault.Decrypt(matterName);
            }
            set
            {
                matterName = Vault.Encrypt(value);
            }
        }


        [DataMember(Name = "InvoiceDate", Order = 3)]
        public string InvoiceDate { get; set; }

        [DataMember(Name = "BilledAmount", Order = 5)]
        public string BilledAmount { get; set; }

        [DataMember(Name = "NetAmount", Order = 6)]
        public string NetAmount { get; set; }
    }
}
