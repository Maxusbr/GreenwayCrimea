using AdvantShop.Configuration;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Customers
{
    public class SaveCustomersSettingsHandler
    {
        private CustomersSettingsModel _model;

        public SaveCustomersSettingsHandler(CustomersSettingsModel model)
        {
            _model = model;
        }

        public void Execute()
        {
            SettingsFreshdesk.FreshdeskDomain = _model.FreshdeskWidgetCode.Replace("http://", "").Replace("https://", "");
        }
    }
}
