using Android.Content;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    internal class EntryRendererHelper<TEditText> : BorderRendererHelper<Entry> where TEditText : EditText
    {
        private ViewRenderer<Entry, TEditText> _renderer;

        protected override Android.Views.View Renderer => (Android.Views.View)(object)_renderer;

        protected override Android.Views.View Control => _renderer.Control;

        protected override Entry Element => _renderer.Element;

        public EntryRendererHelper(ViewRenderer<Entry, TEditText> renderer, Context context)
            : base(context)
        {
            _renderer = renderer;
        }

        protected override float GetHorizontalPadding(Entry control)
        {
            return EntryProperties.GetHorizontalPadding(control);
        }

        protected override Color GetBackgroundColor(Entry control)
        {
            return control.BackgroundColor;
        }

        protected override Color GetBorderColor(Entry control)
        {
            return EntryProperties.GetBorderColor(control);
        }

        protected override float GetBorderCornerRadius(Entry control)
        {
            return EntryProperties.GetBorderCornerRadius(control);
        }

        protected override BorderStyle GetBorderStyle(Entry control)
        {
            return EntryProperties.GetBorderStyle(control);
        }

        protected override float GetBorderWidth(Entry control)
        {
            return EntryProperties.GetBorderWidth(control);
        }

        protected override Color GetPlaceholderColor(Entry control)
        {
            return EntryProperties.GetPlaceholderColor(control);
        }

        protected override void RegisterFocusEvent(Entry control)
        {
            control.Focused += base.OnFocused;
            control.Focused += base.OnLostFocus;
        }

        protected override void UnregisterFocusEvent(Entry control)
        {
            control.Focused -= base.OnFocused;
            control.Focused -= base.OnLostFocus;
        }

        protected override void SetPlaceholderColor(Color color)
        {
            if (Element.PlaceholderColor != color)
            {
                Element.PlaceholderColor = color;
            }
        }
    }
}
