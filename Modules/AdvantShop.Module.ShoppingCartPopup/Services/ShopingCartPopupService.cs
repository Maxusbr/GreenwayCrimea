using System;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Module.ShoppingCartPopup.Services
{
    public class ShopingCartPopupService
    {
        public static object GetCartJson(int cartId)
        {
            var shpCart = ShoppingCartService.CurrentShoppingCart;
            var cartItem = shpCart.Find(x => x.ShoppingCartItemId == cartId);

            if (cartItem == null)
                return new { status = "error" };

            var totalItems = shpCart.TotalItems;
            var totalDiscount = shpCart.TotalDiscount;
            var totalPrice = shpCart.TotalPrice;

            var totalPriceString =
                totalPrice - totalDiscount > 0
                    ? (totalPrice - totalDiscount).RoundAndFormatPrice(CurrencyService.CurrentCurrency)
                    : 0F.FormatPrice();

            var totalCounts = String.Format("В вашей <a href=\"cart\">корзине</a> <span class=\"cart-popup-count\">{0}</span> {1}<br> на сумму {2}",
                totalItems, Strings.Numerals(totalItems, "пусто", "товар", "товара", "товаров"), totalPriceString);

            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            var obj = new
            {
                status = "success",
                showCart = true,

                Sku = cartItem.Offer.ArtNo,
                Name = cartItem.Offer.Product.Name,
                Link = urlHelper.RouteUrl("Product", new { url = cartItem.Offer.Product.UrlPath }),
                Amount = cartItem.Amount,
                Price = cartItem.Price.FormatPrice(),
                Cost = (cartItem.Price * cartItem.Amount).FormatPrice(),
                PhotoPath = cartItem.Offer.Photo.ImageSrcSmall(),
                PhotoAlt = cartItem.Offer.Product.Name,
                SelectedOptions = CustomOptionsService.DeserializeFromXml(cartItem.AttributesXml, cartItem.Offer.Product.Currency.Rate),
                ColorName = cartItem.Offer.Color != null ? cartItem.Offer.Color.ColorName : null,
                SizeName = cartItem.Offer.Size != null ? cartItem.Offer.Size.SizeName : null,
                AvailableAmount = cartItem.Offer.Amount,
                CanOrderByRequest = cartItem.Offer.CanOrderByRequest,
                MinAmount = cartItem.Offer.Product.MinAmount ?? 1,
                MaxAmount = cartItem.Offer.Product.MaxAmount ?? Int32.MaxValue,
                Multiplicity = cartItem.Offer.Product.Multiplicity,
                IsGift = cartItem.IsGift,
                Rating = cartItem.Offer.Product.Ratio,
                Reviews = ReviewService.GetReviewsCount(cartItem.Offer.Product.ProductId, EntityType.Product),

                BrandName = cartItem.Offer.Product.Brand != null ? cartItem.Offer.Product.Brand.Name : null,
                BrandUrl =
                    cartItem.Offer.Product.Brand != null
                        ? urlHelper.RouteUrl("Brand", new { url = cartItem.Offer.Product.UrlPath })
                        : string.Empty,

                ColorHeader = SettingsCatalog.ColorsHeader,
                SizeHeader = SettingsCatalog.SizesHeader,
                TotalPrice = totalPriceString,
                TotalCount = totalCounts,
            };

            return obj;
        }
    }
}
