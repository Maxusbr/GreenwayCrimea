using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Module.RelatedProductsInShoppingCart.Models;
using AdvantShop.Orders;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules.Interfaces;
using System;

namespace AdvantShop.Module.RelatedProductsInShoppingCart.Controllers
{
    public class RelatedProductsInShoppingCartController : ModuleController
    {
        public ActionResult RelatedProducts()
        {
            var productIds = ShoppingCartService.CurrentShoppingCart.Select(p => p.Offer.ProductId).ToList();
            var relatedType = (RelatedType)ModuleSettingsProvider.GetSettingValue<int>("RelatedType", RelatedProductsInShoppingCart.ModuleID);

            var products = new List<ProductModel>();
            var html = "";

            foreach (var pId in productIds)
            {
                var p = ProductService.GetProduct(pId);

                switch (SettingsDesign.RelatedProductSourceType)
                {
                    case SettingsDesign.eRelatedProductSourceType.Default:

                        products.AddRange(ProductService.GetRelatedProducts(pId, relatedType) ?? new List<ProductModel>());
                        break;

                    case SettingsDesign.eRelatedProductSourceType.FromCategory:

                        products.AddRange(ProductService.GetRelatedProducts(pId, relatedType) ?? new List<ProductModel>());
                        if (!products.Any())
                        {
                            products.AddRange(ProductService.GetRelatedProductsFromCategory(p, relatedType) ?? new List<ProductModel>());
                        }
                        break;

                    case SettingsDesign.eRelatedProductSourceType.FromModule:
                        var module = AttachedModules.GetModules<IModuleRelatedProducts>().FirstOrDefault();
                        if (module != null)
                        {
                            var instance = (IModuleRelatedProducts)Activator.CreateInstance(module);

                            products.AddRange(instance.GetRelatedProducts(p, relatedType) ?? new List<ProductModel>());

                            if (products == null || products.Count == 0)
                                html = instance.GetRelatedProductsHtml(p, relatedType);
                        }

                        // get default related products
                        if (!products.Any() && string.IsNullOrEmpty(html))
                        {
                            products.AddRange(ProductService.GetRelatedProducts(p.ProductId, relatedType) ?? new List<ProductModel>());
                        }
                        break;
                }
            }

            if (!products.Any())
                return new EmptyResult();

            var model = new RelatedProductsViewModel()
            {
                ProductIds = products.Select(x => x.ProductId).ToList(),
                Title = "",
                Type = products.ToString(),
                TextBefore = ModuleSettingsProvider.GetSettingValue<string>("TopHtml", RelatedProductsInShoppingCart.ModuleID),
                TextAfter = ModuleSettingsProvider.GetSettingValue<string>("BottomHtml", RelatedProductsInShoppingCart.ModuleID)
            };

            return PartialView("~/Modules/RelatedProductsInShoppingCart/Views/Home/RelatedProducts.cshtml", model);
        }
    }
}
