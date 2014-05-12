using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TyMetrix360.Core.ViewModelBase;

namespace TyMetrix360.App.ViewModel
{
    public class FailureMessageViewModel : ViewModelCore
    {
        private string message;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }
    }
}
