//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using AdvantShop.Catalog;
using AdvantShop.Helpers;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Saas;
using AdvantShop.Repository.Currencies;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.SEO;
using AdvantShop.Taxes;

namespace AdvantShop.ExportImport
{
    public class ExportFeedCsvService
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

        public static IEnumerable<ExportFeedProductModel> GetProducts(int exportFeedId, ExportFeedSettings commonSettings, ExportFeedCsvOptions advancedSettings)
        {


            return SQLDataAccess.ExecuteReadIEnumerable(
                   "[Settings].[sp_GetCsvProducts]",
                    CommandType.StoredProcedure,
                    reader => GetCsvProductsFromReader(reader, commonSettings, advancedSettings),
                    new SqlParameter("@exportFeedId", exportFeedId),
                    new SqlParameter("@onlyCount", false),
                    new SqlParameter("@exportNoInCategory", advancedSettings.CsvExportNoInCategory)
                    //new SqlParameter("@exportNotActive", commonSettings.ExportNotActiveProducts),
                    //new SqlParameter("@exportNotAmount", commonSettings.ExportNotAmountProducts)
                    );
        }

        public static IEnumerable<ExportFeedProductModel> GetAllProducts(ExportFeedSettings commonSettings, ExportFeedCsvOptions advancedSettings)
        {
            return SQLDataAccess.ExecuteReadIEnumerable(
                   "[Settings].[sp_GetAllCsvProducts]",
                    CommandType.StoredProcedure,
                    reader => GetCsvProductsFromReader(reader, commonSettings, advancedSettings),                    
                    new SqlParameter("@onlyCount", false));
        }
        

