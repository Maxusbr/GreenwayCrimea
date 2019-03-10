using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Admin.Handlers.Telephony;
using AdvantShop.Admin.Hubs;
using AdvantShop.Core.Caching;
using AdvantShop.Customers;
using Microsoft.AspNet.SignalR;

namespace AdvantShop.Admin.Controllers
{
    public class CallController : Controller
    {
        protected static void Notify(List<Customer> customers, string callId)
        {
            var cacheName = string.Format("Telephony_NotifySent_{0}", callId);
            if (CacheManager.Get<bool>(cacheName))
                return;

            var handler = new TelephonyHandler();
            var context = GlobalHost.ConnectionManager.GetHubContext<CallHub>();

            context.Clients.All.Notification(
                customers.Select(x => new
                {
                    callId,
                    x.Id,
                    x.FirstName,
                    x.LastName,
                    x.StandardPhone,
                    Exist = x.Id != Guid.Empty,
                    Orders = x.Id != Guid.Empty ? handler.GetOrders(x) : handler.GetOrdersNotRegister(x.StandardPhone),
                    Manager = x.Id != Guid.Empty ? handler.GetManager(x) : null
                }).ToList());
            CacheManager.Insert(cacheName, true, 0.1);  // 6 seconds
        }
    }
}