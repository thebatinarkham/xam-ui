using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ProductItemViewPage : ContentPage
    {
        public ProductItemViewPage()
            : this(null)
        {
        }

        public ProductItemViewPage(ProductData product)
        {
            InitializeComponent();

            BindingContext = new ProductItemViewViewModel(product?.Id);
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
#if !NAVIGATION
            var imagePreview = new ProductImageFullScreenPage((sender as FFImageLoading.Forms.CachedImage).Source);

            await Navigation.PushModalAsync(new NavigationPage(imagePreview));
#endif
        }
    }
}