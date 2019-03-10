using System;
using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;

namespace AdvantShop.ViewModel.Compare
{
    public class CompareProductsViewModel
    {
        public CompareProductsViewModel(List<CompareProductModel> products, List<Property> list)
        {

            DisplayBuyButton = SettingsCatalog.DisplayBuyButton;
            DisplayPreOrderButton = SettingsCatalog.DisplayPreOrderButton;

            BuyButtonText = SettingsCatalog.BuyButtonText;
            PreOrderButtonText = SettingsCatalog.PreOrderButtonText;

            Properties = list ?? new List<Property>();
            Products = products ?? new List<CompareProductModel>();

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
        
        public bool DisplayBuyButton { get; set; }
        public bool DisplayPreOrderButton { get; set; }

        public string BuyButtonText { get; set; }
        public string PreOrderButtonText { get; set; }

        public List<Property> Properties { get; set; }
        public List<CompareProductModel> Products { get; set; }
        
    }

    public class CompareProductModel : ProductModel
    {
        private List<PropertyValue> _productPropertyValues;
        public List<PropertyValue> ProductPropertyValues
        {
            get
            {
                return _productPropertyValues ??
                       (_productPropertyValues = PropertyService.GetPropertyValuesByProductId(ProductId));
            }
        }
    }
}