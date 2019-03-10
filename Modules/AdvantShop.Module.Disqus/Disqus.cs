//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------


using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Module.Disqus.Domain;

namespace AdvantShop.Modules
{
    public class Disqus : IModuleReviews, IRenderModuleByKey
    {
        #region Module methods

        public static string ModuleID
        {
            get { return "Disqus"; }
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
                        return "Комментарии Disqus";

                    case "en":
                        return "Disqus comments";

                    default:
                        return "Disqus comments";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new DisqusSetting() }; }
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
            ModuleSettingsProvider.SetSettingValue("ShortName", string.Empty, ModuleID);
            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("ShortName", ModuleID);
            return true;
        }

        public bool UpdateModule()
        {
            return true;
        }


        private class DisqusSetting : IModuleControl
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
                get { return "DisqusModule.ascx"; }
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
                    Key = "product_reviews",
                    IsSimpleText = true,
                    Content = DisqusService.GetContainer()
                },
                new ModuleRoute()
                {
                    Key = "body_end",
                    IsSimpleText = true,
                    Content = DisqusService.GetScript()
                },
                new ModuleRoute()
                {
                    Key = "mobile_body_end",
                    IsSimpleText = true,
                    Content = DisqusService.GetScript()
                }
            };
        }

        public string GetReviewsCount(string url)
        {
            return string.Format("<span class=\"disqus-comment-count\" data-disqus-url=\"{0}\"></span>", url);
        }
    }
}