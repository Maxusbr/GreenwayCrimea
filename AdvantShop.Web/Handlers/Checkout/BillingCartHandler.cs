using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Models.Cart;
using AdvantShop.Models.Checkout;
using AdvantShop.Orders;
using AdvantShop.Configuration;

namespace AdvantShop.Handlers.Checkout
{
    public class BillingCartHandler
    {
        #region Constructor

        private readonly UrlHelper _urlHelper;

        public BillingCartHandler()
        {
            _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        #endregion

        public CheckoutCartModel Get(Order order)
        {
            var productsPrice = order.OrderCertificates.Any()
                                    ? order.OrderCertificates.Sum(x => x.Sum)
                                    : order.OrderItems.Sum(x => x.Price * x.Amount);

            var currency = order.OrderCurrency;
            var renderCurrency = currency;

            var model = new CheckoutCartModel()
            {
                Items =
                    (from item in order.OrderItems
                        where item.ProductID != null
                        let product = ProductService.GetProduct((int) item.ProductID)
                        select new CheckoutCartItem()
                        {
                            Name = item.Name,
                            Amount = item.Amount,
                            Price = item.Price.RoundAndFormatPrice(renderCurrency, currency.CurrencyValue),
                            Link = product != null ? _urlHelper.RouteUrl("Product", new {url = product.UrlPath}) : string.Empty,
                            Cost = (item.Price * item.Amount).RoundAndFormatPrice(renderCurrency, currency.CurrencyValue),
                            SelectedOptions = item.SelectedOptions,
                            ColorName = item.Color,
                            SizeName = item.Size,
                        }).ToList(),

                ColorHeader = SettingsCatalog.ColorsHeader,
                SizeHeader = SettingsCatalog.SizesHeader,

                Cost = productsPrice.RoundAndFormatPrice(renderCurrency, currency.CurrencyValue),
                Result = order.Sum.RoundAndFormatPrice(renderCurrency, currency.CurrencyValue),
                BuyOneClickEnabled = false
            };

            if (order.ShippingCost != 0)
                model.Delivery = order.ShippingCost.RoundAndFormatPrice(renderCurrency, currency.CurrencyValue);

            if (order.PaymentCost != 0)
                model.Payment = new CheckoutCartParam()
                {
                    Key =
                        order.PaymentCost > 0
                            ? LocalizationService.GetResource("Checkout.PaymentCost")
                            : LocalizationService.GetResource("Checkout.PaymentDiscount"),
                    Value = order.PaymentCost.RoundAndFormatPrice(renderCurrency, currency.CurrencyValue)
                };

            if (order.OrderDiscount > 0)
                model.Discount = new CheckoutCartParam()
                {
                    Key = order.OrderDiscount.ToString(),
                    Value = (order.OrderDiscount*productsPrice /100).RoundAndFormatPrice(renderCurrency, currency.CurrencyValue)
                };

            if (order.BonusCost > 0)
            {
                model.Bonuses = order.BonusCost.RoundAndFormatPrice(renderCurrency, currency.CurrencyValue);
            }

            if (order.Certificate != null)
                model.Certificate = order.Certificate.Price.RoundAndFormatPrice(renderCurrency, currency.CurrencyValue);

            if (order.Coupon != null)
                model.Coupon = new CartCoupon()
                {
                    Code = order.Coupon.Code,
                    Price = order.Coupon.Type == CouponType.Fixed 
                            ? order.Coupon.Value.RoundAndFormatPrice(renderCurrency, currency.CurrencyValue) 
                            : order.TotalDiscount.RoundAndFormatPrice(renderCurrency, currency.CurrencyValue),
                    Percent =
                        order.Coupon.Type == CouponType.Percent
                            ? order.Coupon.Value.FormatPriceInvariant()
                            : null
                };

            model.Taxes =
                order.Taxes.Select(
                    tax =>
                        new CheckoutCartParam()
                        {
                            Key = string.Format("{0} {1}", tax.ShowInPrice ? LocalizationService.GetResource("Core.Tax.IncludeTax") : "", tax.Name),
                            Value = tax.Sum.RoundAndFormatPrice(renderCurrency, currency.CurrencyValue)
                        }).ToList();

            return model;
        }
    }
}