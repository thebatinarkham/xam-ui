using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class SimpleLoginPage : ContentPage
    {
        public SimpleLoginPage()
        {
            InitializeComponent();
        }

        private async void OnCloseButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
    }
}
