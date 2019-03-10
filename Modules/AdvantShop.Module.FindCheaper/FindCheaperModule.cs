//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.FindCheaper
{
    public class FindCheaperModule : IModule, IRenderModuleByKey
    {
        public static string ModuleStringId
        {
            get { return "FindCheaperModule"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleStringId; }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new FindCheaperSettings(), new FindCheaperManager() }; }
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
            return FindCheaperService.InstalModule();
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
                        return "Нашли дешевле?";

                    case "en":
                        return "Found Cheaper?";

                    default:
                        return "Found Cheaper?";
                }
            }
        }

        private class FindCheaperSettings : IModuleControl
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
                get { return "FindCheaperSettings.ascx"; }
            }

            #endregion
        }

        private class FindCheaperManager : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Менеджер запросов";

                        case "en":
                            return "Request manager";

                        default:
                            return "Request manager";
                    }
                }
            }

            public string File
            {
                get { return "FindCheaperManager.ascx"; }
            }

            #endregion
        }


        public List<ModuleRoute> GetModuleRoutes()
        {

            var inner =
                string.Format(
                    "<a href=\"\" data-find-cheaper data-product-name=\"product.productName\" data-is-show-user-agreement-text='{3}' data-user-agreement-text='{4}' data-product-offer-id=\"product.offerSelected.OfferId\" data-product-price=\"product.Price.PriceNumber\" data-modal-title=\"{0}\" data-modal-top-text=\"{1}\" data-modal-final-text=\"{2}\" data-ng-if=\"product.offerSelected != null && product.offerSelected.OfferId != null\">Нашли дешевле?</a>",
                    ModuleSettingsProvider.GetSettingValue<string>("Title", ModuleStringId),
                    HttpUtility.HtmlAttributeEncode(ModuleSettingsProvider.GetSettingValue<string>("TopText", ModuleStringId)),
                    HttpUtility.HtmlAttributeEncode(ModuleSettingsProvider.GetSettingValue<string>("FinalText", ModuleStringId)),
                    SettingsCheckout.IsShowUserAgreementText.ToString().ToLower(), SettingsCheckout.UserAgreementText);

            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "product_right",
                    IsSimpleText = true,
                    Content = string.Format("<div data-oc-lazy-load=\"['modules/FindCheaperModule/scripts/findCheaper.js']\">{0}</div>", inner)
                }
            };
        }
    }
}