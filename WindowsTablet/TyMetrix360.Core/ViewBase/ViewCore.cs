/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

using TyMetrix360.Core.Converters;
using TyMetrix360.Core.ViewModelBase;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System;
using Windows.UI.Xaml.Media.Animation;

namespace TyMetrix360.Core.ViewBase
{
    public class ViewCore : Page, IViewCore
    {
        private const string IsBusy = "IsBusy";

        public ViewCore()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                SizeChanged += OnWindowSizeChanged;
                Loaded += OnLoaded;
            }
        }

        private void PrepareVisualStateGroups()
        {
            IList<VisualStateGroup> visualStateGroups = VisualStateManager.GetVisualStateGroups(this);

            VisualStateGroup visualStateGroup = new VisualStateGroup();
            visualStateGroups.Add(visualStateGroup);

            VisualState fullScreenPortraitState = CreatePortraitState();
            visualStateGroup.States.Add(fullScreenPortraitState);

            VisualState fullScreenLandscapeState = CreateLandscapeState();
            visualStateGroup.States.Add(fullScreenLandscapeState);

            VisualState filledState = CreateFilledState();
            visualStateGroup.States.Add(filledState);

            VisualState snappedState = CreateSnappedState();
            visualStateGroup.States.Add(snappedState);
        }

        protected virtual VisualState CreatePortraitState()
        {
            VisualState portraitState = new VisualState();
            portraitState.Storyboard = PortraitStoryboard;
            return portraitState;
        }

        protected virtual VisualState CreateLandscapeState()
        {
            VisualState landscapeState = new VisualState();
            landscapeState.Storyboard = LandscapeStoryboard;
            return landscapeState;
        }

        protected virtual VisualState CreateFilledState()
        {
            VisualState filledState = new VisualState();
            filledState.Storyboard = FilledStoryboard;
            return filledState;
        }

        protected virtual VisualState CreateSnappedState()
        {
            VisualState snappedState = new VisualState();
            snappedState.Storyboard = SnappedStoryboard;
            return snappedState;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as IViewModelCore;
            if (model != null)
            {
                Grid root = null;
                if (Content is Grid)
                {
                    root = (Grid)Content;
                }
                else if (Content is Border)
                {
                    var child = (Content as Border).Child as Grid;
                    if (child != null)
                    {
                        root = child;
                    }
                }
                if (root != null)
                {
                    var curtainGrid = new Grid();
                    curtainGrid.SetValue(Canvas.ZIndexProperty, 9999);
                    curtainGrid.Opacity = 0.6;

                    var brush = new LinearGradientBrush { EndPoint = new Point(0.5, 1), StartPoint = new Point(0.5, 0) };
                    var stops = new GradientStopCollection();

                    var stop = new GradientStop { Color = new Color { R = 0x80, G = 0x74, B = 0xD4 } };
                    stops.Add(stop);

                    stop = new GradientStop { Color = new Color { R = 0x80, G = 0x74, B = 0xD4 }, Offset = 1 };
                    stops.Add(stop);

                    stop = new GradientStop { Color = new Color { R = 0xB7, G = 0x84, B = 0xD0 }, Offset = 0.5 };
                    stops.Add(stop);
                    brush.GradientStops = stops;

                    curtainGrid.Background = brush;

                    var progressRign = new ProgressRing { MinHeight = 100, MinWidth = 100, TabNavigation = KeyboardNavigationMode.Cycle };
                    var binding = new Binding { Path = new PropertyPath(IsBusy) };
                    progressRign.HorizontalAlignment = HorizontalAlignment.Center;
                    progressRign.VerticalAlignment = VerticalAlignment.Center;
                    progressRign.SetBinding(ProgressRing.IsActiveProperty, binding);
                    curtainGrid.Children.Add(progressRign);

                    binding = new Binding { Path = new PropertyPath(IsBusy), Converter = new BooleanToVisibilityConverter() };
                    curtainGrid.SetBinding(VisibilityProperty, binding);

                    root.Children.Add(curtainGrid);
                    if(root.ColumnDefinitions != null && root.ColumnDefinitions.Count>1)
                    {
                        curtainGrid.SetValue(Grid.ColumnSpanProperty, root.ColumnDefinitions.Count);
                    }
                    if (root.RowDefinitions != null && root.RowDefinitions.Count > 1)
                    {
                        curtainGrid.SetValue(Grid.RowSpanProperty, root.RowDefinitions.Count);
                    }
                    if (model.IsBusy)
                    {
                        Dispatcher.RunAsync(CoreDispatcherPriority.Low, () => progressRign.Focus(FocusState.Programmatic));
                    }

                    model.PropertyChanged += (o1, e1) =>
                                                 {
                                                     if (e1.PropertyName == IsBusy)
                                                     {
                                                         if (model.IsBusy)
                                                         {
                                                             this.IsHitTestVisible = false;
                                                             progressRign.Focus(FocusState.Programmatic);
                                                         }
                                                         else
                                                         {
                                                             Focus(FocusState.Programmatic);
                                                             this.IsHitTestVisible = true;
                                                         }
                                                         Messenger.Default.Send<IViewModelCore>((IViewModelCore)DataContext, "RefreshAppBar");
                                                     }
                                                 };

                    Loaded -= OnLoaded;
                }
            }
        }

        protected virtual void OnWindowSizeChanged() { }

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!VisualStatePrepared)
            {
                PrepareVisualStateGroups();
                VisualStatePrepared = true;
            }

            OnWindowSizeChanged();
            switch (ApplicationView.Value)
            {
                case ApplicationViewState.FullScreenLandscape:
                    PortraitStoryboard.Stop();
                    FilledStoryboard.Stop();
                    SnappedStoryboard.Stop();
                    LandscapeStoryboard.Begin();
                    break;
                case ApplicationViewState.FullScreenPortrait:
                    LandscapeStoryboard.Stop();
                    FilledStoryboard.Stop();
                    SnappedStoryboard.Stop();
                    PortraitStoryboard.Begin();
                    break;
                case ApplicationViewState.Filled:
                    LandscapeStoryboard.Stop();
                    PortraitStoryboard.Stop();
                    SnappedStoryboard.Stop();
                    FilledStoryboard.Begin();
                    break;
                case ApplicationViewState.Snapped:
                    LandscapeStoryboard.Stop();
                    PortraitStoryboard.Stop();
                    FilledStoryboard.Stop();
                    SnappedStoryboard.Begin();
                    break;
            }
        }

        protected TimeSpan CreateTimeSpan(int hours, int mins, int secs)
        {
            return new TimeSpan(hours, mins, secs);
        }

        protected ObjectAnimationUsingKeyFrames CreateObjectKeyFrameAnimation(TimeSpan timeSpan, object value, DependencyObject target, string path)
        {
            ObjectAnimationUsingKeyFrames animation = new ObjectAnimationUsingKeyFrames();
            DiscreteObjectKeyFrame frame = new DiscreteObjectKeyFrame();
            frame.KeyTime = timeSpan;
            frame.Value = value;
            animation.KeyFrames.Add(frame);
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, path);
            return animation;
        }

        protected DoubleAnimation CreateDoubleAnimation(TimeSpan timeSpan, double? to, DependencyObject target, string path)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = to;
            animation.Duration = timeSpan;
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, path);
            return animation;
        }

        protected void AddStoryboardChildren(ApplicationViewState viewState, Timeline timeLine)
        {
            switch (viewState)
            {
                case ApplicationViewState.FullScreenLandscape:
                    LandscapeStoryboard.Children.Add(timeLine);
                    break;
                case ApplicationViewState.FullScreenPortrait:
                    PortraitStoryboard.Children.Add(timeLine);
                    break;
                case ApplicationViewState.Filled:
                    FilledStoryboard.Children.Add(timeLine);
                    break;
                case ApplicationViewState.Snapped:
                    SnappedStoryboard.Children.Add(timeLine);
                    break;
            }
        }

        private Storyboard landscapeStoryboard;
        protected Storyboard LandscapeStoryboard
        {
            get
            {
                if (landscapeStoryboard == null) landscapeStoryboard = new Storyboard();
                return landscapeStoryboard;
            }
        }

        private Storyboard portraitStoryboard;
        protected Storyboard PortraitStoryboard
        {
            get
            {
                if (portraitStoryboard == null) portraitStoryboard = new Storyboard();
                return portraitStoryboard;
            }
        }

        private Storyboard filledStoryboard;
        protected Storyboard FilledStoryboard
        {
            get
            {
                if (filledStoryboard == null) filledStoryboard = new Storyboard();
                return filledStoryboard;
            }
        }

        private Storyboard snappedStoryboard;
        protected Storyboard SnappedStoryboard
        {
            get
            {
                if (snappedStoryboard == null) snappedStoryboard = new Storyboard();
                return snappedStoryboard;
            }
        }

        private bool VisualStatePrepared { get; set; }
    }
}
