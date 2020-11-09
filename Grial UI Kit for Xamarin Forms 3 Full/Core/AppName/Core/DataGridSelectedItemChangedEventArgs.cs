using System;

namespace AppName.Core
{
	public class DataGridSelectedItemChangedEventArgs : EventArgs
	{
		public object SelectedItem
		{
			get;
		}

		public DataGridSelectedItemChangedEventArgs(object selectedItem)
		{
			SelectedItem = selectedItem;
		}
	}
}
