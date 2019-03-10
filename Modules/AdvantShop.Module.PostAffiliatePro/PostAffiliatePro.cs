//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.PostAffiliatePro
{
    public class PostAffiliatePro : IModule, IRenderModuleByKey
    {
        #region IModule

        public static string ModuleID
        {
            get { return "PostAffiliatePro"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleID; }
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "PostAffiliatePro";

                    case "en":
                        return "PostAffiliatePro";

                    default:
                        return "PostAffiliatePro";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> {new PostAffiliateProSetting()}; }
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
            ModuleSettingsProvider.SetSettingValue("PostAffiliateProLogin", string.Empty, ModuleID);
            ModuleSettingsProvider.SetSettingValue("PostAffiliateProProfile", "default1", ModuleID);
            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("PostAffiliateProLogin", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("PostAffiliateProProfile", ModuleID);
            return true;
        }

        public bool UpdateModule()
        {
            return true;
        }
        
        private class PostAffiliateProSetting : IModuleControl
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
                get { return "PostAffiliateProSettings.ascx"; }
            }

            #endregion
        }

        #endregion

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "body_end",
                    ActionName = "RenderBeforeBodyEnd",
                    ControllerName = "PostAffiliatePro"
                },
                new ModuleRoute()
                {
                    Key = "order_success",
                    ActionName = "OrderFinalStep",
                    ControllerName = "PostAffiliatePro"
                },
            };
        }
    }
}