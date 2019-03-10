<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Customers.SetCustomerRating" %>

using System;
using System.Web;

using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.Security;
using AdvantShop.Customers;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Customers
{
    [AuthorizeRole(RoleAction.Customers)]
    public class SetCustomerRating : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            Guid customerId;
            var rating = 0;

            if (!Guid.TryParse(context.Request["id"], out customerId) || !int.TryParse(context.Request["rating"], out rating))
            {
                ReturnResult(context, "error");
                return;
            }
            if (CustomerService.ExistsCustomer(customerId) && rating != 0)
            {
                CustomerService.UpdateCustomerRating(customerId, rating);
            }

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(rating));
            context.Response.End();
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }
    }
}