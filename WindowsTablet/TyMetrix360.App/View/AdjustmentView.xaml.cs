/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using GalaSoft.MvvmLight.Messaging;
using System;
using System.Globalization;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using TyMetrix360.App.Helper;
using TyMetrix360.App.Navigation;
using TyMetrix360.App.ViewModel;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.Core.ViewBase;
using TyMetrix360.App.Common;
using TyMetrix360.BusinessObjects.Invoice;

namespace TyMetrix360.App.View
{
    public sealed partial class AdjustmentView : ViewCore, IAdjustmentView 
    {
        private SolidColorBrush pressed = new SolidColorBrush(Colors.White);
        private SolidColorBrush normal = new SolidColorBrush(Colors.Transparent);

        private SolidColorBrush pressedText = new SolidColorBrush(ColorHelper.FromArgb(255, 26, 70, 145));
        private SolidColorBrush normalText = new SolidColorBrush(Colors.White);

        private string initialValue;
        private InputPaneHelper inputPaneHelper;
        public AdjustmentView()
        {
            this.InitializeComponent();
            inputPaneHelper = new InputPaneHelper();
            InputPaneShowingHandler amountInputPaneShowingHandler = new InputPaneShowingHandler(CustomKeyboardHandler);
            InputPaneShowingHandler narrativeInputPaneShowingHandler = new InputPaneShowingHandler(CustomKeyboardHandler);
            inputPaneHelper.SubscribeToKeyboard(true);
            inputPaneHelper.AddShowingHandler(narrativeText, narrativeInputPaneShowingHandler);
            inputPaneHelper.AddShowingHandler(amountText, amountInputPaneShowingHandler);
            inputPaneHelper.AddShowingHandler(amountTextFilled, amountInputPaneShowingHandler);
            inputPaneHelper.SetHidingHandler(new InputPaneHidingHandler(InputPaneHiding));
            SetAmountPercentage(true);
            SetPositiveNegativeButtons(false);
            Messenger.Default.Register<string>(this, Constants.AdjustDefaultSettings, SetDefaults);
        }

        private void SetDefaults(string emptyValue)
        {
            SetAmountPercentage(true);
            SetPositiveNegativeButtons(false);
        }

