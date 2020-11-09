using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppName.Core
{
	internal class VideoPlayerFullScreenContentPage : ContentPage
	{
		private readonly VideoPlayer _video;

		private readonly VideoPlayerContainer _container;

		private bool _alreadyGone;

		public VideoPlayerFullScreenContentPage(VideoPlayer video, VideoPlayerContainer container)
		{
			_video = video;
			_container = container;
			if (_container != null)
			{
				_container?.GoFullScreen(this);
			}
			else
			{
				base.Content = video;
			}
			SetBinding(VisualElement.BackgroundColorProperty, new Binding("BackgroundColor", BindingMode.Default, null, null, null, video));
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			_alreadyGone = true;
			if (base.Content == _video)
			{
				_video.ToggleFullScreen();
			}
		}

		public Task Close()
		{
			if (_container != null)
			{
				_container.StopFullScreen(this);
			}
			else
			{
				_video.Stop();
				base.Content = null;
			}
			Task result = Task.CompletedTask;
			if (!_alreadyGone)
			{
				result = base.Navigation.PopModalAsync(animated: false);
			}
			return result;
		}

		public Task Show(INavigation navigation)
		{
			return navigation.PushModalAsync(this, animated: false);
		}
	}
}
