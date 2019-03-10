using System;
using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Configuration;

namespace AdvantShop.Models.Catalog
{
    public class CategoryFiltering
    {
        public CategoryFiltering()
        {
            PriceFrom = 0;
            PriceTo = Int32.MaxValue;

            BrandIds = new List<int>();
            SizeIds = new List<int>();
            ColorIds = new List<int>();
            PropertyIds = new List<int>();
            Sorting = ESortOrder.NoSorting;
            ViewMode = ProductViewMode.Tile.ToString().ToLower();
        }

        public CategoryFiltering(int categoryId, bool indepth) : this()
        {
            CategoryId = categoryId;
            Indepth = indepth;
        }

        public int CategoryId { get; set; }

        public bool Indepth { get; set; }

        public List<int> BrandIds { get; set; }
        public List<int> AvailableBrandIds { get; set; }

        public List<int> SizeIds { get; set; }
        public List<int> AvailableSizeIds { get; set; }

        public List<int> ColorIds { get; set; }
        public List<int> AvailableColorIds { get; set; }

        public List<int> PropertyIds { get; set; }
        public List<int> AvailablePropertyIds { get; set; }
        public Dictionary<int, KeyValuePair<float, float>> RangePropertyIds { get; set; }

        public float PriceFrom { get; set; }

        public float PriceTo { get; set; }

        public bool Available { get; set; }

        //public bool Preorder { get; set; }

        public ESortOrder Sorting { get; set; }

        public string ViewMode { get; set; }
        public bool AllowChangeViewMode { get; set; }

        //public EProductOnMain TypeFlag1 { get; set; }
    }
}