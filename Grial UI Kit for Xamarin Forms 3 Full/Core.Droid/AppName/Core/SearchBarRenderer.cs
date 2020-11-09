using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class SearchBarRenderer : Xamarin.Forms.Platform.Android.SearchBarRenderer, IDisposable
    {
        private bool disposed;

        public SearchBarRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                if (e.OldElement != null)
                {
                    e.OldElement.Focused -= OnFocused;
                    e.OldElement.Unfocused -= OnLostFocus;
                }
                if (e.NewElement != null)
                {
                    e.NewElement.Focused += OnFocused;
                    e.NewElement.Unfocused += OnLostFocus;
                }
            }
            if (e.NewElement != null && base.Control != null)
            {
                UpdateProperties();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                disposed = true;
                SearchBar element = base.Element;
                if (element != null)
                {
                    base.Element.Focused -= OnFocused;
                    base.Element.Unfocused -= OnLostFocus;
                }
            }
            base.Dispose(disposing);
        }

        private void OnFocused(object sender, FocusEventArgs e)
        {
            SearchBar element = base.Element;
            if (element == null || !e.IsFocused)
            {
                return;
            }
            SearchView control = base.Control;
            if (control == null)
            {
                return;
            }
            int identifier = control.Context.Resources.GetIdentifier("android:id/search_plate", null, null);
            if (identifier != 0)
            {
                Android.Views.View view = control.FindViewById(identifier);
                if (view != null && view.Background != null)
                {
                    view.Background.SetColorFilter(ColorCache.AccentColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                }
            }
        }

        private void OnLostFocus(object sender, FocusEventArgs e)
        {
            if (e.IsFocused)
            {
                return;
            }
            SearchView control = base.Control;
            if (control == null)
            {
                return;
            }
            int identifier = control.Context.Resources.GetIdentifier("android:id/search_plate", null, null);
            if (identifier != 0)
            {
                Android.Views.View view = control.FindViewById(identifier);
                if (view != null && view.Background != null)
                {
                    view.Background.ClearColorFilter();
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == SearchBarProperties.BorderColorProperty.PropertyName)
            {
                UpdateBorder();
            }
            else if (e.PropertyName == SearchBarProperties.IconColorProperty.PropertyName)
            {
                UpdateIcon();
            }
        }

        private void UpdateProperties()
        {
            UpdateBorder();
            UpdateIcon();
        }

        private void UpdateIcon()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                return;
            }
            Xamarin.Forms.Color iconColor = SearchBarProperties.GetIconColor(base.Element);
            if (iconColor != (Xamarin.Forms.Color)SearchBarProperties.IconColorProperty.DefaultValue)
            {
                ImageView icon = GetIcon();
                if (icon != null)
                {
                    (icon.Drawable?.Mutate())?.SetTint(iconColor.ToAndroid());
                }
            }
        }

        private void UpdateBorder()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Xamarin.Forms.Color borderColor = SearchBarProperties.GetBorderColor(base.Element);
                if (borderColor != (Xamarin.Forms.Color)SearchBarProperties.BorderColorProperty.DefaultValue)
                {
                    GetTextViewBorder()?.SetTint(borderColor.ToAndroid());
                }
            }
        }

        private ImageView GetIcon()
        {
            if (base.Control.ChildCount > 0)
            {
                LinearLayout linearLayout = base.Control.GetChildAt(0) as LinearLayout;
                if (linearLayout != null && linearLayout.ChildCount > 2)
                {
                    linearLayout = (linearLayout.GetChildAt(2) as LinearLayout);
                    if (linearLayout != null && linearLayout.ChildCount > 0)
                    {
                        return linearLayout.GetChildAt(0) as ImageView;
                    }
                }
            }
            return null;
        }

        private Drawable GetTextViewBorder()
        {
            if (base.Control.ChildCount > 0)
            {
                LinearLayout linearLayout = base.Control.GetChildAt(0) as LinearLayout;
                if (linearLayout != null && linearLayout.ChildCount > 2)
                {
                    linearLayout = (linearLayout.GetChildAt(2) as LinearLayout);
                    if (linearLayout != null && linearLayout.ChildCount > 1)
                    {
                        linearLayout = (linearLayout.GetChildAt(1) as LinearLayout);
                        if (linearLayout != null && linearLayout.ChildCount > 0)
                        {
                            return linearLayout.Background;
                        }
                    }
                }
            }
            return null;
        }
    }
}
