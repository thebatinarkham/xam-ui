using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppName.Core
{
	public class VideoPlayerSkin : TemplatedView
	{
		internal static readonly BindablePropertyKey PositionPropertyKey = BindableProperty.CreateReadOnly("Position", typeof(TimeSpan), typeof(VideoPlayerSkin), default(TimeSpan));

		public static readonly BindableProperty PositionProperty = PositionPropertyKey.BindableProperty;

		internal static readonly BindablePropertyKey DurationPropertyKey = BindableProperty.CreateReadOnly("Duration", typeof(TimeSpan), typeof(VideoPlayerSkin), default(TimeSpan));

		public static readonly BindableProperty DurationProperty = DurationPropertyKey.BindableProperty;

		internal static readonly BindablePropertyKey IsFullScreenPropertyKey = BindableProperty.CreateReadOnly("IsFullScreen", typeof(bool), typeof(VideoPlayerSkin), false);

		public static readonly BindableProperty IsFullScreenProperty = IsFullScreenPropertyKey.BindableProperty;

		internal static readonly BindablePropertyKey IsPlayingPropertyKey = BindableProperty.CreateReadOnly("IsPlaying", typeof(bool), typeof(VideoPlayerSkin), false, BindingMode.OneWayToSource, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayerSkin)s).IsPlayingChanged();
		});

		public static readonly BindableProperty IsPlayingProperty = IsPlayingPropertyKey.BindableProperty;

		internal static readonly BindablePropertyKey IsMutedPropertyKey = BindableProperty.CreateReadOnly("IsMuted", typeof(bool), typeof(VideoPlayerSkin), false);

		public static readonly BindableProperty IsMutedProperty = IsMutedPropertyKey.BindableProperty;

		internal static readonly BindablePropertyKey RepeatPropertyKey = BindableProperty.CreateReadOnly("Repeat", typeof(bool), typeof(VideoPlayerSkin), false);

		public static readonly BindableProperty RepeatProperty = RepeatPropertyKey.BindableProperty;

		public static readonly BindableProperty VideoProgressProperty = BindableProperty.Create("VideoProgress", typeof(double), typeof(VideoPlayerSkin), 0.0, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			((VideoPlayerSkin)s).VideoProgressChanged((double)n);
		});

		private readonly VideoPlayer _owner;

		private CancellationTokenSource _hideCancellation;

		private double _internalProgressSetFromVideo;

		public TimeSpan Position
		{
			get
			{
				return (TimeSpan)GetValue(PositionProperty);
			}
			internal set
			{
				SetValue(PositionPropertyKey, value);
			}
		}

		public TimeSpan Duration
		{
			get
			{
				return (TimeSpan)GetValue(DurationProperty);
			}
			internal set
			{
				SetValue(DurationPropertyKey, value);
			}
		}

		public bool IsFullScreen
		{
			get
			{
				return (bool)GetValue(IsFullScreenProperty);
			}
			internal set
			{
				SetValue(IsFullScreenPropertyKey, value);
			}
		}

		public bool IsPlaying
		{
			get
			{
				return (bool)GetValue(IsPlayingProperty);
			}
			internal set
			{
				SetValue(IsPlayingPropertyKey, value);
			}
		}

		public bool IsMuted
		{
			get
			{
				return (bool)GetValue(IsMutedProperty);
			}
			internal set
			{
				SetValue(IsMutedPropertyKey, value);
			}
		}

		public bool Repeat
		{
			get
			{
				return (bool)GetValue(RepeatProperty);
			}
			internal set
			{
				SetValue(RepeatPropertyKey, value);
			}
		}

		public double VideoProgress
		{
			get
			{
				return (double)GetValue(VideoProgressProperty);
			}
			set
			{
				SetValue(VideoProgressProperty, value);
			}
		}

		public ICommand PlayCommand
		{
			get;
		}

		public ICommand PauseCommand
		{
			get;
		}

		public ICommand StopCommand
		{
			get;
		}

		public ICommand ToggleFullScreenCommand
		{
			get;
		}

		public ICommand ToggleMuteCommand
		{
			get;
		}

		public ICommand ToggleRepeat
		{
			get;
		}

		internal int AutohideMillisecondsDelay
		{
			get;
			set;
		}

		public VideoPlayerSkin(VideoPlayer owner)
		{
			_owner = owner;
			PlayCommand = new Command((Action)delegate
			{
				_owner.Play();
			});
			PauseCommand = new Command((Action)delegate
			{
				_owner.Pause();
			});
			StopCommand = new Command((Action)delegate
			{
				_owner.Stop();
			});
			ToggleFullScreenCommand = new Command((Action)delegate
			{
				DoCommandResettingAutohide(_owner.ToggleFullScreen);
			});
			ToggleMuteCommand = new Command((Action)delegate
			{
				DoCommandResettingAutohide(delegate
				{
					_owner.IsMuted = !_owner.IsMuted;
				});
			});
			ToggleRepeat = new Command((Action)delegate
			{
				DoCommandResettingAutohide(delegate
				{
					_owner.Repeat = !_owner.Repeat;
				});
			});
			base.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(OnTapped)
			});
		}

		public void Show()
		{
			base.IsVisible = true;
			PerformAutohide().Forget();
		}

		public void Hide()
		{
			CancelHide();
			base.IsVisible = false;
		}

		protected override void OnParentSet()
		{
			base.OnParentSet();
			IsPlayingChanged();
		}

		private void IsPlayingChanged()
		{
			if (base.Parent == null)
			{
				return;
			}
			if (IsPlaying)
			{
				if (base.IsVisible)
				{
					PerformAutohide().Forget();
				}
			}
			else
			{
				base.IsVisible = !_owner.IsLoading;
			}
		}

		private async Task PerformAutohide()
		{
			CancelHide();
			if (IsPlaying && AutohideMillisecondsDelay > 0)
			{
				_hideCancellation = new CancellationTokenSource();
				await Task.Run(() => HideControlsAction(_hideCancellation.Token), _hideCancellation.Token);
			}
		}

		private async Task HideControlsAction(CancellationToken cancellationToken)
		{
			await Task.Delay(AutohideMillisecondsDelay);
			Device.BeginInvokeOnMainThread(delegate
			{
				if (!cancellationToken.IsCancellationRequested && AutohideMillisecondsDelay > 0 && IsPlaying)
				{
					base.IsVisible = false;
				}
			});
		}

		private void OnTapped()
		{
			CancelHide();
			base.IsVisible = false;
		}

		private void CancelHide()
		{
			if (_hideCancellation != null)
			{
				_hideCancellation.Cancel();
			}
			_hideCancellation = null;
		}

		private void DoCommandResettingAutohide(Action action)
		{
			action();
			PerformAutohide().Forget();
		}

		internal void SetVideoProgressFromVideoPlayer(double progress)
		{
			_internalProgressSetFromVideo = progress;
			VideoProgress = progress;
		}

		private void VideoProgressChanged(double progress)
		{
			if (progress != _internalProgressSetFromVideo)
			{
				_owner.SetProgressFromSkin(progress);
			}
		}
	}
}
