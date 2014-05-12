/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using GalaSoft.MvvmLight.Messaging;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using TyMetrix360.App.CommandParameters;
using TyMetrix360.App.Common;
using TyMetrix360.App.ViewModel;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.Core.Models;
using TyMetrix360.Core.ViewBase;
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Controls.Primitives;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.BusinessObjects.Invoice;

namespace TyMetrix360.App.View
{
    public sealed partial class InvoiceListView : ViewCore ,IInvoiceListView
    {
        private SelectionChangedEventHandler changeHandler;
        private Popup confirmationPopup;
        public InvoiceListView()
        {
            this.InitializeComponent();
            var resolution = Window.Current.Bounds;
            SummaryView.Height = resolution.Height - 100;
            changeHandler = new SelectionChangedEventHandler(InvoiceList_SelectionChanged);
            Messenger.Default.Register<string>(this, Constants.RegisterInvoiceListEvents, Register);
            Register(string.Empty);
        }

        protected override VisualState CreateFilledState()
        {
            ObjectAnimationUsingKeyFrames gridMarginAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0,0,0),new Thickness(10, 0, 0, 0),DetailsGrid,"Margin");
            ObjectAnimationUsingKeyFrames summaryViewMarginAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new Thickness(0, 2, 0, 0), SummaryView, "Margin");
            ObjectAnimationUsingKeyFrames summaryViewWidthAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), 560, SummaryView, "Width");
            ObjectAnimationUsingKeyFrames listColumnAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new GridLength(450, GridUnitType.Pixel), ListColumn, "Width");

            AddStoryboardChildren(ApplicationViewState.Filled, gridMarginAnimation);
            AddStoryboardChildren(ApplicationViewState.Filled, summaryViewMarginAnimation);
            AddStoryboardChildren(ApplicationViewState.Filled, summaryViewWidthAnimation);
            AddStoryboardChildren(ApplicationViewState.Filled, listColumnAnimation);

            return base.CreateFilledState();
        }

        protected override VisualState CreateSnappedState()
        {
            ObjectAnimationUsingKeyFrames summaryAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), Visibility.Collapsed, SummaryRegion, "Visibility");
            DoubleAnimation rootGridAnimation = CreateDoubleAnimation(CreateTimeSpan(0, 0, 0), 320, StandardView, "Width");
            ObjectAnimationUsingKeyFrames gridMarginAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new Thickness(0, 0, 0, 0), DetailsGrid, "Margin");
            ObjectAnimationUsingKeyFrames listColumnAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new GridLength(320, GridUnitType.Pixel), ListColumn, "Width");
            ObjectAnimationUsingKeyFrames invoiceCountAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), Orientation.Vertical, InvoiceCountPanel, "Orientation");
            ObjectAnimationUsingKeyFrames invoiceCountRowAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new GridLength(100, GridUnitType.Pixel), InvoiceCountRow, "Height");
            ObjectAnimationUsingKeyFrames headerImageWidthAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), 200, headerImage, "Width");
            ObjectAnimationUsingKeyFrames headerImageHeightAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), 40, headerImage, "Height");
            ObjectAnimationUsingKeyFrames backButtonAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), Application.Current.Resources["SnappedBackButtonStyle"] as Style, backButton, "Style");
            ObjectAnimationUsingKeyFrames headerImageMarginAnimation = CreateObjectKeyFrameAnimation(CreateTimeSpan(0, 0, 0), new Thickness(70, 12, 0, 15), headerImage, "Margin");

            AddStoryboardChildren(ApplicationViewState.Snapped, summaryAnimation);
            AddStoryboardChildren(ApplicationViewState.Snapped, rootGridAnimation);
            AddStoryboardChildren(ApplicationViewState.Snapped, gridMarginAnimation);
            AddStoryboardChildren(ApplicationViewState.Snapped, listColumnAnimation);
            AddStoryboardChildren(ApplicationViewState.Snapped, invoiceCountAnimation);
            AddStoryboardChildren(ApplicationViewState.Snapped, invoiceCountRowAnimation);
            AddStoryboardChildren(ApplicationViewState.Snapped, headerImageWidthAnimation);
            AddStoryboardChildren(ApplicationViewState.Snapped, headerImageHeightAnimation);
            AddStoryboardChildren(ApplicationViewState.Snapped, backButtonAnimation);
            AddStoryboardChildren(ApplicationViewState.Snapped, headerImageMarginAnimation);

            return base.CreateSnappedState();
        }

        public void Register(string emptyString)
        {
            InvoiceList.SelectionChanged += changeHandler;
            Messenger.Default.Register<string>(this, Constants.UnregisterInvoiceListEvents, Unregister);
            Messenger.Default.Register<string>(this, Constants.MultiSelect_SelectAll, SelectAll);
            Messenger.Default.Register<string>(this, Constants.MultiSelect_ClearAll, ClearAll);
            Messenger.Default.Register<string>(this, Constants.ShowConfirmationPopup, ShowConfirmationPopup);
            Messenger.Default.Register<string>(this, Constants.CloseConfirmationPopup, CloseConfirmationPopup);
            Messenger.Default.Register<InvoiceErrorDetails>(this, Constants.InvoiceErrorDetails, ShowFailurePage);
            Messenger.Default.Register<string>(this, Constants.ShowPopupAfterConfirmation, OpenPopup);
        }

        public void Unregister(string param)
        {
            InvoiceListViewModel invoiceListVM = (InvoiceListViewModel)DataContext;
            InvoiceList.SelectionChanged -= changeHandler;
            Messenger.Default.Unregister<string>(this, Constants.UnregisterInvoiceListEvents, Unregister);
            Messenger.Default.Unregister<string>(this, Constants.MultiSelect_SelectAll, SelectAll);
            Messenger.Default.Unregister<string>(this, Constants.MultiSelect_ClearAll, ClearAll);
            Messenger.Default.Unregister<string>(this, Constants.ShowConfirmationPopup, ShowConfirmationPopup);
            Messenger.Default.Unregister<string>(this, Constants.CloseConfirmationPopup, CloseConfirmationPopup);
            Messenger.Default.Unregister<InvoiceErrorDetails>(this, Constants.InvoiceErrorDetails, ShowFailurePage);
            Messenger.Default.Unregister<string>(this, Constants.ShowPopupAfterConfirmation, OpenPopup);
        }

        private void ShowFailurePage(InvoiceErrorDetails errorDetails)
        {
            confirmationPopup.Child = CreateFailureView(errorDetails);
        }

        private void OpenPopup(string emptyString)
        {
            confirmationPopup.IsOpen = true;
        }

        private void CloseConfirmationPopup(string emptyString)
        {
            confirmationPopup.IsOpen = false;
        }

        private async void ShowConfirmationPopup(string emptyString)
        {
            try
            {
                confirmationPopup = new Popup();

                confirmationPopup.IsLightDismissEnabled = true;
                confirmationPopup.Closed += confirmationPopup_Closed;

                confirmationPopup.Child = await CreateConfirmationView();

                confirmationPopup.Height = 0.6 * Window.Current.CoreWindow.Bounds.Bottom;
                confirmationPopup.Width = 0.5 * Window.Current.CoreWindow.Bounds.Right;
                confirmationPopup.HorizontalOffset = 0.25 * Window.Current.CoreWindow.Bounds.Right;
                confirmationPopup.VerticalOffset = 0.2 * Window.Current.CoreWindow.Bounds.Bottom;

                confirmationPopup.IsOpen = true;
            }
            catch (T360Exception te)
            {
                InvoiceListViewModel invoiceListVM = (InvoiceListViewModel)DataContext;
                if (T360ErrorCodes.NotInReviewerQueue.Equals(te.ErrorCodes[0].Code))
                {
                    invoiceListVM.ReloadList(-1);
                }
                else
                {
                    string msg = invoiceListVM.getErrorMessages(te);
                    invoiceListVM.showErrorMessages(msg);
                }
            }
        }

        private async Task<ConfirmationView> CreateConfirmationView()
        {
            ConfirmationView confirmationView = new ConfirmationView();

            ConfirmationViewModel confirmationVM = new ConfirmationViewModel();
            confirmationVM.HeaderText = "Approve (" + InvoiceList.SelectedItems.Count + ")";
            confirmationVM.ConfirmationList = InvoiceList.SelectedItems.Cast<InvoiceListDisplayFields>().ToList();
            confirmationVM.TotalAmount = await GetTotalConfirmationAmount(confirmationVM.ConfirmationList);
            confirmationVM.InitializeCommands(ConfirmationViewModel.ConfirmationSource.Invoice_Approve);

            confirmationView.DataContext = confirmationVM;

            confirmationView.Height = 0.6 * Window.Current.CoreWindow.Bounds.Bottom;
            confirmationView.Width = 0.5 * Window.Current.CoreWindow.Bounds.Right;

            return confirmationView;
        }

        private FailureView CreateFailureView(InvoiceErrorDetails errorDetails)
        {
            FailureView failureView = new FailureView();

            FailureViewModel failureVM = new FailureViewModel();
            failureVM.HeaderText = errorDetails.Header;
            failureVM.FailureTitle = IsDisallow(errorDetails.PageType)
                ? Constants.DisallowHeader
                : IsWarning(errorDetails.PageType) ? Constants.WarningHeader
                                 : "Failed (" + GetFailedCount(errorDetails.ErrorDetails) + ")";
            failureVM.ApproveErrorItems = GetApproveErrorItems(errorDetails.ErrorDetails);
            failureVM.InvoiceList = errorDetails.InvoiceBasicDetails;
            failureVM.InitializeCommands(IsWarning(errorDetails.PageType));

            failureView.DataContext = failureVM;

            failureView.Height = 0.6 * Window.Current.CoreWindow.Bounds.Bottom;
            failureView.Width = 0.5 * Window.Current.CoreWindow.Bounds.Right;

            return failureView;
        }

        private bool IsDisallow(string input) { return Constants.Disallow.Equals(input); }
        private bool IsWarning(string input) { return Constants.Warning.Equals(input); }
        private bool IsFailure(string input) { return Constants.Failure.Equals(input); }

        private int GetFailedCount(List<Error> errors)
        {
            int count = 0;
            for (int i = 1; i < errors.Count; i++)
            {
                count += errors[i].Data.Count;
            }
            return count;
        }

        private List<ApproveErrorItem> GetApproveErrorItems(List<Error> errors)
        {
            List<ApproveErrorItem> items = new List<ApproveErrorItem>();
            ApproveErrorItem item;
            bool isFailure = Constants.Failure.ToUpper().Equals(errors[0].Code.ToUpper());
            for (int i = 1; i < errors.Count; i++)
            {
                item = new ApproveErrorItem();
                string msg = T360ErrorCodes.GetError(errors[i].Code);
                item.Message = string.IsNullOrEmpty(msg) ? errors[i].Code : msg;
                item.InvoiceNumbers = new List<InvoiceNumber>();
                foreach (string data in errors[i].Data)
                {
                    item.InvoiceNumbers.Add(new InvoiceNumber() { InvoiceNo = data });
                }
                items.Add(item);
            }
            return items;
        }

        private void confirmationPopup_Closed(object sender, object e)
        {
            Popup popup = (Popup)sender;
            popup.Closed -= confirmationPopup_Closed;
        }

        private async Task<string> GetTotalConfirmationAmount(List<InvoiceListDisplayFields> selectedInvoices)
        {
            List<string> selectedIds = new List<string>();
            selectedIds.AddRange(selectedInvoices.Select(x => x.InvoiceId.ToString()));

            Dictionary<string, List<string>> selectedInvoiceIds = new Dictionary<string, List<string>>();
            selectedInvoiceIds.Add("SelectedInvoiceIds", selectedIds);

            return await ServiceInvoker.Instance.InvokeServiceUsingPost<string>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.CalculateInvoiceNetAmount), selectedInvoiceIds, false, false);
        }

        private void SelectAll(string emptyString)
        {
            InvoiceList.SelectAll();
        }

        private void ClearAll(string emptyString)
        {
            InvoiceList.SelectedItems.Clear();
        }

        private bool canCallServer = true;
        private int previousSelectedInvoice = -1;
        private async void InvoiceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InvoiceListViewModel invoiceListVM = (InvoiceListViewModel)DataContext;
            if (invoiceListVM.MultiSelect)
            {
                invoiceListVM.InvoiceCount = InvoiceList.SelectedItems.Count.ToString();
                return;
            }
            if (ApplicationView.Value == ApplicationViewState.Snapped)
            {
                ApplicationView.TryUnsnap();
            }
            try
            {
                if (invoiceListVM.SelectedInvoice > -1)
                {
                    if (canCallServer)
                    {
                        bool selectionSuccess = await invoiceListVM.SetSummary(invoiceListVM.InvoiceDetails[invoiceListVM.SelectedInvoice].InvoiceId);
                        if (selectionSuccess)
                        {
                            previousSelectedInvoice = invoiceListVM.SelectedInvoice;
                        }
                        else
                        {
                            invoiceListVM.SelectedInvoice = previousSelectedInvoice;
                        }
                        if (invoiceListVM.SelectedInvoice == -1)
                        {
                            if (invoiceListVM.InvoiceDetails.Count > 0)
                            {
                                UserPreference.Instance.SelectedInvoiceId = 0;
                                InvoiceList.ScrollIntoView(InvoiceList.Items[0], ScrollIntoViewAlignment.Default);
                            }
                        }
                        else
                        {
                            UserPreference.Instance.SelectedInvoiceId = invoiceListVM.InvoiceDetails[invoiceListVM.SelectedInvoice].InvoiceId;
                            InvoiceList.ScrollIntoView(InvoiceList.Items[invoiceListVM.SelectedInvoice], ScrollIntoViewAlignment.Default);
                        }
                    }
                    else
                    {
                        canCallServer = true;
                    }
                    invoiceListVM.HasSelectedInvoice = Visibility.Visible;
                }
                if (invoiceListVM.SelectedInvoice == -1)
                {
                    invoiceListVM.HasSelectedInvoice = Visibility.Collapsed;
                }
            }
            catch (T360Exception ex)
            {
                if (T360ErrorCodes.NotInReviewerQueue.Equals(ex.ErrorCodes[0].Code))
                {
                    invoiceListVM.ReloadList(-1);
                }
                else
                {
                    string msg = invoiceListVM.getErrorMessages(ex);
                    invoiceListVM.showErrorMessages(msg);
                }
            }
        }

        protected override void OnWindowSizeChanged()
        {
            InvoiceListViewModel invoiceListVM = (InvoiceListViewModel)DataContext;
            invoiceListVM.SetAppBar();
        }
    }
}