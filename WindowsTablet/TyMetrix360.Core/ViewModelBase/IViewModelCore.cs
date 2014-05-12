/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.ComponentModel;

namespace TyMetrix360.Core.ViewModelBase
{
    public interface IViewModelCore : INotifyPropertyChanged
    {
        void Initialize(params object[] parameters);
        bool IsBusy { get; }

        bool ShowAppBar { get; }
        bool ShowSortListButton { get; }
        bool ShowCancelButton { get; }
        bool ShowApproveButton { get; }
        bool ShowAdjustButton { get; }
        bool ShowRejectButton { get; }
        bool ShowUndoButton { get; }
        bool ShowAddNotesButton { get; }
        bool ShowDetailsButton { get; }
        bool ShowPrivacyPolicyButton { get; }
        bool ShowSelectAllButton { get; }
        bool ShowDocumentsButton { get; }
        bool ShowClearButton { get; }
        bool ShowExpandButton { get; }
    }
}
