using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public class ItemAtConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int position = 0;
            if (parameter is int n)
            {
                position = n;
            }
            else if (parameter is string s)
            {
                int.TryParse(s, out position);
            }

            if (value is IList e)
            {
                return e.Count > position ? e[position] : null;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
