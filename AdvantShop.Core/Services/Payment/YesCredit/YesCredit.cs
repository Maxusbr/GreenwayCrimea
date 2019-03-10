using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using System.Text;
using AdvantShop.Taxes;
using Newtonsoft.Json;

namespace AdvantShop.Payment
{
    [PaymentKey("YesCredit")]
    public class YesCredit : PaymentMethod
    {
        /*
         * Примечание:
         * Заказы на сумму менее 15т. руб. не обарабатываются.
         * Похоже действует только в Москве и области, Санкт-Петербурге и области.
         * 
         */
        public string MerchantId { get; set; }
        public float MinimumPrice { get; set; }
        public float FirstPayment { get; set; }
        
        public override ProcessType ProcessType
        {
            get { return ProcessType.Javascript; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl | NotificationType.Handler; }
        }
        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.CancelUrl | UrlStatus.ReturnUrl | UrlStatus.NotificationUrl; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {YesCreditTemplate.MerchantId, MerchantId},
                    {YesCreditTemplate.MinimumPrice, MinimumPrice.ToString()},
                    {YesCreditTemplate.FirstPayment, FirstPayment.ToString()}
                };
            }
            set
            {
                MerchantId = value.ElementOrDefault(YesCreditTemplate.MerchantId);
                MinimumPrice = value.ElementOrDefault(YesCreditTemplate.MinimumPrice).TryParseFloat(0);
                FirstPayment =  value.ElementOrDefault(YesCreditTemplate.FirstPayment).TryParseFloat(10);
            }
        }


        public override string ProcessJavascript(Order order)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<script src=\"http://yes-credit.su/crem/js/jquery-ui-1.8.23.custom.min.js\" type=\"text/javascript\"></script>");
            sb.AppendLine("<script src=\"http://yes-credit.su/crem/js/crem.js\" type=\"text/javascript\"></script>");
            sb.AppendLine("<link href=\"http://yes-credit.su/crem/css/blizter.css\" rel=\"stylesheet\" type=\"text/css\" />");


            sb.AppendLine("<script type=\"text/javascript\"> ");
            sb.AppendFormat("var ycProducts = {0};\r\n", GetOrderJson(order));
            sb.AppendFormat("var ycMerchantId = {0};\r\n", MerchantId);
            sb.AppendFormat("var ycOrderId = \"{0}\";\r\n", order.OrderID);
            sb.AppendLine("</script>");

            return sb.ToString();
        }

        public override string ProcessJavascriptButton(Order order)
        {
            return "yescreditmodul(ycProducts, ycMerchantId, ycOrderId);";
        }

        private string GetOrderJson(Order order)
        {
            var orderItems = order.OrderItems;

            float subtotal = order.OrderItems.Sum(item => item.Amount * item.Price);

            // сумма налогов не включенных в стоимость товара
            var taxTotal = TaxService.GetOrderTaxes(order.OrderItems, order.Sum, order.ShippingCost, order.ShippingTaxType).Where(t => !t.ShowInPrice).Sum(t => t.Sum);
            var totalDiscount = (float)Math.Round(subtotal / 100 * order.OrderDiscount, 2);
            totalDiscount += order.OrderDiscountValue;

            taxTotal += order.PaymentCost;

            foreach (var item in orderItems)
            {
                var percent = item.Price * item.Amount * 100 / subtotal;

                item.Price += percent * taxTotal / (100 * item.Amount);
                item.Price -= percent * totalDiscount / (100 * item.Amount);
            }

            var shopCartItems = orderItems.Select(item => new
            {
                model = item.Name,
                count = item.Amount,
                price = (float)Math.Round(item.Price * item.Amount)
            }).ToList();


            var shippingCost = order.ShippingCost / order.OrderCurrency.CurrencyValue;

            if (shippingCost > 0)
            {
                shopCartItems.Add(new
                {
                    model = "Доставка",
                    count = 1F,
                    price = (float)Math.Round(shippingCost)
                });
            }

            return JsonConvert.SerializeObject(shopCartItems);
        }
    }
}