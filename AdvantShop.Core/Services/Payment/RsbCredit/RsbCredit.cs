using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using AdvantShop.Taxes;

namespace AdvantShop.Payment
{
    [PaymentKey("RsbCredit")]
    public class RsbCredit : PaymentMethod, ICreditPaymentMethod
    {
        private const float MinOrderPrice = 3000;
        private const int DefFirstPayment = 10;

        public string PartnerId { get; set; }
        public float MinimumPrice { get; set; }
        public float FirstPayment { get; set; }

        public decimal OrderSum { get; set; }
        

        public override ProcessType ProcessType
        {
            get { return ProcessType.Javascript; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {RsbCreditTemplate.PartnerId, PartnerId},
                               {RsbCreditTemplate.MinimumPrice, MinimumPrice.ToString()},
                               {RsbCreditTemplate.FirstPayment, FirstPayment.ToString()}
                           };
            }
            set
            {
                PartnerId = value.ElementOrDefault(RsbCreditTemplate.PartnerId);
                MinimumPrice = value.ElementOrDefault(RsbCreditTemplate.MinimumPrice).TryParseFloat(MinOrderPrice);
                FirstPayment = value.ElementOrDefault(RsbCreditTemplate.FirstPayment).TryParseFloat(DefFirstPayment);
            }
        }

        public override string ProcessJavascript(Order order)
        {
            int orderItemsCount = 0;
            string orderItems = "";

            var subtotal = order.OrderItems.Sum(item => item.Amount * item.Price);

            // сумма налогов не включенных в стоимость товара
            var taxTotal = TaxService.GetOrderTaxes(order.OrderItems, order.Sum, order.ShippingCost, order.ShippingTaxType).Where(t => !t.ShowInPrice).Sum(t => t.Sum);
            var totalDiscount = (float)Math.Round(subtotal / 100 * order.OrderDiscount, 2);
            totalDiscount += order.OrderDiscountValue;

            taxTotal += order.PaymentCost;

            foreach (var item in order.OrderItems)
            {
                var percent = item.Price * item.Amount * 100 / subtotal;

                item.Price += percent * taxTotal / (100 * item.Amount);
                item.Price -= percent * totalDiscount / (100 * item.Amount);

                orderItemsCount++;
                orderItems += string.Format("&TC_{0}={1}&TPr_{0}={2}&TName_{0}={3}",
                                            orderItemsCount, item.Amount, item.Price.ToString("F"), HttpUtility.UrlEncode(item.Name));
            }

            var shippingCost = order.ShippingCost / order.OrderCurrency.CurrencyValue;

            if (shippingCost > 0)
            {
                orderItemsCount++;
                orderItems += string.Format("&TC_{0}={1}&TPr_{0}={2}&TName_{0}={3}",
                                            orderItemsCount, 1, ((float)Math.Round(shippingCost)).ToString("F"), HttpUtility.UrlEncode("Доставка"));
            }

            var link = String.Format("https://www.onlinecredit.ru/sites/anketa/minipotreb.php?idTpl={0}&TTName={1}&Order={2}&TCount={3}{4}&UserName={5}&UserLastName={6}&UserMail={7}",
                                        PartnerId,
                                        SettingsMain.SiteUrlPlain,
                                        order.OrderID,
                                        orderItemsCount,
                                        orderItems,
                                        order.OrderCustomer.FirstName,
                                        order.OrderCustomer.LastName,
                                        order.OrderCustomer.Email != "admin" ? order.OrderCustomer.Email : "");

            var sb = new StringBuilder();

            sb.Append("<script type=\"text/javascript\"> ");
            sb.AppendLine("function openrsbcredit() {{");
            sb.AppendFormat("window.open(\"{0}\", \"_blank\");\r\n", link);
            sb.AppendLine("}} ");
            sb.AppendLine("</script>");

            return sb.ToString();
        }

        public override string ProcessJavascriptButton(Order order)
        {
            return "openrsbcredit();";
        }
    }
}