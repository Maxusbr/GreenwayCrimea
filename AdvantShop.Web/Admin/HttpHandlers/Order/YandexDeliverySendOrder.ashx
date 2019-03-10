<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.YandexDeliverySendOrder" %>

using System;
using System.Linq;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Shipping.ShippingYandexDelivery;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Order
{
    public class YandexDeliverySendOrder : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
            {
                return;
            }

            var orderId = 0;
            if (!int.TryParse(context.Request["orderId"], out orderId))
            {
                ReturnResult(context, new { status = false, message = "OrderID is empty" });
                return;
            }

            var order = OrderService.GetOrder(orderId);
            if (order == null)
            {
                ReturnResult(context, new { status = false, message = "Order is null" });
                return;
            }

            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod.ShippingType != "YandexDelivery")
            {
                ReturnResult(context, new { status = false, message = "Order delivery is not 'YandexDelivery'" });
                return;
            }

            try
            {
                var preOrder = new PreOrder()
                {
                    Items = order.OrderItems.Select(x => new PreOrderItem(x)).ToList(),
                    CountryDest = order.OrderCustomer.Country,
                    CityDest = order.OrderCustomer.City,
                    ShippingOption = new BaseShippingOption(shippingMethod),
                    Currency = order.OrderCurrency
                };

                var yandexDelivery = new YandexDelivery(shippingMethod, preOrder);
                var result = yandexDelivery.CreateOrder(order);

                ReturnResult(context, new { error = !result, message = result ? "Черновик заказа успешно создан" : "Не удалось создать черновик заказа" });
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                ReturnResult(context, "error:" + ex.Message);
            }
            
        }

        private static void ReturnResult(HttpContext context, object result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(new { result }));
        }
    }
}