using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppName.Core
{
	public abstract class SortVisibilityBaseConverter : IValueConverter
	{
		private readonly DataGridSortType _target;

		protected SortVisibilityBaseConverter(DataGridSortType target)
		{
			_target = target;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DataGridSortType)
			{
				DataGridSortType dataGridSortType = (DataGridSortType)value;
				return dataGridSortType == _target;
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
