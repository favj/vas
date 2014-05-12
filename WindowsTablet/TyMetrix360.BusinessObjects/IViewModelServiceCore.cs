using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core.ViewModelBase;

namespace TyMetrix360.BusinessObjects
{
    public interface IViewModelServiceCore : IViewModelCore
    {
        IService Service {get;}
    }
}
