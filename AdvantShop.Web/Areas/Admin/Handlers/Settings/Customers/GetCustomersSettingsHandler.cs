using AdvantShop.Configuration;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Customers
{
    public class GetCustomersSettingsHandler
    {
        public CustomersSettingsModel Execute()
        {
            return new CustomersSettingsModel
            {
                FreshdeskDomain = SettingsFreshdesk.FreshdeskDomain,
                FreshdeskWidgetCode = SettingsFreshdesk.FreshdeskWidgetCode,

                ApplicationId = SettingsVk.ApplicationId
            };
        }
    }
}
