using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class CustomActivityIndicatorPage : ContentPage
    {
        public CustomActivityIndicatorPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Indicator.Start();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            Indicator.Stop();
        }
    }
}