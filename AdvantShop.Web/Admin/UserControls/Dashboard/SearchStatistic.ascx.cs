using System;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Statistic;

namespace Admin.UserControls.Dashboard
{
    public partial class SearchStatistic : System.Web.UI.UserControl
    {

        protected void Page_PreRender(object sender, EventArgs e)
        {
            lvSearchStatistic.DataSource = StatisticService.GetHistorySearchStatistic(5);
            lvSearchStatistic.DataBind();
        }
    }
}