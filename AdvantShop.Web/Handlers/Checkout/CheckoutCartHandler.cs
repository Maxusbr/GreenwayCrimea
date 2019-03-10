using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Models.Cart;
using AdvantShop.Models.Checkout;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Taxes;

namespace AdvantShop.Handlers.Checkout
{
    public class CheckoutCartHandler
    {
        #region Constructor

        private readonly UrlHelper _urlHelper;

        public CheckoutCartHandler()
        {
            _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        #endregion

        public CheckoutCartModel Get(CheckoutData checkoutData, ShoppingCart cart, float shippingPrice, float paymentCost, Currency currency)
        {
            var productsPrice = cart.TotalPrice;
            var discountOnTotalPrice = cart.DiscountPercentOnTotalPrice;
            var totalDiscount = cart.TotalDiscount;
            
            var bonusPrice = 0f;
            var bonusPlus = 0f;
           
            if (BonusSystem.IsActive)
            {
                //var bonusCard = BonusSystemService.GetCard(checkoutData.User.BonusCardId);
                //if (bonusCard != null)
                //{
                //    if (checkoutData.Bonus.UseIt)
                //    {
                //        bonusPrice = BonusSystemService.GetBonusCost(productsPrice + shippingPrice - totalDiscount, productsPrice - totalDiscount, (float)bonusCard.BonusesTotalAmount);
                //        totalDiscount += bonusPrice;
                //    }

                //    var bonusPlusPrice = BonusSystemService.GetBonusPlusCost(productsPrice + shippingPrice - totalDiscount, productsPrice - totalDiscount, (float)bonusCard.Grade.BonusPercent);
                //    if (bonusPlusPrice > 0)
                //    {
                //        bonusPlus = bonusPlusPrice;
                //    }
                //}
                //else if ((checkoutData.User.WantRegist || checkoutData.User.WantBonusCard) && BonusSystem.BonusFirstPercent != 0)
                //{
                //    bonusPlus = 
                //        BonusSystem.BonusesForNewCard + 
                //        BonusSystemService.GetBonusPlusCost(productsPrice + shippingPrice - totalDiscount, productsPrice - totalDiscount, (float)BonusSystem.BonusFirstPercent);
                //}

                var bonusCost = BonusSystemService.GetBonusCost(cart, shippingPrice, checkoutData.Bonus.UseIt,
                                                            (checkoutData.User.WantRegist || checkoutData.User.WantBonusCard));
                bonusPrice = bonusCost.BonusPrice;
                bonusPlus = bonusCost.BonusPlus;

                totalDiscount += bonusPrice;
            }

            var shippingTaxType = checkoutData.SelectShipping != null ? checkoutData.SelectShipping.TaxType : TaxType.Without;

            var taxesItems = TaxService.CalculateTaxes(cart, productsPrice - totalDiscount, shippingPrice, shippingTaxType);
            var taxesTotal = taxesItems.Where(tax => !tax.Key.ShowInPrice).Sum(item => item.Value);

            var totalTemp = (productsPrice + shippingPrice + taxesTotal - totalDiscount + paymentCost).RoundPrice(CurrencyService.CurrentCurrency.Rate);
            totalTemp = totalTemp > 0 ? totalTemp : 0;
            var totalPrice = totalTemp.FormatPrice();

            var model = new CheckoutCartModel
            {
                Items = cart.Select(item => new CheckoutCartItem()
                {
                    Name = item.Offer.Product.Name,
                    Amount = item.Amount,
                    Price = item.PriceWithDiscount.FormatPrice(),
                    Link = _urlHelper.RouteUrl("Product", new { url = item.Offer.Product.UrlPath }),
                    Cost = (item.PriceWithDiscount * item.Amount).FormatPrice(),
                    SelectedOptions = CustomOptionsService.DeserializeFromXml(item.AttributesXml, item.Offer.Product.Currency.Rate),
                    ColorName = item.Offer.Color != null ? item.Offer.Color.ColorName : null,
                    SizeName = item.Offer.Size != null ? item.Offer.Size.SizeName : null,
                }).ToList(),

                ColorHeader = SettingsCatalog.ColorsHeader,
                SizeHeader = SettingsCatalog.SizesHeader,

                Cost = productsPrice.FormatPrice(),
                Result = totalPrice,
                BuyOneClickEnabled = SettingsCheckout.BuyInOneClick && !SettingsCheckout.BuyInOneClickDisableInCheckout,
                ShowInCart = true,
                Delivery =
                    shippingPrice > 0
                        ? shippingPrice.FormatPrice()
                        : checkoutData.SelectShipping != null && !string.IsNullOrEmpty(checkoutData.SelectShipping.ZeroPriceMessage) ? checkoutData.SelectShipping.ZeroPriceMessage : null
            };

            if (paymentCost != 0)
                model.Payment = new CheckoutCartParam()
                {
                    Key =
                        paymentCost > 0
                            ? LocalizationService.GetResource("Checkout.PaymentCost")
                            : LocalizationService.GetResource("Checkout.PaymentDiscount"),
                    Value = paymentCost.FormatPrice()
                };

            if (discountOnTotalPrice > 0)
                model.Discount = new CheckoutCartParam()
                {
                    Key = discountOnTotalPrice.ToString(),
                    Value = ((cart.TotalPrice - cart.TotalPriceIgnoreDiscount) * discountOnTotalPrice / 100).RoundPrice(CurrencyService.CurrentCurrency.Rate).FormatPrice()
                };

            if (cart.Certificate != null)
                model.Certificate = cart.Certificate.Sum.FormatPrice();

            if (cart.Coupon != null)
            {
                model.Coupon = totalDiscount != 0
                    ? new CartCoupon()
                    {
                        Code = cart.Coupon.Code,
                        Price = totalDiscount.FormatPrice(),
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

            model.Taxes =
                taxesItems.Select(
                    tax =>
                        new CheckoutCartParam()
                        {
                            Key =
                                string.Format("{0} {1}",
                                    tax.Key.ShowInPrice ? LocalizationService.GetResource("Core.Tax.IncludeTax") : "",
                                    tax.Key.Name),
                            Value = tax.Value.FormatPrice()
                        }).ToList();

            if (bonusPrice != 0)
                model.Bonuses = bonusPrice.FormatPrice();

            if (bonusPlus != 0)
                model.BonusPlus = bonusPlus.FormatPrice();
            
            return model;
        }
    }
}