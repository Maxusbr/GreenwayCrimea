using AdvantShop.Core.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Core.Modules;
using AdvantShop.Module.AdditionalMarkers.Service;
using AdvantShop.ExportImport;

namespace AdvantShop.Module.AdditionalMarkers
{
    public class AdditionalMarkers : IModule, IRenderModuleByKey, IAdminBundles, IModuleBundles, IAdminModuleSettings, IAdminProductTabs, ICSVExportImport
    {
        public bool HasSettings
        {
            get { return true; }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>();
            }
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    default: return "Дополнительные маркеры";
                }
            }
        }

        public static string ModuleStringId { get { return "AdditionalMarkers"; } }

        string IModule.ModuleStringId { get { return ModuleStringId; } }

        public bool CheckAlive() { return true; }

        public bool InstallModule() { return ModuleService.Install(); }

        public bool UninstallModule() { return ModuleService.UnInstall(); }

        public bool UpdateModule() { return true; }

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute> {
                new ModuleRoute
                {
                    Key = "product_info",
                    ControllerName = "AMClient",
                    ActionName = "GetMarkers"
                },
                new ModuleRoute
                {
                    Key = "body_end",
                    ControllerName = "AMClient",
                    ActionName = "GetProductViewMarkers"
                },
                new ModuleRoute
                {
                    Key = "mobile_body_end",
                    ControllerName = "AMClient",
                    ActionName = "GetProductViewMarkers"
                }
            };
        }

        public List<string> AdminCssBottom()
        {
            return null;
        }

        public List<string> AdminJsBottom()
        {
            return null;
        }

        public List<string> GetCssBundles()
        {
            return null;
        }

        public List<string> GetJsBundles()
        {
            return new List<string>{
                "~/modules/" + ModuleStringId + "/content/scripts/client-script.js"
            };
        }

        public IList<AdminProductTabItem> GetAdminProductTabs(int productId)
        {
            return new List<AdminProductTabItem>
            {
                new AdminProductTabItem("Дополнительные маркеры", "AdminProductTab", "AMAdmin")
            };
        }

        public IList<CSVField> GetCSVFields()
        {
            return new List<CSVField>
            {
                new CSVField
                {
                    ObjId = 1122017,
                    DisplayName = "Дополнительные маркеры",
                    StrName = "modulecustomlabels" //from old module
                }
            };
        }

        public string PrepareField(CSVField field, int productId, string columnSeparator, string propertySeparator)
        {
            return MarkerService.PrepareCSVField(field, productId);
        }

        public bool ProcessField(CSVField field, int productId, string value, string columnSeparator, string propertySeparator)
        {
            return MarkerService.ProcessCSVField(field, productId, value);
        }

        public List<ModuleSettingTab> AdminSettings
        {
            get
            {
                return new List<ModuleSettingTab>
                {
                    new ModuleSettingTab
                    {
                        Title = "Настройки",
                        Controller = "AmAdmin",
                        Action = "ModuleSettings"
                    },
                    new ModuleSettingTab()
                    {
                        Title = "Обратная связь",
                        Controller = "AMAdmin",
                        Action = "Feedback"
                    }
                };
            }
        }
    }
}
