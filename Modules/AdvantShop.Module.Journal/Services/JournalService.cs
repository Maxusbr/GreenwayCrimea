using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.Core.SQL2;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Module.Journal.Domain;
using SelectPdf;
using SqlPaging = AdvantShop.Core.SQL2.SqlPaging;


namespace AdvantShop.Module.Journal.Services
{
    public class JournalService
    {
        public const int MaxPageCount = 50;

        private static SqlPaging GetPaging(int categoryId, int currentPageIndex = 1, bool indepth = false)
        {
            var paging = new SqlPaging();
            paging.Select(
                "Product.ProductID",
                //"CountPhoto",
                "Photo.PhotoId",
                "PhotoName",
                "Photo.Description".AsSqlField("PhotoDescription"),
                //"Product.ArtNo",
                "Product.Name",
                //"Recomended",
                //"Product.Bestseller",
                //"Product.New",
                //"Product.OnSale".AsSqlField("Sale"),
                "Product.Discount",
                "Product.BriefDescription",
                "Product.MinAmount",
                "Product.MaxAmount",
                "Product.Enabled",
                "Product.AllowPreOrder",
                "Product.Ratio",
                "Product.UrlPath",
                "Product.DateAdded",
                "Offer.OfferID",
                "Offer.ColorID",
                "MaxAvailable".AsSqlField("Amount"),
                "Colors",
                "NotSamePrices".AsSqlField("MultiPrices"),
                "MinPrice".AsSqlField("BasePrice"),
                //"Comments",
                "CurrencyValue");

            paging.From("[Catalog].[Product]");
            paging.Left_Join("[Catalog].[ProductExt] ON [Product].[ProductID] = [ProductExt].[ProductID]");
            paging.Left_Join("[Catalog].[Photo] ON [Photo].[PhotoId] = [ProductExt].[PhotoId]");
            paging.Left_Join("[Catalog].[Offer] ON [ProductExt].[OfferID] = [Offer].[OfferID]");
            paging.Inner_Join("[Catalog].[Currency] ON [Currency].[CurrencyID] = [Product].[CurrencyID]");

            if(!indepth)
                paging.Inner_Join("[Catalog].[ProductCategories] ON [ProductCategories].[CategoryID] = {0} and  ProductCategories.ProductId = [Product].[ProductID]", categoryId);

            paging.Where("Product.Enabled={0}", true);
            //paging.Where("AND CategoryEnabled={0}", true);
            paging.Where("AND Offer.Main={0} AND Offer.Main IS NOT NULL", true);

            if(indepth)
                paging.Where("AND Exists( select 1 from [Catalog].[ProductCategories] INNER JOIN [Settings].[GetChildCategoryByParent]({0}) AS hCat ON hCat.id = [ProductCategories].[CategoryID] and  ProductCategories.ProductId = [Product].[ProductID])", categoryId);

            if (JournalModuleSetting.ShowOnlyAvalible)
            {
                paging.Where("AND MaxAvailable>{0}", 0);
            }

            if (JournalModuleSetting.ShowArtNo)
            {
                paging.SelectFields().Add(new SqlCritera("Product.ArtNo", "", SqlSort.None));
            }

            if (JournalModuleSetting.MoveNotAvaliableToEnd)
            {
                paging.OrderByDesc("(CASE WHEN Price=0 THEN 0 ELSE 1 END)".AsSqlField("TempSort"));
                paging.OrderByDesc("AmountSort".AsSqlField("TempAmountSort"));
            }
            if (!indepth)
                paging.OrderBy("[ProductCategories].[SortOrder]".AsSqlField("ProductCategorySortOrder"));


            paging.ItemsPerPage = JournalModuleSetting.ItemsPerPage;
            paging.CurrentPageIndex = currentPageIndex;

            return paging;
        }

