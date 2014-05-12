/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using TyMetrix360.BusinessObjects.Common;

namespace TyMetrix360.BusinessObjects.Invoice
{
    public class BaseTyProperty :BusinessObjectCore
    {
        private string _labelText;
        public string LabelText
        {
            get { return _labelText; }
            set { SetProperty(ref _labelText, value); }
        }
        private string _valueText;
        public string ValueText
        {
            get { return _valueText; }
            set { SetProperty(ref _valueText, value); }
        }
    }
}
