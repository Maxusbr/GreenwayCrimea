//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using AdvantShop.Helpers;
using AdvantShop.Core.SQL;

namespace AdvantShop.ExportImport
{
    public class ExportFeedYandexService
    {
        public static IEnumerable<ExportFeedCategories> GetCategories(int exportFeedId)
        {
            return SQLDataAccess.ExecuteReadIEnumerable<ExportFeedCategories>("[Settings].[sp_GetExportFeedCategories]",
                CommandType.StoredProcedure,
                reader => new ExportFeedCategories
                {
                    Id = SQLDataHelper.GetInt(reader, "CategoryID"),
                    ParentCategory = SQLDataHelper.GetInt(reader, "ParentCategory"),
                    Name = SQLDataHelper.GetString(reader, "Name")
                },
                new SqlParameter("@exportFeedId", exportFeedId),
                new SqlParameter("@onlyCount", false));
        }

        public static IEnumerable<ExportFeedProductModel> GetProducts(int exportFeedId, ExportFeedSettings settings, ExportFeedYandexOptions advancedSettings)
        {
            return SQLDataAccess.ExecuteReadIEnumerable<ExportFeedYandexProduct>(
                "[Settings].[sp_GetExportFeedProducts]",
                CommandType.StoredProcedure,
                reader => new ExportFeedYandexProduct
                {
                    ProductId = SQLDataHelper.GetInt(reader, "ProductID"),
                    OfferId = SQLDataHelper.GetInt(reader, "OfferId"),
                    ArtNo = SQLDataHelper.GetString(reader, "ArtNo"),
                    OfferArtNo = SQLDataHelper.GetString(reader, "OfferArtNo"),
                    Amount = SQLDataHelper.GetInt(reader, "Amount"),
                    UrlPath = SQLDataHelper.GetString(reader, "UrlPath"),
                    Price = SQLDataHelper.GetFloat(reader, "Price"),
                    ShippingPrice = SQLDataHelper.GetNullableFloat(reader, "ShippingPrice"),
                    Discount = SQLDataHelper.GetFloat(reader, "Discount"),
                    DiscountAmount = SQLDataHelper.GetFloat(reader, "DiscountAmount"),
                    ParentCategory = SQLDataHelper.GetInt(reader, "ParentCategory"),
                    Name = SQLDataHelper.GetString(reader, "Name"),
                    Description = SQLDataHelper.GetString(reader, "Description"),
                    BriefDescription = SQLDataHelper.GetString(reader, "BriefDescription"),
                    Photos = SQLDataHelper.GetString(reader, "Photos"),
                    SalesNote = SQLDataHelper.GetString(reader, "SalesNote"),
                    ColorId = SQLDataHelper.GetInt(reader, "ColorId"),
                    ColorName = SQLDataHelper.GetString(reader, "ColorName"),
                    SizeId = SQLDataHelper.GetInt(reader, "SizeId"),
                    SizeName = SQLDataHelper.GetString(reader, "SizeName"),
                    BrandName = SQLDataHelper.GetString(reader, "BrandName"),
                    Main = SQLDataHelper.GetBoolean(reader, "Main"),
                    YandexMarketCategory = SQLDataHelper.GetString(reader, "YandexMarketCategory"),
                    YandexTypePrefix = SQLDataHelper.GetString(reader, "YandexTypePrefix"),
                    YandexModel = SQLDataHelper.GetString(reader, "YandexModel"),
                    Adult = SQLDataHelper.GetBoolean(reader, "Adult"),
                    CurrencyValue = SQLDataHelper.GetFloat(reader, "CurrencyValue"),
                    ManufacturerWarranty = SQLDataHelper.GetBoolean(reader, "ManufacturerWarranty"),
                    YandexName = SQLDataHelper.GetString(reader, "YandexName"),

                    Enabled = SQLDataHelper.GetBoolean(reader, "Enabled"),
                    Weight = SQLDataHelper.GetFloat(reader, "Weight"),
                    SupplyPrice = SQLDataHelper.GetFloat(reader, "SupplyPrice"),
                    BrandCountry = SQLDataHelper.GetString(reader, "BrandCountry"),
                    BarCode = SQLDataHelper.GetString(reader, "BarCode"),
                    Fee = SQLDataHelper.GetFloat(reader, "Fee"),
                    Cbid = SQLDataHelper.GetFloat(reader, "Cbid"),
                    YandexSizeUnit = SQLDataHelper.GetString(reader, "YandexSizeUnit"),
                    AllowPreorder = SQLDataHelper.GetBoolean(reader, "AllowPreorder"),
                    Multiplicity = SQLDataHelper.GetFloat(reader, "Multiplicity"),
                    MinAmount = SQLDataHelper.GetFloat(reader, "MinAmount"),
                    // Cpa = SQLDataHelper.GetBoolean(reader, "Cpa")
                },
                new SqlParameter("@exportFeedId", exportFeedId),
                new SqlParameter("@selectedCurrency", advancedSettings.Currency),
                new SqlParameter("@exportNotAvailable", advancedSettings.ExportNotAvailable),
                //new SqlParameter("@exportNotActive", settings.ExportNotActiveProducts),
                //new SqlParameter("@exportNotAmount", settings.ExportNotAmountProducts),
                new SqlParameter("@allowPreOrder", advancedSettings.AllowPreOrderProducts),
                new SqlParameter("@onlyCount", false));
        }


        public static int GetProductsCount(int exportFeedId, ExportFeedSettings commonSettings, ExportFeedYandexOptions advancedSettings)
        {
            return SQLDataAccess.ExecuteScalar<int>("[Settings].[sp_GetExportFeedProducts]",
                CommandType.StoredProcedure,
                new SqlParameter("@exportFeedId", exportFeedId),
                new SqlParameter("@selectedCurrency", advancedSettings.Currency),
                new SqlParameter("@exportNotAvailable", advancedSettings.ExportNotAvailable),
                //new SqlParameter("@exportNotActive", settings.ExportNotActiveProducts),
                //new SqlParameter("@exportNotAmount", settings.ExportNotAmountProducts),
                new SqlParameter("@allowPreOrder", advancedSettings.AllowPreOrderProducts),
                new SqlParameter("@onlyCount", true));

        }

        public static int GetCategoriesCount(int exportFeedId)
        {
            return SQLDataAccess.ExecuteScalar<int>("[Settings].[sp_GetExportFeedCategories]",
                CommandType.StoredProcedure,
                new SqlParameter("@exportFeedId", exportFeedId),
                new SqlParameter("@onlyCount", true));
        }
    }
}