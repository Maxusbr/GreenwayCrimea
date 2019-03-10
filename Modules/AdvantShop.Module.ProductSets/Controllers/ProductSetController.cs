using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Module.ProductSets.Domain;
using AdvantShop.Module.ProductSets.Models;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Catalog;
using AdvantShop.Orders;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Modules;
using System;

namespace AdvantShop.Module.ProductSets.Controllers
{
    public class ProductSetController : ModuleController
    {
        #region ProductSet

        public ActionResult ProductSet(Product product)
        {
            var mainProductOffer = product.Offers.FirstOrDefault(ProductSetsService.CanBuyOffer);

            // products in set
            var offers = ProductSetsService.GetLinkedOffers(product.ProductId, true).ToList();

            if (mainProductOffer == null || !offers.Any())
                return new EmptyResult();

            offers.Insert(0, mainProductOffer);

            var discount = new Discount(ProductSetsService.GetDiscount(product.ProductId), 0);
            
            var productModels = GetProductModels(offers, discount.Percent);

            var model = new ProductSetViewModel()
            {
                ProductSet = productModels,
                TotalPricePrepared = discount.HasValue
                    ? PriceFormatService.FormatPrice(productModels.Sum(pm => pm.RoundedPrice), productModels.Sum(pm => pm.PriceWithDiscount), discount, showDiscount: true)
                    : PriceFormatService.FormatPrice(productModels.Sum(pm => pm.PriceWithDiscount), 0, new Discount())
            };
            return PartialView("~/Modules/ProductSets/Views/ProductSet/ProductSet.cshtml", model);
        }

        [HttpPost]
        public JsonResult GetTotalPrice(int productId, List<int> offerIds)
        {
            var product = ProductService.GetProduct(productId);
            if (!offerIds.Any() || product == null)
                return Json(new { result = string.Empty });

            var currentOffer = product.Offers.FirstOrDefault(x => offerIds.Contains(x.OfferId));
            var set = ProductSetsService.GetLinkedOffers(productId, true);

            var isFullSet = currentOffer != null && set.All(x => offerIds.Contains(x.OfferId));
            var discount = new Discount(isFullSet ? ProductSetsService.GetDiscount(productId) : 0, 0);

            var offers = new List<Offer>();
            if (currentOffer != null)
                offers.Add(currentOffer);
            offers.AddRange(set.Where(x => offerIds.Contains(x.OfferId)));

            var productModels = GetProductModels(offers, discount.Percent);

            var priceFormatted = PriceFormatService.FormatPrice(productModels.Sum(pm => pm.RoundedPrice), productModels.Sum(pm => pm.PriceWithDiscount), discount, showDiscount: true);

            return Json(new {result = priceFormatted });
        }
        
        private List<ProductModel> GetProductModels(List<Offer> offers, float discount)
        {
            float discountByTime = 0;
            var customerGroup = CustomerContext.CurrentCustomer.CustomerGroup;
            var productDiscountModels = new List<ProductDiscount>();

            if (discount <= 0)
            {
                discountByTime = DiscountByTimeService.GetDiscountByTime();
                customerGroup = CustomerGroupService.GetCustomerGroup(CustomerGroupService.DefaultCustomerGroup);
                var discountModules = AttachedModules.GetModules<IDiscount>();
                foreach (var discountModule in discountModules.Where(x => x != null))
                {
                    var classInstance = (IDiscount)Activator.CreateInstance(discountModule);
                    productDiscountModels.AddRange(classInstance.GetProductDiscountsList());
                }
            }

            var productModels = new List<ProductModel>();
            foreach (var offer in offers)
            {
                var pm = new ProductModel()
                {
                    ProductId = offer.ProductId,
                    OfferId = offer.OfferId,
                    UrlPath = offer.Product.UrlPath,
                    Name = offer.Product.Name,
                    BasePrice = offer.BasePrice,
                    CurrencyValue = offer.Product.Currency.Rate,
                    Discount = offer.Product.Discount.Percent,
                    DiscountAmount = offer.Product.Discount.Amount,
                    DiscountByDatetime = discountByTime,
                    ProductDiscounts = productDiscountModels,
                    CustomerGroup = customerGroup
                };
                if (discount > 0)
                {
                    pm.Discount = discount;
                    pm.DiscountAmount = 0;
                }

                var title = !string.IsNullOrEmpty(offer.Photo.Description)
                    ? HttpUtility.HtmlEncode(offer.Photo.Description)
                    : HttpUtility.HtmlEncode(offer.Product.Name);

                pm.Photo = new ProductPhoto()
                {
                    PhotoName = offer.Photo.PhotoName,
                    Title = title,
                    Alt = title
                };
                productModels.Add(pm);
            }
            return productModels;
        }

        #endregion
        
        public ActionResult FullCartMessage()
        {
            var cart = ShoppingCartService.CurrentShoppingCart;
            var htmlSets = new List<string>();
            var sets = ProductSetsService.GetCartProductSets(cart);

            foreach (var kvp in sets)
            {
                var item = kvp.Value[0];
                var discount = ProductSetsService.GetDiscount(item.Offer.ProductId);
                if (discount <= 0)
                    continue;
                htmlSets.Add(string.Format("{0} {1} товаров с \"{2}\" на сумму <b>{3}</b> со скидкой <b class=\"price-discount price-discount-abs\">{4}%</b>",
                    item.Amount,
                    Strings.Numerals(item.Amount, string.Empty, "комплект", "комплекта", "комплектов"),
                    item.Offer.Product.Name + (item.Offer.Size != null ? " " + item.Offer.Size.SizeName : string.Empty) + (item.Offer.Color != null ? " " + item.Offer.Color.ColorName : string.Empty),
                    kvp.Value.Sum(x => x.PriceWithDiscount * x.Amount).FormatPrice(),
                    discount.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            }

            if (htmlSets.Any())
                return Content(string.Format("<div>В вашем заказе {0}</div>", htmlSets.AggregateString(", ")));
            return new EmptyResult();
        }
    }
}
