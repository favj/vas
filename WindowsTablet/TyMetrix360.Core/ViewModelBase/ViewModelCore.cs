/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

using TyMetrix360.Core.Models;

namespace TyMetrix360.Core.ViewModelBase
{
    public class ViewModelCore : NotificationCore, IViewModelCore
    {
        private const string OKLabel = "OK";
        private const string CancelLabel = "Cancel";
        private const string SuccessTitle = "Success";

        public void Initialize(params object[] parameters)
        {
            LoadData(parameters);
        }

        public virtual Task LoadData(params object[] parameters)
        {
            return null;
        }

        protected async Task<IUICommand> ShowErrorMessage(string message, string title)
        {
            var dialog = new MessageDialog(message, title);
            var result = await dialog.ShowAsync();
            IsBusy = false;
            return result;
        }

        private bool ConfirmationResult;
        protected async Task<bool> ShowConfirmationMessage(string message, string title)
        {
            UICommandInvokedHandler okHandler = new UICommandInvokedHandler(OnOkClick);
            UICommandInvokedHandler cancelHandler = new UICommandInvokedHandler(OnCancelClick);
            var dialog = new MessageDialog(message, title);
            dialog.Commands.Add(new UICommand()
            {
                Id = 1,
                Invoked = okHandler,
                Label = OKLabel
            });
            dialog.Commands.Add(new UICommand()
            {
                Id = 2,
                Invoked = cancelHandler,
                Label = CancelLabel
            });
            var result = await dialog.ShowAsync();
            IsBusy = false;
            return ConfirmationResult;
        }

        private void OnOkClick(IUICommand command) { ConfirmationResult = true; }

        private void OnCancelClick(IUICommand command) { ConfirmationResult = false; }

        protected async Task<IUICommand> ShowSuccessMessage(string message)
        {
            var dialog = new MessageDialog(message, SuccessTitle);
            var result = await dialog.ShowAsync();
            return result;
        }

        protected string getMessages(Exception exception)
        {
            string messages = string.Empty;
            if (exception is T360Exception)
            {
                foreach (Error error in ((T360Exception)exception).ErrorCodes)
                {
                    messages += T360ErrorCodes.GetError(error.Code) + "\n";
                }
                return messages;
            }
            return T360ErrorCodes.UnknownErrorMsg;
        }

        public virtual bool ShowAppBar { get { return false; } }
        public virtual bool ShowSortListButton { get { return false; } }
        public virtual bool ShowCancelButton { get { return false; } }
        public virtual bool ShowApproveButton { get { return false; } }
        public virtual bool ShowAdjustButton { get { return false; } }
        public virtual bool ShowRejectButton { get { return false; } }
        public virtual bool ShowUndoButton {  get { return false; } }
        public virtual bool ShowAddNotesButton { get { return false; } }
        public virtual bool ShowDetailsButton { get { return false; } }
        public virtual bool ShowPrivacyPolicyButton { get { return false; } }
        public virtual bool ShowSelectAllButton { get { return false; } }
        public virtual bool ShowDocumentsButton { get { return false; } }
        public virtual bool ShowClearButton { get { return false; } }
        public virtual bool ShowExpandButton { get { return false; } }
    }
}
