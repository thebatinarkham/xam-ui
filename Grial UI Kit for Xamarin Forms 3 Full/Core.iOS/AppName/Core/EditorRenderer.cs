using CoreAnimation;
using CoreGraphics;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class EditorRenderer : Xamarin.Forms.Platform.iOS.EditorRenderer
    {
        private class CustomLabel : UILabel
        {
            private Editor _editor;

            private UITextView _textView;

            public CustomLabel()
            {
                LineBreakMode = UILineBreakMode.TailTruncation;
                AdjustsFontSizeToFitWidth = false;
            }

            public void SetEditor(Editor editor, UITextView textView)
            {
                _editor = editor;
                _textView = textView;
                Apply();
            }

            public override bool PointInside(CGPoint point, UIEvent uievent)
            {
                return false;
            }

            public void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                Apply();
            }

            public void OnSizeChanged()
            {
                SizeToFit();
                if (_editor != null)
                {
                    float v = EditorProperties.GetHorizontalPadding(_editor) + 5f;
                    Frame = new CGRect(v, 8, _textView.Frame.Width, Frame.Height);
                }
            }

            private void Apply()
            {
                Font = _textView.Font;
                Color placeholderColor = EditorProperties.GetPlaceholderColor(_editor);
                if (placeholderColor != (Color)EditorProperties.PlaceholderColorProperty.DefaultValue)
                {
                    TextColor = placeholderColor.ToUIColor();
                }
                else
                {
                    _textView.TextColor.GetRGBA(out nfloat red, out nfloat green, out nfloat blue, out nfloat alpha);
                    TextColor = new UIColor(red, green, blue, alpha * (nfloat)0.3);
                }
                Text = EditorProperties.GetPlaceholder(_editor);
                if (!string.IsNullOrEmpty(Text) && string.IsNullOrEmpty(_textView.Text))
                {
                    if (Superview == null)
                    {
                        _textView.AddSubview(this);
                    }
                }
                else
                {
                    RemoveFromSuperview();
                }
            }
        }

        private readonly CustomLabel _placeholder;

        private CALayer _borderLayer;

        private UIEdgeInsets _originalEdgeInsets;

        public EditorRenderer()
        {
            _placeholder = new CustomLabel();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && base.Control != null)
            {
                _originalEdgeInsets = base.Control.TextContainerInset;
                InitializeBorderStyle();
                UpdateBorderProperties();
                _placeholder.SetEditor(e.NewElement, base.Control);
                UpdateHorizontalPadding();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (string.Equals(e.PropertyName, "BorderColor"))
            {
                UpdateBorderColor();
            }
            else if (string.Equals(e.PropertyName, "BorderWidth"))
            {
                UpdateBorderWidth();
            }
            else if (string.Equals(e.PropertyName, "BorderCornerRadius"))
            {
                UpdateBorderRadius();
            }
            else if (string.Equals(e.PropertyName, "HorizontalPadding"))
            {
                UpdateHorizontalPadding();
            }
            _placeholder.OnPropertyChanged(e);
        }

        private void UpdateBorderProperties()
        {
            UpdateBorderWidth();
            UpdateBorderColor();
            UpdateBorderRadius();
        }

        private void UpdateHorizontalPadding()
        {
            float horizontalPadding = EditorProperties.GetHorizontalPadding(base.Element);
            if (horizontalPadding > 0f)
            {
                base.Control.TextContainerInset = new UIEdgeInsets(base.Control.TextContainerInset.Top, horizontalPadding, base.Control.TextContainerInset.Left, horizontalPadding);
            }
            else
            {
                base.Control.TextContainerInset = _originalEdgeInsets;
            }
            _placeholder.OnSizeChanged();
        }

        private void UpdateBorderWidth()
        {
            if (EditorProperties.GetBorderStyle(base.Element) != BorderStyle.None)
            {
                _borderLayer.BorderWidth = EditorProperties.GetBorderWidth(base.Element);
            }
        }

        private void UpdateBorderRadius()
        {
            BorderStyle borderStyle = EditorProperties.GetBorderStyle(base.Element);
            if (borderStyle == BorderStyle.RoundRect || borderStyle == BorderStyle.Default)
            {
                _borderLayer.CornerRadius = EditorProperties.GetBorderCornerRadius(base.Element);
            }
        }

        private void UpdateBorderColor()
        {
            if (EditorProperties.GetBorderStyle(base.Element) != BorderStyle.None)
            {
                Color borderColor = EditorProperties.GetBorderColor(base.Element);
                if (borderColor != Color.Default)
                {
                    _borderLayer.BorderColor = borderColor.ToCGColor();
                }
                else
                {
                    _borderLayer.BorderColor = UIColor.FromRGBA(0, 0, 0, 50).CGColor;
                }
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            _placeholder.OnSizeChanged();
            if (_borderLayer != base.Control.Layer)
            {
                float borderWidth = EditorProperties.GetBorderWidth(base.Element);
                UpdateFrame(_borderLayer, borderWidth, base.Control);
            }
        }

        private static void UpdateFrame(CALayer layer, float borderWidth, UIView view)
        {
            layer.Frame = new CGRect(0, view.Frame.Height, view.Frame.Width, borderWidth);
        }

        private void InitializeBorderStyle()
        {
            base.Control.ClipsToBounds = true;
            switch (EditorProperties.GetBorderStyle(base.Element))
            {
                case BorderStyle.BottomLine:
                    {
                        CALayer cALayer = new CALayer();
                        Layer.AddSublayer(cALayer);
                        _borderLayer = cALayer;
                        break;
                    }
                case BorderStyle.Rect:
                    _borderLayer = base.Control.Layer;
                    base.Control.Layer.CornerRadius = 0f;
                    break;
                case BorderStyle.Default:
                case BorderStyle.RoundRect:
                    _borderLayer = base.Control.Layer;
                    break;
                case BorderStyle.None:
                    base.Control.Layer.BorderWidth = 0;
                    base.Control.Layer.CornerRadius = 0;
                    base.Control.Layer.BorderColor = Color.Default.ToCGColor();
                    break;
            }
        }
    }
}
