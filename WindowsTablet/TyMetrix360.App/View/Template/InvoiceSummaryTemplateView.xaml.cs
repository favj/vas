using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TyMetrix360.Core.ViewBase;
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
    public sealed partial class InvoiceSummaryTemplateView : UserControlCore
    {
        public InvoiceSummaryTemplateView()
        {
            this.InitializeComponent();
        }

        protected override VisualState CreateFilledState()
        {
            ObjectAnimationUsingKeyFrames rootColumn2Animation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new GridLength(240, GridUnitType.Pixel), RootGridColumn2, "Width");

            AddStoryboardChildren(ApplicationViewState.Filled, rootColumn2Animation);

            return base.CreateFilledState();
        }
    }
}
