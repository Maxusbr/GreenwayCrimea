using AdvantShop.Web.Admin.Models.Home;

namespace AdvantShop.Web.Admin.Handlers.Home
{
    public class SaveStatisticsDasboard
    {
        private StatisticsDashboardModelSetting _model;

        public SaveStatisticsDasboard(StatisticsDashboardModelSetting model)
        {
            _model = model;
        }

        public void Execute()
        {
            StatisticsDashboardSetting.Settings = _model;
        }
    }
}
