/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using TyMetrix360.App.Common;
using TyMetrix360.App.Navigation;
using TyMetrix360.App.Validator;
using TyMetrix360.BusinessObjects.Common;
using TyMetrix360.BusinessObjects.Invoice;
using TyMetrix360.BusinessObjects.Services;
using TyMetrix360.Core;
using TyMetrix360.Core.Container;
using TyMetrix360.Core.Models;
using TyMetrix360.Core.ViewModelBase;
using TyMetrix360.Dto.Invoice;

namespace TyMetrix360.App.ViewModel
{
    public class AdjustmentViewModel : ViewModelCore, IAdjustmentViewModel
    {
        public AdjustmentViewModel()
        {
            PlusCommand = new RelayCommand(e => { Calculate(AdjustmentSign.Positive); });
            MinusCommand = new RelayCommand(e => { Calculate(AdjustmentSign.Negative); });
            DollarCommand = new RelayCommand(e => { Calculate(AdjustmentType.Straight); });
            PercentageCommand = new RelayCommand(e => { Calculate(AdjustmentType.Percentage); });

            OKCommand = new RelayCommand(e => { SaveAdjustment(); });
            CancelCommand = new RelayCommand(e => { ClearAdjustment(); });

            ByCommand = new RelayCommand(e => { OnByClick(); });
            ToCommand = new RelayCommand(e => { OnToClick(); });

            FeeCommand = new RelayCommand(e => { OnFeeClick(); });
            ExpenseCommand = new RelayCommand(e => { OnExpenseClick(); });

            AmountChangeCommand = new RelayCommand(e => { Calculate(); });
            GoToInvoiceSummaryCommand = new RelayCommand(e => { GotoInvoiceListPage(); });
        }
        private IRelayCommand _plusCommand;
        public IRelayCommand PlusCommand
        {
            get { return _plusCommand; }
            set { SetProperty(ref _plusCommand, value); }
        }
        private IRelayCommand _minusCommand;
        public IRelayCommand MinusCommand
        {
            get { return _minusCommand; }
            set { SetProperty(ref _minusCommand, value); }
        }
        private IRelayCommand _dollarCommand;
        public IRelayCommand DollarCommand
        {
            get { return _dollarCommand; }
            set { SetProperty(ref _dollarCommand, value); }
        }
        private IRelayCommand _percentageCommand;
        public IRelayCommand PercentageCommand
        {
            get { return _percentageCommand; }
            set { SetProperty(ref _percentageCommand, value); }
        }
        private IRelayCommand _okCommand;
        public IRelayCommand OKCommand
        {
            get { return _okCommand; }
            set { SetProperty(ref _okCommand, value); }
        }
        private IRelayCommand _cancelCommand;
        public IRelayCommand CancelCommand
        {
            get { return _cancelCommand; }
            set { SetProperty(ref _cancelCommand, value); }
        }
        private IRelayCommand _feeCommand;
        public IRelayCommand FeeCommand
        {
            get { return _feeCommand; }
            set { SetProperty(ref _feeCommand, value); }
        }
        private IRelayCommand _expenseCommand;
        public IRelayCommand ExpenseCommand
        {
            get { return _expenseCommand; }
            set { SetProperty(ref _expenseCommand, value); }
        }
        private IRelayCommand _byCommand;
        public IRelayCommand ByCommand
        {
            get { return _byCommand; }
            set { SetProperty(ref _byCommand, value); }
        }
        private IRelayCommand _toCommand;
        public IRelayCommand ToCommand
        {
            get { return _toCommand; }
            set { SetProperty(ref _toCommand, value); }
        }
        private IRelayCommand _goToInvoiceSummaryCommand;
        public IRelayCommand GoToInvoiceSummaryCommand
        {
            get { return _goToInvoiceSummaryCommand; }
            set { SetProperty(ref _goToInvoiceSummaryCommand, value); }
        }
        private IRelayCommand _amountChangeCommand;
        public IRelayCommand AmountChangeCommand
        {
            get { return _amountChangeCommand; }
            set { SetProperty(ref _amountChangeCommand, value); }
        }
        private ObservableCollection<InvoiceSummary> _adjustItems;
        public ObservableCollection<InvoiceSummary> AdjustItems
        {
            get { return _adjustItems; }
            set { SetProperty(ref _adjustItems, value); }
        }
        private InvoiceSummary _invoiceSummary;
        public InvoiceSummary InvoiceSummary
        {
            get { return _invoiceSummary; }
            set { SetProperty(ref _invoiceSummary, value); }
        }
        private ObservableCollection<ReasonCode> _reasonCodes;
        public ObservableCollection<ReasonCode> ReasonCodes
        {
            get { return _reasonCodes; }
            set { SetProperty(ref _reasonCodes, value); }
        }
        private string _netAmount;
        public string NetAmount
        {
            get { return _netAmount; }
            set { SetProperty(ref _netAmount, value); }
        }
        private double _adjustedAmount;
        public double AdjustedAmount
        {
            get { return _adjustedAmount; }
            set
            {
                SetProperty(ref _adjustedAmount, value);
                AdjustedAmountStr = CalculateAdjustmentAmount(value);
            }
        }
        private string _adjustedAmountStr;
        public string AdjustedAmountStr
        {
            get { return _adjustedAmountStr; }
            set
            {
                if ("0".Equals(value))
                {
                    value = string.Empty;
                }
                SetProperty(ref _adjustedAmountStr, value);
            }
        }
        private AdjustmentType _adjustmentType;
        public AdjustmentType AdjustmentType
        {
            get { return _adjustmentType; }
            set
            {
                SetProperty(ref _adjustmentType, value);
                IsPercentage = (AdjustmentType.Percentage == value);
                IsDollar = (AdjustmentType.Straight == value);
            }
        }
        private AdjustmentToggleType _adjustmentToggleType;
        public AdjustmentToggleType AdjustmentToggleType
        {
            get { return _adjustmentToggleType; }
            set
            {
                SetProperty(ref _adjustmentToggleType, value);
                IsExpense = (AdjustmentToggleType.Expenses == value);
                IsFee = (AdjustmentToggleType.Fees == value);
            }
        }
        private AdjustmentSign _adjustmentSign;
        public AdjustmentSign AdjustmentSign
        {
            get { return _adjustmentSign; }
            set
            {
                SetProperty(ref _adjustmentSign, value);
                IsPlus = (AdjustmentSign.Positive == value);
                IsMinus = (AdjustmentSign.Negative == value);
            }
        }
        private AdjustmentByTo _adjustmentByTo;
        public AdjustmentByTo AdjustmentByTo
        {
            get { return _adjustmentByTo; }
            set
            {
                SetProperty(ref _adjustmentByTo, value);
                IsTo = (AdjustmentByTo.AdjustTo == value);
                IsBy = (AdjustmentByTo.AdjustBy == value);
            }
        }
        private InvoiceAdjustment _adjustmentInfo;
        public InvoiceAdjustment AdjustmentInfo
        {
            get { return _adjustmentInfo; }
            set { SetProperty(ref _adjustmentInfo, value); }
        }
        private int _selectedReasonId;
        public int SelectedReasonId
        {
            get { return _selectedReasonId; }
            set { SetProperty(ref _selectedReasonId, value); }
        }
        private string _narrativeText;
        public string NarrativeText
        {
            get { return _narrativeText; }
            set { SetProperty(ref _narrativeText, value); }
        }
        private int _selectedInvoice;
        public int SelectedInvoice
        {
            get { return _selectedInvoice; }
            set { SetProperty(ref _selectedInvoice, value); }
        }
        private bool _isFee;
        public bool IsFee
        {
            get { return _isFee; }
            set { SetProperty(ref _isFee, value); }
        }
        private bool _isExpenseEnabled;
        public bool IsExpenseEnabled
        {
            get { return _isExpenseEnabled; }
            set { SetProperty(ref _isExpenseEnabled, value); }
        }
        private bool _isFeeEnabled;
        public bool IsFeeEnabled
        {
            get { return _isFeeEnabled; }
            set { SetProperty(ref _isFeeEnabled, value); }
        }
        private bool _isExpense;
        public bool IsExpense
        {
            get { return _isExpense; }
            set { SetProperty(ref _isExpense, value); }
        }
        private bool _isBy;
        public bool IsBy
        {
            get { return _isBy; }
            set { SetProperty(ref _isBy, value); }
        }
        private bool _isTo;
        public bool IsTo
        {
            get { return _isTo; }
            set { SetProperty(ref _isTo, value); }
        }
        private bool _isPlus;
        public bool IsPlus
        {
            get { return _isPlus; }
            set { SetProperty(ref _isPlus, value); }
        }
        private bool _isPositiveEnabled;
        public bool IsPositiveEnabled
        {
            get { return _isPositiveEnabled; }
            set { SetProperty(ref _isPositiveEnabled, value); }
        }
        private bool _isMinus;
        public bool IsMinus
        {
            get { return _isMinus; }
            set { SetProperty(ref _isMinus, value); }
        }
        private bool _isDollar;
        public bool IsDollar
        {
            get { return _isDollar; }
            set { SetProperty(ref _isDollar, value); }
        }
        private bool _isPercentage;
        public bool IsPercentage
        {
            get { return _isPercentage; }
            set { SetProperty(ref _isPercentage, value); }
        }
        private string _currencyType;
        public string CurrencyType
        {
            get { return _currencyType; }
            set { SetProperty(ref _currencyType, value); }
        }
        private string GroupSeparator
        {
            get { return NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator; }
        }
        private string DecimalSeparator
        {
            get { return NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator; }
        }
        private string CurrencySymbol
        {
            get { return NumberFormatInfo.CurrentInfo.CurrencySymbol; }
        }

