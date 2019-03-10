<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Customers.GetData" %>

using System;
using System.Linq;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Orders;
using Newtonsoft.Json;


namespace Admin.HttpHandlers.Customers
{
    public class GetData : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {

            context.Response.AddHeader("Access-Control-Allow-Origin", SettingsFreshdesk.FreshdeskDomain);
			context.Response.AddHeader("Access-Control-Allow-Credentials", "true");
			context.Response.ContentType = "text/html";

            if (!CustomerContext.CurrentCustomer.IsAdmin && !AdvantShop.Trial.TrialService.IsTrialEnabled)
            {
                context.Response.Write(
                    JsonConvert.SerializeObject(
                        new {ErrorMessage = string.Format("Для получения данных необходимо <a href='{0}/login' target='_blank'>авторизоваться в магазине</a>.", SettingsMain.SiteUrl )}));
				return;
            }
           
            			
			if (context.Request["type"] == "html")
            {
			    context.Response.WriteFile(context.Server.MapPath("~/content/freshdesk/template.html"));
				return;
            }
			
            string email = context.Request["email"];
            if (email.IsNullOrEmpty())
            {
                context.Response.Write(JsonConvert.SerializeObject(null));
                return;
            }

            var orders = OrderService.GetOrders(email);
            var customer = CustomerService.GetCustomerByEmail(email) ?? (Customer)(orders.Any() ? orders.First().OrderCustomer : new OrderCustomer());

            context.Response.Write(JsonConvert.SerializeObject(new
            {
                ID = customer.Id,
                Name = customer.LastName + " " + customer.FirstName,
                Email = customer.EMail,
                Phone = customer.Phone,
                Link = customer.Id != Guid.Empty ? SettingsMain.SiteUrl + "/admin/viewcustomer.aspx?customerid=" + customer.Id : string.Empty,
                OrdersCount = orders.Count,
                OrdersSum = orders.Sum(o => o.Sum/o.OrderCurrency.CurrencyValue).FormatPrice(),
                LastOrders = orders.Take(3).Select(o => new
                {
                    ID = o.OrderID,
                    Date = o.OrderDate.ToString("dd.MM.yyyy HH:mm"),
                    Payed = o.Payed,
                    Status = o.OrderStatus.StatusName,
                    StatusColor = o.OrderStatus.Color,
                    Sum =(o.Sum/o.OrderCurrency.CurrencyValue).FormatPrice(),
                    Link = SettingsMain.SiteUrl + "/admin/vieworder.aspx?orderid=" + o.OrderID
                })
            }));

        }
    }
}