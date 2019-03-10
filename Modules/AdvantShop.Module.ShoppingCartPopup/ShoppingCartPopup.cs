//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------


using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.ShoppingCartPopup
{
    public class ShoppingCartPopup : IModule, IRenderModuleByKey
    {

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "body_end",
                    IsSimpleText = true,
                    Content = "<link href=\"modules/shoppingcartpopup/styles/cartpopup.css\" rel=\"stylesheet\">"
                },
                new ModuleRoute()
                {
                    Key = "body_end",
                    ControllerName ="ShoppingCartPopup",
                    ActionName = "BodyBottomScript"
                }
            };
        }

        #region Module methods

        public static string ModuleID
        {
            get { return "ShoppingCartPopup"; }
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
                        return "Всплывающая корзина";

                    case "en":
                        return "ShoppingCartPopup";

                    default:
                        return "ShoppingCartPopup";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new ShoppingCartPopupSetting() }; }
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
            ModuleSettingsProvider.SetSettingValue("showmode", "related", ModuleID);
            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("showmode", ModuleID);
            return true;
        }

        public bool UpdateModule()
        {
            return true;
        }

        private class ShoppingCartPopupSetting : IModuleControl
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
                get { return "ShoppingCartPopupModule.ascx"; }
            }

            #endregion
        }

        #endregion
    }
}