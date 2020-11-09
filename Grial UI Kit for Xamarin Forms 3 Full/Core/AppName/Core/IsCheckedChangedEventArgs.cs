using System;

namespace AppName.Core
{
	public class IsCheckedChangedEventArgs : EventArgs
	{
		public bool IsChecked
		{
			get;
			set;
		}
	}
}
