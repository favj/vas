/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

using Tymetrix.T360.Mobile.Client.Common.Base.Util;
using Tymetrix.T360.Mobile.Client.Common.Base.View;
using Tymetrix.T360.Mobile.Client.Core;
using Tymetrix.T360.Mobile.Client.Model.Base;
using Tymetrix.T360.Mobile.Client.Model.Invoice;
using Microsoft.Phone.Controls;

namespace Tymetrix.T360.Mobile.Client.AppWP8.Invoice
{
    public partial class AdjustPage : BasePage
    {
        private string initialValue = string.Empty;
        private const string InvoiceReasonsError = "Invoice Reasons Failed";
        private const string AdjustInvoiceError = "Invoice Adjust Failed";
        private const string AdjustLineItemError = "Line Item Adjust Failed";
        private string CurrencySymbol { get; set; }
        public LineItemConfirmationDetails LineItemAdjustDetails { get; set; }
        public InvoiceSummary InvoiceDetails { get; set; }
        public List<ReasonCode> Reasons { get; set; }
        public string LineItemNetAmount { get; set; }

        private readonly Thickness LineItemListWithKeyboardOpen = new Thickness(0, 0, 0, 280);
        private readonly Thickness InvoiceListWithKeyboardOpen = new Thickness(0, 0, 0, 220);
        private readonly Thickness ListWithKeyboardClose = new Thickness(0, 0, 0, 0);

