<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.SetManagerConfirm" %>

using System;
using System.Web;
using AdvantShop.Core.HttpHandlers;

using AdvantShop.Orders;

namespace Admin.HttpHandlers.Order
{
    public class SetManagerConfirm : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var confirm = false;
            var orderId = 0;

            if (!bool.TryParse(context.Request["confirm"], out confirm) || !Int32.TryParse(context.Request["orderid"], out orderId))
            {
                ReturnResult(context, "error");
            }

            OrderService.ManagerConfirmOrder(orderId, confirm);
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