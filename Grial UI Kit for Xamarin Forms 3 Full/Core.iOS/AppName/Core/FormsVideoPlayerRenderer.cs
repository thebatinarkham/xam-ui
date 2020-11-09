using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using System;
using System.ComponentModel;
using System.IO;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    internal class FormsVideoPlayerRenderer : ViewRenderer<FormsVideoPlayer, UIView>
    {
        private iOSVideoView _iOSVideoView;

        private bool atLeastOnePlay;

        private NSObject _videoEndNotificationToken;

        private bool _isInError;

        private AVPlayerItem _suscribedItem;

        protected override void OnElementChanged(ElementChangedEventArgs<FormsVideoPlayer> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                ReleaseIOSVideoView();
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
            try
            {
                AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback, AVAudioSessionCategoryOptions.DefaultToSpeaker);
                AVAudioSession.SharedInstance().SetActive(beActive: true);
            }
            catch
            {
            }
            iOSVideoView iOSVideoView = base.Element.NativeViewHandleForTogglingFullScreen as iOSVideoView;
            if (iOSVideoView == null)
            {
                flag = true;
                _iOSVideoView = new iOSVideoView();
                _iOSVideoView.PlayerViewController = new AVPlayerViewController();
                _iOSVideoView.Player = new AVPlayer();
                _iOSVideoView.PlayerViewController.Player = _iOSVideoView.Player;
            }
            else
            {
                _iOSVideoView = iOSVideoView;
            }
            base.Element.NativeViewHandleForTogglingFullScreen = null;
            SetNativeControl(_iOSVideoView.PlayerViewController.View);
            SetAreTransportControlsEnabled();
            if (flag)
            {
                SetSource();
            }
            else
            {
                SuscribeToErrors();
                SubscribeToVideoEnd();
            }
            SetMute();
            SetAspect();
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

        private void SubscribeToVideoEnd()
        {
            UnsuscribeFromVideoEnd();
            if (_iOSVideoView.PlayerItem != null)
            {
                _iOSVideoView.Player.ActionAtItemEnd = AVPlayerActionAtItemEnd.None;
                _videoEndNotificationToken = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, VideoDidFinishPlaying, _iOSVideoView.PlayerItem);
            }
        }

        private void UnsuscribeFromVideoEnd()
        {
            if (_videoEndNotificationToken != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_videoEndNotificationToken);
                _videoEndNotificationToken = null;
            }
        }

        private void SuscribeToErrors()
        {
            if (_suscribedItem != null)
            {
                UnsuscribeFromErrors();
            }
            _suscribedItem = _iOSVideoView?.Player?.CurrentItem;
            if (_suscribedItem != null)
            {
                _suscribedItem.AddObserver(this, "status", NSKeyValueObservingOptions.New, base.Handle);
                _suscribedItem.AddObserver(this, "error", NSKeyValueObservingOptions.New, base.Handle);
            }
        }

        private void UnsuscribeFromErrors()
        {
            if (_suscribedItem != null)
            {
                _suscribedItem.RemoveObserver(this, "status");
                _suscribedItem.RemoveObserver(this, "error");
                _suscribedItem = null;
            }
        }

        private void ReleaseIOSVideoView()
        {
            UnsuscribeFromErrors();
            UnsuscribeFromVideoEnd();
            if (base.Element != null && base.Element.NativeViewHandleForTogglingFullScreen != _iOSVideoView)
            {
                _iOSVideoView.Player.ReplaceCurrentItemWithPlayerItem(null);
            }
        }

        private void SetAspect()
        {
            _iOSVideoView.PlayerViewController.VideoGravity = AVLayerVideoGravity.ResizeAspect;
            _iOSVideoView.PlayerViewController.VideoGravity = AspectToGravity(base.Element.Aspect);
        }

        private AVLayerVideoGravity AspectToGravity(Aspect aspect)
        {
            switch (aspect)
            {
                case Aspect.Fill:
                    return AVLayerVideoGravity.Resize;
                case Aspect.AspectFill:
                    return AVLayerVideoGravity.ResizeAspectFill;
                default:
                    return AVLayerVideoGravity.ResizeAspect;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_iOSVideoView?.Player != null)
            {
                try
                {
                    ReleaseIOSVideoView();
                }
                catch
                {
                }
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

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
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
                SetMute();
            }
            else if (e.PropertyName == FormsVideoPlayer.AspectProperty.PropertyName)
            {
                _iOSVideoView.PlayerViewController.VideoGravity = AspectToGravity(base.Element.Aspect);
            }
            base.OnElementPropertyChanged(sender, e);
        }

        private void SetMute()
        {
            _iOSVideoView.Player.Muted = base.Element.Mute;
        }

        private void SetAreTransportControlsEnabled()
        {
            AVPlayerViewController aVPlayerViewController = _iOSVideoView?.PlayerViewController;
            if (aVPlayerViewController != null)
            {
                aVPlayerViewController.ShowsPlaybackControls = base.Element.AreTransportControlsEnabled;
            }
        }

        private void SetSource()
        {
            if (base.Element.Source == null)
            {
                return;
            }
            AVAsset aVAsset = null;
            if (base.Element.Source is UriVideoSource)
            {
                string uri = (base.Element.Source as UriVideoSource).Uri;
                if (!string.IsNullOrWhiteSpace(uri))
                {
                    aVAsset = AVAsset.FromUrl(new NSUrl(uri));
                }
            }
            else if (base.Element.Source is FileVideoSource)
            {
                string file = (base.Element.Source as FileVideoSource).File;
                if (!string.IsNullOrWhiteSpace(file))
                {
                    aVAsset = AVAsset.FromUrl(new NSUrl(file));
                }
            }
            else if (base.Element.Source is ResourceVideoSource)
            {
                string path = (base.Element.Source as ResourceVideoSource).Path;
                if (!string.IsNullOrWhiteSpace(path))
                {
                    string directoryName = Path.GetDirectoryName(path);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                    string fileExtension = Path.GetExtension(path).Substring(1);
                    aVAsset = AVAsset.FromUrl(NSBundle.MainBundle.GetUrlForResource(fileNameWithoutExtension, fileExtension, directoryName));
                }
            }
            if (aVAsset != null)
            {
                _iOSVideoView.PlayerItem = new AVPlayerItem(aVAsset);
            }
            else
            {
                _iOSVideoView.PlayerItem = null;
            }
            _iOSVideoView.Player.ReplaceCurrentItemWithPlayerItem(_iOSVideoView.PlayerItem);
            SuscribeToErrors();
            SubscribeToVideoEnd();
            if (base.Element.StartPosition.TotalMilliseconds > 0.0)
            {
                _iOSVideoView.Player.Seek(CMTime.FromSeconds(base.Element.StartPosition.TotalSeconds, 1));
            }
            atLeastOnePlay = false;
            if (_iOSVideoView.PlayerItem != null && base.Element.AutoPlay)
            {
                _iOSVideoView.Player.Play();
                atLeastOnePlay = true;
            }
        }

        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            if (_iOSVideoView == null || _iOSVideoView.PlayerItem == null || !object.Equals(ofObject, _iOSVideoView.Player.CurrentItem))
            {
                return;
            }
            if (keyPath.Equals((NSString)"status"))
            {
                if (_iOSVideoView.Player.Status == AVPlayerStatus.Failed)
                {
                    _isInError = true;
                    base.Element.RaiseError(new VideoPlayerErrorEventArgs(_iOSVideoView.PlayerItem.Error?.Description));
                }
            }
            else if (keyPath.Equals((NSString)"error"))
            {
                _isInError = true;
                base.Element.RaiseError(new VideoPlayerErrorEventArgs(_iOSVideoView.PlayerItem.Error?.Description));
            }
        }

        private bool IsStillInError()
        {
            if (_iOSVideoView.Player.Status != AVPlayerStatus.Failed)
            {
                AVPlayerItem currentItem = _iOSVideoView.Player.CurrentItem;
                if (currentItem == null)
                {
                    return false;
                }
                return currentItem.Status == AVPlayerItemStatus.Failed;
            }
            return true;
        }

        private void OnUpdateStatus(object sender, UpdateStatusEventArgs args)
        {
            VideoStatus status = VideoStatus.NotReady;
            if (_iOSVideoView.Player.Status == AVPlayerStatus.ReadyToPlay)
            {
                AVPlayerTimeControlStatus timeControlStatus = _iOSVideoView.Player.TimeControlStatus;
                AVPlayerTimeControlStatus num = timeControlStatus;
                if ((ulong)num <= 2uL)
                {
                    switch (num)
                    {
                        case AVPlayerTimeControlStatus.Playing:
                            status = VideoStatus.Playing;
                            _iOSVideoView.Player.Play();
                            break;
                        case AVPlayerTimeControlStatus.Paused:
                            status = ((base.Element.Status == VideoStatus.Ended) ? VideoStatus.Ended : VideoStatus.Paused);
                            break;
                        case AVPlayerTimeControlStatus.WaitingToPlayAtSpecifiedRate:
                            if (atLeastOnePlay)
                            {
                                _iOSVideoView.Player.Play();
                            }
                            break;
                    }
                }
            }
            if (_isInError && IsStillInError())
            {
                status = VideoStatus.Error;
            }
            else
            {
                _isInError = false;
            }
            base.Element.Status = status;
            if (_iOSVideoView.PlayerItem != null)
            {
                base.Element.Duration = ConvertTime(_iOSVideoView.PlayerItem.Duration);
                base.Element.Position = ConvertTime(_iOSVideoView.PlayerItem.CurrentTime);
            }
        }

        private void VideoDidFinishPlaying(NSNotification obj)
        {
            if (base.Element.Repeat)
            {
                _iOSVideoView.Player.Seek(new CMTime(0L, 1));
                return;
            }
            Stop();
            base.Element.Status = VideoStatus.Ended;
        }

        private TimeSpan ConvertTime(CMTime cmTime)
        {
            return TimeSpan.FromSeconds(double.IsNaN(cmTime.Seconds) ? 0.0 : cmTime.Seconds);
        }

        private void OnPlayRequested(object sender, EventArgs args)
        {
            _iOSVideoView.Player.Play();
            atLeastOnePlay = true;
        }

        private void OnPauseRequested(object sender, EventArgs args)
        {
            _iOSVideoView.Player.Pause();
        }

        private void OnStopRequested(object sender, EventArgs args)
        {
            Stop();
        }

        private void Stop()
        {
            _iOSVideoView.Player.Seek(new CMTime(0L, 1));
            _iOSVideoView.Player.Pause();
        }

        private void OnSetPositionRequested(object sender, SetPositionEventArgs args)
        {
            _iOSVideoView.Player.Seek(CMTime.FromSeconds(args.NewPosition.TotalSeconds, 1), CMTime.Zero, CMTime.Zero);
        }

        private void BeforeClosingFullScreen(object sender, EventArgs e)
        {
            PrepareFullScreenStop();
        }

        private void BeforeFullScreen(object sender, EventArgs e)
        {
            PrepareFullScreen();
        }

        private void PrepareFullScreen()
        {
            base.Element.NativeViewHandleForTogglingFullScreen = _iOSVideoView;
        }

        private void PrepareFullScreenStop()
        {
            base.Element.NativeViewHandleForTogglingFullScreen = _iOSVideoView;
        }
    }
}
