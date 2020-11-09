using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class RoundedLabel : ContentView
    {
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(
                nameof(TextColor),
                typeof(Color),
                typeof(RoundedLabel),
                defaultValue: Color.White);

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty HorizontalTextAlignmentProperty =
            BindableProperty.Create(
                nameof(HorizontalTextAlignment),
                typeof(TextAlignment),
                typeof(RoundedLabel),
                defaultValue: TextAlignment.Start);

        public TextAlignment HorizontalTextAlignment
        {
            get { return (TextAlignment)GetValue(HorizontalTextAlignmentProperty); }
            set { SetValue(HorizontalTextAlignmentProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(RoundedLabel),
                defaultValue: string.Empty);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(
                nameof(CornerRadius),
                typeof(double),
                typeof(RoundedLabel),
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
                typeof(RoundedLabel),
                defaultValue: 10.0);

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(
                nameof(FontAttributes),
                typeof(FontAttributes),
                typeof(RoundedLabel),
                defaultValue: null);

        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }

        public static readonly BindableProperty LineBreakModeProperty =
            BindableProperty.Create(
                nameof(LineBreakMode),
                typeof(LineBreakMode),
                typeof(RoundedLabel),
                defaultValue: LineBreakMode.WordWrap,
                propertyChanged: (s, o, n) => ((RoundedLabel)s).label.LineBreakMode = (LineBreakMode)n);

        public LineBreakMode LineBreakMode
        {
            get { return (LineBreakMode)GetValue(LineBreakModeProperty); }
            set { SetValue(LineBreakModeProperty, value); }
        }

        public RoundedLabel()
        {
            InitializeComponent();
        }
    }
}
