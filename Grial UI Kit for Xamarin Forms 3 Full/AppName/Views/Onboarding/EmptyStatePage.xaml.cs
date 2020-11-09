using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class EmptyStatePage : ContentPage
    {
        public EmptyStatePage()
        {
            InitializeComponent();
        }

        private async void OnCloseButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }
    }
}