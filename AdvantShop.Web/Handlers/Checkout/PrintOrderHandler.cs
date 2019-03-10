using System;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Taxes;
using AdvantShop.ViewModel.Checkout;

namespace AdvantShop.Handlers.Checkout
{
    public class PrintOrderHandler
    {
        private readonly Order _order;
        private readonly bool _showMap;

        public PrintOrderHandler(Order order, bool showMap)
        {
            _order = order;
            _showMap = showMap;
        }

        public PrintOrderViewModel Execute()
        {
            var currency = _order.OrderCurrency;

            var model = new PrintOrderViewModel()
            {
                Order = _order,
                OrderCurrency = currency,
                ShowStatusInfo = SettingsCheckout.PrintOrder_ShowStatusInfo,
                ShowMap = SettingsCheckout.PrintOrder_ShowMap && _showMap,
                MapType = SettingsCheckout.PrintOrder_MapType,
                MapAdress =
                    StringHelper.AggregateStrings(", ", _order.OrderCustomer.Country, _order.OrderCustomer.Region,
                        _order.OrderCustomer.City, _order.OrderCustomer.Street + " " + _order.OrderCustomer.House),
                ShowContacts = _order.OrderCertificates == null || _order.OrderCertificates.Count == 0
            };

            var productPrice = _order.OrderCertificates != null && _order.OrderCertificates.Count > 0
                                    ? _order.OrderCertificates.Sum(item => item.Sum)
                                    : _order.OrderItems.Sum(item => item.Amount * item.Price);
            var productsIgnoreDiscountPrice = _order.OrderItems.Where(item => item.IgnoreOrderDiscount).Sum(item => item.Price * item.Amount);

            model.ProductsPrice = productPrice.FormatPrice(currency);

            if (_order.OrderDiscount != 0 || _order.OrderDiscountValue != 0)
            {
                model.OrderDiscount = PriceFormatService.FormatDiscountPercent(productPrice - productsIgnoreDiscountPrice, _order.OrderDiscount,
                    _order.OrderDiscountValue, currency.CurrencyValue, currency.CurrencySymbol, currency.IsCodeBefore,
                    false);
            }

            if (_order.BonusCost != 0)
            {
                model.OrderBonus = _order.BonusCost.FormatPrice(currency);
            }

            if (_order.Certificate != null)
            {
                model.Certificate = _order.Certificate.Price.FormatPrice(currency);
            }

            if (_order.Coupon != null)
            {
                switch (_order.Coupon.Type)
                {
                    case CouponType.Fixed:
                        model.Coupon = String.Format("-{0} ({1})",
                                        _order.Coupon.Value.FormatPrice(currency),
                                        _order.Coupon.Code);
                        break;
                    case CouponType.Percent:
                        var productsWithCoupon = _order.OrderItems.Where(item => item.IsCouponApplied).Sum(item => item.Price * item.Amount);

                        model.Coupon = String.Format("-{0} ({1}%) ({2})",
                                        PriceFormatService.FormatPrice((productsWithCoupon * _order.Coupon.Value / 100).RoundPrice(currency), currency),
                                        _order.Coupon.Value.FormatPriceInvariant(),
                                        _order.Coupon.Code);
                        break;
                }
            }

            model.ShippingPrice = _order.ShippingCost.FormatPrice(currency);
            model.ShippingMethodName = _order.ArchivedShippingName +
                                       (_order.OrderPickPoint != null && !string.IsNullOrEmpty(_order.OrderPickPoint.PickPointAddress)
                                           ? " (" + _order.OrderPickPoint.PickPointAddress + ")"
                                           : "");
            
            model.PaymentPriceTitle = _order.PaymentCost >= 0
                                        ? LocalizationService.GetResource("Checkout.PaymentCost")
                                        : LocalizationService.GetResource("Checkout.PaymentDiscount");
            model.PaymentPrice = _order.PaymentCost.FormatPrice(currency);
            model.PaymentMethodName = _order.ArchivedPaymentName;

            model.Taxes = TaxService.GetOrderTaxes(_order.OrderItems, _order.Sum, _order.ShippingCost, _order.ShippingTaxType);
            model.TotalPrice = _order.Sum.FormatPrice(currency);

            return model;
        }
    }
}