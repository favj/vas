/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;
using System.Windows;
using Tymetrix.T360.Mobile.Client.Model.Base;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class InvoiceListingModel : BaseModel
    {
        private Thickness listMargin = new Thickness(0,0,0,0);
        public Thickness ListMargin
        {
            get { return listMargin; }
            set { SetProperty(ref listMargin, value, "ListMargin"); }
        }
        private double listOpacity = 1.0;
        public double ListOpacity
        {
            get { return listOpacity; }
            set { SetProperty(ref listOpacity, value, "ListOpacity"); }
        }
        private bool listEnable = true;
        public bool ListEnable
        {
            get { return listEnable; }
            set { SetProperty(ref listEnable, value, "ListEnable"); }
        }
        private string headerTitle;
        public string HeaderTitle
        {
            get { return headerTitle; }
            set { SetProperty(ref headerTitle, value, "HeaderTitle"); }
        }
        private Visibility searchTextVisible = Visibility.Collapsed;
        public Visibility SearchTextVisible
        {
            get { return searchTextVisible; }
            set { SetProperty(ref searchTextVisible, value, "SearchTextVisible"); }
        }
        private Visibility headerVisible = Visibility.Visible;
        public Visibility HeaderVisible
        {
            get { return headerVisible; }
            set { SetProperty(ref headerVisible, value, "HeaderVisible"); }
        }
        private Visibility noDataFound = Visibility.Collapsed;
        public Visibility NoDataFound
        {
            get { return noDataFound; }
            set { SetProperty(ref noDataFound, value, "NoDataFound"); }
        }
        private Visibility dataFound;
        public Visibility DataFound
        {
            get { return dataFound; }
            set { SetProperty(ref dataFound, value, "DataFound"); }
        }
        private int selectedIndex = -1;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { SetProperty(ref selectedIndex, value, "SelectedIndex"); }
        }
        private List<InvoiceModel> invoiceList;
        public List<InvoiceModel> InvoiceList
        {
            get { return invoiceList; }
            set { SetProperty(ref invoiceList, value, "InvoiceList"); }
        }
        private List<InvoiceModel> originalInvoiceList;
        public List<InvoiceModel> OriginalInvoiceList
        {
            get { return originalInvoiceList; }
            set { SetProperty(ref originalInvoiceList, value, "OriginalInvoiceList"); }
        }
    }
}
