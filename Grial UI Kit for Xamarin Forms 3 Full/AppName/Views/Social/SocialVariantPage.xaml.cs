using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class SocialVariantPage : ContentPage
    {
        public SocialVariantPage()
        {
            InitializeComponent();

            BindingContext = new SocialViewModel(variantPageName: $"{GetType().Name}.xaml");
        }

        private async void OnAvatarTapped(object sender, EventArgs e)
        {
#if !NAVIGATION
            await Navigation.PushAsync(new ProfilePage());
#endif
        }
    }
}