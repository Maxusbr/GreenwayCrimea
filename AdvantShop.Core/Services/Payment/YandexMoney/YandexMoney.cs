//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    [PaymentKey("YandexMoney")]
    public class YandexMoney : PaymentMethod
    {
        public string ShopId { get; set; }
        public string ScId { get; set; }
        public string YaPaymentType { get; set; }

        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {YandexMoneyTemplate.ShopId, ShopId},
                    {YandexMoneyTemplate.ScId, ScId},
                    {YandexMoneyTemplate.YaPaymentType, YaPaymentType}
                };
            }
            set
            {
                ShopId = value.ElementOrDefault(YandexMoneyTemplate.ShopId);
                ScId = value.ElementOrDefault(YandexMoneyTemplate.ScId);
                YaPaymentType = value.ElementOrDefault(YandexMoneyTemplate.YaPaymentType);
            }
        }

        public override void ProcessForm(Order order)
        {
            new PaymentFormHandler
            {
                Url = "http://money.yandex.ru/eshop.xml",
                InputValues =
                {
                    {"scid", ScId},
                    {"shopId", ShopId},
                    {"CustName", order.OrderCustomer.FirstName + order.OrderCustomer.LastName},
                    {"CustEMail", order.OrderCustomer.Email},
                    {"CustAddr", order.OrderCustomer.GetCustomerAddress()},
                    {"OrderDetails", string.Join(",", order.OrderItems.Select(item => item.Name + " - " + item.Amount))},
                    {"Sum", (order.Sum*GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", ".")},
                    {"paymentType", YaPaymentType}
                }
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            return new PaymentFormHandler
            {
                Url = "http://money.yandex.ru/eshop.xml",
                Page = page,
                InputValues =
                {
                    {"scid", ScId},
                    {"shopId", ShopId},
                    {"CustName", order.OrderCustomer.FirstName + order.OrderCustomer.LastName},
                    {"CustEMail", order.OrderCustomer.Email},
                    {"CustAddr", order.OrderCustomer.GetCustomerAddress()},
                    {"OrderDetails", string.Join(",", order.OrderItems.Select(item => item.Name + " - " + item.Amount))},
                    {"Sum", (order.Sum*GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", ".")},
                    {"paymentType", YaPaymentType}
                }
            }.ProcessRequest();
        }
    }
}