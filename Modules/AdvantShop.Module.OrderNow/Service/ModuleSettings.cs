using AdvantShop.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.OrderNow.Service
{
    public class ModuleSettings
    {
        public static readonly int Version = 0; //версия для скриптов

        public static string ModuleID = OrderNow.ModuleStringId;

        public static string Message
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("Message", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("Message", value, ModuleID); }
        }

        public static string TimeoutMessage
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("TimeoutMessage", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("TimeoutMessage", value, ModuleID); }
        }

        public static string TimeoutTime
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("TimeoutTime", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("TimeoutTime", value, ModuleID); }
        }

        public static bool IncludeWeekend 
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("IncludeWeekend", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("IncludeWeekend", value, ModuleID); }
        }

        public static bool LookForAvailability
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("LookForAvailability", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("LookForAvailability", value, ModuleID); }
        }

        public static bool ShowInCart
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ShowInCart", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ShowInCart", value, ModuleID); }
        }

        public static bool ShowInOrderConfirm
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ShowInOrderConfirm", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ShowInOrderConfirm", value, ModuleID); }
        }

        public static string ShowAt
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ShowAt", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ShowAt", value, ModuleID); }
        }

        public static bool IconUsed
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("IconUsed", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("IconUsed", value, ModuleID); }
        }

        public static int IconHeight
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("IconHeight", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("IconHeight", value, ModuleID); }
        }

        public static string ShowStart
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ShowStart", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ShowStart", value, ModuleID); }
        }

        public static string ShowFinish
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ShowFinish", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ShowFinish", value, ModuleID); }
        }

        public static int Ndays
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("Ndays", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("Ndays", value, ModuleID); }
        }

        public static bool ShowInMobile
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("ShowInMobile", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ShowInMobile", value, ModuleID); }
        }

        public static bool SetDefaultSettings()
        {
            Message = "Хотите получить этот товар быстрее? Закажите в течение #DATE# и мы отправим его уже #TODAY#.";
            TimeoutTime = "19:00";
            IncludeWeekend = false;
            LookForAvailability = false;
            ShowInCart = false;
            ShowInOrderConfirm = false;
            ShowAt = "Над рейтингом";
            IconUsed = false;
            IconHeight = 45;
            ShowStart = "10:00";
            ShowFinish = "19:00";
            Ndays = 4;
            ShowInMobile = false;
            TimeoutMessage = "Товар будет отправлен #TOMORROW#.";
            return true;
        }

        public static bool RemoveSettings()
        {
            ModuleSettingsProvider.RemoveSqlSetting("Message", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("TimeoutTime", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("IncludeWeekend", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("LookForAvailability", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("ShowInCart", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("ShowInOrderConfirm", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("ShowAt", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("IconUsed", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("IconHeight", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("Ndays", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("ShowStart", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("ShowFinish", ModuleID);
            return true;
        }
    }
}
