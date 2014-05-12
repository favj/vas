/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Tymetrix.T360.Mobile.Client.Common.Base.View;

namespace Tymetrix.T360.Mobile.Client.Common.Base.View
{
    public partial class OverlayProgressBar : UserControl
    {
        internal Popup ChildWindowPopup
        {
            get;
            private set;
        }

        public OverlayProgressBar()
        {
            InitializeComponent();
        }

        public void Show()
        {
            if (this.ChildWindowPopup == null)
            {
                this.ChildWindowPopup = new Popup();

                try
                {
                    this.ChildWindowPopup.Child = this;
                }
                catch (ArgumentException)
                {
                    throw new InvalidOperationException("The control is already shown.");
                }
            }

            if (this.ChildWindowPopup != null && Application.Current.RootVisual != null)
            {
                // Show popup
                this.ChildWindowPopup.IsOpen = true;
                this.ParentPage.IsEnabled = false;
            }
        }

        public void Hide()
        {
            if (this.ChildWindowPopup == null) return;
            this.ParentPage.IsEnabled = true;
            this.ChildWindowPopup.IsOpen = false;
        }

        public BasePage ParentPage { get; set; }
    }
}
