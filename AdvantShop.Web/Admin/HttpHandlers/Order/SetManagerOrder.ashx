<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.SetManagerOrder" %>

using System;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Customers;
using AdvantShop.Mails;
using AdvantShop.Orders;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Order
{
    public class SetManagerOrder : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            int managerId = 0;
            var orderId = 0;

            if (!Int32.TryParse(context.Request["orderid"], out orderId) ||
                (context.Request["managerid"].IsNotEmpty() && !int.TryParse(context.Request["managerid"], out managerId)))
            {
                ReturnResult(context, "error");
                return;
            }
            
            OrderService.UpdateOrderManager(orderId, context.Request["managerid"].IsNullOrEmpty() ? null : (int?)managerId);

            if (context.Request["managerid"].IsNullOrEmpty())
            {
                ReturnResult(context, true);
                return;
            }
            
            var manager = ManagerService.GetManager(managerId);
            if (manager == null)
            {
                ReturnResult(context, "error");
                return;
            }

            var setOrderManagerMailTemplate = new SetOrderManagerMailTemplate(SettingsMain.SiteUrl, SettingsMain.ShopName, manager.FirstName + " " + manager.LastName, orderId);
            setOrderManagerMailTemplate.BuildMail();

            SendMail.SendMailNow(manager.CustomerId, manager.Email, setOrderManagerMailTemplate.Subject, setOrderManagerMailTemplate.Body, true);

            ReturnResult(context, true);
        }

        private static void ReturnResult(HttpContext context, object result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(result));
            context.Response.End();
        }

    }
}
