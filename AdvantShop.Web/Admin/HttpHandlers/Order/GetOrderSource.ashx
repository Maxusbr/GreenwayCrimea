<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.GetOrderSource" %>

using System;
using System.Linq;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Orders;
using AdvantShop.SEO;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Order
{
    public class GetOrderSource : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context) || !SettingsSEO.GoogleAnalyticsApiEnabled)
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

            var data = GoogleAnalyticsService.GetOrderSource(order);
            if (data == null)
            {
                ReturnResult(context, new { status = false, message = "" });
            }

            ReturnResult(context, new { status = true, message = data });
        }

        private static void ReturnResult(HttpContext context, object result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(new { result }));
        }
    }
}