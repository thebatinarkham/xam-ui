using System;
using System.Collections.Generic;
using System.ComponentModel;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Xamarin.Essentials;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public class CircleCachedImage : CachedImage
    {
        public static BindableProperty BorderColorProperty =
            BindableProperty.Create(
                nameof(BorderColor),
                typeof(Color),
                typeof(CircleCachedImage),
                defaultValue: Color.Transparent,
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: OnBorderColorChanged);

        public static BindableProperty BorderSizeProperty =
            BindableProperty.Create(
                nameof(BorderSize),
                typeof(double),
                typeof(CircleCachedImage),
                defaultValue: 1.0,
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: OnBorderSizeChanged);

        private readonly CircleTransformation _transformation;

        public CircleCachedImage()
        {
            _transformation = new CircleTransformation(BorderSize * DeviceDisplay.MainDisplayInfo.Density, BorderColor.ToHexString());

            base.Transformations.Add(_transformation);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new List<ITransformation> Transformations
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public double BorderSize
        {
            get { return (double)GetValue(BorderSizeProperty); }
            set { SetValue(BorderSizeProperty, value); }
        }

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        private static void OnBorderColorChanged(BindableObject view, object oldValue, object newValue)
        {
            if (view is CircleCachedImage cacheImage && cacheImage._transformation != null)
            {
                cacheImage._transformation.BorderHexColor = cacheImage.BorderColor.ToHexString();
            }
        }

        private static void OnBorderSizeChanged(BindableObject view, object oldValue, object newValue)
        {
            if (view is CircleCachedImage cacheImage && cacheImage._transformation != null)
            {
                cacheImage._transformation.BorderSize = cacheImage.BorderSize * DeviceDisplay.MainDisplayInfo.Density;
            }
        }
    }
}
