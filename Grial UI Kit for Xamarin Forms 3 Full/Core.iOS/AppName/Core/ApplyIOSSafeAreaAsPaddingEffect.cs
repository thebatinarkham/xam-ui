using System;
using System.ComponentModel;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class ApplyIOSSafeAreaAsPaddingEffect : PlatformEffect
    {
        private class LayoutWrapper
        {
            private Layout _layout;

            private Page _page;

            public Thickness Padding
            {
                get
                {
                    return _layout?.Padding ?? _page.Padding;
                }
                set
                {
                    if (_layout != null)
                    {
                        _layout.Padding = value;
                    }
                    else
                    {
                        _page.Padding = value;
                    }
                }
            }

            public static LayoutWrapper BuildWrapper(Element element)
            {
                Layout layout = element as Layout;
                if (layout != null)
                {
                    return new LayoutWrapper
                    {
                        _layout = layout
                    };
                }
                Page page = element as Page;
                if (page != null)
                {
                    return new LayoutWrapper
                    {
                        _page = page
                    };
                }
                return null;
            }
        }

        private UIEdgeInsets _lastKnownInsets;

        private LayoutWrapper _layout;

        private Thickness _originalPadding;

        private bool _didChangePadding;

        internal static readonly BindableProperty ForceSafeAreaRecalculationCountProperty = BindableProperty.CreateAttached("__ForceSafeAreaRecalculationCount", typeof(int), typeof(Element), 0);

        private UIView View => base.Control ?? base.Container;

        internal static void SetForceSafeAreaRecalculationCount(BindableObject view, int value)
        {
            view.SetValue(ForceSafeAreaRecalculationCountProperty, value);
        }

        internal static int GetForceSafeAreaRecalculationCount(BindableObject view)
        {
            return (int)view.GetValue(ForceSafeAreaRecalculationCountProperty);
        }

        public ApplyIOSSafeAreaAsPaddingEffect()
        {
        }

        protected override void OnAttached()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                _didChangePadding = false;
                _lastKnownInsets = UIEdgeInsets.Zero;
                _layout = LayoutWrapper.BuildWrapper(base.Element);
                if (_layout != null)
                {
                    _originalPadding = _layout.Padding;
                }
                Setup(forced: false);
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
            if (View != null)
            {
                bool forced = args.PropertyName == Layout.PaddingProperty.PropertyName || args.PropertyName == Effects.IOSSafeAreaBottomSizeProperty.PropertyName || args.PropertyName == Effects.IOSSafeAreaTopSizeProperty.PropertyName || args.PropertyName == Effects.IOSSafeAreaLeftSizeProperty.PropertyName || args.PropertyName == Effects.IOSSafeAreaRightSizeProperty.PropertyName;
                bool forceDelay = args.PropertyName == ForceSafeAreaRecalculationCountProperty.PropertyName;
                Device.BeginInvokeOnMainThread(async delegate
                {
                    if (forceDelay)
                    {
                        await Task.Delay(new TimeSpan(0, 0, 0, 0, 1));
                    }
                    try
                    {
                        Setup(forced);
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                });
            }
        }

        private void Setup(bool forced)
        {
            if (View != null && _layout != null && (forced || View.SafeAreaInsets != _lastKnownInsets))
            {
                _lastKnownInsets = View.SafeAreaInsets;
                bool flag = _didChangePadding;
                IOSSafeArea applyIOSSafeAreaAsPadding = Effects.GetApplyIOSSafeAreaAsPadding(base.Element);
                if (applyIOSSafeAreaAsPadding != 0 && _lastKnownInsets != UIEdgeInsets.Zero)
                {
                    flag = false;
                    _didChangePadding = true;
                    _layout.Padding = new Thickness((((applyIOSSafeAreaAsPadding & IOSSafeArea.Left) != 0) ? GetValue(_lastKnownInsets.Left, Effects.GetIOSSafeAreaLeftSize(base.Element)) : 0.0) + _originalPadding.Left, (((applyIOSSafeAreaAsPadding & IOSSafeArea.Top) != 0) ? GetValue(_lastKnownInsets.Top, Effects.GetIOSSafeAreaTopSize(base.Element)) : 0.0) + _originalPadding.Top, (((applyIOSSafeAreaAsPadding & IOSSafeArea.Right) != 0) ? GetValue(_lastKnownInsets.Right, Effects.GetIOSSafeAreaRightSize(base.Element)) : 0.0) + _originalPadding.Right, (((applyIOSSafeAreaAsPadding & IOSSafeArea.Bottom) != 0) ? GetValue(_lastKnownInsets.Bottom, Effects.GetIOSSafeAreaBottomSize(base.Element)) : 0.0) + _originalPadding.Bottom);
                }
                if (flag)
                {
                    _layout.Padding = _originalPadding;
                    _didChangePadding = false;
                }
            }
        }

        private double GetValue(nfloat left, double? @override)
        {
            if (left == 0)
            {
                return 0.0;
            }
            return @override ?? ((double)left);
        }

        private void Restore()
        {
            try
            {
                if (_layout != null)
                {
                    if (_didChangePadding)
                    {
                        _layout.Padding = _originalPadding;
                    }
                    _layout = null;
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}
