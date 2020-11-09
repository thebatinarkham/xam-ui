using System.Collections.Generic;
using AppName.Core;

namespace AppName
{
    public class FlowProductData
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public double Price { get; set; }
        public bool IsNew { get; set; }
        public bool IsFavorite { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public int[] Quantities { get; set; } = new int[] { 1, 2, 3, 4, 5 };
        public int SelectedQuantity { get; set; } = 1;
        public FlowProductData[] RelatedProducts { get; set; }
        public ColorsData[] AvailableColors { get; set; }
        public ReviewsData Reviews { get; set; }

        public class ColorsData
        {
            public string Color { get; set; }
            public string[] Gallery { get; set; }
        }

        public class ReviewsData
        {
            public ReviewData[] List { get; set; }
            public int Count { get; set; }

            public class ReviewData
            {
                public string Title { get; set; }
                public string Reviewer { get; set; }
                public double RatingValue { get; set; }
                public double RatingMax { get; set; }
                public string Date { get; set; }
                public string Comment { get; set; }
            }
        }
    }

    public class FlowProductCategoryData
    {
        public string Name { get; set; }
        public FlowProductData[] Content { get; set; }
    }
}
