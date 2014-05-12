using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TyMetrix360.App.Common;
using TyMetrix360.App.View.Template;
using TyMetrix360.BusinessObjects.Invoice;
using TyMetrix360.Core.ViewBase;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TyMetrix360.App.ViewModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TyMetrix360.App.View
{
    public sealed partial class FailureView : UserControlCore
    {
        public FailureView()
        {
            this.InitializeComponent();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            Messenger.Default.Register<string>(this, Constants.UnregisterFailureViewEvents, UnregisterEvents);
            Messenger.Default.Register<List<ApproveErrorItem>>(this, Constants.AddApproveItems, AddApproveItems);
        }

        private void UnregisterEvents(string emptyString)
        {
            Messenger.Default.Unregister<string>(this, Constants.UnregisterFailureViewEvents, UnregisterEvents);
            Messenger.Default.Unregister<List<ApproveErrorItem>>(this, Constants.AddApproveItems, AddApproveItems);
        }

        private void AddApproveItems(List<ApproveErrorItem> approveItems)
        {
            foreach (ApproveErrorItem item in approveItems)
            {
                DetailsPanel.Children.Add(CreateFailureMessageTemplate(item.Message));
                DetailsPanel.Children.Add(CreateFailureNumberListTemplate(item.InvoiceNumbers));
            }
        }

        private FailureNumberListTemplate CreateFailureNumberListTemplate(List<InvoiceNumber> invoiceList)
        {
            FailureNumberListTemplate numListTemplate = new FailureNumberListTemplate();
            FailureNumberListViewModel failureNumListVM = new FailureNumberListViewModel();
            failureNumListVM.InvoiceNumbers = invoiceList;
            numListTemplate.DataContext = failureNumListVM;
            return numListTemplate;
        }

        private FailureMessageTemplate CreateFailureMessageTemplate(string message)
        {
            FailureMessageTemplate msgTemplate = new FailureMessageTemplate();
            FailureMessageViewModel failureMessageVM = new FailureMessageViewModel();
            failureMessageVM.Message = message;
            msgTemplate.DataContext = failureMessageVM;
            return msgTemplate;
        }
    }
}
