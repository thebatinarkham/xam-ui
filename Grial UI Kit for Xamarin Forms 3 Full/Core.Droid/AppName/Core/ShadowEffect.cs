using Android.Graphics;
using Android.OS;
using Android.Views;
using Xamarin.Forms;

namespace AppName.Core
{
    public class ShadowEffect : BaseEffect
    {
        private class ShadowOutlineProvider : ViewOutlineProvider
        {
            private readonly Element _element;

            public ViewOutlineProvider Inner
            {
                get;
            }

            public ShadowOutlineProvider(Element element, ViewOutlineProvider inner)
            {
                _element = element;
                Inner = inner;
            }

            public override void GetOutline(Android.Views.View view, Outline outline)
            {
                Inner?.GetOutline(view, outline);
                Button button = _element as Button;
                if (button != null)
                {
                    float num = (float)button.CornerRadius * view.Resources.DisplayMetrics.Density;
                    if (num < 0f)
                    {
                        num = 0f;
                    }
                    outline.SetRoundRect(new Rect(0, 0, view.Width, view.Height), (int)num);
                }
                outline.Alpha = Effects.GetShadowOpacity(_element);
            }
        }

        private float _originalElevation;

        protected override bool CanBeApplied()
        {
            if (base.Container != null)
            {
                return Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop;
            }
            return false;
        }

        protected override void OnAttachedInternal()
        {
            _originalElevation = base.Container.Elevation;
            UpdateShadow();
        }

        protected override void OnDetachedInternal()
        {
            base.Container.Elevation = _originalElevation;
            ShadowOutlineProvider shadowOutlineProvider = base.Container.OutlineProvider as ShadowOutlineProvider;
            if (shadowOutlineProvider != null)
            {
                base.Container.OutlineProvider = shadowOutlineProvider.Inner;
            }
        }

        protected override void OnElementPropertyChangedInternal(string propertyName)
        {
            if (propertyName == Effects.ShadowProperty.PropertyName || propertyName == Effects.ShadowSizeProperty.PropertyName)
            {
                UpdateShadow();
            }
            else if (propertyName == Button.CornerRadiusProperty.PropertyName || propertyName == Effects.ShadowOpacityProperty.PropertyName)
            {
                if (!(base.Container.OutlineProvider is ShadowOutlineProvider))
                {
                    UpdateShadow();
                }
                else
                {
                    base.Container.InvalidateOutline();
                }
            }
        }

        private void UpdateShadow()
        {
            float shadowSize = Effects.GetShadowSize(base.Element);
            if (!(shadowSize < 0f))
            {
                base.Container.Elevation = shadowSize * base.Container.Resources.DisplayMetrics.Density;
                if (!(base.Container.OutlineProvider is ShadowOutlineProvider))
                {
                    base.Container.OutlineProvider = new ShadowOutlineProvider(base.Element, base.Container.OutlineProvider);
                }
            }
        }
    }
}
