using System;

namespace AppName.Core
{
	public class VideoPlayerErrorEventArgs : EventArgs
	{
		public string Error
		{
			get;
			private set;
		}

		public Exception Exception
		{
			get;
			private set;
		}

		public VideoPlayerErrorEventArgs(Exception exception)
		{
			Exception = exception;
		}

		public VideoPlayerErrorEventArgs(string error)
		{
			Error = error;
		}
	}
}