        public void Calculate(object e)
        {
            if (e is AdjustmentSign) AdjustmentSign = (AdjustmentSign)e;
            else AdjustmentType = (AdjustmentType)e;
            Calculate();
        }

        public void GotoInvoiceListPage()
        {
            try
            {
                if (!ServiceInvoker.Instance.IsNetworkConnected) throw new T360Exception(T360ErrorCodes.NetworkConnectionFailed);

                Navigator.Navigate(Destination.InvoiceListView, ExistingViewBehavior.Remove, new object[] { InvoiceSummary });
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.AdjustmentFailed);
            }
        }

        public void Calculate()
        {
            try
            {
                if (InvoiceSummary.BilledFees == null || InvoiceSummary.BilledExpenses == null) return;

                string adjustAmount = AdjustedAmount.ToString();

                WillAdjustNegative();

                double billedAmount = AdjustmentToggleType == AdjustmentToggleType.Fees
                              ? (double.Parse(CleanNetTotal(RemoveCurrencySymbol(InvoiceSummary.BilledFees, true), true), CultureInfo.CurrentCulture))
                              : (double.Parse(CleanNetTotal(RemoveCurrencySymbol(InvoiceSummary.BilledExpenses, true), true), CultureInfo.CurrentCulture));

                if (AdjustmentType == AdjustmentType.Straight)
                {
                    if (AdjustmentByTo == AdjustmentByTo.AdjustBy)
                    {
                        if (AdjustmentSign == AdjustmentSign.Positive)
                        {
                            NetAmount = (billedAmount + AdjustedAmount).ToString();
                        }
                        else
                        {
                            NetAmount = (billedAmount - AdjustedAmount).ToString();
                        }
                    }
                    else
                    {
                        NetAmount = (billedAmount.ToString().Replace(GroupSeparator, "")).ToString();
                    }
                }
                //Percentage
                else
                {
                    CalculatePercentageAmount(GroupSeparator, billedAmount);
                }
                if ("0".Equals(NetAmount))
                {
                    NetAmount = InvoiceSummary.CurrencySymbol + " " + NetAmount;
                }
                else
                {
                    NetAmount = InvoiceSummary.CurrencySymbol + 
                                CleanNetTotal(RemoveCurrencySymbol(String.Format("{0:c}", Convert.ToDecimal(NetAmount)), false), true);
                }
            }
            catch (Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.AdjustmentFailed);
            }
        }

        public async void SaveAdjustment()
        {
            try
            {
                T360Validator.ValidateAdjustInvoice(AdjustedAmount.ToString(), SelectedReasonId);

                var isOk = await ShowConfirmationMessage(Constants.AdjustConfirmationMsg, Constants.AdjustmentConfirmation);
                if (!isOk) return;

                IsBusy = true;
                AdjustmentInfo = new InvoiceAdjustment();
                AdjustmentInfo.NarrativeText = NarrativeText;
                AdjustmentInfo.AdjustmentStyle = AdjustmentByTo;
                AdjustmentInfo.InvoiceId = this.InvoiceSummary.InvoiceId.ToString();
                AdjustmentInfo.AdjustmentAmount = AdjustmentSign == AdjustmentSign.Positive
                                                ? Convert.ToDouble(AdjustedAmount).ToString(CultureInfo.InvariantCulture)
                                                : "-" + Convert.ToDouble(AdjustedAmount).ToString(CultureInfo.InvariantCulture);
                AdjustmentInfo.ReasonId = SelectedReasonId;
                AdjustmentInfo.NetTotal = CleanNetTotal(NetAmount, true);
                AdjustmentInfo.AdjustmentMode = AdjustmentToggleType;
                AdjustmentInfo.AdjustmentType = AdjustmentType;

                var serializableData = GetAdjustmentSerializationData(AdjustmentInfo);
                var data = await ServiceInvoker.Instance.InvokeServiceUsingPost<string>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.AdjustInvoiceService), serializableData, true, false);
                IsBusy = false;
                Messenger.Default.Send<ResetListParameter>(new ResetListParameter() { });
                Navigator.Navigate(Destination.InvoiceListView, ExistingViewBehavior.Remove, new object[] { });
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.AdjustmentFailed);
            }
        }

        private object GetAdjustmentSerializationData(InvoiceAdjustment param)
        {
            return new
            {
                InvoiceId = !string.IsNullOrEmpty(param.InvoiceId) ? param.InvoiceId : string.Empty,
                ReasonId = param.ReasonId,
                NarrativeText = !string.IsNullOrEmpty(param.NarrativeText) ? param.NarrativeText : string.Empty,
                AdjustmentMode = !string.IsNullOrEmpty(param.AdjustmentModeText) ? param.AdjustmentModeText : string.Empty,
                AdjustmentAmount = !string.IsNullOrEmpty(param.AdjustmentAmount) ? param.AdjustmentAmount : string.Empty,
                AdjustmentType = !string.IsNullOrEmpty(param.AdjustmentTypeText) ? param.AdjustmentTypeText : string.Empty,
                AdjustmentStyle = !string.IsNullOrEmpty(param.AdjustmentStyleText) ? param.AdjustmentStyleText : string.Empty,
                NetTotal = !string.IsNullOrEmpty(param.NetTotal) ? param.NetTotal : string.Empty
            };

        }

        private void ClearAdjustment()
        {
            SetDefaults();
            SelectedReasonId = 0;
            Messenger.Default.Send<string>(string.Empty, Constants.AdjustDefaultSettings);
        }

        private void OnByClick()
        {
            AdjustmentByTo = AdjustmentByTo.AdjustBy;
        }

        private void OnToClick()
        {
            AdjustmentByTo = AdjustmentByTo.AdjustTo;
        }

        private async void OnFeeClick()
        {
            AdjustmentToggleType = AdjustmentToggleType.Fees;
            await SetReasonCodes();
        }

        private async void OnExpenseClick()
        {
            AdjustmentToggleType = AdjustmentToggleType.Expenses;
            await SetReasonCodes();
        }

        public override async Task LoadData(params object[] parameters)
        {
            try
            {
                InvoiceSummary = parameters[0] as InvoiceSummary;

                var items = new ObservableCollection<InvoiceSummary>();
                items.Add(InvoiceSummary);
                AdjustItems = items;
                SelectedInvoice = 0;

                IsBusy = true;
                ManagePermissions();
                SetDefaults();
                await SetReasonCodes();
                IsBusy = false;
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.AdjustmentFailed);
            }
        }

        private void ManagePermissions()
        {
            IsExpenseEnabled = InvoiceSummary.Permissions.AdjustExpense;
            IsFeeEnabled = InvoiceSummary.Permissions.AdjustFee;
            IsPositiveEnabled = InvoiceSummary.Permissions.PositiveAdjustmentAllowed;
        }

        public async Task SetReasonCodes()
        {
            try
            {
                bool isExpense = (this.AdjustmentToggleType == AdjustmentToggleType.Expenses);
                string reasonType = isExpense ? Constants.AdjustExpenses : Constants.AdjustFees;

                var serializableData = new { Action = reasonType };
                List<ReasonCode> adjustReasons = await ServiceInvoker.Instance.InvokeServiceUsingPost<List<ReasonCode>>(ServiceInvoker.Instance.AppendUrl(ServiceInvoker.GetReasonCodesService), serializableData, false, false);
                this.ReasonCodes = new ObservableCollection<ReasonCode>(adjustReasons);

                SelectedReasonId = 0;
            }
            catch (T360Exception ex)
            {
                string message = getMessages(ex);
                ShowErrorMessage(message, Constants.ReasonsFailed);
                if (T360ErrorCodes.NotInReviewerQueue.Equals(ex.ErrorCodes[0].Code))
                {
                    Navigator.Navigate(Destination.InvoiceListView);
                }
            }
        }

        private List<ReasonCode> GetReasons(ReasonCodeDto[] reasons)
        {
            List<ReasonCode> reasonList = new List<ReasonCode>();
            reasonList.AddRange(reasons.Select(reason =>
                new ReasonCode()
                {
                    Id = reason.Id,
                    Description = reason.Description
                }));
            return reasonList;
        }

        private void SetDefaults()
        {
            AdjustmentToggleType = IsFeeEnabled ? AdjustmentToggleType.Fees : AdjustmentToggleType.Expenses;
            AdjustmentByTo = AdjustmentByTo.AdjustBy;
            AdjustmentSign = AdjustmentSign.Negative;
            AdjustmentType = AdjustmentType.Straight;
            NarrativeText = string.Empty;
            AdjustedAmount = 0.0;
            CurrencyType = InvoiceSummary.CurrencySymbol;
            NetAmount = AdjustmentToggleType.Fees == AdjustmentToggleType ? InvoiceSummary.BilledFees : InvoiceSummary.BilledExpenses;
        }

        private string CleanNetTotal(string total, bool allowNegative)
        {
            var acceptedIntoDecimal = "0123456789" + DecimalSeparator + GroupSeparator + (allowNegative ? "-" : "");
            StringBuilder sb = new StringBuilder();
            foreach (char c in total)
            {
                if (acceptedIntoDecimal.Contains(c.ToString()))
                {
                    sb.Append(c);
                }
            }
            if (sb.Length == 0)
            {
                sb.Append('0');
            }
            return sb.ToString();
        }

        private void WillAdjustNegative()
        {
            if (AdjustedAmount != 0)
            {
                if ((AdjustmentSign == AdjustmentSign.Positive && AdjustedAmount < 0) || (AdjustmentSign == AdjustmentSign.Negative && AdjustedAmount > 0))
                {
                    if (AdjustmentByTo == AdjustmentByTo.AdjustBy)
                    {
                        if (AdjustmentToggleType == AdjustmentToggleType.Fees)
                        {
                            NetAmount = (double.Parse(RemoveCurrencySymbol(InvoiceSummary.BilledFees, true)) -
                                ((double.Parse(RemoveCurrencySymbol(InvoiceSummary.NetTotal, true)) *
                                (AdjustedAmount / 100)))).ToString();
                        }
                        else
                        {
                            NetAmount = (double.Parse(RemoveCurrencySymbol(InvoiceSummary.BilledExpenses, true)) - ((double.Parse(RemoveCurrencySymbol(InvoiceSummary.NetTotal, true)) * (AdjustedAmount / 100)))).ToString();
                        }
                    }
                }
            }
        }

        private string CalculateAdjustmentAmount(double aamount)
        {
            string adjustAmount = aamount.ToString();
            string initialValue = adjustAmount;

            if (adjustAmount.Contains(DecimalSeparator))
            {
                string[] tmpStrArr = adjustAmount.Split(DecimalSeparator.ToCharArray());
                if (tmpStrArr.Length < 2)
                {
                    return adjustAmount;
                }
                if (!string.IsNullOrEmpty(tmpStrArr[1]) && tmpStrArr[1].Length > 2)
                {
                    adjustAmount = String.Format("{0:c}", initialValue.Replace(GroupSeparator, ""));
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
                adjustAmount = string.Format("{0:c" + decimalCount + "}", amount) +
                                    (adjustAmount.EndsWith(DecimalSeparator) ? DecimalSeparator : string.Empty);

                adjustAmount = RemoveCurrencySymbol(adjustAmount, false);

                if (!adjustAmount.Contains(DecimalSeparator))
                {
                    return initialValue;
                }
                else
                {
                    return adjustAmount;
                }
            }
            else
            {
                if (adjustAmount.Length > 12) adjustAmount = initialValue.Replace(GroupSeparator, string.Empty);
                double amount = Convert.ToDouble(adjustAmount);
                adjustAmount = amount.ToString();
                return CleanNetTotal(RemoveCurrencySymbol(String.Format("{0:c0}", amount), false), false);
            }
        }

        public string RemoveCurrencySymbol(string amountToRemoveCurrency, bool removeGroupSeparator)
        {
            string amount = removeGroupSeparator
                          ? amountToRemoveCurrency.Replace(InvoiceSummary.CurrencySymbol, "").Replace(CurrencySymbol, "").Replace(GroupSeparator, "").Trim()
                          : amountToRemoveCurrency.Replace(InvoiceSummary.CurrencySymbol, "").Replace(CurrencySymbol, "").Trim();
            return FormatAmount(amount);
        }

        private string FormatAmount(string strNetAmount)
        {
            if (strNetAmount.StartsWith("("))
            {
                strNetAmount = "-" + strNetAmount.Substring(1, strNetAmount.Length - 2);
            }
            return strNetAmount;
        }

        private void CalculatePercentageAmount(string groupSeparator, double billedAmount)
        {
            if (AdjustmentByTo == AdjustmentByTo.AdjustBy)
            {
                double percent = (billedAmount * Convert.ToDouble(AdjustedAmount) / 100);
                if (AdjustmentSign == AdjustmentSign.Positive)
                {
                    NetAmount = (billedAmount + percent).ToString();
                }
                else
                {
                    NetAmount = (billedAmount - percent).ToString();
                }
            }
            else
            {
                NetAmount = (billedAmount.ToString().Replace(groupSeparator, "")).ToString();
            }
        }

        public override bool ShowAppBar
        {
            get { return false; }
        }
    }
}
