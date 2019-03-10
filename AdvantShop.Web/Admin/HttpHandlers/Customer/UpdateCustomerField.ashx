<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Customers.UpdateCustomerField" %>

using System;
using System.Web;

using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.Security;
using AdvantShop.Customers;

namespace Admin.HttpHandlers.Customers
{
    [AuthorizeRole(RoleAction.Customers)]
    public class UpdateCustomerField : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            Guid customerId;
            var text = context.Request["text"];
            var field = context.Request["field"];

            if (!Guid.TryParse(context.Request["customerid"], out customerId) || string.IsNullOrEmpty(field))
            {
                ReturnResult(context, "error");
                return;
            }

            if (string.Equals(field, "comment"))
            {
                CustomerService.UpdateAdminComment(customerId, text);
            }
            
            //ReturnResult(context, subscribe ? "Подписка отменена " + email : "Подписка оформлена " + email);
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }
    }
}