using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ImagesDashboardPage : ContentPage
    {
        public ImagesDashboardPage()
        {
            InitializeComponent();

            BindingContext = new NavigationViewModel(variantPageName: $"{GetType().Name}.xaml");
        }
    }
}