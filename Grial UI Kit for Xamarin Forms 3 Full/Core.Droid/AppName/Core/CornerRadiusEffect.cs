using Android.Graphics;
using Android.OS;
using Android.Views;
using Xamarin.Forms;

namespace AppName.Core
{
    public class CornerRadiusEffect : BaseEffect
    {
        private class CornerRadiusOutlineProvider : ViewOutlineProvider
        {
            private readonly Element _element;

            public ViewOutlineProvider Inner
            {
                get;
            }

            public CornerRadiusOutlineProvider(Element element, ViewOutlineProvider inner)
            {
                _element = element;
                Inner = inner;
            }

            public override void GetOutline(Android.Views.View view, Outline outline)
            {
                Inner?.GetOutline(view, outline);
                float num = (float)Effects.GetCornerRadius(_element) * view.Resources.DisplayMetrics.Density;
                outline.SetRoundRect(new Rect(0, 0, view.Width, view.Height), (int)num);
            }
        }

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
            base.Container.OutlineProvider = new CornerRadiusOutlineProvider(base.Element, base.Container.OutlineProvider);
            base.Container.ClipToOutline = true;
        }

        protected override void OnDetachedInternal()
        {
            base.Container.ClipToOutline = false;
            CornerRadiusOutlineProvider cornerRadiusOutlineProvider = base.Container.OutlineProvider as CornerRadiusOutlineProvider;
            if (cornerRadiusOutlineProvider != null)
            {
                base.Container.OutlineProvider = cornerRadiusOutlineProvider.Inner;
            }
        }

        protected override void OnElementPropertyChangedInternal(string propertyName)
        {
            if (propertyName == Effects.CornerRadiusProperty.PropertyName)
            {
                base.Container.Invalidate();
            }
        }
    }
}
