//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------


using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.BingImagesSearch
{
    public class BingImagesSearchModule : IPhotoSearcher, IAdminBundles
    {
        #region IModule

        public static string ModuleID
        {
            get { return "BingImagesSearchModule"; }
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
                        return "Поиск фотографий для товара(Bing)";

                    case "en":
                        return "Search for products photos(Bing)";

                    default:
                        return "Search for products photos(Bing)";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new BingImagesSearchSetting() }; }
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

        private class BingImagesSearchSetting : IModuleControl
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
                get { return "BingImagesSearchModule.ascx"; }
            }

            #endregion
        }

        #endregion


        #region IAdminBundles

        public List<string> AdminCssBottom()
        {
            return new List<string>()
            {
                "~/modules/bingImagesSearchModule/styles/bing-images-search.css"
            };
        }

        public List<string> AdminJsBottom()
        {
            return new List<string>()
            {
                "~/modules/bingImagesSearchModule/scripts/admin/components/bingImageSearch/bingImageSearch.js",
                "~/modules/bingImagesSearchModule/scripts/admin/modals/modalSearchImages/modalSearchImages.js"
            };
        }

        #endregion
    }
}