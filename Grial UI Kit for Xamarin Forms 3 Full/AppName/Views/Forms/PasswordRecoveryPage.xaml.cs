using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class PasswordRecoveryPage : ContentPage
    {
        public PasswordRecoveryPage()
        {
            InitializeComponent();
        }

        private async void OnCloseButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
    }
}