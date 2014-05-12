/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using TyMetrix360.Core.Interfaces;

namespace TyMetrix360.BusinessObjects.Common
{
    public class FAQItem : BusinessObjectCore, ISupportRowIndex 
    {
        private int _index;
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        private string _question;
        public string Question
        {
            get { return _question; }
            set { SetProperty(ref _question, value); }
        }

        private string _answer;
        public string Answer
        {
            get { return _answer; }
            set { SetProperty(ref _answer, value); }
        }
    }
}
