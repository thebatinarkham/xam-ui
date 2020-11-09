using Android.Content;
using Android.Views;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

namespace AppName.Core
{
    internal class PickerRendererHelper
    {
        private Action<GravityFlags> _setTextAlignment;

        public PickerRendererHelper(Action<GravityFlags> setTextAlignment)
        {
            _setTextAlignment = setTextAlignment;
        }

        public void OnElementPropertyChanged(Xamarin.Forms.View element, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PickerProperties.HorizontalTextAlignmentProperty.PropertyName)
            {
                UpdateTextAlignment(element);
            }
        }

        public void UpdateTextAlignment(Xamarin.Forms.View element)
        {
            switch (PickerProperties.GetHorizontalTextAlignment(element))
            {
                case Xamarin.Forms.TextAlignment.Start:
                    _setTextAlignment(GravityFlags.Start);
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    _setTextAlignment(GravityFlags.End);
                    break;
                case Xamarin.Forms.TextAlignment.Center:
                    _setTextAlignment(GravityFlags.AxisSpecified);
                    break;
            }
        }
    }
    internal class PickerRendererHelper<TPicker, TControl> : BorderRendererHelper<TPicker> where TPicker : Xamarin.Forms.View where TControl : Android.Views.View
    {
        private Action<GravityFlags> _setTextAlignment;

        private Xamarin.Forms.Platform.Android.ViewRenderer<TPicker, TControl> _renderer;

        private Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<TPicker, TControl> _rendererAppCompat;

        protected override Android.Views.View Renderer => (Android.Views.View)(object)(_renderer ?? _rendererAppCompat);

        protected override Android.Views.View Control => (_renderer != null) ? _renderer.Control : _rendererAppCompat.Control;

        protected override TPicker Element
        {
            get
            {
                if (_renderer == null)
                {
                    return _rendererAppCompat.Element;
                }
                return _renderer.Element;
            }
        }

        public PickerRendererHelper(Xamarin.Forms.Platform.Android.ViewRenderer<TPicker, TControl> renderer, Action<GravityFlags> setTextAlignment, Context context)
            : base(context)
        {
            _renderer = renderer;
            _setTextAlignment = setTextAlignment;
        }

        public PickerRendererHelper(Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<TPicker, TControl> renderer, Action<GravityFlags> setTextAlignment, Context context)
            : base(context)
        {
            _rendererAppCompat = renderer;
            _setTextAlignment = setTextAlignment;
        }

        protected override float GetHorizontalPadding(TPicker control)
        {
            return PickerProperties.GetHorizontalPadding(control);
        }

        public override void OnElementChangedHandler(ElementChangedEventArgs<TPicker> e)
        {
            if (e.NewElement != null && Control != null)
            {
                UpdateTextAlignment(e.NewElement);
                base.OnElementChangedHandler(e);
            }
        }

        public override void OnElementPropertyChangedHandler(object element, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PickerProperties.HorizontalTextAlignmentProperty.PropertyName)
            {
                UpdateTextAlignment((Xamarin.Forms.View)element);
            }
            else
            {
                base.OnElementPropertyChangedHandler(element, e);
            }
        }

        public void UpdateTextAlignment(Xamarin.Forms.View element)
        {
            switch (PickerProperties.GetHorizontalTextAlignment(element))
            {
                case Xamarin.Forms.TextAlignment.Start:
                    _setTextAlignment(GravityFlags.Start);
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    _setTextAlignment(GravityFlags.End);
                    break;
                case Xamarin.Forms.TextAlignment.Center:
                    _setTextAlignment(GravityFlags.AxisSpecified);
                    break;
            }
        }

        protected override Color GetBackgroundColor(TPicker control)
        {
            return control.BackgroundColor;
        }

        protected override Color GetBorderColor(TPicker control)
        {
            return PickerProperties.GetBorderColor(control);
        }

        protected override float GetBorderCornerRadius(TPicker control)
        {
            return PickerProperties.GetBorderCornerRadius(control);
        }

        protected override BorderStyle GetBorderStyle(TPicker control)
        {
            return PickerProperties.GetBorderStyle(control);
        }

        protected override float GetBorderWidth(TPicker control)
        {
            return PickerProperties.GetBorderWidth(control);
        }

        protected override Color GetPlaceholderColor(TPicker control)
        {
            return PickerProperties.GetPlaceholderColor(control);
        }

        protected override void RegisterFocusEvent(TPicker control)
        {
            control.Focused += base.OnFocused;
            control.Focused += base.OnLostFocus;
        }

        protected override void UnregisterFocusEvent(TPicker control)
        {
            control.Focused -= base.OnFocused;
            control.Focused -= base.OnLostFocus;
        }

        protected override void SetPlaceholderColor(Color color)
        {
            Picker picker = Element as Picker;
            if (picker != null && picker.TitleColor != color)
            {
                picker.TitleColor = color;
            }
        }
    }
}
