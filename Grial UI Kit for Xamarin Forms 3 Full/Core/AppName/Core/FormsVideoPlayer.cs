using System;
using Xamarin.Forms;

namespace AppName.Core
{
	internal class FormsVideoPlayer : View
	{
		private VideoStatus _oldStatus;

		public static readonly BindableProperty AspectProperty = BindableProperty.Create("Aspect", typeof(Aspect), typeof(FormsVideoPlayer), Aspect.AspectFit);

		public static readonly BindableProperty AreTransportControlsEnabledProperty = BindableProperty.Create("AreTransportControlsEnabled", typeof(bool), typeof(FormsVideoPlayer), false);

		public static readonly BindableProperty SourceProperty = BindableProperty.Create("Source", typeof(VideoSource), typeof(FormsVideoPlayer));

		public static readonly BindableProperty AutoPlayProperty = BindableProperty.Create("AutoPlay", typeof(bool), typeof(FormsVideoPlayer), false);

		public static readonly BindableProperty MuteProperty = BindableProperty.Create("Mute", typeof(bool), typeof(FormsVideoPlayer), false);

		public static readonly BindableProperty RepeatProperty = BindableProperty.Create("Repeat", typeof(bool), typeof(FormsVideoPlayer), false);

		private bool _isTimerRunning;

		private bool _keepTimerRunning;

		private VideoStatus _status;

		private static readonly BindableProperty DurationProperty = BindableProperty.Create("Duration", typeof(TimeSpan), typeof(FormsVideoPlayer), default(TimeSpan), BindingMode.OneWay, null, delegate(BindableObject bindable, object oldValue, object newValue)
		{
			((FormsVideoPlayer)bindable).SetTimeToEnd();
		});

		public static readonly BindableProperty PositionProperty = BindableProperty.Create("Position", typeof(TimeSpan), typeof(FormsVideoPlayer), default(TimeSpan), BindingMode.OneWay, null, delegate(BindableObject bindable, object oldValue, object newValue)
		{
			((FormsVideoPlayer)bindable).SetTimeToEnd();
		});

		public static readonly BindableProperty StartPositionProperty = BindableProperty.Create("StartPosition", typeof(TimeSpan), typeof(FormsVideoPlayer), default(TimeSpan));

		private static readonly BindablePropertyKey TimeToEndPropertyKey = BindableProperty.CreateReadOnly("TimeToEnd", typeof(TimeSpan), typeof(FormsVideoPlayer), default(TimeSpan));

		public static readonly BindableProperty TimeToEndProperty = TimeToEndPropertyKey.BindableProperty;

		public bool AreTransportControlsEnabled
		{
			get
			{
				return (bool)GetValue(AreTransportControlsEnabledProperty);
			}
			set
			{
				SetValue(AreTransportControlsEnabledProperty, value);
			}
		}

		public VideoSource Source
		{
			get
			{
				return (VideoSource)GetValue(SourceProperty);
			}
			set
			{
				SetValue(SourceProperty, value);
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

		public bool Mute
		{
			get
			{
				return (bool)GetValue(MuteProperty);
			}
			set
			{
				SetValue(MuteProperty, value);
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

		public VideoStatus Status
		{
			get
			{
				return _status;
			}
			set
			{
				_oldStatus = value;
				_status = value;
			}
		}

		public TimeSpan Duration
		{
			get
			{
				return (TimeSpan)GetValue(DurationProperty);
			}
			set
			{
				SetValue(DurationProperty, value);
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

		public TimeSpan StartPosition
		{
			get
			{
				return (TimeSpan)GetValue(StartPositionProperty);
			}
			set
			{
				SetValue(StartPositionProperty, value);
			}
		}

		public TimeSpan TimeToEnd
		{
			get
			{
				return (TimeSpan)GetValue(TimeToEndProperty);
			}
			private set
			{
				SetValue(TimeToEndPropertyKey, value);
			}
		}

		public object NativeViewHandleForTogglingFullScreen
		{
			get;
			set;
		}

		public bool HadAndroidFullscreenFlag
		{
			get;
			set;
		}

		public bool HadAndroidForceNotFullscreen
		{
			get;
			set;
		}

		public event EventHandler<UpdateStatusEventArgs> UpdateStatus;

		public event EventHandler<VideoPlayerErrorEventArgs> LoadingError;

		public event EventHandler PlayRequested;

		public event EventHandler PauseRequested;

		public event EventHandler StopRequested;

		public event EventHandler<SetPositionEventArgs> SetPositionRequested;

		public event EventHandler BeforeFullScreen;

		public event EventHandler BeforeClosingFullScreen;

		internal void RaiseError(VideoPlayerErrorEventArgs args)
		{
			Status = VideoStatus.Error;
			this.LoadingError?.Invoke(this, args);
		}

		internal void StartTimer()
		{
			_keepTimerRunning = true;
			if (!_isTimerRunning)
			{
				_isTimerRunning = true;
				Device.StartTimer(TimeSpan.FromMilliseconds(100.0), delegate
				{
					this.UpdateStatus?.Invoke(this, new UpdateStatusEventArgs
					{
						OldStatus = _oldStatus,
						NewStatus = Status
					});
					_oldStatus = Status;
					if (!_keepTimerRunning)
					{
						_isTimerRunning = false;
					}
					return _keepTimerRunning;
				});
			}
		}

		internal void StopTimer()
		{
			_keepTimerRunning = false;
		}

		public FormsVideoPlayer()
		{
			base.VerticalOptions = LayoutOptions.FillAndExpand;
			base.HorizontalOptions = LayoutOptions.FillAndExpand;
		}

		private void SetTimeToEnd()
		{
			TimeToEnd = Duration - Position;
		}

		public void Play()
		{
			this.PlayRequested?.Invoke(this, EventArgs.Empty);
		}

		public void Pause()
		{
			this.PauseRequested?.Invoke(this, EventArgs.Empty);
		}

		public void Stop()
		{
			this.StopRequested?.Invoke(this, EventArgs.Empty);
		}

		public void SetPosition(TimeSpan newPosition)
		{
			this.SetPositionRequested?.Invoke(this, new SetPositionEventArgs(newPosition));
		}

		public void WillTurnFullScreen()
		{
			this.BeforeFullScreen?.Invoke(this, EventArgs.Empty);
		}

		public void WillCloseFullScreen()
		{
			this.BeforeClosingFullScreen?.Invoke(this, EventArgs.Empty);
		}
	}
}
