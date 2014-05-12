using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.Core.Interfaces;
using Windows.UI.Xaml;

namespace TyMetrix360.BusinessObjects.LineItem
{
    public class LineItem : BusinessObjectCore, ISupportRowIndex
    {
        private int _index;
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }
        private string _lineItemId;
        public string LineItemId
        {
            get { return _lineItemId; }
            set { SetProperty(ref _lineItemId, value); }
        }
        private string _date;
        public string Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }
        private string _flags;
        public string Flags
        {
            get { return _flags; }
            set { SetProperty(ref _flags, value); }
        }
        private Visibility _flagVisible;
        public Visibility FlagVisible
        {
            get { return _flagVisible; }
            set { SetProperty(ref _flagVisible, value); }
        }
        private string _timeKeeper;
        public string TimeKeeper
        {
            get { return _timeKeeper; }
            set { SetProperty(ref _timeKeeper, value); }
        }
        private string _amount;
        public string Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }
        private string _narrativeText;
        public string NarrativeText
        {
            get { return _narrativeText; }
            set { SetProperty(ref _narrativeText, value); }
        }
        private string _netTotal;
        public string NetTotal
        {
            get { return _netTotal; }
            set { SetProperty(ref _netTotal, value); }
        }
    }
}
