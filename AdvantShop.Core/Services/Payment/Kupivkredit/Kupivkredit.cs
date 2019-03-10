using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using AdvantShop.Configuration;
using AdvantShop.Taxes;
using AdvantShop.Catalog;
using AdvantShop.Helpers;

namespace AdvantShop.Payment
{
    [PaymentKey("Kupivkredit")]
    public class Kupivkredit : PaymentMethod, ICreditPaymentMethod
    {
        /*
         * Примечание:
         * Заказы на сумму менее 3000 руб. не обарабатываются
         * 
         * Тестовые данные:
         * Id партнера: 1-178YO4Z
         * API key: 123qwe
         * API secret ($salt или "соль" подписи сообщения): 321ewq
         * СМС код подтверждения - 1010
         * 
         */
        #region Fields
        private const int MinOrderPrice = 3000;
        private const int DefFirstPayment = 10;

        public float MinimumPrice { get; set; }
        public float FirstPayment { get; set; }


        public static string ShopId { get; set; }
        public static string ShowCaseId { get; set; }
        public static string PromoCode { get; set; }
        private bool useTest { get; set; }
        public string UseTest
        {
            set { useTest = value == null ? true : value.TryParseBool(); }
        }
        #endregion

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
                               {KupivkreditTemplate.MinimumPrice, MinimumPrice.ToString()},
                               {KupivkreditTemplate.FirstPayment, FirstPayment.ToString()},
                               {KupivkreditTemplate.ShopId, ShopId},
                               {KupivkreditTemplate.UseTest, useTest.ToString()},
                               {KupivkreditTemplate.ShowCaseId, ShowCaseId},
                               {KupivkreditTemplate.PromoCode, PromoCode},
                           };
            }
            set
            {
                MinimumPrice = value.ElementOrDefault(KupivkreditTemplate.MinimumPrice).TryParseFloat();
                if (MinimumPrice < MinOrderPrice)
                {
                    MinimumPrice = MinOrderPrice;
                }
                FirstPayment = value.ElementOrDefault(KupivkreditTemplate.FirstPayment).TryParseFloat();
                UseTest = value.ElementOrDefault(KupivkreditTemplate.UseTest);
                ShopId = value.ElementOrDefault(KupivkreditTemplate.ShopId);
                ShowCaseId = value.ElementOrDefault(KupivkreditTemplate.ShowCaseId);
                PromoCode = value.ElementOrDefault(KupivkreditTemplate.PromoCode);
            }
        }

        public override void ProcessForm(Order order)
        {
            throw new NotImplementedException();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            return new PaymentFormHandler
            {
                FormName = "_xclick",
                Method = FormMethod.POST,
                Url = useTest ? "https://loans-qa.tcsbank.ru/api/partners/v1/lightweight/create" : "https://loans.tinkoff.ru/api/partners/v1/lightweight/create",
                InputValues = GetParam(order)
            }.ProcessRequest();
        }

        private static Dictionary<string, string> GetParam(Order order)
        {
            var result = new Dictionary<string, string>();

            result.Add(key: "shopId", value: ShopId);
            if (ShowCaseId.IsNotEmpty())
                result.Add(key: "showcaseId", value: ShowCaseId);
            if(PromoCode.IsNotEmpty())
                result.Add(key: "promoCode", value: PromoCode);
            result.Add(key: "sum", value: Math.Round(order.Sum, 2).ToString("F2").Replace(",", "."));
            if (order.OrderID > 0)
                result.Add(key: "orderNumber", value: order.OrderID.ToString());

            var orderItems = OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems, order.ShippingCost, order.Sum);
            int i = 0;

            //Что бы доставка в чеке выходила первой
            if (order.ShippingCost > 0)
            {
                var orderitem = new OrderItem()
                {
                    Name = "Доставка",
                    Price = order.ShippingCost,
                    Amount = 1,
                };
                result.AddRange(GetItemsDictionary(orderitem, i));
                i++;
            }

            foreach (var item in orderItems)
            {
                result.AddRange(GetItemsDictionary(item, i));
                i++;
            }

            if (order.OrderCustomer != null)
            {
                result.Add(key: "customerEmail", value: order.OrderCustomer.Email);
                result.Add(key: "customerPhone", value: order.OrderCustomer.Phone);
            }

            return result;
        }

        private static Dictionary<string, string> GetItemsDictionary(OrderItem orderItems, int iteration)
        {
            var result = new Dictionary<string, string>();

            result.Add(key: "itemName_" + iteration, value: orderItems.Name);
            result.Add(key: "itemPrice_" + iteration, value: Math.Round(orderItems.Price, 2).ToString("F2").Replace(",", "."));
            result.Add(key: "itemQuantity_" + iteration, value: Math.Round(orderItems.Amount, 2).ToString().Replace(",", "."));

            if (orderItems.ArtNo.IsNotEmpty())
            {
                result.Add(key: "itemVendorCode_" + iteration, value: orderItems.ArtNo);
            }

            var product = orderItems.ProductID == null ? null : ProductService.GetProduct(orderItems.ProductID.Value);
            if (product != null)
            {
                if (product.MainCategory != null && product.MainCategory.Enabled)
                {
                    result.Add(key: "itemCategory_" + iteration, value: product.MainCategory.Name);
                }
            }

            return result;
        }
        
        public override Dictionary<string, string> GetDefaultParameters()
        {
            var parametrs = new Dictionary<string, string>() {
                {KupivkreditTemplate.MinimumPrice, DefFirstPayment.ToString() },
                {KupivkreditTemplate.FirstPayment, FirstPayment.ToString()},
                {KupivkreditTemplate.ShopId, "test_online" },
                {KupivkreditTemplate.UseTest, "True" },
                {KupivkreditTemplate.ShowCaseId, "test_online" },
                {KupivkreditTemplate.PromoCode, "default" },
            };

            return parametrs;
        }
    }
}