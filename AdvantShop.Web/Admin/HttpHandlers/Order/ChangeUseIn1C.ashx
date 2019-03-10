<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.ChangeUseIn1C" %>

using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Orders;

namespace Admin.HttpHandlers.Order
{
    public class ChangeUseIn1C : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            if (string.IsNullOrEmpty(context.Request["orderid"]) || string.IsNullOrEmpty(context.Request["use"]))
            {
                ReturnResult(context, "error");
                return;
            }

            var orderId = context.Request["orderid"].TryParseInt();
            var useIn1C = context.Request["use"].TryParseBool();
            
            var order = OrderService.GetOrder(orderId);
            if (order != null)
            {
                order.UseIn1C = useIn1C;
                OrderService.UpdateOrderMain(order, false);
            }
            //OrderService.ChangeUseIn1C(orderId, useIn1C);

            ReturnResult(context, "true");
        }

        private static void ReturnResult(HttpContext context, object result)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(result);
            context.Response.End();
        }
    }
}