using AdvantShop.Core.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Core.Modules;
using AdvantShop.Module.OrderNow.Service;
using AdvantShop.ExportImport;

namespace AdvantShop.Module.OrderNow
{
    public class OrderNow : IModule, IRenderModuleByKey, IAdminProductTabs, ICSVExportImport
    {
        public bool HasSettings { get { return true;} }

        public List<IModuleControl> ModuleControls
        { get { return new List<IModuleControl> { new ModuleWrapViewControl() }; } }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    default: return "Закажи сейчас";
                }
            }
        }

        public static string ModuleStringId { get { return "OrderNow"; } }

        string IModule.ModuleStringId { get { return ModuleStringId; } }

        public bool CheckAlive()
        {
            return true;
        }

        public bool InstallModule()
        {
            return ModuleService.Install();
        }

        public bool UninstallModule()
        {
            return ModuleService.UnInstall();
        }

        public bool UpdateModule()
        {
            return ModuleService.Update();
        }

        public List<ModuleRoute> GetModuleRoutes()
        {
            List<ModuleRoute> routes = new List<ModuleRoute>();

            routes.Add(
            new ModuleRoute
            {
                Key = "body_end",
                ControllerName = "ONClient",
                ActionName = "LinkScriptStyle"
            });
            routes.Add(new ModuleRoute
            {
                Key = "admin_body_end",
                ControllerName = "ONAdmin",
                ActionName = "LinkScriptStyle"
            });
            if(Service.ModuleSettings.ShowInMobile)
                routes.Add(
                new ModuleRoute
                {
                    Key = "mobile_body_end",
                    ControllerName = "ONClient",
                    ActionName = "LinkScriptStyle"
                });

            return routes;
        } //IRenderModuleByKey

        public IList<AdminProductTabItem> GetAdminProductTabs(int productId)
        {
            var tabctrl = new AdminProductTabItem("Модуль \"Закажи сейчас\"", "OrderNowTab", "ONAdmin");
            return new List<AdminProductTabItem>() { tabctrl };
        }

        public IList<CSVField> GetCSVFields()
        {
            return new List<CSVField>
            {
                new CSVField
                {
                    DisplayName = "Текст уведомления (Закажи сейчас)",
                    StrName = "onproductmessage"
                },
                new CSVField
                {
                    DisplayName = "Текст уведомления после завершения показа (Закажи сейчас)",
                    StrName = "onproducttimeoutmessage"
                }
            };
        }
    

        public string PrepareField(CSVField field, int productId, string columnSeparator, string propertySeparator)
        {
            if (field.StrName == "onproductmessage")
                return ModuleService.WriteMessage(productId.ToString());
            else if (field.StrName == "onproducttimeoutmessage")
                return ModuleService.WriteTimeoutMessage(productId.ToString());
            return string.Empty;
        }

        public bool ProcessField(CSVField field, int productId, string value, string columnSeparator, string propertySeparator)
        {
            if (field.StrName == "onproductmessage")
                return ModuleService.ProcessMessage(productId.ToString(), value);
            else if (field.StrName == "onproducttimeoutmessage")
                return ModuleService.ProcessTimeoutMessage(productId.ToString(), value);
            return true;
        }

        private class ModuleWrapViewControl : IModuleControl
        {
            public string File { get { return "Admin_ModuleWrapView.ascx"; } }

            public string NameTab { get { return "Закажи сейчас"; } }
        } //IModuleControl

    }
}
