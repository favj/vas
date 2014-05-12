using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TyMetrix360.BusinessObjects.Invoice;
using TyMetrix360.Core.ViewModelBase;

namespace TyMetrix360.App.ViewModel
{
    public class FailureNumberListViewModel : ViewModelCore
    {
        private List<InvoiceNumber> invoiceNumbers;
        public List<InvoiceNumber> InvoiceNumbers
        {
            get { return invoiceNumbers; }
            set { SetProperty(ref invoiceNumbers, value); }
        }
    }
}
