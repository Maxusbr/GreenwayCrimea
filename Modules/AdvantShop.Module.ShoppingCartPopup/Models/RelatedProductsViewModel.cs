using System.Collections.Generic;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Module.ShoppingCartPopup.Models
{
    public class RelatedProductsViewModel
    {
        public List<ProductModel> RelatedProducts { get; set; }

        public int PhotoHeight { get; set; }

        public int PhotoWidth { get; set; }

        public bool DisplayRating { get; set; }

        public string Title { get; set; }

        public bool DisplayBuyButton { get; set; }

        public bool DisplayPreOrderButton { get; set; }

        public string BuyButtonText { get; set; }

        public string PreOrderButtonText { get; set; }

        public int ColorImageWidth { get; set; }

        public int ColorImageHeight { get; set; }

        public bool AllowBuyOutOfStockProducts {get; set;}

    }
}
