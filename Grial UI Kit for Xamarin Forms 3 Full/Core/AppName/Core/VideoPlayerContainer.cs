using Xamarin.Forms;

namespace AppName.Core
{
	public class VideoPlayerContainer : ContentView
	{
		private double? _height;

		private double? _width;

		private Thickness _margin;

		internal void GoFullScreen(ContentPage page)
		{
			VideoPlayer videoPlayer = base.Content as VideoPlayer;
			if (videoPlayer != null)
			{
				videoPlayer.HorizontalOptions = LayoutOptions.Fill;
				videoPlayer.VerticalOptions = LayoutOptions.Fill;
				_margin = videoPlayer.Margin;
				videoPlayer.Margin = default(Thickness);
				if (videoPlayer.HeightRequest > 0.0)
				{
					_height = videoPlayer.HeightRequest;
					videoPlayer.HeightRequest = -1.0;
				}
				if (videoPlayer.WidthRequest > 0.0)
				{
					_width = videoPlayer.WidthRequest;
					videoPlayer.WidthRequest = -1.0;
				}
				if (videoPlayer.BindingContext == base.BindingContext && !videoPlayer.IsSet(BindableObject.BindingContextProperty))
				{
					videoPlayer.SetBinding(BindableObject.BindingContextProperty, new Binding("BindingContext", BindingMode.Default, null, null, null, this));
				}
				base.Content = null;
				page.Content = videoPlayer;
			}
		}

		internal void StopFullScreen(ContentPage page)
		{
			VideoPlayer videoPlayer = page.Content as VideoPlayer;
			if (videoPlayer != null)
			{
				page.Content = null;
				base.Content = videoPlayer;
				videoPlayer.Margin = _margin;
				double? height = _height;
				if (height.HasValue)
				{
					double num = videoPlayer.HeightRequest = height.GetValueOrDefault();
					_height = null;
				}
				height = _width;
				if (height.HasValue)
				{
					double num2 = videoPlayer.WidthRequest = height.GetValueOrDefault();
					_width = null;
				}
			}
		}
	}
}
