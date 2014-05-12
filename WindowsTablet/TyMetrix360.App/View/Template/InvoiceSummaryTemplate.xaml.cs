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
    public sealed partial class InvoiceSummaryTemplate : UserControlCore
    {
        public InvoiceSummaryTemplate()
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
            ObjectAnimationUsingKeyFrames rootGridMarginAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new Thickness(0, 0, 0, 0), RootGrid, "Margin");
            ObjectAnimationUsingKeyFrames summaryStuffMarginAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new Thickness(0, 10, 0, 0), SummaryStuff, "Margin");

            AddStoryboardChildren(ApplicationViewState.Filled, rootGridMarginAnimation);
            AddStoryboardChildren(ApplicationViewState.Filled, summaryStuffMarginAnimation);

            return base.CreateFilledState();
        }

        protected override void OnWindowSizeChanged()
        {
        }
    }
}
