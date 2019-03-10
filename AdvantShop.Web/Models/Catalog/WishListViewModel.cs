using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Models.Catalog
{
    public class WishListViewModel
    {
        public WishListViewModel()
        {
            DisplayBuyButton = SettingsCatalog.DisplayBuyButton;
            DisplayPreOrderButton = SettingsCatalog.DisplayBuyButton;

            BuyButtonText = SettingsCatalog.BuyButtonText;
            PreOrderButtonText = SettingsCatalog.PreOrderButtonText;

            Products = Products ?? new List<WishListProductModel>();
        }

        public WishListViewModel(List<WishListProductModel> products)
            : this()
        {
            Products = products;
        }

        public bool DisplayBuyButton { get; set; }
        public bool DisplayPreOrderButton { get; set; }

        public string BuyButtonText { get; set; }
        public string PreOrderButtonText { get; set; }

        public List<WishListProductModel> Products { get; set; }
        
    }

    public class WishListProductModel : ProductModel
    {
        public int WishListItemId { get; set; }

        public string AttributesXml { get; set; }

        public Offer Offer { get; set; }
    }
}