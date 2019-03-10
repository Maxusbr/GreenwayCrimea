//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Orders;
using AdvantShop.Core.Services.Configuration.Settings;

namespace AdvantShop.Module.OrderConfirmationInShoppingCart
{
    public class OrderConfirmationInShoppingCart : IShoppingCart, IRenderModuleByKey, IModule
    {
        #region IModule

        public static string ModuleID
        {
            get { return "OrderConfirmationInShoppingCart"; } // "CheckoutInShoppingCart"
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
                        return "Быстрый заказ из корзины";

                    case "en":
                        return "Quick order from cart";

                    default:
                        return "Quick order from cart";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> {new CheckoutInShoppingCartSettings()}; }
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

        private class CheckoutInShoppingCartSettings : IModuleControl
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
                            return "Module settings";

                        default:
                            return "Настройки модуля";
                    }
                }
            }

            public string File
            {
                get { return "OCInShoppingCartSettings.ascx"; }
            }
        }

        #endregion

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "shoppingcart", 
                    ActionName = "Checkout", 
                    ControllerName = "OrderConfirmationInShoppingCart"
                }
            };
        }

        #region IShoppingCart

        public void AddToCart(ShoppingCartItem cartItem)
        {
            return;
        }

        public void RemoveFromCart(ShoppingCartItem cartItem)
        {
            return;
        }

        public void UpdateCart(ShoppingCartItem cartItem)
        {
            return;
        }

        public bool ShowConfirmButtons
        {
            get { return SettingsMobile.IsMobileTemplateActive && AdvantShop.Helpers.MobileHelper.IsMobileEnabled(); }
        }

        public void UpdateCart(ShoppingCart cartItem)
        {
            return;
        }

        #endregion
    }
}