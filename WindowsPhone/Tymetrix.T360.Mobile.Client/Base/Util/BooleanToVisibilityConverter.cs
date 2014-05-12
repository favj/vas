using System;
using System.Windows.Data;

namespace Tymetrix.T360.Mobile.Client.Common.Base.Util
{
    public class BooleanToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            Boolean result = true;

            if (parameter != null) Boolean.TryParse(parameter.ToString(), out result);

            if (value != null && value is Boolean)
            {
                if (System.Convert.ToBoolean(value) == result)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Collapsed;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
