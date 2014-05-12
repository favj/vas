/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

using TyMetrix360.App.View;
using TyMetrix360.App.ViewModel;
using TyMetrix360.Core.Container;
using TyMetrix360.Core.ViewModelBase;
using System;

namespace TyMetrix360.App.Navigation
{
    public static class Navigator
    {
        public static bool isKeyBoardOpen { get; set; }
        static DependencyObject _shell;
        public static void Init(DependencyObject shellView)
        {
            _shell = shellView;
        }
        
        public static IShellView ShellView
        {
            get
            {
                return _shell as IShellView;
            }
        }

        public static void Navigate(
            Destination destination,
            EventHandler callBack,
            ExistingViewBehavior existingViewBehavior = ExistingViewBehavior.Remove,
            params object[] parameters)
        {
            Navigate(NavigationFactory.GetNavigationItem(destination), existingViewBehavior, callBack, parameters);
        }

        public static void Navigate(
            Destination destination,
            ExistingViewBehavior existingViewBehavior = ExistingViewBehavior.Remove,
            params object[] parameters)
        {
            Navigate(NavigationFactory.GetNavigationItem(destination), existingViewBehavior, null, parameters);
        }

        public static void ClosePopup()
        {
            ShellView.ChildPopup.Visibility = Visibility.Collapsed;
            ShellView.ChildPopup.IsOpen = false;
            ShellView.UseSunGlasses(false);
        }
        public static void  KeyboardOpen()
        {
            ShellView.ChildPopup.VerticalOffset = 0;
        }
        public static void KeyboardClosed()
        {
            var resolution = Window.Current.Bounds;
            var height = resolution.Height;
            ShellView.ChildPopup.VerticalOffset = height - 464;
        }
        public static void NavigateToPopUp(
            Destination destination,
            ExistingViewBehavior existingViewBehavior = ExistingViewBehavior.Remove,
            params object[] parameters)
        {
            var navigationItem = NavigationFactory.GetNavigationItem(destination);
            navigationItem.Parameters = parameters;
            var viewModel = Container.ResolveViewModel(navigationItem.ViewModelType);
            viewModel.Initialize(parameters);
            var view = Container.ResolveView(navigationItem.ViewType);
            view.DataContext = viewModel;
            ShellView.ChildPopup.IsOpen = false;
            var resolution = Window.Current.Bounds;
            var height = resolution.Height;
            var width = resolution.Width;
            ShellView.ChildPopup.Child = (UIElement)view;
            ShellView.ChildPopup.Visibility = Visibility.Visible;
            ShellView.ChildPopup.VerticalOffset = height - 464;
            ShellView.ChildPopup.MaxHeight = (height / 2);
            ShellView.ChildPopup.MaxWidth = width;
            ShellView.ChildPopup.HorizontalOffset = 0;
            ShellView.ChildPopup.IsOpen = true;
            ShellView.UseSunGlasses(true);
        }

        private static void Navigate(
            INavigationItem navigationItem,
            ExistingViewBehavior existingViewBehavior,
            EventHandler callBack,
            params object[] parameters
            )
        {
            if (parameters.Length > 0)
            { 
                
            }
            var viewModel = Container.ResolveViewModel(navigationItem.ViewModelType);
            var view = Container.ResolveView(navigationItem.ViewType);
            var grid = FindGrid(_shell, navigationItem.RegionName);
            SetAppBar(viewModel);
            if (existingViewBehavior == ExistingViewBehavior.Remove)
            {
                grid.Children.Clear();
            }
            else
            {
                foreach (var child in grid.Children)
                {
                    child.Visibility = Visibility.Collapsed;
                }
            }
            if (callBack != null)
            {
                callBack(null, EventArgs.Empty);
            }
            grid.Children.Add((UIElement)view);
            viewModel.Initialize(parameters);
            view.DataContext = viewModel;
        }

        private static void SetAppBar(IViewModelCore viewModelCore)
        {
            if (ShellView != null)
            {
                var shellViewModel = ShellView.DataContext as IShellViewModel;
                if (shellViewModel != null)
                {
                    shellViewModel.RefreshAppBar(viewModelCore);
                }
            }
        }

        private static Grid FindGrid(DependencyObject start, string gridName)
        {
            var count = VisualTreeHelper.GetChildrenCount(start);
            for (var childCounter = 0; childCounter < count; childCounter++)
            {
                var element = VisualTreeHelper.GetChild(start, childCounter);
                var grid = element as Grid;
                if (grid == null)
                {
                    return FindGrid(element, gridName);
                }
                if (grid.Name.ToLower() == gridName.ToLower())
                {
                    return grid;
                }
                return FindGrid(element, gridName);
            }
            return null;
        }
    }
}
