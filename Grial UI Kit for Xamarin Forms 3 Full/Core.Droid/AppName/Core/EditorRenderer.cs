using Android.Content;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class EditorRenderer : Xamarin.Forms.Platform.Android.EditorRenderer
    {
        private readonly EditorRendererHelper<FormsEditText> _helper;

        private int? _originalPadding;

        public EditorRenderer(Context context)
            : base(context)
        {
            _helper = new EditorRendererHelper<FormsEditText>(this, context);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            _helper.OnElementChangedHandler(e);
            if (e.NewElement != null && base.Control != null)
            {
                if (!_originalPadding.HasValue)
                {
                    _originalPadding = base.Control.PaddingLeft;
                }
                UpdateProperties();
            }
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            _helper.OnSizeChangedHandler(w, h, oldw, oldh);
        }

        protected override void Dispose(bool disposing)
        {
            _helper.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            _helper.OnElementPropertyChangedHandler(sender, e);
            if (e.PropertyName == EditorProperties.PlaceholderProperty.PropertyName)
            {
                UpdateGrialPlaceholder();
            }
            else if (e.PropertyName == EditorProperties.PlaceholderColorProperty.PropertyName)
            {
                UpdateGrialPlaceholderColor();
            }
            else if (e.PropertyName == EditorProperties.BorderStyleProperty.PropertyName)
            {
                UpdatePadding();
            }
        }

        private void UpdateProperties()
        {
            UpdateGrialPlaceholder();
            UpdateGrialPlaceholderColor();
            UpdatePadding();
        }

        private void UpdatePadding()
        {
            BorderStyle borderStyle = EditorProperties.GetBorderStyle(base.Element);
            int num = _originalPadding ?? 10;
            if (borderStyle == BorderStyle.Rect || borderStyle == BorderStyle.RoundRect)
            {
                num = Math.Max(10, num);
            }
            base.Control.SetPadding(num, base.Control.PaddingTop, num, base.Control.PaddingBottom);
        }

        private void UpdateGrialPlaceholder()
        {
            string placeholder = EditorProperties.GetPlaceholder(base.Element);
            if (!string.IsNullOrEmpty(placeholder))
            {
                base.Control.Hint = placeholder;
            }
        }

        private void UpdateGrialPlaceholderColor()
        {
            Color placeholderColor = EditorProperties.GetPlaceholderColor(base.Element);
            if (placeholderColor != (Color)EditorProperties.PlaceholderColorProperty.DefaultValue)
            {
                base.Control.SetHintTextColor(placeholderColor.ToAndroid());
            }
        }
    }
}
