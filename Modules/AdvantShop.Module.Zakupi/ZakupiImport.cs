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
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Scheduler;
using AdvantShop.Module.ZakupiImport.Domain;

namespace AdvantShop.Module.ZakupiImport
{
    public class ZakupiImport : IModule, IModuleTask
    {
        #region Module

        public static string ModuleID
        {
            get { return "zakupiimport"; }
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
                        return "Импорт каталога от поставщика zakupi.net";

                    case "en":
                        return "Catalog import from supplier zakupi.net";

                    default:
                        return "Catalog import from supplier zakupi.net";
                }
            }
        }


        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>
                    {
                        new ZakupiImportCatalog()
                        //,new ZakupiImportSettings()
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
            ModuleSettingsProvider.SetSettingValue("AmountMappingType", "None", ModuleID);
            ModuleSettingsProvider.SetSettingValue("AmountMappingTypeField", string.Empty, ModuleID);

            ModuleSettingsProvider.SetSettingValue("ArtnoMappingType", "Attribute", ModuleID);
            ModuleSettingsProvider.SetSettingValue("ArtnoMappingTypeField", string.Empty, ModuleID);

            ModuleSettingsProvider.SetSettingValue("UpdateType", "Full", ModuleID);

            ModuleSettingsProvider.SetSettingValue("TimePeriod", "1", ModuleID);
            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("DefaultCurrencyIso", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("Process301Redirect", ModuleID);
            return true;
        }

        private class ZakupiImportCatalog : IModuleControl
        {
            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Импорт каталога от поставщика zakupi.net";

                        case "en":
                            return "Catalog import from supplier zakupi.net";

                        default:
                            return "Catalog import from supplier zakupi.net";
                    }
                }
            }

            public string File
            {
                get { return "ZakupiImportCatalog.ascx"; }
            }
        }

        #endregion

        public List<TaskSetting> GetTasks()
        {
            if (!ModuleSettingsProvider.GetSettingValue<bool>("AutoUpdateActive", ModuleID))
            {
                return new List<TaskSetting>();
            }

            var path = "~\\Modules\\" + ModuleID + "\\temp\\";

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(path)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
            }

            var filePath = HttpContext.Current.Server.MapPath(path);
            var fullPath = filePath + "products.yml";

            if (!string.IsNullOrEmpty(ModuleSettingsProvider.GetSettingValue<string>("FileUrlPath", ModuleID)))
            {
                try
                {
                    new WebClient().DownloadFile(
                        ModuleSettingsProvider.GetSettingValue<string>("FileUrlPath", ModuleID),
                        fullPath);
                }
                catch (Exception)
                {
                    return new List<TaskSetting>();
                }
            }

            if (!File.Exists(fullPath))
            {
                return new List<TaskSetting>();
            }

            ModuleSettingsProvider.SetSettingValue("FileUrlFullPath", fullPath, ModuleID);

            return new List<TaskSetting>()
            {
                new TaskSetting()
                {
                    Enabled = true,
                    JobType =  typeof (ZakupiImportJob).FullName + "," + typeof (ZakupiImportJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Hours,
                    TimeInterval = 5
                },

                new TaskSetting()
                {
                    Enabled = true,
                    JobType = typeof (ZakupiImportPartialJob).FullName + "," + typeof (ZakupiImportPartialJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Hours,
                    TimeInterval = 23
                }

            };
        }

        #region Settings

        public static bool UpdateName
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("UpdateName", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UpdateName", value, ModuleID); }
        }

        public static bool UpdateDescription
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("UpdateDescription", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UpdateDescription", value, ModuleID); }
        }

        public static bool UpdateParams
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("UpdateParams", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UpdateParams", value, ModuleID); }
        }

        public static bool UpdatePhotos
        {
            get { return ModuleSettingsProvider.GetSettingValue<bool>("UpdatePhotos", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("UpdatePhotos", value, ModuleID); }
        }

        public static string FileUrlPath
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("FileUrlPath", ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("FileUrlPath", value, ModuleID); }
        }

        #endregion
    }
}