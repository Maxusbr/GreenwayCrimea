//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;


namespace AdvantShop.Modules
{
    public class RatingMailRu : IModule, IRenderModuleByKey
    {
        #region Module methods

        public static string ModuleID
        {
            get { return "RatingMailRu"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleID; }
        }

        public string ModuleName
        {
            get { return "Рейтинг@Mail.Ru"; }
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
                get { return "RatingMailRuSettings.ascx"; }
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
                    ActionName = "AfterBodyStart",
                    ControllerName = "RatingMailRu"
                },

                  new ModuleRoute()
                {
                    Key = "mobile_body_start",
                    ActionName = "AfterBodyStart",
                    ControllerName = "RatingMailRu"
                },


                new ModuleRoute()
                {
                    Key = "body_end",
                    ActionName = "BeforeBodyEnd",
                    ControllerName = "RatingMailRu"
                },

                new ModuleRoute()
                {
                    Key = "order_success",
                    ActionName = "CheckoutFinalStep",
                    ControllerName = "RatingMailRu"
                }
            };
        }
    }
}