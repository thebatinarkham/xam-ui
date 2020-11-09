using CoreAnimation;
using CoreGraphics;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class GrialNavigationPageRenderer : NavigationRenderer
    {
        private CAGradientLayer _barBackground;

        private CAGradientLayer _extendedBarBackground;

        private CALayer _extendedBarSolidBackground;

        private GradientChangeListener _listener;

        private Gradient _background;

        private UIImage _shadowImage;

        private bool _disposed;

        private bool _orientationDidChange;

        private bool _pageHasNavBar;

        private DeviceOrientation _lastOrientation;

        private bool _is13orLater;

        private bool _doesSafeAreaConceptExist;

        private IPageController PageController => base.Element as IPageController;

        private bool IsTransparent => GrialNavigationPage.GetIsBarTransparent(base.Element as NavigationPage);

        public GrialNavigationPageRenderer()
        {
            _pageHasNavBar = true;
            _is13orLater = UIDevice.CurrentDevice.CheckSystemVersion(13, 0);
            _doesSafeAreaConceptExist = UIDevice.CurrentDevice.CheckSystemVersion(11, 0);
            _lastOrientation = ResponsiveHelper.Orientation.Orientation;
        }

        public override void ViewLayoutMarginsDidChange()
        {
            base.ViewLayoutMarginsDidChange();
            _orientationDidChange = true;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            if (_lastOrientation != ResponsiveHelper.Orientation.Orientation)
            {
                _lastOrientation = ResponsiveHelper.Orientation.Orientation;
                _orientationDidChange = true;
            }
            if (_is13orLater)
            {
                if (_barBackground != null)
                {
                    CALayer layer = NavigationBar.Layer;
                    int num;
                    if (layer == null)
                    {
                        num = 0;
                    }
                    else
                    {
                        CALayer[] sublayers = layer.Sublayers;
                        num = ((((sublayers != null) ? new int?(sublayers.Length) : null) == 2) ? 1 : 0);
                    }
                    if (num != 0)
                    {
                        NavigationBar.Layer.Sublayers[0].InsertSublayer(_barBackground, 2);
                    }
                }
            }
            else if (_barBackground != null)
            {
                CALayer layer2 = NavigationBar.Layer;
                if (layer2 != null && layer2.Sublayers.Length == 4)
                {
                    NavigationBar.Layer.InsertSublayer(_barBackground, 1);
                }
            }
            bool flag = PageController.ContainerArea.Height < (double)Toolbar.Frame.Height;
            if (flag != _pageHasNavBar)
            {
                _pageHasNavBar = flag;
                SetupBackground();
                _orientationDidChange = false;
            }
            else if (_orientationDidChange)
            {
                _orientationDidChange = false;
                UpdateBackgroundSize();
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (IsTransparent)
            {
                NavigationBar.Translucent = true;
                NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
                NavigationBar.ShadowImage = new UIImage();
            }
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            ClearBackground(disposing: false, clearSolidToo: true);
            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;
            }
            base.OnElementChanged(e);
            _shadowImage = NavigationBar.ShadowImage;
            if (base.Element != null)
            {
                base.Element.PropertyChanged -= OnElementPropertyChanged;
                base.Element.PropertyChanged += OnElementPropertyChanged;
                SetupBackground();
                UpdateFont();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            if (disposing)
            {
                if (base.Element != null)
                {
                    base.Element.PropertyChanged -= OnElementPropertyChanged;
                }
                ClearBackground(disposing: true, clearSolidToo: true);
            }
            base.Dispose(disposing);
        }

        private void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GrialNavigationPage.BarBackgroundProperty.PropertyName || e.PropertyName == GrialNavigationPage.IsBarTransparentProperty.PropertyName || e.PropertyName == NavigationPage.BarBackgroundColorProperty.PropertyName)
            {
                SetupBackground();
            }
            else if (e.PropertyName == GrialNavigationPage.BarBackgroundHeightProperty.PropertyName)
            {
                UpdateBackgroundSize();
            }
            else if (e.PropertyName == GrialNavigationPage.TitleFontSizeProperty.PropertyName || e.PropertyName == GrialNavigationPage.TitleFontFamilyProperty.PropertyName)
            {
                UpdateFont();
            }
        }

        private void SetupBackground()
        {
            bool isTransparent = IsTransparent;
            Gradient gradient = isTransparent ? null : GrialNavigationPage.GetBarBackground(base.Element as NavigationPage);
            if (_background != gradient)
            {
                ClearBackground(disposing: false, clearSolidToo: false);
                _background = gradient;
                if (gradient != null)
                {
                    _listener = new GradientChangeListener(gradient);
                    _listener.Updated += OnGradientUpdated;
                    _barBackground = GradientFactory.GetLayer(gradient);
                    CALayer cALayer = new CALayer();
                    cALayer.BackgroundColor = UIColor.Black.CGColor;
                    _barBackground.Mask = cALayer;
                    _extendedBarBackground = GradientFactory.GetLayer(gradient);
                    int index = 0;
                    for (int i = 0; i < View.Layer.Sublayers.Length; i++)
                    {
                        if (View.Layer.Sublayers[i] == _extendedBarSolidBackground)
                        {
                            index = i + 1;
                            break;
                        }
                    }
                    View.Layer.InsertSublayer(_extendedBarBackground, index);
                }
            }
            if (_extendedBarSolidBackground == null)
            {
                _extendedBarSolidBackground = new CALayer();
                View.Layer.InsertSublayer(_extendedBarSolidBackground, 0);
            }
            if (isTransparent || !_pageHasNavBar)
            {
                _extendedBarSolidBackground.BackgroundColor = Color.Transparent.ToCGColor();
                if (_extendedBarBackground != null)
                {
                    _extendedBarBackground.Opacity = 0f;
                }
            }
            else
            {
                Color barBackgroundColor = (base.Element as NavigationPage).BarBackgroundColor;
                _extendedBarSolidBackground.BackgroundColor = barBackgroundColor.ToCGColor();
                if (_extendedBarBackground != null)
                {
                    _extendedBarBackground.Opacity = 1f;
                }
            }
            UpdateBackgroundSize();
        }

        private void ClearBackground(bool disposing, bool clearSolidToo)
        {
            if (_listener != null)
            {
                _listener.Updated -= OnGradientUpdated;
            }
            if (_barBackground != null)
            {
                _barBackground.RemoveFromSuperLayer();
                _barBackground.Dispose();
                _barBackground = null;
            }
            if (_extendedBarBackground != null)
            {
                _extendedBarBackground.RemoveFromSuperLayer();
                _extendedBarBackground.Dispose();
                _extendedBarBackground = null;
            }
            if (clearSolidToo && _extendedBarSolidBackground != null)
            {
                _extendedBarSolidBackground.RemoveFromSuperLayer();
                _extendedBarSolidBackground.Dispose();
                _extendedBarSolidBackground = null;
            }
            _listener = null;
            _background = null;
            if (!disposing)
            {
                NavigationBar.ShadowImage = _shadowImage;
            }
        }

        private nfloat GetNavHeight()
        {
            nfloat height = UIApplication.SharedApplication.StatusBarFrame.Height;
            nfloat height2 = NavigationBar.Frame.Height;
            if (_doesSafeAreaConceptExist)
            {
                UIEdgeInsets safeAreaInsets = UIApplication.SharedApplication.Windows[0].SafeAreaInsets;
                if (safeAreaInsets.Top > 0)
                {
                    return safeAreaInsets.Top + height2;
                }
            }
            return height + height2;
        }

        private nfloat GetNavTop()
        {
            if (_doesSafeAreaConceptExist)
            {
                UIEdgeInsets safeAreaInsets = UIApplication.SharedApplication.Windows[0].SafeAreaInsets;
                if (safeAreaInsets.Top > 0)
                {
                    return -safeAreaInsets.Top;
                }
            }
            return -UIApplication.SharedApplication.StatusBarFrame.Height;
        }

        private void UpdateBackgroundSize()
        {
            nfloat navHeight = GetNavHeight();
            GrialNavigationBarExtensionsInternal.SetNavigationBarHeight(base.Element as NavigationPage, navHeight);
            double barBackgroundHeight = GrialNavigationPage.GetBarBackgroundHeight(base.Element as NavigationPage);
            if (barBackgroundHeight > 0.0 || IsTransparent)
            {
                if (NavigationBar.ShadowImage == _shadowImage)
                {
                    NavigationBar.ShadowImage = new UIImage();
                }
            }
            else
            {
                NavigationBar.ShadowImage = _shadowImage;
            }
            barBackgroundHeight = ((barBackgroundHeight <= 0.0) ? ((double)navHeight) : Math.Max(navHeight, barBackgroundHeight));
            CGRect frame = new CGRect(0.0, 0.0, NavigationBar.Frame.Width, barBackgroundHeight);
            if (_extendedBarSolidBackground != null)
            {
                _extendedBarSolidBackground.Frame = frame;
            }
            if (_barBackground != null)
            {
                _extendedBarBackground.Frame = frame;
                nfloat navTop = GetNavTop();
                _barBackground.Frame = new CGRect(0.0, _is13orLater ? ((nfloat)0) : navTop, NavigationBar.Frame.Width, barBackgroundHeight);
                _barBackground.Mask.Frame = new CGRect(0, navTop, NavigationBar.Frame.Width, navHeight - navTop);
                GradientFactory.UpdateRadialRadius(_background, _barBackground);
                GradientFactory.UpdateRadialRadius(_background, _extendedBarBackground);
            }
        }

        private void OnGradientUpdated(object sender, EventArgs e)
        {
            GradientFactory.UpdateLayer(_background, _barBackground);
            GradientFactory.UpdateLayer(_background, _extendedBarBackground);
        }

        private void UpdateFont()
        {
            NavigationPage page = base.Element as NavigationPage;
            string titleFontFamily = GrialNavigationPage.GetTitleFontFamily(page);
            nfloat nfloat = (nfloat)GrialNavigationPage.GetTitleFontSize(page);
            UIStringAttributes titleTextAttributes = NavigationBar.TitleTextAttributes;
            if (titleTextAttributes != null && !string.IsNullOrEmpty(titleFontFamily))
            {
                if (nfloat <= 0)
                {
                    nfloat = (titleTextAttributes.Font?.PointSize ?? ((nfloat)18));
                }
                titleTextAttributes.Font = UIFont.FromName(titleFontFamily, nfloat);
                NavigationBar.TitleTextAttributes.Font = null;
                NavigationBar.TitleTextAttributes = titleTextAttributes;
            }
        }
    }
}
