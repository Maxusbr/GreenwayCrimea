using System;
using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;

namespace AdvantShop.ViewModel.Catalog
{
    public partial class ProductViewModel
    {
        public ProductViewModel(List<ProductModel> products)
        {
            DisplayBuyButton = SettingsCatalog.DisplayBuyButton;
            DisplayPreOrderButton = SettingsCatalog.DisplayPreOrderButton;
            AllowBuyOutOfStockProducts = SettingsCheckout.OutOfStockAction == eOutOfStockAction.Cart;

            DisplayRating = SettingsCatalog.EnableProductRating;
            DisplayComparison = SettingsCatalog.EnableCompareProducts;
            DisplayPhotoPreviews = SettingsCatalog.EnablePhotoPreviews;
            DisplayPhotoCount = SettingsCatalog.ShowCountPhoto;
            DisplayQuickView = SettingsCatalog.ShowQuickView;
            DisplayProductArtNo = SettingsCatalog.ShowProductArtNo;

            BuyButtonText = SettingsCatalog.BuyButtonText;
            PreOrderButtonText = SettingsCatalog.PreOrderButtonText;

            ColorImageHeight = SettingsPictureSize.ColorIconHeightCatalog;
            ColorImageWidth = SettingsPictureSize.ColorIconWidthCatalog;

            PhotoWidth = SettingsPictureSize.SmallProductImageWidth;
            PhotoHeight = SettingsPictureSize.SmallProductImageHeight;

            PhotoPreviewWidth = SettingsPictureSize.XSmallProductImageWidth;
            PhotoPreviewHeight = SettingsPictureSize.XSmallProductImageHeight;

            Products = products ?? new List<ProductModel>();

            CountProductsInLine = SettingsDesign.CountCatalogProductInLine;

            if (Products != null && Products.Count > 0)
            {
                var productDiscounts = new List<ProductDiscount>();
                var discountByTime = DiscountByTimeService.GetDiscountByTime();
                var customerGroup = CustomerContext.CurrentCustomer.CustomerGroup;

                var discountModules = AttachedModules.GetModules<IDiscount>();
                foreach (var discountModule in discountModules)
                {
                    if (discountModule != null)
                    {
                        var classInstance = (IDiscount)Activator.CreateInstance(discountModule);
                        productDiscounts.AddRange(classInstance.GetProductDiscountsList());
                    }
                }

                foreach (var product in Products)
                {
                    product.DiscountByDatetime = discountByTime;
                    product.CustomerGroup = customerGroup;
                    product.ProductDiscounts = productDiscounts;
                }
            }
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public bool DisplayBuyButton { get; set; }
        public bool DisplayPreOrderButton { get; set; }
        public bool AllowBuyOutOfStockProducts { get; set; }

        public bool DisplayPhotoPreviews { get; set; }
        public bool DisplayComparison { get; set; }
        public bool DisplayRating { get; set; }
        public bool DisplayCategory { get; set; }
        public bool DisplayQuickView { get; set; }
        public bool DisplayProductArtNo { get; set; }
        public bool DisplayPhotoCount { get; set; }

        public int CountProductsInLine { get; set; }       
        
        public string BuyButtonText { get; set; }
        public string PreOrderButtonText { get; set; }

        public int ColorImageHeight { get; set; }
        public int ColorImageWidth { get; set; }
        public string SelectedColors { get; set; }

        public int PhotoWidth { get; set; }
        public int PhotoHeight { get; set; }

        public int PhotoPreviewWidth { get; set; }
        public int PhotoPreviewHeight { get; set; }


        public List<ProductModel> Products { get; private set; }
    }
}