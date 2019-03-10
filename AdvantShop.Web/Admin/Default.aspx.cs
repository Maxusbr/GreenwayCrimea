//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;

namespace Admin
{
    public partial class DefaultPage : AdvantShopAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(SettingsMain.ShopName);

            var _customer = CustomerContext.CurrentCustomer;

            if (_customer.CustomerRole == Role.Moderator)
            {
                var actions = RoleActionService.GetCustomerRoleActionsByCustomerId(_customer.Id);
                bool showMainPageStatistics = actions.Any(item => item.Role == RoleAction.Orders);

                IndicatorsStatistic.Visible = showMainPageStatistics;
                SearchStatistic.Visible = showMainPageStatistics;
                PlanProgressChart.Visible = showMainPageStatistics;
                GoogleAnaliticStatistic.Visible = showMainPageStatistics;
                ordersStatuses.Visible = showMainPageStatistics;

                bool showOrderDashBoard = actions.Any(item => item.Role == RoleAction.Orders);

                BigOrdersChart.Visible = showOrderDashBoard;
                LastOrders.Visible = showOrderDashBoard;
            }
        }
    }
}