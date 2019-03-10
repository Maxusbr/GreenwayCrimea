<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.UpdateOrderField" %>

using System;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Customers;
using AdvantShop.Orders;

namespace Admin.HttpHandlers.Order
{
    public class UpdateOrderField : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var orderId = 0;
            var text = context.Request["text"];
            var field = context.Request["field"];

            if (!Int32.TryParse(context.Request["orderid"], out orderId) || string.IsNullOrEmpty(field))
            {
                ReturnResult(context, "error");
            }

            var changedBy = new OrderChangedBy(CustomerContext.CurrentCustomer);

            if (string.Equals(field.ToLower(), "adminordercomment"))
            {
                OrderService.UpdateAdminOrderComment(orderId, text, changedBy);
            }

            if (string.Equals(field.ToLower(), "statuscomment"))
            {
                OrderService.UpdateStatusComment(orderId, text, changedBy);
            }

            if (string.Equals(field.ToLower(), "tracknumber"))
            {
                var order = OrderService.GetOrder(orderId);
                order.TrackNumber = text;
                OrderService.UpdateOrderMain(order, false, changedBy);
            }
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }
    }
}