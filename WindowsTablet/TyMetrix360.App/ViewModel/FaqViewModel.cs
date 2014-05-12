/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TyMetrix360.App.Common;
using TyMetrix360.App.Navigation;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core;
using TyMetrix360.Core.Container;
using TyMetrix360.Core.Models;
using TyMetrix360.Core.ViewModelBase;

namespace TyMetrix360.App.ViewModel
{
    public class FaqViewModel : ViewModelCore, IFaqViewModel
    {
        private ObservableCollection<FAQItem> _faqList;
        public ObservableCollection<FAQItem> FaqList
        {
            get { return _faqList; }
            set { SetProperty(ref _faqList, value); }
        }

        public override async Task LoadData(params object[] parameters)
        {

            FaqList = new ObservableCollection<FAQItem>();
            FaqList.Add(new FAQItem()
            {
                Question = "I can’t see any of my matter information, why not?",
                Answer = "The module for matters has not been built yet, we hope to include this functionality in a future release. To view matter information, please log into the desktop version of TyMetrix 360°."
            });
            FaqList.Add(new FAQItem()
            {
                Question = "Can I approve, reject, and adjust Line Items on an invoice?",
                Answer = "No. If you need to perform these functions you must log into the desktop version of TyMetrix 360°. We will deliver these features in a future version of the mobile application. You can, however, perform the approve, reject and adjust actions at the invoice level."
            });
            FaqList.Add(new FAQItem()
            {
                Question = "Can I undo my adjustments?",
                Answer = "No. If you need to perform this function you must log into the desktop version of TyMetrix 360°."
            });
            FaqList.Add(new FAQItem()
            {
                Question = "I accidentally approved an invoice. Can I undo the approval?",
                Answer = "No, this is not allowed"
            });
            FaqList.Add(new FAQItem()
            {
                Question = "I am a law firm or vendor that has been allowed to login, but all I can do is view the FAQs and support information, why is that?",
                Answer = "This first version of the TyMetrix 360° mobile application is not intended for law firm or vendor use.  We do plan to include features for you in the near future."
            });
            FaqList.Add(new FAQItem()
            {
                Question = "In my network instead of Approve and Adjust buttons we have an ‘Approve and Forward’ and ‘Adjust Net’ buttons, is this functionality included in the mobile app?",
                Answer = "No. If you need to perform these functions you must log into the desktop version of TyMetrix 360°."
            });
            FaqList.Add(new FAQItem()
            {
                Question = "In the desktop version, I am able to define which screen I see after I approve or reject an invoice (such as next invoice summary, next invoice line item details or stay on the same invoice), can I do this on the mobile app as well?",
                Answer = "Not at this time. Once you approve an invoice the mobile application will bring you to the invoice summary of the next invoice pending your approval."
            });
            FaqList.Add(new FAQItem()
            {
                Question = "I’m used to seeing a currency selector icon on my invoice screens; does this functionality exist in the mobile version of the application?",
                Answer = "Yes. On the homepage of the mobile application, click on Settings to switch between your preferred and original currency for reviewing invoices."
            });
            FaqList.Add(new FAQItem()
            {
                Question = "I’m used to seeing the credit notes added by the previous reviewer; does this functionality exist in the mobile version of the application?",
                Answer = "No. If you need to see the credit notes added by the previous reviewer you must log into the desktop version of TyMetrix 360°."
            });
        }

        private IRelayCommand _goBackCommand;
        public IRelayCommand GoBackCommand
        {
            get { return _goBackCommand; }
            set { SetProperty(ref _goBackCommand, value); }
        }

        private void GoBackToPage()
        {
            try
            {
                if (!ServiceInvoker.Instance.IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);

                Navigator.Navigate(Destination.DashboardView, ExistingViewBehavior.Remove, new object[] { null });
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.FAQFailed);
            }
        }

        public FaqViewModel()
        {
            GoBackCommand = new RelayCommand((e) => { GoBackToPage(); });
        }
    }
}
