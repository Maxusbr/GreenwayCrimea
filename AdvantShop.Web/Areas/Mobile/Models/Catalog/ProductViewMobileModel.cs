using System;
using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Configuration;

namespace AdvantShop.Areas.Mobile.Models.Catalog
{
    public class ProductViewMobileModel
    {
        public ProductViewMobileModel(List<ProductModel> products)
        {
            Products = products ?? new List<ProductModel>();

            PhotoWidth = SettingsPictureSize.SmallProductImageWidth;
            PhotoHeight = SettingsPictureSize.SmallProductImageHeight;

            if (Products.Count > 0)
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

        public List<ProductModel> Products { get; private set; }

        public int Id { get; set; }
        public string Title { get; set; }
        public bool DisplayCategory { get; set; }
        public int PhotoWidth { get; set; }
        public int PhotoHeight { get; set; }
        public string SelectedColors { get; set; }
    }
}