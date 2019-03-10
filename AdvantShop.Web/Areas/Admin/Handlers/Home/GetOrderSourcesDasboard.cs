using System;
using System.Linq;
using AdvantShop.Core.SQL;
using AdvantShop.Web.Admin.ViewModels.Home;

namespace AdvantShop.Web.Admin.Handlers.Home
{
    public class GetOrderSourcesDasboard
    {
        public GetOrderSourcesDasboard()
        {
        }

        public OrderSourcesDasboardViewModel Execute()
        {
            var sources = SQLDataAccess.Query<OrderSourceItemDasboardViewModel>(
                "SELECT Id,Name,Type, (Select Count(OrderID) From [Order].[Order] Where [Order].[OrderSourceId] = [OrderSource].[Id]) as OrdersCount FROM [Order].[OrderSource] order by SortOrder")
                .ToList();

            var totalCount = sources.Sum(x => x.OrdersCount);

            sources = sources.OrderByDescending(x => x.OrdersCount).Take(10).ToList();

            foreach (var source in sources)
            {
                source.Percent = totalCount==0 ? 0 :(int)Math.Round((decimal)source.OrdersCount * 100 / totalCount);
            }

            return new OrderSourcesDasboardViewModel() { OrderSources = sources };
        }
    }
}
