using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ProductCardItemTemplate : ContentView
    {
        public ProductCardItemTemplate()
        {
            InitializeComponent();
        }

        private async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
#if !NAVIGATION
            var productView = new ProductDetailPage(
                ((VisualElement)sender).BindingContext as FlowProductData);

            await Navigation.PushAsync(productView);
#endif
        }
    }
}
