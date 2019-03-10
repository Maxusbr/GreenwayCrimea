using System.Linq;
using System.Web.Mvc;
using AdvantShop.Areas.Api.Models;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Security;
using System;

namespace AdvantShop.Areas.Api.Controllers
{
    public class FreshDeskController : BaseApiController
    {
        // GET: api/freshdesk/
        public ActionResult Index(string apikey, string email)
        {
            if (!CustomerContext.CurrentCustomer.IsAdmin && !Trial.TrialService.IsTrialEnabled)
            {
                return RedirectToAction("Login");
            }

            if (email.IsNullOrEmpty())
            {
                return Content("no email");
            }

            var orders = OrderService.GetOrders(email);
            var customer = CustomerService.GetCustomerByEmail(email) ?? (Customer)(orders.Any() ? orders.First().OrderCustomer : new OrderCustomer());

            var model = new FreshDeskModel()
            {
                ID = customer.Id,
                Name = customer.LastName + " " + customer.FirstName,
                Email = customer.EMail,
                Phone = customer.Phone,
                Link = customer.Id != Guid.Empty ? SettingsMain.SiteUrl + "/admin/viewcustomer.aspx?customerid=" + customer.Id : string.Empty,
                OrdersCount = orders.Count,
                OrdersSum = orders.Sum(o => o.Sum / o.OrderCurrency.CurrencyValue).FormatPrice(),
                LastOrders = orders.Take(3).Select(o => new FreshDeskOrder
                {
                    ID = o.OrderID,
                    Date = o.OrderDate.ToString("dd.MM.yyyy HH:mm"),
                    Payed = o.Payed,
                    Status = o.OrderStatus.StatusName,
                    StatusColor = o.OrderStatus.Color,
                    Sum = (o.Sum / o.OrderCurrency.CurrencyValue).FormatPrice(),
                    Link = SettingsMain.SiteUrl + "/admin/vieworder.aspx?orderid=" + o.OrderID
                }).ToList()
            };

            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            AuthorizeService.SignIn(email, password, false, true);
            return RedirectToAction("Index");
        }

    }
}