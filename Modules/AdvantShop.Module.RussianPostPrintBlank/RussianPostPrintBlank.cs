using AdvantShop.Core.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Core.Modules;
using AdvantShop.Module.RussianPostPrintBlank.Service;

namespace AdvantShop.Module.RussianPostPrintBlank
{
    public class RussianPostPrintBlank : IModule, IAdminModuleSettings, IAdminBundles, IRenderModuleByKey, IRenderAdminModuleByKey
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
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru": return "Печать бланков почты РФ";
                    case "en": return "Russian post print blanks";
                    default: return "Russian post print blanks";
                }
            }
        }

        public static string ModuleStringId { get { return "RussianPostPrintBlank"; } }

        string IModule.ModuleStringId { get { return ModuleStringId; } }

        public bool CheckAlive() { return true; }

        public bool InstallModule() { return RPPBService.Install(); }

        public bool UninstallModule() { return RPPBService.UnInstall(); }

        public bool UpdateModule() { return true; }
        
        public List<string> AdminCssBottom()
        {
            return null;
        }

        public List<string> AdminJsBottom()
        {
            return new List<string>()
            {
                "~/modules/RussianPostPrintBlank/content/Scripts/admin-script.js",
            };
        }

        public List<ModuleRoute> GetAdminModuleRoutes()
        {
            return new List<ModuleRoute>
            {
                new ModuleRoute
                {
                    Key = "admin_order_orderinfo",
                    ControllerName = "RPPBAdmin",
                    ActionName = "OrderInfoTemplatesList"
                }
            };
        }

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>
            {
                new ModuleRoute
                {
                    Key = "admin_order_orderinfo",
                    ControllerName = "RPPBAdmin",
                    ActionName = "OrderInfoTemplatesList"
                }
            };
        }

        public List<ModuleSettingTab> AdminSettings
        {
            get
            {
                return new List<ModuleSettingTab>()
                {
                    new ModuleSettingTab()
                    {
                        Title = "Список заказов",
                        Controller = "RPPBAdmin",
                        Action = "OrderSearch"
                    },
                    new ModuleSettingTab()
                    {
                        Title = "Шаблоны",
                        Controller = "RPPBAdmin",
                        Action = "Templates"
                    },
                    new ModuleSettingTab()
                    {
                        Title = "Обратная связь",
                        Controller = "RPPBAdmin",
                        Action = "Feedback"
                    }
                };
            }
        }
    }
}
