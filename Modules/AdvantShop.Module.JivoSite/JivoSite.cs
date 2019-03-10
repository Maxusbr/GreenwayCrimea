//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Module.JivoSite.Domain;

namespace AdvantShop.Module.JivoSite
{

    public class JivoSite : IModule, IRenderModuleByKey
    {
        #region IModule

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "JivoSite";

                    case "en":
                        return "JivoChat";

                    default:
                        return "JivoSite";
                }
            }
        }

        public string ModuleStringId
        {
            get { return ModuleId; }
        }
        
        public static string ModuleId {
            get { return "JivoSite"; }
        }


        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> {new JivoSiteSetting()}; }
        }

        public bool HasSettings
        {
            get { return true; }
        }

        public bool CheckAlive()
        {
            return true;
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

        private class JivoSiteSetting : IModuleControl
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
                get { return "JivoSiteModule.ascx"; }
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
                    IsSimpleText = true,
                    Content = JivoService.GetWidgetCode()
                },
                new ModuleRoute()
                {
                    Key = "mobile_body_end",
                    IsSimpleText = true,
                    Content = JivoService.GetWidgetCode()
                }
            };
        }
    }
}