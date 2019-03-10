<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.SetOrderPaid" %>

using System;
using System.Web;
using AdvantShop.Core.HttpHandlers;

using AdvantShop.Orders;

namespace Admin.HttpHandlers.Order
{
    public class SetOrderPaid : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var paid = 0;
            var orderId = 0;

            if (!Int32.TryParse(context.Request["paid"], out paid) || !Int32.TryParse(context.Request["orderid"], out orderId))
            {
                ReturnResult(context, "error");
            }

            OrderService.PayOrder(orderId, paid == 1);
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }

    }
}