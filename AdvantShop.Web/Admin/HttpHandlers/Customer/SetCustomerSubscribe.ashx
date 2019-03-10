<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Customers.SetCustomerSubscribe" %>

using System.Web;

using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.Security;
using AdvantShop.Customers;

namespace Admin.HttpHandlers.Customers
{
    [AuthorizeRole(RoleAction.Customers)]
    public class SetCustomerSubscribe : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            bool subscribe;
            var email = context.Request["email"];

            if (!bool.TryParse(context.Request["subscribe"], out subscribe) || string.IsNullOrEmpty(email))
            {
                ReturnResult(context, "error");
                return;
            }

            if (subscribe)
            {
                SubscriptionService.Subscribe(email);
            }
            else
            {
                SubscriptionService.Unsubscribe(email);
            }

            ReturnResult(context, subscribe ? "Подписка отменена " + email : "Подписка оформлена " + email);
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }
    }
}