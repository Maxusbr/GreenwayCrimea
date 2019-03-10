using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Scheduler;
using AdvantShop.Module.VkMarket.Services;

namespace AdvantShop.Module.VkMarket
{
    public class VkMarket : IModule, IAdminModuleSettings, IModuleTask, IVkProduct
    {
        public const string ModuleId = "VkMarket";

        #region IModule

        public string ModuleStringId
        {
            get { return ModuleId; }
        }
        
        public string ModuleName
        {
            get
            {
                return
                    CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru"
                        ? "Товары Вконтакте"
                        : "Vk.com market";
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return null; }
        }

        public bool HasSettings
        {
            get { return false; }
        }

        public bool CheckAlive()
        {
            return true;
        }

        public bool InstallModule()
        {
            return InstallUpdateModuleService.Install();
        }

        public bool UpdateModule()
        {
            return true;
        }

        public bool UninstallModule()
        {
            InstallUpdateModuleService.Delete();

            var taskManager = TaskManager.TaskManagerInstance();
            foreach (var task in GetTasks())
                taskManager.RemoveModuleTask(task);
            
            return true;
        }

        #endregion

        #region IAdminModuleSettings

        public List<ModuleSettingTab> AdminSettings
        {
            get
            {
                return new List<ModuleSettingTab>()
                {
                    new ModuleSettingTab()
                    {
                        Title = "Экспорт товаров из магазина в ВКонтакте",
                        Controller = "VkMarketSettings",
                        Action = "ExportSettings",
                    },
                    new ModuleSettingTab()
                    {
                        Title = "Импорт товаров из ВКонтакте в магазин",
                        Controller = "VkMarketSettings",
                        Action = "ImportSettings",
                    },
                };
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
                    JobType = typeof (VkMarketExportJob).FullName + "," + typeof (VkMarketExportJob).Assembly.FullName,
                    TimeType = TimeIntervalType.Hours,
                    TimeInterval = 12
                }
            };
        }

        #endregion

        #region IVkProduct

        public int GetProductIdByMarketId(long marketId)
        {
            return new VkProductService().GetProductIdByMarketId(marketId);
        }

        #endregion
    }
}
