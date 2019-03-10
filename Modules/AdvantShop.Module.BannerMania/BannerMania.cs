using AdvantShop.Core.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Core.Modules;
using AdvantShop.Module.BannerMania.Service;

namespace AdvantShop.Module.BannerMania
{
    public class BannerMania : IModule, IAdminModuleSettings, IRenderModuleByKey, IModuleBundles, IAdminBundles
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
                return CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? "Баннер-микс" : "Banner mix";
            }
        }

        public static string ModuleStringId { get { return "BannerMania"; } }

        string IModule.ModuleStringId { get { return ModuleStringId; } }

        public bool CheckAlive() { return true; }

        public bool InstallModule() { return BMService.Install(); }

        public bool UninstallModule() { return BMService.UnInstall(); }

        public bool UpdateModule() { return BMService.Update(); }

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>
            {
                new ModuleRoute
                {
                    Key = "body_start",
                    ControllerName = "BMClient",
                    ActionName = "ShowBannerInTop"
                },
                new ModuleRoute
                {
                    Key = "product_info",
                    ControllerName = "BMClient",
                    ActionName = "ShowProductEntityBanners"
                },
                new ModuleRoute
                {
                    Key = "category_top",
                    ControllerName = "BMClient",
                    ActionName = "ShowCategoryEntityBanners"
                }
            };
        }

        public List<string> GetCssBundles()
        {
            return new List<string>()
            {
                "~/modules/bannermania/content/styles/client-style.css"
            };
        }

        public List<string> GetJsBundles()
        {
            return new List<string>()
            {
                "~/modules/bannermania/content/scripts/client-script.js"
            };
        }

        public List<string> AdminCssBottom()
        {
            return new List<string>()
            {
                "~/modules/bannermania/content/styles/admin-style.css",
            };
        }

        public List<string> AdminJsBottom()
        {
            return null;
        }

        public List<ModuleSettingTab> AdminSettings
        {
            get
            {
                return new List<ModuleSettingTab>()
                {
                    new ModuleSettingTab()
                    {
                        Title = "Баннер в шапке",
                        Controller = "BITAdmin",
                        Action = "BannerInTop"
                    },
                    new ModuleSettingTab()
                    {
                        Title = "Каталог и товары",
                        Controller = "BFPAdmin",
                        Action = "BannerForProducts"
                    },
                    new ModuleSettingTab()
                    {
                        Title = "Настройки",
                        Controller = "BMSettingsAdmin",
                        Action = "CommonSettings"
                    },
                    new ModuleSettingTab()
                    {
                        Title = "Обратная связь",
                        Controller = "BMSettingsAdmin",
                        Action = "Feedback"
                    }
                };
            }
        }
    }
}
