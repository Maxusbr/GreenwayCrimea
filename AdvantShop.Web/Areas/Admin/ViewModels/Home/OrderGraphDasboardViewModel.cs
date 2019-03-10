using AdvantShop.Core.Services.Localization;
using AdvantShop.Web.Admin.Models.Common;

namespace AdvantShop.Web.Admin.ViewModels.Home
{
    public class OrderGraphDasboardViewModel
    {
        public OrdersChartDataModel ChartWeek { get; set; }
        public OrdersChartDataModel ChartMonth { get; set; }
        public OrdersChartDataModel ChartYear { get; set; }
    }

    public class OrdersChartDataModel : ChartDataModel
    {
        public OrdersChartDataModel()
        {
            Series = string.Format("['{0}', '{1}']",
                LocalizationService.GetResource("Admin.Home.OrderGraphDasboard.Profit"),
                LocalizationService.GetResource("Admin.Home.OrderGraphDasboard.Orders"));
            Colors = "['#71c73e', '#77b7c4']";
        }
    }
}
