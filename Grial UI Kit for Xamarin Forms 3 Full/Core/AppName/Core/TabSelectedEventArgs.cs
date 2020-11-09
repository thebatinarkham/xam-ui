using System;

namespace AppName.Core
{
	public class TabSelectedEventArgs : EventArgs
	{
		public object Selected
		{
			get;
		}

		public TabSelectedEventArgs(object selected)
		{
			Selected = selected;
		}
	}
}
