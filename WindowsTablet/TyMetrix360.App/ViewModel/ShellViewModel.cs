/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;

using TyMetrix360.App.CommandParameters;
using TyMetrix360.App.Common;
using TyMetrix360.App.Navigation;
using TyMetrix360.App.View;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core;
using TyMetrix360.Core.Container;
using TyMetrix360.Core.Models;
using TyMetrix360.Core.ViewModelBase;
using TyMetrix360.BusinessObjects.Invoice;
using TyMetrix360.BusinessObjects.Security;
using TyMetrix360.BusinessObjects.Common;

namespace TyMetrix360.App.ViewModel
{
    public class ShellViewModel : ViewModelCore, IShellViewModel
    {
        private DispatcherTimer timer;

        private InvoiceSummary _invoice;
        public InvoiceSummary Invoice
        {
            get { return _invoice; }
            set { SetProperty(ref _invoice, value); }
        }
        private int _lineItemId;
        public int LineItemId
        {
            get { return _lineItemId; }
            set { SetProperty(ref _lineItemId, value); }
        }
        public INavigationItem _currentPage;
        public INavigationItem CurrentPage
        {
            get { return _currentPage; }
            set { SetProperty(ref _currentPage, value); }
        }
        private IRelayCommand _sortListCommand;
        public IRelayCommand SortListCommand
        {
            get { return _sortListCommand; }
            set { SetProperty(ref _sortListCommand, value); }
        }
        private IRelayCommand _cancelCommand;
        public IRelayCommand CancelCommand
        {
            get { return _cancelCommand; }
            set { SetProperty(ref _cancelCommand, value); }
        }
        private IRelayCommand _approveCommand;
        public IRelayCommand ApproveCommand
        {
            get { return _approveCommand; }
            set { SetProperty(ref _approveCommand, value); }
        }
        private IRelayCommand _adjustCommand;
        public IRelayCommand AdjustCommand
        {
            get { return _adjustCommand; }
            set { SetProperty(ref _adjustCommand, value); }
        }
        private IRelayCommand _rejectionCommand;
        public IRelayCommand RejectionCommand
        {
            get { return _rejectionCommand; }
            set { SetProperty(ref _rejectionCommand, value); }
        }
        private IRelayCommand _undoCommand;
        public IRelayCommand UndoCommand
        {
            get { return _undoCommand; }
            set { SetProperty(ref _undoCommand, value); }
        }
        private IRelayCommand _addNotesCommand;
        public IRelayCommand AddNotesCommand
        {
            get { return _addNotesCommand; }
            set { SetProperty(ref _addNotesCommand, value); }
        }
        private IRelayCommand _detailsCommand;
        public IRelayCommand DetailsCommand
        {
            get { return _detailsCommand; }
            set { SetProperty(ref _detailsCommand, value); }
        }
        private IRelayCommand _privacyPolicyCommand;
        public IRelayCommand PrivacyPolicyCommand
        {
            get { return _privacyPolicyCommand; }
            set { SetProperty(ref _privacyPolicyCommand, value); }
        }
        private IRelayCommand _selectAllCommand;
        public IRelayCommand SelectAllCommand
        {
            get { return _selectAllCommand; }
            set { SetProperty(ref _selectAllCommand, value); }
        }
        private IRelayCommand _documentsCommand;
        public IRelayCommand DocumentsCommand
        {
            get { return _documentsCommand; }
            set { SetProperty(ref _documentsCommand, value); }
        }
        private IRelayCommand _clearCommand;
        public IRelayCommand ClearCommand
        {
            get { return _clearCommand; }
            set { SetProperty(ref _clearCommand, value); }
        }
        private IRelayCommand _expandCommand;
        public IRelayCommand ExpandCommand
        {
            get { return _expandCommand; }
            set { SetProperty(ref _expandCommand, value); }
        }
        private LoginInfo _loginInfo;
        public LoginInfo LoginInfo
        {
            get { return _loginInfo; }
            set { SetProperty(ref _loginInfo, value); }
        }

        public ShellViewModel()
        {
            LoginInfo = new LoginInfo();
            CreateRegister();
            InitializeAppBarCommands();
            ResetTheTimer();
        }

        private void InitializeAppBarCommands()
        {
            SortListCommand = new RelayCommand(e => SortList());
            CancelCommand = new RelayCommand(e => CancelMultiSelect());
            ApproveCommand = new RelayCommand(e => Approve());
            AdjustCommand = new RelayCommand(e => CallAdjustment());
            RejectionCommand = new RelayCommand(e => CallRejection());
            UndoCommand = new RelayCommand(e => Undo());
            AddNotesCommand = new RelayCommand(e => AddNotes());
            DetailsCommand = new RelayCommand(e => GetDetailsPage());
            PrivacyPolicyCommand = new RelayCommand(e => CallPrivacy());
            SelectAllCommand = new RelayCommand(e => SelectAll());
            DocumentsCommand = new RelayCommand(e => ShowDocuments());
            ClearCommand = new RelayCommand(e => ClearAll());
            ExpandCommand = new RelayCommand(e => CallExpand());
        }

