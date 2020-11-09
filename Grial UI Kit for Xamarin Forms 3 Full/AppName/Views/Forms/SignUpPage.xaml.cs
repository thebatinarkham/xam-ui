using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private async void OnLoginTapped(object sender, EventArgs e)
        {
#if !NAVIGATION
            await Navigation.PushAsync(new LoginPage());
#endif
        }

        private async void OnCloseButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
    }
}