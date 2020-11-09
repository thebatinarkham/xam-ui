namespace AppName.Core
{
	internal class UpdateStatusEventArgs
	{
		public VideoStatus OldStatus
		{
			get;
			set;
		}

		public VideoStatus NewStatus
		{
			get;
			set;
		}
	}
}
