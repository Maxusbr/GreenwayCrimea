<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.CheckoutSendOrder" %>

using System.Web;

using AdvantShop.Core.HttpHandlers;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Shipping.CheckoutRu;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Order
{
    public class CheckoutSendOrder : AdminHandler, IHttpHandler
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
                ReturnResult(context, new { status = false, message = "error" });
                return;
            }

            var order = OrderService.GetOrder(orderId);
            if (order == null)
            {
                ReturnResult(context, new { status = false, message = "error" });
                return;
            }

            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod.ShippingType != "CheckoutRu")
            {
                ReturnResult(context, new { status = false, message = "error" });
                return;
            }

            var response = (new CheckoutRu(shippingMethod, null)).CreateOrder(order);

            ReturnResult(context, new { error = response.error, message = response.error ? "Ошибка при добавлении заказа, ответ сервера : " + response.errorMessage : "Заказ добавлен в систему под номером " + response.order.id });
        }

        private static void ReturnResult(HttpContext context, object result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }
    }
}