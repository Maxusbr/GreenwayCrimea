//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Scheduler;
using AdvantShop.Module.SupplierOfHappiness.Domain;

namespace AdvantShop.Module.SupplierOfHappiness
{
    public class SupplierOfHappiness : IModule, IModuleTask, IModuleChangeActive
    {
        #region IModule

        public static string ModuleID
        {
            get { return "SupplierOfHappiness"; }
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
                        return "Импорт каталога от Компаниии «Поставщик счастья»";

                    case "en":
                        return "Catalog import from Company «Supplier of Happiness»";

                    default:
                        return "Catalog import from Company «Supplier of Happiness»";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>
                    {
                        new SupplierOfHappinessImportControl(),
                        new SupplierOfHappinessSettingsControl()
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

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~\\Modules\\" + ModuleID + "\\temp\\")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~\\Modules\\" + ModuleID + "\\temp\\"));
            }

            ModuleSettingsProvider.SetSettingValue("DefaultCurrencyIso", "1", ModuleID);
            ModuleSettingsProvider.SetSettingValue("Process301Redirect", "true", ModuleID);

            if (SupplierOfHappinessRepository.InstallModule())
            {
                //SupplierOfHappinessService.UpdateCategoriesList();
                //SupplierOfHappinessService.SetDefaultCategories();
                return true;
            }

            return false;
        }

        public bool UpdateModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("TimePeriodValueFull", ModuleID);
            return SupplierOfHappinessRepository.UpdateModule();
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("DefaultCurrencyIso", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("Process301Redirect", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("TimePeriodValueFull", ModuleID);

            var taskManager = TaskManager.TaskManagerInstance();
            foreach (var task in GetTasks())
            {
                taskManager.RemoveModuleTask(task);
            }

            return true;
        }

        private class SupplierOfHappinessImportControl : IModuleControl
        {
            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Импорт каталога";

                        case "en":
                            return "Catalog import";

                        default:
                            return "Catalog import";
                    }
                }
            }

            public string File
            {
                get { return "SupplierOfHappinessImport.ascx"; }
            }
        }
        private class SupplierOfHappinessSettingsControl : IModuleControl
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
                get { return "SupplierOfHappinessSettings.ascx"; }
            }
        }

        #endregion

        #region IModuleTask

        public List<TaskSetting> GetTasks()
        {
            var timePeriodValueQuick = ModuleSettingsProvider.GetSettingValue<string>("TimePeriodValueQuick", ModuleID);

            var quickPeriod = 0;
            if (string.IsNullOrWhiteSpace(timePeriodValueQuick) || !Int32.TryParse(timePeriodValueQuick, out quickPeriod) || quickPeriod <= 0)
            {
                quickPeriod = 4;
                ModuleSettingsProvider.SetSettingValue("TimePeriodValueQuick", quickPeriod, ModuleID);
            }

            return new List<TaskSetting>()
            {
                new TaskSetting()
                {
                    Enabled = true,
                    JobType = typeof (SupplierOfHappinessFullJob).FullName + "," + typeof (SupplierOfHappinessFullJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Days,
                    TimeHours = 7,
                    TimeMinutes = new System.Random().Next(30)
                },
                new TaskSetting()
                {
                    Enabled = true,
                    JobType = typeof (SupplierOfHappinessQuickJob).FullName + "," + typeof (SupplierOfHappinessQuickJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Hours,
                    TimeInterval =  quickPeriod
                },
            };
        }

        #endregion

        #region IModuleChangeActive

        public void ModuleChangeActive(bool active)
        {
            if (!active)
            {
                var taskManager = TaskManager.TaskManagerInstance();
                foreach (var task in GetTasks())
                {
                    taskManager.RemoveModuleTask(task);
                }
            }
            else
            {
                TaskManager.TaskManagerInstance().ManagedTask(TaskSettings.Settings);
            }
        }

        #endregion

        #region SettingsModule

        public static string UrlPathFull
        {
            get { return "http://stripmag.ru/datafeed/advantshop_full.csv"; }
        }

        public static string UrlPathQuick
        {
            get { return "http://stripmag.ru/datafeed/advantshop_quick.csv"; }
        }

        public static string FilePathFull
        {
            get { return "~/modules/SupplierOfHappiness/temp/fullFileImport.txt"; }
        }

        public static string FilePathQuick
        {
            get { return "~/modules/SupplierOfHappiness/temp/quickFileImport.txt"; }
        }

        #endregion
    }
}