        private void CreateRegister()
        {
            Messenger.Default.Register<ReturnToPage>(this, (e) =>
            {
                SetCurrentValuesForPage(e);
            });
            Messenger.Default.Register<InvoiceParameter>(this, Constants.Adjust, (s) =>
            {
                CallAdjustInvoice(s.Invoice);
            });
            Messenger.Default.Register<InvoiceParameter>(this, Constants.Reject, (s) =>
            {
                CallRejectInvoice(s.Invoice);
            });
            Messenger.Default.Register<InvoiceParameter>(this, Constants.SetInvoice, (s) =>
            {
                Invoice = s.Invoice;
            });
            Messenger.Default.Register<InvoiceParameter>(this, Constants.SetInvoiceSummary, (s) =>
            {
                if (Invoice != null)
                {
                    Messenger.Default.Send<InvoiceParameter>(new InvoiceParameter() { Invoice = this.Invoice }, Constants.SendBackToInvoicePage);
                }
                else
                {
                    Messenger.Default.Send<InvoiceParameter>(new InvoiceParameter() { Invoice = s.Invoice }, Constants.SendBackToInvoicePage);
                }
            });
            Messenger.Default.Register<InvoiceParameter>(this, Constants.Details, (s) =>
            {
                GetDetailsPageMain(s.Invoice.InvoiceId);
            });
            Messenger.Default.Register<DetailParameter>(this, (s) =>
            {
                Messenger.Default.Send<InvoiceWithDetailParameter>(new InvoiceWithDetailParameter() { InvoiceId = Invoice.InvoiceId, InvoiceSummaryDetailId = s.InvoiceSummaryDetailId });
            });
            Messenger.Default.Register<ResetTimer>(this, (s) =>
            {
                ResetTheTimer();
            });
            Messenger.Default.Register<InvoiceParameter>(this, Constants.SendBackToLineItemDetailPage, (s) =>
            {
                SendBackToPage();
            });
            Messenger.Default.Register<IViewModelCore>(this, Constants.RefreshAppBar, (s) =>
            {
                RefreshAppBar(s);
            });
        }

        private void SetCurrentValuesForPage(ReturnToPage e)
        {
            if (e.Invoice != null)
            {
                this.Invoice = e.Invoice;
            }
            if (e.LineItemId > 0)
            {
                this.LineItemId = e.LineItemId;
            }
            this.CurrentPage = e.PageItem;
        }

        public void SendBackToPage()
        {
           
            if (CurrentPage == null || CurrentPage.ViewType == null || CurrentPage.ViewType == typeof(IInvoiceListView))
            {
                Navigator.Navigate(Destination.InvoiceListView,parameters: Invoice.InvoiceId);
                return;
            }
            if (CurrentPage.ViewType == typeof(IInvoiceLineItemsView))
            {
                Navigator.Navigate(Destination.InvoiceLineItemsView, parameters: new object[] {Invoice.InvoiceId, LineItemId});
            }
        }

        private void ResetTheTimer()
        {
            if (timer != null)
            {
                timer.Stop();
            }
            timer = new DispatcherTimer();
            //  System.Diagnostics.Debug.WriteLine("Touch Hit");
            timer.Interval = new System.TimeSpan(0,10,0);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, object e)
        {
            if (!string.IsNullOrWhiteSpace(UserPreference.Instance.CurrentUserName))
            {
                Navigator.ClosePopup();
                LogOut();
                ShowErrorMessage(T360ErrorCodes.SessionTimeOutMsg, Constants.AutoLogOff);
            }
        }

        private async Task LogOut()
        {
            UserPreference.Instance.Clear();
            await ServiceInvoker.Instance.LogOut();
            Navigator.Navigate(Destination.LoginView);
        }

        private Visibility _isMain;
        public Visibility IsMain
        {
            get { return _isMain; }
            set { SetProperty(ref _isMain, value); }
        }

#region Appbar Commands Implementation

        public void SortList()
        {
        }

        public void CancelMultiSelect()
        {
            Messenger.Default.Send<string>(string.Empty, Constants.CancelMultiSelect);
        }

        public void Approve()
        {
            Messenger.Default.Send(string.Empty, Constants.ApproveInvoice);
        }

        public void CallAdjustment()
        {
            Messenger.Default.Send<InvoiceParameter>(new InvoiceParameter() { Invoice = this.Invoice }, Constants.Adjust);
        }

