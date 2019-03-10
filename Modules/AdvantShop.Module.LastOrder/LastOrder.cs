using AdvantShop.Core.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Core.Modules;
using AdvantShop.Module.LastOrder.Service;
using AdvantShop.Orders;
using AdvantShop.Core.Scheduler;

namespace AdvantShop.Module.LastOrder
{
    public class LastOrder : IModule, IRenderModuleByKey, IOrderChanged, IModuleTask, IModuleChangeActive
    {
        public bool HasSettings { get { return true; } }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl> { new ModuleWrapViewControl() };
            }
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    default: return "Купили сейчас";
                }
            }
        }

        public static string ModuleStringId
        {
            get
            {
                return "LastOrder";
            }
        }

        string IModule.ModuleStringId
        {
            get
            {
                return ModuleStringId;
            }
        }

        public bool CheckAlive() { return true; }

        public bool InstallModule() { return ModuleService.Install(); }

        public bool UninstallModule() { return ModuleService.UnInstall(); }

        public bool UpdateModule()
        {
            return true;
        }

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>
            {
                new ModuleRoute
                {
                    Key = "body_end",
                    ControllerName = "FNPClient",
                    ActionName = "LinkScriptStyle"
                },
                new ModuleRoute
                {
                    Key = "mobile_body_end",
                    ControllerName = "FNPClient",
                    ActionName = "LinkScriptStyle"
                },
                new ModuleRoute
                {
                    Key = "product_info",
                    ControllerName = "FNPClient",
                    ActionName = "Notify"
                }
            };
        } //IRenderModuleByKey

        public void DoOrderAdded(IOrder order)
        {
            FNPService.InsertOrUpdateNotification((Order)order);
        }

        public void DoOrderChangeStatus(IOrder order)
        {
        }

        public void DoOrderUpdated(IOrder order)
        {
        }

        public void DoOrderDeleted(int orderId)
        {
        }

        public void PayOrder(int orderId, bool payed)
        {
        }

        public static List<TaskSetting> GetTasks()
        {
            return new List<TaskSetting>()
            {
                new TaskSetting()
                {
                    Enabled = ModulesRepository.IsActiveModule(ModuleStringId),
                    JobType = typeof(FNPJob).FullName + "," + typeof(FNPJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Hours,
                    TimeInterval = 1
                }
            };
        }

        List<TaskSetting> IModuleTask.GetTasks()
        {
            return GetTasks();
        }

        public void ModuleChangeActive(bool active)
        {
            if (!active)
            {
                TaskManager.TaskManagerInstance().RemoveModuleTask(GetTasks().First());
                return;
            }
            TaskManager.TaskManagerInstance().ManagedTask(TaskSettings.Settings);
        }

        private class ModuleWrapViewControl : IModuleControl
        {
            public string File
            {
                get
                {
                    return "AdminView.ascx";
                }
            }

            public string NameTab
            {
                get
                {
                    return "Настройки";
                }
            }
        } //IModuleControl
    }
}
