//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//-------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Module.Blog.Domain;
using AdvantShop.SEO;
using AdvantShop.Core.UrlRewriter;

namespace AdvantShop.Module.Blog
{
    public class Blog : IModule, ISiteMap
    {
        #region Module

        public string ModuleStringId
        {
            get { return "Blog"; }
        }

        public static string ModuleID
        {
            get { return "Blog"; }
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Модуль блог.новости.статьи";

                    case "en":
                        return "Module Blog.Article.News";

                    default:
                        return "Module Blog.Article.News";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new BlogListItems(), new BlogListCategories(), new BlogSettings() }; }
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
            return BlogService.InstallBlogModule();
        }

        public bool UpdateModule()
        {
            return BlogService.UpdateBlogModule();
        }

        public bool UninstallModule()
        {
            return true;
        }

        public List<SiteMapData> GetData()
        {
            var result = new List<SiteMapData>();
            foreach (var blogItem in BlogService.GetListBlogItem(true))
            {
                result.Add(new SiteMapData()
                {
                    Title = blogItem.Title,
                    Loc = Configuration.SettingsMain.SiteUrl.Trim('/') + "/blog/" + blogItem.UrlPath,
                    Lastmod = blogItem.AddingDate
                });
            }
            return result;
        }

        private class BlogSettings : IModuleControl
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
                get { return "BlogSettings.ascx"; }
            }
        }

        private class BlogListItems : IModuleControl
        {
            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Управление записями";

                        case "en":
                            return "Blog records management";

                        default:
                            return "Blog records management";
                    }
                }
            }

            public string File
            {
                get { return "BlogListItems.ascx"; }
            }
        }

        private class BlogListCategories : IModuleControl
        {
            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Управление категориями";

                        case "en":
                            return "Blog categories management";

                        default:
                            return "Blog categories management";
                    }
                }
            }

            public string File
            {
                get { return "BlogListCategories.ascx"; }
            }
        }

        #endregion

    }
}