        public void CallRejection()
        {
            Messenger.Default.Send<InvoiceParameter>(new InvoiceParameter() { Invoice = this.Invoice }, Constants.Reject);
        }

        public void Undo()
        {
        }

        public void AddNotes()
        {
        }

        public void GetDetailsPage()
        {
            try
            {
                if (!ServiceInvoker.Instance.IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);

                Messenger.Default.Send<InvoiceParameter>(new InvoiceParameter() { Invoice = this.Invoice }, Constants.Details);  
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.InvoiceLineItemFailed);
            }
        }

        public async void CallPrivacy()
        {
            var uri = new Uri("http://wvw.tymetrix.com/privacy/");
            Windows.System.Launcher.LaunchUriAsync(uri);
        }

        public void SelectAll()
        {
            Messenger.Default.Send<string>(string.Empty, Constants.MultiSelect_SelectAll);
        }

        public void ShowDocuments()
        {
        }

        public void ClearAll()
        {
            Messenger.Default.Send<string>(string.Empty, Constants.MultiSelect_ClearAll);
        }

        public void CallExpand()
        {
            Messenger.Default.Send<string>(string.Empty, Constants.Expand);
        }
#endregion

        public void GetDetailsPageMain(int invoiceNumber)
        {
            try
            {
                if (!ServiceInvoker.Instance.IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);

                if (invoiceNumber > 0)
                {
                    IsBusy = true;
                    Messenger.Default.Send<string>(string.Empty, Constants.UnregisterInvoiceListEvents);
                    Navigator.Navigate(Destination.InvoiceLineItemsView, OnUnloadView, parameters: invoiceNumber);
                    IsBusy = false;
                }
                else
                {
                    ShowErrorMessage(T360ErrorCodes.NotSelectedInvoiceDetailsMsg, Constants.Selection);
                }
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.InvoiceLineItemFailed);
                if (T360ErrorCodes.NotInReviewerQueue.Equals(ex.ErrorCodes[0].Code))
                {
                    Navigator.Navigate(Destination.InvoiceListView);
                }
            }
        }

        private void OnUnloadView(object sender, EventArgs e)
        {
            Messenger.Default.Send<string>(string.Empty, Constants.ToLineItemsFromInvoice);
        }

        public void CallAdjustInvoice(InvoiceSummary invoice)
        {
            try
            {
                if (!ServiceInvoker.Instance.IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);

                if (invoice != null)
                {
                    if (!invoice.Permissions.AdjustInvoiceAllowed)
                    {
                        ShowErrorMessage(T360ErrorCodes.InvoiceAdjustMsg, Constants.AdjustNotAllowed);
                        return;
                    }
                    Messenger.Default.Send<string>(string.Empty, Constants.UnregisterInvoiceListEvents);
                    Navigator.Navigate(Destination.AdjustmentView, parameters: invoice);
                }
                else
                {
                    ShowErrorMessage(T360ErrorCodes.NotSelectedInvoiceAdjustMsg, Constants.Selection);
                }
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.AdjustmentFailed);
            }
        }

        public void CallRejectInvoice(InvoiceSummary invoice)
        {
            try
            {
                if (!ServiceInvoker.Instance.IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);

                if (invoice != null)
                {
                    IsBusy = true;
                    Messenger.Default.Send<string>(string.Empty, Constants.UnregisterInvoiceListEvents);
                    Navigator.Navigate(Destination.RejectionView, parameters: invoice);
                    IsBusy = false;
                }
                else
                {
                    ShowErrorMessage(T360ErrorCodes.NotSelectedInvoiceRejectMsg, Constants.Selection);
                }
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.RejectionFailed);
            }
        }

        private bool _showAppBar;
        public override bool ShowAppBar { get { return _showAppBar; } }

        private bool _showSortListButton;
        public override bool ShowSortListButton { get { return _showSortListButton; } }

        private bool _showCancelButton;
        public override bool ShowCancelButton { get { return _showCancelButton; } }

        private bool _showApproveButton;
        public override bool ShowApproveButton { get { return _showApproveButton; } }

        private bool _showAdjustButton;
        public override bool ShowAdjustButton { get { return _showAdjustButton; } }

        private bool _showRejectButton;
        public override bool ShowRejectButton { get { return _showRejectButton; } }

        private bool _showUndoButton;
        public override bool ShowUndoButton { get { return _showUndoButton; } }

        private bool _showAddNotesButton;
        public override bool ShowAddNotesButton { get { return _showAddNotesButton; } }

        private bool _showDetailsButton;
        public override bool ShowDetailsButton { get { return _showDetailsButton; } }

        private bool _showPrivacyPolicyButton;
        public override bool ShowPrivacyPolicyButton { get { return _showPrivacyPolicyButton; } }

        private bool _showSelectAllButton;
        public override bool ShowSelectAllButton { get { return _showSelectAllButton; } }

        private bool _showDocumentsButton;
        public override bool ShowDocumentsButton { get { return _showDocumentsButton; } }

        private bool _showClearButton;
        public override bool ShowClearButton { get { return _showClearButton; } }

        private bool _showExpandButton;
        public override bool ShowExpandButton { get { return _showExpandButton; } }

        public void RefreshAppBar(IViewModelCore viewModelCore)
        {
            SetAppBarState(viewModelCore.ShowAppBar);
            SetAppBarSortListButtonsState(viewModelCore.ShowSortListButton);
            SetAppBarCancelButtonsState(viewModelCore.ShowCancelButton);
            SetAppBarApproveButtonsState(viewModelCore.ShowApproveButton);
            SetAppBarAdjustButtonsState(viewModelCore.ShowAdjustButton);
            SetAppBarRejectButtonsState(viewModelCore.ShowRejectButton);
            SetAppBarUndoButtonsState(viewModelCore.ShowUndoButton);
            SetAppBarAddNotesButtonsState(viewModelCore.ShowAddNotesButton);
            SetAppBarDetailsButtonsState(viewModelCore.ShowDetailsButton);
            SetAppBarPrivacyPolicyButtonsState(viewModelCore.ShowPrivacyPolicyButton);
            SetAppBarSelectAllButtonsState(viewModelCore.ShowSelectAllButton);
            SetAppBarDocumentsButtonsState(viewModelCore.ShowDocumentsButton);
            SetAppBarClearButtonsState(viewModelCore.ShowClearButton);
            SetAppBarExpandButtonsState(viewModelCore.ShowExpandButton);
        }

        private void SetAppBarState(bool isVisible)
        {
            SetProperty(ref _showAppBar, isVisible);
            OnPropertyChanged(Constants.ShowAppBar);
        }

        private void SetAppBarSortListButtonsState(bool isVisible)
        {
            SetProperty(ref _showSortListButton, isVisible);
            OnPropertyChanged(Constants.ShowSortListButton);
        }

        private void SetAppBarCancelButtonsState(bool isVisible)
        {
            SetProperty(ref _showCancelButton, isVisible);
            OnPropertyChanged(Constants.ShowCancelButton);
        }

        private void SetAppBarApproveButtonsState(bool isVisible)
        {
            SetProperty(ref _showApproveButton, isVisible);
            OnPropertyChanged(Constants.ShowApproveButton);
        }

        private void SetAppBarAdjustButtonsState(bool isVisible)
        {
            SetProperty(ref _showAdjustButton, isVisible);
            OnPropertyChanged(Constants.ShowAdjustButton);
        }

        private void SetAppBarRejectButtonsState(bool isVisible)
        {
            SetProperty(ref _showRejectButton, isVisible);
            OnPropertyChanged(Constants.ShowRejectButton);
        }

        private void SetAppBarUndoButtonsState(bool isVisible)
        {
            SetProperty(ref _showUndoButton, isVisible);
            OnPropertyChanged(Constants.ShowUndoButton);
        }

        private void SetAppBarAddNotesButtonsState(bool isVisible)
        {
            SetProperty(ref _showAddNotesButton, isVisible);
            OnPropertyChanged(Constants.ShowAddNotesButton);
        }

        private void SetAppBarPrivacyPolicyButtonsState(bool isVisible)
        {
            SetProperty(ref _showPrivacyPolicyButton, isVisible);
            OnPropertyChanged(Constants.ShowPrivacyPolicyButton);
        }

        private void SetAppBarSelectAllButtonsState(bool isVisible)
        {
            SetProperty(ref _showSelectAllButton, isVisible);
            OnPropertyChanged(Constants.ShowSelectAllButton);
        }

        private void SetAppBarDocumentsButtonsState(bool isVisible)
        {
            SetProperty(ref _showDocumentsButton, isVisible);
            OnPropertyChanged(Constants.ShowDocumentsButton);
        }

        private void SetAppBarClearButtonsState(bool isVisible)
        {
            SetProperty(ref _showClearButton, isVisible);
            OnPropertyChanged(Constants.ShowClearButton);
        }

        private void SetAppBarExpandButtonsState(bool isVisible)
        {
            SetProperty(ref _showExpandButton, isVisible);
            OnPropertyChanged(Constants.ShowExpandButton);
        }

        private void SetAppBarDetailsButtonsState(bool isVisible)
        {
            SetProperty(ref _showDetailsButton, isVisible);
            OnPropertyChanged(Constants.ShowDetailsButton);
        }
    }
}
