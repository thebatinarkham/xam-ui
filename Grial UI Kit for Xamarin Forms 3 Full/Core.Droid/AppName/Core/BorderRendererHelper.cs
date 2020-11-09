using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Views;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    internal abstract class BorderRendererHelper<TControl> : IDisposable where TControl : Element
    {
        private bool disposed;

        private WeakReference<ShapeDrawable> _buttomLineShape;

        private WeakReference<GradientDrawable> _borderShape;

        private Context _context;

        private int? _width;

        private int? _height;

        private int? _originalLeftPadding;

        private int? _originalRightPadding;

        protected abstract Android.Views.View Renderer
        {
            get;
        }

        protected abstract Android.Views.View Control
        {
            get;
        }

        protected abstract TControl Element
        {
            get;
        }

        protected BorderRendererHelper(Context context)
        {
            _context = context;
        }

        protected float ToPixels(double dp)
        {
            return _context.ToPixels(dp);
        }

        protected abstract float GetHorizontalPadding(TControl control);

        protected abstract Xamarin.Forms.Color GetBackgroundColor(TControl control);

        protected abstract BorderStyle GetBorderStyle(TControl control);

        protected abstract Xamarin.Forms.Color GetBorderColor(TControl control);

        protected abstract float GetBorderWidth(TControl control);

        protected abstract float GetBorderCornerRadius(TControl control);

        protected abstract Xamarin.Forms.Color GetPlaceholderColor(TControl control);

        protected abstract void SetPlaceholderColor(Xamarin.Forms.Color color);

        protected abstract void RegisterFocusEvent(TControl control);

        protected abstract void UnregisterFocusEvent(TControl control);

        private float GetHorizontalPaddingInPixes(TControl control)
        {
            return GetHorizontalPadding(control);
        }

        private float GetBorderCornerRadiusInPixels(TControl control)
        {
            return ToPixels(GetBorderCornerRadius(control));
        }

        private float GetBorderWidthInPixels(TControl control)
        {
            return ToPixels(GetBorderWidth(control));
        }

        public void OnSizeChangedHandler(int w, int h, int oldw, int oldh)
        {
            _width = w;
            _height = h;
            if (_buttomLineShape != null)
            {
                UpdateBottomLine();
            }
        }

        public virtual void OnElementChangedHandler(ElementChangedEventArgs<TControl> e)
        {
            if (e.NewElement == null || Control == null)
            {
                _originalLeftPadding = (_originalRightPadding = null);
                return;
            }
            _originalLeftPadding = Control.PaddingLeft;
            _originalRightPadding = Control.PaddingRight;
            UpdateHorizontalPadding();
            UpdatePlaceholderColor(e.NewElement);
            if (e.OldElement != null)
            {
                UnregisterFocusEvent(e.OldElement);
            }
            switch (GetBorderStyle(e.NewElement))
            {
                case BorderStyle.Default:
                    if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                    {
                        Xamarin.Forms.Color borderColorInternal3 = GetBorderColorInternal(e.NewElement);
                        if (Control.Background != null && borderColorInternal3 != Xamarin.Forms.Color.Default)
                        {
                            RegisterFocusEvent(e.NewElement);
                        }
                    }
                    break;
                case BorderStyle.None:
                    Control.Background = null;
                    break;
                case BorderStyle.BottomLine:
                    {
                        Xamarin.Forms.Color borderColorInternal4 = GetBorderColorInternal(e.NewElement);
                        if (borderColorInternal4 != Xamarin.Forms.Color.Default)
                        {
                            ShapeDrawable shapeDrawable = new ShapeDrawable();
                            shapeDrawable.Paint.SetStyle(Paint.Style.Stroke);
                            shapeDrawable.Paint.Color = borderColorInternal4.ToAndroid();
                            shapeDrawable.Paint.StrokeWidth = GetBorderWidthInPixels(e.NewElement);
                            Control.Background = shapeDrawable;
                            _buttomLineShape = new WeakReference<ShapeDrawable>(shapeDrawable);
                        }
                        break;
                    }
                case BorderStyle.Rect:
                    {
                        Xamarin.Forms.Color borderColorInternal2 = GetBorderColorInternal(e.NewElement);
                        if (borderColorInternal2 != Xamarin.Forms.Color.Default)
                        {
                            GradientDrawable gradientDrawable2 = new GradientDrawable();
                            gradientDrawable2.SetStroke((int)GetBorderWidthInPixels(e.NewElement), borderColorInternal2.ToAndroid());
                            Control.Background = gradientDrawable2;
                            _borderShape = new WeakReference<GradientDrawable>(gradientDrawable2);
                        }
                        break;
                    }
                case BorderStyle.RoundRect:
                    {
                        Xamarin.Forms.Color borderColorInternal = GetBorderColorInternal(e.NewElement);
                        float borderCornerRadiusInPixels = GetBorderCornerRadiusInPixels(e.NewElement);
                        if (borderColorInternal != Xamarin.Forms.Color.Default || borderCornerRadiusInPixels > 0f)
                        {
                            Renderer.Background = null;
                            GradientDrawable gradientDrawable = new GradientDrawable();
                            gradientDrawable.SetStroke((int)GetBorderWidthInPixels(e.NewElement), borderColorInternal.ToAndroid());
                            gradientDrawable.SetCornerRadius(borderCornerRadiusInPixels);
                            gradientDrawable.SetColor(GetBackgroundColor(e.NewElement).ToAndroid());
                            int paddingTop = Control.PaddingTop;
                            int paddingBottom = Control.PaddingBottom;
                            int paddingLeft = Control.PaddingLeft;
                            int paddingRight = Control.PaddingRight;
                            Control.Background = gradientDrawable;
                            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                            {
                                Control.SetPadding(paddingLeft, paddingTop, paddingRight, paddingBottom);
                            }
                            _borderShape = new WeakReference<GradientDrawable>(gradientDrawable);
                        }
                        break;
                    }
            }
        }

        private Xamarin.Forms.Color GetBorderColorInternal(TControl element)
        {
            Xamarin.Forms.Color color = GetBorderColor(element);
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop && color == Xamarin.Forms.Color.Default)
            {
                color = ColorCache.AccentColor;
            }
            return color;
        }

        public virtual void OnElementPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "BorderColor"))
            {
                BorderStyle borderStyle = GetBorderStyle(Element);
                Xamarin.Forms.Color borderColorInternal = GetBorderColorInternal(Element);
                switch (borderStyle)
                {
                    case BorderStyle.Default:
                        if (Control.HasFocus && Control.Background != null)
                        {
                            Control.Background.SetColorFilter(borderColorInternal.ToAndroid(), PorterDuff.Mode.SrcAtop);
                        }
                        break;
                    case BorderStyle.BottomLine:
                        {
                            if (_buttomLineShape != null && _buttomLineShape.TryGetTarget(out ShapeDrawable target2))
                            {
                                target2.Paint.Color = borderColorInternal.ToAndroid();
                                target2.InvalidateSelf();
                            }
                            break;
                        }
                    case BorderStyle.Rect:
                    case BorderStyle.RoundRect:
                        {
                            if (_borderShape != null && _borderShape.TryGetTarget(out GradientDrawable target))
                            {
                                target.SetStroke((int)GetBorderWidthInPixels(Element), borderColorInternal.ToAndroid());
                                target.InvalidateSelf();
                            }
                            break;
                        }
                }
            }
            else if (string.Equals(e.PropertyName, "BorderWidth"))
            {
                switch (GetBorderStyle(Element))
                {
                    case BorderStyle.BottomLine:
                        {
                            if (_buttomLineShape != null && _buttomLineShape.TryGetTarget(out ShapeDrawable target4))
                            {
                                target4.Paint.StrokeWidth = GetBorderWidthInPixels(Element);
                                UpdateBottomLine();
                            }
                            break;
                        }
                    case BorderStyle.Rect:
                    case BorderStyle.RoundRect:
                        {
                            if (_borderShape != null && _borderShape.TryGetTarget(out GradientDrawable target3))
                            {
                                target3.SetStroke((int)GetBorderWidthInPixels(Element), GetBorderColorInternal(Element).ToAndroid());
                                target3.InvalidateSelf();
                            }
                            break;
                        }
                }
            }
            else if (string.Equals(e.PropertyName, "BorderCornerRadius"))
            {
                BorderStyle borderStyle2 = GetBorderStyle(Element);
                if (borderStyle2 == BorderStyle.RoundRect && _borderShape != null && _borderShape.TryGetTarget(out GradientDrawable target5))
                {
                    target5.SetCornerRadius(GetBorderCornerRadiusInPixels(Element));
                    target5.InvalidateSelf();
                }
            }
            else if (e.PropertyName == EntryProperties.PlaceholderColorProperty.PropertyName || e.PropertyName == EditorProperties.PlaceholderColorProperty.PropertyName || e.PropertyName == PickerProperties.PlaceholderColorProperty.PropertyName)
            {
                UpdatePlaceholderColor(Element);
            }
            else if (e.PropertyName == "BackgroundColor")
            {
                BorderStyle borderStyle3 = GetBorderStyle(Element);
                if (borderStyle3 == BorderStyle.RoundRect)
                {
                    Renderer.Background = null;
                    if (_borderShape != null && _borderShape.TryGetTarget(out GradientDrawable target6))
                    {
                        target6.SetColor(GetBackgroundColor(Element).ToAndroid());
                        target6.InvalidateSelf();
                    }
                }
            }
            else if (e.PropertyName == "HorizontalPadding")
            {
                UpdateHorizontalPadding();
            }
        }

        private void UpdateHorizontalPadding()
        {
            if (_originalLeftPadding.HasValue)
            {
                int num = (int)GetHorizontalPaddingInPixes(Element);
                Control.SetPadding(_originalLeftPadding.Value + num, Control.PaddingTop, _originalRightPadding.Value + num, Control.PaddingBottom);
            }
        }

        private void UpdatePlaceholderColor(TControl element)
        {
            Xamarin.Forms.Color placeholderColor = GetPlaceholderColor(element);
            if (placeholderColor != Xamarin.Forms.Color.Default)
            {
                SetPlaceholderColor(placeholderColor);
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                disposed = true;
                TControl element = Element;
                if (element != null)
                {
                    UnregisterFocusEvent(Element);
                }
            }
        }

        protected void OnFocused(object sender, FocusEventArgs e)
        {
            TControl element = Element;
            if (element != null && e.IsFocused)
            {
                Android.Views.View control = Control;
                Xamarin.Forms.Color borderColorInternal = GetBorderColorInternal(element);
                if (control != null && control.Background != null && borderColorInternal != Xamarin.Forms.Color.Default)
                {
                    control.Background.SetColorFilter(borderColorInternal.ToAndroid(), PorterDuff.Mode.SrcAtop);
                }
            }
        }

        protected void OnLostFocus(object sender, FocusEventArgs e)
        {
            if (!e.IsFocused)
            {
                Android.Views.View control = Control;
                if (control != null && control.Background != null)
                {
                    control.Background.ClearColorFilter();
                }
            }
        }

        private void UpdateBottomLine()
        {
            if (_width.HasValue && _height.HasValue)
            {
                int value = _height.Value;
                int value2 = _width.Value;
                if (_buttomLineShape.TryGetTarget(out ShapeDrawable target))
                {
                    Path path = new Path();
                    float y = (float)value - GetBorderWidthInPixels(Element) / 2f;
                    path.MoveTo(0f, y);
                    path.LineTo(value2, y);
                    target.Shape = new PathShape(path, value2, value);
                    target.InvalidateSelf();
                }
            }
        }
    }
}
