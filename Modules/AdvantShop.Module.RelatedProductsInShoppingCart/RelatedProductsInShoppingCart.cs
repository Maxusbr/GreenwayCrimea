//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.RelatedProductsInShoppingCart
{
    public class RelatedProductsInShoppingCart : IModule, IRenderModuleByKey
    {
        #region IModule

        public static string ModuleID
        {
            get { return "RelatedProductsInShoppingCart"; }
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
                        return "Рекомендованные товары в корзине";

                    case "en":
                        return "Related products in Shopping cart";

                    default:
                        return "Related products in Shopping cart";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>
                    {
                        new RelatedProductsInSCSettings()
                    };
            }
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

        public bool UpdateModule()
        {
            return true;
        }

        public bool UninstallModule()
        {
            return true;
        }

        private class RelatedProductsInSCSettings : IModuleControl
        {
            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Настройки модуля";

                        case "en":
                            return "Настройки модуля";

                        default:
                            return "Настройки модуля";
                    }
                }
            }

            public string File
            {
                get { return "RelatedProductsInSCSettings.ascx"; }
            }
        }

        #endregion

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "shoppingcart_after",
                    ActionName = "RelatedProducts",
                    ControllerName = "RelatedProductsInShoppingCart"
                }
            };
        }
    }
}