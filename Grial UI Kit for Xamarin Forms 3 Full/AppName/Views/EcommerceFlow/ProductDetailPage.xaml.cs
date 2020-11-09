using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ProductDetailPage : ContentPage
    {
        public ProductDetailPage()
        {
            InitializeComponent();

            // Use any product to showcase the page
            BindingContext = new EcommerceMainViewModel().HighlightedProducts[0];
        }

        public ProductDetailPage(FlowProductData product)
        { 
            InitializeComponent();

            BindingContext = product;
        }

        private async void OnBuy(object sender, EventArgs e)
        {
#if !NAVIGATION
            var page = new OrderConfirmationPage(
                ((VisualElement)sender).BindingContext as FlowProductData);

            await Navigation.PushAsync(page);
#endif
        }
    }
}
