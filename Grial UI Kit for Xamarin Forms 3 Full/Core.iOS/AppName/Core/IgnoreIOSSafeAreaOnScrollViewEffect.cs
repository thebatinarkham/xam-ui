using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class IgnoreIOSSafeAreaOnScrollViewEffect : PlatformEffect
    {
        private UIEdgeInsets _lastKnownInsets;

        private UIScrollView _uiScrollView;

        private ScrollView _scrollView;

        private Thickness _originalPadding;

        private UIScrollViewContentInsetAdjustmentBehavior _originalInsetBehavior;

        private bool _didChangePadding;

        private bool _didChangeInsetBehavior;

        protected override void OnAttached()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                _didChangePadding = false;
                _didChangeInsetBehavior = false;
                _lastKnownInsets = UIEdgeInsets.Zero;
                _uiScrollView = (base.Control as UIScrollView);
                if (_uiScrollView != null)
                {
                    _originalInsetBehavior = _uiScrollView.ContentInsetAdjustmentBehavior;
                }
                _scrollView = (base.Element as ScrollView);
                if (_scrollView != null)
                {
                    _originalPadding = _scrollView.Padding;
                }
                Setup();
            }
        }

        protected override void OnDetached()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                Restore();
            }
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (_uiScrollView != null && (_uiScrollView.SafeAreaInsets != _lastKnownInsets || args.PropertyName == "Renderer"))
            {
                Device.BeginInvokeOnMainThread(delegate
                {
                    try
                    {
                        Setup();
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                });
            }
        }

        private void Setup()
        {
            if (_uiScrollView == null || _scrollView == null || !(_uiScrollView.SafeAreaInsets != _lastKnownInsets))
            {
                return;
            }
            _lastKnownInsets = _uiScrollView.SafeAreaInsets;
            bool flag = _didChangePadding;
            bool flag2 = _didChangeInsetBehavior;
            IOSSafeArea ignoreIOSSafeAreaOnScrollView = Effects.GetIgnoreIOSSafeAreaOnScrollView(base.Element);
            if (ignoreIOSSafeAreaOnScrollView != 0 && _lastKnownInsets != UIEdgeInsets.Zero)
            {
                _didChangeInsetBehavior = true;
                flag2 = false;
                _uiScrollView.ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
                _didChangePadding = true;
                flag = false;
                _scrollView.Padding = new Thickness((double)(((ignoreIOSSafeAreaOnScrollView & IOSSafeArea.Left) == IOSSafeArea.None) ? _lastKnownInsets.Left : ((nfloat)0)) + _originalPadding.Left, (double)(((ignoreIOSSafeAreaOnScrollView & IOSSafeArea.Top) == IOSSafeArea.None) ? _lastKnownInsets.Top : ((nfloat)0)) + _originalPadding.Top, (double)(((ignoreIOSSafeAreaOnScrollView & IOSSafeArea.Right) == IOSSafeArea.None) ? _lastKnownInsets.Right : ((nfloat)0)) + _originalPadding.Right, (double)(((ignoreIOSSafeAreaOnScrollView & IOSSafeArea.Bottom) == IOSSafeArea.None) ? _lastKnownInsets.Bottom : ((nfloat)0)) + _originalPadding.Bottom);
                if (_scrollView.Content != null)
                {
                    ApplyIOSSafeAreaAsPaddingEffect.SetForceSafeAreaRecalculationCount(_scrollView.Content, ApplyIOSSafeAreaAsPaddingEffect.GetForceSafeAreaRecalculationCount(_scrollView.Content) + 1);
                }
            }
            if (flag)
            {
                _scrollView.Padding = _originalPadding;
                _didChangePadding = false;
            }
            if (flag2)
            {
                _uiScrollView.ContentInsetAdjustmentBehavior = _originalInsetBehavior;
                _didChangeInsetBehavior = false;
            }
        }

        private void Restore()
        {
            try
            {
                if (_uiScrollView != null)
                {
                    if (_didChangeInsetBehavior)
                    {
                        _uiScrollView.ContentInsetAdjustmentBehavior = _originalInsetBehavior;
                    }
                    _uiScrollView = null;
                }
                if (_scrollView != null)
                {
                    if (_didChangePadding)
                    {
                        _scrollView.Padding = _originalPadding;
                    }
                    _scrollView = null;
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}
