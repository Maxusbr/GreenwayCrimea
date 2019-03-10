//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Module.RetailRocket.Domain;

namespace AdvantShop.Module.RetailRocket
{
    public class RetailRocket : IRenderModuleByKey, IModuleRelatedProducts
    {
        #region Module methods

        public static string ModuleStringId
        {
            get { return "RetailRocket"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleStringId; }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new RetailRocketModuleSetting() }; }
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
            RRSettings.Limit = 8;
            RRSettings.UseApi = false;
            RRSettings.RelatedProductRecoms = RRSettings.AlternativeProductRecoms = RRSettings.ShoppingCartRecoms = string.Empty;

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
        
        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Retail Rocket";

                    case "en":
                        return "Retail Rocket";

                    default:
                        return "Retail Rocket";
                }
            }
        }

        private class RetailRocketModuleSetting : IModuleControl
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
                get { return "RetailRocketModule.ascx"; }
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
                    Key = "body_start",
                    ActionName = "RrScript",
                    ControllerName = "RetailRocket",
                },
                new ModuleRoute()
                {
                    Key = "mobile_body_start",
                    ActionName = "RrScript",
                    ControllerName = "RetailRocket",
                },
                new ModuleRoute()
                {
                    Key = "mainpage_products_before",
                    ActionName = "MainPageProductsBefore",
                    ControllerName = "RetailRocket"
                },
                new ModuleRoute()
                {
                    Key = "mainpage_products",
                    ActionName = "MainPageProductsAfter",
                    ControllerName = "RetailRocket"
                },
                new ModuleRoute()
                {
                    Key = "category_top",
                    ActionName = "CategoryTop",
                    ControllerName = "RetailRocket"
                },
                new ModuleRoute()
                {
                    Key = "category_bottom",
                    ActionName = "CategoryBottom",
                    ControllerName = "RetailRocket"
                },
                new ModuleRoute()
                {
                    Key = "product_right",
                    ActionName = "ProductRight",
                    ControllerName = "RetailRocket"
                },
                new ModuleRoute()
                {
                    Key = "search_page_top",
                    ActionName = "SearchTop",
                    ControllerName = "RetailRocket"
                },
                new ModuleRoute()
                {
                    Key = "shoppingcart_after",
                    ActionName = "CartBottomScript",
                    ControllerName = "RetailRocket"
                },
                new ModuleRoute()
                {
                    Key = "order_success",
                    ActionName = "OrderFinalStep",
                    ControllerName = "RetailRocket"
                }
            };
        }

        #endregion

        #region IModuleRelatedProducts

        public string GetRelatedProductsHtml(Product product, RelatedType relatedType)
        {
            if (RRSettings.UseApi)
                return string.Empty;

            var offer = product.Offers.OrderByDescending(x => x.Main).FirstOrDefault();
            if (offer != null)
            {
                return relatedType == RelatedType.Related
                    ? RRSettings.RelatedProductRecoms.ToLower().Replace("<products_id>", offer.OfferId.ToString()).Replace("<product_id>", offer.OfferId.ToString()).Replace("<category_id>", product.MainCategory != null ? product.MainCategory.ID.ToString() : "0")
                    : RRSettings.AlternativeProductRecoms.ToLower().Replace("<products_id>", offer.OfferId.ToString()).Replace("<product_id>", offer.OfferId.ToString()).Replace("<category_id>", product.MainCategory != null ? product.MainCategory.ID.ToString() : "0");
            }

            return string.Empty;
        }

        public List<ProductModel> GetRelatedProducts(Product product, RelatedType relatedType)
        {
            if (!RRSettings.UseApi)
                return null;

            var offer = product.Offers.OrderByDescending(x => x.Main).FirstOrDefault();
            if (offer != null)
            {
                return relatedType == RelatedType.Related
                    ? RetailRocketService.GetProductUpSellRecomendations(offer.OfferId)
                    : RetailRocketService.GetProductCrossSellRecomendations(offer.OfferId);
            }

            return null;
        }

        #endregion
    }
}