        public static int GetProductsCount(int exportFeedId, ExportFeedSettings commonSettings, ExportFeedCsvOptions advancedSettings)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "[Settings].[sp_GetCsvProducts]",
                CommandType.StoredProcedure,
                new SqlParameter("@exportFeedId", exportFeedId),
                new SqlParameter("@onlyCount", true),
                new SqlParameter("@exportNoInCategory", advancedSettings.CsvExportNoInCategory)
                //new SqlParameter("@exportNotActive", commonSettings.ExportNotActiveProducts),
                //new SqlParameter("@exportNotAmount", commonSettings.ExportNotAmountProducts)
                );
        }

        public static int GetAllCsvProductsCount()
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "[Settings].[sp_GetAllCsvProducts]",
                CommandType.StoredProcedure,
                new SqlParameter("@onlyCount", true));
        }

        public static int GetCategoriesCount(int exportFeedId)
        {
            return SQLDataAccess.ExecuteScalar<int>("[Settings].[sp_GetExportFeedCategories]",
                                                                               CommandType.StoredProcedure,
                                                                               new SqlParameter("@exportFeedId", exportFeedId),
                                                                               new SqlParameter("@onlyCount", true));
        }

        private static ExportFeedCsvProduct GetCsvProductsFromReader(SqlDataReader reader, ExportFeedSettings commonSettings, ExportFeedCsvOptions advancedSettings)
        {
            var product = ProductService.GetProductFromReader(reader);

            var fieldMapping = advancedSettings.FieldMapping;

            var productCsv = new ExportFeedCsvProduct
            {
                ProductId = product.ProductId,
                ArtNo = product.ArtNo,
                Name = product.Name,
                UrlPath = product.UrlPath,
                Enabled = product.Enabled ? "+" : "-",
                Unit = product.Unit,
                ShippingPrice = product.ShippingPrice == null ? "" : product.ShippingPrice.Value.ToString("F2"),
                Discount = product.Discount.Type == DiscountType.Percent ? product.Discount.Percent.ToString("F2") : "",
                DiscountAmount = product.Discount.Type == DiscountType.Amount ? product.Discount.Amount.ToString("F2") : "",
                Weight = product.Weight.ToString("F2"),
                Size = product.Length + "x" + product.Width + "x" + product.Height,
                BriefDescription = product.BriefDescription,
                Description = product.Description,
                OrderByRequest = product.AllowPreOrder ? "+" : "-",
                SalesNote = product.SalesNote,
                Gtin = product.Gtin,
                GoogleProductCategory = product.GoogleProductCategory,
                YandexProductCategory = product.YandexMarketCategory,
                YandexTypePrefix = product.YandexTypePrefix,
                YandexModel = product.YandexModel,
                Adult = product.Adult ? "+" : "-",
                ManufacturerWarranty = product.ManufacturerWarranty ? "+" : "-",
                MinAmount = product.MinAmount == null ? "" : product.MinAmount.Value.ToString("F2"),
                MaxAmount = product.MaxAmount == null ? "" : product.MaxAmount.Value.ToString("F2"),
                Multiplicity = product.Multiplicity.ToString("F5").Trim('0'),
                Fee = product.Fee.ToString(),
                Cbid = product.Cbid.ToString(),
                YandexName = product.YandexName ?? string.Empty
            };

            if (fieldMapping.Contains(ProductFields.Producer))
            {
                productCsv.Producer = BrandService.BrandToString(product.BrandId);
            }

            if (fieldMapping.Contains(ProductFields.Category))
            {
                productCsv.Category = CategoryService.GetCategoryStringByProductId(product.ProductId, advancedSettings.CsvColumSeparator);
            }

            //if (fieldMapping.Contains(ProductFields.Sorting))
            if (advancedSettings.CsvCategorySort)
            {
                productCsv.Sorting = CategoryService.GetCategoryStringByProductId(product.ProductId, advancedSettings.CsvColumSeparator, true);
            }

            if (fieldMapping.Contains(ProductFields.Markers))
            {
                productCsv.Markers = ProductService.MarkersToString(product.BestSeller, product.New,
                    product.Recomended, product.OnSale, advancedSettings.CsvColumSeparator);
            }

            if (fieldMapping.Contains(ProductFields.Photos))
            {
                productCsv.Photos =
                    PhotoService.PhotoToString(
                        PhotoService.GetPhotos<ProductPhoto>(product.ProductId, PhotoType.Product),
                        advancedSettings.CsvColumSeparator, advancedSettings.CsvPropertySeparator);
            }

            if (fieldMapping.Contains(ProductFields.Videos))
            {
                productCsv.Videos =
                    ProductVideoService.VideoToString(
                        ProductVideoService.GetProductVideos(product.ProductId), advancedSettings.CsvColumSeparator);
            }

            if (fieldMapping.Contains(ProductFields.Properties))
            {
                productCsv.Properties =
                    PropertyService.PropertiesToString(
                        PropertyService.GetPropertyValuesByProductId(product.ProductId),
                        advancedSettings.CsvColumSeparator, advancedSettings.CsvPropertySeparator);
            }

            if (fieldMapping.Contains(ProductFields.Related))
            {
                productCsv.Related =
                    ProductService.LinkedProductToString(product.ProductId,
                        RelatedType.Related, advancedSettings.CsvColumSeparator);
            }

            if (fieldMapping.Contains(ProductFields.Alternative))
            {
                productCsv.Alternative =
                    ProductService.LinkedProductToString(product.ProductId,
                        RelatedType.Alternative, advancedSettings.CsvColumSeparator);
            }

            if (fieldMapping.Contains(ProductFields.CustomOption))
            {
                productCsv.CustomOption =
                    CustomOptionsService.CustomOptionsToString(
                        CustomOptionsService.GetCustomOptionsByProductId(product.ProductId));
            }

            if (fieldMapping.Contains(ProductFields.Gifts))
            {
                productCsv.Gifts = OfferService.ProductGiftsToString(product.ProductId,
                    advancedSettings.CsvColumSeparator);
            }

            if (fieldMapping.Contains(ProductFields.Title) ||
                fieldMapping.Contains(ProductFields.H1) ||
                fieldMapping.Contains(ProductFields.MetaKeywords) ||
                fieldMapping.Contains(ProductFields.MetaDescription))
            {
                var meta = MetaInfoService.GetMetaInfo(productCsv.ProductId, MetaType.Product) ??
                           new MetaInfo(0, 0, MetaType.Product, string.Empty, string.Empty, string.Empty, string.Empty);

                productCsv.Title = meta.Title;
                productCsv.H1 = meta.H1;
                productCsv.MetaKeywords = meta.MetaKeywords;
                productCsv.MetaDescription = meta.MetaDescription;
            }


            if (fieldMapping.Contains(ProductFields.Price) ||
                fieldMapping.Contains(ProductFields.PurchasePrice) ||
                fieldMapping.Contains(ProductFields.Amount) ||
                fieldMapping.Contains(ProductFields.MultiOffer))
            {
                if (commonSettings.PriceMargin != 0)
                {
                    foreach (var offer in product.Offers)
                    {
                        offer.BasePrice += offer.BasePrice * commonSettings.PriceMargin / 100;
                        //offer.SupplyPrice += offer.SupplyPrice * commonSettings.PriceMargin / 100;
                    }
                }

                if (!product.Offers.Any(offer => offer.ColorID.HasValue || offer.SizeID.HasValue) && !advancedSettings.AllOffersToMultiOfferColumn)
                {
                    var offer = product.Offers.FirstOrDefault() ?? new Offer();
                    productCsv.Price = offer.BasePrice.ToString("F2");
                    productCsv.PurchasePrice = offer.SupplyPrice.ToString("F2");
                    productCsv.Amount = offer.Amount.ToString(CultureInfo.InvariantCulture);
                    productCsv.MultiOffer = string.Empty;
                }
                else
                {
                    productCsv.Price = string.Empty;
                    productCsv.PurchasePrice = string.Empty;
                    productCsv.Amount = string.Empty;
                    productCsv.MultiOffer = OfferService.OffersToString(product.Offers,
                        advancedSettings.CsvColumSeparator, advancedSettings.CsvPropertySeparator);
                }
            }

            if (fieldMapping.Contains(ProductFields.Currency))
            {
                productCsv.Currency = CurrencyService.GetCurrency(product.CurrencyID).Iso3;
            }

            if (fieldMapping.Contains(ProductFields.Tags) &&
                (!SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveTags))
            {
                var tags = TagService.Gets(product.ProductId, ETagType.Product);
                if (tags != null)
                {
                    productCsv.Tags =
                        tags.Select(item => item.Name).AggregateString(advancedSettings.CsvColumSeparator);
                }
            }

            if (fieldMapping.Contains(ProductFields.BarCode))
            {
                productCsv.BarCode = product.BarCode;
            }

            if (fieldMapping.Contains(ProductFields.Tax))
            {
                if (product.TaxId != null)
                {
                    var tax = TaxService.GetTax(product.TaxId.Value);
                    if (tax != null)
                        productCsv.Tax = tax.Name;
                }
            }

            return productCsv;
        }
    }
}