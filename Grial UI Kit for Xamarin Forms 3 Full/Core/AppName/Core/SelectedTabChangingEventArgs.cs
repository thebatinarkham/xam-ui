using System;

namespace AppName.Core
{
	public class SelectedTabChangingEventArgs : EventArgs
	{
		public bool CancelSelection
		{
			get;
			set;
		}

		public object NewSelection
		{
			get;
		}

		public SelectedTabChangingEventArgs(object selection)
		{
			NewSelection = selection;
		}
	}
}
