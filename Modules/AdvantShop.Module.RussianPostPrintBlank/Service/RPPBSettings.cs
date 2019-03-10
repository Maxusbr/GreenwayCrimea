using AdvantShop.Core.Modules;

namespace AdvantShop.Module.RussianPostPrintBlank.Service
{
    public class RPPBSettings
    {
        public static readonly int Version = 0; //версия для скриптов

        public static string ModuleID = RussianPostPrintBlank.ModuleStringId;

        /*public static string 
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("", value, ModuleID); }
        }*/
        

        public static bool SetDefaultSettings()
        {
            return true;
        }

        public static bool RemoveSettings()
        {
            //ModuleSettingsProvider.RemoveSqlSetting("", ModuleID);
            return true;
        }
    }
}
