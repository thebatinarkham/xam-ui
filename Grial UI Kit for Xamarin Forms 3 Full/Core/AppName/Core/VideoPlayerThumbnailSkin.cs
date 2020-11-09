using System.Windows.Input;
using Xamarin.Forms;

namespace AppName.Core
{
	public class VideoPlayerThumbnailSkin : VideoPlayerSkinBase
	{
		internal static readonly BindablePropertyKey ThumbnailPropertyKey = BindableProperty.CreateReadOnly("Thumbnail", typeof(ImageSource), typeof(VideoPlayerThumbnailSkin), null);

		public static readonly BindableProperty ThumbnailProperty = ThumbnailPropertyKey.BindableProperty;

		public ImageSource Thumbnail
		{
			get
			{
				return (ImageSource)GetValue(ThumbnailProperty);
			}
			internal set
			{
				SetValue(ThumbnailPropertyKey, value);
			}
		}

		public ICommand PlayCommand
		{
			get;
		}

		internal bool HasContent
		{
			get
			{
				if (Thumbnail == null)
				{
					return base.ControlTemplate != null;
				}
				return true;
			}
		}

		public VideoPlayerThumbnailSkin(VideoPlayer owner)
			: base(owner)
		{
			PlayCommand = new Command(base.Owner.Play);
		}
	}
}
