//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Module.BuyInTime.Domain;

namespace AdvantShop.Module.BuyInTime
{
    public class BuyInTime : IDiscount, ILabel, IRenderModuleByKey, IModuleBundles
    {
        #region IModule

        public static string ModuleStringId
        {
            get { return "BuyInTime"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleStringId; }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new BuyInTimeSetting() }; }
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
            return BuyInTimeService.InstallBuyInTimeModule() && BuyInTimeService.UpdateBuyInTimeModule();
        }

        public bool UninstallModule()
        {
            return BuyInTimeService.UninstallBuyInTimeModule();
        }

        public bool UpdateModule()
        {
            return BuyInTimeService.UpdateBuyInTimeModule();
        }
        
        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Успей купить";

                    case "en":
                        return "BuyInTime";

                    default:
                        return "BuyInTime";
                }
            }
        }

        private class BuyInTimeSetting : IModuleControl
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
                get { return "BuyInTimeModule.ascx"; }
            }

            #endregion
        }

        #endregion

        #region IDiscount

        public float GetDiscount(int productId)
        {
            var now = DateTime.Now;
            var dt = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

            var model = BuyInTimeService.GetByProduct(productId, dt);
            return model != null ? model.Discount : 0;
        }

        public List<ProductDiscount> GetProductDiscountsList()
        {
            return BuyInTimeService.GetProductDiscountsList(DateTime.Now);
        }

        public Discount GetCartItemDiscount(int cartItemId)
        {
            return null;
        }

        #endregion

        #region IRenderModuleByKey

        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "mainpage_block",
                    ActionName = "MainPageBlock",
                    ControllerName = "BuyInTime"
                },
                new ModuleRoute()
                {
                    Key = "mainpage_after_carousel",
                    ActionName = "MainPageAfterCarouselBlock",
                    ControllerName = "BuyInTime"
                },
                new ModuleRoute()
                {
                    Key = "product_info",
                    ActionName = "ProductInformation",
                    ControllerName = "BuyInTime"
                },
                new ModuleRoute()
                {
                    Key = "mobile_after_carousel",
                    ActionName = "MobileAfterCarousel",
                    ControllerName = "BuyInTime"
                }

            };
        }

        #endregion

        #region IModuleBundles

        public List<string> GetCssBundles()
        {
            return new List<string>() { "~/modules/buyintime/styles/styles.css" };
        }

        public List<string> GetJsBundles()
        {
            return null;
        }

        #endregion

        #region ILabel

        public ProductLabel GetLabel()
        {
            var labelCode = ModuleSettingsProvider.GetSettingValue<string>("BuyInTimeLabel", ModuleStringId);
            if (labelCode.IsNullOrEmpty())
                return null;

            var productDiscounts = BuyInTimeService.GetProductDiscountsList(DateTime.Now);
            if (productDiscounts == null || productDiscounts.Count == 0)
                return null;
            
            return new ProductLabel()
            {
                LabelCode = labelCode,
                ProductIds = productDiscounts.Select(p => p.ProductId).ToList()
            };
        }

        public List<ProductLabel> GetLabels()
        {
            return null;
        }

        #endregion
    }
}