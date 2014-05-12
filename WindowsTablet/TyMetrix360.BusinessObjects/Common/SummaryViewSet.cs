/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */

using Windows.UI.Xaml;

using TyMetrix360.Core.Interfaces;

namespace TyMetrix360.BusinessObjects.Common
{
    public class SummaryViewSet : BusinessObjectCore, ISupportRowIndex
    {
        private int _index;
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }
        private GridLength _columnWidth = new GridLength(250, GridUnitType.Pixel);
        public GridLength ColumnWidth
        {
            get { return _columnWidth; }
            set { SetProperty(ref _columnWidth, value); }
        }
        private string _key0;
        public string Key0
        {
            get { return _key0; }
            set { SetProperty(ref _key0, value); }
        }
        private string _key;
        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }
        private string _key1;
        public string Key1
        {
            get { return _key1; }
            set { SetProperty(ref _key1, value); }
        }
        private string _key2;
        public string Key2
        {
            get { return _key2; }
            set { SetProperty(ref _key2, value); }
        }
        private string _key3;
        public string Key3
        {
            get { return _key3; }
            set { SetProperty(ref _key3, value); }
        }
      
        private string _value;
        public string Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
        private string _sourceLeft;
        public string SourceLeft
        {
            get { return _sourceLeft; }
            set { SetProperty(ref _sourceLeft, value); }
        }
        private string _textLeft;
        public string TextLeft
        {
            get { return _textLeft; }
            set { SetProperty(ref _textLeft, value); }
        }
        private string _sourceRight;
        public string SourceRight
        {
            get { return _sourceRight; }
            set { SetProperty(ref _sourceRight, value); }
        }
        private Thickness _margin;
        public Thickness Margin
        {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }
        private VerticalAlignment _valign = VerticalAlignment.Center;
        public VerticalAlignment Valign
        {
            get { return _valign; }
            set { SetProperty(ref _valign, value); }
        }
        private string _value2;
        public string Value2
        {
            get { return _value2; }
            set { SetProperty(ref _value2, value); }
        }
        private int _rowHeight;
        public int RowHeight
        {
            get { return _rowHeight; }
            set { SetProperty(ref _rowHeight, value); }
        }
        private SummaryCellType _cellType;
        public SummaryCellType CellType
        {
            get { return _cellType; }
            set
            {
                SetProperty(ref _cellType, value);
                SetVisiblityValues(value);
            }
        }
        private void SetVisiblityValues(SummaryCellType cellType)
        {
            switch (cellType)
            {
                case SummaryCellType.OneColumn:
                    _showImageRight = Visibility.Collapsed;
                    _showImageLeft = Visibility.Collapsed;
                    _leftText = Visibility.Collapsed;
                    _showTextLeft = Visibility.Collapsed;
                    _rightText = Visibility.Collapsed;
                    _centerText = Visibility.Visible;
                    _darkLine = Visibility.Collapsed;
                    break;
                case SummaryCellType.TwoColumn:
                    _showImageRight = Visibility.Collapsed;
                    _showImageLeft = Visibility.Collapsed;
                    _leftText = Visibility.Visible;
                    _showTextLeft = Visibility.Collapsed;
                    _rightText = Visibility.Visible;
                    _centerText = Visibility.Collapsed;
                    _darkLine = Visibility.Collapsed;
                    break;
                case SummaryCellType.TwoColumnImageLeft:
                    _showImageRight = Visibility.Collapsed;
                    _showImageLeft = Visibility.Visible;
                    _leftText = Visibility.Visible;
                    _showTextLeft = Visibility.Collapsed;
                    _rightText = Visibility.Collapsed;
                    _centerText = Visibility.Collapsed;
                    _darkLine = Visibility.Collapsed;
                    break;
                case SummaryCellType.TwoColumnImageRight:
                    _showImageRight = Visibility.Visible;
                    _showImageLeft = Visibility.Collapsed;
                    _leftText = Visibility.Visible;
                    _showTextLeft = Visibility.Collapsed;
                    _rightText = Visibility.Collapsed;
                    _centerText = Visibility.Collapsed;
                    _darkLine = Visibility.Collapsed;
                    break;
                case SummaryCellType.TwoColumnTextLeft:
                    _showImageRight = Visibility.Visible;
                    _showImageLeft = Visibility.Collapsed;
                    _leftText = Visibility.Visible;
                    _showTextLeft = Visibility.Visible;
                    _rightText = Visibility.Collapsed;
                    _centerText = Visibility.Collapsed;
                    _darkLine = Visibility.Collapsed;
                    break;
            }
        }
        private Visibility _showImageRight;
        private Visibility ShowImageRight
        {
            get { return _showImageRight; }
        }
        private Visibility _showImageLeft;
        private Visibility ShowImageLeft
        {
            get { return _showImageLeft; }
        }
        private Visibility _showTextLeft;
        private Visibility ShowTextLeft
        {
            get { return _showTextLeft; }
        }
        private Visibility _leftText;
        private Visibility LeftText
        {
            get { return _leftText; }
        }
        private Visibility _rightText;
        private Visibility RightText
        {
            get { return _rightText; }
        }
        private Visibility _centerText;
        private Visibility CenterText
        {
            get { return _centerText; }
        }
        private Visibility _darkLine;
        public Visibility DarkLine
        {
            get { return _darkLine; }
            set { SetProperty(ref _darkLine, value); }
        }
        private Visibility _thinLine;
        public Visibility ThinLine
        {
            get { return _thinLine; }
            set { SetProperty(ref _thinLine, value); }
        }
    }
}
