using Xamarin.Forms;

namespace AppName.Core
{
	public class VideoPlayerLoadingSkin : VideoPlayerSkinBase
	{
		internal static readonly BindablePropertyKey SpinnerColorPropertyKey = BindableProperty.CreateReadOnly("SpinnerColor", typeof(Color), typeof(VideoPlayerLoadingSkin), Color.Gray);

		public static readonly BindableProperty SpinnerColorProperty = SpinnerColorPropertyKey.BindableProperty;

		public Color SpinnerColor
		{
			get
			{
				return (Color)GetValue(SpinnerColorProperty);
			}
			internal set
			{
				SetValue(SpinnerColorPropertyKey, value);
			}
		}

		public VideoPlayerLoadingSkin(VideoPlayer owner)
			: base(owner)
		{
		}
	}
}
