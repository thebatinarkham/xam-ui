using System;

namespace AppName.Core
{
	internal class SetPositionEventArgs : EventArgs
	{
		public TimeSpan NewPosition
		{
			get;
			private set;
		}

		public SetPositionEventArgs(TimeSpan newPosition)
		{
			NewPosition = newPosition;
		}
	}
}
