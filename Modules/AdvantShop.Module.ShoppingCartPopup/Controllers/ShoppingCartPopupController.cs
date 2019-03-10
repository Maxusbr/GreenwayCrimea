using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Module.ShoppingCartPopup.Models;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;
using System.Collections.Generic;

namespace AdvantShop.Module.ShoppingCartPopup.Controllers
{
    public class ShoppingCartPopupController : ModuleController
    {

        public ActionResult BodyBottomScript()
        {
            var isCart = (string)Request.RequestContext.RouteData.Values["controller"] == "Cart";
            var goToCheckout = ModuleSettingsProvider.GetSettingValue<bool>("goToCheckout", ShoppingCartPopup.ModuleID);

            return
                Content(
                    string.Format(
                        "<div data-oc-lazy-load=\"['modules/shoppingcartpopup/scripts/cartPopup.js']\"><div data-cart-popup data-button-confirm-url=\"{0}\"></div></div>",
                         isCart || goToCheckout ? Url.RouteUrl("Checkout") : Url.RouteUrl("Cart")));
        }

        [HttpGet]
        public ActionResult GetCartPopup(int cartId)
        {
            var shpCart = ShoppingCartService.CurrentShoppingCart;
            var cartItem = shpCart.Find(x => x.ShoppingCartItemId == cartId);

            if (cartItem == null)
                return Json(new { status = "error" });

            var totalItems = shpCart.TotalItems;
            var totalDiscount = shpCart.TotalDiscount;
            var totalPrice = shpCart.TotalPrice;

            var totalPriceString =
                totalPrice - totalDiscount > 0
                    ? (totalPrice - totalDiscount).FormatPrice()
                    : 0F.FormatPrice();

            var totalCounts =
                String.Format(
                    "В вашей <a href=\"cart\">корзине</a> <span class=\"cart-popup-count\">{0}</span> {1}<br> на сумму  <span class=\"cart-popup-cost\">{2}</span>",
                    totalItems, Strings.Numerals(totalItems, "пусто", "товар", "товара", "товаров"), totalPriceString);

            var offer = cartItem.Offer;
            var product = cartItem.Offer.Product;

            var showMode = ModuleSettingsProvider.GetSettingValue<string>("showmode", ShoppingCartPopup.ModuleID);

            var prodMinAmount = product.MinAmount == null
                            ? product.Multiplicity
                            : product.Multiplicity > product.MinAmount
                                ? product.Multiplicity
                                : product.MinAmount.Value;

            var obj = new
            {
                status = "success",
                showCart = true,

                Sku = offer.ArtNo,
                Name = product.Name,
                Link = Url.RouteUrl("Product", new { url = product.UrlPath }),
                Amount = cartItem.Amount,
                Price = cartItem.PriceWithDiscount.FormatPrice(),
                Cost = (cartItem.PriceWithDiscount * cartItem.Amount).FormatPrice(),
                PhotoPath = offer.Photo.ImageSrcSmall(),
                PhotoAlt = product.Name,
                SelectedOptions = CustomOptionsService.DeserializeFromXml(cartItem.AttributesXml, product.Currency.Rate),
                ColorName = offer.Color != null ? offer.Color.ColorName : null,
                SizeName = offer.Size != null ? offer.Size.SizeName : null,
                AvailableAmount = offer.Amount,
                CanOrderByRequest = offer.CanOrderByRequest,
                MinAmount = prodMinAmount,
                MaxAmount = product.MaxAmount ?? Int32.MaxValue,
                Multiplicity = product.Multiplicity > 0 ? product.Multiplicity : 1,
                IsGift = cartItem.IsGift,
                Rating = product.Ratio,
                Reviews = GetReviewsCount(product),
                ColorHeader = SettingsCatalog.ColorsHeader,
                SizeHeader = SettingsCatalog.SizesHeader,
                TotalPrice = totalPriceString,
                TotalCount = totalCounts,

                RelatedProducts = GetrelatedProducts(product.ProductId, showMode)
            };

            return Json(obj);
        }

        #region Help methods

