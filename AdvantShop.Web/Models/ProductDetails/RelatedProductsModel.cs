using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Models.ProductDetails
{
    public class RelatedProductsViewModel
    {
        public bool IsNotEmpty
        {
            get { return (Products != null && Products.Products.Count > 0) || !string.IsNullOrWhiteSpace(Html); }
        }

        public ProductViewModel Products { get; set; }

        public string RelatedType { get; set; }

        public string Html { get; set; }

        public bool EnabledCarousel { get; set; }
    }
}