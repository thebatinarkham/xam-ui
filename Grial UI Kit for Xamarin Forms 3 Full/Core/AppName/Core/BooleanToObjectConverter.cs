using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppName.Core
{
	public class BooleanToObjectConverter<T> : IValueConverter
	{
		public T TrueValue
		{
			get;
			set;
		}

		public T FalseValue
		{
			get;
			set;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool))
			{
				return default(T);
			}
			return ((bool)value) ? TrueValue : FalseValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
