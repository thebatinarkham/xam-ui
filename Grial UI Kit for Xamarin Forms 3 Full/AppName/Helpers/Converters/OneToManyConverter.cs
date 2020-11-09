using System;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public class OneToManyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int count = 0;
            if (parameter is int n)
            {
                count = n;
            }
            else if (parameter is string s)
            {
                int.TryParse(s, out count);
            }

            if (count > 0)
            {
                var result = new object[count];
                for (var i = 0; i < count; i++)
                {
                    result[i] = new { Value = value, Position = i };
                }

                return result;
            }

            return new[] { new { Value = value, Position = 0 } };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
