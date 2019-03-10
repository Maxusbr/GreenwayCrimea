<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Customers.SetCustomerGroup" %>

using System;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.Security;
using AdvantShop.Customers;


namespace Admin.HttpHandlers.Customers
{
    [AuthorizeRole(RoleAction.Customers)]
    public class SetCustomerGroup : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;
            
            var customerId = new Guid();
            var customerGroupId = 0;

            if (!Int32.TryParse(context.Request["customerGroupId"], out customerGroupId) || !Guid.TryParse(context.Request["customerId"], out customerId))
            {
                ReturnResult(context, "error");
                return;
            }

            if (!CustomerService.ExistsCustomer(customerId))
            {
                ReturnResult(context, "error");
                return;
            }
            
            CustomerService.ChangeCustomerGroup(customerId, customerGroupId);
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }
    }
}