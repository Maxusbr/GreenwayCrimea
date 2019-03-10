using AdvantShop.Core.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Module.CategoriesOnMainPage.Service;

namespace AdvantShop.Module.CategoriesOnMainPage
{
    public class CategoriesOnMainPage : IModule, IRenderModuleByKey
    {
        public bool HasSettings
        {
            get { return true; }
        }
        
        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new CategoriesList() }; }
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru": return "Категории на главной";
                    case "en": return "Categories on main page";
                    default: return "Categories on main page";
                }
            }
        }

        public static string ModuleStringId
        {
            get { return "CategoriesOnMainPage"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleStringId; }
        }

        public bool CheckAlive()
        {
            return ModulesRepository.IsInstallModule(ModuleStringId);
        }

        public bool InstallModule()
        {
            return COMPService.Install();
        }

        public bool UninstallModule()
        {
            return COMPService.UnInstall();
        }

        public bool UpdateModule()
        {
            return COMPService.Update();
        }

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>
            {
                new ModuleRoute
                {
                    Key = "categories_on_main_page",
                    IsSimpleText = true,
                    Content = "<div id=\"categoriesOnMainPageRM\"></div>"
                },
                new ModuleRoute
                {
                    Key = "body_end",
                    ControllerName = "COMPClient",
                    ActionName = "LinkScriptStyle"
                },
                new ModuleRoute
                {
                    Key = "mainpage_block",
                    ControllerName = "COMPClient",
                    ActionName = "ShowCategories"
                }
            };
        }

        private class CategoriesList : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Список категорий";

                        case "en":
                            return "Categories List";

                        default:
                            return "Categories List";
                    }
                }
            }

            public string File
            {
                get { return "CategoriesList.ascx"; }
            }

            #endregion
        }
    }
}
