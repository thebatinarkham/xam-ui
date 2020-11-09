using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using System;
using System.ComponentModel;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

namespace AppName.Core
{
    public class GrialNavigationPageRenderer : NavigationPageRenderer
    {
        private bool _disposed;

        private MethodInfo _toolbarHeightMethod;

        private PropertyInfo _toolbarVisibleProperty;

        private LayerDrawable _pageBackgroundDrawable;

        private LayerDrawable _toolbarBackgroundDrawable;

        private ColorDrawable _pageBackgroundColor;

        private ColorDrawable _pageToolbarColor;

        private ColorDrawable _toolbarColor;

        private PaintDrawable _pageGradient;

        private PaintDrawable _toolbarGradient;

        private GradientChangeListener _listener;

        private Gradient _background;

        private PorterDuff.Mode _originalTintMode;

        private float _originalElevation;

        private bool _pageHasNavBar;

        private AppCompatTextView _textView;

        public Toolbar Toolbar
        {
            get;
            private set;
        }

        private IPageController PageController => base.Element;

        private bool IsTransparent => GrialNavigationPage.GetIsBarTransparent(base.Element);

        private bool NeedsToolbarBackground
        {
            get
            {
                if (!IsTransparent)
                {
                    return GrialNavigationPage.GetBarBackground(base.Element) != null;
                }
                return false;
            }
        }

        private bool NeedsPageBackground
        {
            get
            {
                if (_pageHasNavBar && !IsTransparent)
                {
                    return GrialNavigationPage.GetBarBackgroundHeight(base.Element) > 0.0;
                }
                return false;
            }
        }

        public GrialNavigationPageRenderer(Context context)
            : base(context)
        {
            _pageHasNavBar = true;
        }

