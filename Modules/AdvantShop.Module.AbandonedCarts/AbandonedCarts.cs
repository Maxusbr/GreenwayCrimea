using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Scheduler;
using AdvantShop.Module.AbandonedCarts.Domain;

namespace AdvantShop.Module.AbandonedCarts
{
    public class AbandonedCarts : IModule, IModuleTask, IModuleChangeActive
    {
        #region Module methods

        public string ModuleStringId
        {
            get { return "AbandonedCarts"; }
        }
        
        public bool HasSettings
        {
            get { return true; }
        }

        public bool CheckAlive()
        {
            return true;
        }

        public bool InstallModule()
        {
            return AbandonedCartsService.InstallModule();
        }

        public bool UpdateModule()
        {
            return true;
        }

        public bool UninstallModule()
        {
            var taskManager = TaskManager.TaskManagerInstance();
            foreach (var task in GetTasks())
            {
                taskManager.RemoveModuleTask(task);
            }
            return true;
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Брошенные корзины";

                    case "en":
                        return "Abandoned carts";

                    default:
                        return "Abandoned carts";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>
                    {
                        new AbandonedCartsSetting(),
                        new AbandonedCartsSettingUnReg(),
                        new AbandonedCartsModuleWishList(),
                        new AbandonedCartsModuleWishListUnReg(),
                        new AbandonedCartsTemplateSetting()
                    };
            }
        }

        private class AbandonedCartsSetting : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Корзины зарег. пол.";

                        case "en":
                            return "Shopping carts (reg. users)";

                        default:
                            return "Shopping carts (reg. users)";
                    }
                }
            }

            public string File
            {
                get { return "AbandonedCartsModule.ascx"; }
            }

            #endregion
        }

        private class AbandonedCartsSettingUnReg : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Корзины незарег. пол.";

                        case "en":
                            return "Shopping carts (guests)";

                        default:
                            return "Shopping carts (guests)";
                    }
                }
            }

            public string File
            {
                get { return "AbandonedCartsModuleUnReg.ascx"; }
            }

            #endregion
        }

        private class AbandonedCartsModuleWishList : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Список желаний зарег. пол.";

                        case "en":
                            return "Wish List (guests)";

                        default:
                            return "Wish List (guests)";
                    }
                }
            }

            public string File
            {
                get { return "AbandonedCartsModuleWishList.ascx"; }
            }

            #endregion
        }

        private class AbandonedCartsModuleWishListUnReg : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Список желаний незарег. пол.";

                        case "en":
                            return "Wish List (guests)";

                        default:
                            return "Wish List (guests)";
                    }
                }
            }

            public string File
            {
                get { return "AbandonedCartsModuleWishListUnReg.ascx"; }
            }

            #endregion
        }

        private class AbandonedCartsTemplateSetting : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Шаблоны уведомлений";

                        case "en":
                            return "Notification templates";

                        default:
                            return "Notification templates";
                    }
                }
            }

            public string File
            {
                get { return "AbandonedCartsTemplates.ascx"; }
            }

            #endregion
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
            return new List<TaskSetting>
            {
                new TaskSetting
                {
                    Enabled = true,
                    JobType = typeof (AbandonedCartsJob).FullName + "," + typeof (AbandonedCartsJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Hours,
                    TimeInterval = 1
                }
            };
        }

        #endregion
    }
}