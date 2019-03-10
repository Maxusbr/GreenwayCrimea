using System;
using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Module.Journal.Domain;

namespace AdvantShop.Module.Journal.Models
{
    public class JournalProductViewModel
    {
        public JournalProductViewModel(List<ProductModel> products)
        {
            DisplayRating = SettingsCatalog.EnableProductRating;
            ShowArtNo = JournalModuleSetting.ShowArtNo;

            ColorImageHeight = SettingsPictureSize.ColorIconHeightCatalog;
            ColorImageWidth = SettingsPictureSize.ColorIconWidthCatalog;

            PhotoWidth = SettingsPictureSize.SmallProductImageWidth;
            PhotoHeight = SettingsPictureSize.SmallProductImageHeight;
            
            Products = products ?? new List<ProductModel>();
            
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

        public int ColorImageHeight { get; set; }
        public int ColorImageWidth { get; set; }

        public int PhotoWidth { get; set; }
        public int PhotoHeight { get; set; }

        public bool DisplayRating { get; set; }

        public bool ShowArtNo { get; set; }

        public List<ProductModel> Products { get; private set; }
    }
}