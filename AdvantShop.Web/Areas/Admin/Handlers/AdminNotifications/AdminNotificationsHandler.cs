using System;
using System.Linq;
using AdvantShop.CMS;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Hubs;
using Microsoft.AspNet.SignalR;

namespace AdvantShop.Web.Admin.Handlers.AdminNotifications
{
    public class AdminNotificationsHandler
    {
        private Lazy<IHubContext> hub = new Lazy<IHubContext>(
          () => GlobalHost.ConnectionManager.GetHubContext<NotifyHub>()
        );

        protected IHubContext Hub
        {
            get { return hub.Value; }
        }

        private static readonly Object SyncObject = new Object();

        /// <summary>
        /// notify all customers in all tabs by toaster
        /// </summary>
        /// <param name="notification"></param>
        public void NotifyByToaster(AdminNotification notification)
        {
            lock (SyncObject)
            {
                Hub.Clients.All.popNotification(notification);
            }
        }

        /// <summary>
        /// close toaster notification
        /// </summary>
        /// <param name="toastId"></param>
        public void CloseToaster(string toastId)
        {
            lock (SyncObject)
            {
                Hub.Clients.All.closeToaster(toastId);
            }
        }

        /// <summary>
        /// notify all online customers by webNotification
        /// </summary>
        /// <param name="notification"></param>
        public void NotifyAllCustomers(AdminNotification notification)
        {
            lock (SyncObject)
            {
                var connectionIds = NotifyHub.GetOnlineConnectionIds();
                foreach (var connectionId in connectionIds)
                    Hub.Clients.Client(connectionId).showNotification(notification);
            }
        }

        /// <summary>
        /// notify all online customers by webNotification considering access rights
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="roleAction"></param>
        public void NotifyAllCustomers(AdminNotification notification, RoleAction roleAction, Guid? currentCustomerId = null)
        {
            lock (SyncObject)
            {
                var customerIds = NotifyHub.GetOnlineCustomerIds();
                foreach (var customerId in customerIds.Where(x => !currentCustomerId.HasValue || x != currentCustomerId.Value))
                {
                    Customer customer = null;
                    if (roleAction != RoleAction.None && (customer = CustomerService.GetCustomer(customerId)) != null && !customer.HasRoleAction(roleAction))
                        continue;
                    var connectionId = NotifyHub.GetConnectionId(customerId);
                    if (connectionId.IsNotEmpty())
                        Hub.Clients.Client(connectionId).showNotification(notification);
                }
            }
        }

        /// <summary>
        /// notify one customer by webNotification
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="notifications"></param>
        public void NotifyCustomer(Guid customerId, params AdminNotification[] notifications)
        {
            var connectionId = NotifyHub.GetConnectionId(customerId);
            if (connectionId.IsNotEmpty())
                Hub.Clients.Client(connectionId).showNotifications(notifications);
        }

        /// <summary>
        /// notify customers by webNotification
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="customerIds"></param>
        public void NotifyCustomers(AdminNotification notification, params Guid[] customerIds)
        {
            lock (SyncObject)
            {
                foreach (var customerId in customerIds.Where(x => x != Guid.Empty).Distinct())
                {
                    var connectionId = NotifyHub.GetConnectionId(customerId);
                    if (connectionId.IsNotEmpty())
                        Hub.Clients.Client(connectionId).showNotification(notification);
                    else
                        AdminNotificationService.AddAdminNotification(notification, customerId); 
                }
            }
        }

        public void NotifyCustomers(AdminNotification[] notifications, params Guid[] customerIds)
        {
            if (notifications == null || !notifications.Any())
                return;
            lock (SyncObject)
            {
                for (int i = 0; i < notifications.Length; i++)
                {
                    if (notifications[i].Tag.IsNullOrEmpty())
                        notifications[i].Tag = i.ToString();
                }
                foreach (var customerId in customerIds.Where(x => x != Guid.Empty).Distinct())
                {
                    var connectionId = NotifyHub.GetConnectionId(customerId);
                    if (connectionId.IsNotEmpty())
                        Hub.Clients.Client(connectionId).showNotifications(notifications);
                    else
                    {
                        foreach (var notification in notifications)
                            AdminNotificationService.AddAdminNotification(notification, customerId);
                    }
                }
            }
        }

        public void UpdateOrders()
        {
            lock (SyncObject)
            {
                Hub.Clients.All.updateOrders();
            }
        }

        public void UpdateLeads()
        {
            lock (SyncObject)
            {
                Hub.Clients.All.updateLeads();
            }
        }

        public void UpdateTasks()
        {
            lock (SyncObject)
            {
                Hub.Clients.All.updateTasks();
            }
        }
    }
}
