using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ProductItemFullScreenPage : ContentPage
    {
        public ProductItemFullScreenPage()
            : this(null)
        {
        }

        public ProductItemFullScreenPage(ProductData product)
        {
            InitializeComponent();

            BindingContext = new ProductItemViewViewModel(product?.Id, variantPageName: $"{GetType().Name}.xaml");
        }
    }
}