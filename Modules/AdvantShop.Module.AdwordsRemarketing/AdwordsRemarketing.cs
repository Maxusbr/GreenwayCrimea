using System.Collections.Generic;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using System.Globalization;

namespace AdvantShop.Modules
{
    public class AdwordsRemarketing : IModule, IRenderModuleByKey
    {

        public static string ModuleID
        {
            get { return "AdwordsRemarketing"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleID; }
        }

        public string ModuleName
        {
            get { return "AdWords Remarketing"; }
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
            return true;
        }

        public bool UninstallModule()
        {
            return true;
        }

        public bool UpdateModule()
        {
            return true;
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new AdwordsRemarketingSetting() }; }
        }


        private class AdwordsRemarketingSetting : IModuleControl
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
                get { return "AdwordsRemarketingSettings.ascx"; }
            }


        }


        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "body_start",
                    ActionName = "AfterBodyStart",
                    ControllerName = "AdwordsRemarketing"
                },

                new ModuleRoute()
                {
                    Key = "mobile_body_start",
                    ActionName = "AfterBodyStart",
                    ControllerName = "AdwordsRemarketing"
                },

                new ModuleRoute()
                {
                    Key = "order_success",
                    ActionName = "CheckoutFinalStep",
                    ControllerName = "AdwordsRemarketing"
                }
            };
        }
    }
}
