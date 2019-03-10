using AdvantShop.Core.Modules;
using Newtonsoft.Json;

namespace AdvantShop.Module.Journal.Domain
{
    public class JournalModuleSetting
    {
        public static int CoverType
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("CoverType", Journal.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CoverType", value, Journal.ModuleStringId); }
        }

        public static string CoverTop
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CoverTop", Journal.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CoverTop", value, Journal.ModuleStringId); } 
        }

        public static string CoverMiddle
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CoverMiddle", Journal.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CoverMiddle", value, Journal.ModuleStringId); }
        }

        public static string CoverBottom
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CoverBottom", Journal.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CoverBottom", value, Journal.ModuleStringId); }
        }

        public static bool ShowCover
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ShowCover", Journal.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ShowCover", value, Journal.ModuleStringId); }
        }

        public static int ViewMode
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("ViewMode", Journal.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ViewMode", value, Journal.ModuleStringId); }
        }

        public static string CatalogPageHead
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CatalogPageHead", Journal.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CatalogPageHead", value, Journal.ModuleStringId); }
        }
        
        public static string CatalogPageBottomLeft
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CatalogPageBottomLeft", Journal.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CatalogPageBottomLeft", value, Journal.ModuleStringId); }
        }

        public static string CatalogPageBottomRight
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CatalogPageBottomRight", Journal.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("CatalogPageBottomRight", value, Journal.ModuleStringId); }
        }

        public static JournalExport JournalExport
        {
            get
            {
                var ids = ModuleSettingsProvider.GetSettingValue<string>("CategoryIds", Journal.ModuleStringId);

                return !string.IsNullOrEmpty(ids)
                            ? JsonConvert.DeserializeObject<JournalExport>(ids)
                            : new JournalExport();
            }
            set
            {
                ModuleSettingsProvider.SetSettingValue("CategoryIds", JsonConvert.SerializeObject(value), Journal.ModuleStringId);
            }
        }

        public static int ItemsPerPage
        {
            get
            {
                var count = ModuleSettingsProvider.GetSettingValue<int>("ItemsPerPage", Journal.ModuleStringId);
                return count != 0 ? count : 3;
            }
            set { ModuleSettingsProvider.SetSettingValue("ItemsPerPage", value, Journal.ModuleStringId); }
        }

        public static bool ShowOnlyAvalible
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ShowOnlyAvalible", Journal.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ShowOnlyAvalible", value, Journal.ModuleStringId); }
        }

        public static bool ShowArtNo
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ShowArtNo", Journal.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ShowArtNo", value, Journal.ModuleStringId); }
        }

        public static bool MoveNotAvaliableToEnd
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("MoveNotAvaliableToEnd", Journal.ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("MoveNotAvaliableToEnd", value, Journal.ModuleStringId); }
        }
    }
}
