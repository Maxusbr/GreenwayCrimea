<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Customers.SendLetterToCustomer" %>

using System;
using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.Security;
using AdvantShop.Customers;
using AdvantShop.Mails;
using AdvantShop.Orders;
using AdvantShop.Core.Services.Localization;


namespace Admin.HttpHandlers.Customers
{
    [AuthorizeRole(RoleAction.Customers)]
    public class SendLetterToCustomer : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var subject = context.Request["subject"].ToString();
            var text = context.Request["text"].ToString();
            string trackNumber = null;

            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(subject))
            {
                ReturnResult(context, "error");
                return;
            }

            string firstName = null, lastName = null, patronomic = null, email = "";
            var customerId = Guid.Empty;

            if (context.Request["orderId"] != null)
            {
                var order = OrderService.GetOrder(context.Request["orderId"].TryParseInt());
                if (order != null && order.OrderCustomer != null)
                {
                    if(order.OrderCustomer != null)
                    {
                        email = order.OrderCustomer.Email;
                        firstName = order.OrderCustomer.FirstName;
                        lastName = order.OrderCustomer.LastName;
                        patronomic = order.OrderCustomer.Patronymic;
                        customerId = order.OrderCustomer.CustomerID;
                    }
                    trackNumber = order.TrackNumber;
                }
            }
            else if (context.Request["customerId"] != null)
            {
                var customer = CustomerService.GetCustomer(context.Request["customerId"].TryParseGuid());
                if (customer != null)
                {
                    email = customer.EMail;
                    firstName = customer.FirstName;
                    lastName = customer.LastName;
                    patronomic = customer.Patronymic;
                    customerId = customer.Id;
                }
            }

            if (customerId == Guid.Empty)
            {
                ReturnResult(context, "error");
                return;
            }

            var mailTemplate = new SendToCustomerTemplate(firstName ?? "", lastName ?? "", patronomic ?? "", text, 
                string.IsNullOrEmpty(trackNumber) ? LocalizationService.GetResource("Core.Customers.SendMail.NotTrackNumber") : trackNumber);
            mailTemplate.BuildMail();

            SendMail.SendMailNow(customerId, email, subject, mailTemplate.Body, true);
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }
    }
}