using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class SearchBarRenderer : Xamarin.Forms.Platform.iOS.SearchBarRenderer
    {
        public static readonly BindableProperty __GrialSearchBarBorderChanged = BindableProperty.CreateAttached("__GrialSearchBarBorderChanged", typeof(bool), typeof(AppName.Core.SearchBarRenderer), false);

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && base.Control != null)
            {
                UpdateProperties();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == SearchBarProperties.BorderColorProperty.PropertyName || e.PropertyName == SearchBarProperties.BorderWidthProperty.PropertyName)
            {
                UpdateBorder(null);
            }
            else if (e.PropertyName == SearchBarProperties.IconColorProperty.PropertyName)
            {
                UpdateIcon(null);
            }
            else if (e.PropertyName == SearchBarProperties.FieldBackgroundColorProperty.PropertyName)
            {
                UpdateBackground(null);
            }
        }

        private void UpdateProperties()
        {
            UITextField field = null;
            if (EnsureField(ref field))
            {
                UpdateBackground(field);
                UpdateIcon(field);
                UpdateBorder(field);
            }
        }

        private void UpdateBackground(UITextField field)
        {
            Color fieldBackgroundColor = SearchBarProperties.GetFieldBackgroundColor(base.Element);
            if (fieldBackgroundColor != (Color)SearchBarProperties.FieldBackgroundColorProperty.DefaultValue && EnsureField(ref field))
            {
                field.BackgroundColor = fieldBackgroundColor.ToUIColor();
            }
        }

        private void UpdateIcon(UITextField field)
        {
            Color iconColor = SearchBarProperties.GetIconColor(base.Element);
            if (iconColor != (Color)SearchBarProperties.IconColorProperty.DefaultValue && EnsureField(ref field))
            {
                UIImageView uIImageView = field.LeftView as UIImageView;
                if (uIImageView != null)
                {
                    uIImageView.Image = uIImageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                    uIImageView.TintColor = iconColor.ToUIColor();
                }
            }
        }

        private void UpdateBorder(UITextField field)
        {
            float borderWidth = SearchBarProperties.GetBorderWidth(base.Element);
            Color borderColor = SearchBarProperties.GetBorderColor(base.Element);
            if (EnsureField(ref field) && ((borderColor != (Color)SearchBarProperties.BorderColorProperty.DefaultValue && borderWidth > 0f) || (bool)base.Element.GetValue(__GrialSearchBarBorderChanged)))
            {
                base.Element.SetValue(__GrialSearchBarBorderChanged, true);
                field.Layer.MasksToBounds = true;
                field.Layer.BorderColor = borderColor.ToCGColor();
                field.Layer.BorderWidth = borderWidth;
                field.Layer.CornerRadius = 10f;
            }
        }

        private bool EnsureField(ref UITextField field)
        {
            if (field != null)
            {
                return true;
            }
            UIView[] subviews = base.Control.Subviews;
            for (int i = 0; i < subviews.Length; i++)
            {
                UIView[] subviews2 = subviews[i].Subviews;
                foreach (UIView uIView in subviews2)
                {
                    UITextField uITextField = uIView as UITextField;
                    if (uITextField != null)
                    {
                        field = uITextField;
                        return true;
                    }
                    UIView[] subviews3 = uIView.Subviews;
                    for (int k = 0; k < subviews3.Length; k++)
                    {
                        UITextField uITextField2 = subviews3[k] as UITextField;
                        if (uITextField2 != null)
                        {
                            field = uITextField2;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
