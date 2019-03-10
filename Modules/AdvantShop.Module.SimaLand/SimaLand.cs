using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using System.Globalization;
using AdvantShop.Module.SimaLand.Service;
using AdvantShop.Core.Scheduler;

namespace AdvantShop.Module.SimaLand
{
    public class SimaLand : IModule, IModuleTask, IRenderModuleByKey
    {
        #region Implementation of IModule

        public bool HasSettings
        {
            get
            {
                return true;
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl> { new ComparisonCategory(),  new PriceAndBalance(), new ProductAttributes(), new ModuleSettings(), new CurrentProcess() };
            }
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru": return "СимаЛенд";
                    case "en": return "SimaLand";
                    default: return "SimaLand";
                }
            }
        }
        
        string IModule.ModuleStringId
        {
            get
            {
                return ModuleStringId;
            }
        }

        public bool CheckAlive()
        {
            return true;
        }

        public bool InstallModule()
        {
            PSLModuleSettings.SetSettings();
            return ModuleService.Install();
        }

        public bool UninstallModule()
        {
            var un = ModuleService.UnInstall();
            PSLModuleSettings.RemoveSettings();
            return un;
        }

        public bool UpdateModule()
        {
            return true;
        }

        #endregion

        public static List<TaskSetting> GetTasks()
        {
            return new List<TaskSetting>()
            {
                new TaskSetting()
                {
                    Enabled = PSLModuleSettings.AutoUpdate,
                    JobType = typeof (JobService).FullName + "," + typeof(JobService).Assembly.FullName,
                    TimeType = TimeIntervalType.Days,
                    TimeHours = Convert.ToInt32(PSLModuleSettings.TimeUpdate.Split(':')[0]),
                    TimeMinutes = Convert.ToInt32(PSLModuleSettings.TimeUpdate.Split(':')[1])
                },
                new TaskSetting()
                {
                    Enabled = PSLModuleSettings.AutoUpdateBalance,
                    JobType = typeof (UpdateBalanceJob).FullName + "," + typeof(UpdateBalanceJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Hours,
                    TimeInterval = PSLModuleSettings.TimePeriodBalance
                }
            };
        }

        List<TaskSetting> IModuleTask.GetTasks()
        {
            return GetTasks();
        }

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>
            {
                new ModuleRoute
                {
                   Key = "product_middle",
                   ControllerName = "Client",
                   ActionName = "Index"
                },
                new ModuleRoute
                {
                   Key = "product_middle",
                   ControllerName = "Client",
                   ActionName = "GetLabel"                   
                },
                new ModuleRoute
                {
                   Key = "product_middle",
                   ControllerName = "Client",
                   ActionName = "GetButtons"
                },
                new ModuleRoute
                {
                   Key = "body_end",
                   ControllerName = "Client",
                   ActionName = "GetLabelsScript"
                },
                new ModuleRoute
                {
                   Key = "body_start",
                   ControllerName = "Client",
                   ActionName = "LinkCss"
                }
            };
        }

        public static string ModuleStringId
        {
            get
            {
                return "SimaLand";
            }
        }

        #region Implementation of IModuleControl

        private class ModuleSettings : IModuleControl
        {
            public string File
            {
                get
                {
                    return "ModuleSettings.ascx";
                }
            }

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru": return "Загрузка товаров";
                        case "en": return "Settings";
                        default: return "Settings";
                    }
                }
            }
        }

        private class ProductAttributes : IModuleControl
        {
            public string File
            {
                get
                {
                    return "ProductAttributes.ascx";
                }
            }

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru": return "Обновление атрибутов товара";
                        default: return "Update attributes";
                    }
                }
            }
        }

        private class CurrentProcess : IModuleControl
        {
            public string File
            {
                get
                {
                    return "CurrentProcess.ascx";
                }
            }

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru": return "Текущий процесс";
                        case "en": return "Settings";
                        default: return "Settings";
                    }
                }
            }
        }

        private class PriceAndBalance : IModuleControl
        {
            public string File
            {
                get
                {
                    return "PriceAndBalance.ascx";
                }
            }

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru": return "Цены";
                        case "en": return "Pricing";
                        default: return "Pricing";
                    }
                }
            }
        }

        private class ComparisonCategory : IModuleControl
        {
            public string File
            {
                get
                {
                    return "ComparisonCategory.ascx";
                }
            }

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
                    {
                        case "ru": return "Работа с каталогами";
                        case "en": return "Comparison category";
                        default: return "Comparison category";
                    }
                }
            }
        }

        #endregion       

    }
}
