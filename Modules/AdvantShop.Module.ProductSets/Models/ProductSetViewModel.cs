using System.Collections.Generic;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Module.ProductSets.Domain;

namespace AdvantShop.Module.ProductSets.Models
{
    public class ProductSetViewModel
    {
        public ProductSetViewModel()
        {
            Title = ProductSetsSettings.Title;
            PhotoWidth = SettingsPictureSize.SmallProductImageWidth;
            PhotoHeight = SettingsPictureSize.SmallProductImageHeight;
        }

        public string Title { get; set; }
        public int PhotoWidth { get; set; }
        public int PhotoHeight { get; set; }
        public List<ProductModel> ProductSet { get; set; }
        public string TotalPricePrepared { get; set; }
    }
}