        public AdjustPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Source == Source.EXTERNAL)
            {
                this.Source = this.PreviousSource;
                return;
            }
            ApplicationBar = new ApplicationBar();
            PrePopulate();
            PageInProgress = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if ((bool)PhoneApplicationService.Current.State.ContainsKey("isTombStoned"))
            {
                base.OnNavigatedTo(e);
                return;
            }
            if (this.Source == Source.EXTERNAL)
            {
                base.OnNavigatedTo(e);
                return;
            }
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (Constants.ExternalURI.Equals(e.Uri.ToString()))
            {
                this.PreviousSource = this.Source;
                this.Source = Source.EXTERNAL;
                return;
            }
            InvoiceCommonDetails commonDetailsPage = e.Content as InvoiceCommonDetails;
            if (commonDetailsPage != null)
            {
                if (Source == Model.Base.Source.INVOICE_SINGLE_ADJUST)
                {
                    commonDetailsPage.Source = Model.Base.Source.BACK_TO_INVOICE_SUMMARY;
                    commonDetailsPage.InvoiceSummaryDetails = InvoiceDetails;
                }
                else if (Source == Model.Base.Source.LINE_ITEM_MULTI_ADJUST_CONFIRMATION)
                {
                    commonDetailsPage.Source = Model.Base.Source.BACK_TO_LINE_ITEM_LIST;
                    commonDetailsPage.MultiConfirmation = LineItemAdjustDetails;
                }
            }
            LineItemSummary lineItemSummary = e.Content as LineItemSummary;
            if (lineItemSummary != null)
            {
                lineItemSummary.Source = Source.BACK_TO_LINE_ITEM_SUMMARY;
                lineItemSummary.SelectedLineItemId = LineItemAdjustDetails.SelectedLineItemIds[0];
            }
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            base.OnOrientationChanged(e);
            SetListMargin();
        }

        private void SetListMargin()
        {
            if (this.CurrentOrientation == PageOrientation.LandscapeRight ||
               this.CurrentOrientation == PageOrientation.LandscapeLeft ||
               this.CurrentOrientation == PageOrientation.Landscape)
                ContentPanel.Margin = ListWithKeyboardClose;
            else
            {
                if (Source == Model.Base.Source.LINE_ITEM_MULTI_ADJUST_CONFIRMATION || Source == Model.Base.Source.LINE_ITEM_SINGLE_ADJUST)
                {
                     ContentPanel.Margin = adjustTextBox.Focus() ? LineItemListWithKeyboardOpen : ListWithKeyboardClose;
                }
                else
                {
                     ContentPanel.Margin = adjustTextBox.Focus() ? InvoiceListWithKeyboardOpen : ListWithKeyboardClose;
                }
            }
        }

        private void PopulateHeaders()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                invoiceHeader.SetHeaderDetails(InvoiceDetails);

                if (!InvoiceDetails.Permissions.AdjustExpense)
                {
                    expenseButton.IsEnabled = false;
                }
                if (!InvoiceDetails.Permissions.AdjustFee)
                {
                    feeButton.IsEnabled = false;
                }
                if (!InvoiceDetails.Permissions.PositiveAdjustmentAllowed)
                {
                    plusButton.IsEnabled = false;
                }

                if (InvoiceDetails.Permissions.AdjustExpense && !InvoiceDetails.Permissions.AdjustFee)
                {
                    netAmountLabel.Text = TrimAmount(InvoiceDetails.BilledExpenses);
                    expenseButton.Opacity = 1;
                    expenseButton.Tag = 1;
                }
                else if (!InvoiceDetails.Permissions.AdjustExpense && InvoiceDetails.Permissions.AdjustFee)
                {
                    netAmountLabel.Text = TrimAmount(InvoiceDetails.BilledFees);
                    feeButton.Tag = 1;
                }
                else
                {
                    netAmountLabel.Text = TrimAmount(InvoiceDetails.BilledFees);
                    feeButton.Tag = 1;
                }

            });
        }

        private string TrimAmount(string amount)
        {
            NumberFormatInfo numberFormat = NumberFormatInfo.CurrentInfo;
            string decimalSeparator = numberFormat.CurrencyDecimalSeparator;

            string trimmedVal = FormatAmount(amount).Replace(CurrencySymbol, "").Trim();
            string negativeSymbol = "-";
            string[] vals = trimmedVal.Split(new string[] { decimalSeparator }, StringSplitOptions.None);
            if (vals.Length > 1)
            {
                string decimalVal = (vals[1].Length > 2) ? vals[1].Substring(0, 2) : vals[1];
                trimmedVal = vals[0] + decimalSeparator + decimalVal;
            }
            if (trimmedVal.StartsWith(negativeSymbol))
            {
                trimmedVal = trimmedVal.Replace(negativeSymbol, string.Empty).Trim();
                trimmedVal = negativeSymbol + trimmedVal;
            }
            return CurrencySymbol + trimmedVal;
        }

        private void PopulateLineItemHeaders()
        {
            invoiceHeader.SetHeaderDetails(LineItemAdjustDetails.InvoiceDetails);
            feeButton.Visibility = System.Windows.Visibility.Collapsed;
            expenseButton.Visibility = System.Windows.Visibility.Collapsed;
            currencyButton.Content = CurrencySymbol = LineItemAdjustDetails.CurrencySymbol;
            plusButton.IsEnabled = LineItemAdjustDetails.PositiveAdjustment;
            netAmountLabel.Text = TrimAmount(LineItemAdjustDetails.NetAmount);
        }

        private void PrePopulate()
        {
            if (Source == Model.Base.Source.LINE_ITEM_MULTI_ADJUST_CONFIRMATION || Source == Model.Base.Source.LINE_ITEM_SINGLE_ADJUST)
            {
                PopulateLineItemHeaders();
                LoadReasons();
                ShowAdjustAppBar();
                PageInProgress = false;
            }
            else
            {
                this.ProgressBar.Show();
                PageInProgress = true;
                currencyButton.Content = CurrencySymbol = InvoiceDetails.CurrencySymbol;
                PopulateHeaders();
                LoadReasons();
                ShowAdjustAppBar();
                this.ProgressBar.Hide();
                PageInProgress = false;
            }
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

                string strNetAmount;
                int adjustItemsCount;
                if (Source == Model.Base.Source.LINE_ITEM_MULTI_ADJUST_CONFIRMATION || Source == Model.Base.Source.LINE_ITEM_SINGLE_ADJUST)
                {
                    strNetAmount = LineItemAdjustDetails.NetAmount;
                    adjustItemsCount = LineItemAdjustDetails.SelectedLineItemIds.Count;
                }
                else
                {
                    CurrencySymbol = InvoiceDetails.CurrencySymbol;
                    strNetAmount = (IsFeeSelected) ? InvoiceDetails.BilledFees : InvoiceDetails.BilledExpenses;
                    adjustItemsCount = 1;
                }

                strNetAmount = FormatAmount(strNetAmount);
                if (strNetAmount.Contains(CurrencySymbol))
                {
                    strNetAmount = strNetAmount.Replace(CurrencySymbol, "").Replace(" ", "").Trim();
                }

                double netAmount = Convert.ToDouble(strNetAmount);
                netAmountLabel.Text = CurrencySymbol + strNetAmount;

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
                        netAmount = netAmount + (adjustItemsCount * Convert.ToDouble(adjustTextBox.Text.Trim()));
                    }
                    else
                    {
                        netAmount = netAmount - (adjustItemsCount * Convert.ToDouble(adjustTextBox.Text.Trim()));
                    }
                }
                netAmountLabel.Text = CurrencySymbol + String.Format("{0:n}", netAmount);
            }
            catch (Exception)
            {

            }
        }

        private string FormatAmount(string strNetAmount)
        {
            if (strNetAmount != null && strNetAmount.StartsWith("("))
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
            string strNetTotal;
            string InvoiceId;
            if (Source == Model.Base.Source.LINE_ITEM_MULTI_ADJUST_CONFIRMATION || Source == Model.Base.Source.LINE_ITEM_SINGLE_ADJUST)
            {
                strNetTotal = LineItemAdjustDetails.NetAmount;
                adjustmentMode = string.Empty;
                InvoiceId = LineItemAdjustDetails.InvoiceDetails.InvoiceId.ToString();
            }
            else
            {
                if (IsFeeSelected)
                {
                    adjustmentMode = "Fees";
                }
                else
                {
                    adjustmentMode = "Expense";
                }

                strNetTotal = IsFeeSelected ? InvoiceDetails.BilledFees : InvoiceDetails.BilledExpenses;
                InvoiceId = InvoiceDetails.InvoiceId.ToString();
            }

            if (strNetTotal.StartsWith("("))
            {
                strNetTotal = strNetTotal.Substring(1, strNetTotal.Length - 2);
            }
            if (strNetTotal.StartsWith("$") || strNetTotal.StartsWith("€"))
            {
                strNetTotal = strNetTotal.Substring(1, strNetTotal.Length - 1);
            }

            ReasonCode reason;
            string adjustAmount = string.IsNullOrWhiteSpace(adjustTextBox.Text) ? "0" : adjustTextBox.Text;
            AdjustInputDetails adjustInputDetails = new AdjustInputDetails()
            {
                InvoiceId = InvoiceId,
                AdjustmentAmount = this.IsMinusSelected ? "-" + Convert.ToDouble(adjustAmount.Trim()).ToString(CultureInfo.InvariantCulture)
                                                        : Convert.ToDouble(adjustAmount.Trim()).ToString(CultureInfo.InvariantCulture),
                AdjustmentMode = adjustmentMode,
                AdjustmentStyle = adjustmentStyle,
                AdjustmentType = adjustmentType,
                NetTotal = strNetTotal,
                NarrativeText = narrativeTextBox.Text.Trim()
            };
            adjustInputDetails.LineItemIds = Source == Model.Base.Source.LINE_ITEM_MULTI_ADJUST_CONFIRMATION || 
                                                Source == Model.Base.Source.LINE_ITEM_SINGLE_ADJUST
                                           ? LineItemAdjustDetails.SelectedLineItemIds
                                           : null;
            if (reasonListPicker.Items.Count == 0)
            {
                adjustInputDetails.ReasonId = string.Empty;
            }
            else
            {
                reason = reasonListPicker.SelectedItem as ReasonCode;
                adjustInputDetails.ReasonId = reason.Id.ToString();
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

            PageInProgress = true;
            this.ProgressBar.Show();
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
                            this.ProgressBar.Hide();
                            PageInProgress = false;
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

        private void AdjustSelectedLineItem(AdjustInputDetails adjustInputDetails)
        {
            BaseValidator validator = new BaseValidator();

            if (!validator.AdjustInvoice(adjustInputDetails))
            {
                ShowError(validator.ClientException, AdjustLineItemError);
                return;
            }
            MessageBoxResult msgResult;
            if (LineItemAdjustDetails.SelectedLineItemIds.Count == 1)
            {
                msgResult = MessageBox.Show("Are you sure you want to adjust this line item?", "Adjustment Confirmation", MessageBoxButton.OKCancel);
            }
            else
            {
                msgResult = MessageBox.Show("Are you sure you want to adjust these line items?", "Adjustment Confirmation", MessageBoxButton.OKCancel);
            }

            if (msgResult == MessageBoxResult.Cancel)
            {
                EnableApplicationBar();
                return;
            }

            PageInProgress = true;
            this.ProgressBar.Show();

            string postData = JsonConvert.SerializeObject(adjustInputDetails);
            try
            {
                ServiceInvoker.InvokeServiceUsingPost("api/t360/LineItem/AdjustLineItems", postData, false, delegate(object a, ServiceEventArgs serviceEventArgs)
                {
                    ServiceResponse result = serviceEventArgs.Result;
                    if (result.Status)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            EnableApplicationBar();
                            this.ProgressBar.Hide();
                            PageInProgress = false;
                            NavigationService.GoBack();
                        });
                    }
                    else
                    {
                        List<Error> resultError = result.ErrorDetails;
                        ShowError(new AppException(resultError), AdjustLineItemError);
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

            if (IsPercentageSelected)
            {
                double temp;
                if (double.TryParse(adjustTextBox.Text, out temp))
                {
                    double adjustAmount = Convert.ToDouble(adjustTextBox.Text);
                    if (adjustAmount == 100)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void submitAppBar_Click(object sender, EventArgs e)
        {
            DisableApplicationBar();
            try
            {
                if (!ServiceInvoker.IsConnected)
                {
                    EnableApplicationBar();
                    throw new AppException(T360ErrorCodes.UnableToConnectServer);
                }
                if (Source == Model.Base.Source.LINE_ITEM_MULTI_ADJUST_CONFIRMATION || Source == Model.Base.Source.LINE_ITEM_SINGLE_ADJUST)
                {
                    AdjustSelectedLineItem(GetData());
                }
                else
                {
                    this.AdjustSelectedInvoice(GetData());
                }
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
                return;
            }
            base.OnBackKeyPress(e);
        }

        private void adjustTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SetListMargin();
        }

        private void adjustTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ContentPanel.Margin = ListWithKeyboardClose;
        }
    }
}