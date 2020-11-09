using Android.Content;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    internal class EditorRendererHelper<TEditText> : BorderRendererHelper<Editor> where TEditText : EditText
    {
        private ViewRenderer<Editor, TEditText> _renderer;

        protected override Android.Views.View Renderer => (Android.Views.View)(object)_renderer;

        protected override Android.Views.View Control => _renderer.Control;

        protected override Editor Element => _renderer.Element;

        public EditorRendererHelper(ViewRenderer<Editor, TEditText> renderer, Context context, bool honorPlaceholderProperties = true)
            : base(context)
        {
            _renderer = renderer;
        }

        protected override float GetHorizontalPadding(Editor control)
        {
            return EditorProperties.GetHorizontalPadding(control);
        }

        protected override Color GetBackgroundColor(Editor control)
        {
            return control.BackgroundColor;
        }

        protected override Color GetBorderColor(Editor control)
        {
            return EditorProperties.GetBorderColor(control);
        }

        protected override float GetBorderCornerRadius(Editor control)
        {
            return EditorProperties.GetBorderCornerRadius(control);
        }

        protected override BorderStyle GetBorderStyle(Editor control)
        {
            return EditorProperties.GetBorderStyle(control);
        }

        protected override float GetBorderWidth(Editor control)
        {
            return EditorProperties.GetBorderWidth(control);
        }

        protected override Color GetPlaceholderColor(Editor control)
        {
            return EditorProperties.GetPlaceholderColor(control);
        }

        protected override void RegisterFocusEvent(Editor control)
        {
            control.Focused += base.OnFocused;
            control.Focused += base.OnLostFocus;
        }

        protected override void UnregisterFocusEvent(Editor control)
        {
            control.Focused -= base.OnFocused;
            control.Focused -= base.OnLostFocus;
        }

        protected override void SetPlaceholderColor(Color color)
        {
        }
    }
}
