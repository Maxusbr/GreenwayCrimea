using AdvantShop.Configuration;
using Newtonsoft.Json;

namespace AdvantShop.Web.Admin.Models.Home
{
    public class StatisticsDashboardSetting
    {
        public static StatisticsDashboardModelSetting Settings
        {
            get
            {
                var settings = SettingProvider.Items["StatisticsDashboardSetting"];
                if (string.IsNullOrEmpty(settings))
                {
                    var model = new StatisticsDashboardModelSetting()
                    {
                        ShowTotalProducts = true,
                        ShowTotalOrdersToday = true,
                        ShowTotalOrdersYesterday = true,
                        ShowTotalCallsToday = true,
                        ShowTotalLeadsToday = true,
                        ShowTotalReviewsToday = true
                    };

                    SettingProvider.Items["StatisticsDashboardSetting"] = JsonConvert.SerializeObject(model);

                    return model;
                }
                return JsonConvert.DeserializeObject<StatisticsDashboardModelSetting>(settings);
            }
            set
            {
                SettingProvider.Items["StatisticsDashboardSetting"] = JsonConvert.SerializeObject(value);
            }
        }
    }

    public class StatisticsDashboardModelSetting
    {
        public bool ShowTotalProducts { get; set; }
        public bool ShowTotalOrdersToday { get; set; }
        public bool ShowTotalOrdersYesterday { get; set; }
        public bool ShowTotalOrdersMonth { get; set; }
        public bool ShowTotalOrdersAllTime { get; set; }
        public bool ShowTotalLeadsToday { get; set; }
        public bool ShowTotalLeadsYesterday { get; set; }
        public bool ShowTotalLeadsMonth { get; set; }
        public bool ShowTotalReviewsToday { get; set; }
        public bool ShowTotalReviewsYesterday { get; set; }
        public bool ShowTotalReviewsMonth { get; set; }
        public bool ShowTotalCallsToday { get; set; }
        public bool ShowTotalCallsYesterday { get; set; }
        public bool ShowTotalCallsMonth { get; set; }
    }
}
