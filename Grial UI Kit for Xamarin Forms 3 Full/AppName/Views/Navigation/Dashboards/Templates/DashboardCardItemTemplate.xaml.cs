using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class DashboardCardItemTemplate : ContentView
    {
        public DashboardCardItemTemplate()
        {
            InitializeComponent();
        }

        private async void OnCardTapped(object sender, EventArgs args)
        {
            await Application.Current.MainPage.DisplayAlert("Card Tapped!", "You have tapped a Card", "OK");
        }
    }
}