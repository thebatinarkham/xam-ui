using CoreAnimation;
using CoreGraphics;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class EntryRenderer : Xamarin.Forms.Platform.iOS.EntryRenderer
    {
        private CALayer _borderLayer;

        public EntryRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && base.Control != null)
            {
                UpdatePlaceholderColor(e.NewElement);
                SetupBorderProperties(e.NewElement);
                UpdateHorizontalPadding(e.NewElement);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (string.Equals(e.PropertyName, "BorderColor"))
            {
                BorderStyle borderStyle = EntryProperties.GetBorderStyle(base.Element);
                Color borderColor = EntryProperties.GetBorderColor(base.Element);
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
                        base.Control.Layer.BorderColor = borderColor.ToCGColor();
                        break;
                }
            }
            else if (string.Equals(e.PropertyName, "BorderWidth"))
            {
                BorderStyle borderStyle2 = EntryProperties.GetBorderStyle(base.Element);
                if (borderStyle2 == BorderStyle.Rect || borderStyle2 == BorderStyle.RoundRect)
                {
                    base.Control.Layer.BorderWidth = EntryProperties.GetBorderWidth(base.Element);
                }
            }
            else if (string.Equals(e.PropertyName, "BorderCornerRadius"))
            {
                if (EntryProperties.GetBorderStyle(base.Element) == BorderStyle.RoundRect)
                {
                    base.Control.Layer.CornerRadius = EntryProperties.GetBorderCornerRadius(base.Element);
                }
            }
            else if (string.Equals(e.PropertyName, EntryProperties.PlaceholderColorProperty.PropertyName))
            {
                UpdatePlaceholderColor(base.Element);
            }
            else if (e.PropertyName == EntryProperties.HorizontalPaddingProperty.PropertyName)
            {
                UpdateHorizontalPadding(base.Element);
            }
        }

        private void UpdatePlaceholderColor(Entry entry)
        {
            Color placeholderColor = EntryProperties.GetPlaceholderColor(entry);
            if (placeholderColor != Color.Default && placeholderColor != entry.PlaceholderColor)
            {
                entry.PlaceholderColor = placeholderColor;
            }
        }

        private void UpdateHorizontalPadding(Entry entry)
        {
            float horizontalPadding = EntryProperties.GetHorizontalPadding(entry);
            if (horizontalPadding > 0f)
            {
                base.Control.LeftView = new UIView(new CGRect(0f, 0f, horizontalPadding, 0f));
                base.Control.RightView = new UIView(new CGRect(0f, 0f, horizontalPadding, 0f));
                base.Control.LeftViewMode = UITextFieldViewMode.Always;
                base.Control.RightViewMode = UITextFieldViewMode.Always;
            }
            else
            {
                base.Control.LeftViewMode = UITextFieldViewMode.Never;
                base.Control.RightViewMode = UITextFieldViewMode.Never;
                base.Control.LeftView = null;
                base.Control.RightView = null;
            }
        }

        private void SetupBorderProperties(Entry entry)
        {
            switch (EntryProperties.GetBorderStyle(entry))
            {
                case BorderStyle.BottomLine:
                    {
                        base.Control.BorderStyle = UITextBorderStyle.None;
                        Color borderColor4 = EntryProperties.GetBorderColor(entry);
                        CALayer cALayer = new CALayer();
                        cALayer.BackgroundColor = borderColor4.ToCGColor();
                        base.Control.Layer.AddSublayer(cALayer);
                        _borderLayer = cALayer;
                        break;
                    }
                case BorderStyle.Rect:
                    {
                        base.Control.BorderStyle = UITextBorderStyle.Line;
                        base.Control.Layer.BorderWidth = EntryProperties.GetBorderWidth(entry);
                        base.Control.Layer.CornerRadius = 0f;
                        Color borderColor2 = EntryProperties.GetBorderColor(entry);
                        if (borderColor2 != Color.Default)
                        {
                            base.Control.Layer.BorderColor = borderColor2.ToCGColor();
                        }
                        break;
                    }
                case BorderStyle.RoundRect:
                    {
                        base.Control.BorderStyle = UITextBorderStyle.RoundedRect;
                        base.Control.Layer.BorderWidth = EntryProperties.GetBorderWidth(entry);
                        base.Control.Layer.CornerRadius = EntryProperties.GetBorderCornerRadius(entry);
                        Color borderColor3 = EntryProperties.GetBorderColor(entry);
                        if (borderColor3 != Color.Default)
                        {
                            base.Control.Layer.BorderColor = borderColor3.ToCGColor();
                        }
                        break;
                    }
                case BorderStyle.None:
                    base.Control.BorderStyle = UITextBorderStyle.None;
                    base.Control.Layer.BorderWidth = 0;
                    base.Control.Layer.CornerRadius = 0;
                    base.Control.Layer.BorderColor = Color.Default.ToCGColor();
                    break;
                case BorderStyle.Default:
                    {
                        Color borderColor = EntryProperties.GetBorderColor(entry);
                        if (borderColor != Color.Default)
                        {
                            base.Control.Layer.BorderColor = borderColor.ToCGColor();
                        }
                        break;
                    }
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (_borderLayer != null)
            {
                float borderWidth = EntryProperties.GetBorderWidth(base.Element);
                UpdateFrame(_borderLayer, borderWidth, base.Control);
            }
        }

        private static void UpdateFrame(CALayer layer, float borderWidth, UIView view)
        {
            layer.Frame = new CGRect(0, view.Frame.Height - borderWidth, view.Frame.Width, borderWidth);
        }
    }
}
