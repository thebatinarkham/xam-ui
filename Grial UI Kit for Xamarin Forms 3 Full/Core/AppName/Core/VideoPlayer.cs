using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppName.Core
{
    public class VideoPlayer : AbsoluteLayout
	{
		private static readonly BindablePropertyKey IsLoadingPropertyKey = BindableProperty.CreateReadOnly("IsLoading", typeof(bool), typeof(VideoPlayer), false, BindingMode.OneWayToSource, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayer)s)._visualTreeHelper.IsLoadingChanged();
		});

		public static readonly BindableProperty IsLoadingProperty = IsLoadingPropertyKey.BindableProperty;

		private static readonly BindablePropertyKey DurationPropertyKey = BindableProperty.CreateReadOnly("Duration", typeof(TimeSpan), typeof(VideoPlayer), default(TimeSpan), BindingMode.OneWayToSource, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayer)s)._visualTreeHelper.VideoSkin.Duration = (TimeSpan)n;
		});

		public static readonly BindableProperty DurationProperty = DurationPropertyKey.BindableProperty;

		public static readonly BindableProperty PositionProperty = BindableProperty.Create("Position", typeof(TimeSpan), typeof(VideoPlayerSkin), default(TimeSpan), BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayer)s).OnPositionChanged();
		});

		public static readonly BindableProperty SkinTemplateProperty = BindableProperty.Create("SkinTemplate", typeof(ControlTemplate), typeof(VideoPlayer), null, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayer)s)._visualTreeHelper.RefreshSkinTemplate();
		});

		public static readonly BindableProperty ErrorTemplateProperty = BindableProperty.Create("ErrorTemplate", typeof(ControlTemplate), typeof(VideoPlayer), null, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayer)s)._visualTreeHelper.RefreshErrorTemplate();
		});

		public static readonly BindableProperty SkinAutohideMillisecondsDelayProperty = BindableProperty.Create("SkinAutohideMillisecondsDelay", typeof(int), typeof(VideoPlayer), 4000, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayer)s)._visualTreeHelper.VideoSkin.AutohideMillisecondsDelay = (int)n;
		});

		public static readonly BindableProperty LoadingTemplateProperty = BindableProperty.Create("LoadingTemplate", typeof(ControlTemplate), typeof(VideoPlayer), null, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayer)s)._visualTreeHelper.RefreshLoadingTemplate();
		});

		public static readonly BindableProperty LoadingSpinnerColorProperty = BindableProperty.Create("LoadingSpinnerColor", typeof(Color), typeof(VideoPlayer), Color.Gray, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayer)s)._visualTreeHelper.LoadingSkin.SpinnerColor = (Color)n;
		});

		public static readonly BindableProperty UseNativeControlsProperty = BindableProperty.Create("UseNativeControls", typeof(bool), typeof(VideoPlayer), false, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			VideoPlayer obj3 = (VideoPlayer)s;
			obj3._visualTreeHelper.UseNativeControlsChanged();
			obj3._visualTreeHelper.FormsPlayer.AreTransportControlsEnabled = (bool)n;
		});

		public static readonly BindableProperty SourceProperty = BindableProperty.Create("Source", typeof(string), typeof(VideoPlayer), string.Empty, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayer)s).OnSourceUpdated().Forget();
		});

		public static readonly BindableProperty RepeatProperty = BindableProperty.Create("Repeat", typeof(bool), typeof(VideoPlayer), false, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			VideoPlayer obj2 = (VideoPlayer)s;
			bool repeat = (bool)n;
			obj2._visualTreeHelper.VideoSkin.Repeat = repeat;
			obj2._visualTreeHelper.FormsPlayer.Repeat = repeat;
		});

		public static readonly BindableProperty AutoPlayProperty = BindableProperty.Create("AutoPlay", typeof(bool), typeof(VideoPlayer), false, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayer)s)._visualTreeHelper.FormsPlayer.AutoPlay = (bool)n;
		});

		public static readonly BindableProperty IsMutedProperty = BindableProperty.Create("IsMuted", typeof(bool), typeof(VideoPlayer), false, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			VideoPlayer obj = (VideoPlayer)s;
			bool isMuted = (bool)n;
			obj._visualTreeHelper.VideoSkin.IsMuted = isMuted;
			obj._visualTreeHelper.FormsPlayer.Mute = (bool)n;
		});

		public static readonly BindableProperty AspectProperty = BindableProperty.Create("Aspect", typeof(Aspect), typeof(VideoPlayer), Aspect.AspectFit, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayer)s)._visualTreeHelper.FormsPlayer.Aspect = (Aspect)n;
		});

		public static readonly BindableProperty ThumbnailTemplateProperty = BindableProperty.Create("ThumbnailTemplate", typeof(ControlTemplate), typeof(VideoPlayer), null, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayer)s)._visualTreeHelper.RefreshThumbnailTemplate();
		});

		public static readonly BindableProperty ThumbnailProperty = BindableProperty.Create("Thumbnail", typeof(ImageSource), typeof(VideoPlayer), null, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayer)s)._visualTreeHelper.RefreshThumbnailTemplate();
		});

		private readonly VideoPlayerVisualTreeHelper _visualTreeHelper;

		private bool _lastPositionSetThroughSkinAndRemainedPausedEverSince;

		private TimeSpan _lastKnownPosition;

		private bool _errorOccurredWhileGettingSource;

		public bool IsLoading
		{
			get
			{
				return (bool)GetValue(IsLoadingProperty);
			}
			private set
			{
				SetValue(IsLoadingPropertyKey, value);
			}
		}

		public TimeSpan Duration
		{
			get
			{
				return (TimeSpan)GetValue(DurationProperty);
			}
			private set
			{
				SetValue(DurationPropertyKey, value);
			}
		}

		public TimeSpan Position
		{
			get
			{
				return (TimeSpan)GetValue(PositionProperty);
			}
			set
			{
				SetValue(PositionProperty, value);
			}
		}

		public ControlTemplate SkinTemplate
		{
			get
			{
				return (ControlTemplate)GetValue(SkinTemplateProperty);
			}
			set
			{
				SetValue(SkinTemplateProperty, value);
			}
		}

		public ControlTemplate ErrorTemplate
		{
			get
			{
				return (ControlTemplate)GetValue(ErrorTemplateProperty);
			}
			set
			{
				SetValue(ErrorTemplateProperty, value);
			}
		}

		public int SkinAutohideMillisecondsDelay
		{
			get
			{
				return (int)GetValue(SkinAutohideMillisecondsDelayProperty);
			}
			set
			{
				SetValue(SkinAutohideMillisecondsDelayProperty, value);
			}
		}

		public ControlTemplate LoadingTemplate
		{
			get
			{
				return (ControlTemplate)GetValue(LoadingTemplateProperty);
			}
			set
			{
				SetValue(LoadingTemplateProperty, value);
			}
		}

		public Color LoadingSpinnerColor
		{
			get
			{
				return (Color)GetValue(LoadingSpinnerColorProperty);
			}
			set
			{
				SetValue(LoadingSpinnerColorProperty, value);
			}
		}

		public bool UseNativeControls
		{
			get
			{
				return (bool)GetValue(UseNativeControlsProperty);
			}
			set
			{
				SetValue(UseNativeControlsProperty, value);
			}
		}

		public string Source
		{
			get
			{
				return (string)GetValue(SourceProperty);
			}
			set
			{
				SetValue(SourceProperty, value);
			}
		}

		public bool Repeat
		{
			get
			{
				return (bool)GetValue(RepeatProperty);
			}
			set
			{
				SetValue(RepeatProperty, value);
			}
		}

		public bool AutoPlay
		{
			get
			{
				return (bool)GetValue(AutoPlayProperty);
			}
			set
			{
				SetValue(AutoPlayProperty, value);
			}
		}

		public bool IsMuted
		{
			get
			{
				return (bool)GetValue(IsMutedProperty);
			}
			set
			{
				SetValue(IsMutedProperty, value);
			}
		}

		public Aspect Aspect
		{
			get
			{
				return (Aspect)GetValue(AspectProperty);
			}
			set
			{
				SetValue(AspectProperty, value);
			}
		}

		public ControlTemplate ThumbnailTemplate
		{
			get
			{
				return (ControlTemplate)GetValue(ThumbnailTemplateProperty);
			}
			set
			{
				SetValue(ThumbnailTemplateProperty, value);
			}
		}

		public ImageSource Thumbnail
		{
			get
			{
				return (ImageSource)GetValue(ThumbnailProperty);
			}
			set
			{
				SetValue(ThumbnailProperty, value);
			}
		}

		public event EventHandler<VideoPlayerErrorEventArgs> Error;

		public event EventHandler Ended;

		public VideoPlayer()
		{
			base.BackgroundColor = Color.Black;
			_visualTreeHelper = new VideoPlayerVisualTreeHelper(this, CreatePlayer());
		}

		protected override void OnParentSet()
		{
			base.OnParentSet();
			if (_visualTreeHelper.TryInit())
			{
				_visualTreeHelper.GotoInitialState();
			}
		}

		public void Play()
		{
			PlayPrivate();
		}

		private async void PlayPrivate()
		{
			if (_errorOccurredWhileGettingSource)
			{
				await OnSourceUpdated();
			}
			PrivatePlay();
		}

		internal async void TryPlayAfterErrorFromSkin()
		{
			await OnSourceUpdated();
			PrivatePlay();
		}

		private void PrivatePlay()
		{
			if (!_errorOccurredWhileGettingSource)
			{
				if (_visualTreeHelper.FormsPlayer.Status != VideoStatus.Error)
				{
					_visualTreeHelper.GotoVideoState();
				}
				_visualTreeHelper.FormsPlayer.Play();
			}
		}

		public void Pause()
		{
			_visualTreeHelper.FormsPlayer.Pause();
		}

		public void Stop()
		{
			_visualTreeHelper.FormsPlayer.Stop();
		}

		public void HideSkin()
		{
			_visualTreeHelper.VideoSkin.Hide();
		}

		public void ShowSkin()
		{
			_visualTreeHelper.VideoSkin.Show();
		}

		private void Player_UpdateStatus(object sender, UpdateStatusEventArgs e)
		{
			IsLoading = (_visualTreeHelper.FormsPlayer.Status == VideoStatus.NotReady);
			if (!_errorOccurredWhileGettingSource && e.NewStatus != VideoStatus.Error)
			{
				_visualTreeHelper.GotoVideoState();
			}
			FormsVideoPlayer formsVideoPlayer = sender as FormsVideoPlayer;
			if (formsVideoPlayer != null && formsVideoPlayer.Duration.TotalMilliseconds > 0.0)
			{
				VideoPlayerSkin videoSkin = _visualTreeHelper.VideoSkin;
				if (!_lastPositionSetThroughSkinAndRemainedPausedEverSince || formsVideoPlayer.Status != VideoStatus.Paused)
				{
					videoSkin.SetVideoProgressFromVideoPlayer(formsVideoPlayer.Position.TotalMilliseconds / formsVideoPlayer.Duration.TotalMilliseconds);
					_lastPositionSetThroughSkinAndRemainedPausedEverSince = false;
				}
				videoSkin.IsPlaying = (formsVideoPlayer.Status == VideoStatus.Playing);
				Duration = formsVideoPlayer.Duration;
				_lastKnownPosition = formsVideoPlayer.Position;
				Position = _lastKnownPosition;
				if (e.OldStatus != VideoStatus.Ended && e.NewStatus == VideoStatus.Ended)
				{
					this.Ended?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		internal void SetProgressFromSkin(double progress)
		{
			FormsVideoPlayer formsPlayer = _visualTreeHelper.FormsPlayer;
			if (progress < 0.0)
			{
				progress = 0.0;
			}
			else if (progress > 1.0)
			{
				progress = 1.0;
			}
			TimeSpan position = TimeSpan.FromMilliseconds(formsPlayer.Duration.TotalMilliseconds * progress);
			_visualTreeHelper.FormsPlayer.SetPosition(position);
			_lastPositionSetThroughSkinAndRemainedPausedEverSince = true;
			Pause();
		}

		private void OnPositionChanged()
		{
			TimeSpan position = Position;
			if (position != _lastKnownPosition)
			{
				_visualTreeHelper.FormsPlayer.SetPosition(position);
			}
			_visualTreeHelper.VideoSkin.Position = position;
		}

		internal async void MakeParentlessVideoPlayerFullScreen(INavigation navigation)
		{
			if (base.Parent == null)
			{
				_visualTreeHelper.FormsPlayer.WillTurnFullScreen();
				await new VideoPlayerFullScreenContentPage(this, null).Show(navigation);
				_visualTreeHelper.UpdateFullScreen(isFullScreen: true);
			}
		}

		public async void ToggleFullScreen()
		{
			if (_visualTreeHelper.VideoSkin.IsFullScreen)
			{
				VideoPlayerFullScreenContentPage videoPlayerFullScreenContentPage = base.Parent as VideoPlayerFullScreenContentPage;
				if (videoPlayerFullScreenContentPage != null)
				{
					_visualTreeHelper.FormsPlayer.WillCloseFullScreen();
					await videoPlayerFullScreenContentPage.Close();
					_visualTreeHelper.UpdateFullScreen(isFullScreen: false);
				}
			}
			else
			{
				VideoPlayerContainer videoPlayerContainer = base.Parent as VideoPlayerContainer;
				if (videoPlayerContainer != null)
				{
					_visualTreeHelper.FormsPlayer.WillTurnFullScreen();
					await new VideoPlayerFullScreenContentPage(this, videoPlayerContainer).Show(videoPlayerContainer.Navigation);
					_visualTreeHelper.UpdateFullScreen(isFullScreen: true);
				}
			}
		}

		private void Player_LoadingError(object sender, VideoPlayerErrorEventArgs e)
		{
			OnError(e);
		}

		private void OnError(VideoPlayerErrorEventArgs e)
		{
			_visualTreeHelper.GotoErrorState(e.Error ?? e.Exception.Message);
			this.Error?.Invoke(this, e);
		}

		private async Task OnSourceUpdated()
		{
			try
			{
				_errorOccurredWhileGettingSource = false;
				string originalSource = Source;
				string text = originalSource;
				if (!string.IsNullOrWhiteSpace(originalSource) && YouTubeUtilities.IsYouTubeSource(originalSource))
				{
					text = await YouTubeUtilities.GetYouTubeUrl(originalSource);
				}
				if (originalSource == Source)
				{
					text = text.Trim();
					if (text.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
					{
						_visualTreeHelper.FormsPlayer.Source = VideoSource.FromUri(text);
					}
					else if (text.StartsWith("file:", StringComparison.InvariantCultureIgnoreCase))
					{
						_visualTreeHelper.FormsPlayer.Source = VideoSource.FromFile(text);
					}
					else
					{
						_visualTreeHelper.FormsPlayer.Source = VideoSource.FromResource(text);
					}
				}
			}
			catch (Exception exception)
			{
				OnError(new VideoPlayerErrorEventArgs(exception));
				_errorOccurredWhileGettingSource = true;
			}
		}

		private FormsVideoPlayer CreatePlayer()
		{
			FormsVideoPlayer formsVideoPlayer = new FormsVideoPlayer();
			formsVideoPlayer.SetBinding(VisualElement.BackgroundColorProperty, new Binding("BackgroundColor", BindingMode.Default, null, null, null, this));
			formsVideoPlayer.LoadingError += Player_LoadingError;
			formsVideoPlayer.UpdateStatus += Player_UpdateStatus;
			return formsVideoPlayer;
		}
	}
}
