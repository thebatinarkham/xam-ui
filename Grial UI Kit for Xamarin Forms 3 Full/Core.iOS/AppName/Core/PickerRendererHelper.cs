using CoreAnimation;
using CoreGraphics;
using System;
using System.ComponentModel;
using UIKit;
using AppName.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    internal class PickerRendererHelper
    {
        private CALayer _borderLayer;

        private Action<Color> _setPlaceholderColor;

        private Action<UITextBorderStyle> _setBorderStyle;

        private Action<UITextAlignment> _setTextAlignment;

        private Func<UITextField> _getTextField;

        public PickerRendererHelper(Action<Color> setPlaceholderColor, Action<UITextBorderStyle> setBorderStyle, Action<UITextAlignment> setTextAlignment, Func<UITextField> getTextField)
        {
            _setPlaceholderColor = setPlaceholderColor;
            _setBorderStyle = setBorderStyle;
            _setTextAlignment = setTextAlignment;
            _getTextField = getTextField;
        }

        public void OnElementPropertyChanged(View element, UIView control, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PickerProperties.BorderColorProperty.PropertyName)
            {
                BorderStyle borderStyle = PickerProperties.GetBorderStyle(element);
                Color borderColor = PickerProperties.GetBorderColor(element);
                switch (borderStyle)
                {
                    case BorderStyle.BottomLine:
                        if (_borderLayer != null)
                        {
                            _borderLayer.BackgroundColor = borderColor.ToCGColor();
                        }
                        break;
                    case BorderStyle.Default:
                    case BorderStyle.Rect:
                    case BorderStyle.RoundRect:
                        control.Layer.BorderColor = borderColor.ToCGColor();
                        break;
                }
            }
            else if (e.PropertyName == PickerProperties.BorderWidthProperty.PropertyName)
            {
                BorderStyle borderStyle2 = PickerProperties.GetBorderStyle(element);
                if (borderStyle2 == BorderStyle.Rect || borderStyle2 == BorderStyle.RoundRect)
                {
                    control.Layer.BorderWidth = PickerProperties.GetBorderWidth(element);
                }
            }
            else if (e.PropertyName == PickerProperties.BorderCornerRadiusProperty.PropertyName)
            {
                if (PickerProperties.GetBorderStyle(element) == BorderStyle.RoundRect)
                {
                    SetCornerRadius(control, PickerProperties.GetBorderCornerRadius(element));
                }
                else
                {
                    ResetCornerRadius(control);
                }
            }
            else if (e.PropertyName == PickerProperties.PlaceholderColorProperty.PropertyName)
            {
                UpdatePlaceholderColor(element);
            }
            else if (e.PropertyName == PickerProperties.HorizontalTextAlignmentProperty.PropertyName)
            {
                UpdateTextAlignment(element);
            }
            else if (e.PropertyName == PickerProperties.HorizontalPaddingProperty.PropertyName)
            {
                UpdateHorizontalPadding(element);
            }
        }

        public void UpdatePlaceholderColor(View entry)
        {
            Color placeholderColor = PickerProperties.GetPlaceholderColor(entry);
            if (placeholderColor != Color.Default)
            {
                _setPlaceholderColor(placeholderColor);
            }
        }

        public void SetupBorderProperties(View element, UIView control)
        {
            switch (PickerProperties.GetBorderStyle(element))
            {
                case BorderStyle.BottomLine:
                    {
                        _setBorderStyle(UITextBorderStyle.None);
                        Color borderColor4 = PickerProperties.GetBorderColor(element);
                        CALayer cALayer = new CALayer();
                        cALayer.BackgroundColor = borderColor4.ToCGColor();
                        control.Layer.AddSublayer(cALayer);
                        _borderLayer = cALayer;
                        break;
                    }
                case BorderStyle.Rect:
                    {
                        _setBorderStyle(UITextBorderStyle.Line);
                        control.Layer.BorderWidth = PickerProperties.GetBorderWidth(element);
                        ResetCornerRadius(control);
                        Color borderColor2 = PickerProperties.GetBorderColor(element);
                        if (borderColor2 != Color.Default)
                        {
                            control.Layer.BorderColor = borderColor2.ToCGColor();
                        }
                        break;
                    }
                case BorderStyle.RoundRect:
                    {
                        _setBorderStyle(UITextBorderStyle.RoundedRect);
                        control.Layer.BorderWidth = PickerProperties.GetBorderWidth(element);
                        SetCornerRadius(control, PickerProperties.GetBorderCornerRadius(element));
                        Color borderColor3 = PickerProperties.GetBorderColor(element);
                        if (borderColor3 != Color.Default)
                        {
                            control.Layer.BorderColor = borderColor3.ToCGColor();
                        }
                        break;
                    }
                case BorderStyle.None:
                    _setBorderStyle(UITextBorderStyle.None);
                    control.Layer.BorderWidth = 0;
                    ResetCornerRadius(control);
                    control.Layer.BorderColor = Color.Default.ToCGColor();
                    break;
                case BorderStyle.Default:
                    {
                        Color borderColor = PickerProperties.GetBorderColor(element);
                        if (borderColor != Color.Default)
                        {
                            control.Layer.BorderColor = borderColor.ToCGColor();
                        }
                        break;
                    }
            }
        }

        public void LayoutSubviews(View element, UIView control)
        {
            if (_borderLayer != null)
            {
                float borderWidth = PickerProperties.GetBorderWidth(element);
                UpdateFrame(_borderLayer, borderWidth, control);
            }
        }

        public void UpdateTextAlignment(View element)
        {
            switch (PickerProperties.GetHorizontalTextAlignment(element))
            {
                case TextAlignment.Start:
                    _setTextAlignment(UITextAlignment.Left);
                    break;
                case TextAlignment.End:
                    _setTextAlignment(UITextAlignment.Right);
                    break;
                case TextAlignment.Center:
                    _setTextAlignment(UITextAlignment.Center);
                    break;
            }
        }

        public void UpdateHorizontalPadding(View element)
        {
            float horizontalPadding = PickerProperties.GetHorizontalPadding(element);
            UITextField uITextField = _getTextField();
            if (uITextField != null)
            {
                if (horizontalPadding > 0f)
                {
                    uITextField.LeftView = new UIView(new CGRect(0f, 0f, horizontalPadding, 0f));
                    uITextField.RightView = new UIView(new CGRect(0f, 0f, horizontalPadding, 0f));
                    uITextField.LeftViewMode = UITextFieldViewMode.Always;
                    uITextField.RightViewMode = UITextFieldViewMode.Always;
                }
                else
                {
                    uITextField.LeftViewMode = UITextFieldViewMode.Never;
                    uITextField.RightViewMode = UITextFieldViewMode.Never;
                    uITextField.LeftView = null;
                    uITextField.RightView = null;
                }
            }
        }

        private static void UpdateFrame(CALayer layer, float borderWidth, UIView view)
        {
            layer.Frame = new CGRect(0, view.Frame.Height - borderWidth, view.Frame.Width, borderWidth);
        }

        private void ResetCornerRadius(UIView control)
        {
            control.Layer.CornerRadius = 0;
            UIView[] subviews = control.Subviews;
            if (subviews != null && subviews.Length != 0)
            {
                control.Subviews[0].Layer.CornerRadius = 0;
                control.Subviews[0].Layer.MasksToBounds = false;
            }
        }

        private void SetCornerRadius(UIView control, float radius)
        {
            control.Layer.CornerRadius = radius;
            UIView[] subviews = control.Subviews;
            if (subviews != null && subviews.Length != 0)
            {
                control.Subviews[0].Layer.CornerRadius = radius;
                control.Subviews[0].Layer.MasksToBounds = true;
            }
        }
    }
}
