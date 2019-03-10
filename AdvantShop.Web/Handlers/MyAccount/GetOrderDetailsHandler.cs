using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Shipping;
using AdvantShop.Shipping.CheckoutRu;
using AdvantShop.Shipping.Edost;
using AdvantShop.Shipping.Sdek;
using AdvantShop.Taxes;

namespace AdvantShop.Handlers.MyAccount
{
    public class GetOrderDetailsHandler
    {
        private readonly Order _order;
        private readonly UrlHelper _urlHelper;

        public GetOrderDetailsHandler(Order order)
        {
            _order = order;
            _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        // TODO: return class
        public object Get()
        {
            var productsPrice = _order.OrderItems.Sum(item => item.Amount * item.Price);
            string couponPrice = string.Empty, couponPersent = string.Empty;

            if (_order.Coupon != null)
            {
                couponPrice = _order.Coupon.Value != 0
                                  ? string.Format("-{0} ({1})",
                                                  _order.TotalDiscount.FormatPrice(_order.OrderCurrency),
                                                  _order.Coupon.Code)
                                  : string.Empty;

                couponPersent = _order.Coupon.Type == CouponType.Percent
                                    ? PriceFormatService.FormatPriceInvariant(_order.Coupon.Value)
                                    : string.Empty;
            }
            
            var shippingInfo = new CustomerContact
            {
                Name = _order.OrderCustomer.FirstName + " " + _order.OrderCustomer.LastName,
                Street = _order.OrderCustomer.GetCustomerAddress(),
                City = _order.OrderCustomer.City,
                Country = _order.OrderCustomer.Country,
                Region = string.IsNullOrEmpty(_order.OrderCustomer.Region)
                                     ? "-"
                                     : _order.OrderCustomer.Region,
                Zip = string.IsNullOrEmpty(_order.OrderCustomer.Zip)
                              ? "-"
                              : _order.OrderCustomer.Zip,
            };

            var items = new object[_order.OrderItems.Count];
            for (int i = 0; i < _order.OrderItems.Count; i++)
            {
                string photoPath = "";
                string url = string.Empty;

                var productID = _order.OrderItems[i].ProductID;
                if (productID != null)
                {
                    var product = ProductService.GetProduct((int)productID);
                    photoPath = product != null && product.Photo.IsNotEmpty()
                        ? FoldersHelper.GetImageProductPath(ProductImageType.XSmall, product.Photo, false)
                        : "images/nophoto_xsmall.jpg";
                    if (product != null)
                    {
                        url = _urlHelper.RouteUrl("Product", new { url = product.UrlPath });
                    }
                }

                items[i] = new
                {
                    _order.OrderItems[i].Name,
                    Price = PriceFormatService.FormatPrice(_order.OrderItems[i].Price, _order.OrderCurrency),
                    Amount = _order.OrderItems[i].Amount,
                    ArtNo = _order.OrderItems[i].ArtNo,
                    Id = _order.OrderItems[i].ProductID,
                    TotalPrice = PriceFormatService.FormatPrice(_order.OrderItems[i].Amount * _order.OrderItems[i].Price, _order.OrderCurrency),
                    Photo = photoPath,
                    Url = url,
                    ColorHeader = SettingsCatalog.ColorsHeader,
                    SizeHeader = SettingsCatalog.SizesHeader,
                    ColorName = _order.OrderItems[i].Color,
                    SizeName = _order.OrderItems[i].Size,
                    CustomOptions = _order.OrderItems[i].SelectedOptions,
                    Width = _order.OrderItems[i].Width,
                    Length = _order.OrderItems[i].Length,
                    Height = _order.OrderItems[i].Height,
                };
            }

            var isEnabledPayment = true;
            object[] payments = null;
            var onclickEvent = string.Empty;
            if (_order.OrderStatusId != OrderStatusService.CanceledOrderStatus && (!SettingsCheckout.ManagerConfirmed || _order.ManagerConfirmed))
            {
                var allPaymentstemp = PaymentService.GetAllPaymentMethods(true);//.Where(payment => payment.Type != PaymentType.GiftCertificate);
                var allPayments = new List<PaymentMethod>();
                foreach (var method in allPaymentstemp)
                {
                    if (_order.ShippingMethodId != 0 && ShippingMethodService.IsPaymentNotUsed(_order.ShippingMethodId, method.PaymentMethodId))
                        continue;

                    var methodType = method.GetType();

                    if (typeof (CashOnDelivery).IsAssignableFrom(methodType))
                    {
                        if (!typeof (EdostCashOnDeliveryOption).IsAssignableFrom(methodType) &&
                            methodType != typeof (SdekOption) && methodType != typeof (CheckoutOption))
                            continue;
                    }

                    allPayments.Add(method);
                }

                allPayments = PaymentService.UseGeoMapping(allPayments, _order.OrderCustomer.Country, _order.OrderCustomer.City);
                isEnabledPayment = allPayments.Any(item => item.PaymentMethodId == _order.PaymentMethodId);
                payments = new object[allPayments.Count()];
                for (var i = 0; i < allPayments.Count(); ++i)
                {
                    payments[i] = new
                    {
                        id = allPayments.ElementAt(i).PaymentMethodId,
                        name = allPayments.ElementAt(i).Name
                    };
                }

                onclickEvent = isEnabledPayment
                    ? OrderService.ProcessOrder(_order, PageWithPaymentButton.myaccount)
                    : OrderService.ProcessOrder(_order, PageWithPaymentButton.myaccount, allPayments.Any() ? allPayments.ElementAt(0) : null);
            }
            else
            {
                payments = new object[1] {
                 new {
                       id = _order.PaymentMethodId,
                        name = _order.PaymentMethodName
                 }
                };
            }

            var taxes = TaxService.GetOrderTaxes(_order.OrderItems, _order.Sum, _order.ShippingCost, _order.ShippingTaxType);

            var orderDetails = new
            {
                Email = CustomerContext.CurrentCustomer.EMail,
                _order.OrderID,
                _order.Number,
                _order.Code,
                StatusName = _order.OrderStatus.Hidden ? _order.PreviousStatus : _order.OrderStatus.StatusName,
                OrderItems = items,
                OrderCertificates = _order.OrderCertificates != null && _order.OrderCertificates.Count > 0
                                            ? _order.OrderCertificates.Select(x => new { Code = x.CertificateCode, Price = x.Sum.FormatPrice(_order.OrderCurrency) })
                                            : null,
                BillingAddress = shippingInfo,
                ShippingName = _order.OrderCustomer.FirstName + " " + _order.OrderCustomer.LastName,
                ShippingInfo = shippingInfo,
                _order.ArchivedShippingName,
				ShippingAddress = _order.OrderPickPoint != null ? _order.OrderPickPoint.PickPointAddress : null,
                _order.PaymentMethodId,
                _order.PaymentMethodName,
                TotalDiscountPrice = _order.TotalDiscount.FormatPrice(_order.OrderCurrency),
                ProductsPrice = productsPrice.FormatPrice(_order.OrderCurrency),
                TotalDiscount = _order.OrderDiscount,
                CertificatePrice = _order.Certificate != null
                                        ? _order.Certificate.Price.FormatPrice(_order.OrderCurrency)
                                        : string.Empty,
                TotalPrice = _order.Sum.FormatPrice(_order.OrderCurrency),
                CouponPrice = couponPrice,
                CouponPersent = couponPersent,
                ShippingPrice = _order.ShippingCost.FormatPrice(_order.OrderCurrency),
                PaymentPrice = _order.PaymentCost != 0 ?
                                  (_order.PaymentCost > 0 ? "+" : "") + _order.PaymentCost.FormatPrice(_order.OrderCurrency)
                                : string.Empty,
                PaymentPriceText = _order.PaymentCost != 0 
                                        ? LocalizationService.GetResource("Checkout.PaymentCost")
                                        : LocalizationService.GetResource("Checkout.PaymentDiscount"),
                _order.CustomerComment,
                Payments = payments,
                _order.PaymentDetails,
                PaymentForm = onclickEvent,
                Canceled = _order.OrderStatusId == OrderStatusService.CanceledOrderStatus,
                Payed = _order.Payed,
                Bonus = _order.BonusCost.FormatPrice(_order.OrderCurrency),
                TaxesNames = taxes.Select(tax => tax.Name).AggregateString(','),
                TaxesPrice = taxes.Sum(x => x.Sum).FormatPrice(_order.OrderCurrency),

                _order.TrackNumber,
            };

            return orderDetails;
        }
    }
}