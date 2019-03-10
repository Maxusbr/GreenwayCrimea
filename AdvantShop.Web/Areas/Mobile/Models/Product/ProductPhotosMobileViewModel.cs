using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.ViewModel.ProductDetails;

namespace AdvantShop.Areas.Mobile.Models.ProductDetails
{
    public class ProductPhotosMobileViewModel
    {
        public List<ProductPhoto> Photos { get; set; }

        public Discount Discount { get; set; }

        public Product Product { get; set; }

        public List<string> Labels { get; set; }

        public BaseProductViewModel ProductModel { get; set; }
    }
}