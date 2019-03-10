//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Scheduler;
using AdvantShop.Module.Resellers.Domain;

namespace AdvantShop.Module.Resellers
{
    public class Reseller : IModule, IModuleTask, IModuleChangeActive
    {
        #region Module

        public static string ModuleID
        {
            get { return "Reseller"; }
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
                        return "Дропшипперы (Реселлеры). Импорт каталога.";

                    case "en":
                        return "Dropshippers. Import catalog";

                    default:
                        return "Dropshippers. Import catalog";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> {new ResellerSettingsControl()}; }
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
            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("DefaultCurrencyIso", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("Process301Redirect", ModuleID);
            return true;
        }

        private class ResellerSettingsControl : IModuleControl
        {
            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Настройки";

                        case "en":
                            return "Settings";

                        default:
                            return "Settings";
                    }
                }
            }

            public string File
            {
                get { return "ResellerSettings.ascx"; }
            }
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

        #region IModuleTask

        public List<TaskSetting> GetTasks()
        {
            return new List<TaskSetting>()
            {
                new TaskSetting()
                {
                    Enabled = true,
                    JobType = typeof(ImportJob).FullName + "," + typeof(ImportJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Hours,
                    TimeInterval = 6
                }
            };
        }

        #endregion
    }
}