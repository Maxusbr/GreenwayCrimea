using AdvantShop.App.Landing.Domain;
using AdvantShop.App.Landing.Domain.Settings;

namespace AdvantShop.App.Landing.Controllers.Domain.Settings
{
    public static class SeoSettings
    {
        private static LpSettingsService _settingsService = new LpSettingsService();

        public static string PageTitle
        {
            get { return _settingsService.Get(LpService.CurrentLanding.Id, "SeoSettings.PageTitle"); }
            set { _settingsService.AddOrUpdate(LpService.CurrentLanding.Id, "SeoSettings.PageTitle", value); }
        }

        public static string PageKeywords
        {
            get { return _settingsService.Get(LpService.CurrentLanding.Id, "SeoSettings.PageKeywords"); }
            set { _settingsService.AddOrUpdate(LpService.CurrentLanding.Id, "SeoSettings.PageKeywords", value); }
        }

        public static string PageDescription
        {
            get { return _settingsService.Get(LpService.CurrentLanding.Id, "SeoSettings.PageDescription"); }
            set { _settingsService.AddOrUpdate(LpService.CurrentLanding.Id, "SeoSettings.PageDescription", value); }
        }
    }
}