        private string GetReviewsCount(Product product)
        {
            var reviewsCountString = string.Empty;

            if (AttachedModules.GetModules<IModuleReviews>().Any())
            {
                foreach (var module in AttachedModules.GetModules<IModuleReviews>())
                {
                    var instance = (IModuleReviews)Activator.CreateInstance(module);
                    reviewsCountString = instance.GetReviewsCount(Url.AbsoluteRouteUrl("Product", new { url = product.UrlPath }));
                }
            }
            else
            {
                var reviewsCount = SettingsCatalog.ModerateReviews
                    ? ReviewService.GetCheckedReviewsCount(product.ProductId, EntityType.Product)
                    : ReviewService.GetReviewsCount(product.ProductId, EntityType.Product);

                reviewsCountString = string.Format("{0} {1}", reviewsCount, Strings.Numerals(reviewsCount, "отзывов", "отзыв", "отзыва", "отзывов"));
            }

            return reviewsCountString;
        }

        private string GetrelatedProducts(int productId, string showMode)
        {
            if (showMode == "none")
                return string.Empty;

            List<ProductModel> relatedProducts = new List<ProductModel>();
            var model = new RelatedProductsViewModel();

            switch (SettingsDesign.RelatedProductSourceType)
            {
                case SettingsDesign.eRelatedProductSourceType.Default:
                    relatedProducts =
                   ProductService.GetRelatedProducts(productId,
                       showMode == "related" ? RelatedType.Related : RelatedType.Alternative);

                    model = new RelatedProductsViewModel()
                    {
                        RelatedProducts = relatedProducts,
                        PhotoWidth = SettingsPictureSize.SmallProductImageWidth,
                        PhotoHeight = SettingsPictureSize.SmallProductImageHeight,
                        DisplayRating = SettingsCatalog.EnableProductRating,
                        Title = showMode == "related" ? SettingsCatalog.RelatedProductName : SettingsCatalog.AlternativeProductName,
                        DisplayBuyButton = SettingsCatalog.DisplayBuyButton,
                        DisplayPreOrderButton = SettingsCatalog.DisplayBuyButton,
                        BuyButtonText = SettingsCatalog.BuyButtonText,
                        PreOrderButtonText = SettingsCatalog.PreOrderButtonText,
                        ColorImageHeight = SettingsPictureSize.ColorIconHeightCatalog,
                        ColorImageWidth = SettingsPictureSize.ColorIconWidthCatalog,
                        AllowBuyOutOfStockProducts = SettingsCheckout.OutOfStockAction == eOutOfStockAction.Cart
                    };

                    return this.RenderForce("~/Modules/ShoppingCartPopup/Views/ShoppingCartPopup/RelatedProducts.cshtml", model);

                case SettingsDesign.eRelatedProductSourceType.FromCategory:

                    relatedProducts = ProductService.GetRelatedProducts(productId, showMode == "related" ? RelatedType.Related : RelatedType.Alternative);
                    if (relatedProducts == null || relatedProducts.Count == 0)
                    {
                        var product = ProductService.GetProduct(productId);
                        relatedProducts = ProductService.GetRelatedProductsFromCategory(product, showMode == "related" ? RelatedType.Related : RelatedType.Alternative);
                    }
                    if (relatedProducts != null)
                    {
                        model = new RelatedProductsViewModel()
                        {
                            RelatedProducts = relatedProducts,
                            PhotoWidth = SettingsPictureSize.SmallProductImageWidth,
                            PhotoHeight = SettingsPictureSize.SmallProductImageHeight,
                            DisplayRating = SettingsCatalog.EnableProductRating,
                            Title = showMode == "related" ? SettingsCatalog.RelatedProductName : SettingsCatalog.AlternativeProductName,
                            DisplayBuyButton = SettingsCatalog.DisplayBuyButton,
                            DisplayPreOrderButton = SettingsCatalog.DisplayBuyButton,
                            BuyButtonText = SettingsCatalog.BuyButtonText,
                            PreOrderButtonText = SettingsCatalog.PreOrderButtonText,
                            ColorImageHeight = SettingsPictureSize.ColorIconHeightCatalog,
                            ColorImageWidth = SettingsPictureSize.ColorIconWidthCatalog,
                            AllowBuyOutOfStockProducts = SettingsCheckout.OutOfStockAction == eOutOfStockAction.Cart
                    };

                        return this.RenderForce("~/Modules/ShoppingCartPopup/Views/ShoppingCartPopup/RelatedProducts.cshtml", model);
                    }
                    else
                        return string.Empty;

                case SettingsDesign.eRelatedProductSourceType.FromModule:
                    return this.RenderForce("~/Modules/ShoppingCartPopup/Views/ShoppingCartPopup/RelatedProductsFromModule.cshtml", null);

                default: return string.Empty;
            }
        }
        #endregion
    }
}
