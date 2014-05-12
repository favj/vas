/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.Invoice;

namespace Tymetrix.T360.Mobile.Client.AppWP8.Invoice
{
    public partial class AdjustInvoice : BasePage
    {
        private string initialValue = string.Empty;
        private const string InvoiceReasonsError = "Invoice Reasons Failed";
        private const string AdjustInvoiceError = "Invoice Adjust Failed";

        public List<ReasonCode> Reasons { get; set; }

        public AdjustInvoice()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationBar = new ApplicationBar();
            PrePopulate();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (PhoneApplicationService.Current.State.ContainsKey("isFromActivated") && PageInProgress)
            {
                PhoneApplicationService.Current.State.Remove("isFromActivated");
                PhoneApplicationPage_Loaded(null, null);
            }
            base.OnNavigatedTo(e);
        }

        private void PopulateHeaders()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                InvoiceBasicInfo invoiceHeaderDetails = (InvoiceBasicInfo)PhoneApplicationService.Current.State[SelectedInvoice];
                invoiceHeader.HeaderInfo = invoiceHeaderDetails;

                if (!invoiceHeaderDetails.Permissions.AdjustExpense)
                {
                    expenseButton.IsEnabled = false;
                }
                if (!invoiceHeaderDetails.Permissions.AdjustFee)
                {
                    feeButton.IsEnabled = false;
                }
                if (!invoiceHeaderDetails.Permissions.PositiveAdjustmentAllowed)
                {
                    plusButton.IsEnabled = false;
                }