        public static List<ProductModel> GetProducts(int categoryId = 0, int currentPageIndex = 1, bool indepth = false)
        {
            var paging = GetPaging(categoryId, currentPageIndex, indepth);
            var products = paging.PageItemsList<ProductModel>();

            return products;
        }

        public static int GetPagesCount(int categoryId)
        {
            var paging = GetPaging(categoryId, 0);
            var totalCount = paging.TotalRowsCount;
            var totalPages = paging.PageCount(totalCount);

            return totalPages;
        }

        /// <summary>
        /// Export categories to pdf
        /// </summary>
        public static bool ExportToPdf(string filePath, int? pageCountPreview = null)
        {
            var rootCategoryIds = JournalModuleSetting.JournalExport.CategoryIds;
            if (rootCategoryIds.Count == 0)
                return false;

            var categories = new List<Category>();

            foreach (var rootCategoryId in rootCategoryIds)
            {
                var category = CategoryService.GetCategory(rootCategoryId);

                if (category != null && category.Enabled && category.ProductsCount > 0)
                {
                    categories.Add(category);
                }

                GetSubCategories(rootCategoryId, categories);
            }

            var resultDoc = new PdfDocument()
            {
                //JpegCompressionEnabled = false,
                //JpegCompressionLevel = 0,
                CompressionLevel = PdfCompressionLevel.Normal,
                JpegCompressionLevel = 15,
            };

            var converter = new HtmlToPdf();

            // cover
            if (JournalModuleSetting.ShowCover)
            {
                var coverDoc = converter.ConvertUrl(UrlService.GetUrl("journal/cover"));

                //coverDoc.CompressionLevel = PdfCompressionLevel.NoCompression;
                //coverDoc.JpegCompressionEnabled = false;
                coverDoc.JpegCompressionLevel = 15;

                resultDoc.Append(coverDoc);
                coverDoc.Close();
            }

            // pages
            var pageIndex = 0;
            
            foreach (var category in categories)
            {
                if ((pageCountPreview != null && pageCountPreview == pageIndex) || pageIndex >= MaxPageCount)
                    break;
                
                var pagesCount = GetPagesCount(category.CategoryId);
                if (pagesCount == 0)
                    continue;

                for (var i = 0; i < pagesCount; i++)
                {
                    if ((pageCountPreview != null && pageCountPreview == pageIndex) )
                        break;

                    if (pageIndex >= MaxPageCount)
                        break;

                    try
                    {
                        var url =
                            UrlService.GetUrl(
                                string.Format("journal/pdf?page={0}&categoryId={1}&isLeft={2}&pageIndex={3}&categoryName={4}",
                                    (i + 1),
                                    category.CategoryId,
                                    pageIndex%2 != 0,
                                    (pageIndex + 1),
                                    i == 0 ? category.Name : ""));

                        var doc = converter.ConvertUrl(url);

                        //doc.CompressionLevel = PdfCompressionLevel.NoCompression;
                        //doc.JpegCompressionEnabled = false;
                        doc.JpegCompressionLevel = 15;

                        resultDoc.Append(doc);
                        doc.Close();

                        pageIndex++;
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                }
            }

            resultDoc.Save(filePath);
            resultDoc.Close();

            return true;
        }

        private static void GetSubCategories(int categoryId, List<Category> categories)
        {
            foreach (var category in CategoryService.GetChildCategoriesByCategoryId(categoryId, true))
            {
                if (category == null || !category.Enabled || category.ProductsCount == 0)
                    continue;

                if (!category.HasChild)
                {
                    categories.Add(category);
                }

                GetSubCategories(category.CategoryId, categories);
            }
        }

        public static bool CategoryHasProducts(int categoryId)
        {
            return
                SQLDataHelper.GetInt(
                    SQLDataAccess.ExecuteScalar(
                        "Select Count(ProductID) From Catalog.ProductCategories Where CategoryId = @CategoryId",
                        CommandType.Text, 
                        new SqlParameter("@CategoryId", categoryId))) > 0;
        }
    }
}
