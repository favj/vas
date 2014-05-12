/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Windows.UI.Xaml.Controls;

using TyMetrix360.Core.ViewBase;
using Windows.UI.Xaml;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Animation;
using Windows.ApplicationModel;
using Windows.UI.ViewManagement;
using System;

namespace TyMetrix360.App.View
{
    public sealed partial class InvoiceListSummaryView : UserControlCore, IInvoiceListSummaryView
    {
        public InvoiceListSummaryView()
        {
            this.InitializeComponent();
        }

        private void SummaryStuff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = sender as ListView;
            lv.SelectedIndex = -1;
        }

        protected override VisualState CreateFilledState()
        {
            ObjectAnimationUsingKeyFrames mainViewMarginAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0,0,0),new Thickness(18, 0, 10, 0),MainView,"Margin");
            ObjectAnimationUsingKeyFrames headerTextMarginAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new Thickness(3, 0, 3, 20), HeaderText, "Margin");
            ObjectAnimationUsingKeyFrames nameDetailsGridMarginAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new Thickness(3, 0, 0, 0), NameDetailsGrid, "Margin");
            ObjectAnimationUsingKeyFrames nameDetailsGridCol1Animation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new GridLength(2.5, GridUnitType.Star), NameDetailsGridColumn1, "Width");
            ObjectAnimationUsingKeyFrames nameDetailsGridCol2Animation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new GridLength(2, GridUnitType.Star), NameDetailsGridColumn2, "Width");
            ObjectAnimationUsingKeyFrames numberDetailsGridMarginAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new Thickness(3, 0, 0, -5), NumberDetailsGrid, "Margin");
            ObjectAnimationUsingKeyFrames numberDetailsGridCol1Animation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new GridLength(2.5, GridUnitType.Star), NumberDetailsGridColumn1, "Width");
            ObjectAnimationUsingKeyFrames numberDetailsGridCol2Animation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new GridLength(2, GridUnitType.Star), NumberDetailsGridColumn2, "Width");
            ObjectAnimationUsingKeyFrames flipViewWidthAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), 530, InvoiceSummaryListView, "Width");
            ObjectAnimationUsingKeyFrames standardViewMarginAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new Thickness(0,0,0,0), StandardView, "Margin");

            AddStoryboardChildren(ApplicationViewState.Filled, mainViewMarginAnimation);
            AddStoryboardChildren(ApplicationViewState.Filled, headerTextMarginAnimation);
            AddStoryboardChildren(ApplicationViewState.Filled, nameDetailsGridMarginAnimation);
            AddStoryboardChildren(ApplicationViewState.Filled, nameDetailsGridCol1Animation);
            AddStoryboardChildren(ApplicationViewState.Filled, nameDetailsGridCol2Animation);
            AddStoryboardChildren(ApplicationViewState.Filled, numberDetailsGridMarginAnimation);
            AddStoryboardChildren(ApplicationViewState.Filled, numberDetailsGridCol1Animation);
            AddStoryboardChildren(ApplicationViewState.Filled, numberDetailsGridCol2Animation);
            AddStoryboardChildren(ApplicationViewState.Filled, flipViewWidthAnimation);
            AddStoryboardChildren(ApplicationViewState.Filled, standardViewMarginAnimation);

            return base.CreateFilledState();
        }
    }
}
