<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.SendBillingLink" %>

using System;
using System.Linq;
using System.Text;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Mails;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using Resources;

namespace Admin.HttpHandlers.Order
{
    public class SendBillingLink : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var order = OrderService.GetOrder(context.Request["orderid"].TryParseInt());
            if (order == null)
            {
                ReturnResult(context, "error", Resource.Admin_ViewOrder_SendBillingLinkError);
                return;   
            }
            
            if (SettingsCheckout.ManagerConfirmed && !order.ManagerConfirmed)
            {
                ReturnResult(context, "error", Resource.Admin_ViewOrder_SendBillingLink_OrderNotConfirmed);
                return;   
            }
            
            var currency = (Currency) order.OrderCurrency;
            
            var orderTable = OrderService.GenerateOrderItemsHtml(order.OrderItems, currency, order.OrderItems.Sum(x => x.Price * x.Amount), 
                                                                 order.OrderDiscount, order.OrderDiscountValue, order.Coupon,
                                                                 order.Certificate, order.TotalDiscount, order.ShippingCost,
                                                                 order.PaymentCost, order.TaxCost, order.BonusCost, 0);

            var mailTemplate = new BillingLinkMailTemplate(order.OrderID.ToString(), order.Number, order.OrderCustomer.FirstName, 
                                                            BuildCustomerContacts(order.OrderCustomer),
                                                            OrderService.GetBillingLinkHash(order), "", orderTable);

            mailTemplate.BuildMail();
            SendMail.SendMailNow(order.OrderCustomer.CustomerID, order.OrderCustomer.Email, mailTemplate.Subject, mailTemplate.Body, true);
            SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForOrders, mailTemplate.Subject, mailTemplate.Body, true);

            ReturnResult(context, "success", Resource.Admin_ViewOrder_SendBillingLinkSuccess);
        }

        private static void ReturnResult(HttpContext context, string result, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result, message }));
            context.Response.End();
        }

        private static string BuildCustomerContacts(OrderCustomer customer)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(customer.FirstName))
                sb.AppendFormat(Resource.Client_Registration_Name + " {0}<br/>", customer.FirstName);

            if (!string.IsNullOrEmpty(customer.LastName))
                sb.AppendFormat(Resource.Client_Registration_Surname + " {0}<br/>", customer.LastName);

            if (!string.IsNullOrEmpty(customer.Country))
                sb.AppendFormat(Resource.Client_Registration_Country + " {0}<br/>", customer.Country);

            if (!string.IsNullOrEmpty(customer.Region))
                sb.AppendFormat(Resource.Client_Registration_State + " {0}<br/>", customer.Region);

            if (!string.IsNullOrEmpty(customer.City))
                sb.AppendFormat(Resource.Client_Registration_City + " {0}<br/>", customer.City);

            if (!string.IsNullOrEmpty(customer.Zip))
                sb.AppendFormat(Resource.Client_Registration_Zip + " {0}<br/>", customer.Zip);

            if (!string.IsNullOrEmpty(customer.GetCustomerAddress()))
                sb.AppendFormat(Resource.Client_Registration_Address + ": {0}<br/>", string.IsNullOrEmpty(customer.GetCustomerAddress())
                                                                                              ? Resource.Client_OrderConfirmation_NotDefined
                                                                                              : customer.GetCustomerAddress());
            return sb.ToString();
        }
    }
}