/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Windows;
using System.Windows.Input;
using Tymetrix.T360.Mobile.Client.Model.Base;

namespace Tymetrix.T360.Mobile.Client.Model.Invoice
{
    public class SummaryView : BaseModel
    {
        private Visibility isNoneView = Visibility.Collapsed;
        public Visibility IsNoneView
        {
            get { return isNoneView; }
            set { SetProperty(ref isNoneView, value, "IsNoneView"); }
        }
        private Visibility isHeaderView = Visibility.Collapsed;
        public Visibility IsHeaderView
        {
            get { return isHeaderView; }
            set { SetProperty(ref isHeaderView, value, "IsHeaderView"); }
        }
        private Visibility isHeaderValueView = Visibility.Collapsed;
        public Visibility IsHeaderValueView
        {
            get { return isHeaderValueView; }
            set { SetProperty(ref isHeaderValueView, value, "IsHeaderValueView"); }
        }
        private Visibility isDetailView = Visibility.Collapsed;
        public Visibility IsDetailView
        {
            get { return isDetailView; }
            set { SetProperty(ref isDetailView, value, "IsDetailView"); }
        }
        private Visibility isFlagView = Visibility.Collapsed;
        public Visibility IsFlagView
        {
            get { return isFlagView; }
            set { SetProperty(ref isFlagView, value, "IsFlagView"); }
        }
        private Visibility isReviewRouteView = Visibility.Collapsed;
        public Visibility IsReviewRouteView
        {
            get { return isReviewRouteView; }
            set { SetProperty(ref isReviewRouteView, value, "IsReviewRouteView"); }
        }
        private Visibility isNotesView = Visibility.Collapsed;
        public Visibility IsNotesView
        {
            get { return isNotesView; }
            set { SetProperty(ref isNotesView, value, "IsNotesView"); }
        }
        private Visibility isAdjustmentsView = Visibility.Collapsed;
        public Visibility IsAdjustmentsView
        {
            get { return isAdjustmentsView; }
            set { SetProperty(ref isAdjustmentsView, value, "IsAdjustmentsView"); }
        }
        private Visibility isNotesRow = Visibility.Collapsed;
        public Visibility IsNotesRow
        {
            get { return isNotesRow; }
            set { SetProperty(ref isNotesRow, value, "IsNotesRow"); }
        }
        private Visibility isButtonRow = Visibility.Collapsed;
        public Visibility IsButtonRow
        {
            get { return isButtonRow; }
            set { SetProperty(ref isButtonRow, value, "IsButtonRow"); }
        }
        private string headerText = string.Empty;
        public string HeaderText
        {
            get { return headerText; }
            set { SetProperty(ref headerText, value, "HeaderText"); }
        }
        private string valueText = string.Empty;
        public string ValueText
        {
            get { return valueText; }
            set { SetProperty(ref valueText, value, "ValueText"); }
        }
        private string detailHeader = string.Empty;
        public string DetailHeader
        {
            get { return detailHeader; }
            set { SetProperty(ref detailHeader, value, "DetailHeader"); }
        }
        private string detailValue = string.Empty;
        public string DetailValue
        {
            get { return detailValue; }
            set { SetProperty(ref detailValue, value, "DetailValue"); }
        }
        private string flagName = string.Empty;
        public string FlagName
        {
            get { return flagName; }
            set { SetProperty(ref flagName, value, "FlagName"); }
        }
        private string currentReviewerLabel = string.Empty;
        public string CurrentReviewerLabel
        {
            get { return currentReviewerLabel; }
            set { SetProperty(ref currentReviewerLabel, value, "CurrentReviewerLabel"); }
        }
        private Uri imagePath;
        public Uri ImagePath
        {
            get { return imagePath; }
            set { SetProperty(ref imagePath, value, "ImagePath"); }
        }
        private Uri imageTitle;
        public Uri ImageTitle
        {
            get { return imageTitle; }
            set { SetProperty(ref imageTitle, value, "ImageTitle"); }
        }
        private string reviewerName = string.Empty;
        public string ReviewerName
        {
            get { return reviewerName; }
            set { SetProperty(ref reviewerName, value, "ReviewerName"); }
        }
        private double reviewRouteImageWidth;
        public double ReviewRouteImageWidth
        {
            get { return reviewRouteImageWidth; }
            set { SetProperty(ref reviewRouteImageWidth, value, "ReviewRouteImageWidth"); }
        }
        private Visibility currentReviewerVisible = Visibility.Collapsed;
        public Visibility CurrentReviewerVisible
        {
            get { return currentReviewerVisible; }
            set { SetProperty(ref currentReviewerVisible, value, "CurrentReviewerVisible"); }
        }
        private Thickness reviewRouteImageMargin = new Thickness(10,0,0,0);
        public Thickness ReviewRouteImageMargin
        {
            get { return reviewRouteImageMargin; }
            set { SetProperty(ref reviewRouteImageMargin, value, "ReviewRouteImageMargin"); }
        }
        private string creator = string.Empty;
        public string Creator
        {
            get { return creator; }
            set { SetProperty(ref creator, value, "Creator"); }
        }
        private string createdTime  = string.Empty;
        public string CreatedTime
        {
            get { return createdTime; }
            set { SetProperty(ref createdTime, value, "CreatedTime"); }
        }
        private string description = string.Empty;
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value, "Description"); }
        }
        private string owner = string.Empty;
        public string Owner
        {
            get { return owner; }
            set { SetProperty(ref owner, value, "Owner"); }
        }
        private string amount = string.Empty;
        public string Amount
        {
            get { return amount; }
            set { SetProperty(ref amount, value, "Amount"); }
        }
        private ICommand showAllNotesCommand;
        public ICommand ShowAllNotesCommand
        {
            get { return showAllNotesCommand; }
            set { SetProperty(ref showAllNotesCommand, value, "ShowAllNotesCommand"); }
        }
        private object tag;
        public object Tag
        {
            get { return tag; }
            set { SetProperty(ref tag, value, "Tag"); }
        }
    }
}
