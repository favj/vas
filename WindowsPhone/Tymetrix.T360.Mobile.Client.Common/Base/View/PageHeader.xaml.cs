/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System.Windows.Controls;
using System.Windows.Media;

namespace Tymetrix.T360.Mobile.Client.Common.Base.View
{
    public partial class PageHeader : UserControl
    {
        public PageHeader()
        {
            InitializeComponent();
        }

        private bool isNavBarRequired;
        public bool IsNavBarRequired
        {
            get
            {
                return isNavBarRequired;
            }
            set
            {
                if (value)
                {
                    if (value)
                    {
                        leftImageButton.Visibility = System.Windows.Visibility.Visible;
                        rightImageButton.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        leftImageButton.Visibility = System.Windows.Visibility.Collapsed;
                        rightImageButton.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
                isNavBarRequired = value;
            }
        }

        private string headerTitle;
        public string HeaderTitle
        {
            set
            {
                headerTitle = value;
                headerlabel.Text = headerTitle;
            }
            get
            {
                return headerTitle;
            }
        }

        private string headerCount;
        public string HeaderCount
        {
            set
            {
                headerCount = value;
                headerlabel.Text = string.Format("{0} ({1})", HeaderTitle, headerCount);
            }
            get
            {
                return headerCount;
            }
        }

        private Color rectColor;
        public Color HeaderRectColor
        {
            set
            {
                rectColor = value;
                leftRectangle.Fill = new SolidColorBrush(rectColor);
            }
            get
            {
                return rectColor;
            }
        }
    }
}
