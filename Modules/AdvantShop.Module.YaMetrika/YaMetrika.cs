//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.YaMetrika
{
    public class YaMetrika : IModule, IRenderModuleByKey
    {
        #region Module methods

        public static string ModuleID
        {
            get { return "YaMetrika"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleID; }
        }

        public string ModuleName
        {
            get { return "Яндекс.Метрика"; }
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
            ModuleSettingsProvider.SetSettingValue("COUNTER_ID", string.Empty, ModuleID);
            ModuleSettingsProvider.SetSettingValue("COUNTER", string.Empty, ModuleID);

            if (!ModuleSettingsProvider.IsSqlSettingExist("OldApiEnabled", ModuleID))
            {
                ModuleSettingsProvider.SetSettingValue("OldApiEnabled", true, ModuleID);
            }

            if (!ModuleSettingsProvider.IsSqlSettingExist("EcomerceApiEnabled", ModuleID))
            {
                ModuleSettingsProvider.SetSettingValue("EcomerceApiEnabled", false, ModuleID);
            }

            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("COUNTER_ID", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("COUNTER", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("OldApiEnabled", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("EcomerceApiEnabled", ModuleID);
            return true;
        }

        public bool UpdateModule()
        {
            if (!ModuleSettingsProvider.IsSqlSettingExist("OldApiEnabled", ModuleID))
            {
                ModuleSettingsProvider.SetSettingValue("OldApiEnabled", true, ModuleID);
            }

            if (!ModuleSettingsProvider.IsSqlSettingExist("EcomerceApiEnabled", ModuleID))
            {
                ModuleSettingsProvider.SetSettingValue("EcomerceApiEnabled", false, ModuleID);
            }

            return true;
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> {new YaMetrikaSetting()}; }
        }

        private class YaMetrikaSetting : IModuleControl
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
                get { return "YaMetrikaSettings.ascx"; }
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
                    Key = "body_start",
                    ActionName = "YaMetrikaScript",
                    ControllerName = "YaMetrika"
                },
                new ModuleRoute()
                {
                    Key = "body_start",
                    ActionName = "EcommerceDataLayer",
                    ControllerName = "YaMetrika"
                },
                new ModuleRoute()
                {
                    Key = "product_page_before",
                    ActionName = "EcommercePropductDetail",
                    ControllerName = "YaMetrika"
                },
                new ModuleRoute()
                {
                    Key = "order_success",
                    ActionName = "CheckoutFinalStep",
                    ControllerName = "YaMetrika"
                },
                new ModuleRoute()
                {
                    Key = "order_success",
                    ActionName = "EcommerceCheckoutFinalStep",
                    ControllerName = "YaMetrika"
                },
                new ModuleRoute()
                {
                    Key = "mobile_body_start",
                    ActionName = "YaMetrikaScript",
                    ControllerName = "YaMetrika"
                },
                new ModuleRoute()
                {
                    Key = "mobile_body_start",
                    ActionName = "EcommerceDataLayer",
                    ControllerName = "YaMetrika"
                },
            };
        }
    }
}