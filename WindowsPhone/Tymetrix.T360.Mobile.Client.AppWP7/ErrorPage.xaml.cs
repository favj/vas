/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using Microsoft.Phone.Controls;

namespace Tymetrix.T360.Mobile.Client.AppWP7
{
    public partial class ErrorPage : PhoneApplicationPage
    {
        public ErrorPage()
        {
            InitializeComponent();
        }

        public static Exception Exception
        {
            get;
            set;
        }
    }
}