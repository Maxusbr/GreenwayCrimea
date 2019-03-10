using AdvantShop.Core.Modules;

namespace AdvantShop.Module.VkMarket.Domain
{
    public enum ShowDescriptionMode
    {
        No = 0,
        Full = 1,
        Short = 2
    }
    public enum AddLinkToSiteMode
    {
        No = 0,
        Top = 1,
        Bottom = 2
    }


    public class VkMarketExportSettings
    {
        //public static bool ExportOffers
        //{
        //    get { return ModuleSettingsProvider.GetSettingValue<bool>("ExportOffers", VkMarket.ModuleId); }
        //    set { ModuleSettingsProvider.SetSettingValue("ExportOffers", value.ToString(), VkMarket.ModuleId); }
        //}

        public static bool ExportUnavailableProducts
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ExportUnavailableProducts", VkMarket.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("ExportUnavailableProducts", value.ToString(), VkMarket.ModuleId); }
        }

        public static bool AddSizeAndColorInName
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("AddSizeAndColorInName", VkMarket.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("AddSizeAndColorInName", value.ToString(), VkMarket.ModuleId); }
        }

        public static bool AddSizeAndColorInDescription
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("AddSizeAndColorInDescription", VkMarket.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("AddSizeAndColorInDescription", value.ToString(), VkMarket.ModuleId); }
        }

        public static ShowDescriptionMode ShowDescription
        {
            get { return (ShowDescriptionMode)ModuleSettingsProvider.GetSettingValue<int>("ShowDescription", VkMarket.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("ShowDescription",((int) value).ToString(), VkMarket.ModuleId); }
        }

        public static AddLinkToSiteMode AddLinkToSite
        {
            get { return (AddLinkToSiteMode)ModuleSettingsProvider.GetSettingValue<int>("AddLinkToSite", VkMarket.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("AddLinkToSite", ((int)value).ToString(), VkMarket.ModuleId); }
        }

        public static string TextBeforeLinkToSite
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("TextBeforeLinkToSite", VkMarket.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("TextBeforeLinkToSite", value, VkMarket.ModuleId); }
        }

        public static bool ExportOnShedule
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ExportOnShedule", VkMarket.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("ExportOnShedule", value.ToString(), VkMarket.ModuleId); }
        }
        
        public static bool ShowProperties
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ShowProperties", VkMarket.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("ShowProperties", value.ToString(), VkMarket.ModuleId); }
        }
    }
}
