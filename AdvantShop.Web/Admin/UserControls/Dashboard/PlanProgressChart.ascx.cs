using System;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Orders;

namespace Admin.UserControls.Dashboard
{
    public partial class PlanProgressChart : System.Web.UI.UserControl
    {
        protected double planPercent;
        protected float sales;

        protected void Page_Load(object sender, EventArgs e)
        {
            var plannedSales = OrderStatisticsService.SalesPlan.RoundPrice(1);
            sales = OrderStatisticsService.GetMonthProgress().Key.RoundPrice(1);
            planPercent = Math.Round(sales / (plannedSales / 100));
        }
    }
}