<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Customers.SetCustomerManager" %>

using System;
using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.Security;
using AdvantShop.Customers;

using Newtonsoft.Json;

namespace Admin.HttpHandlers.Customers
{
    [AuthorizeRole(RoleAction.Customers)]
    public class SetCustomerManager : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            Guid customerId;
            var managerId = 0;

            if (!Guid.TryParse(context.Request["customerid"], out customerId) || (context.Request["managerid"].IsNotEmpty() && !int.TryParse(context.Request["managerid"], out managerId)))
            {
                ReturnResult(context, "error");
                return;
            }

            if (!CustomerService.ExistsCustomer(customerId))
            {
                ReturnResult(context, "error");
                return;
            }

            CustomerService.ChangeCustomerManager(customerId, context.Request["managerid"].IsNullOrEmpty() ? null : (int?)managerId);
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }
    }
}