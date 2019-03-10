//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using AdvantShop.Helpers;
using AdvantShop.Core.SQL;

namespace AdvantShop.ExportImport
{
    public class ExportFeedGoogleMerchantCenterService
    {
        public static IEnumerable<ExportFeedProductModel> GetProducts(int exportFeedId, ExportFeedSettings commonSettings, ExportFeedGoogleMerchantCenterOptions advancedSettings)
        {
            return
                SQLDataAccess.ExecuteReadIEnumerable<ExportFeedGoogleMerchantCenterProduct>(
                    "[Settings].[sp_GetExportFeedProducts]",
                    CommandType.StoredProcedure,
                    reader => new ExportFeedGoogleMerchantCenterProduct
                    {
                        ProductId = SQLDataHelper.GetInt(reader, "ProductID"),
                        OfferId = SQLDataHelper.GetInt(reader, "OfferId"),
                        ArtNo = SQLDataHelper.GetString(reader, "ArtNo"),
                        OfferArtNo = SQLDataHelper.GetString(reader, "OfferArtNo"),
                        Amount = SQLDataHelper.GetInt(reader, "Amount"),
                        UrlPath = SQLDataHelper.GetString(reader, "UrlPath"),
                        Price = SQLDataHelper.GetFloat(reader, "Price"),
                        ShippingPrice = SQLDataHelper.GetFloat(reader, "ShippingPrice"),
                        Discount = SQLDataHelper.GetFloat(reader, "Discount"),
                        DiscountAmount = SQLDataHelper.GetFloat(reader, "DiscountAmount"),
                        ParentCategory = SQLDataHelper.GetInt(reader, "ParentCategory"),
                        Name = SQLDataHelper.GetString(reader, "Name"),
                        Description = SQLDataHelper.GetString(reader, "Description"),
                        BriefDescription = SQLDataHelper.GetString(reader, "BriefDescription"),
                        Photos = SQLDataHelper.GetString(reader, "Photos"),
                        ColorId = SQLDataHelper.GetInt(reader, "ColorId"),
                        ColorName = SQLDataHelper.GetString(reader, "ColorName"),
                        SizeId = SQLDataHelper.GetInt(reader, "SizeId"),
                        SizeName = SQLDataHelper.GetString(reader, "SizeName"),
                        BrandName = SQLDataHelper.GetString(reader, "BrandName"),
                        Main = SQLDataHelper.GetBoolean(reader, "Main"),
                        GoogleProductCategory = SQLDataHelper.GetString(reader, "GoogleProductCategory"),
                        Gtin = SQLDataHelper.GetString(reader, "Gtin"),
                        Adult = SQLDataHelper.GetBoolean(reader, "Adult"),
                        CurrencyValue = SQLDataHelper.GetFloat(reader, "CurrencyValue"),
                        AllowPreorder = SQLDataHelper.GetBoolean(reader, "AllowPreOrder")
                    },
                    new SqlParameter("@exportFeedId", exportFeedId),
                    new SqlParameter("@selectedCurrency", advancedSettings.Currency),
                    new SqlParameter("@exportNotAvailable", advancedSettings.ExportNotAvailable),
                    new SqlParameter("@allowPreOrder", advancedSettings.AllowPreOrderProducts),
                    //new SqlParameter("@exportNotActive", commonSettings.ExportNotActiveProducts),
                    //new SqlParameter("@exportNotAmount", commonSettings.ExportNotAmountProducts),
                    new SqlParameter("@onlyCount", false));
        }

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

        public static int GetCategoriesCount(int exportFeedId)
        {
            return SQLDataAccess.ExecuteScalar<int>("[Settings].[sp_GetExportFeedCategories]",
                                                                               CommandType.StoredProcedure,
                                                                               new SqlParameter("@exportFeedId", exportFeedId),
                                                                               new SqlParameter("@onlyCount", true));
        }

        public static int GetProductsCount(int exportFeedId, ExportFeedSettings commonSettings, ExportFeedGoogleMerchantCenterOptions advancedSettings)
        {
            return SQLDataAccess.ExecuteScalar<int>("[Settings].[sp_GetExportFeedProducts]",
                CommandType.StoredProcedure,
                new SqlParameter("@exportFeedId", exportFeedId),
                new SqlParameter("@selectedCurrency", advancedSettings.Currency),
                new SqlParameter("@exportNotAvailable", advancedSettings.ExportNotAvailable),
                //new SqlParameter("@exportNotActive", commonSettings.ExportNotActiveProducts),
                //new SqlParameter("@exportNotAmount", commonSettings.ExportNotAmountProducts),
                new SqlParameter("@allowPreOrder", advancedSettings.AllowPreOrderProducts),
                new SqlParameter("@onlyCount", true));
        }
    }
}