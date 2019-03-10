//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Modules;
using AdvantShop.Module.MailChimp;

namespace AdvantShop.Modules
{
    public class MailChimpSettings
    {
        private static string ModuleID
        {
            get { return MailChimp.ModuleID; }
        }

        public static string ApiKey
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MailChimpId", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("MailChimpId", value, ModuleID); }
        }

        public static string FromEmail
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MailChimpFromEmail", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("MailChimpFromEmail", value, ModuleID); }
        }

        public static string FromName
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MailChimpFromName", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("MailChimpFromName", value, ModuleID); }
        }

        public static string RegUsersList
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MailChimpRegUsersList", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("MailChimpRegUsersList", value,  ModuleID); }
        }

        public static string OrderCustomersList
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MailChimpOrderCustomer", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("MailChimpOrderCustomer", value, ModuleID); }
        }
    }
}