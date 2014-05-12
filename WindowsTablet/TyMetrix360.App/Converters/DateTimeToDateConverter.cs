/*
 * Copyright © 2004 - 2013 TyMetrix, Inc. All Rights Reserved
 */
using System;
using Windows.UI.Xaml.Data;

using TyMetrix360.App.Common;

namespace TyMetrix360.App.Converters
{
    public class DateTimeToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is DateTime)
            {
                return ((DateTime) value).ToString(Constants.DateFormat2);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
