using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public class ListToGroupsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int groupCount = 0;
            if (parameter is int n)
            {
                groupCount = n;
            }
            else if (parameter is string s)
            {
                int.TryParse(s, out groupCount);
            }

            if (groupCount > 0 && value is IEnumerable e)
            {
                return GetElements(groupCount, e);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Collection<object> GetElements(int groupSize, IEnumerable elements)
        {
            var result = new Collection<object>();

            var i = 0;
            object[] group = null;
            foreach (var item in elements)
            {
                if (i == 0)
                {
                    group = new object[groupSize];
                }

                group[i] = item;
                i = (i + 1) % groupSize;

                if (i == 0)
                {
                    result.Add(new { List = group });
                }
            }

            if (i > 0)
            {
                if (i < groupSize - 1)
                {
                    var oldGroup = group;
                    group = new object[i];
                    Array.Copy(oldGroup, group, i);
                }

                result.Add(new { List = group });
            }

            return result;
        }
    }
}
