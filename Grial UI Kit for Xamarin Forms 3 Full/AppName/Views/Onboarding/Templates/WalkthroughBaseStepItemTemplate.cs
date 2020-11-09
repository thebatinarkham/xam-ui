using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public class WalkthroughBaseStepItemTemplate : ContentPage
    {
        public event EventHandler ItemAppearing;

        public event EventHandler ItemDisappearing;

        public event EventHandler ItemInitialized;

        /* TEXT */

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(WalkthroughBaseStepItemTemplate),
                string.Empty);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /* HEADER */

        public static readonly BindableProperty HeaderProperty =
            BindableProperty.Create(
                nameof(Header),
                typeof(string),
                typeof(WalkthroughBaseStepItemTemplate),
                string.Empty);

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /* BUTTON */

        public static readonly BindableProperty ButtonTextProperty =
            BindableProperty.Create(
                nameof(ButtonText),
                typeof(string),
                typeof(WalkthroughBaseStepItemTemplate),
                string.Empty);

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        public static readonly BindableProperty ButtonBackgroundColorProperty =
            BindableProperty.Create(
                nameof(ButtonBackgroundColor),
                typeof(Color),
                typeof(WalkthroughBaseStepItemTemplate),
                Color.Default);

        public Color ButtonBackgroundColor
        {
            get { return (Color)GetValue(ButtonBackgroundColorProperty); }
            set { SetValue(ButtonBackgroundColorProperty, value); }
        }

        /* ICON */

        public static readonly BindableProperty IconColorProperty =
            BindableProperty.Create(
                nameof(IconColor),
                typeof(Color),
                typeof(WalkthroughBaseStepItemTemplate),
                Color.Default);

        public Color IconColor
        {
            get { return (Color)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        /* IMAGE */

        public static readonly BindableProperty StepBackgroundImageProperty =
            BindableProperty.Create(
                nameof(StepBackgroundImage),
                typeof(string),
                typeof(WalkthroughBaseStepItemTemplate),
                string.Empty);

        public string StepBackgroundImage
        {
            get { return (string)GetValue(StepBackgroundImageProperty); }
            set { SetValue(StepBackgroundImageProperty, value); }
        }

        public static readonly BindableProperty IconTextProperty =
            BindableProperty.Create(
                nameof(IconText),
                typeof(string),
                typeof(WalkthroughBaseStepItemTemplate),
                string.Empty);

        public string IconText
        {
            get { return (string)GetValue(IconTextProperty); }
            set { SetValue(IconTextProperty, value); }
        }

        public void Initialize()
        {
            ItemInitialized?.Invoke(this, EventArgs.Empty);
        }

        public void ItemAppear()
        {
            ItemAppearing?.Invoke(this, EventArgs.Empty);
        }

        public void ItemDisappear()
        {
            ItemDisappearing?.Invoke(this, EventArgs.Empty);
        }
    }
}
