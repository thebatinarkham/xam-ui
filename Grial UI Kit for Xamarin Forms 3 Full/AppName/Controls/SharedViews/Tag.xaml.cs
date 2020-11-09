using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class Tag : ContentView
    {
        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create(
                nameof(Image),
                typeof(ImageSource),
                typeof(Tag),
                defaultValue: null);

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly BindableProperty ImageSizeProperty =
            BindableProperty.Create(
                nameof(ImageSize),
                typeof(double),
                typeof(Tag),
                defaultValue: 20.0);

        public double ImageSize
        {
            get { return (double)GetValue(ImageSizeProperty); }
            set { SetValue(ImageSizeProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(
                nameof(TextColor),
                typeof(Color),
                typeof(Tag),
                defaultValue: Color.White);

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(Tag),
                defaultValue: string.Empty);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(
                nameof(Icon),
                typeof(string),
                typeof(Tag),
                defaultValue: string.Empty);

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(
                nameof(CornerRadius),
                typeof(double),
                typeof(Tag),
                defaultValue: 6.0);

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(
                nameof(FontSize),
                typeof(double),
                typeof(Tag),
                defaultValue: 10.0);

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty IconFontSizeProperty =
            BindableProperty.Create(
                nameof(IconFontSize),
                typeof(double),
                typeof(Tag),
                defaultValue: 10.0);

        public double IconFontSize
        {
            get { return (double)GetValue(IconFontSizeProperty); }
            set { SetValue(IconFontSizeProperty, value); }
        }

        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(
                nameof(FontAttributes),
                typeof(Enum),
                typeof(Tag),
                defaultValue: null);

        public Enum FontAttributes
        {
            get { return (Enum)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }

        public Tag()
        {
            InitializeComponent();
        }
    }
}