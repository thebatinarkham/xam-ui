using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class PickerRenderer : Xamarin.Forms.Platform.iOS.PickerRenderer
    {
        private readonly PickerRendererHelper _helper;

        public PickerRenderer()
        {
            _helper = new PickerRendererHelper(SetPlaceholderColor, SetBorderStyle, SetTextAlignment, () => base.Control);
        }

        private void SetTextAlignment(UITextAlignment textAlignment)
        {
            base.Control.TextAlignment = textAlignment;
        }

        private void SetPlaceholderColor(Color color)
        {
            if (base.Element.TitleColor != color)
            {
                base.Element.TitleColor = color;
            }
        }

        private void SetBorderStyle(UITextBorderStyle style)
        {
            base.Control.BorderStyle = style;
            base.Control.ClipsToBounds = (style == UITextBorderStyle.RoundedRect);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && base.Control != null)
            {
                _helper.UpdatePlaceholderColor(e.NewElement);
                _helper.SetupBorderProperties(e.NewElement, base.Control);
                _helper.UpdateTextAlignment(e.NewElement);
                _helper.UpdateHorizontalPadding(e.NewElement);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            _helper.OnElementPropertyChanged(base.Element, base.Control, e);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            _helper.LayoutSubviews(base.Element, base.Control);
        }
    }
}
