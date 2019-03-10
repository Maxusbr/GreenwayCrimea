using System;
using System.Web.UI.WebControls;
using AdvantShop.Core.Services.Statistic;

namespace AdvantShop.Admin.UserControls.Order
{
    public partial class OrdersSearch : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblTotalOrdersCount.Text = StatisticService.GetOrdersCount().ToString();
        }

        protected void sdsStatuses_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
        }
    }
}