using Android.Content;
using Android.Graphics;
using Android.OS;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class ActivityIndicatorRenderer : Xamarin.Forms.Platform.Android.ActivityIndicatorRenderer
    {
        public ActivityIndicatorRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicator> e)
        {
            base.OnElementChanged(e);
            if (base.Control == null)
            {
                return;
            }
            Xamarin.Forms.Color color = e.NewElement.Color;
            if (!(color != Xamarin.Forms.Color.Default))
            {
                return;
            }
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                if (base.Control.IndeterminateDrawable != null)
                {
                    SetColorFilter(e.NewElement.IsEnabled ? color : ColorCache.GetDisabledColorFor(color));
                }
            }
            else
            {
                Android.Graphics.Color color2 = color.ToAndroid();
                Android.Graphics.Color disabled = ColorCache.GetDisabledColorFor(color).ToAndroid();
                base.Control.IndeterminateTintList = Utils.CreateColorStateList(color2, disabled, color2);
                base.Control.IndeterminateTintMode = PorterDuff.Mode.SrcAtop;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop && e.PropertyName == "IsEnabled")
            {
                Xamarin.Forms.Color color = base.Element.Color;
                if (color != Xamarin.Forms.Color.Default)
                {
                    SetColorFilter(base.Element.IsEnabled ? color : ColorCache.GetDisabledColorFor(color));
                }
            }
        }

        private void SetColorFilter(Xamarin.Forms.Color tintColor)
        {
            base.Control.IndeterminateDrawable.SetColorFilter(new PorterDuffColorFilter(tintColor.ToAndroid(), PorterDuff.Mode.Multiply));
        }
    }
}
