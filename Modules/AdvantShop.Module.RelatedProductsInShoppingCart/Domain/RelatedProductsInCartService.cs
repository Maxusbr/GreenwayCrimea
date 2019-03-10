using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Web.Infrastructure.Extensions;
using Newtonsoft.Json;

namespace AdvantShop.Module.RelatedProductsInShoppingCart.Domain
{
    public class RelatedProductModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ArtNo { get; set; }
        public string Link { get; set; }
        public string Price { get; set; }
        public string Photo { get; set; }
        public string Buttons { get; set; }
    }

    public class RelatedProductsInCartService
    {
        private const string ModuleId = "RelatedProductsInShoppingCart";

        public static string RelatedProductsInShoppingCartJson(List<int> productIds, RelatedType relatedType)
        {
            var relatedProductsFinalList = new List<ProductModel>();

            foreach (int id in productIds)
            {
                relatedProductsFinalList.AddRange(
                    ProductService.GetRelatedProducts(id, relatedType)
                                  .Where(
                                      product =>
                                      !productIds.Contains(product.ProductId) &&
                                      relatedProductsFinalList.All(item => product.ProductId != item.ProductId)));
            }

            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var relatedProducts = new List<RelatedProductModel>();

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

            foreach (var p in relatedProductsFinalList)
            {
                p.DiscountByDatetime = discountByTime;
                p.CustomerGroup = customerGroup;
                p.ProductDiscounts = productDiscounts;
                
                relatedProducts.Add(new RelatedProductModel()
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    ArtNo = p.ArtNo,
                    Link = urlHelper.AbsoluteRouteUrl("Product", new { url = p.UrlPath }),
                    Price = p.PreparedPrice,
                    Photo = p.Photo.ImageSrcSmall(),
                });
            }

            return JsonConvert.SerializeObject(
                new
                {
                    ImageMaxWidth = SettingsPictureSize.SmallProductImageWidth,
                    ImageMaxHeight = SettingsPictureSize.SmallProductImageHeight,
                    Template = "Modules/RelatedProductsInShoppingCart/relatedProductsInSc.tpl",
                    TopHtml = ModuleSettingsProvider.GetSettingValue<string>("TopHtml", ModuleId),
                    BottomHtml = ModuleSettingsProvider.GetSettingValue<string>("BottomHtml", ModuleId),
                    RelatedProducts = relatedProducts
                });
        }

    }
}
