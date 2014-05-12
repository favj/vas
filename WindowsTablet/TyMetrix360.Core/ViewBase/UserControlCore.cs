/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace TyMetrix360.Core.ViewBase
{
    public class UserControlCore : UserControl , IUserControlCore
    {
        public UserControlCore()
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

        protected virtual void OnWindowSizeChanged() { }

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!HasPreparedVisualGroups)
            {
                PrepareVisualStateGroups();
                HasPreparedVisualGroups = true;
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

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //LandscapeStoryboard.Stop();
            //PortraitStoryboard.Stop();
            //FilledStoryboard.Stop();
            //SnappedStoryboard.Stop();
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

        private bool HasPreparedVisualGroups { get; set; }
    }
}
