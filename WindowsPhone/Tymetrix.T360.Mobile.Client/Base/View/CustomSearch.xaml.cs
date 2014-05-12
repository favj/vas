using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Tymetrix.T360.Mobile.Client.Common.Base.View
{
    public partial class CustomSearch : UserControl
    {
        public CustomSearch()
        {
            InitializeComponent();
            txtCustomSearch.Focus();
        }

        public TextBox GetSearchTextBox()
        {
            return txtCustomSearch;
        }
    }
}
