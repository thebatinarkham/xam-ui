using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class SimpleSignUpPage : ContentPage
    {
        public SimpleSignUpPage()
        {
            InitializeComponent();
        }

        private async void OnCloseButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
    }
}
