using Android.Content;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Droid
{
    /// <summary>
    /// This renderer forces a relayout on horizontal ScrollViewers as they do not resize
    /// their content after a device orientation change.
    /// </summary>
    public class ScrollViewRendererOrientationFix : ScrollViewRenderer
    {
        private DisplayOrientation _lastOrientation;

        public ScrollViewRendererOrientationFix(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            _lastOrientation = DeviceDisplay.MainDisplayInfo.Orientation;
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            var scrollView = Element as ScrollView;
            if (scrollView?.Orientation == ScrollOrientation.Vertical)
            {
                return;
            }

            if (DeviceDisplay.MainDisplayInfo.Orientation != _lastOrientation)
            {
                _lastOrientation = DeviceDisplay.MainDisplayInfo.Orientation;

                var originalPadding = scrollView.Padding;

                scrollView.Padding = new Thickness(originalPadding.Left + 1, originalPadding.Top, originalPadding.Right, originalPadding.Bottom);
                Device.BeginInvokeOnMainThread(() => scrollView.Padding = originalPadding);
            }
        }
    }
}
