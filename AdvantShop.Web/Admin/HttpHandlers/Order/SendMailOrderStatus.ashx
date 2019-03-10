<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.SendMailOrderStatus" %>

using System;
using System.Linq;
using System.Web;

using AdvantShop.Core.HttpHandlers;
using AdvantShop.Mails;
using AdvantShop.Orders;
using AdvantShop.Core.Modules;

namespace Admin.HttpHandlers.Order
{
    public class SendMailOrderStatus : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var orderId = 0;

            if (!Int32.TryParse(context.Request["orderid"], out orderId))
            {
                ReturnResult(context, "error");
                return;
            }

            var order = OrderService.GetOrder(orderId);
            if (order != null)
            {

                if (order.OrderStatus.Hidden)
                {
                    ReturnResult(context, "error");
                    return;
                }

                float total = order.OrderItems != null ? order.OrderItems.Sum(item => item.Price * item.Amount) : order.OrderCertificates != null ?  order.OrderCertificates.Sum(item=> item.Sum) : 0;

                var orderItemsHtml = OrderService.GenerateOrderItemsHtml(order.OrderItems, order.OrderCurrency,
                                                           total, //order.Sum,
                                                           order.OrderDiscount, order.OrderDiscountValue,
                                                           order.Coupon, order.Certificate,
                                                           order.TotalDiscount,
                                                           order.ShippingCost, order.PaymentCost,
                                                           order.TaxCost,
                                                           order.BonusCost,
                                                           0);

                var mailTemplate = new OrderStatusMailTemplate(order.OrderStatus.StatusName, order.StatusComment.Replace("\r\n", "<br />"), order.Number, orderItemsHtml, order.TrackNumber ?? string.Empty);
                mailTemplate.BuildMail();

                SendMail.SendMailNow(order.OrderCustomer.CustomerID, order.OrderCustomer.Email, mailTemplate.Subject, mailTemplate.Body, true);

                ModulesExecuter.SendNotificationsOnOrderChangeStatus(order);

                ReturnResult(context, string.Empty);
            }

            ReturnResult(context, "error");
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }
    }
}