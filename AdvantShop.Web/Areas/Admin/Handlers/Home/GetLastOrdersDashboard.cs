using System.Collections.Generic;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Localization;
using AdvantShop.Orders;

namespace AdvantShop.Web.Admin.Handlers.Home
{
    public class LastOrdersItemModel
    {
        public int OrderId { get; set; }
        public string StatusColor { get; set; }
        public string StatusName { get; set; }
        public string Number { get; set; }
        public string CustomerName { get; set; }
        public string OrderDate { get; set; }
        public string Sum { get; set; }
    }

    public class GetLastOrdersDashboard
    {
        private readonly string _status;

        public GetLastOrdersDashboard(string status)
        {
            _status = status;
        }

        public List<LastOrdersItemModel> Execute()
        {
            var orders = new List<LastOrdersItem>();

            if (_status == "all")
            {
                orders = OrderService.GetLastOrders(7);
            }
            else
            {
                Manager currentManager;
                if (CustomerContext.CurrentCustomer.IsManager &&
                    (currentManager = ManagerService.GetManager(CustomerContext.CurrentCustomer.Id)) != null)
                {
                    if (_status == "my")
                        orders = OrderService.GetLastOrders(7, currentManager.ManagerId);

                    if (_status == "notmy")
                        orders = OrderService.GetLastOrders(7, null);
                }
            }

            var model = new List<LastOrdersItemModel>();

            foreach (var order in orders)
            {
                model.Add(new LastOrdersItemModel()
                {
                    OrderId = order.OrderId,
                    StatusColor = order.Color,
                    StatusName = order.StatusName,
                    Number = order.Number,
                    CustomerName = order.FirstName + " " + order.LastName,
                    OrderDate = Culture.ConvertDateWithoutSeconds(order.OrderDate),
                    Sum = order.CurrencyValue != 0 
                            ? PriceFormatService.FormatPrice(order.Sum < 0 ? 0 : order.Sum, order.CurrencyValue, order.CurrencySymbol, order.CurrencyCode, order.IsCodeBefore)
                            : order.Sum.FormatPrice()
                });
            }

            return model;
        }
    }
}
