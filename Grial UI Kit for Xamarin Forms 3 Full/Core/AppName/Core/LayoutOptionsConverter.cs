using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppName.Core
{
	internal class LayoutOptionsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DataGridVerticalAlignment)
			{
				DataGridVerticalAlignment verticalAlignment = (DataGridVerticalAlignment)value;
				return verticalAlignment.ToLayoutOptions();
			}
			if (value is TextAlignment)
			{
				TextAlignment textAlignment = (TextAlignment)value;
				return textAlignment.ToLayoutOptions();
			}
			return value ?? ((object)LayoutOptions.Start);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
