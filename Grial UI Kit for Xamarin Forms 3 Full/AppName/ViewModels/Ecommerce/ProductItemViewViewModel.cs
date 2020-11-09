using System.Globalization;
using System.Linq;
using AppName.Core;

namespace AppName
{
    public class ProductItemViewViewModel : ObservableObject
    {
        private readonly string _variantPageName;
        private readonly string _productId;
        private ProductData _product;

        public ProductItemViewViewModel(string productId = null, string variantPageName = null)
            : base(listenCultureChanges: true)
        {
            _variantPageName = variantPageName;
            _productId = productId;

            LoadData();
        }

        public ProductData Product
        {
            get { return _product; }
            set { SetProperty(ref _product, value); }
        }
        
        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            Product = null;

            JsonHelper.Instance.LoadViewModel(this, pageName: _variantPageName, source: "Ecommerce.json");

            if (_productId != null)
            {
                Product = new ProductsCatalogViewModel()
                    .List.FirstOrDefault(product => product.Id == _productId);
            }
        }
    }
}
