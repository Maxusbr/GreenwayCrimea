using System.Linq;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Core.SQL;
using AdvantShop.Web.Admin.ViewModels.Home;

namespace AdvantShop.Web.Admin.Handlers.Home
{
    public class GetOrdersDasboard
    {
        public GetOrdersDasboard()
        {
        }

        public OrdersDasboardViewModel Execute()
        {
            var statuses = SQLDataAccess.Query<OrderItemDasboardViewModel>(
                "SELECT OrderStatusID, StatusName, Color, " +
                "(Select Count(OrderID) From [Order].[Order] Where IsDraft = 0 AND [OrderStatusID] = OrderStatus.[OrderStatusID]) as OrdersCount " +
                "FROM [Order].OrderStatus " +
                "ORDER BY SortOrder")
                .ToList();
            
            statuses.Insert(0, new OrderItemDasboardViewModel()
            {
                Color = "000",
                StatusName = "Все заказы",
                OrdersCount = StatisticService.GetOrdersCount()
            });

            return new OrdersDasboardViewModel() {OrderStatuses = statuses};
        }
    }
}
