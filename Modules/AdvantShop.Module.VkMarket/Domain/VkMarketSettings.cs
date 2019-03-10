using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Vk;
using Newtonsoft.Json;

namespace AdvantShop.Module.VkMarket.Domain
{
    public class VkMarketSettings
    {
        public static string ApplicationId
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ApplicationId", VkMarket.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("ApplicationId", value, VkMarket.ModuleId); }
        }

        public static long UserId
        {
            get { return ModuleSettingsProvider.GetSettingValue<long>("UserId", VkMarket.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("UserId", value, VkMarket.ModuleId); }
        }

        public static string AuthToken
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("AuthToken", VkMarket.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("AuthToken", value, VkMarket.ModuleId); }
        }

        public static int TokenErrorsCount
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("TokenErrorsCount", VkMarket.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("TokenErrorsCount", value, VkMarket.ModuleId); }
        }


        public static VkGroup Group
        {
            get
            {
                var str = ModuleSettingsProvider.GetSettingValue<string>("Group", VkMarket.ModuleId);
                return !string.IsNullOrWhiteSpace(str) ? JsonConvert.DeserializeObject<VkGroup>(str) : null;
            }
            set
            {
                ModuleSettingsProvider.SetSettingValue("Group", value != null ? JsonConvert.SerializeObject(value) : "", VkMarket.ModuleId);
            }
        }
        

        public static string CurrencyIso3
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CurrencyIso3", VkMarket.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("CurrencyIso3", value, VkMarket.ModuleId); }
        }
    }
}