        private IAdjustmentViewModel _viewModel;
        public IAdjustmentViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = (IAdjustmentViewModel)this.DataContext;
                }
                return _viewModel;
            }
            set 
            { 
                  _viewModel = value; 
            }
        }

        private void Fee_Checked(object sender, RoutedEventArgs e)
        {
            CalculateAndSetReasons(true);
        }

        private void Expense_Checked(object sender, RoutedEventArgs e)
        {
            CalculateAndSetReasons(false);
        }

        private void CalculateAndSetReasons(bool isFee)
        {
            feeButton.IsChecked = isFee;
            expenseButton.IsChecked = !isFee;
            var d = DataContext as IAdjustmentViewModel;
            
            if (d.NetAmount == null) return;

            d.AdjustmentToggleType = isFee ? AdjustmentToggleType.Fees : AdjustmentToggleType.Expenses;
            string adjustAmount = d.AdjustmentToggleType == AdjustmentToggleType.Expenses
                                ? d.InvoiceSummary.BilledExpenses
                                : d.InvoiceSummary.BilledFees;
            if (d.AdjustmentByTo == AdjustmentByTo.AdjustBy)
            {
                d.Calculate();
                NetAmountText.Text = d.NetAmount;
            }
            else
            {
                if (d.AdjustmentToggleType == AdjustmentToggleType.Expenses)
                {
                    d.NetAmount = d.InvoiceSummary.BilledExpenses;
                }
                else
                {
                    d.NetAmount = d.InvoiceSummary.BilledFees;
                }
            }
        }

        private void By_Checked(object sender, RoutedEventArgs e)
        {
            Calculate(true);
        }

        private void To_Checked(object sender, RoutedEventArgs e)
        {
            Calculate(false);
        }

        private void Calculate(bool isBy)
        {
            byButton.IsChecked = isBy;
            toButton.IsChecked = !isBy;
            var d = DataContext as AdjustmentViewModel;
            d.AdjustmentByTo = isBy ? AdjustmentByTo.AdjustBy : AdjustmentByTo.AdjustTo;
            d.Calculate();
        }
        private void CustomKeyboardHandler(object sender, InputPaneVisibilityEventArgs e)
        {
            Navigator.KeyboardOpen();
        }
        private void InputPaneHiding(InputPane sender, InputPaneVisibilityEventArgs e)
        {
            Navigator.KeyboardClosed();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            inputPaneHelper.SubscribeToKeyboard(false);
            inputPaneHelper.RemoveShowingHandler(narrativeText);
            inputPaneHelper.SetHidingHandler(null);
        }

        private void SetAmountPercentage(bool isAmount)
        {
            IAdjustmentViewModel d = DataContext as IAdjustmentViewModel;
            if (d != null) d.AdjustedAmount = 0.0;

            dollarPanelFilled.Background = dollarPanel.Background = isAmount ? pressed : normal;
            percentagePanelFilled.Background = percentagePanel.Background = isAmount ? normal : pressed;

            dollarTextFilled.Foreground = dollarText.Foreground = isAmount ? pressedText : normalText;
            percentageTextFilled.Foreground = percentageText.Foreground = isAmount ? normalText : pressedText;
        }
        private void SetPositiveNegativeButtons(bool isPositive)
        {
            plusPanelFilled.Background = plusPanel.Background = isPositive ? pressed : normal;
            minusPanelFilled.Background =  minusPanel.Background = isPositive ? normal : pressed;

            plusTextFilled.Foreground = plusText.Foreground = isPositive ? pressedText : normalText;
            minusTextFilled.Foreground = minusText.Foreground = isPositive ? normalText : pressedText;
        }
        private void AdjustmentTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            Clipboard.Clear();
            IAdjustmentViewModel viewModel = DataContext as IAdjustmentViewModel;
            TextBox txtInput = sender as TextBox;
            NumberFormatInfo numberFormat = NumberFormatInfo.CurrentInfo;
            string decimalSeparator = numberFormat.CurrencyDecimalSeparator;
            string groupSeparator = numberFormat.CurrencyGroupSeparator;
            initialValue = txtInput.Text;
            
            if (!e.Key.isValidNumberKey() && e.Key != VirtualKey.Back &&
                !e.Key.ToString().Equals(Constants.KeyCode190) && !e.Key.ToString().Equals(Constants.KeyCode188))
            {
                e.Handled = true;
            }
            string strValue = initialValue.Replace(groupSeparator, "");
            string[] digit = initialValue.Split(decimalSeparator.ToCharArray());
            double result;
            bool isDouble = double.TryParse(strValue, out result);
            if (isDouble && strValue.Length >= 12 && !strValue.Contains(decimalSeparator) &&
                e.Key != VirtualKey.Decimal && !e.Key.ToString().Equals(Constants.KeyCode188) && !e.Key.ToString().Equals(Constants.KeyCode190))
            {
                AssignValue(initialValue);
                e.Handled = true;
            }
            else if ((isDouble && strValue.Contains(decimalSeparator) && (e.Key == VirtualKey.Decimal ||
                e.Key.ToString().Equals(Constants.KeyCode188) || e.Key.ToString().Equals(Constants.KeyCode190))) || strValue.Length >= 15)
            {
                AssignValue(initialValue);
                e.Handled = true;
                return;
            }
            else if (e.Key.ToString().Equals(Constants.KeyCode190) &&
                groupSeparator.Equals(".") && !strValue.Contains("."))
            {
                AssignValue(initialValue);
                Focus(initialValue);
                e.Handled = true;
                return;
            }
            else if (e.Key.ToString() == Constants.KeyCode188 && groupSeparator.Equals(","))
            {
                AssignValue(initialValue);
                Focus(initialValue);
                e.Handled = true;
            }
            else if (!isDouble && (e.Key == VirtualKey.Back || e.Key.ToString().Equals(Constants.KeyCode188) ||
                e.Key.ToString().Equals(Constants.KeyCode190)))
            {
                AssignValue(initialValue);
                e.Handled = true;
            }
            else if ("0".Equals(initialValue) && (e.Key == VirtualKey.Number0 || e.Key == VirtualKey.NumberPad0))
            {
                AssignValue(initialValue);
                e.Handled = true;
            }
            else if (digit.Length == 2 && !string.IsNullOrWhiteSpace(digit[1]) && digit[1].Length == 2)
            {
                AssignValue(initialValue);
                e.Handled = true;
            }

            if (!String.IsNullOrWhiteSpace(txtInput.Text) && viewModel.AdjustmentType == AdjustmentType.Percentage)
            {
                try
                {
                    double val = Convert.ToDouble(txtInput.Text + GetNumber(e.Key));
                    e.Handled = val > 100;
                    if (e.Key == VirtualKey.Back || (!e.Key.isValidNumberKey() && !e.Key.ToString().Equals(Constants.KeyCode190) && 
                        !e.Key.ToString().Equals(Constants.KeyCode188)) || (digit.Length == 2 && digit[1] != null && digit[1].Length == 2))
                    {
                        e.Handled = true;
                    }
                }
                catch (Exception)
                {
                    AssignValue(string.Empty);
                    e.Handled = true;
                }
            }
        }

        private void AdjustmentTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            try
            {
                Clipboard.Clear();
                NumberFormatInfo numberFormat = NumberFormatInfo.CurrentInfo;
                string groupSeparator = numberFormat.CurrencyGroupSeparator;
                string decimalSeparator = numberFormat.CurrencyDecimalSeparator;
                TextBox tbAmount = sender as TextBox;

                if (e.Key == VirtualKey.Back && tbAmount.Text.EndsWith(decimalSeparator))
                {
                    AssignValue(initialValue);
                    Focus(initialValue);
                    return;
                }

                string strTextValue = tbAmount.Text.Replace(groupSeparator, "");
                double num;
                if ((double.TryParse(strTextValue, out num) && strTextValue.Length <= 12) ||
                    (e.Key == VirtualKey.Back && string.IsNullOrEmpty(strTextValue)) ||
                    e.Key == VirtualKey.Enter || strTextValue.Contains(decimalSeparator))
                {
                    PreCallRecalculate(sender);
                    Focus(tbAmount.Text);
                }
                else
                {
                    if (string.IsNullOrEmpty(initialValue))
                    {
                        if (double.TryParse(strTextValue, out num))
                        {
                            PreCallRecalculate(sender);
                        }
                        else
                        {
                            AssignValue(string.Empty);
                        }
                    }
                    else if (initialValue != null)
                    {
                        AssignValue(initialValue);
                        Focus(initialValue);
                    }
                }
                initialValue = string.Empty;
            }
            catch (Exception)
            {
                AssignValue(string.Empty);
            }
        }

        private string GetNumber(VirtualKey key)
        {
            switch (key)
            {
                case VirtualKey.Number0: return "0";
                case VirtualKey.Number1: return "1";
                case VirtualKey.Number2: return "2";
                case VirtualKey.Number3: return "3";
                case VirtualKey.Number4: return "4";
                case VirtualKey.Number5: return "5";
                case VirtualKey.Number6: return "6";
                case VirtualKey.Number7: return "7";
                case VirtualKey.Number8: return "8";
                case VirtualKey.Number9: return "9";

                case VirtualKey.NumberPad0: return "0";
                case VirtualKey.NumberPad1: return "1";
                case VirtualKey.NumberPad2: return "2";
                case VirtualKey.NumberPad3: return "3";
                case VirtualKey.NumberPad4: return "4";
                case VirtualKey.NumberPad5: return "5";
                case VirtualKey.NumberPad6: return "6";
                case VirtualKey.NumberPad7: return "7";
                case VirtualKey.NumberPad8: return "8";
                case VirtualKey.NumberPad9: return "9";

                default: return "";
            }
        }

        private void AdjustmentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtInput = sender as TextBox;
            if (string.IsNullOrEmpty(txtInput.Text))
            {
                PreCallRecalculate(sender);
            }
        }

        private void PreCallRecalculate(object sender)
        {
            var txtInput = sender as TextBox;
            NumberFormatInfo numberFormat = NumberFormatInfo.CurrentInfo;
            string decimalSeparator = numberFormat.CurrencyDecimalSeparator;
            string groupSeparator = numberFormat.CurrencyGroupSeparator;
            var d = DataContext as IAdjustmentViewModel;
            try
            {
                string value = txtInput.Text;
                if (value == null || string.IsNullOrEmpty(value))
                {
                    value = string.Empty;
                    d.AdjustedAmount = 0;
                    d.Calculate();
                    return;
                }
                value = value.Replace(groupSeparator, "");
                if (value.Contains(decimalSeparator))
                {
                    string[] strDecimalData = value.Split(decimalSeparator.ToCharArray());
                    if (strDecimalData[1].Length > 2)
                    {
                        value = strDecimalData[0] + decimalSeparator + strDecimalData[1].Remove(2);
                        d.AdjustedAmount = Convert.ToDouble(value);
                        AssignValue(d.AdjustedAmount.ToString());
                    }
                    else if (strDecimalData[0].Length > 12)
                    {
                        value = initialValue.Replace(groupSeparator, "");
                        d.AdjustedAmount = Convert.ToDouble(value);
                    }
                    else
                    {
                        d.AdjustedAmount = double.Parse(value);
                    }
                }
                else
                {
                    d.AdjustedAmount = double.Parse(value);
                }
                d.Calculate();
            }
            catch (Exception)
            {
                d.AdjustedAmount = 0.0;
            }
        }
        
        private void AssignValue(string data)
        {
            if (ApplicationView.Value == ApplicationViewState.FullScreenLandscape)
            {
                amountTextFilled.Text = amountText.Text = data;
            }
            else
            {
                amountText.Text = amountTextFilled.Text = data;
            }
        }

        private void Focus(string amount)
        {
            if (ApplicationView.Value == ApplicationViewState.FullScreenLandscape)
            {
                amountText.Select(amount.Length, amount.Length);
            }
            else
            {
                amountTextFilled.Select(amount.Length, amount.Length);
            }
        }

        private void Subtract_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SetPositiveNegativeButtons(false);
        }

        private void Subtract_Click(object sender, RoutedEventArgs e)
        {
            SetPositiveNegativeButtons(false);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            SetPositiveNegativeButtons(true);
        }

        private void Add_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SetPositiveNegativeButtons(true);
        }

        private void Amount_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SetAmountPercentage(true);
        }

        private void Amount_Click(object sender, RoutedEventArgs e)
        {
            SetAmountPercentage(true);
        }

        private void Percentage_Click(object sender, RoutedEventArgs e)
        {
            SetAmountPercentage(false);
        }

        private void Percentage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SetAmountPercentage(false);
        }

        public bool IsMain { get; set; }

        private void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ApplicationView.Value == ApplicationViewState.Snapped)
            {
                ApplicationView.TryUnsnap();
            }
        }

        private void onContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }

        private void narrativeText_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            Clipboard.Clear();
        }

        private void narrativeText_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            Clipboard.Clear();
        }
    }
}