        public override void AddView(Android.Views.View child)
        {
            base.AddView(child);
            Toolbar toolbar = child as Toolbar;
            if (toolbar != null)
            {
                if (Toolbar != null)
                {
                    Toolbar.ChildViewAdded -= OnToolbarChildViewAdded;
                }
                Toolbar = toolbar;
                Toolbar.ChildViewAdded += OnToolbarChildViewAdded;
                if (SupportsElevationAndTint())
                {
                    _originalTintMode = Toolbar.BackgroundTintMode;
                    _originalElevation = Toolbar.Elevation;
                }
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            bool? flag = IsToolbarVisible();
            if (flag.HasValue && _pageHasNavBar != flag.Value)
            {
                _pageHasNavBar = flag.Value;
                SetupBackground(forced: true);
            }
            if (!GrialNavigationPage.GetIsBarTransparent(base.Element))
            {
                base.OnLayout(changed, l, t, r, b);
                return;
            }
            int? toolbarHeight = GetToolbarHeight();
            if (toolbarHeight.HasValue)
            {
                base.OnLayout(changed, l, t, r, b + toolbarHeight.Value);
            }
            else
            {
                base.OnLayout(changed, l, t, r, b);
                PageController.ContainerArea = new Rectangle(0.0, 0.0, base.Context.FromPixels(r - l), base.Context.FromPixels(b - t));
            }
            for (int i = 0; i < ((ViewGroup)(object)this).ChildCount; i++)
            {
                Android.Views.View childAt = ((ViewGroup)(object)this).GetChildAt(i);
                if (!(childAt is Toolbar))
                {
                    childAt.Layout(0, 0, r, b);
                }
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            ClearBackground(disposing: false, forceOriginalRestore: true);
            base.OnElementChanged(e);
            if (base.Element != null)
            {
                SetupBackground(forced: false);
            }
            DeviceOrientationImpl.Instance.OrientationChanged -= OnOrientationChanged;
            DeviceOrientationImpl.Instance.OrientationChanged += OnOrientationChanged;
        }

        private void OnOrientationChanged(object sender, EventArgs e)
        {
            SetupBackground(forced: true);
            OnOrientationChanged(DeviceOrientationImpl.Instance.Orientation);
        }

        protected override void Dispose(bool disposing)
        {
            DeviceOrientationImpl.Instance.OrientationChanged -= OnOrientationChanged;
            if (!_disposed)
            {
                _disposed = true;
                if (Toolbar != null)
                {
                    Toolbar.ChildViewAdded -= OnToolbarChildViewAdded;
                }
                ClearBackground(disposing: true, forceOriginalRestore: false);
                base.Dispose(disposing);
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            UpdateToolbarProperties();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == GrialNavigationPage.BarBackgroundProperty.PropertyName || e.PropertyName == GrialNavigationPage.BarBackgroundHeightProperty.PropertyName || e.PropertyName == GrialNavigationPage.IsBarTransparentProperty.PropertyName)
            {
                SetupBackground(forced: true);
            }
            else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName || e.PropertyName == NavigationPage.BarBackgroundColorProperty.PropertyName)
            {
                UpdateSolidColors();
            }
            else if (e.PropertyName == GrialNavigationPage.TitleFontSizeProperty.PropertyName || e.PropertyName == GrialNavigationPage.TitleFontFamilyProperty.PropertyName || e.PropertyName == GrialNavigationPage.AndroidTitleBaselineCorrectionProperty.PropertyName)
            {
                UpdateFont();
            }
        }

        protected virtual void OnOrientationChanged(DeviceOrientation orientation)
        {
        }

        private void UpdateToolbarProperties()
        {
            int num = GetToolbarHeight() ?? Toolbar.Height;
            GrialNavigationBarExtensionsInternal.SetNavigationBarHeight(base.Element, base.Context.FromPixels(num));
            if ((IsTransparent || NeedsPageBackground) && SupportsElevationAndTint())
            {
                Toolbar.Elevation = 0f;
            }
            if (_toolbarBackgroundDrawable != null && Toolbar.Background != _toolbarBackgroundDrawable)
            {
                Toolbar.Background = _toolbarBackgroundDrawable;
            }
            if (IsTransparent)
            {
                Toolbar.SetBackgroundColor(Xamarin.Forms.Color.Transparent.ToAndroid());
            }
            if ((IsTransparent || NeedsToolbarBackground) && SupportsElevationAndTint())
            {
                Toolbar.BackgroundTintMode = null;
            }
            if (!NeedsPageBackground || base.ViewGroup.Height <= 0)
            {
                return;
            }
            double barBackgroundHeight = GrialNavigationPage.GetBarBackgroundHeight(base.Element);
            barBackgroundHeight = Math.Max(num, (int)base.Context.ToPixels(barBackgroundHeight));
            int b = base.ViewGroup.Height - (int)barBackgroundHeight;
            if (_pageBackgroundDrawable == null)
            {
                return;
            }
            for (int i = 0; i < _pageBackgroundDrawable.NumberOfLayers; i++)
            {
                Drawable drawable = _pageBackgroundDrawable.GetDrawable(i);
                if (_pageToolbarColor == drawable || _pageGradient == drawable)
                {
                    _pageBackgroundDrawable.SetLayerInset(i, 0, 0, 0, b);
                }
            }
        }

        private void SetupBackground(bool forced)
        {
            ClearBackground(disposing: false, forceOriginalRestore: false);
            if (IsTransparent)
            {
                UpdateToolbarProperties();
                return;
            }
            Gradient barBackground = GrialNavigationPage.GetBarBackground(base.Element);
            if (forced || _background != barBackground)
            {
                _background = barBackground;
                if (barBackground != null)
                {
                    _listener = new GradientChangeListener(barBackground);
                    _listener.Updated += OnGradientUpdated;
                    double barBackgroundHeight = GrialNavigationPage.GetBarBackgroundHeight(base.Element);
                    _toolbarGradient = GradientFactory.GetDrawable(barBackground, base.Context, barBackgroundHeight);
                    _toolbarColor = new ColorDrawable(base.Element.BarBackgroundColor.ToAndroid());
                    _toolbarBackgroundDrawable = new LayerDrawable(new Drawable[2]
                    {
                        _toolbarColor,
                        _toolbarGradient
                    });
                    Toolbar.Background = _pageBackgroundDrawable;
                    if (NeedsPageBackground)
                    {
                        _pageGradient = GradientFactory.GetDrawable(barBackground, base.Context, barBackgroundHeight);
                        _pageBackgroundColor = new ColorDrawable(base.Element.BackgroundColor.ToAndroid());
                        _pageToolbarColor = new ColorDrawable(base.Element.BarBackgroundColor.ToAndroid());
                        _pageBackgroundDrawable = new LayerDrawable(new Drawable[3]
                        {
                            _pageBackgroundColor,
                            _pageToolbarColor,
                            _pageGradient
                        });
                        base.ViewGroup.Background = _pageBackgroundDrawable;
                    }
                    UpdateToolbarProperties();
                    return;
                }
            }
            if (NeedsPageBackground)
            {
                _pageToolbarColor = new ColorDrawable(base.Element.BarBackgroundColor.ToAndroid());
                _pageBackgroundColor = new ColorDrawable(base.Element.BackgroundColor.ToAndroid());
                _pageBackgroundDrawable = new LayerDrawable(new Drawable[2]
                {
                    _pageBackgroundColor,
                    _pageToolbarColor
                });
                base.ViewGroup.Background = _pageBackgroundDrawable;
            }
            UpdateToolbarProperties();
        }

        private void UpdateSolidColors()
        {
            if (_pageBackgroundColor != null)
            {
                _pageBackgroundColor.Color = base.Element.BackgroundColor.ToAndroid();
            }
            if (_pageToolbarColor != null)
            {
                _pageToolbarColor.Color = base.Element.BarBackgroundColor.ToAndroid();
            }
            if (_toolbarColor != null)
            {
                _toolbarColor.Color = base.Element.BarBackgroundColor.ToAndroid();
            }
        }

        private void ClearBackground(bool disposing, bool forceOriginalRestore)
        {
            if (_listener != null)
            {
                _listener.Updated -= OnGradientUpdated;
            }
            if (!disposing)
            {
                if (Toolbar != null && (forceOriginalRestore || !NeedsToolbarBackground))
                {
                    Toolbar.SetBackgroundColor(base.Element.BarBackgroundColor.ToAndroid());
                    if (SupportsElevationAndTint())
                    {
                        Toolbar.BackgroundTintMode = _originalTintMode;
                    }
                }
                if (base.ViewGroup != null && (forceOriginalRestore || !NeedsPageBackground))
                {
                    base.ViewGroup.SetBackgroundColor(base.Element.BackgroundColor.ToAndroid());
                    if (Toolbar != null && SupportsElevationAndTint())
                    {
                        Toolbar.Elevation = _originalElevation;
                    }
                }
            }
            if (_pageBackgroundDrawable != null)
            {
                _pageBackgroundDrawable.Dispose();
                _pageBackgroundDrawable = null;
            }
            if (_toolbarBackgroundDrawable != null)
            {
                _toolbarBackgroundDrawable.Dispose();
                _toolbarBackgroundDrawable = null;
            }
            if (_pageGradient != null)
            {
                _pageGradient.Dispose();
                _pageGradient = null;
            }
            if (_toolbarGradient != null)
            {
                _toolbarGradient.Dispose();
                _toolbarGradient = null;
            }
            if (_toolbarColor != null)
            {
                _toolbarColor.Dispose();
                _toolbarColor = null;
            }
            if (_pageToolbarColor != null)
            {
                _pageToolbarColor.Dispose();
                _pageToolbarColor = null;
            }
            if (_pageBackgroundColor != null)
            {
                _pageBackgroundColor.Dispose();
                _pageBackgroundColor = null;
            }
            _listener = null;
            _background = null;
        }

        private void OnGradientUpdated(object sender, EventArgs e)
        {
            SetupBackground(forced: true);
        }

        private bool SupportsElevationAndTint()
        {
            return Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop;
        }

        private int? GetToolbarHeight()
        {
            if (_toolbarHeightMethod == null)
            {
                _toolbarHeightMethod = typeof(NavigationPageRenderer).GetMethod("GetNavBarHeight", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            }
            if (_toolbarHeightMethod != null)
            {
                try
                {
                    return (int)_toolbarHeightMethod.Invoke(this, null);
                }
                catch
                {
                }
            }
            return null;
        }

        private bool? IsToolbarVisible()
        {
            if (_toolbarVisibleProperty == null)
            {
                _toolbarVisibleProperty = typeof(NavigationPageRenderer).GetProperty("ToolbarVisible", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            }
            if (_toolbarVisibleProperty != null)
            {
                try
                {
                    return (bool)_toolbarVisibleProperty.GetValue(this);
                }
                catch
                {
                }
            }
            return null;
        }

        private void OnToolbarChildViewAdded(object sender, ViewGroup.ChildViewAddedEventArgs e)
        {
            AppCompatTextView appCompatTextView = e.Child as AppCompatTextView;
            if (appCompatTextView != null)
            {
                _textView = appCompatTextView;
                Toolbar.ChildViewAdded -= OnToolbarChildViewAdded;
                UpdateFont();
            }
        }

        private void UpdateFont()
        {
            if (_textView == null)
            {
                return;
            }
            string titleFontFamily = GrialNavigationPage.GetTitleFontFamily(base.Element);
            if (!string.IsNullOrEmpty(titleFontFamily))
            {
                Font self = Font.OfSize(titleFontFamily, 10.0);
                _textView.Typeface = self.ToTypeface();
                double titleFontSize = GrialNavigationPage.GetTitleFontSize(base.Element);
                if (titleFontSize > 0.0)
                {
                    _textView.SetTextSize(ComplexUnitType.Sp, (float)titleFontSize);
                }
                int androidTitleBaselineCorrection = GrialNavigationPage.GetAndroidTitleBaselineCorrection(base.Element);
                if (androidTitleBaselineCorrection >= 0)
                {
                    _textView.SetPadding(0, androidTitleBaselineCorrection, 0, 0);
                }
                else
                {
                    _textView.SetPadding(0, 0, 0, -androidTitleBaselineCorrection);
                }
            }
        }
    }
}