                if (invoiceHeaderDetails.Permissions.AdjustExpense && !invoiceHeaderDetails.Permissions.AdjustFee)
                {
                    netAmountLabel.Text = FormatAmount(invoiceHeaderDetails.BilledExpenses);
                    expenseButton.Opacity = 1;
                    expenseButton.Tag = 1;
                }
                else if (!invoiceHeaderDetails.Permissions.AdjustExpense && invoiceHeaderDetails.Permissions.AdjustFee)
                {
                    netAmountLabel.Text = FormatAmount(invoiceHeaderDetails.BilledFees);
                    feeButton.Tag = 1;
                }
                else
                {
                    netAmountLabel.Text = FormatAmount(invoiceHeaderDetails.BilledFees);
                    feeButton.Tag = 1;
                }

            });
        }

        private void PrePopulate()
        {
            this.ProgressBar.Show();
            PageInProgress = true;
            InvoiceBasicInfo invoiceSummary = (InvoiceBasicInfo)PhoneApplicationService.Current.State[SelectedInvoice];
            currencyButton.Content = invoiceSummary.CurrencySymbol;
            PopulateHeaders();
            LoadReasons();
            ShowAdjustAppBar();
            this.ProgressBar.Hide();
            PageInProgress = false;
        }

        private void ShowAdjustAppBar()
        {
            RemoveApplicationBarButtons();
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = false;

            ApplicationBarIconButton submitButton = new ApplicationBarIconButton(new Uri("Resources\\images\\Approve.png", UriKind.Relative));
            submitButton.Text = "Submit";
            submitButton.Click += submitAppBar_Click;
            ApplicationBar.Buttons.Add(submitButton);

            ApplicationBarIconButton cancelButton = new ApplicationBarIconButton(new Uri("Resources\\images\\Reject.png", UriKind.Relative));
            cancelButton.Text = "Cancel";
            cancelButton.Click += cancelAppBar_Click;
            ApplicationBar.Buttons.Add(cancelButton);
        }

        private void LoadReasons()
        {
            PopulateReasons(Reasons);
        }

        private void LoadReasons(string action)
        {
            string postData = JsonConvert.SerializeObject(new ReasonCodeInputDetails() { Action = action });
            try
            {
                ServiceInvoker.InvokeServiceUsingPost("/api/t360/Invoice/GetReasonCodes", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
                {
                    ServiceResponse result = serviceEventArgs.Result;
                    if (result.Status)
                    {
                        Reasons = JsonConvert.DeserializeObject<List<ReasonCode>>(result.Output);
                        LoadReasons();
                    }
                    else
                    {
                        List<Error> resultError = result.ErrorDetails;
                        ShowError(new AppException(resultError), InvoiceReasonsError);
                        if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                RedirectToInvoiceList();
                            });
                        }
                    }
                    PageInProgress = false;
                });
            }
            catch (Exception ex)
            {
                ShowError((AppException)ex);
            }
        }

        private void PopulateReasons(List<ReasonCode> reasons)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                reasonListPicker.IsEnabled = false;
                this.reasonListPicker.ItemsSource = reasons;
                reasonListPicker.IsEnabled = true;
            });
        }

        private void CalculateAdjustmentAmount()
        {
            try
            {
                NumberFormatInfo numberFormat = NumberFormatInfo.CurrentInfo;
                string decimalSeparator = numberFormat.CurrencyDecimalSeparator;
                string groupSeparator = numberFormat.CurrencyGroupSeparator;

                InvoiceBasicInfo invoiceSummary = (InvoiceBasicInfo)PhoneApplicationService.Current.State[SelectedInvoice];

                string strNetAmount = (IsFeeSelected) ? invoiceSummary.BilledFees : invoiceSummary.BilledExpenses;

                strNetAmount = FormatAmount(strNetAmount);
                if (strNetAmount.Contains(invoiceSummary.CurrencySymbol))
                {
                    strNetAmount = strNetAmount.Replace(invoiceSummary.CurrencySymbol, "").Trim();
                }

                double netAmount = Convert.ToDouble(strNetAmount);
                netAmountLabel.Text = invoiceSummary.CurrencySymbol + strNetAmount;

                if (String.IsNullOrEmpty(adjustTextBox.Text.Trim()))
                {
                    return;
                }

                string adjustAmount = adjustTextBox.Text.Replace(groupSeparator, "");

                if (IsPercentageSelected)
                {
                    if (Convert.ToDouble(adjustAmount) > 100)
                    {
                        adjustTextBox.Text = initialValue;
                        adjustAmount = initialValue;
                    }
                }

                if (string.IsNullOrEmpty(initialValue)) initialValue = adjustAmount;

                if (adjustAmount.Contains(decimalSeparator))
                {
                    string[] tmpStrArr = adjustAmount.Split(decimalSeparator.ToCharArray());
                    if (tmpStrArr.Length < 2)
                    {
                        return;
                    }
                    if (!string.IsNullOrEmpty(tmpStrArr[1]) && tmpStrArr[1].Length > 2)
                    {
                        adjustAmount = String.Format("{0:n}", initialValue.Replace(groupSeparator, ""));
                    }
                    else if (tmpStrArr[0].Length > 12)
                    {
                        adjustAmount = initialValue;
                    }
                    else if (string.IsNullOrEmpty(tmpStrArr[0]))
                    {
                        adjustAmount = initialValue;
                    }

                    double amount = Convert.ToDouble(adjustAmount);
                    int decimalCount = tmpStrArr[1].Length > 2 ? 2 : tmpStrArr[1].Length;
                    adjustAmount = string.Format("{0:n" + decimalCount + "}", amount) + 
                                        (adjustAmount.EndsWith(decimalSeparator) ? decimalSeparator : string.Empty);

                    if (!adjustAmount.Contains(decimalSeparator))
                    {
                        adjustTextBox.Text = initialValue;
                    }
                    else
                    {
                        adjustTextBox.Text = adjustAmount;
                    }
                }
                else
                {
                    if (adjustAmount.Length > 12) adjustAmount = initialValue.Replace(groupSeparator, "");
                    double amount = Convert.ToDouble(adjustAmount);
                    adjustAmount = amount.ToString();
                    adjustTextBox.Text = String.Format("{0:n0}", amount);
                }
                adjustTextBox.Select(adjustTextBox.Text.Length, adjustTextBox.Text.Length);

                if ((bool)adjustToRadioButton.IsChecked) return;

                if (IsPercentageSelected)
                {
                    double percent = netAmount * Convert.ToDouble(adjustTextBox.Text.Trim()) / 100;
                    if (IsPlusSelected)
                    {
                        netAmount = netAmount + percent;
                    }
                    else
                    {
                        netAmount = netAmount - percent;
                    }
                }
                else
                {
                    if (IsPlusSelected)
                    {
                        netAmount = netAmount + Convert.ToDouble(adjustTextBox.Text.Trim());
                    }
                    else
                    {
                        netAmount = netAmount - Convert.ToDouble(adjustTextBox.Text.Trim());
                    }
                }
                netAmountLabel.Text = invoiceSummary.CurrencySymbol + String.Format("{0:n}", netAmount);
            }
            catch (Exception)
            {

            }
        }

        private string FormatAmount(string strNetAmount)
        {
            if (strNetAmount.StartsWith("("))
            {
                strNetAmount = strNetAmount.Substring(1, 1) + "-" + strNetAmount.Substring(2, strNetAmount.Length - 3);
            }
            return strNetAmount;
        }

        private void SwapOpacity(Button selectedButton, Button deselectButton)
        {
            selectedButton.Opacity = 1;
            deselectButton.Opacity = 0.5;
            selectedButton.Tag = true;
            deselectButton.Tag = false;
        }

        private void SwapBackGround(Button buttonToSelect, Button buttonToDeselect)
        {
            Brush brush = buttonToSelect.Background;
            buttonToSelect.Background = buttonToDeselect.Background;
            buttonToDeselect.Background = brush;
            buttonToSelect.Tag = !Convert.ToBoolean(buttonToSelect.Tag);
            buttonToDeselect.Tag = !Convert.ToBoolean(buttonToDeselect.Tag);
        }

        private void expenseButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsExpenseSelected) return;
            this.ProgressBar.Show();
            SwapOpacity(expenseButton, feeButton);
            LoadReasons("AdjustExpenses");
            CalculateAdjustmentAmount();
            this.ProgressBar.Hide();
        }

        private void feeButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsFeeSelected) return;
            this.ProgressBar.Show();
            SwapOpacity(feeButton, expenseButton);
            LoadReasons("AdjustFees");
            CalculateAdjustmentAmount();
            this.ProgressBar.Hide();
        }

        private void plusButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsPlusSelected) return;
            SwapBackGround(plusButton, minusButton);
            CalculateAdjustmentAmount();
        }

        private void minusButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsMinusSelected) return;
            SwapBackGround(minusButton, plusButton);
            CalculateAdjustmentAmount();
        }

        private void percentageButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsPercentageSelected) return;
            adjustTextBox.Text = string.Empty;
            SwapBackGround(percentageButton, currencyButton);
            CalculateAdjustmentAmount();
        }

        private void currencyButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsCurrencySelected) return;
            adjustTextBox.Text = string.Empty;
            SwapBackGround(currencyButton, percentageButton);
            CalculateAdjustmentAmount();
        }

        private AdjustInputDetails GetData()
        {
            InvoiceBasicInfo invoiceSummary = (InvoiceBasicInfo)PhoneApplicationService.Current.State[SelectedInvoice];

            string adjustmentType;
            if (IsPercentageSelected)
            {
                adjustmentType = "%";
            }
            else
            {
                adjustmentType = "$";
            }

            string adjustmentStyle;
            if (adjustByRadioButton.IsChecked == true)
            {
                adjustmentStyle = "Adjust By";
            }
            else
            {
                adjustmentStyle = "Adjust To";
            }

            string adjustmentMode;
            if (IsFeeSelected)
            {
                adjustmentMode = "Fees";
            }
            else
            {
                adjustmentMode = "Expense";
            }

            string strNetTotal = IsFeeSelected ? invoiceSummary.BilledFees : invoiceSummary.BilledExpenses;
            if (strNetTotal.StartsWith("("))
            {
                strNetTotal = strNetTotal.Substring(1, strNetTotal.Length - 2);
            }
            if (strNetTotal.StartsWith("$") || strNetTotal.StartsWith("€"))
            {
                strNetTotal = strNetTotal.Substring(1, strNetTotal.Length - 1);
            }

            ReasonCode reason;
            AdjustInputDetails adjustInputDetails;
            string adjustAmount = string.IsNullOrWhiteSpace(adjustTextBox.Text) ? "0" : adjustTextBox.Text;
            if (reasonListPicker.Items.Count == 0)
            {
                adjustInputDetails = new AdjustInputDetails()
                {
                    InvoiceId = invoiceSummary.InvoiceId,
                    ReasonId = string.Empty,
                    AdjustmentAmount = this.IsMinusSelected ? "-" + Convert.ToDouble(adjustAmount.Trim()).ToString(CultureInfo.InvariantCulture)
                                                            : Convert.ToDouble(adjustAmount.Trim()).ToString(CultureInfo.InvariantCulture),
                    AdjustmentMode = adjustmentMode,
                    AdjustmentStyle = adjustmentStyle,
                    AdjustmentType = adjustmentType,
                    NetTotal = strNetTotal,
                    NarrativeText = narrativeTextBox.Text.Trim()
                };
            }
            else
            {
                reason = reasonListPicker.SelectedItem as ReasonCode;
                adjustInputDetails = new AdjustInputDetails()
                {
                    InvoiceId = invoiceSummary.InvoiceId,
                    ReasonId = reason.Id.ToString(),
                    AdjustmentAmount = this.IsMinusSelected ? "-" + Convert.ToDouble(adjustAmount.Trim()).ToString(CultureInfo.InvariantCulture)
                                                            : Convert.ToDouble(adjustAmount.Trim()).ToString(CultureInfo.InvariantCulture),
                    AdjustmentMode = adjustmentMode,
                    AdjustmentStyle = adjustmentStyle,
                    AdjustmentType = adjustmentType,
                    NetTotal = strNetTotal,
                    NarrativeText = narrativeTextBox.Text.Trim()
                };
            }

            return adjustInputDetails;
        }

        private void AdjustSelectedInvoice(AdjustInputDetails adjustInputDetails)
        {
            BaseValidator validator = new BaseValidator();

            if (!validator.AdjustInvoice(adjustInputDetails))
            {
                ShowError(validator.ClientException, AdjustInvoiceError);
                return;
            }

            MessageBoxResult msgResult = MessageBox.Show("Are you sure you want to adjust this invoice?", "Adjustment Confirmation", MessageBoxButton.OKCancel);
            if (msgResult == MessageBoxResult.Cancel)
            {
                EnableApplicationBar();
                return;
            }

            string postData = JsonConvert.SerializeObject(adjustInputDetails);
            try
            {
                ServiceInvoker.InvokeServiceUsingPost("api/t360/Invoice/AdjustInvoice", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
                {
                    ServiceResponse result = serviceEventArgs.Result;
                    if (result.Status)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            EnableApplicationBar();
                            NavigationService.GoBack();
                        });
                    }
                    else
                    {
                        List<Error> resultError = result.ErrorDetails;
                        ShowError(new AppException(resultError), AdjustInvoiceError);
                        if (resultError[0] != null && T360ErrorCodes.NotInReviewerQueue == resultError[0].Code)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                RedirectToInvoiceList();
                            });
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                ShowError((AppException)ex);
            }
        }

        private void adjustTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            NumberFormatInfo numberFormat = NumberFormatInfo.CurrentInfo;
            string decimalSeparator = numberFormat.CurrencyDecimalSeparator;

            initialValue = adjustTextBox.Text;
            if (string.Empty.Equals(initialValue) && (e.Key == Key.D0 || e.Key == Key.NumPad0))
            {
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Decimal || e.Key == Key.Unknown || e.PlatformKeyCode == 190)
            {
                if (initialValue.Contains(decimalSeparator))
                {
                    e.Handled = true;
                }
            }
            if (!initialValue.Contains(decimalSeparator) && initialValue.Length >= 15 && e.Key != Key.Back)
            {
                if (e.Key != Key.Decimal && e.Key != Key.Unknown || e.PlatformKeyCode != 190)
                {
                    MessageBox.Show("Invalid adjustment amount value. Valid range is -999,999,999,999.99 to 999,999,999,999.99.");
                    e.Handled = true;
                }
            }
        }

        private void submitAppBar_Click(object sender, EventArgs e)
        {
            try
            {
                DisableApplicationBar();
                if (!ServiceInvoker.IsConnected)
                {
                    EnableApplicationBar();
                    throw new AppException(T360ErrorCodes.UnableToConnectServer);
                }
                this.ProgressBar.Show();
                this.AdjustSelectedInvoice(GetData());
                this.ProgressBar.Hide();
            }
            catch (Exception ex)
            {
                ShowError((AppException)ex);
            }
        }

        private void adjustTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            NumberFormatInfo numberFormat = NumberFormatInfo.CurrentInfo;
            string groupSeparator = numberFormat.CurrencyGroupSeparator;

            double num;
            if (double.TryParse(adjustTextBox.Text, out num) || (e.Key == Key.Back && string.IsNullOrEmpty(adjustTextBox.Text)))
            {
                CalculateAdjustmentAmount();
            }
            else
            {
                if (string.IsNullOrEmpty(initialValue))
                {
                    if (double.TryParse(adjustTextBox.Text.Replace(groupSeparator, ""), out num))
                    {
                        CalculateAdjustmentAmount();
                    }
                }
                adjustTextBox.Text = initialValue;
                adjustTextBox.Select(initialValue.Length, initialValue.Length);
            }
            initialValue = string.Empty;
        }

        private String ReverseString(String originalString)
        {

            int length = originalString.Length;
            string reverse = string.Empty;

            for (int i = (length - 1); i >= 0; i--)
            {
                reverse += originalString[i];
            }
            return reverse.ToString();
        }

        private bool IsFeeSelected
        {
            get
            {
                return Convert.ToBoolean(feeButton.Tag);
            }
        }

        private bool IsExpenseSelected
        {
            get
            {
                return Convert.ToBoolean(expenseButton.Tag);
            }
        }

        private bool IsMinusSelected
        {
            get
            {
                return Convert.ToBoolean(minusButton.Tag);
            }
        }

        private bool IsPlusSelected
        {
            get
            {
                return Convert.ToBoolean(plusButton.Tag);
            }
        }

        private bool IsCurrencySelected
        {
            get
            {
                return Convert.ToBoolean(currencyButton.Tag);
            }
        }

        private bool IsPercentageSelected
        {
            get
            {
                return Convert.ToBoolean(percentageButton.Tag);
            }
        }

        private void adjustByRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (feeButton != null && expenseButton != null) CalculateAdjustmentAmount();
        }

        private void adjustToRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (feeButton != null && expenseButton != null) CalculateAdjustmentAmount();
        }

        private void cancelAppBar_Click(object sender, EventArgs e)
        {
            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                return;
            }
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (PageInProgress)
            {
                e.Cancel = true;
                return;
            }

            if (!ServiceInvoker.IsConnected)
            {
                ShowError(new AppException(T360ErrorCodes.UnableToConnectServer));
                e.Cancel = true;
            }
            base.OnBackKeyPress(e);
        }
    }
}