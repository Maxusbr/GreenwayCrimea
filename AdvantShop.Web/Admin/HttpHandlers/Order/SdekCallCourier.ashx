<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.SdekCallCourier" %>

using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Order
{
    public class SdekCallCourier : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var orderId = context.Request["orderId"].TryParseInt();
          
            var order = OrderService.GetOrder(orderId);
            if (order == null)
            {
                ReturnResult(context, new { status = false, message = "error" });
                return;
            }

            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod.ShippingType != "Sdek")
            {
                ReturnResult(context, new { status = false, message = "error" });
                return;
            }

            //SdekStatusAnswer result = (new Sdek(shippingMethod.Params)).CallCourier(order);

            //ReturnResult(context, new { status = result.Status, message = result.Message });
        }

        private static void ReturnResult(HttpContext context, object result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }
    }
}