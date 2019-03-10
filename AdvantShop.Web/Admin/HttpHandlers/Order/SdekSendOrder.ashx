<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.SdekSendOrder" %>

using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Shipping.Sdek;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Order
{
    public class SdekSendOrder : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
            {
                return;
            }

            var orderId = context.Request["orderId"].TryParseInt();


            if (orderId == 0)
            {
                ReturnResult(context, new { status = false, message = "error: no order" });
                return;
            }

            var order = OrderService.GetOrder(orderId);
            if (order == null || order.OrderPickPoint == null)
            {
                //ReturnResult(context, new { status = false, message = "error: no pickpoint" });
                //return;
            }

            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod.ShippingType != "Sdek")
            {
                ReturnResult(context, new { status = false, message = "error: not Sdek" });
                return;
            }

            var tariffId = shippingMethod.Params[SdekTemplate.Tariff].TryParseInt();

            SdekStatusAnswer result = (new Sdek(shippingMethod, null)).SendNewOrders(order, tariffId);

            ReturnResult(context, new { status = result.Status, message = result.Message });
        }

        private static void ReturnResult(HttpContext context, object result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }
    }
}