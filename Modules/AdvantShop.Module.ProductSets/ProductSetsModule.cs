using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using AdvantShop.Catalog;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.ExportImport;
using AdvantShop.Module.ProductSets.Domain;
using AdvantShop.Orders;

namespace AdvantShop.Module.ProductSets
{
    public class ProductSets : IProductAdminControls, IRenderModuleByKey, IModuleBundles, ICSVExportImport, IProductCopy, IAdminProductTabs, IShoppingCart, IDiscount
    {
        #region IModule

        public static string ModuleId
        {
            get { return "ProductSets"; }
        }

        public string ModuleStringId
        {
            get { return ModuleId; }
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Комплекты товаров";

                    case "en":
                        return "Product sets";

                    default:
                        return "Product sets";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new ProductSetsSetting() }; }
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
            return ProductSetsService.InstallProductSetsModule();
        }

        public bool UninstallModule()
        {
            return ProductSetsService.UninstallProductSetsModule();
        }

        public bool UpdateModule()
        {
            return ProductSetsService.UpdateProductSetsModule();
        }

        private class ProductSetsSetting : IModuleControl
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
                get { return "ProductSetsSettings.ascx"; }
            }

            #endregion
        }

        #endregion

        #region IProductAdminControls

        public IList<ProductAdminControl> GetProductAdminControls(TemplateControl page)
        {
            return new List<ProductAdminControl>()
            {
                new ProductAdminControl(ModuleName, string.Format("~/Modules/{0}/AdminProductSets.ascx", ModuleStringId), page)
            };
        }

        #endregion

        #region IRenderModuleByKey

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "product_middle",
                    ActionName = "ProductSet",
                    ControllerName = "ProductSet"
                },
                new ModuleRoute()
                {
                    Key = "fullcartmessage",
                    ActionName = "FullCartMessage",
                    ControllerName = "ProductSet"
                }
            };
        }

        #endregion

        #region IModuleBundles

        public List<string> GetCssBundles()
        {
            return new List<string>() { "~/modules/productsets/styles/productsets.css" };
        }

        public List<string> GetJsBundles()
        {
            return null;
        }

        #endregion

        #region ICSVExportImport

        public IList<CSVField> GetCSVFields()
        {
            return new List<CSVField>
            {
                new CSVField
                {
                    DisplayName = "Комплект товаров",
                    StrName = "productsets"
                },
                new CSVField
                {
                    DisplayName = "Скидка на комплект товаров",
                    StrName = "productsetsdiscount"
                }
            };
        }

        public string PrepareField(CSVField field, int productId, string columnSeparator, string propertySeparator)
        {
            if (field.StrName == "productsets")
                return ProductSetsService.LinkedOffersToString(productId, columnSeparator);
            else if (field.StrName == "productsetsdiscount")
                return ProductSetsService.GetDiscount(productId).ToString("F2");
            return string.Empty;
        }

        public bool ProcessField(CSVField field, int productId, string value, string columnSeparator, string propertySeparator)
        {
            if (field.StrName == "productsets")
                return ProductSetsService.LinkedOffersFromString(productId, value, columnSeparator);
            else if (field.StrName == "productsetsdiscount")
                return ProductSetsService.SetDiscountFromString(productId, value);
            return true;
        }

        #endregion

        #region IProductCopy

        public void AfterCopyProduct(Product sourceProduct, Product newProduct)
        {
            ProductSetsService.CopyProductSets(sourceProduct, newProduct);
        }

        #endregion

        #region IAdminProductTabs

        public IList<AdminProductTabItem> GetAdminProductTabs(int productId)
        {
            return new List<AdminProductTabItem>()
            {
                new AdminProductTabItem("Комплекты товаров", "AdminProductTab", "AdminProductSet")
            };
        }

        #endregion

        #region IShoppingCart

        public void AddToCart(ShoppingCartItem cartItem)
        {
            ProductSetsService.ProcessCart();
        }

        public void RemoveFromCart(ShoppingCartItem cartItem)
        {
            ProductSetsService.ProcessCart();
        }

        public void UpdateCart(ShoppingCartItem cartItem)
        {
            ProductSetsService.ProcessCart();
        }

        public void UpdateCart(ShoppingCart cart)
        {
        }

        public bool ShowConfirmButtons
        {
            get { return true; }
        }

        #endregion

        #region IDiscount

        public float GetDiscount(int productId)
        {
            return 0;
        }

        public List<ProductDiscount> GetProductDiscountsList()
        {
            return new List<ProductDiscount>();
        }

        public Discount GetCartItemDiscount(int cartItemId)
        {
            return ProductSetsService.GetCartItemDiscount(cartItemId);
        }


        #endregion
    }
}
