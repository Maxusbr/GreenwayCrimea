using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdvantShop.CMS;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Handlers.AdminNotifications;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AdvantShop.Web.Admin.Hubs
{
    [HubName("notifyHub")]
    public class NotifyHub : Hub
    {
        private static ConcurrentDictionary<Guid, string> _customersMap = new ConcurrentDictionary<Guid, string>();

        public static string GetConnectionId(Guid customerId)
        {
            string connectionId;
            _customersMap.TryGetValue(customerId, out connectionId);
            return connectionId;
        }

        public static List<string> GetOnlineConnectionIds()
        {
            return _customersMap.Values.ToList();
        }

        public static List<Guid> GetOnlineCustomerIds()
        {
            return _customersMap.Keys.ToList();
        }

        public override Task OnConnected()
        {
            var customerId = CustomerContext.CustomerId;
            if (!_customersMap.TryAdd(customerId, Context.ConnectionId))
                _customersMap.TryUpdate(customerId, Context.ConnectionId, _customersMap[customerId]);

            new Thread(() => SendMissedNotifications(customerId)).Start();

            return base.OnConnected();
        }

        private void SendMissedNotifications(Guid customerId)
        {
            var notifications = AdminNotificationService.GetMissedAdminNotifications(customerId);
            if (notifications.Any())
            {
                new AdminNotificationsHandler().NotifyCustomer(customerId, notifications.ToArray());
                AdminNotificationService.DeleteCustomerAdminNotifications(customerId);
            }
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var kvp = _customersMap.FirstOrDefault(x => x.Value == Context.ConnectionId);
            string connectionId;
            _customersMap.TryRemove(kvp.Key, out connectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}