using Xamarin.Forms;

namespace AppName.Core
{
	internal class VideoPlayerVisualTreeHelper
	{
		public enum State
		{
			Initial = 0,
			Video = 1,
			Error = 2
		}

		private readonly VideoPlayer _player;

		private readonly View _tapElement;

		private bool _isInitialized;

		private State _currentState;

		public FormsVideoPlayer FormsPlayer
		{
			get;
		}

		public VideoPlayerSkin VideoSkin
		{
			get;
		}

		public VideoPlayerLoadingSkin LoadingSkin
		{
			get;
		}

		public VideoPlayerThumbnailSkin ThumbnailSkin
		{
			get;
		}

		public VideoPlayerErrorSkin ErrorSkin
		{
			get;
		}

		public VideoPlayerVisualTreeHelper(VideoPlayer player, FormsVideoPlayer formsPlayer)
		{
			_player = player;
			FormsPlayer = formsPlayer;
			AbsoluteLayout.SetLayoutBounds(FormsPlayer, new Rectangle(0.0, 0.0, 1.0, 1.0));
			AbsoluteLayout.SetLayoutFlags(FormsPlayer, AbsoluteLayoutFlags.All);
			VideoSkin = new VideoPlayerSkin(player)
			{
				AutohideMillisecondsDelay = player.SkinAutohideMillisecondsDelay
			};
			AbsoluteLayout.SetLayoutBounds(VideoSkin, new Rectangle(0.0, 0.0, 1.0, 1.0));
			AbsoluteLayout.SetLayoutFlags(VideoSkin, AbsoluteLayoutFlags.All);
			RefreshSkinTemplate();
			LoadingSkin = new VideoPlayerLoadingSkin(player)
			{
				InputTransparent = true
			};
			LoadingSkin.SetBinding(VisualElement.IsVisibleProperty, new Binding("IsLoading", BindingMode.Default, null, null, null, player));
			AbsoluteLayout.SetLayoutBounds(LoadingSkin, new Rectangle(0.0, 0.0, 1.0, 1.0));
			AbsoluteLayout.SetLayoutFlags(LoadingSkin, AbsoluteLayoutFlags.All);
			RefreshLoadingTemplate();
			ErrorSkin = new VideoPlayerErrorSkin(player);
			AbsoluteLayout.SetLayoutBounds(ErrorSkin, new Rectangle(0.0, 0.0, 1.0, 1.0));
			AbsoluteLayout.SetLayoutFlags(ErrorSkin, AbsoluteLayoutFlags.All);
			RefreshErrorTemplate();
			ThumbnailSkin = new VideoPlayerThumbnailSkin(player);
			AbsoluteLayout.SetLayoutBounds(ThumbnailSkin, new Rectangle(0.0, 0.0, 1.0, 1.0));
			AbsoluteLayout.SetLayoutFlags(ThumbnailSkin, AbsoluteLayoutFlags.All);
			RefreshThumbnailTemplate();
			_tapElement = new BoxView
			{
				BackgroundColor = Color.Transparent
			};
			_tapElement.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(OnSkinShowRequested)
			});
			AbsoluteLayout.SetLayoutBounds(_tapElement, new Rectangle(0.0, 0.0, 1.0, 1.0));
			AbsoluteLayout.SetLayoutFlags(_tapElement, AbsoluteLayoutFlags.All);
		}

		public void UpdateFullScreen(bool isFullScreen)
		{
			VideoSkin.IsFullScreen = isFullScreen;
			ThumbnailSkin.IsFullScreen = isFullScreen;
			LoadingSkin.IsFullScreen = isFullScreen;
			ErrorSkin.IsFullScreen = isFullScreen;
		}

		public void GotoInitialState()
		{
			_currentState = State.Initial;
			UpdateVisuals();
		}

		public void GotoVideoState()
		{
			if (_currentState != State.Video)
			{
				_currentState = State.Video;
				UpdateVisuals();
			}
		}

		public void GotoErrorState(string message)
		{
			_currentState = State.Error;
			ErrorSkin.Message = message;
			UpdateVisuals();
		}

		internal bool TryInit()
		{
			if (!_isInitialized)
			{
				_isInitialized = true;
				_player.Children.Add(FormsPlayer);
				if (!_player.UseNativeControls)
				{
					_player.Children.Add(_tapElement);
				}
				_player.Children.Add(ThumbnailSkin);
				if (!_player.UseNativeControls)
				{
					_player.Children.Add(VideoSkin);
					_player.Children.Add(LoadingSkin);
				}
				return true;
			}
			return false;
		}

		internal void IsLoadingChanged()
		{
			UpdateVisuals();
		}

		internal void UseNativeControlsChanged()
		{
			if (_player.UseNativeControls)
			{
				_player.Children.Remove(VideoSkin);
				_player.Children.Remove(LoadingSkin);
				_player.Children.Remove(_tapElement);
			}
			else
			{
				if (_tapElement == null)
				{
					int num = _player.Children.IndexOf(ThumbnailSkin);
					if (num >= 0)
					{
						_player.Children.Insert(num, _tapElement);
					}
					else
					{
						_player.Children.Add(_tapElement);
					}
				}
				if (VideoSkin.Parent == null)
				{
					_player.Children.Add(VideoSkin);
				}
				if (LoadingSkin.Parent == null)
				{
					_player.Children.Add(LoadingSkin);
				}
			}
			UpdateVisuals();
		}

		internal void RefreshSkinTemplate()
		{
			VideoSkin.ControlTemplate = _player.SkinTemplate;
		}

		internal void RefreshErrorTemplate()
		{
			if (_player.ErrorTemplate != null)
			{
				ErrorSkin.ControlTemplate = _player.ErrorTemplate;
				ErrorSkin.Content = null;
				return;
			}
			ErrorSkin.ControlTemplate = null;
			if (ErrorSkin.Content == null)
			{
				Grid grid = new Grid
				{
					Padding = new Thickness(15.0),
					Margin = new Thickness(20.0),
					BackgroundColor = Color.Black
				};
				Effects.SetCornerRadius(grid, 8.0);
				grid.HorizontalOptions = LayoutOptions.Center;
				grid.VerticalOptions = LayoutOptions.Center;
				grid.GestureRecognizers.Add(new TapGestureRecognizer
				{
					Command = new Command(_player.Play)
				});
				Label label = new Label
				{
					TextColor = Color.White
				};
				label.SetBinding(Label.TextProperty, new Binding("Message", BindingMode.Default, null, null, null, ErrorSkin));
				grid.Children.Add(label);
				ErrorSkin.Content = grid;
			}
		}

		internal void RefreshThumbnailTemplate()
		{
			ThumbnailSkin.Thumbnail = _player.Thumbnail;
			if (_player.ThumbnailTemplate != null)
			{
				ThumbnailSkin.ControlTemplate = _player.ThumbnailTemplate;
				ThumbnailSkin.Content = null;
			}
			else
			{
				ThumbnailSkin.ControlTemplate = null;
				if (ThumbnailSkin.Content == null)
				{
					Image image = new Image
					{
						Aspect = Aspect.AspectFill
					};
					image.SetBinding(Image.SourceProperty, new Binding("Thumbnail", BindingMode.Default, null, null, null, _player));
					image.GestureRecognizers.Add(new TapGestureRecognizer
					{
						Command = new Command(_player.Play)
					});
					ThumbnailSkin.Content = image;
				}
			}
			UpdateVisuals();
		}

		internal void RefreshLoadingTemplate()
		{
			if (_player.LoadingTemplate != null)
			{
				LoadingSkin.ControlTemplate = _player.LoadingTemplate;
				LoadingSkin.Content = null;
				return;
			}
			LoadingSkin.ControlTemplate = null;
			if (LoadingSkin.Content == null)
			{
				ActivityIndicator activityIndicator = new ActivityIndicator
				{
					IsRunning = true,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center
				};
				activityIndicator.SetBinding(ActivityIndicator.ColorProperty, new Binding("LoadingSpinnerColor", BindingMode.Default, null, null, null, _player));
				LoadingSkin.Content = activityIndicator;
			}
		}

		private void OnSkinShowRequested(object obj)
		{
			if (!_player.IsLoading)
			{
				VideoSkin.Show();
			}
		}

		private void UpdateVisuals()
		{
			switch (_currentState)
			{
			case State.Initial:
				if (ErrorSkin.Parent != null)
				{
					_player.Children.Remove(ErrorSkin);
				}
				ThumbnailSkin.IsVisible = ThumbnailSkin.HasContent;
				if (!_player.UseNativeControls)
				{
					if (!ThumbnailSkin.HasContent && !_player.IsLoading && !_player.AutoPlay)
					{
						VideoSkin.Show();
					}
					else
					{
						VideoSkin.Hide();
					}
				}
				break;
			case State.Video:
				if (ErrorSkin.Parent != null)
				{
					_player.Children.Remove(ErrorSkin);
				}
				ThumbnailSkin.IsVisible = false;
				if (_player.IsLoading)
				{
					VideoSkin.Hide();
				}
				break;
			case State.Error:
				ThumbnailSkin.IsVisible = false;
				if (ErrorSkin.Parent == null)
				{
					_player.Children.Add(ErrorSkin);
				}
				break;
			}
		}
	}
}
