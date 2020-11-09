using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppName.Core
{
	public class GreaterThanZeroConverter<T> : IValueConverter
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
			return GreaterThanConverter<T>.Compare(value, 0.0) ? TrueValue : FalseValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
	public class GreaterThanZeroConverter : GreaterThanZeroConverter<bool>
	{
		public GreaterThanZeroConverter()
		{
			base.TrueValue = true;
			base.FalseValue = false;
		}
	}
}
