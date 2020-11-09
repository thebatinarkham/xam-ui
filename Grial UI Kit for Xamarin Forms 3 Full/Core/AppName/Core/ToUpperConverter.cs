using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppName.Core
{
	public class ToUpperConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return string.Empty;
			}
			return ((value as string) ?? value.ToString()).ToUpper();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
