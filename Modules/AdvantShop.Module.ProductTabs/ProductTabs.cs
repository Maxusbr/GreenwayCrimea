//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;

using AdvantShop.Core.Controls;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.ExportImport;
using AdvantShop.Module.ProductTabs.Domain;

namespace AdvantShop.Module.ProductTabs
{
    public class ProductTabs : IProductTabs, ICSVExportImport, IProductAdminControls, IAdminProductTabs
    {
        public static string ModuleID
        {
            get { return "ProductTabs"; }
        }
        
        #region Implementation of IModule
        
        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Дополнительные вкладки в карточке товара";

                    case "en":
                        return "Tabs in product details page";

                    default:
                        return "Tabs in product details page";
                }
            }
        }

        public string ModuleStringId
        {
            get { return ModuleID; }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>
                    {
                        new DetailsCommonTabsSettings(),
                        new DetailsProductTabsSettings()
                    };
            }
        }

        public bool HasSettings
        {
            get { return true; }
        }

        public bool CheckAlive()
        {
            return ModulesRepository.IsInstallModule(ModuleID);
        }

        public bool InstallModule()
        {
            return ProductTabsRepository.InstallDetailsCommonTabs();
        }

        public bool UpdateModule()
        {
            return true;
        }

        public bool UninstallModule()
        {
            return true;
        }
        

        private class DetailsCommonTabsSettings : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Управление общими вкладками";

                        case "en":
                            return "Tab manager";

                        default:
                            return "Tab manager";
                    }
                }
            }

            public string File
            {
                get { return "DetailsCommonTabsSettings.ascx"; }
            }

            #endregion
        }

        private class DetailsProductTabsSettings : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Управление вкладками товара";

                        case "en":
                            return "Product tabs manager";

                        default:
                            return "Product tabs manager";
                    }
                }
            }

            public string File
            {
                get { return "DetailsProductTabsSettings.ascx"; }
            }

            #endregion
        }

        #endregion
        
        #region Implementation of IProductTabs

        public List<ITab> GetProductDetailsTabsCollection(int productId)
        {
            return ProductTabsRepository.GetTabsByProductId(productId);
        }

        #endregion

        #region Implementation of ICSVExportImport
        
        public IList<CSVField> GetCSVFields()
        {
            return ProductTabsRepository.GetProductTabTitles()
                .Select(tabTitle => new CSVField
                {
                    DisplayName = tabTitle.Title,
                    ObjId = tabTitle.TabTitleId,
                    StrName = "producttab_" + tabTitle.Title.ToLower()
                }).ToList();
        }

        public string PrepareField(CSVField field, int productId, string columnSeparator, string propertySeparator)
        {
            return ProductTabsRepository.PrepareCSVField(field, productId);
        }

        public bool ProcessField(CSVField field, int productId, string value, string columnSeparator, string propertySeparator)
        {
            return ProductTabsRepository.ProcessCSVField(field, productId, value);
        }

        #endregion

        #region Implementation of IProductAdminControls

        public IList<ProductAdminControl> GetProductAdminControls(TemplateControl page)
        {
            var result = new List<ProductAdminControl>();
            foreach (var tabTitle in ProductTabsRepository.GetProductTabTitles())
            {
                var tabCtrl = new ProductAdminControl(tabTitle.Title, string.Format("~/Modules/{0}/AdminProductTab.ascx", ModuleStringId), page);
                if (tabCtrl.Control != null)
                    ((AdminProductTab)tabCtrl.Control).TabTitle = tabTitle;
                result.Add(tabCtrl);
            }
            return result;
        }

        #endregion

        #region IAdminProductTabs

        public IList<AdminProductTabItem> GetAdminProductTabs(int productId)
        {
            var tabs = ProductTabsRepository.GetProductTabTitles();
            if (tabs.Count == 0)
                return new List<AdminProductTabItem>();

            return new List<AdminProductTabItem>()
            {
                new AdminProductTabItem("Дополнительные вкладки в карточке товара", "AdminProductTab", "ProductTabs")
            };
        }
        
        #endregion
    }
}