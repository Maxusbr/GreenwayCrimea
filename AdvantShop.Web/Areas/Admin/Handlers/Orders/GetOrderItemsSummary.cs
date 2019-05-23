using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Shipping;
using AdvantShop.Web.Admin.Models.OrdersEdit;
using AdvantShop.Web.Infrastructure.Extensions;

namespace AdvantShop.Web.Admin.Handlers.Orders
{
	public class GetOrderItemsSummary
	{
		private readonly int _orderId;
		private readonly UrlHelper _urlHelper;

		public GetOrderItemsSummary(int orderId)
		{
			_orderId = orderId;

			_urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
		}

		public OrderItemsSummaryModel Execute()
		{
			var order = _orderId != 0 ? OrderService.GetOrder(_orderId) : null;

			var orderCurrency =
				order != null && order.OrderCurrency != null
					? (Currency)order.OrderCurrency
					: CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3);

			if (order == null)
				return new OrderItemsSummaryModel() { OrderCurrency = orderCurrency };

			var shipping = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
			var payment = order.PaymentMethod;

			var model = new OrderItemsSummaryModel
			{
				OrderCurrency = orderCurrency,

				ProductsCost = order.OrderCertificates != null && order.OrderCertificates.Count > 0
					? order.OrderCertificates.Sum(x => x.Sum)
					: order.OrderItems.Sum(x => x.Price * x.Amount),

				ProductsWeight = order.OrderItems.Sum(x => (x.Weight > 0 ? x.Weight:
					ProductService.GetProduct(x.ProductID ?? 0)?.Weight ?? 0) * x.Amount),

				OrderDiscount = order.OrderDiscount,
				OrderDiscountValue = order.OrderDiscountValue,

				Certificate = order.Certificate,
				Coupon = order.Coupon != null ? order.Coupon.ToString() : null,
				BonusCost = order.BonusCost,

				ShippingName = order.ArchivedShippingName,
				ShippingCost = order.ShippingCost,
				ShippingType = shipping != null ? shipping.ShippingType.ToLower() : "",
				OrderPickPoint = order.OrderPickPoint,

				DeliveryDate = order.DeliveryDate != null ? order.DeliveryDate.Value.ToShortDateTime() : null,
				DeliveryTime = order.DeliveryTime,

				PaymentName = order.PaymentMethodName,
				PaymentCost = order.PaymentCost,
				PaymentDetails = order.PaymentDetails,
				PaymentKey = payment != null ? payment.PaymentKey.ToLower() : null,

				Taxes =
					order.Taxes.Select(
						tax =>
							new KeyValuePair<string, string>(
								(tax.ShowInPrice ? LocalizationService.GetResource("Core.Tax.IncludeTax") : "") + " " + tax.Name,
								tax.Sum.FormatPrice(orderCurrency))).ToList(),

				Sum = order.Sum,
				Paid = order.Payed,
			};

			model.ProductsDiscountPrice = order.GetOrderDiscountPrice();

			if (order.Coupon != null && order.Coupon.Value != 0)
			{
				switch (order.Coupon.Type)
				{
					case CouponType.Fixed:
						var productsPrice = order.OrderItems.Where(p => p.IsCouponApplied).Sum(p => p.Price * p.Amount);
						model.CouponPrice = productsPrice >= order.Coupon.Value ? order.Coupon.Value : productsPrice;
						break;
					case CouponType.Percent:
						model.CouponPrice = order.OrderItems.Where(p => p.IsCouponApplied).Sum(p => order.Coupon.Value * p.Price / 100 * p.Amount);
						break;
				}
			}

			if (model.Certificate != null)
				model.CertificatePrice = order.Certificate.Price;

			model.ShowSendBillingLink = !string.IsNullOrEmpty(order.ArchivedShippingName) && order.ShowBillingLink();

			if (payment != null && model.PaymentKey != null)
			{
				model.ShowPrintPaymentDetails = (new List<string>() { "sberbank", "bill", "billua", "check", "qiwi" }).Contains(model.PaymentKey);
				if (model.ShowPrintPaymentDetails)
				{
					model.PrintPaymentDetailsText = payment.ButtonText;
					model.PrintPaymentDetailsLink = _urlHelper.AbsoluteActionUrl(payment.PaymentKey, "PaymentReceipt", new { area = "", ordernumber = order.Number });
				}
			}

			model.BonusCard = order.GetOrderBonusCard();
			model.BonusCardPurchase = PurchaseService.GetByOrderId(order.OrderID);
			model.BonusesAvailable = model.BonusCard != null
				? (float)model.BonusCard.BonusesTotalAmount +
				  (model.BonusCardPurchase != null
					  ? (float)(model.BonusCardPurchase.MainBonusAmount + model.BonusCardPurchase.AdditionBonusAmount)
					  : 0)
				: 0;
			model.CanChangeBonusAmount = BonusSystemService.CanChangeBonusAmount(order, model.BonusCard, model.BonusCardPurchase);

			return model;
		}
	}
}
