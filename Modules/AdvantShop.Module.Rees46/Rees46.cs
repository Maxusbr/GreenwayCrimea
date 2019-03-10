//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Helpers;
using AdvantShop.Module.Rees46.Domain;
using AdvantShop.Configuration;
using AdvantShop.Diagnostics;

namespace AdvantShop.Module.Rees46
{
    public class Rees46 : IModuleRelatedProducts, ISearch, IRenderModuleByKey
    {
        #region Module methods

        public static string ModuleStringId
        {
            get { return "Rees46"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleStringId; }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new Rees46ModuleSetting() }; }
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
            Rees46Settings.Limit = 8;
            Rees46Settings.RelatedProduct = Recomender.similar.ToString();
            Rees46Settings.AlternativeProduct = Recomender.also_bought.ToString();

            Rees46Settings.MainPageBlock = Recomender.none.ToString();
            Rees46Settings.CatalogTopBlock = Recomender.popular.ToString();
            Rees46Settings.CatalogBottomBlock = Recomender.recently_viewed.ToString();

            Rees46Settings.DisplayInShoppingCart = false;
            Rees46Settings.PathFilePushSW = SettingsMain.SiteUrl.Trim('/') + "/modules/rees46/js/push_sw.js";
            try
            {
                var filename = SettingsGeneral.AbsolutePath.Trim('\\') + "\\manifest.json";
                if (!File.Exists(filename))
                {
                    FileHelpers.CreateFile(filename);
                    var text = "{\n\t\"name\": \"REES46\",\n\t\"gcm_sender_id\": \"605730184710\"\n}";
                    using (var wr = new StreamWriter(filename))
                    {
                        wr.Write(text);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return true;
        }

        public bool UninstallModule()
        {
            FileHelpers.DeleteFile(SettingsMain.SiteUrl.Trim('/') + "/manifest.json");
            return true;
        }
        
        public bool UpdateModule()
        {
            if(Rees46Settings.PathFilePushSW == null)
                Rees46Settings.PathFilePushSW = SettingsMain.SiteUrl.Trim('/') + "/modules/ress46/push_sw.js";
            try
            {
                var filename = SettingsGeneral.AbsolutePath.Trim('\\') + "\\manifest.json";
                if (!File.Exists(filename))
                {
                    FileHelpers.CreateFile(filename);
                    var text = "{\n\t\"name\": \"REES46\",\n\t\"gcm_sender_id\": \"605730184710\"\n}";
                    using (var wr = new StreamWriter(filename))
                    {
                        wr.Write(text);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return true;
        }
        
        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Rees46 - Персональные рекомендации товаров";

                    case "en":
                        return "Rees46";

                    default:
                        return "Rees46";
                }
            }
        }

        private class Rees46ModuleSetting : IModuleControl
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
                get { return "Rees46Module.ascx"; }
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
                    Key = "head",
                    ActionName = "GetScript",
                    ControllerName = "Rees46",
                },
                new ModuleRoute()
                {
                    Key = "mobile_body_end",
                    ActionName = "GetScript",
                    ControllerName = "Rees46",
                },
                new ModuleRoute()
                {
                    Key = "shoppingcart_after",
                    ActionName = "ShoppingcartAfter",
                    ControllerName = "Rees46",
                },
                new ModuleRoute()
                {
                    Key = "order_success",
                    ActionName = "CheckoutFinalStep",
                    ControllerName = "Rees46",
                },
                new ModuleRoute()
                {
                    Key = "product_right",
                    ActionName = "ProductRight",
                    ControllerName = "Rees46",
                },
                new ModuleRoute()
                {
                    Key = "category_top",
                    ActionName = "CategoryTop",
                    ControllerName = "Rees46",
                },
                new ModuleRoute()
                {
                    Key = "category_bottom",
                    ActionName = "CategoryBottom",
                    ControllerName = "Rees46",
                },
                new ModuleRoute()
                {
                    Key = "mainpage_products",
                    ActionName = "MainPage",
                    ControllerName = "Rees46",
                },
            };
        }

        #endregion
        
        #region IModuleRelatedProducts

        public string GetRelatedProductsHtml(Product product, RelatedType relatedType)
        {
            var recom = (Recomender) Enum.Parse(typeof (Recomender),
                                                              relatedType == RelatedType.Related
                                                                    ? Rees46Settings.RelatedProduct
                                                                    : Rees46Settings.AlternativeProduct);
            var offerId = product.Offers.FirstOrDefault();

            return Rees46Service.GetRecomender(recom, (offerId != null ? offerId.OfferId : 0), product.CategoryId, relatedType: relatedType.ToString().ToLower());
        }

        public List<ProductModel> GetRelatedProducts(Product product, RelatedType relatedType)
        {
            return null;
        }

        #endregion
        
        #region ISearch

        public string RenderContent(string term)
        {
            return string.Empty;
        }

        public string RenderBottom(string term)
        {
            return Rees46Service.GetRecomender(Recomender.popular);
        }

        public bool OverrideStandardSearch()
        {
            return false;
        }

        #endregion
    }
}