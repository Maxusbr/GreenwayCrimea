using AdvantShop.Core.Modules;
using AdvantShop.Module.LastOrder.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.LastOrder.Service
{
    public class ModuleSettings
    {
        public static readonly int Version = 3; //версия для скриптов

        public static string ModuleID = LastOrder.ModuleStringId;

        public static string LastClearNotifications
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("LastClearNotifications", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("LastClearNotifications", value, ModuleID); }
        }

        #region Settings

        public static RangeTime rTime
        {
            get { return JsonConvert.DeserializeObject<RangeTime>(ModuleSettingsProvider.GetSettingValue<string>("rMinutes", ModuleID)); }
            set { ModuleSettingsProvider.SetSettingValue("rMinutes", JsonConvert.SerializeObject(value), ModuleID); }
        }

        public static SaveMaxPeriodModel SaveMaxPeriod
        {
            get { return JsonConvert.DeserializeObject<SaveMaxPeriodModel>(ModuleSettingsProvider.GetSettingValue<string>("SaveMaxPeriod", ModuleID)); }
            set { ModuleSettingsProvider.SetSettingValue("SaveMaxPeriod", JsonConvert.SerializeObject(value), ModuleID); }
        }

        public static int ShowPeriod
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("ShowPeriod", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ShowPeriod", value, ModuleID); }
        }

        public static List<string> Names
        {
            get { return JsonConvert.DeserializeObject<List<string>>(ModuleSettingsProvider.GetSettingValue<string>("Names", ModuleID)); }
            set { ModuleSettingsProvider.SetSettingValue("Names", JsonConvert.SerializeObject(value), ModuleID); }
        }

        public static List<string> Citys
        {
            get { return JsonConvert.DeserializeObject<List<string>>(ModuleSettingsProvider.GetSettingValue<string>("Citys", ModuleID)); }
            set { ModuleSettingsProvider.SetSettingValue("Citys", JsonConvert.SerializeObject(value), ModuleID); }
        }

        public static bool UseCustomNameAndCity
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("UseCustomName", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UseCustomName", value, ModuleID); }
        }

        public static string Location
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("Location", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("Location", value, ModuleID); }
        }

        public static int ShowUserCity
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("ShowUserCity", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ShowUserCity", value, ModuleID); }
        }

        /*public static bool AlwaysShow // Показывать в товарах "Не в наличии"
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("AlwaysShow", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AlwaysShow", value, ModuleID); }
        }*/

        public static bool SetDefaultSettings()
        {
            rTime = new RangeTime { from = 10, to = 45, rType = RangeTimeType.Minutes };
            SaveMaxPeriod = new SaveMaxPeriodModel { Period = 1, PeriodType = SaveMaxPeriodType.Days };
            ShowPeriod = 4;
            Names = new List<string>();
            Citys = new List<string>();
            UseCustomNameAndCity = false;
            LastClearNotifications = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Location = ".details-payment";
            ShowUserCity = 0;
            //AlwaysShow = false;
            return true;
        }

        public static bool RemoveSettings()
        {
            ModuleSettingsProvider.RemoveSqlSetting("rMinutes", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("SaveMaxPeriod", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("ShowPeriod", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("Names", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("Citys", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("UseCustomNameAndCity", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("LastClearNotifications", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("Location", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("ShowUserCity", ModuleID);
            //ModuleSettingsProvider.RemoveSqlSetting("AlwaysShow", ModuleID);
            return true;
        }
        
        #endregion 

        public RangeTime rm { get; set; }
        public SaveMaxPeriodModel smpm { get; set; }
        public int sp { get; set; }
        public List<string> n { get; set; }
        public List<string> c { get; set; }
        public bool ucnac { get; set; }
        public string loc { get; set; }
        public int userCity { get; set; }
        //public bool alwaysShow { get; set; }

        public object SaveSettings()
        {
            rTime = rm;
            SaveMaxPeriod = smpm;
            ShowPeriod = sp;
            UseCustomNameAndCity = ucnac;
            if (ucnac)
            {
                Names = n;
                Citys = c;
            }
            Location = loc;
            ShowUserCity = userCity;
            //AlwaysShow = alwaysShow;
            return new { success = true };
        }

        public ModuleSettings()
        {
            rm = rTime;
            smpm = SaveMaxPeriod;
            sp = ShowPeriod;
            n = Names;
            c = Citys;
            ucnac = UseCustomNameAndCity;
            loc = Location;
            userCity = ShowUserCity;
            //alwaysShow = AlwaysShow;
        }
    }
}
