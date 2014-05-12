using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core.Container;
using TyMetrix360.Core.ViewModelBase;

namespace TyMetrix360.BusinessObjects
{
    public class ViewModelServiceCore : ViewModelCore, IViewModelServiceCore
    {
        private IService _service;
        public IService Service
        {
            get
            {
                if (_service == null)
                {
                    _service = Container.Resolve<IService>();
                }
                return _service;
            }
        } 
    }
}
