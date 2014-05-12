/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Threading.Tasks;

using TyMetrix360.Core.ViewModelBase;

namespace TyMetrix360.App.ViewModel
{
    public interface ISettingsViewModel : IViewModelCore
    {
        Task LoadData();
    }
}
