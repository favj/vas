/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using Tymetrix.T360.Mobile.Client.Model.Base;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class LineItem : BaseModel
    {
        private string timeKeeper;
        public string TimeKeeper
        {
            get { return timeKeeper; }
            set { SetProperty(ref timeKeeper, value, "TimeKeeper"); }
        }
        private string narrativeText;
        public string NarrativeText
        {
            get { return narrativeText; }
            set { SetProperty(ref narrativeText, value, "NarrativeText"); }
        }
        private string amount;
        public string Amount
        {
            get { return amount; }
            set { SetProperty(ref amount, value, "Amount"); }
        }
        private string date;
        public string Date
        {
            get { return date; }
            set { SetProperty(ref date, value, "Date"); }
        }
        private string flags;
        public string Flags
        {
            get { return flags; }
            set { SetProperty(ref flags, value, "Flags"); }
        }
        private string lineItemId;
        public string LineItemId
        {
            get { return lineItemId; }
            set { SetProperty(ref lineItemId, value, "LineItemId"); }
        }
        private string netTotal;
        public string NetTotal
        {
            get { return netTotal; }
            set { SetProperty(ref netTotal, value, "NetTotal"); }
        }
        private string netTotalToDisplay;
        public string NetTotalToDisplay
        {
            get { return netTotalToDisplay; }
            set { SetProperty(ref netTotalToDisplay, value, "NetTotalToDisplay"); }
        }
        public string ListItemFlag
        {
            get { return "(" + flags + ")"; }
            set {  }
        }

        //view specific
        private Visibility isVisibility = Visibility.Collapsed;
        public Visibility IsVisibility
        {
            get { return isVisibility; }
            set { SetProperty(ref isVisibility, value, "IsVisibility"); }
        }
        private Visibility flagVisible = Visibility.Visible;
        public Visibility FlagVisible
        {
            get { return flagVisible; }
            set { SetProperty(ref flagVisible, value, "FlagVisible"); }
        }
        private bool isCheckboxChecked;
        public bool IsCheckboxChecked
        {
            get { return isCheckboxChecked; }
            set
            {
                SetProperty(ref isCheckboxChecked, value, "IsCheckboxChecked");
                Messenger.Default.Send<LineItem>(this, Constants.LineItemMultiCheckChange);
            }
        }
        private GridLength lineItemColumn1Width = new GridLength(0, GridUnitType.Star);
        public GridLength LineItemColumn1Width
        {
            get { return lineItemColumn1Width; }
            set { SetProperty(ref lineItemColumn1Width, value, "LineItemColumn1Width"); }
        }
        private GridLength lineItemColumn2Width = new GridLength(1, GridUnitType.Star);
        public GridLength LineItemColumn2Width
        {
            get { return lineItemColumn2Width; }
            set { SetProperty(ref lineItemColumn2Width, value, "LineItemColumn2Width"); }
        }
    }
}
