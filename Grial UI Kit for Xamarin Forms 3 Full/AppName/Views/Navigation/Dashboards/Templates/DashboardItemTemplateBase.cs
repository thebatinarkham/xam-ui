using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public class DashboardItemTemplateBase : ContentView
    {
        public static readonly BindableProperty ShowBackgroundImageProperty =
            BindableProperty.Create(
                nameof(ShowBackgroundImage),
                typeof(bool),
                typeof(DashboardItemTemplateBase),
                true,
                defaultBindingMode: BindingMode.OneWay);

        public bool ShowBackgroundImage
        {
            get { return (bool)GetValue(ShowBackgroundImageProperty); }
            set { SetValue(ShowBackgroundImageProperty, value); }
        }

        public static readonly BindableProperty ShowBackgroundColorProperty =
            BindableProperty.Create(
                nameof(ShowBackgroundColor),
                typeof(bool),
                typeof(DashboardItemTemplateBase),
                false,
                defaultBindingMode: BindingMode.OneWay);

        public bool ShowBackgroundColor
        {
            get { return (bool)GetValue(ShowBackgroundColorProperty); }
            set { SetValue(ShowBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty ShowiconColoredCircleBackgroundProperty =
            BindableProperty.Create(
                nameof(ShowiconColoredCircleBackground),
                typeof(bool),
                typeof(DashboardItemTemplateBase),
                true,
                defaultBindingMode: BindingMode.OneWay);

        public bool ShowiconColoredCircleBackground
        {
            get { return (bool)GetValue(ShowiconColoredCircleBackgroundProperty); }
            set { SetValue(ShowiconColoredCircleBackgroundProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(
                nameof(TextColor),
                typeof(Color),
                typeof(DashboardItemTemplateBase),
                defaultValue: Color.White,
                defaultBindingMode: BindingMode.OneWay);

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        private const uint AnimationDuration = 250;
        private bool _processingTag = false;

        public DashboardItemTemplateBase()
        {
            var recognizer = new TapGestureRecognizer();
            GestureRecognizers.Add(recognizer);
            recognizer.Tapped += OnItemTapped;
        }

        protected virtual void OnTapped()
        {
        }

        private async void OnItemTapped(object sender, EventArgs e)
        {
            if (_processingTag)
            {
                return;
            }

            _processingTag = true;

            await AnimateItem(this, AnimationDuration);

            OnTapped();

            _processingTag = false;
        }

        private async Task AnimateItem(View uiElement, uint duration)
        {
            var originalOpacity = uiElement.Opacity;

            await uiElement.FadeTo(.5, duration / 2, Easing.CubicIn);
            await uiElement.FadeTo(originalOpacity, duration / 2, Easing.CubicIn);
        }
    }
}