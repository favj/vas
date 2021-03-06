﻿/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Windows.Controls;
using Tymetrix.T360.Mobile.Client.Model.Invoice;

namespace Tymetrix.T360.Mobile.Client.Common.Base.View
{
    public partial class InvoiceHeader : UserControl
    {
        public InvoiceHeader()
        {
            InitializeComponent();
        }

        private InvoiceBasicInfo headerInfo;
        public InvoiceBasicInfo HeaderInfo
        {
            get
            {
                return headerInfo;
            }
            set
            {
                headerInfo = value;
                invoiceLabel.Text = "Inv # " + headerInfo.InvoiceNumber;
                matterLabel.Text = headerInfo.MatterName;
                companyLabel.Text = headerInfo.CompanyName;
                matterNumberLabel.Text = "Matter #" + headerInfo.MatterNumber;
            }
        }

    }
}
