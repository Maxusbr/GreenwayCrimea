//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Module.Callback.Services;

namespace AdvantShop.Module.Callback
{
    public class Callback :  IModule, IRenderModuleByKey
    {
        public static string ModuleStringId
        {
            get { return "Callback"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleStringId; }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new CallbackSetting(), new ViewCallbacks() }; }
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
            return CallbackRepository.InstallCallbackModule();
        }

        public bool UninstallModule()
        {
            return CallbackRepository.UninstallCallbackModule();
        }

        public bool UpdateModule()
        {
            return CallbackRepository.UpdateCallbackModule();
        }
        
        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Обратный звонок";

                    case "en":
                        return "Callback";

                    default:
                        return "Callback";
                }
            }
        }

        private class CallbackSetting : IModuleControl
        {
            #region Implementation of IModuleControl

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
                get { return "CallbackModule.ascx"; }
            }

            #endregion
        }

        private class ViewCallbacks : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Список заявок";

                        case "en":
                            return "List of applications";

                        default:
                            return "List of applications";
                    }
                }
            }

            public string File
            {
                get { return "ViewCallbacks.ascx"; }
            }

            #endregion
        }

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "body_end",
                    ActionName = "GetCallback",
                    ControllerName = "Callback"
                },
                new ModuleRoute()
                {
                    Key = "mobile_body_end",
                    ActionName = "GetCallback",
                    ControllerName = "Callback"
                }
            };
        }
    }
}