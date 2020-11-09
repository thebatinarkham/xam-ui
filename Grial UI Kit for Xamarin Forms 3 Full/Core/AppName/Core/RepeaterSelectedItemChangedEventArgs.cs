using System;

namespace AppName.Core
{
	public class RepeaterSelectedItemChangedEventArgs : EventArgs
	{
		public object SelectedItem
		{
			get;
		}

		public RepeaterSelectedItemChangedEventArgs(object selectedItem)
		{
			SelectedItem = selectedItem;
		}
	}
}
