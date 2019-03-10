//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Scheduler;
using AdvantShop.Module.YandexMarketImport.Domain;

namespace AdvantShop.Module.YandexMarketImport
{
    public class YandexMarketImport : IModule, IModuleTask
    {
        #region Module

        public static string ModuleID
        {
            get { return "yandexmarketimport"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleID; }
        }


        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Импорт каталога из market.yandex.ru";

                    case "en":
                        return "Catalog import from market.yandex.ru";

                    default:
                        return "Catalog import from market.yandex.ru";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>
                    {
                        new YandexMarketImportCatalog(),
                        new YandexMarketImportSettings()
                    };
            }
        }

        public bool HasSettings
        {
            get { return true; }
        }

        public bool CheckAlive()
        {
            return ModulesRepository.IsInstallModule(ModuleID);
        }

        public bool InstallModule()
        {
            ModuleSettingsProvider.SetSettingValue("DefaultCurrencyIso", "1", ModuleID);
            ModuleSettingsProvider.SetSettingValue("Process301Redirect", "true", ModuleID);
            return true;
        }

        public bool UpdateModule()
        {
            ModuleSettingsProvider.SetSettingValue("AmountMappingType", "None", YandexMarketImport.ModuleID);
            ModuleSettingsProvider.SetSettingValue("AmountMappingTypeField", string.Empty, YandexMarketImport.ModuleID);

            ModuleSettingsProvider.SetSettingValue("ArtnoMappingType", "Attribute", YandexMarketImport.ModuleID);
            ModuleSettingsProvider.SetSettingValue("ArtnoMappingTypeField", string.Empty, YandexMarketImport.ModuleID);

            ModuleSettingsProvider.SetSettingValue("UpdateType", "Full", YandexMarketImport.ModuleID);

            ModuleSettingsProvider.SetSettingValue("TimePeriod", "1", YandexMarketImport.ModuleID);
            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("DefaultCurrencyIso", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("Process301Redirect", ModuleID);
            return true;
        }

        private class YandexMarketImportCatalog : IModuleControl
        {
            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Импорт каталога из market.yandex.ru";

                        case "en":
                            return "Catalog import from market.yandex.ru";

                        default:
                            return "Catalog import from market.yandex.ru";
                    }
                }
            }

            public string File
            {
                get { return "YandexMarketImportCatalog.ascx"; }
            }
        }

        private class YandexMarketImportSettings : IModuleControl
        {
            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Расширенные настройки";

                        case "en":
                            return "Additional settings";

                        default:
                            return "Additional settings";
                    }
                }
            }

            public string File
            {
                get { return "YandexMarketImportSettings.ascx"; }
            }
        }

        #endregion

        #region settings

        public static float DefaultCurrencyIso
        {
            get { return ModuleSettingsProvider.GetSettingValue<float>("DefaultCurrencyIso", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("DefaultCurrencyIso", value, ModuleID); }
        }

        public static bool Process301Redirect
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("Process301Redirect", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("Process301Redirect", value, ModuleID); }
        }

        public static bool AmountNulling
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("AmountNulling", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AmountNulling", value, ModuleID); }
        }

        public static bool DeactivateProducts
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("DeactivateProducts", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("DeactivateProducts", value, ModuleID); }
        }

        public static string AmountMappingType
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("AmountMappingType", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AmountMappingType", value, ModuleID); }
        }

        public static string TagForNameProduct
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("TagForNameProduct", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("TagForNameProduct", value, ModuleID); }
        }

        public static string AmountMappingTypeField
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("AmountMappingTypeField", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AmountMappingTypeField", value, ModuleID); }
        }

        public static string ArtnoMappingType
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ArtnoMappingType", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ArtnoMappingType", value, ModuleID); }
        }

        public static string ArtnoMappingTypeField
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ArtnoMappingTypeField", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ArtnoMappingTypeField", value, ModuleID); }
        }
        
        public static string ArtnoProductMappingType
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ArtnoProductMappingType", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ArtnoProductMappingType", value, ModuleID); }
        }

        public static string ArtnoProductMappingTypeField
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ArtnoProductMappingTypeField", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ArtnoProductMappingTypeField", value, ModuleID); }
        }

        public static bool AutoUpdateActive
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("AutoUpdateActive", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AutoUpdateActive", value, ModuleID); }
        }

        public static string UpdateType
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("UpdateType", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UpdateType", value, ModuleID); }
        }

        public static string TimePeriod
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("TimePeriod", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("TimePeriod", value, ModuleID); }
        }

        public static string TimePeriodValue
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("TimePeriodValue", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("TimePeriodValue", value, ModuleID); }
        }

        public static string FileUrlPath
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("FileUrlPath", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("FileUrlPath", value, ModuleID); }
        }

        public static bool DeleteOldPrices
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("DeleteOldPrices", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("DeleteOldPrices", value, ModuleID); }
        }

        public static bool AllowPreorder
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("AllowPreorder", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("AllowPreorder", value, ModuleID); }
        }

        public static int ExtraCharge
        {
            get { return ModuleSettingsProvider.GetSettingValue<int>("ExtraCharge", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ExtraCharge", value, ModuleID); }
        }

        #endregion settings

        public List<TaskSetting> GetTasks()
        {
            if (!ModuleSettingsProvider.GetSettingValue<bool>("AutoUpdateActive", ModuleID))
            {
                return new List<TaskSetting>();
            }

            //var path = "~\\Modules\\" + ModuleID + "\\temp\\";

            //if (!Directory.Exists(HttpContext.Current.Server.MapPath(path)))
            //{
            //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
            //}

            //var filePath = HttpContext.Current.Server.MapPath(path);
            //var fullPath = filePath + "products.yml";

            //if (!string.IsNullOrEmpty(ModuleSettingsProvider.GetSettingValue<string>("FileUrlPath", ModuleID)))
            //{
            //    try
            //    {
            //        new WebClient().DownloadFile(
            //            ModuleSettingsProvider.GetSettingValue<string>("FileUrlPath", ModuleID),
            //            fullPath);
            //    }
            //    catch (Exception)
            //    {
            //        return new List<TaskSetting>();
            //    }
            //}

            //if (!File.Exists(fullPath))
            //{
            //    return new List<TaskSetting>();
            //}

            //ModuleSettingsProvider.SetSettingValue("FileUrlFullPath", fullPath, ModuleID);

            return new List<TaskSetting>()
            {
                new TaskSetting()
                {
                    Enabled = true,
                    JobType = typeof (YandexMarketImportJob).FullName + "," + typeof (YandexMarketImportJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Hours,
                    TimeInterval = ModuleSettingsProvider.GetSettingValue<string>("TimePeriodValue", ModuleID).TryParseInt()
                }
            };
        }
    }
}