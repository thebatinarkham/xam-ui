using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class FlatDashboardPage : ContentPage
    {
        public FlatDashboardPage()
        {
            InitializeComponent();

            BindingContext = new NavigationViewModel(variantPageName: $"{GetType().Name}.xaml");
        }
    }
}