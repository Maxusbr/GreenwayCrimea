using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.MobileVersion
{
    public class MobileVersion : IRenderModuleByKey, IModule
    {
        #region Module

        public string ModuleStringId
        {
            get { return "MobileVersion"; }
        }

        public static string ModuleID
        {
            get { return "MobileVersion"; }
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Модуль мобильной версии";

                    case "en":
                        return "Module mobile version";

                    default:
                        return "Module mobile version";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new MobileVersionSettings() }; }
        }

        public bool HasSettings
        {
            get { return true; }
        }

        public bool CheckAlive()
        {
            return ModulesRepository.IsInstallModule(ModuleStringId);
        }

        public bool InstallModule()
        {
            return true;
        }

        public bool UpdateModule()
        {
            return true;
        }

        public bool UninstallModule()
        {
            return true;
        }

        private class MobileVersionSettings : IModuleControl
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
                get { return "MobileVersionSettings.ascx"; }
            }
        }

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "body_end",
                    ControllerName = "MobileVersion",
                    ActionName = "MobileOverlap"
                },
                new ModuleRoute()
                {
                    Key = "body_end",
                    ControllerName = "MobileVersion",
                    ActionName = "ToMobileIcon"
                },
                new ModuleRoute()
                {
                    Key = "body_end",
                    IsSimpleText = true,
                    Content = "<link href=\"modules/mobileversion/styles/mobileOverlap.css\" rel=\"stylesheet\">"
                }
            };
        }
        
        #endregion
    }
}
