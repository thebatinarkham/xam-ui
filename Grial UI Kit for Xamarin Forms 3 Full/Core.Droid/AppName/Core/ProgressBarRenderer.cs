using Android.Content;
using Android.Graphics;
using Android.OS;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class ProgressBarRenderer : Xamarin.Forms.Platform.Android.ProgressBarRenderer
    {
        public ProgressBarRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            base.OnElementChanged(e);
            if (base.Control == null)
            {
                return;
            }
            Xamarin.Forms.Color progressColor = GetProgressColor(base.Element);
            if (progressColor != Xamarin.Forms.Color.Default)
            {
                if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                {
                    SetColorFilter(e.NewElement.IsEnabled ? progressColor : ColorCache.GetDisabledColorFor(progressColor));
                    return;
                }
                Android.Graphics.Color color = progressColor.ToAndroid();
                Android.Graphics.Color disabled = ColorCache.GetDisabledColorFor(progressColor).ToAndroid();
                base.Control.ProgressBackgroundTintList = Utils.CreateColorStateList(Android.Graphics.Color.Gray);
                base.Control.ProgressBackgroundTintMode = PorterDuff.Mode.SrcAtop;
                base.Control.ProgressTintList = Utils.CreateColorStateList(color, disabled, color);
                base.Control.ProgressTintMode = PorterDuff.Mode.SrcAtop;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop && e.PropertyName == "IsEnabled")
            {
                Xamarin.Forms.Color progressColor = GetProgressColor(base.Element);
                if (progressColor != Xamarin.Forms.Color.Default)
                {
                    SetColorFilter(base.Element.IsEnabled ? progressColor : ColorCache.GetDisabledColorFor(progressColor));
                }
            }
        }

        private void SetColorFilter(Xamarin.Forms.Color tintColor)
        {
            if (base.Control.ProgressDrawable != null)
            {
                base.Control.ProgressDrawable.SetColorFilter(new PorterDuffColorFilter(tintColor.ToAndroid(), PorterDuff.Mode.SrcAtop));
            }
        }

        private Xamarin.Forms.Color GetProgressColor(ProgressBar progress)
        {
            Xamarin.Forms.Color color = ProgressBarProperties.GetTintColor(progress);
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop && color == Xamarin.Forms.Color.Default)
            {
                color = ColorCache.AccentColor;
            }
            return color;
        }
    }
}
