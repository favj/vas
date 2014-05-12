/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Threading.Tasks;

using TyMetrix360.Core.ViewModelBase;

namespace TyMetrix360.App.ViewModel
{
    public interface ILoginViewModel : IViewModelCore
    {
        bool IntegratedLogin { get; set; }
        string IntegratedLoginID { get; set; }

        Task LoadData();
        void OnLogin(object password);
    }
}
