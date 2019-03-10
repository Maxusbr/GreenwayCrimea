using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Models.Cart;
using AdvantShop.Orders;
using AdvantShop.Web.Infrastructure.Extensions;

namespace AdvantShop.Handlers.Cart
{
    public class GetCartHandler
    {
        private readonly UrlHelper _urlHelper;

        public GetCartHandler()
        {
            _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public CartModel Get()
        {
            var cart = ShoppingCartService.CurrentShoppingCart;

            var cartProducts =
                (from item in cart
                 select new CartItemModel()
                 {
                     Sku = item.Offer.ArtNo,
                     Name = item.Offer.Product.Name,
                     Link = _urlHelper.AbsoluteRouteUrl("Product", new { url = item.Offer.Product.UrlPath }),
                     Amount = item.Amount,
                     Price = item.Price.FormatPrice(),
                     PriceWithDiscount = item.PriceWithDiscount.FormatPrice(),
                     Discount = item.Discount,
                     DiscountText = item.Discount.GetText(),
                     Cost = (item.PriceWithDiscount * item.Amount).FormatPrice(),
                     PhotoPath = item.Offer.Photo.ImageSrcXSmall(),
                     PhotoAlt = item.Offer.Product.Name,
                     ShoppingCartItemId = item.ShoppingCartItemId,
                     SelectedOptions = CustomOptionsService.DeserializeFromXml(item.AttributesXml, item.Offer.Product.Currency.Rate),
                     ColorName = item.Offer.Color != null ? item.Offer.Color.ColorName : null,
                     SizeName = item.Offer.Size != null ? item.Offer.Size.SizeName : null,
                     Avalible = GetAvalibleState(item),
                     AvailableAmount = item.Offer.Amount,
                     MinAmount = item.Offer.Product.MinAmount == null
                            ? item.Offer.Product.Multiplicity
                            : item.Offer.Product.Multiplicity > item.Offer.Product.MinAmount
                                ? item.Offer.Product.Multiplicity
                                : item.Offer.Product.MinAmount.Value,
                     MaxAmount = item.Offer.Product.MaxAmount ?? Int32.MaxValue,
                     Multiplicity = item.Offer.Product.Multiplicity > 0 ? item.Offer.Product.Multiplicity : 1,
                     FrozenAmount = item.FrozenAmount,
                     IsGift = item.IsGift,
                     Unit = item.Offer.Product.Unit
                 }).ToList();

            var totalPrice = cart.TotalPrice;
            var totalDiscount = cart.TotalDiscount;
            var priceWithDiscount = totalPrice - totalDiscount;
            var totalItems = cart.TotalItems;
            var discountOnTotalPrice = cart.DiscountPercentOnTotalPrice;

            var count = string.Format("{0} {1}",
                totalItems == 0 ? "" : totalItems.ToString(CultureInfo.InvariantCulture),
                Strings.Numerals(totalItems,
                    LocalizationService.GetResource("Cart.Product0"),
                    LocalizationService.GetResource("Cart.Product1"),
                    LocalizationService.GetResource("Cart.Product2"),
                    LocalizationService.GetResource("Cart.Product5")));

            float bonusPlus = 0;

            if (totalPrice > 0 && BonusSystem.IsActive)
            {
                bonusPlus = BonusSystemService.GetBonusCost(cart).BonusPlus;
            }

            var showConfirmButtons = true;
            foreach (var module in AttachedModules.GetModules<IShoppingCart>())
            {
                var moduleObject = (IShoppingCart)Activator.CreateInstance(module);
                showConfirmButtons &= moduleObject.ShowConfirmButtons;
                if (module.FullName.Contains("OrderConfirmationInShoppingCart"))
                {
                    showConfirmButtons = false;
                }
            }

            string isValidCart = IsValidCart(cartProducts, totalItems, totalPrice);

            var model = new CartModel
            {
                CartProducts = cartProducts,
                ColorHeader = SettingsCatalog.ColorsHeader,
                SizeHeader = SettingsCatalog.SizesHeader,
                Count = count,
                TotalItems = totalItems,
                BonusPlus = bonusPlus > 0 ? bonusPlus.FormatPrice() : null,
                Valid = isValidCart,

                CouponInputVisible =
                    cart.HasItems && cart.Coupon == null && cart.Certificate == null &&
                    CustomerContext.CurrentCustomer.CustomerGroup.CustomerGroupId ==
                    CustomerGroupService.DefaultCustomerGroup && SettingsCheckout.DisplayPromoTextbox,

                ShowConfirmButtons = showConfirmButtons,
                ShowBuyInOneClick = showConfirmButtons && SettingsCheckout.BuyInOneClick,
                BuyInOneClickText = SettingsCheckout.BuyInOneClickLinkText,
                //BuyInOneClickText = LocalizationService.GetResource("Cart.BuyInOneClick.BuyInOneClickText"),

                TotalProductPrice = totalPrice.FormatPrice(),
                TotalPrice = priceWithDiscount > 0 ? priceWithDiscount.FormatPrice() : 0F.FormatPrice(),

                DiscountPrice =
                    discountOnTotalPrice > 0
                        ? PriceFormatService.FormatDiscountPercent(totalPrice - cart.TotalPriceIgnoreDiscount, discountOnTotalPrice, 0, true)
                        : null,

                Certificate = cart.Certificate != null ? cart.Certificate.Sum.FormatPrice() : null,
                MobileIsFullCheckout = SettingsMobile.IsFullCheckout && showConfirmButtons
            };

            if (cart.Coupon != null)
            {
                model.Coupon = totalDiscount != 0
                    ? new CartCoupon()
                    {
                        Code = cart.Coupon.Code,
                        Price = (cart.Coupon.Type == CouponType.Percent ? totalDiscount : cart.Coupon.GetRate()).FormatPrice(),
                        Percent =
                            cart.Coupon.Type == CouponType.Percent
                                ? cart.Coupon.Value.FormatPriceInvariant()
                                : null
                    }
                    : new CartCoupon()
                    {
                        Code = cart.Coupon.Code,
                        Price = 0f.FormatPrice(),
                        NotApplied = true,
                    };
            }

            return model;
        }


        private static string GetAvalibleState(ShoppingCartItem item)
        {
            if (!item.Offer.Product.Enabled || !item.Offer.Product.CategoryEnabled)
            {
                return LocalizationService.GetResource("Cart.Error.NotAvailable") + " 0 " + item.Offer.Product.Unit;
            }

            if ((item.AddedByRequest && !SettingsCheckout.DenyToByPreorderedProductsWithZerroAmount )|| item.IsGift)
                return string.Empty;
                       

            if (SettingsCheckout.AmountLimitation && (item.Amount > item.Offer.Amount) && !(item.Offer.Amount <= 0 && SettingsCheckout.OutOfStockAction == eOutOfStockAction.Cart))
            {
                return LocalizationService.GetResource("Cart.Error.NotAvailable") + " " + (item.Offer.Amount < 0 ? 0 : item.Offer.Amount) +
                       " " + item.Offer.Product.Unit;
            }

            if (item.Amount > item.Offer.Product.MaxAmount)
            {
                return LocalizationService.GetResource("Cart.Error.MaximumOrder") + " " + item.Offer.Product.MaxAmount + " " + item.Offer.Product.Unit;
            }

            if (item.Amount < item.Offer.Product.MinAmount)
            {
                return LocalizationService.GetResource("Cart.Error.MinimumOrder") + " " + item.Offer.Product.MinAmount + " " + item.Offer.Product.Unit;
            }


            return string.Empty;
        }

        private string IsValidCart(List<CartItemModel> cartProducts, float itemsCount, float totalPrice)
        {
            if (itemsCount == 0)
                return LocalizationService.GetResource("Cart.Error.NoProducts");

            var minimumOrderPrice = CustomerGroupService.GetMinimumOrderPrice();

            if (totalPrice < minimumOrderPrice)
            {
                return string.Format(LocalizationService.GetResource("Cart.Error.MinimalOrderPrice"), minimumOrderPrice.FormatPrice(),
                    (minimumOrderPrice - totalPrice).FormatPrice());
            }

            if (cartProducts.Any(x => x.Avalible.IsNotEmpty()))
                return LocalizationService.GetResource("Cart.Error.NotAvailableProducts");

            return string.Empty;
        }
    }
}