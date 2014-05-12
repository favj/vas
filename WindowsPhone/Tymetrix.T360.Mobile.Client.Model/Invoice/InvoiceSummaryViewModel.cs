/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.Generic;
using System.Windows;
using Tymetrix.T360.Mobile.Client.Model.Base;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class InvoiceSummaryViewModel : BaseModel
    {
        private List<SummaryView> detailsList;
        public List<SummaryView> DetailsList
        {
            get { return detailsList; }
            set { SetProperty(ref detailsList, value, "DetailsList"); }
        }
        private List<LineItem> lineItemList;
        public List<LineItem> LineItemList
        {
            get { return lineItemList; }
            set { SetProperty(ref lineItemList, value, "LineItemList"); }
        }
        private int selectedIndex = -1;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { SetProperty(ref selectedIndex, value, "SelectedIndex"); }
        }
        private List<LineItem> originalLineItemList;
        public List<LineItem> OriginalLineItemList
        {
            get { return originalLineItemList; }
            set { SetProperty(ref originalLineItemList, value, "OriginalLineItemList"); }
        }
        private List<LineItem> searchList;
        public List<LineItem> SearchList
        {
            get { return searchList; }
            set { SetProperty(ref searchList, value, "SearchList"); }
        }

        //view specific
        private Thickness listMargin = new Thickness(0,0,0,0);
        public Thickness ListMargin
        {
            get { return listMargin; }
            set { SetProperty(ref listMargin, value, "ListMargin"); }
        }
        private double pivotOpacity = 1.0;
        public double PivotOpacity
        {
            get { return pivotOpacity; }
            set { SetProperty(ref pivotOpacity, value, "PivotOpacity"); }
        }
        private bool pivotEnable = true;
        public bool PivotEnable
        {
            get { return pivotEnable; }
            set { SetProperty(ref pivotEnable, value, "PivotEnable"); }
        }
        private Visibility searchListVisible = Visibility.Collapsed;
        public Visibility SearchListVisible
        {
            get { return searchListVisible; }
            set { SetProperty(ref searchListVisible, value, "SearchListVisible"); }
        }
        private Visibility searchTextVisible = Visibility.Collapsed;
        public Visibility SearchTextVisible
        {
            get { return searchTextVisible; }
            set { SetProperty(ref searchTextVisible, value, "SearchTextVisible"); }
        }
        private string lineItemsTitle = Constants.LineItemPivotHeader;
        public string LineItemsTitle
        {
            get { return lineItemsTitle; }
            set { SetProperty(ref lineItemsTitle, value, "LineItemsTitle"); }
        }
        private Visibility headerVisible = Visibility.Visible;
        public Visibility HeaderVisible
        {
            get { return headerVisible; }
            set { SetProperty(ref headerVisible, value, "HeaderVisible"); }
        }
        private Visibility noDataVisible = Visibility.Collapsed;
        public Visibility NoDataVisible
        {
            get { return noDataVisible; }
            set { SetProperty(ref noDataVisible, value, "NoDataVisible"); }
        }
        private Visibility pivotVisible = Visibility.Visible;
        public Visibility PivotVisible
        {
            get { return pivotVisible; }
            set { SetProperty(ref pivotVisible, value, "PivotVisible"); }
        }
        private Visibility isLineItemVisible = Visibility.Visible;
        public Visibility IsLineItemVisible
        {
            get { return isLineItemVisible; }
            set { SetProperty(ref isLineItemVisible, value, "IsLineItemVisible"); }
        }
        private Visibility lineItemVisible = Visibility.Visible;
        public Visibility LineItemVisible
        {
            get { return lineItemVisible; }
            set { SetProperty(ref lineItemVisible, value, "LineItemVisible"); }
        }
        private Visibility noLineItemVisible = Visibility.Collapsed;
        public Visibility NoLineItemVisible
        {
            get { return noLineItemVisible; }
            set { SetProperty(ref noLineItemVisible, value, "NoLineItemVisible"); }
        }
    }
}
