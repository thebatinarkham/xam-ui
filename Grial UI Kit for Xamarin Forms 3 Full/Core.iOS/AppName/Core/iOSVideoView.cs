using AVFoundation;
using AVKit;

namespace AppName.Core
{
	internal class iOSVideoView
	{
		public AVPlayer Player
		{
			get;
			set;
		}

		public AVPlayerItem PlayerItem
		{
			get;
			set;
		}

		public AVPlayerViewController PlayerViewController
		{
			get;
			set;
		}
	}
}
