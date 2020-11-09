using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AppName.Core;

namespace AppName
{
    public class EcommerceMainViewModel : ObservableObject
    {
        public EcommerceMainViewModel()
        {
            LoadData();
        }

        public ObservableCollection<FlowProductData> HighlightedProducts { get; } = new ObservableCollection<FlowProductData>();

        public ObservableCollection<FlowProductCategoryData> Categories { get; } = new ObservableCollection<FlowProductCategoryData>();
        
        public string Banner { get; set; }

        private void LoadData()
        {
            HighlightedProducts.Clear();
            Categories.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "EcommerceFlow.json");

            foreach (var c in Categories)
            {
                LoadRelated(c, Categories.Where(x => x != c));
            }

            foreach (var p in HighlightedProducts)
            {
                LoadRelated(p, Categories);
            }
        }

        private void LoadRelated(
            FlowProductCategoryData category, 
            IEnumerable<FlowProductCategoryData> others)
        {
            // Simulates related products by picking some other products
            foreach (var p in category.Content)
            {
                p.RelatedProducts =
                    category.Content.Where(x => x != p)
                        .Concat(others.Select(o => o.Content.FirstOrDefault()))
                        .ToArray();
            }
        }

        private void LoadRelated(
            FlowProductData product,
            IEnumerable<FlowProductCategoryData> categories)
        {
            // Simulates related products by picking some other products
            product.RelatedProducts =
                categories.Select(o => o.Content.FirstOrDefault())
                .Where(x => x.Name != product.Name)
                .ToArray();
        }
    }
}
