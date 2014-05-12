using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TyMetrix360.Core.ViewBase;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TyMetrix360.App.View.Template
{
    public sealed partial class InvoiceListTemplate : UserControlCore
    {
        public InvoiceListTemplate()
        {
            this.InitializeComponent();
        }

        protected override VisualState CreateSnappedState()
        {
            ObjectAnimationUsingKeyFrames rootColumn1Animation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0,0,0), new GridLength(5, GridUnitType.Pixel), RootColumn1, "Width");
            ObjectAnimationUsingKeyFrames rootColumn2Animation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new GridLength(150, GridUnitType.Pixel), RootColumn2, "Width");
            ObjectAnimationUsingKeyFrames rootColumn3Animation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new GridLength(150, GridUnitType.Pixel), RootColumn3, "Width");
            ObjectAnimationUsingKeyFrames templateGridAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new GridLength(320, GridUnitType.Pixel), TemplateGrid, "Width");
            ObjectAnimationUsingKeyFrames invoiceNumberLabelAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new Thickness(0, 0, 3, 0), InvoiceNumberLabel, "Margin");
            ObjectAnimationUsingKeyFrames companyNameWidthAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), 290, CompanyNameLabel, "Width");
            ObjectAnimationUsingKeyFrames companyNameMarginAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new Thickness(0, 0, 5, 0), CompanyNameLabel, "Margin");
            ObjectAnimationUsingKeyFrames invoiceDateTrimAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), TextTrimming.WordEllipsis, InvoiceDateLabel, "TextTrimming");
            ObjectAnimationUsingKeyFrames invoiceDateWrapAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), TextWrapping.NoWrap, InvoiceDateLabel, "TextWrapping");
            ObjectAnimationUsingKeyFrames billedAmountLabelAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new Thickness(0, 0, 15, 0), BilledAmountLabel, "Margin");
            ObjectAnimationUsingKeyFrames netAmountLabelAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new Thickness(0, 0, 15, 0), NetAmountLabel, "Margin");

            AddStoryboardChildren(ApplicationViewState.Snapped, rootColumn1Animation);
            AddStoryboardChildren(ApplicationViewState.Snapped, rootColumn2Animation);
            AddStoryboardChildren(ApplicationViewState.Snapped, rootColumn3Animation);
            AddStoryboardChildren(ApplicationViewState.Snapped, invoiceNumberLabelAnimation);
            AddStoryboardChildren(ApplicationViewState.Snapped, companyNameWidthAnimation);
            AddStoryboardChildren(ApplicationViewState.Snapped, companyNameMarginAnimation);
            AddStoryboardChildren(ApplicationViewState.Snapped, invoiceDateTrimAnimation);
            AddStoryboardChildren(ApplicationViewState.Snapped, invoiceDateWrapAnimation);
            AddStoryboardChildren(ApplicationViewState.Snapped, billedAmountLabelAnimation);

            return base.CreateSnappedState();
        }

        protected override void OnWindowSizeChanged()
        {
            switch (ApplicationView.Value)
            {
                case ApplicationViewState.FullScreenLandscape:
                    Grid.SetColumnSpan(InvoiceNumberLabel, 2);
                    break;
                case ApplicationViewState.FullScreenPortrait:
                    Grid.SetColumnSpan(InvoiceNumberLabel, 2);
                    break;
                case ApplicationViewState.Filled:
                    Grid.SetColumnSpan(InvoiceNumberLabel, 2);
                    break;
                case ApplicationViewState.Snapped:
                    Grid.SetColumnSpan(InvoiceNumberLabel, 3);
                    break;
            }
        }
    }
}
