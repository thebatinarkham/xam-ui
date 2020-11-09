using System;

namespace AppName.Core
{
	public class DataGridSortChangedEventArgs : EventArgs
	{
		public DataGridSortType SortType
		{
			get;
		}

		public DataGridSortChangedEventArgs(DataGridSortType sortType)
		{
			SortType = sortType;
		}
	}
}
