using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class SwitchRenderer : Xamarin.Forms.Platform.Android.AppCompat.SwitchRenderer
    {
        public SwitchRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);
            if (base.Control == null)
            {
                return;
            }
            Xamarin.Forms.Color switchColor = GetSwitchColor(e.NewElement);
            if (switchColor != Xamarin.Forms.Color.Default)
            {
                if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                {
                    SetColorFilter(GetCurrentStateColor(e.NewElement, switchColor));
                    return;
                }
                Android.Graphics.Color color = switchColor.ToAndroid();
                ColorStateList tintList = new ColorStateList(new int[3][]
                {
                    new int[1]
                    {
                        16842912
                    },
                    new int[1]
                    {
                        -16842910
                    },
                    new int[0]
                }, new int[3]
                {
                    color,
                    Android.Graphics.Color.Gray,
                    Android.Graphics.Color.Gray
                });
                base.Control.TrackDrawable.SetTintList(tintList);
                base.Control.ThumbDrawable.SetColorFilter(new PorterDuffColorFilter(color, PorterDuff.Mode.Multiply));
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop && (e.PropertyName == "IsEnabled" || e.PropertyName == "IsToggled"))
            {
                Xamarin.Forms.Color switchColor = GetSwitchColor(base.Element);
                if (switchColor != Xamarin.Forms.Color.Default)
                {
                    SetColorFilter(GetCurrentStateColor(base.Element, switchColor));
                }
            }
        }

        private Xamarin.Forms.Color GetCurrentStateColor(Switch control, Xamarin.Forms.Color tint)
        {
            if (control.IsEnabled)
            {
                if (!control.IsToggled)
                {
                    return tint.WithLuminosity(tint.Luminosity * 1.5);
                }
                return tint;
            }
            return ColorCache.GetDisabledColorFor(tint);
        }

        private void SetColorFilter(Xamarin.Forms.Color tintColor)
        {
            if (base.Control.ThumbDrawable != null)
            {
                base.Control.ThumbDrawable.SetColorFilter(new PorterDuffColorFilter(tintColor.ToAndroid(), PorterDuff.Mode.SrcAtop));
            }
        }

        private Xamarin.Forms.Color GetSwitchColor(Switch control)
        {
            Xamarin.Forms.Color color = SwitchProperties.GetTintColor(control);
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop && color == Xamarin.Forms.Color.Default)
            {
                color = ColorCache.AccentColor;
            }
            return color;
        }
    }
}
