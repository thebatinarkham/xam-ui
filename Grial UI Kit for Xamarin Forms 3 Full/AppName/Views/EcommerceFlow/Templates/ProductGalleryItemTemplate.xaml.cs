using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName 
{
    public partial class ProductGalleryItemTemplate : ContentView
    {
        public ProductGalleryItemTemplate()
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
