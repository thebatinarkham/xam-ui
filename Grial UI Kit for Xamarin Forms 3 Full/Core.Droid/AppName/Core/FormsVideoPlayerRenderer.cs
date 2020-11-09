using Android.App;
using Android.Content;
using Android.Media;
using Android.Views;
using Android.Widget;
using System;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    internal class FormsVideoPlayerRenderer : ViewRenderer<FormsVideoPlayer, FrameLayout>
    {
        private AndroidVideoView _androidVideoView;

        private bool isPrepared;

        private bool _isInError;

        private bool _isReloadingAfterFullScreenToggle;

        private int? _positionToSeekAfterWait;

        private bool _isWaitingForSeek;

        private int NaturalVideoHeight => (_androidVideoView?.VideoView?.VideoHeight).GetValueOrDefault();

        private int NaturalVideoWidth => _androidVideoView.VideoView?.VideoWidth ?? 0;

        public FormsVideoPlayerRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<FormsVideoPlayer> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                ReleaseAndroidVideoView();
                UnsubscribeFromControlEvents(e.OldElement);
            }
            if (e.NewElement != null)
            {
                SetupControl();
                SubscribeToControlEvents(e.NewElement);
            }
        }

        private void SetupControl()
        {
            bool flag = false;
            AndroidVideoView androidVideoView = base.Element.NativeViewHandleForTogglingFullScreen as AndroidVideoView;
            if (androidVideoView == null)
            {
                flag = true;
                _androidVideoView = new AndroidVideoView();
                _androidVideoView.VideoView = new FormsVideoView(base.Context);
            }
            else
            {
                _androidVideoView = androidVideoView;
                isPrepared = true;
                if (_androidVideoView.VideoView.Parent != null)
                {
                    _androidVideoView.VideoView.RemoveFromParent();
                }
            }
            base.Element.NativeViewHandleForTogglingFullScreen = null;
            FrameLayout frameLayout = new FrameLayout(base.Context);
            frameLayout.AddView(_androidVideoView.VideoView);
            _androidVideoView.VideoView.Prepared += OnVideoViewPrepared;
            _androidVideoView.VideoView.Completion += OnVideoViewCompletition;
            _androidVideoView.VideoView.Error += OnVideoViewError;
            _androidVideoView.VideoView.Info += OnVideoViewInfo;
            SetNativeControl(frameLayout);
            SetAreTransportControlsEnabled();
            if (flag)
            {
                SetSource();
                return;
            }
            _androidVideoView.VideoView.SeekTo((int)base.Element.Position.TotalMilliseconds);
            if (base.Element.Status == VideoStatus.Playing)
            {
                _isReloadingAfterFullScreenToggle = true;
                _androidVideoView.VideoView.Start();
            }
        }

        private void SubscribeToControlEvents(FormsVideoPlayer videoPlayer)
        {
            videoPlayer.StartTimer();
            videoPlayer.UpdateStatus += OnUpdateStatus;
            videoPlayer.PlayRequested += OnPlayRequested;
            videoPlayer.PauseRequested += OnPauseRequested;
            videoPlayer.StopRequested += OnStopRequested;
            videoPlayer.SetPositionRequested += OnSetPositionRequested;
            videoPlayer.BeforeFullScreen += BeforeFullScreen;
            videoPlayer.BeforeClosingFullScreen += BeforeClosingFullScreen;
        }

        private void UnsubscribeFromControlEvents(FormsVideoPlayer videoPlayer)
        {
            videoPlayer.StopTimer();
            videoPlayer.UpdateStatus -= OnUpdateStatus;
            videoPlayer.PlayRequested -= OnPlayRequested;
            videoPlayer.PauseRequested -= OnPauseRequested;
            videoPlayer.StopRequested -= OnStopRequested;
            videoPlayer.SetPositionRequested -= OnSetPositionRequested;
            videoPlayer.BeforeFullScreen -= BeforeFullScreen;
            videoPlayer.BeforeClosingFullScreen -= BeforeClosingFullScreen;
        }

        private void ReleaseAndroidVideoView()
        {
            if (_androidVideoView?.VideoView != null)
            {
                _androidVideoView.VideoView.Prepared -= OnVideoViewPrepared;
                _androidVideoView.VideoView.Completion -= OnVideoViewCompletition;
                _androidVideoView.VideoView.Error -= OnVideoViewError;
                _androidVideoView.VideoView.Info -= OnVideoViewInfo;
                if (_androidVideoView.MediaPlayer != null)
                {
                    _androidVideoView.MediaPlayer.SeekComplete -= OnMediaPlayerSeekComplete;
                }
                if (base.Element != null && base.Element.NativeViewHandleForTogglingFullScreen != _androidVideoView)
                {
                    _androidVideoView.VideoView.StopPlayback();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                ReleaseAndroidVideoView();
            }
            catch
            {
            }
            if (base.Element != null)
            {
                try
                {
                    UnsubscribeFromControlEvents(base.Element);
                }
                catch
                {
                }
            }
            base.Dispose(disposing);
        }

        private void OnVideoViewCompletition(object sender, EventArgs args)
        {
            if (base.Element != null)
            {
                if (base.Element.Repeat)
                {
                    _androidVideoView.VideoView.Start();
                    return;
                }
                Stop();
                base.Element.Status = VideoStatus.Ended;
            }
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            UpdateLayoutParameters();
        }

        private void OnVideoViewPrepared(object sender, EventArgs args)
        {
            isPrepared = true;
            _androidVideoView.MediaPlayer = (MediaPlayer)sender;
            _androidVideoView.MediaPlayer.SeekComplete += OnMediaPlayerSeekComplete;
            TimeSpan duration = TimeSpan.FromMilliseconds(_androidVideoView.VideoView.Duration);
            base.Element.Duration = duration;
            UpdateLayoutParameters();
            MuteOrUnmute();
        }

        private void MuteOrUnmute()
        {
            if (base.Element != null && _androidVideoView.MediaPlayer != null)
            {
                if (base.Element.Mute)
                {
                    _androidVideoView.MediaPlayer.SetVolume(0f, 0f);
                }
                else
                {
                    _androidVideoView.MediaPlayer.SetVolume(1f, 1f);
                }
            }
        }

        private void OnVideoViewError(object sender, MediaPlayer.ErrorEventArgs e)
        {
            string empty = string.Empty;
            switch (e.Extra)
            {
                case -1004:
                    empty = "File or network related operation errors.";
                    break;
                case -1007:
                    empty = "Bitstream is not conforming to the related coding standard or file spec.";
                    break;
                case -1010:
                    empty = "Bitstream is conforming to the related coding standard or file spec, but the media framework does not support the feature.";
                    break;
                case -110:
                    empty = "Some operation takes too long to complete, usually more than 3-5 seconds.";
                    break;
                case -1005:
                    empty = "Connection lost.";
                    break;
                default:
                    empty = "Unspecified media player error.";
                    break;
            }
            string error = e.What.ToString() + " - " + empty;
            base.Element.RaiseError(new VideoPlayerErrorEventArgs(error));
            _isInError = true;
        }

        private void OnVideoViewInfo(object sender, MediaPlayer.InfoEventArgs e)
        {
            if (e.What.HasFlag(MediaInfo.BufferingStart))
            {
                _isInError = false;
                isPrepared = false;
            }
            else if (e.What.HasFlag(MediaInfo.BufferingEnd))
            {
                _isInError = false;
                isPrepared = true;
            }
            else if (e.What.HasFlag(MediaInfo.VideoRenderingStart))
            {
                _isInError = false;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == FormsVideoPlayer.AreTransportControlsEnabledProperty.PropertyName)
            {
                SetAreTransportControlsEnabled();
            }
            else if (e.PropertyName == FormsVideoPlayer.SourceProperty.PropertyName)
            {
                SetSource();
            }
            else if (e.PropertyName == FormsVideoPlayer.MuteProperty.PropertyName)
            {
                MuteOrUnmute();
            }
            else if (e.PropertyName == FormsVideoPlayer.AspectProperty.PropertyName)
            {
                UpdateLayoutParameters();
            }
        }

        private void UpdateLayoutParameters()
        {
            if (NaturalVideoWidth == 0 || NaturalVideoHeight == 0)
            {
                _androidVideoView.VideoView.LayoutParameters = new FrameLayout.LayoutParams(base.Width, base.Height, GravityFlags.Fill);
                return;
            }
            float num = (float)NaturalVideoWidth / (float)NaturalVideoHeight;
            float num2 = (float)base.Width / (float)base.Height;
            switch (base.Element.Aspect)
            {
                case Aspect.Fill:
                    _androidVideoView.VideoView.LayoutParameters = new FrameLayout.LayoutParams(base.Width, base.Height, GravityFlags.Fill)
                    {
                        LeftMargin = 0,
                        RightMargin = 0,
                        TopMargin = 0,
                        BottomMargin = 0
                    };
                    break;
                case Aspect.AspectFit:
                    if (num > num2)
                    {
                        int num7 = (int)((float)base.Width / num);
                        int num8 = (base.Height - num7) / 2;
                        _androidVideoView.VideoView.LayoutParameters = new FrameLayout.LayoutParams(base.Width, num7, GravityFlags.AxisPullAfter | GravityFlags.AxisPullBefore | GravityFlags.AxisSpecified | GravityFlags.CenterVertical)
                        {
                            LeftMargin = 0,
                            RightMargin = 0,
                            TopMargin = num8,
                            BottomMargin = num8
                        };
                    }
                    else
                    {
                        int num9 = (int)((float)base.Height * num);
                        int num10 = (base.Width - num9) / 2;
                        _androidVideoView.VideoView.LayoutParameters = new FrameLayout.LayoutParams(num9, base.Height, (GravityFlags)113)
                        {
                            LeftMargin = num10,
                            RightMargin = num10,
                            TopMargin = 0,
                            BottomMargin = 0
                        };
                    }
                    break;
                case Aspect.AspectFill:
                    if (num > num2)
                    {
                        int num3 = (int)((float)base.Height * num);
                        int num4 = (base.Width - num3) / 2;
                        _androidVideoView.VideoView.LayoutParameters = new FrameLayout.LayoutParams((int)((float)base.Height * num), base.Height, (GravityFlags)113)
                        {
                            TopMargin = 0,
                            BottomMargin = 0,
                            LeftMargin = num4,
                            RightMargin = num4
                        };
                    }
                    else
                    {
                        int num5 = (int)((float)base.Width / num);
                        int num6 = (base.Height - num5) / 2;
                        _androidVideoView.VideoView.LayoutParameters = new FrameLayout.LayoutParams(base.Width, num5, GravityFlags.AxisPullAfter | GravityFlags.AxisPullBefore | GravityFlags.AxisSpecified | GravityFlags.CenterVertical)
                        {
                            LeftMargin = 0,
                            RightMargin = 0,
                            TopMargin = num6,
                            BottomMargin = num6
                        };
                    }
                    break;
            }
        }

        private void SetAreTransportControlsEnabled()
        {
            if (base.Element.AreTransportControlsEnabled)
            {
                _androidVideoView.MediaController = new MediaController(base.Context);
                _androidVideoView.MediaController.SetMediaPlayer(_androidVideoView.VideoView);
                _androidVideoView.VideoView.SetMediaController(_androidVideoView.MediaController);
                return;
            }
            _androidVideoView.VideoView.SetMediaController(null);
            if (_androidVideoView.MediaController != null)
            {
                _androidVideoView.MediaController.SetMediaPlayer(null);
                _androidVideoView.MediaController = null;
            }
        }

        private void SetSource()
        {
            _isInError = false;
            isPrepared = false;
            bool flag = false;
            if (base.Element.Source is UriVideoSource)
            {
                string uri = (base.Element.Source as UriVideoSource).Uri;
                if (!string.IsNullOrWhiteSpace(uri))
                {
                    _androidVideoView.VideoView.SetVideoURI(Android.Net.Uri.Parse(uri));
                    flag = true;
                }
            }
            else if (base.Element.Source is FileVideoSource)
            {
                string file = (base.Element.Source as FileVideoSource).File;
                if (!string.IsNullOrWhiteSpace(file))
                {
                    _androidVideoView.VideoView.SetVideoPath(file);
                    flag = true;
                }
            }
            else if (base.Element.Source is ResourceVideoSource)
            {
                string packageName = base.Context.PackageName;
                string path = (base.Element.Source as ResourceVideoSource).Path;
                if (!string.IsNullOrWhiteSpace(path))
                {
                    string str = Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
                    string uriString = "android.resource://" + packageName + "/raw/" + str;
                    _androidVideoView.VideoView.SetVideoURI(Android.Net.Uri.Parse(uriString));
                    flag = true;
                }
            }
            if (base.Element.StartPosition.TotalMilliseconds > 0.0)
            {
                _androidVideoView.VideoView.SeekTo((int)base.Element.StartPosition.TotalMilliseconds);
            }
            if (flag && base.Element.AutoPlay)
            {
                _androidVideoView.VideoView.Start();
            }
        }

        private void OnUpdateStatus(object sender, UpdateStatusEventArgs args)
        {
            VideoStatus videoStatus = VideoStatus.NotReady;
            if (_isInError)
            {
                videoStatus = VideoStatus.Error;
            }
            else if (isPrepared)
            {
                videoStatus = (_androidVideoView.VideoView.IsPlaying ? VideoStatus.Playing : VideoStatus.Paused);
                if (videoStatus == VideoStatus.Paused && _isReloadingAfterFullScreenToggle)
                {
                    videoStatus = VideoStatus.NotReady;
                }
                else
                {
                    _isReloadingAfterFullScreenToggle = false;
                }
            }
            if (videoStatus != VideoStatus.Paused || base.Element.Status != VideoStatus.Ended)
            {
                base.Element.Status = videoStatus;
            }
            TimeSpan position = TimeSpan.FromMilliseconds(_androidVideoView.VideoView.CurrentPosition);
            base.Element.Position = position;
        }

        private void OnPlayRequested(object sender, EventArgs args)
        {
            _androidVideoView.VideoView.Start();
        }

        private void OnPauseRequested(object sender, EventArgs args)
        {
            _androidVideoView.VideoView.Pause();
        }

        private void OnStopRequested(object sender, EventArgs args)
        {
            Stop();
        }

        private void Stop()
        {
            _androidVideoView.VideoView.SeekTo(1);
            _androidVideoView.VideoView.Pause();
        }

        private void BeforeFullScreen(object sender, EventArgs args)
        {
            PrepareFullScreen();
        }

        private void BeforeClosingFullScreen(object sender, EventArgs args)
        {
            PrepareFullScreenStop();
        }

        private void PrepareFullScreen()
        {
            base.Element.NativeViewHandleForTogglingFullScreen = _androidVideoView;
            _androidVideoView.VideoView.RemoveFromParent();
            Activity activity = base.Context as Activity;
            if (activity != null)
            {
                base.Element.HadAndroidFullscreenFlag = activity.Window.Attributes.Flags.HasFlag(WindowManagerFlags.Fullscreen);
                base.Element.HadAndroidForceNotFullscreen = activity.Window.Attributes.Flags.HasFlag(WindowManagerFlags.ForceNotFullscreen);
                activity.Window.AddFlags(WindowManagerFlags.Fullscreen);
                activity.Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);
            }
        }

        private void PrepareFullScreenStop()
        {
            base.Element.NativeViewHandleForTogglingFullScreen = _androidVideoView;
            _androidVideoView.VideoView.RemoveFromParent();
            Activity activity = base.Context as Activity;
            if (activity != null)
            {
                if (!base.Element.HadAndroidFullscreenFlag)
                {
                    activity.Window.ClearFlags(WindowManagerFlags.Fullscreen);
                }
                if (base.Element.HadAndroidForceNotFullscreen)
                {
                    activity.Window.AddFlags(WindowManagerFlags.ForceNotFullscreen);
                }
            }
        }

        private void OnSetPositionRequested(object sender, SetPositionEventArgs args)
        {
            int num = (int)args.NewPosition.TotalMilliseconds;
            int currentPosition = _androidVideoView.VideoView.CurrentPosition;
            if (Math.Abs(num - currentPosition) >= 100)
            {
                if (!_isWaitingForSeek)
                {
                    _isWaitingForSeek = true;
                    _androidVideoView.VideoView.SeekTo(num);
                }
                else
                {
                    _positionToSeekAfterWait = num;
                }
            }
        }

        private void OnMediaPlayerSeekComplete(object sender, EventArgs e)
        {
            int currentPosition = _androidVideoView.VideoView.CurrentPosition;
            if (_positionToSeekAfterWait.HasValue && _positionToSeekAfterWait != currentPosition)
            {
                _isWaitingForSeek = true;
                int value = _positionToSeekAfterWait.Value;
                _positionToSeekAfterWait = null;
                _androidVideoView.VideoView.SeekTo(value);
            }
            else
            {
                _positionToSeekAfterWait = null;
                _isWaitingForSeek = false;
            }
        }
    }
}
