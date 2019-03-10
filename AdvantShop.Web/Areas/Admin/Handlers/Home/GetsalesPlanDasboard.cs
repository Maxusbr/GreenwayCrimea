using System;
using System.Collections.Generic;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.ViewModels.Home;

namespace AdvantShop.Web.Admin.Handlers.Home
{
    public class GetSalesPlanDasboard
    {
        public GetSalesPlanDasboard()
        {
        }

        public SalesPlanDashboardViewModel Execute()
        {
            var plannedSales = OrderStatisticsService.SalesPlan.RoundPrice();
            var sales = OrderStatisticsService.GetMonthProgress().Key.RoundPrice();
            var planPercent = Math.Round(sales / (plannedSales / 100));

            return new SalesPlanDashboardViewModel()
            {
                PlanPercent = planPercent,
                Sales = PriceFormatService.FormatPrice(sales),
                PlannedSales = PriceFormatService.FormatPrice(plannedSales),
            };
        }
    }
}
