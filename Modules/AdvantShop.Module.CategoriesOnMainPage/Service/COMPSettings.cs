using AdvantShop.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.CategoriesOnMainPage.Service
{
    public class COMPSettings
    {
        public static readonly int Version = 0; //версия для скриптов

        public static string ModuleID = CategoriesOnMainPage.ModuleStringId;

        public enum Effects
        {
            none = 0,
            scale = 1,
            blackout = 2,
            contrast = 3
        }

        public static int PicturesQuantityInLine
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("PicturesQuantityInLine", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("PicturesQuantityInLine", value, ModuleID); }
        }

        public static int ImageWidth
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("ImageWidth", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ImageWidth", value, ModuleID); }
        }

        public static int ImageHeight
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("ImageHeight", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ImageHeight", value, ModuleID); }
        }

        public static bool NewWindow
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("NewWindow", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("NewWindow", value, ModuleID); }
        }

        public static bool NoShowCategoryName
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("NoShowCategoryName", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("NoShowCategoryName", value, ModuleID); }
        }

        public static bool NoShowBorder
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("NoShowBorder", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("NoShowBorder", value, ModuleID); }
        }

        public static bool UnderCarousel
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("UnderCarousel", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UnderCarousel", value, ModuleID); }
        }

        public static string Effect
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("Effect", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("Effect", value, ModuleID); }
        }

        public static bool SetDefaultSettings()
        {
            PicturesQuantityInLine = 4;
            ImageWidth = AdvantShop.Configuration.SettingsPictureSize.SmallCategoryImageWidth;
            ImageHeight = AdvantShop.Configuration.SettingsPictureSize.SmallCategoryImageHeight;
            NewWindow = true;
            NoShowCategoryName = false;
            NoShowBorder = false;
            UnderCarousel = true;
            Effect = Effects.scale.ToString();
            return true;
        }

        public static bool RemoveSettings()
        {
            ModuleSettingsProvider.RemoveSqlSetting("PicturesQuantityInLine", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("ImageWidth", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("ImageHeight", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("NewWindow", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("NoShowCategoryName", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("NoShowBorder", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("UnderCarousel", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("Effect", ModuleID);
            return true;
        }
    }
}
