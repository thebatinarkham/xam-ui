using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppName.Core
{
	public class LessThanZeroConverter<T> : IValueConverter
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
			return LessThanConverter<T>.Compare(value, 0.0) ? TrueValue : FalseValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
	public class LessThanZeroConverter : LessThanZeroConverter<bool>
	{
		public LessThanZeroConverter()
		{
			base.TrueValue = true;
			base.FalseValue = false;
		}
	}
}
