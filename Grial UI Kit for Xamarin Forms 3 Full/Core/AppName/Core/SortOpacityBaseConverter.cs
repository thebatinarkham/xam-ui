using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppName.Core
{
	public abstract class SortOpacityBaseConverter : IValueConverter
	{
		private readonly DataGridSortType _target;

		protected SortOpacityBaseConverter(DataGridSortType target)
		{
			_target = target;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DataGridSortType)
			{
				DataGridSortType dataGridSortType = (DataGridSortType)value;
				return (dataGridSortType == _target) ? 1 : 0;
			}
			return 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
