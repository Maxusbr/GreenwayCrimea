using AdvantShop.Core.Modules;

namespace AdvantShop.Module.RetailRocket.Domain
{
    public class RRSettings
    {
        private const string ModuleStringId = "RetailRocket";

        public static string PartnerId
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("PartnerId", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("PartnerId", value, ModuleStringId); }
        }

        public static int Limit
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("Limit", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("Limit", value, ModuleStringId); }
        }

        public static bool UseApi
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("UseApi", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("UseApi", value, ModuleStringId); }
        }

        public static string RelatedProductRecoms
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("RelatedProductRecoms", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("RelatedProductRecoms", value, ModuleStringId); }
        }

        public static string AlternativeProductRecoms
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("AlternativeProductRecoms", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("AlternativeProductRecoms", value, ModuleStringId); }
        }

        public static string ShoppingCartRecoms
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ShoppingCartRecoms", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ShoppingCartRecoms", value, ModuleStringId); }
        }

        public static bool SendMail
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("SendMail", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("SendMail", value, ModuleStringId); }
        }



        public static EMainPageProductsType MainPageBeforeType
        {
            get { return (EMainPageProductsType)ModuleSettingsProvider.GetSettingValue<int>("MainPageBeforeType", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("MainPageBeforeType", (int)value, ModuleStringId); }
        }

        public static string MainPageBeforeTitle
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MainPageBeforeTitle", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("MainPageBeforeTitle", value, ModuleStringId); }
        }

        public static EMainPageProductsType MainPageAfterType
        {
            get { return (EMainPageProductsType)ModuleSettingsProvider.GetSettingValue<int>("MainPageAfterType", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("MainPageAfterType", (int)value, ModuleStringId); }
        }

        public static string MainPageAfterTitle
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MainPageProductsTitle", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("MainPageProductsTitle", value, ModuleStringId); }
        }

        public static ECategoryProductsType CategoryTopType
        {
            get { return (ECategoryProductsType)ModuleSettingsProvider.GetSettingValue<int>("CategoryTopType", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CategoryTopType", (int)value, ModuleStringId); }
        }

        public static string CategoryTopTitle
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CategoryTopTitle", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CategoryTopTitle", value, ModuleStringId); }
        }

        public static ECategoryProductsType CategoryBottomType
        {
            get { return (ECategoryProductsType)ModuleSettingsProvider.GetSettingValue<int>("CategoryBottomType", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CategoryBottomType", (int)value, ModuleStringId); }
        }

        public static string CategoryBottomTitle
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CategoryBottomTitle", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CategoryBottomTitle", value, ModuleStringId); }
        }
    }
}