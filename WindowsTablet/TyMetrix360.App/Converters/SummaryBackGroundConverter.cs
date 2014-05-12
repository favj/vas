/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

using TyMetrix360.App.Common;
using TyMetrix360.Core.Interfaces;

namespace TyMetrix360.App.Converters
{
    public  class SummaryBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var supportRowIndex = value as ISupportRowIndex;
            if (supportRowIndex != null)
            {
                if (supportRowIndex.Index % 2 == 0)
                    return Application.Current.Resources[Constants.EvenBrushSummary];
                return Application.Current.Resources[Constants.OddBrushSummary];
            }
            return null;
        }


        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
