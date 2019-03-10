//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.BannerInShoppingCart
{
    public class BannerInShoppingCartModule : IModule, IRenderModuleByKey
    {
        public static string ModuleID
        {
            get { return "BannerInShoppingCartModule"; }
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
                        return "Баннер в корзине";

                    case "en":
                        return "Banner in shopping cart";

                    default:
                        return "Banner in shopping cart";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>
                    {
                        new BannerInShoppingCartSettings()
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
            ModuleSettingsProvider.SetSettingValue("BannerStaticBlock",
                "<div style='float:right'><a href='javascript:void(0);'><img alt='' src='userfiles/image/banner.jpg' style='width: 230px; height: 340px;' /></a></div>",
                ModuleID);

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

        private class BannerInShoppingCartSettings : IModuleControl
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
                get { return "BannerInShoppingCartSettings.ascx"; }
            }
        }

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "cart_right_column",
                    IsSimpleText = true,
                    Content =  "<div class='col-xs-3'><div class='site-body-cell-no-right'>" + ModuleSettingsProvider.GetSettingValue<string>("BannerStaticBlock", ModuleID) + "</div></div>"
                }
            };
        }
    }
}