//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Module.Elbuz.Domain;

namespace AdvantShop.Module.Elbuz
{
    public class Elbuz : IModule
    {
        public static string ModuleID
        {
            get { return "Elbuz"; }
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
                        return "E-Trade";

                    case "en":
                        return "E-Trade";

                    default:
                        return "E-Trade";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>
                    {
                        new ElbuzImport(),
                        new ElbuzPropertiesImport()
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
            return ElbuzRepository.InstallElbuzModule();
        }

        public bool UpdateModule()
        {
            return true;
        }

        public bool UninstallModule()
        {
            return true;
        }

        private class ElbuzImport : IModuleControl
        {
            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Импорт данных из E-Trade PriceList Importer (категории, товары и цены)";

                        case "en":
                            return "Import data from E-trade PriceList Importer (categories, products and prices)";

                        default:
                            return "Import data from E-trade PriceList Importer (categories, products and prices)";
                    }
                }
            }

            public string File
            {
                get { return "ElbuzImport.ascx"; }
            }
        }

        private class ElbuzPropertiesImport : IModuleControl
        {
            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Импорт данных из E-Trade Content Creator (свойства товаров)";

                        case "en":
                            return "Data import from E-Trade Content Creator (products properties)";

                        default:
                            return "Data import from E-Trade Content Creator (products properties)";
                    }
                }
            }

            public string File
            {
                get { return "ElbuzPropertiesImport.ascx"; }
            }
        }
    }
}