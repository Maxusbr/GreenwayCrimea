using AdvantShop.Core.Modules.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Module.ReturnCustomer.Service;
using AdvantShop.Core.Scheduler;
using AdvantShop.Orders;

namespace AdvantShop.Module.ReturnCustomer
{
    public class ReturnCustomer : IModule, IAdminModuleSettings, IRenderModuleByKey, IModuleTask, IModuleChangeActive, IOrderChanged
    {
        public bool HasSettings
        {
            get { return true; }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return null;
            }
        }
        
        public string ModuleName
        {
            get
            {
                return CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? "Верните покупателя" : "Return customer";
            }
        }

        public static string ModuleStringId { get { return "ReturnCustomer"; } }

        string IModule.ModuleStringId { get { return ModuleStringId; } }

        public bool CheckAlive() { return true; }

        public bool InstallModule() { return RCService.Install(); }

        public bool UninstallModule() { return RCService.UnInstall(); }

        public bool UpdateModule() { return RCService.Update(); }

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>
            {
                new ModuleRoute
                {
                    Key = "body_start",
                    ControllerName = "RCClient",
                    ActionName = "UserAction"
                }
            };
        } 
       
        public List<TaskSetting> GetTasks()
        {
            return new List<TaskSetting>()
            {
                new TaskSetting()
                {
                    Enabled = RCSettings.AutoSending,
                    JobType = typeof (RCJob).FullName + "," + typeof (RCJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Hours,
                    TimeInterval = 1,
                    TimeHours = 0,
                    TimeMinutes = 0
                }
            };
        }

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

        public List<ModuleSettingTab> AdminSettings
        {
            get
            {
                return new List<ModuleSettingTab>()
                {
                    new ModuleSettingTab()
                    {
                        Title = "Список пользователей",
                        Controller = "RCAdmin",
                        Action = "RecordsList"
                    },
                    new ModuleSettingTab()
                    {
                        Title = "Настройки",
                        Controller = "RCAdmin",
                        Action = "Settings"
                    },
                    new ModuleSettingTab()
                    {
                        Title = "Обратная связь",
                        Controller = "RCAdmin",
                        Action = "Feedback"
                    }
                };
            }
        }
        
        public void DoOrderAdded(IOrder order)
        {
            RCService.UpdateRecordsWhenDoOrderAdded(order);
        }

        public void DoOrderChangeStatus(IOrder order)
        {
            return;
        }

        public void DoOrderUpdated(IOrder order)
        {
            return;
        }

        public void DoOrderDeleted(int orderId)
        {
            return;
        }

        public void PayOrder(int orderId, bool payed)
        {
            return;
        }
    }
}
