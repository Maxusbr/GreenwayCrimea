//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Module.StoreReviews.Domain;
using AdvantShop.SEO;
using AdvantShop.Core.UrlRewriter;
using System.IO;
using System.Web.Hosting;
using AdvantShop.Diagnostics;

namespace AdvantShop.Module.StoreReviews
{
    public class StoreReviews : IModule, IModuleBundles, ISiteMap
    {
        public const string ImagePath = "~/pictures/modules/storereviews/";
        public const string ImagePathRelative = "pictures/modules/storereviews/";

        public static string ModuleID
        {
            get { return "StoreReviews"; }
        }

        #region IModule

        public string ModuleStringId
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
                        return "Отзывы о магазине";

                    case "en":
                        return "Store reviews";

                    default:
                        return "Store reviews";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>
                    {
                        new StoreReviewsManager(),
                        new StoreReviewsEmailSettings(),
                        new StoreReviewsSettings()
                    };
            }
        }

        public bool HasSettings
        {
            get { return true; }
        }

        public bool CheckAlive()
        {
            return StoreReviewRepository.IsAliveStoreReviewsModule() && ModulesRepository.IsInstallModule(ModuleStringId);
        }

        public bool InstallModule()
        {
            return StoreReviewRepository.InstallStoreReviewsModule();
        }

        public bool UninstallModule()
        {
            return StoreReviewRepository.UninstallStoreReviewsModule();
        }

        public bool UpdateModule()
        {
            var from = HostingEnvironment.MapPath("~/Modules/" + StoreReviews.ModuleID + "/pictures/");
            var to = HostingEnvironment.MapPath(ImagePath);
            DirectoryCopy(from, to, true);
            return true;
        }


        private class StoreReviewsSettings : IModuleControl
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
                get { return "StoreReviewsSettings.ascx"; }
            }

            #endregion
        }

        private class StoreReviewsEmailSettings : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Настройка оповещений";

                        case "en":
                            return "Email settings ";

                        default:
                            return "Email settings";
                    }
                }
            }

            public string File
            {
                get { return "StoreReviewsEmailSettings.ascx"; }
            }

            #endregion
        }

        private class StoreReviewsManager : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Управление отзывами";

                        case "en":
                            return "Reviews manager";

                        default:
                            return "Reviews manager";
                    }
                }
            }

            public string File
            {
                get { return "StoreReviewsManager.ascx"; }
            }

            #endregion
        }

        #endregion

        #region IModuleBundles

        public List<string> GetCssBundles()
        {
            return new List<string>() {"~/modules/storereviews/styles/storereviews.css"};
        }

        public List<string> GetJsBundles()
        {
            return null;
        }

        #endregion

        #region ISiteMap

        public List<SiteMapData> GetData()
        {
            return new List<SiteMapData>()
            {
                new SiteMapData
                {
                    Title = "Страница отзывов",
                    Loc = Configuration.SettingsMain.SiteUrl.Trim('/') + "/storereviews",
                    Changefreq = null,
                    Lastmod = DateTime.Now
                }
            };
        }

        #endregion

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            // If the source directory does not exist, exit.
            if (!dir.Exists)
            {
                if (!Directory.Exists(destDirName))
                    Directory.CreateDirectory(destDirName);

                return;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
                       

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);


            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                try
                {
                    // Create the path to the new copy of the file.
                    var temppath = Path.Combine(destDirName, file.Name);

                    if (!File.Exists(temppath))
                        file.CopyTo(temppath, false);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {

                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}