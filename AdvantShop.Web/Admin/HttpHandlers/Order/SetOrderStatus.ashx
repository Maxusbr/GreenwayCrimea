<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.SetOrderStatus" %>

using System;
using System.Web;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;

using AdvantShop.Orders;
using AdvantShop.Trial;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Order
{
    public class SetOrderStatus : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var statusId = 0;
            var orderId = 0;
            string basis = context.Request["basis"];

            if (!Int32.TryParse(context.Request["statusid"], out statusId) ||
                !Int32.TryParse(context.Request["orderid"], out orderId))
            {
                ReturnResult(context, "error");
            }
            var status = OrderStatusService.GetOrderStatus(statusId);
            if (status != null)
            {
                OrderStatusService.ChangeOrderStatus(orderId, statusId, basis);
                TrialService.TrackEvent(TrialEvents.ChangeOrderStatus, "");

                var order = OrderService.GetOrder(orderId);

                ReturnResult(context, new
                {
                    status.Color,
                    IsNotifyUser = order != null && (order.OrderCustomer.Email.IsNotEmpty() || order.OrderCustomer.Phone.IsNotEmpty()) && !status.Hidden
                });
            }

            ReturnResult(context, "error");
        }

        private static void ReturnResult(HttpContext context, object result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(result));
            context.Response.End();
        }

    }
}
