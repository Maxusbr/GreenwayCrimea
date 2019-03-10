using AdvantShop.Core.Modules;

namespace AdvantShop.Module.Rees46.Domain
{
    public class Rees46Settings
    {
        private const string ModuleStringId = "Rees46";

        public static string ShopKey
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ShopKey", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ShopKey", value, ModuleStringId); }
        }

        public static string RelatedProduct
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("RelatedProduct", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("RelatedProduct", value, ModuleStringId); }
        }

        public static string AlternativeProduct
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("AlternativeProduct", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("AlternativeProduct", value, ModuleStringId); }
        }

        public static int Limit
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("Limit", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("Limit", value, ModuleStringId); }
        }

        // catalog bottom
        public static string CatalogBottomBlock
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CatalogBottomBlock", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CatalogBottomBlock", value, ModuleStringId); }
        }

        public static string CatalogBottomBlockTitle
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CatalogBottomBlockTitle", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CatalogBottomBlockTitle", value, ModuleStringId); }
        }

        // catalog top
        public static string CatalogTopBlock
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CatalogTopBlock", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CatalogTopBlock", value, ModuleStringId); }
        }

        public static string CatalogTopBlockTitle
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CatalogTopBlockTitle", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CatalogTopBlockTitle", value, ModuleStringId); }
        }

        // main page
        public static string MainPageBlock
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MainPageBlock", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("MainPageBlock", value, ModuleStringId); }
        }

        public static string MainPageBlockTitle
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MainPageBlockTitle", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("MainPageBlockTitle", value, ModuleStringId); }
        }


        // display in shopping cart
        public static bool DisplayInShoppingCart
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("DisplayInShoppingCart", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("DisplayInShoppingCart", value, ModuleStringId); }
        }

        public static string ShoppingCartTitle
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ShoppingCartTitle", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ShoppingCartTitle", value, ModuleStringId); }
        }

        public static string PathFilePushSW
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("PathFilePushSW", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("PathFilePushSW", value, ModuleStringId); }
        }

        public static bool RegisteredShop
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("RegisteredShop", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("RegisteredShop", value, ModuleStringId); }
        }

        public static string SecretKey
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("SecretKey", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("SecretKey", value, ModuleStringId); }
        }
    }
}