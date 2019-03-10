using System.Linq;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Customers;
using AdvantShop.Orders;

namespace AdvantShop.Admin.Handlers.Telephony
{
    public class TelephonyHandler
    {
        public object GetOrders(Customer customer)
        {
            var orders = OrderService.GetCustomerOrderHistory(customer.Id);
            var totalOrderPrice = orders.Where(x => x.Payed).Sum(x => x.Sum * x.CurrencyValue).FormatPrice();
            var totalOrderCount = orders.Count(x => x.Payed);
            var lastOrder = orders.OrderByDescending(x => x.OrderDate).FirstOrDefault();
            var leads = LeadService.GetLeadsByCustomer(customer.Id);
            var lastLead = leads != null ? leads.OrderByDescending(x => x.CreatedDate).FirstOrDefault() : null;

            return new
            {
                lastOrder = lastOrder != null
                    ? new
                    {
                        orderId = lastOrder.OrderID,
                        orderNumber = lastOrder.OrderNumber,
                        status = lastOrder.Status,
                        date = lastOrder.OrderDate.ToShortDateString()
                    }
                    : null,
                lastLead = lastLead != null
                    ? new
                    {
                        id = lastLead.Id,
                        status = lastLead.DealStatus != null ? lastLead.DealStatus.Name : "",
                        date = lastLead.CreatedDate.ToShortDateString()
                    }
                    : null,
                totalOrderPrice,
                totalOrderCount
            };
        }

        public object GetOrdersNotRegister(long? standardPhone)
        {
            if (standardPhone == null || standardPhone == 0)
                return null;

            var phone = standardPhone.ToString();

            var orders = OrderService.GetOrdersByPhone(phone);
            var totalOrderPrice = orders.Where(x => x.Payed).Sum(x => x.Sum * x.OrderCurrency.CurrencyValue).FormatPrice();
            var totalOrderCount = orders.Count(x => x.Payed);
            var lastOrder = orders.OrderByDescending(x => x.OrderDate).FirstOrDefault();
            var leads = LeadService.GetLeadsByPhone(phone);
            var lastLead = leads != null ? leads.OrderByDescending(x => x.CreatedDate).FirstOrDefault() : null;

            return new
            {
                lastOrder = lastOrder != null
                    ? new
                    {
                        orderId = lastOrder.OrderID,
                        orderNumber = lastOrder.Number,
                        status = lastOrder.OrderStatus.StatusName,
                        date = lastOrder.OrderDate.ToShortDateString()
                    }
                    : null,
                lastLead = lastLead != null
                    ? new
                    {
                        id = lastLead.Id,
                        status = lastLead.DealStatus != null ? lastLead.DealStatus.Name : "",
                        date = lastLead.CreatedDate.ToShortDateString()
                    }
                    : null,
                totalOrderPrice,
                totalOrderCount
            };
        }

        public object GetManager(Customer customer)
        {
            if (!customer.ManagerId.HasValue)
                return null;

            var manager = ManagerService.GetManager(customer.ManagerId.Value);
            if (manager == null)
                return null;

            return new
            {
                Name = manager.LastName + " " + manager.FirstName,
                CustomerId = manager.CustomerId
            };
        }
    }
}