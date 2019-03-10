using AdvantShop.Core.Modules;

namespace AdvantShop.Module.BannerMania.Service
{
    public class BMSettings
    {
        public static readonly int Version = 0; //версия для скриптов

        public static string ModuleID = BannerMania.ModuleStringId;

        public static string BannerInTopURL
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("BannerInTopURL", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("BannerInTopURL", value, ModuleID); }
        }

        public static bool BannerInTopOnlyOnMainPage
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("BannerInTopOnlyOnMainPage", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("BannerInTopOnlyOnMainPage", value, ModuleID); }
        }

        public static bool BannerInTopTargetBlank
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("BannerInTopTargetBlank", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("BannerInTopTargetBlank", value, ModuleID); }
        }

        public static string BannerInTopImagePath
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("BannerInTopImagePath", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("BannerInTopImagePath", value, ModuleID); }
        }

        public static int BannerInTopWidth
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("BannerInTopWidth", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("BannerInTopWidth", value, ModuleID); }
        }

        public static int BannerInTopHeight
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("BannerInTopHeight", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("BannerInTopHeight", value, ModuleID); }
        }

        public static int UnderDeliveryInfoWidth
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("UnderDeliveryInfoWidth", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UnderDeliveryInfoWidth", value, ModuleID); }
        }

        public static int UnderDeliveryInfoHeight
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("UnderDeliveryInfoHeight", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UnderDeliveryInfoHeight", value, ModuleID); }
        }

        public static int AboveDeliveryInfoWidth
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("AboveDeliveryInfoWidth", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AboveDeliveryInfoWidth", value, ModuleID); }
        }

        public static int AboveDeliveryInfoHeight
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("AboveDeliveryInfoHeight", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AboveDeliveryInfoHeight", value, ModuleID); }
        }

        public static int UnderFilterWidth
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("UnderFilterWidth", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UnderFilterWidth", value, ModuleID); }
        }

        public static int UnderFilterHeight
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("UnderFilterHeight", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UnderFilterHeight", value, ModuleID); }
        }

        public static int AboveFilterWidth
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("AboveFilterWidth", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AboveFilterWidth", value, ModuleID); }
        }

        public static int AboveFilterHeight
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("AboveFilterHeight", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AboveFilterHeight", value, ModuleID); }
        }

        public static int UnderMenuWidth
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("UnderMenuWidth", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UnderMenuWidth", value, ModuleID); }
        }

        public static int UnderMenuHeight
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("UnderMenuHeight", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UnderMenuHeight", value, ModuleID); }
        }

        public static int AboveFooterWidth
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("AboveFooterWidth", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AboveFooterWidth", value, ModuleID); }
        }

        public static int AboveFooterHeight
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("AboveFooterHeight", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AboveFooterHeight", value, ModuleID); }
        }

        public static bool SetDefaultSettings()
        {
            BannerInTopURL = string.Empty;
            BannerInTopOnlyOnMainPage = false;
            BannerInTopTargetBlank = false;
            BannerInTopImagePath = string.Empty;
            BannerInTopWidth = 1200;
            BannerInTopHeight = 170;
            UnderDeliveryInfoWidth = 245;
            UnderDeliveryInfoHeight = 300;
            AboveDeliveryInfoWidth = 245;
            AboveDeliveryInfoHeight = 300;
            UnderFilterWidth = 245;
            UnderFilterHeight = 300;
            AboveFilterWidth = 245;
            AboveFilterHeight = 300;
            UnderMenuWidth = 1160;
            UnderMenuHeight = 100;
            AboveFooterWidth = 1160;
            AboveFooterHeight = 100;

            return true;
        }
    }
}
