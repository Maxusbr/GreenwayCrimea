//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Modules;

namespace AdvantShop.Module.UniSender.Domain
{
    public class UniSenderSettings
    {
        private static string ModuleID
        {
            get { return UniSender.ModuleID; }
        }

        public static string ApiKey
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("UniSenderId", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UniSenderId", value, ModuleID); }
        }

        public static string FromEmail
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("UniSenderFromEmail", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UniSenderFromEmail", value, ModuleID); }
        }

        public static string FromName
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("UniSenderFromName", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UniSenderFromName", value, ModuleID); }
        }

        public static string RegUsersList
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("UniSenderRegUsersList", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UniSenderRegUsersList", value, ModuleID); }
        }

        public static string OrderCustomersList
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("UniSenderOrderCustomersList", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UniSenderOrderCustomersList", value, ModuleID); }
        }

        public static bool SendConfirmation
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("SendConfirmation", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("SendConfirmation", value, ModuleID); }
        }


    }
}