//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Globalization;
using AdvantShop.Modules.Interfaces;

namespace AdvantShop.Modules
{
    public class Ritmz : IModule
    {
        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Ritmz";

                    case "en":
                        return "Ritmz";

                    default:
                        return "Ritmz";
                }
            }
        }

        public static string ModuleID
        {
            get { return "Ritmz"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleID; }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>
                {
                    new RitmzSettings()
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
            if (!ModulesRepository.IsExistsModuleProcedure("RitmzGetExportProducts"))
            {
                ModulesRepository.ModuleExecuteNonQuery(@"                   
                    CREATE PROCEDURE [Module].[RitmzGetExportProducts]
	                    @selectedCurrency nvarchar(10)
                    AS
                    BEGIN
                    DECLARE @defaultCurrencyRatio float;
                    SELECT @defaultCurrencyRatio = CurrencyValue FROM [Catalog].[Currency] WHERE CurrencyIso3 = @selectedCurrency;
  
                    SELECT [Product].[Enabled], [Product].[ProductID], [Product].[Discount], [Product].[Weight], AllowPreOrder,Amount, [ProductCategories].[CategoryId] AS [ParentCategory],
                    ([Offer].[Price] / @defaultCurrencyRatio) AS Price, [Product].[Name],[Product].[UrlPath], [Product].[Description], 
                    [Product].[BriefDescription], [Product].SalesNote, OfferId, ([Offer].SupplyPrice / @defaultCurrencyRatio) as SupplyPrice,[Offer].ArtNo,[Offer].Main,[Offer].ColorID, ColorName, [Offer].SizeID,SizeName,BrandName,
                    [Settings].PhotoToString(Offer.ColorID, Product.ProductId) as Photos
                    from  [Catalog].[Product]
                    inner JOIN [Catalog].[Offer] ON [Offer].[ProductID] = [Product].[ProductID]
                    INNER JOIN [Catalog].[ProductCategories] on [ProductCategories].[ProductID] = [Product].[ProductID]    
                    Left join [Catalog].[Color] on [Color].ColorID = [Offer].ColorID
                    Left join [Catalog].[Size] on [Size].SizeID = [Offer].SizeID
                    left join [Catalog].Brand on Brand.BrandID = [Product].BrandID
                    where 
                    (SELECT TOP(1) [ProductCategories].[CategoryId] FROM [Catalog].[ProductCategories]
                        INNER JOIN [Catalog].[Category] on [Category].[CategoryId] = [ProductCategories].[CategoryId]
                        WHERE [ProductID] = [Product].[ProductID] AND [Enabled] = 1 AND [Main] = 1) = [ProductCategories].[CategoryId]
                    and Offer.Price > 0 and (Offer.Amount > 0 or Product.AllowPreOrder=1) and CategoryEnabled=1
                    END", CommandType.Text);
            }

            return ModulesRepository.IsExistsModuleProcedure("RitmzGetExportProducts");
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.SetSettingValue("RitmzLogin", string.Empty, ModuleID);
            ModuleSettingsProvider.SetSettingValue("RitmzPassword", string.Empty, ModuleID);
            return true;
        }

        public bool UpdateModule()
        {
            return true;
        }

        private class RitmzSettings : IModuleControl
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
                get { return "RitmzSettings.ascx"; }
            }
        }
    }
}