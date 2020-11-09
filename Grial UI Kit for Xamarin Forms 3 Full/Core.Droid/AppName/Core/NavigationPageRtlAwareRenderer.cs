using Android.Content;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class NavigationPageRtlAwareRenderer : GrialNavigationPageRenderer
    {
        private bool _orientationHasChanged;

        public NavigationPageRtlAwareRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            base.OnElementChanged(e);
            UpdateLayoutDirection();
            LayoutDirectionService.Instance.LayoutDirectionChanged -= OnLayoutDirectionChanged;
            LayoutDirectionService.Instance.LayoutDirectionChanged += OnLayoutDirectionChanged;
        }

        protected override void Dispose(bool disposing)
        {
            LayoutDirectionService.Instance.LayoutDirectionChanged -= OnLayoutDirectionChanged;
            base.Dispose(disposing);
        }

        protected override void OnOrientationChanged(DeviceOrientation orientation)
        {
            base.OnOrientationChanged(orientation);
            _orientationHasChanged = true;
        }

        private void OnLayoutDirectionChanged(object sender, EventArgs e)
        {
            UpdateLayoutDirection();
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            if (_orientationHasChanged)
            {
                UpdateLayoutDirection();
                _orientationHasChanged = false;
            }
        }

        private void UpdateLayoutDirection()
        {
            if (base.Toolbar != null)
            {
                LayoutDirection layoutDirection = LayoutDirectionService.Instance.LayoutDirection;
                base.Toolbar.LayoutDirection = ((layoutDirection != 0) ? Android.Views.LayoutDirection.Rtl : Android.Views.LayoutDirection.Ltr);
                try
                {
                    for (int i = 0; i < base.Toolbar.ChildCount; i++)
                    {
                        base.Toolbar.GetChildAt(i).ForceLayout();
                    }
                }
                catch
                {
                }
            }
        }
    }
}
