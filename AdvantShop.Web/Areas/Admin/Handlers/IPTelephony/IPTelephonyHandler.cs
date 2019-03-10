using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.CMS;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Saas;

namespace AdvantShop.Web.Admin.Handlers.IPTelephony
{
    public class IPTelephonyHandler
    {
        private List<Customer> _customers;
        private Call _call;

        public IPTelephonyHandler(Call call)
        {
            _call = call;
            _customers = CustomerService.GetCustomersByPhone(call.Phone);
            call.ManagerId = _customers.Select(x => x.ManagerId).Where(x => x.HasValue).FirstOrDefault();
            if (!_customers.Any())
            {
                _customers.Add(new Customer(CustomerGroupService.DefaultCustomerGroup)
                {
                    Phone = call.Phone,
                    StandardPhone = StringHelper.ConvertToStandardPhone(call.Phone)
                });
            }
        }

        public List<AdminNotification> GetWebNotifications()
        {
            return _customers.Select(x => new AdminNotification
            {
                Type = AdminNotificationType.Call,
                Title = string.Format("{0} - {1}", LocalizationService.GetResource("Admin.Calls.IncomingCall"), DateTime.Now.ToString("HH:mm")),
                Body = x.Id != Guid.Empty
                        ? string.Format("{0} {1}\n{2}", x.FirstName, x.LastName, x.StandardPhone)
                        : x.StandardPhone.ToString(),
                Tag = _call.CallId
            }).ToList();
        }

        public List<AdminNotification> GetToasterNotifications()
        {
            return _customers.Select(x => new AdminNotification
            {
                Type = AdminNotificationType.Call,
                Title = string.Format("{0} - {1}", LocalizationService.GetResource("Admin.Calls.IncomingCall"), _call.CallDate.ToString("HH:mm")),
                Data = GetNotificationData(x),
                Tag = _call.CallId
            }).ToList();
        }

        private object GetNotificationData(Customer customer)
        {
            if (customer.Id == Guid.Empty && (!customer.StandardPhone.HasValue || customer.StandardPhone.Value == 0))
                return null;

            var orders = customer.Id != Guid.Empty
                ? OrderService.GetCustomerOrderHistory(customer.Id)
                : OrderService.GetOrdersByPhone(customer.StandardPhone.ToString()).Select(x => new OrderInformation
                {
                    OrderID = x.OrderID,
                    OrderNumber = x.Number,
                    Status = x.OrderStatus.StatusName,
                    Sum = x.Sum,
                    CurrencyValue = x.OrderCurrency.CurrencyValue,
                    Payed = x.Payed,
                    OrderDate = x.OrderDate,
                }).ToList();
            var lastOrder = orders.OrderByDescending(x => x.OrderDate).FirstOrDefault();

            Lead lead = null;
            Lead lastLead = null;
            var haveCrm = SaasDataService.IsEnabledFeature(ESaasProperty.HaveCrm);
            if (haveCrm)
            {
                var leads = customer.Id != Guid.Empty
                    ? LeadService.GetLeadsByCustomer(customer.Id)
                    : LeadService.GetLeadsByPhone(customer.StandardPhone.ToString());
                lastLead = leads.OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (lastLead == null)
                {
                    var orderSource = OrderSourceService.GetOrderSource(OrderType.Phone);
                    lead = new Lead
                    {
                        Phone = customer.StandardPhone.ToString(),
                        CustomerId = customer.Id,
                        Customer = customer,
                        IsFromAdminArea = true,
                        OrderSourceId = orderSource.Id
                    };
                    LeadService.AddLead(lead, true);
                }
            }

            return new
            {
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.StandardPhone,
                Exist = customer.Id != Guid.Empty,
                leadId = lead == null ? null : lead.Id.ToString(),
                lastOrder = lastOrder == null ? null : new
                {
                    orderId = lastOrder.OrderID,
                    orderNumber = lastOrder.OrderNumber,
                    status = lastOrder.Status,
                    date = lastOrder.OrderDate.ToShortDateString()
                },
                lastLead = lastLead == null ? null : new
                {
                    id = lastLead.Id,
                    status = lastLead.DealStatus != null ? lastLead.DealStatus.Name : "",
                    date = lastLead.CreatedDate.ToShortDateString()
                },
                totalOrdersPrice = orders.Sum(x => x.Sum * x.CurrencyValue).FormatPrice(),
                totalOrdersCount = orders.Count,
                payedOrdersPrice = orders.Where(x => x.Payed).Sum(x => x.Sum * x.CurrencyValue).FormatPrice(),
                payedOrdersCount = orders.Count(x => x.Payed),
                Manager = customer.Manager == null ? null : new
                {
                    Name = customer.Manager.FullName,
                    CustomerId = customer.Manager.CustomerId
                },
                haveCrm
            };
        }
    }
}