//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Module.BuyMore.Domain;
using AdvantShop.Orders;
using AdvantShop.Shipping;

namespace AdvantShop.Module.BuyMore
{
    public class BuyMore : IRenderModuleByKey, IShippingCalculator, IShoppingCart, IModuleBundles
    {
        #region IModule

        public static string ModuleStringId
        {
            get { return "BuyMore"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleStringId; }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new BuyMoreSetting() }; }
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
            return BuyMoreService.InstallBuyMoreModule();
        }

        public bool UninstallModule()
        {
            return BuyMoreService.UninstallBuyMoreModule();
        }
        

        public bool UpdateModule()
        {
            return BuyMoreService.UpdateBuyMoreModule();
        }
        
        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Купи еще";

                    case "en":
                        return "Buy More";

                    default:
                        return "Buy More";
                }
            }
        }

        private class BuyMoreSetting : IModuleControl
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
                get { return "BuyMoreModule.ascx"; }
            }

            #endregion
        }

        #endregion
        
        #region IRenderModuleByKey

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "minicartmessage",
                    ActionName = "minicartmessage",
                    ControllerName = "BuyMore"
                },
                new ModuleRoute()
                {
                    Key = "fullcartmessage",
                    ActionName = "fullcartmessage",
                    ControllerName = "BuyMore"
                }

            };
        }

        #endregion

        #region IShippingCalculator

        public void ProcessOptions(List<BaseShippingOption> options, List<PreOrderItem> cart)
        {
            BuyMoreService.ProcessOptions(options, cart);
        }

        #endregion

        #region IShoppingCart

        public void AddToCart(ShoppingCartItem cartItem)
        {
            BuyMoreService.AddOrRemoveCartItem();
        }

        public void RemoveFromCart(ShoppingCartItem cartItem)
        {
            BuyMoreService.AddOrRemoveCartItem();
        }

        public void UpdateCart(ShoppingCartItem cartItem)
        {
            BuyMoreService.AddOrRemoveCartItem();
        }

        public bool ShowConfirmButtons { get { return true; } }

        public void UpdateCart(ShoppingCart cart)
        {
            BuyMoreService.AddOrRemoveCart(cart);
        }

        #endregion

        #region IModuleBundles

        public List<string> GetCssBundles()
        {
            return new List<string>() { "~/modules/buymore/styles/modulebuymore.css" };
        }

        public List<string> GetJsBundles()
        {
            return null;
        }
        
        #endregion
    }
}