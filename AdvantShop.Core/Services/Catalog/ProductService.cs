//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Core.Services.FullSearch;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Saas;
using AdvantShop.SEO;

namespace AdvantShop.Catalog
{
    public class ProductService
    {
        #region Categories

        public static void SetProductHierarchicallyEnabled(int productId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[SetProductHierarchicallyEnabled]", CommandType.StoredProcedure,
                                            new SqlParameter("@ProductId", productId));
        }

        /// <summary>
        /// get first categoryId by productId(сделал инклуд индекс)
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static int GetFirstCategoryIdByProductId(int productId)
        {
            int categoryId = SQLDataHelper.GetInt(SQLDataAccess.ExecuteScalar("[Catalog].[sp_GetCategoryIDByProductID]", CommandType.StoredProcedure, new SqlParameter("@ProductID", productId)), CategoryService.DefaultNonCategoryId);
            return categoryId;
        }

        /// <summary>
        /// get categoryIds by productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetCategoriesIDsByProductId(int productId)
        {
            return SQLDataAccess.ExecuteReadColumnIEnumerable<int>("[Catalog].[sp_GetCategoriesIDsByProductId]",
                                                                   CommandType.StoredProcedure,
                                                                   "CategoryID",
                                                                   new SqlParameter("@ProductID", productId));
        }

        /// <summary>
        /// get categories by productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<Category> GetCategoriesByProductId(int productId)
        {
            return SQLDataAccess.ExecuteReadList("[Catalog].[sp_GetProductCategories]", CommandType.StoredProcedure,
                CategoryService.GetCategoryFromReader, new SqlParameter("@ProductID", productId));
        }

        /// <summary>
        /// create html for tooltip
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static string CreateTooltipContent(int productId)
        {
            var res = new StringBuilder();
            var content = new StringBuilder();
            int categoryCounter = 0;
            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText = "[Catalog].[sp_GetCategoriesPathesByProductID]";
                    db.cmd.CommandType = CommandType.StoredProcedure;
                    db.cmd.Parameters.Clear();
                    db.cmd.Parameters.AddWithValue("@ProductID", productId);

                    db.cnOpen();
                    using (SqlDataReader reader = db.cmd.ExecuteReader())

                        while (reader.Read())
                        {
                            content.Append("<br/>&nbsp;&nbsp;&nbsp;" + HttpUtility.HtmlEncode(SQLDataHelper.GetString(reader, "CategoryPath")));
                            categoryCounter++;
                        }

                    db.cnClose();
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            if (categoryCounter > 0)
            {
                var strHead = new StringBuilder();
                strHead.Append("<div class=\'tooltipDiv\'><span class=\'tooltipBold\'>");
                strHead.Append(string.Format(LocalizationService.GetResource("Core.Catalog.Product.ProductInCategories"), categoryCounter));
                strHead.Append("<br/><div style=\'height:5px;width:0px;\' />");
                strHead.Append(LocalizationService.GetResource("Core.Catalog.Product.Categories"));

                res.Append(strHead);
                res.Append(content);
                res.Append("</span></div>");
            }
            else
            {
                res.Append("");
            }

            return res.ToString();
        }

        #endregion

        #region Related Products

        public static string LinkedProductToString(int productId, RelatedType related, string columSeparator)
        {
            var temp = SQLDataAccess.ExecuteReadList("Select ArtNo from Catalog.Product inner join Catalog.RelatedProducts on " +
                                                    "RelatedProducts.LinkedProductID=Product.ProductId where RelatedType=@type and RelatedProducts.ProductID=@productId",
                                                    CommandType.Text,
                                                    reader => SQLDataHelper.GetString(reader, "ArtNo"),
                                                    new SqlParameter("@productId", productId),
                                                    new SqlParameter("@type", (int)related));
            return temp.AggregateString(columSeparator);
        }

        public static void LinkedProductFromString(int productId, string linkproducts, RelatedType type, string columSeparator)
        {
            if (string.IsNullOrWhiteSpace(columSeparator))
                _LinkedProductFromString(productId, linkproducts, type);
            else
                _LinkedProductFromString(productId, linkproducts, type, columSeparator);
        }

        private static void _LinkedProductFromString(int productId, string linkproducts, RelatedType type)
        {
            ClearRelatedProducts(productId, type);

            if (!string.IsNullOrEmpty(linkproducts))
            {
                var arrArt = linkproducts.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string t in arrArt)
                {
                    var artNo = t.Trim();
                    if (string.IsNullOrWhiteSpace(artNo)) continue;
                    int linkProductId = GetProductId(artNo);
                    if (linkProductId != 0)
                        AddRelatedProduct(productId, linkProductId, type);
                }
            }
        }

        private static void _LinkedProductFromString(int productId, string linkproducts, RelatedType type, string columSeparator)
        {
            ClearRelatedProducts(productId, type);

            if (string.IsNullOrEmpty(linkproducts)) return;
            var arrArt = linkproducts.Split(columSeparator);
            foreach (string t in arrArt)
            {
                var artNo = t.Trim();
                if (string.IsNullOrWhiteSpace(artNo)) continue;
                int linkProductId = GetProductId(artNo);
                if (linkProductId != 0)
                    AddRelatedProduct(productId, linkProductId, type);
            }
        }

        public static bool IsExistRelatedProduct(int productId, int relatedProductId, RelatedType relatedType)
        {
            //[Catalog].[RelatedProducts]
            return SQLDataAccess.ExecuteScalar<int>("Select 1 from [Catalog].[RelatedProducts] where [ProductID]=@ProductID and [LinkedProductID]=@RelatedProductID and [RelatedType]=@RelatedType",
                                                  CommandType.Text,
                                                  new SqlParameter("@ProductID", productId),
                                                  new SqlParameter("@RelatedProductID", relatedProductId),
                                                  new SqlParameter("@RelatedType", (int)relatedType)) > 0;

        }

        /// <summary>
        /// Add related product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="relatedProductId"></param>
        /// <param name="relatedType"></param>
        public static void AddRelatedProduct(int productId, int relatedProductId, RelatedType relatedType)
        {
            if (!IsExistRelatedProduct(productId, relatedProductId, relatedType))
                SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_AddRelatedProduct]", CommandType.StoredProcedure,
                                                    new SqlParameter("@ProductID", productId),
                                                    new SqlParameter("@RelatedProductID", relatedProductId),
                                                    new SqlParameter("@RelatedType", (int)relatedType));
        }

        /// <summary>
        /// delete ralated product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="relatedProductId"></param>
        /// <param name="relatedType"></param>
        public static void DeleteRelatedProduct(int productId, int relatedProductId, RelatedType relatedType)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DeleteRelatedProduct]", CommandType.StoredProcedure,
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@RelatedProductID", relatedProductId),
                                            new SqlParameter("@RelatedType", (int)relatedType));
        }

        public static void DeleteRelatedProducts(int productId, RelatedType relatedType)
        {
            SQLDataAccess.ExecuteNonQuery("Delete from catalog.RelatedProducts Where ProductId=@ProductID Or LinkedProductID=@ProductID and RelatedType=@RelatedType",
                                            CommandType.Text,
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@RelatedType", (int)relatedType));
        }

        public static void ClearRelatedProducts(int productId, RelatedType relatedType)
        {
            SQLDataAccess.ExecuteNonQuery("Delete from catalog.RelatedProducts Where ProductId=@ProductID and RelatedType=@RelatedType",
                                            CommandType.Text,
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@RelatedType", (int)relatedType));
        }

        public static List<ProductModel> GetRelatedProducts(int productId, RelatedType relatedType)
        {
            return SQLDataAccess.Query<ProductModel>(
                "Select Product.ProductID, Product.ArtNo, Product.Name, Product.BriefDescription, Product.Recomended, Product.Bestseller, Product.New, Product.OnSale as Sale, " +
                "Product.Enabled, Product.UrlPath, Product.AllowPreOrder, Product.Ratio, Product.Discount, Product.DiscountAmount, Product.MinAmount, Product.MaxAmount, Product.DateAdded, " +
                "Offer.OfferID, Offer.ColorID, MaxAvailable AS Amount, MinPrice as BasePrice, Colors, CurrencyValue, " +
                "CountPhoto, Photo.PhotoId, PhotoName, Photo.Description as PhotoDescription, NotSamePrices as MultiPrices, Gifts, Product.BarCode " +
                "From [Catalog].[Product] " +
                "Left Join [Catalog].[ProductExt]  On [Product].[ProductID] = [ProductExt].[ProductID]  " +
                "Inner Join [Catalog].[RelatedProducts] ON [Product].[ProductID] = [RelatedProducts].[LinkedProductID] " +
                "Inner Join [Catalog].[Currency] On [Currency].[CurrencyID] = [Product].[CurrencyID] " +
                "Left Join [Catalog].[Photo] On [Photo].[PhotoId] = [ProductExt].[PhotoId] " +
                "Left Join [Catalog].[Offer] On [ProductExt].[OfferID] = [Offer].[OfferID] " +
                "Where [RelatedProducts].[ProductID] = @ProductID and RelatedProducts.RelatedType = @RelatedType and Product.Enabled=1 and CategoryEnabled=1 and Product.ProductID<> @ProductID " +
                "Order by (CASE WHEN Price=0 OR AmountSort=0 THEN 0 ELSE 1 END) DESC, AllowPreOrder DESC",
                new
                {
                    ProductId = productId,
                    RelatedType = (int)relatedType,
                    Type = PhotoType.Product.ToString(),
                })
                .ToList();
        }

        public static List<ProductModel> GetAllRelatedProducts(int productId, RelatedType relatedType)
        {
            return SQLDataAccess.Query<ProductModel>(
                "Select Product.ProductID, Product.ArtNo, Product.Name, Product.UrlPath, Photo.PhotoId, PhotoName, Photo.Description as PhotoDescription " +
                "From [Catalog].[Product] " +
                "Left Join [Catalog].[ProductExt]  On [Product].[ProductID] = [ProductExt].[ProductID]  " +
                "Inner Join [Catalog].[RelatedProducts] ON [Product].[ProductID] = [RelatedProducts].[LinkedProductID] " +
                "Inner Join [Catalog].[Currency] On [Currency].[CurrencyID] = [Product].[CurrencyID] " +
                "Left Join [Catalog].[Photo] On [Photo].[PhotoId] = [ProductExt].[PhotoId] " +
                "Left Join [Catalog].[Offer] On [ProductExt].[OfferID] = [Offer].[OfferID] " +
                "Where [RelatedProducts].[ProductID] = @ProductID and RelatedProducts.RelatedType = @RelatedType and Product.ProductID<> @ProductID " +
                "Order by AllowPreOrder DESC, AmountSort DESC, (CASE WHEN Price=0 THEN 0 ELSE 1 END) DESC",
                new
                {
                    ProductId = productId,
                    RelatedType = (int)relatedType,
                    Type = PhotoType.Product.ToString(),
                })
                .ToList();
        }

        public static List<ProductModel> GetRelatedProductsFromCategory(Product product, RelatedType relatedType)
        {
            var categoryIds = CategoryService.GetRelatedCategoryIds(product.CategoryId, relatedType);
            if (categoryIds.Count <= 0)
                return null;

            var propValues = new List<string>();

            // Достаем значения свойств как у данного товара
            var propertyIds = CategoryService.GetRelatedPropertyIds(product.CategoryId, relatedType);
            if (propertyIds != null && propertyIds.Count > 0)
            {
                var productPropertyValues =
                    product.ProductPropertyValues.Where(p => propertyIds.Contains(p.PropertyId)).ToList();

                var list = new List<int>();

                foreach (var productPropertyValue in productPropertyValues)
                {
                    var ids =
                        productPropertyValues.Where(
                            x =>
                                x.PropertyId == productPropertyValue.PropertyId &&
                                !list.Contains(productPropertyValue.PropertyValueId))
                            .Select(x => x.PropertyValueId)
                            .ToList();

                    if (ids.Any())
                    {
                        propValues.Add(String.Join(",", ids));
                        list.AddRange(ids);
                    }
                }
            }

            // Достаем значения свойств, которые выбрали
            var propertyValuesIds =
                CategoryService.GetRelatedPropertyValuesIds(product.CategoryId, relatedType)
                    .Select(x => x.ToString())
                    .ToList();

            if (propertyValuesIds.Count > 0)
            {
                propValues = propValues.Union(propertyValuesIds).ToList();
            }

            if (propValues.Count > 0)
                return GetRelatedProductsByCategoryAndProperties(categoryIds, propValues, relatedType, SettingsCatalog.RelatedProductsMaxCount);

            return GetRelatedProductsByCategory(categoryIds, relatedType, SettingsCatalog.RelatedProductsMaxCount);
        }


        private static List<ProductModel> GetRelatedProductsByCategory(List<int> categoryIds, RelatedType relatedType, int count)
        {
            return SQLDataAccess.Query<ProductModel>(
                "Select Product.ProductID, Product.ArtNo, Product.Name, Product.BriefDescription, Product.Recomended, Product.Bestseller, Product.New, Product.OnSale as Sale, " +
                "Product.Enabled, Product.UrlPath, Product.AllowPreOrder, Product.Ratio, Product.Discount, Product.DiscountAmount, Product.MinAmount, Product.MaxAmount, Product.DateAdded, " +
                "Offer.OfferID, Offer.ColorID, MaxAvailable AS Amount, MinPrice as BasePrice, Colors, CurrencyValue, " +
                "CountPhoto, Photo.PhotoId, PhotoName, Photo.Description as PhotoDescription, NotSamePrices as MultiPrices, Gifts, Product.BarCode " +
                "From [Catalog].[Product] " +
                "Left Join [Catalog].[ProductExt]  On [Product].[ProductID] = [ProductExt].[ProductID]  " +
                "Inner Join [Catalog].[Currency] On [Currency].[CurrencyID] = [Product].[CurrencyID] " +
                "Left Join [Catalog].[Photo] On [Photo].[PhotoId] = [ProductExt].[PhotoId] " +
                "Left Join [Catalog].[Offer] On [ProductExt].[OfferID] = [Offer].[OfferID] " +
                "Where Product.ProductId in (Select top(@Count)ProductID From ( " +
                        "Select distinct Product.ProductID From Catalog.ProductCategories " +
                        "Left Join catalog.Product On Product.ProductId = ProductCategories.ProductId " +
                        "Where CategoryID in (" + string.Join(",", categoryIds) + ") and Product.Enabled=1 and CategoryEnabled=1) as p order by newid()) " +
                "Order by AllowPreOrder DESC, AmountSort DESC, (CASE WHEN Price=0 THEN 0 ELSE 1 END) DESC, ProductId",
                new
                {
                    Count = count,
                    RelatedType = (int)relatedType,
                    Type = PhotoType.Product.ToString(),
                })
                .ToList();
        }

        private static List<ProductModel> GetRelatedProductsByCategoryAndProperties(List<int> categoryIds, List<string> propValues, RelatedType relatedType, int count)
        {
            var categories = String.Join(",", categoryIds);
            var subQuery =
                "Select top(@Count)ProductID From (" +
                    "Select distinct Product.ProductID From Catalog.ProductCategories " +
                    "Left Join catalog.Product On Product.ProductId = ProductCategories.ProductId " +
                    "Where CategoryID in (" + categoryIds + ") and Product.Enabled=1 and CategoryEnabled=1) as p order by newid()";

            if (propValues.Count > 0)
            {
                var queryProductIds = propValues.Aggregate("",
                    (current, propValue) =>
                        current +
                        ((current != "" ? " Intersect " : "") +
                         "Select distinct ProductCategories.ProductID " +
                         "From Catalog.ProductCategories " +
                         "Left Join catalog.Product On Product.ProductId = ProductCategories.ProductId " +
                         "Left Join Catalog.ProductPropertyValue On ProductPropertyValue.ProductId=ProductCategories.ProductId " +
                         "Where CategoryID In (" + categories + ") and PropertyValueId In (" + propValue + ") and Product.Enabled=1 and CategoryEnabled=1 "));

                subQuery = "Select top(@Count)ProductID From (" + queryProductIds + ") as p order by newid()";
            }

            return SQLDataAccess.Query<ProductModel>(
                "Select Product.ProductID, Product.ArtNo, Product.Name, Product.BriefDescription, Product.Recomended, Product.Bestseller, Product.New, Product.OnSale as Sale, " +
                "Product.Enabled, Product.UrlPath, Product.AllowPreOrder, Product.Ratio, Product.Discount, Product.DiscountAmount, Product.MinAmount, Product.MaxAmount, Product.DateAdded, " +
                "Offer.OfferID, Offer.ColorID, MaxAvailable AS Amount, MinPrice as BasePrice, Colors, CurrencyValue, " +
                "CountPhoto, Photo.PhotoId, PhotoName, Photo.Description as PhotoDescription, NotSamePrices as MultiPrices, Gifts, Product.BarCode " +
                "From [Catalog].[Product] " +
                "Left Join [Catalog].[ProductExt] On [Product].[ProductID] = [ProductExt].[ProductID]  " +
                "Inner Join [Catalog].[Currency] On [Currency].[CurrencyID] = [Product].[CurrencyID] " +
                "Left Join [Catalog].[Photo] On [Photo].[PhotoId] = [ProductExt].[PhotoId] " +
                "Left Join [Catalog].[Offer] On [ProductExt].[OfferID] = [Offer].[OfferID] " +
                "Where Product.ProductId in (" + subQuery + ") and Product.Enabled=1 and CategoryEnabled=1 " +
                "Order by AllowPreOrder DESC, AmountSort DESC, (CASE WHEN Price=0 THEN 0 ELSE 1 END) DESC, ProductId",
                new
                {
                    Count = count,
                    RelatedType = (int)relatedType,
                    Type = PhotoType.Product.ToString()
                })
                .ToList();
        }

        public static List<ProductModel> GetProductsByIds(List<int> productIds)
        {
            return SQLDataAccess.Query<ProductModel>(
                "Select Product.ProductID, Product.ArtNo, Product.Name, Product.BriefDescription, Product.Recomended, Product.Bestseller, Product.New, Product.OnSale as Sale, " +
                "Product.Enabled, Product.UrlPath, Product.AllowPreOrder, Product.Ratio, Product.Discount, Product.DiscountAmount, Product.MinAmount, Product.MaxAmount, Product.DateAdded, Offer.OfferID, " +
                "Offer.ColorID, MaxAvailable AS Amount, MinPrice as BasePrice, Colors, CurrencyValue, " +
                "CountPhoto, Photo.PhotoId, PhotoName, Photo.Description as PhotoDescription, NotSamePrices as MultiPrices, Gifts, Product.BarCode " +
                "From [Catalog].[Product] " +
                "Inner Join (Select item, sort From [Settings].[ParsingBySeperator](@productIds, ',')) pt On pt.item = [Product].[ProductId] " +
                "Left Join [Catalog].[ProductExt] On [Product].[ProductID] = [ProductExt].[ProductID]  " +
                "Inner Join [Catalog].[Currency] On [Currency].[CurrencyID] = [Product].[CurrencyID] " +
                "Left Join [Catalog].[Photo] On [Photo].[PhotoId] = [ProductExt].[PhotoId] " +
                "Left Join [Catalog].[Offer] On [ProductExt].[OfferID] = [Offer].[OfferID] " +
                "Where Product.Enabled=1 and CategoryEnabled=1 ORDER BY pt.sort",
                new
                {
                    Type = PhotoType.Product.ToString(),
                    productIds = string.Join(",", productIds)
                })
                .ToList();
        }

        #endregion


        #region Get Add Update Delete

        /// <summary>
        /// delete product by productId
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="sentToLuceneIndex"></param>
        /// <returns></returns>
        public static bool DeleteProduct(int productId, bool sentToLuceneIndex)
        {
            PhotoService.DeletePhotos(productId, PhotoType.Product);
            DeleteRelatedProducts(productId, RelatedType.Related);
            DeleteRelatedProducts(productId, RelatedType.Alternative);

            if (Settings1C.Enabled && IsExists(productId))
            {
                SQLDataAccess.ExecuteNonQuery(
                    "Insert Into [Catalog].[DeletedProducts] (ProductId,ArtNo,DateTime) Values (@ProductId, (Select top 1 ArtNo From Catalog.Product Where ProductId=@ProductId), GetDate())",
                    CommandType.Text, new SqlParameter("@ProductID", productId));
            }

            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DeleteProduct]", CommandType.StoredProcedure, new SqlParameter("@ProductID", productId));

            CategoryService.ClearCategoryCache();
            if (sentToLuceneIndex)
            {
                ProductWriter.Delete(productId);
            }
            return true;
        }

        /// <summary>
        /// add product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="updateIndexes"></param>
        /// <returns></returns>
        public static int AddProduct(Product product, bool updateIndexes)
        {
            if (SaasDataService.IsSaasEnabled && GetProductsCount("[Enabled] = 1") >= SaasDataService.CurrentSaasData.ProductsCount)
            {
                return 0;
            }

            product.ProductId = SQLDataHelper.GetInt(SQLDataAccess.ExecuteScalar("[Catalog].[sp_AddProduct]",
                CommandType.StoredProcedure,
                new SqlParameter("@ArtNo", product.ArtNo != null ? product.ArtNo.Trim() : ""),
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@Ratio", product.Ratio),
                new SqlParameter("@Discount", product.Discount.Percent),
                new SqlParameter("@DiscountAmount", product.Discount.Amount),
                new SqlParameter("@Weight", product.Weight),
                new SqlParameter("@BriefDescription", product.BriefDescription ?? (object)DBNull.Value),
                new SqlParameter("@Description", product.Description ?? (object)DBNull.Value),
                new SqlParameter("@Enabled", product.Enabled),
                new SqlParameter("@Recomended", product.Recomended),
                new SqlParameter("@New", product.New),
                new SqlParameter("@BestSeller", product.BestSeller),
                new SqlParameter("@OnSale", product.OnSale),
                new SqlParameter("@AllowPreOrder", product.AllowPreOrder),
                new SqlParameter("@BrandID", product.BrandId != 0 ? product.BrandId : (object)DBNull.Value),
                new SqlParameter("@UrlPath", product.UrlPath),
                new SqlParameter("@Unit", product.Unit ?? (object)DBNull.Value),
                new SqlParameter("@ShippingPrice", product.ShippingPrice ?? (object)DBNull.Value),
                new SqlParameter("@MinAmount", product.MinAmount ?? (object)DBNull.Value),
                new SqlParameter("@MaxAmount", product.MaxAmount ?? (object)DBNull.Value),
                new SqlParameter("@Multiplicity", product.Multiplicity),
                new SqlParameter("@SalesNote", product.SalesNote ?? (object)DBNull.Value),
                new SqlParameter("@HasMultiOffer", product.HasMultiOffer),
                new SqlParameter("@GoogleProductCategory", product.GoogleProductCategory ?? (object)DBNull.Value),
                new SqlParameter("@YandexMarketCategory", product.YandexMarketCategory ?? (object)DBNull.Value),
                new SqlParameter("@Gtin", product.Gtin ?? (object)DBNull.Value),
                new SqlParameter("@Adult", product.Adult),
                new SqlParameter("@Length", product.Length),
                new SqlParameter("@Width", product.Width),
                new SqlParameter("@Height", product.Height),
                new SqlParameter("@CurrencyID", product.CurrencyID),
                new SqlParameter("@ActiveView360", product.ActiveView360),
                new SqlParameter("@ManufacturerWarranty", product.ManufacturerWarranty),
                new SqlParameter("@ModifiedBy", product.ModifiedBy),
                new SqlParameter("@YandexTypePrefix", product.YandexTypePrefix ?? (object)DBNull.Value),
                new SqlParameter("@YandexModel", product.YandexModel ?? (object)DBNull.Value),
                new SqlParameter("@BarCode", product.BarCode ?? (object)DBNull.Value),
                new SqlParameter("@Cbid", product.Cbid),
                new SqlParameter("@Fee", product.Fee),
                new SqlParameter("@AccrueBonuses", product.AccrueBonuses),
                new SqlParameter("@TaxId", product.TaxId ?? (object)DBNull.Value),
                new SqlParameter("@YandexSizeUnit", string.IsNullOrEmpty(product.YandexSizeUnit) ? (object)DBNull.Value : product.YandexSizeUnit),
                new SqlParameter("@YandexName", string.IsNullOrWhiteSpace(product.YandexName) ? string.Empty : product.YandexName)
                ));

            if (product.ProductId == 0)
                return 0;
            //by default in bd set ID if artNo is Null
            if (string.IsNullOrEmpty(product.ArtNo))
            {
                product.ArtNo = GetProductArtNoByProductID(product.ProductId);
            }


            // ---- Offers
            if (product.Offers != null && product.Offers.Count != 0)
            {
                foreach (var offer in product.Offers)
                {
                    if (offer.ArtNo.IsNullOrEmpty())
                        offer.ArtNo = product.ArtNo.Trim();

                    offer.ProductId = product.ProductId;
                    OfferService.AddOffer(offer);
                }
            }
            // ---- Meta
            if (product.Meta != null)
            {
                if (!product.Meta.Title.IsNullOrEmpty() || !product.Meta.MetaKeywords.IsNullOrEmpty() || !product.Meta.MetaDescription.IsNullOrEmpty() || !product.Meta.H1.IsNullOrEmpty())
                {
                    product.Meta.ObjId = product.ProductId;
                    MetaInfoService.SetMeta(product.Meta);
                }
            }
            //tag
            var tags = product.Tags;
            if (tags != null)
            {
                for (var i = 0; i < tags.Count; i++)
                {
                    var tag = TagService.Get(tags[i].Name);
                    tags[i].Id = tag == null ? TagService.Add(tags[i]) : tag.Id;
                    TagService.AddMap(product.ProductId, tags[i].Id, ETagType.Product, i * 10);
                }
            }

            if (Settings1C.Enabled)
            {
                SQLDataAccess.ExecuteNonQuery("Delete From Catalog.DeletedProducts Where ArtNo = @ArtNo",
                    CommandType.Text, new SqlParameter("@ArtNo", product.ArtNo));
            }

            if (updateIndexes)
            {
                SetProductHierarchicallyEnabled(product.ProductId);
                ProductWriter.AddUpdate(product);
                PreCalcProductParams(product.ProductId);
            }

            return product.ProductId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        public static string GetProductArtNoByProductID(int productId)
        {
            return SQLDataAccess.ExecuteScalar<string>("SELECT [ArtNo] FROM [Catalog].[Product] WHERE [ProductID] = @ProductID",
                                                                   CommandType.Text, new SqlParameter("@ProductID", productId));
        }

        public static void UpdateProductByArtNo(Product product, bool sentToLuceneIndex)
        {
            product.ProductId = SQLDataAccess.ExecuteScalar<int>("SELECT [ProductID] FROM [Catalog].[Product] WHERE [ArtNo] = @ArtNo",
                                                                   CommandType.Text, new SqlParameter("@ArtNo", product.ArtNo));
            if (product.ProductId > 0)
                UpdateProduct(product, sentToLuceneIndex);
        }

        public static void UpdateProduct(Product product, bool updateIndexes)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateProductById]", 
                CommandType.StoredProcedure,
                new SqlParameter("@ArtNo", product.ArtNo != null ? product.ArtNo.Trim() : string.Empty),
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@ProductID", product.ProductId),
                new SqlParameter("@Ratio", product.Ratio),
                new SqlParameter("@Discount", product.Discount.Percent),
                new SqlParameter("@DiscountAmount", product.Discount.Amount),
                new SqlParameter("@Weight", product.Weight),
                new SqlParameter("@BriefDescription", product.BriefDescription ?? (object) DBNull.Value),
                new SqlParameter("@Description", product.Description ?? (object) DBNull.Value),
                new SqlParameter("@Enabled", product.Enabled),
                new SqlParameter("@AllowPreOrder", product.AllowPreOrder),
                new SqlParameter("@Recomended", product.Recomended),
                new SqlParameter("@New", product.New),
                new SqlParameter("@BestSeller", product.BestSeller),
                new SqlParameter("@OnSale", product.OnSale),
                new SqlParameter("@BrandID", product.BrandId != 0 ? product.BrandId : (object) DBNull.Value),
                new SqlParameter("@UrlPath", product.UrlPath),
                new SqlParameter("@Unit", product.Unit ?? (object) DBNull.Value),
                new SqlParameter("@ShippingPrice", product.ShippingPrice ?? (object) DBNull.Value),
                new SqlParameter("@MinAmount", product.MinAmount ?? (object) DBNull.Value),
                new SqlParameter("@MaxAmount", product.MaxAmount ?? (object) DBNull.Value),
                new SqlParameter("@Multiplicity", product.Multiplicity),
                new SqlParameter("@SalesNote", product.SalesNote ?? (object) DBNull.Value),
                new SqlParameter("@HasMultiOffer", product.HasMultiOffer),
                new SqlParameter("@GoogleProductCategory", product.GoogleProductCategory ?? (object) DBNull.Value),
                new SqlParameter("@YandexMarketCategory", product.YandexMarketCategory ?? (object) DBNull.Value),
                new SqlParameter("@Gtin", product.Gtin ?? (object) DBNull.Value),
                new SqlParameter("@Adult", product.Adult),
                new SqlParameter("@Length", product.Length),
                new SqlParameter("@Width", product.Width),
                new SqlParameter("@Height", product.Height),
                new SqlParameter("@CurrencyID", product.CurrencyID),
                new SqlParameter("@ActiveView360", product.ActiveView360),
                new SqlParameter("@ManufacturerWarranty", product.ManufacturerWarranty),
                new SqlParameter("@ModifiedBy", product.ModifiedBy),
                new SqlParameter("@YandexTypePrefix", product.YandexTypePrefix ?? (object) DBNull.Value),
                new SqlParameter("@YandexModel", product.YandexModel ?? (object) DBNull.Value),
                new SqlParameter("@BarCode", product.BarCode ?? (object) DBNull.Value),
                new SqlParameter("@Cbid", product.Cbid),
                new SqlParameter("@Fee", product.Fee),
                new SqlParameter("@AccrueBonuses", product.AccrueBonuses),
                new SqlParameter("@TaxId", product.TaxId ?? (object) DBNull.Value),
                new SqlParameter("@YandexSizeUnit", string.IsNullOrEmpty(product.YandexSizeUnit) ? (object)DBNull.Value : product.YandexSizeUnit),
                new SqlParameter("@YandexName", string.IsNullOrWhiteSpace(product.YandexName) ? string.Empty : product.YandexName)
            );

            OfferService.DeleteOldOffers(product.ProductId, product.Offers);

            if (product.Offers != null)
            {
                if (!product.HasMultiOffer && product.Offers.Any())
                {
                    var offfer = product.Offers.First();

                    if (string.IsNullOrEmpty(offfer.ArtNo) && !OfferService.IsArtNoExist(product.ArtNo, offfer.OfferId))
                        offfer.ArtNo = product.ArtNo.Trim();
                }


                foreach (var offer in product.Offers)
                {
                    if (offer.OfferId <= 0)
                    {
                        OfferService.AddOffer(offer);
                    }
                    else
                    {
                        OfferService.UpdateOffer(offer);
                    }
                }
            }

            if (product.Meta != null)
            {
                if (product.Meta.Title.IsNullOrEmpty() && product.Meta.MetaKeywords.IsNullOrEmpty() && product.Meta.MetaDescription.IsNullOrEmpty() && product.Meta.H1.IsNullOrEmpty())
                {
                    if (MetaInfoService.IsMetaExist(product.ProductId, MetaType.Product))
                        MetaInfoService.DeleteMetaInfo(product.ProductId, MetaType.Product);
                }
                else
                    MetaInfoService.SetMeta(product.Meta);
            }

            //tag
            var tags = product.Tags;
            TagService.DeleteMap(product.ProductId, ETagType.Product);
            if (tags != null && tags.Count != 0)
            {
                for (var i = 0; i < tags.Count; i++)
                {
                    var tag = TagService.Get(tags[i].Name);
                    tags[i].Id = tag == null ? TagService.Add(tags[i]) : tag.Id;
                    TagService.AddMap(product.ProductId, tags[i].Id, ETagType.Product, i * 10);
                }
            }


            if (updateIndexes)
            {
                SetProductHierarchicallyEnabled(product.ProductId);
                PreCalcProductParams(product.ProductId);

                CacheManager.RemoveByPattern(CacheNames.BrandsInCategory);
                CacheManager.RemoveByPattern(CacheNames.PropertiesInCategory);

                ProductWriter.AddUpdate(product);
            }
        }

        public static void UpdateProductDiscount(int productId, float discount)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Catalog].[Product] SET [Discount] = @Discount WHERE [ProductId] = @ProductId", CommandType.Text, new SqlParameter("@Discount", discount), new SqlParameter("@ProductId", productId));
        }

        public static Product GetProductFromReader(SqlDataReader reader)
        {
            var p = new Product
            {
                ProductId = SQLDataHelper.GetInt(reader, "ProductId"),
                ArtNo = SQLDataHelper.GetString(reader, "ArtNo"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                BriefDescription = SQLDataHelper.GetString(reader, "BriefDescription", string.Empty),
                Description = SQLDataHelper.GetString(reader, "Description", string.Empty),
                Photo = SQLDataHelper.GetString(reader, "PhotoName"),
                Discount = new Discount(SQLDataHelper.GetFloat(reader, "Discount"), SQLDataHelper.GetFloat(reader, "DiscountAmount")),
                Weight = SQLDataHelper.GetFloat(reader, "Weight"),
                Ratio = SQLDataHelper.GetDouble(reader, "Ratio"),
                Enabled = SQLDataHelper.GetBoolean(reader, "Enabled", true),
                AllowPreOrder = SQLDataHelper.GetBoolean(reader, "AllowPreOrder"),
                Recomended = SQLDataHelper.GetBoolean(reader, "Recomended"),
                New = SQLDataHelper.GetBoolean(reader, "New"),
                BestSeller = SQLDataHelper.GetBoolean(reader, "Bestseller"),
                OnSale = SQLDataHelper.GetBoolean(reader, "OnSale"),
                BrandId = SQLDataHelper.GetInt(reader, "BrandID", 0),
                UrlPath = SQLDataHelper.GetString(reader, "UrlPath"),
                CategoryEnabled = SQLDataHelper.GetBoolean(reader, "CategoryEnabled"),
                Unit = SQLDataHelper.GetString(reader, "Unit"),
                ShippingPrice = SQLDataHelper.GetNullableFloat(reader, "ShippingPrice"),
                Multiplicity = SQLDataHelper.GetFloat(reader, "Multiplicity"),
                MinAmount = SQLDataHelper.GetNullableFloat(reader, "MinAmount"),
                MaxAmount = SQLDataHelper.GetNullableFloat(reader, "MaxAmount"),
                SalesNote = SQLDataHelper.GetString(reader, "SalesNote"),
                HasMultiOffer = SQLDataHelper.GetBoolean(reader, "HasMultiOffer"),
                GoogleProductCategory = SQLDataHelper.GetString(reader, "GoogleProductCategory"),
                YandexMarketCategory = SQLDataHelper.GetString(reader, "YandexMarketCategory"),
                Gtin = SQLDataHelper.GetString(reader, "Gtin"),
                Adult = SQLDataHelper.GetBoolean(reader, "Adult"),
                Length = SQLDataHelper.GetFloat(reader, "Length"),
                Width = SQLDataHelper.GetFloat(reader, "Width"),
                Height = SQLDataHelper.GetFloat(reader, "Height"),
                CurrencyID = SQLDataHelper.GetInt(reader, "CurrencyID"),
                ActiveView360 = SQLDataHelper.GetBoolean(reader, "ActiveView360"),
                ManufacturerWarranty = SQLDataHelper.GetBoolean(reader, "ManufacturerWarranty"),
                ModifiedBy = SQLDataHelper.GetString(reader, "ModifiedBy"),
                YandexTypePrefix = SQLDataHelper.GetString(reader, "YandexTypePrefix"),
                YandexModel = SQLDataHelper.GetString(reader, "YandexModel"),
                BarCode = SQLDataHelper.GetString(reader, "BarCode"),
                Cbid = SQLDataHelper.GetFloat(reader, "Cbid"),
                Fee = SQLDataHelper.GetFloat(reader, "Fee"),
                AccrueBonuses = SQLDataHelper.GetBoolean(reader, "AccrueBonuses"),
                TaxId = SQLDataHelper.GetNullableInt(reader, "TaxId"),
                YandexSizeUnit = SQLDataHelper.GetString(reader, "YandexSizeUnit"),
                YandexName = SQLDataHelper.GetString(reader, "YandexName")
            };

            if (p.TaxId == null)
                p.TaxId = SettingsCatalog.DefaultTaxId;

            if (p.Multiplicity <= 0)
                p.Multiplicity = 1;

            return p;
        }

        public static Product GetProduct(int productId)
        {
            return SQLDataAccess.ExecuteReadOne("[Catalog].[sp_GetProductById]", CommandType.StoredProcedure, GetProductFromReader,
                                                new SqlParameter("@ProductID", productId), new SqlParameter("@Type", PhotoType.Product.ToString()));
        }

        public static Product GetProduct(string artNo, bool includeOfferArtNo = false)
        {
            var productId = GetProductId(artNo);
            if (productId == 0 && includeOfferArtNo)
            {
                productId = GetProductIDByOfferArtNo(artNo);
            }

            return productId > 0 ? GetProduct(productId) : null;
        }

        public static Product GetProductByUrl(string url)
        {
            return SQLDataAccess.ExecuteReadOne(
                "Select * From [Catalog].[Product] " +
                "LEFT JOIN [Catalog].[Photo] ON [Photo].[ObjId] = [Product].[ProductID] and [Type]=@Type AND [Main] = 1 " +
                "Where UrlPath=@UrlPath",
                CommandType.Text, GetProductFromReader,
                new SqlParameter("@UrlPath", url), new SqlParameter("@Type", PhotoType.Product.ToString()));
        }


        public static bool IsUniqueArtNo(string artNo)
        {
            return !SQLDataAccess.ExecuteScalar<bool>("Select Top(1) ProductID FROM [Catalog].[Product] WHERE ArtNo=@artNo", CommandType.Text, new SqlParameter("@artNo", artNo));
        }


        public static int GetProductId(string artNo)
        {
            return SQLDataAccess.ExecuteScalar<int>("Select Top(1) ProductID FROM [Catalog].[Product] WHERE ArtNo=@artNo", CommandType.Text, new SqlParameter("@artNo", artNo));
        }


        public static int GetProductIdByName(string name)
        {
            return SQLDataAccess.ExecuteScalar<int>("Select Top(1) ProductID FROM [Catalog].[Product] WHERE Name=@Name", CommandType.Text, new SqlParameter("@Name", name));
        }

        public static Product GetFirstProduct()
        {
            var productId = SQLDataAccess.ExecuteScalar<int>("Select TOP(1) product.productId from Catalog.ProductCategories inner join catalog.product on ProductCategories.productid=product.Productid where Enabled=1 and categoryEnabled=1", CommandType.Text);
            return GetProduct(productId);
        }

        public static Product GetProductByName(string name)
        {
            var productId = GetProductIdByName(name);
            return productId > 0 ? GetProduct(productId) : null;
        }

        public static int GetProductIDByOfferArtNo(string artNo)
        {
            return SQLDataAccess.ExecuteScalar<int>("Select Top(1) ProductID FROM [Catalog].[Offer] WHERE ArtNo=@artNo", CommandType.Text, new SqlParameter("@artNo", artNo));
        }

        public static List<int> GetProductsIDs()
        {
            return SQLDataAccess.ExecuteReadColumnIEnumerable<int>("SELECT [ProductID] FROM [Catalog].[Product]", CommandType.Text, "ProductID").ToList();
        }
        #endregion

        #region ProductLinks


        public static int DeleteAllProductLink(int productId)
        {
            var res = SQLDataAccess.ExecuteReadList<int>("Select [CategoryID] FROM [Catalog].[ProductCategories] WHERE [ProductID] =  @ProductId",
                                                        CommandType.Text,
                                                        reader => SQLDataHelper.GetInt(reader, "CategoryID"),
                                                        new SqlParameter("@ProductID", productId));
            foreach (var item in res)
            {
                CacheManager.Remove(CacheNames.GetCategoryCacheObjectName(item));
            }

            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Catalog].[ProductCategories] WHERE [ProductID] =  @ProductId", CommandType.Text, new SqlParameter("@ProductID", productId));

            return 0;
        }
        /// <summary>
        /// delete relationship between product and category
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="catId"></param>
        /// <returns></returns>
        public static int DeleteProductLink(int productId, int catId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_RemoveProductFromCategory]", CommandType.StoredProcedure, new SqlParameter("@ProductID", productId), new SqlParameter("@CategoryID", catId));
            CacheManager.Remove(CacheNames.GetCategoryCacheObjectName(catId));

            return 0;
        }

        /// <summary>
        /// enabled active trigger
        /// </summary>
        public static void EnableDynamicProductLinkRecalc()
        {
            SQLDataAccess.ExecuteNonQuery("ALTER TABLE [Catalog].[ProductCategories] ENABLE TRIGGER [InsertProductInCategory];" +
                                         " ALTER TABLE [Catalog].[ProductCategories] ENABLE TRIGGER [RemoveProductFromCategory];" +
                                         " ALTER TABLE [Catalog].[Product] ENABLE TRIGGER [EnabledChanged];", CommandType.Text);
        }

        /// <summary>
        /// disabled active trigger
        /// </summary>
        public static void DisableDynamicProductLinkRecalc()
        {
            SQLDataAccess.ExecuteNonQuery("ALTER TABLE [Catalog].[ProductCategories] DISABLE TRIGGER [InsertProductInCategory];" +
                                         " ALTER TABLE [Catalog].[ProductCategories] DISABLE TRIGGER [RemoveProductFromCategory];" +
                                         " ALTER TABLE [Catalog].[Product] DISABLE TRIGGER [EnabledChanged];",
                                            CommandType.Text);
        }

        public static void AddProductLink(int productId, int catId, int sortOrder, bool updatecache, bool mainCat = false)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_AddProductToCategory]", CommandType.StoredProcedure,
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@CategoryID", catId),
                                            new SqlParameter("@sortOrder", sortOrder),
                                            new SqlParameter("@mainCategory", mainCat));
            if (updatecache)
            {
                CategoryService.ClearCategoryCache();
            }
        }

        /// <summary>
        /// Update relationship
        /// </summary>
        /// <param name="productid"></param>
        /// <param name="sort"></param>
        /// <param name="cat"></param>
        /// <returns></returns>
        public static bool UpdateProductLinkSort(int productid, int sort, int cat)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateProductLinkSort]", CommandType.StoredProcedure,
                                            new SqlParameter { ParameterName = "@ProductID", Value = productid },
                                            new SqlParameter { ParameterName = "@CategoryID", Value = cat },
                                            new SqlParameter { ParameterName = "@SortOrder", Value = sort });

            return true;
        }

        #endregion

        #region Is Enabled
        /// <summary>
        /// Cheak if product enabled
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static bool IsProductEnabled(int productId)
        {
            var res = SQLDataAccess.ExecuteScalar<bool>("SELECT ([Enabled] & [CategoryEnabled]) as Enabled FROM [Catalog].[Product] WHERE [ProductID] = @id", CommandType.Text, new SqlParameter("@id", productId));

            return res;
        }

        /// <summary>
        /// disabled all products
        /// </summary>
        [Obsolete()]
        public static void DisableAllProducts()
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DisableAllProducts]", CommandType.StoredProcedure);
            CategoryService.ClearCategoryCache();
        }

        public static void ClearAmountAllProducts()
        {
            SQLDataAccess.ExecuteNonQuery("Update [Catalog].[Offer] Set [Amount] = 0", CommandType.Text);
        }

        public static void DisableAllProducts(DateTime startAt)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Catalog].[Product] SET [Enabled] = 0 where DateModified< @startAt ",
                                           CommandType.Text,
                                           new SqlParameter("startAt", startAt));
            CategoryService.ClearCategoryCache();
        }

        #endregion

        #region Filtered Select
        ///// <summary>
        ///// get all products
        ///// </summary>
        ///// <returns></returns>
        //public static IEnumerable<Product> GetAllProducts()
        //{
        //    const string query = @"select * FROM [Catalog].[Product] LEFT JOIN [Catalog].[Photo] ON [Photo].[ObjId] = [Product].[ProductID] AND Type=@Type AND Photo.[Main] = 1";
        //    return SQLDataAccess.ExecuteReadIEnumerable<Product>(query, CommandType.Text, GetProductFromReader, new SqlParameter("@Type", PhotoType.Product.ToString()));
        //}

        public static IEnumerable<int> GetAllProductIDs(bool onlyDemo = false)
        {
            return SQLDataAccess.ExecuteReadColumn<int>(
                "SELECT ProductId FROM [Catalog].[Product]" + (onlyDemo ? " WHERE IsDemo = 1" : string.Empty), 
                CommandType.Text, "ProductId");
        }

        /// <summary>
        /// Get products without category
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<int> GetProductIDsWithoutCategory()
        {
            return SQLDataAccess.ExecuteReadColumnIEnumerable<int>(
                    "SELECT [Product].[ProductID] FROM [Catalog].[Product] WHERE [Product].[ProductID] not in (select distinct [ProductID] from [Catalog].[ProductCategories])",
                    CommandType.Text,
                    "ProductID");
        }

        /// <summary>
        /// get products in categories
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<int> GetProductIDsInCategories()
        {
            return SQLDataAccess.ExecuteReadColumnIEnumerable<int>(
                    "select distinct [ProductID] from [Catalog].[ProductCategories]",
                    CommandType.Text,
                    "ProductID");
        }



        #endregion

        #region Products Count and Existance
        /// <summary>
        /// get products count
        /// </summary>
        /// <returns></returns>
        public static int GetProductsCount(string condition = null, params SqlParameter[] parameters)
        {
            if (condition == null)
            {
                return SQLDataAccess.ExecuteScalar<int>("SELECT Count([ProductID]) FROM [Catalog].[Product]",
                                                           CommandType.Text);
            }
            return SQLDataAccess.ExecuteScalar<int>("SELECT Count([ProductID]) FROM [Catalog].[Product]" + " WHERE " + condition,
                                                    CommandType.Text, parameters);
        }

        /// <summary>
        /// cheak exist product by productid
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static bool IsExists(int productId)
        {
            bool boolres = SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_GetProductCOUNTbyID]", CommandType.StoredProcedure, new SqlParameter("@ProductID", productId)) > 0;
            return boolres;
        }

        #endregion

        #region Offers
        #endregion

        #region Photos
        public static void AddProductPhotoByArtNo(string artNo, string fullfileName, string description, bool isMain, int? colorId, bool skipOriginal = false)
        {
            AddProductPhotoByProductId(GetProductId(artNo), fullfileName, description, isMain, colorId, skipOriginal);
        }

        public static void AddProductPhotoByProductId(int productId, string fullfilename, string description, bool isMain, int? colorId, bool skipOriginal = false)
        {
            if (string.IsNullOrWhiteSpace(fullfilename) || (!IsExists(productId))) return;

            var tempName = PhotoService.AddPhoto(new Photo(0, productId, PhotoType.Product)
            {
                Description = description,
                OriginName = Path.GetFileName(fullfilename),
                PhotoSortOrder = 0,
                ColorID = colorId
            });

            if (string.IsNullOrWhiteSpace(tempName)) return;

            using (var image = Image.FromFile(fullfilename))
            {
                FileHelpers.SaveProductImageUseCompress(tempName, image, skipOriginal);
            }
        }

        public static void UpdateProductPhotoByProductId(int productId, string fullfilename, string description, bool isMain, int? colorId, bool skipOriginal = false)
        {
            if (string.IsNullOrWhiteSpace(fullfilename) || (!IsExists(productId))) return;

            var productPhoto = PhotoService.GetProductPhoto(productId, Path.GetFileName(fullfilename));
            if (productPhoto == null || !productPhoto.PhotoName.IsNotEmpty()) return;
            productPhoto.ColorID = colorId;
            PhotoService.UpdatePhoto(productPhoto);

            using (var image = Image.FromFile(fullfilename))
            {
                FileHelpers.SaveProductImageUseCompress(productPhoto.PhotoName, image, skipOriginal);
            }
        }


        #endregion

        #region Product Price Change

        public static void IncrementProductsPrice(float value, bool bySupply, List<int> categoryIds, bool percent = true, bool allProducts = false)
        {
            ChangeProductsPriceByCategories(value, false, bySupply, categoryIds, percent, allProducts);
        }

        public static void DecrementProductsPrice(float value, bool bySupply, List<int> categoryIds, bool percent = true, bool allProducts = false)
        {
            ChangeProductsPriceByCategories(value, true, bySupply, categoryIds, percent, allProducts);
        }

        private static void ChangeProductsPriceByCategories(float value, bool negative, bool bySupply, List<int> categoryIds, bool percent = true, bool allProducts = false)
        {
            if (!allProducts && (categoryIds == null || !categoryIds.Any()))
                return;

            var price = bySupply ? "SupplyPrice" : "Price";

            var cmd = string.Format(
                "DECLARE @ProductId TABLE(id int); " +
                "Insert into @ProductId " +
                (allProducts
                    ? "Select ProductId From Catalog.Product"
                    : "Select ProductId FROM Catalog.ProductCategories WHERE CategoryID IN ({0}) AND Catalog.ProductCategories.Main = 1") +
                " " +
                "Update [Catalog].[Offer] set [Price] = [{1}] {2} {4} Where {5} {3} [ProductID] IN (SELECT id FROM @ProductId); " +
                "Update [Catalog].[Offer] set [Price] = 0 Where [Price] < 0; ",
                categoryIds != null ? categoryIds.AggregateString(",") : "",
                price,
                negative ? "-" : "+",
                bySupply ? "[SupplyPrice] > 0 and" : "",
                percent ? "([" + price + "] * @Value / 100)" : "@Value",
                bySupply ? " " : "[Price] > 0 and");

            SQLDataAccess.ExecuteNonQuery(cmd, CommandType.Text, new SqlParameter("@Value", value));
        }

        #endregion

        public static void SetMainLink(int productId, int categoryId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_SetMainCategoryLink]", CommandType.StoredProcedure,
                new SqlParameter("@ProductID", productId), new SqlParameter("@CategoryID", categoryId));
        }

        public static bool IsMainLink(int productId, int categoryId)
        {
            return
                SQLDataAccess.ExecuteReadColumn<bool>(
                    "SELECT Main FROM [Catalog].[ProductCategories] WHERE [ProductID] = @ProductID AND [CategoryID] = @CategoryID",
                    CommandType.Text, "Main", new SqlParameter("@ProductID", productId),
                    new SqlParameter("@CategoryID", categoryId)).FirstOrDefault();
        }

        public static void SetActive(int productId, bool active)
        {
            SQLDataAccess.ExecuteNonQuery(
                 "Update [Catalog].[Product] Set Enabled = @Enabled Where ProductID = @ProductID",
                 CommandType.Text,
                 new SqlParameter("@ProductID", productId),
                 new SqlParameter("@Enabled", active));
        }


        public static void SetBrand(int productId, int brandId)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Catalog].[Product] SET BrandID = @BrandID WHERE ProductID = @ProductID", CommandType.Text, new SqlParameter("@ProductID", productId), new SqlParameter("@BrandID", brandId));
        }

        public static void DeleteBrand(int productId)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Catalog].[Product] SET BrandID = NULL Where ProductID = @ProductID", CommandType.Text, new SqlParameter("@ProductID", productId));
        }

        public static List<string> GetForAutoCompleteByIdsInAdmin(string productIds)
        {
            if (string.IsNullOrEmpty(productIds))
                return new List<string>();

            return
                SQLDataAccess.ExecuteReadList<string>(
                    "Select Product.ProductID, Product.Name, Product.ArtNo, Product.UrlPath from Catalog.Product " +
                    " Inner Join (select item, sort from [Settings].[ParsingBySeperator](@productIds,'/') ) as dtt on Product.ProductId=convert(int, dtt.item) " +
                    " Where Enabled = 1 And CategoryEnabled = 1 " +
                    " order by dtt.sort",
                    CommandType.Text,
                    reader =>
                        string.Format("<a href=\"{2}\">{0}<span>({1})</span></a>",
                            SQLDataHelper.GetString(reader, "Name"), SQLDataHelper.GetString(reader, "ArtNo"),
                            String.Format("Product.aspx?ProductID={0}", SQLDataHelper.GetInt(reader, "ProductID"))),
                    new SqlParameter("@productIds", productIds));
        }


        public static List<Product> GetForAutoCompleteProductsByIds(string productIds)
        {
            if (string.IsNullOrEmpty(productIds))
                return new List<Product>();

            return SQLDataAccess.ExecuteReadList<Product>(
                "Select Product.ProductID, Product.Name, Product.ArtNo from Catalog.Product "
                + "Inner Join (select item, sort from [Settings].[ParsingBySeperator](@productIds,'/') ) as dtt on Product.ProductId=convert(int, dtt.item) "
                + "order by dtt.sort",
                CommandType.Text,
                reader => new Product()
                {
                    ProductId = SQLDataHelper.GetInt(reader, "ProductID"),
                    ArtNo = SQLDataHelper.GetString(reader, "ArtNo"),
                    Name = SQLDataHelper.GetString(reader, "Name")
                },
                new SqlParameter("@productIds", productIds));
        }

        public static List<ProductModel> GetForAutoCompleteProducts(string productIds)
        {
            return
                SQLDataAccess.Query<ProductModel>(
                    "Select Product.ProductID, Product.Name, Product.ArtNo, Product.UrlPath, Product.Enabled, Product.AllowPreOrder, Product.Ratio, " +
                    "Product.Discount, Product.DiscountAmount, Product.MinAmount, Product.MaxAmount, MaxAvailable AS Amount, MinPrice as BasePrice, CurrencyValue, " +
                    "Photo.PhotoId, PhotoName, Photo.Description as PhotoDescription, Gifts, Product.BarCode " +
                    "From Catalog.Product " +
                    "Inner Join (select item, sort from [Settings].[ParsingBySeperator](@productIds,'/') ) as dtt on Product.ProductId=convert(int, dtt.item) " +
                    "Left Join [Catalog].[ProductExt]  On [Product].[ProductID] = [ProductExt].[ProductID] " +
                    "Inner Join [Catalog].[Currency] On [Currency].[CurrencyID] = [Product].[CurrencyID] " +
                    "Left Join [Catalog].[Photo]  On [Photo].[PhotoId] = [ProductExt].[PhotoId] " +
                    "Where Enabled = 1 And CategoryEnabled = 1 " +
                    (SettingsCatalog.ShowOnlyAvalible ? "AND MaxAvailable>0 " : "") +
                    "Order by dtt.sort",
                    new
                    {
                        productIds = productIds,
                        Type = PhotoType.Product.ToString(),
                    }).ToList();
        }

        public static void MarkersFromString(Product product, string source, string columSeparator)
        {
            if (string.IsNullOrWhiteSpace(columSeparator))
                _MarkersFromString(product, source);
            else
                _MarkersFromString(product, source, columSeparator);
        }

        private static void _MarkersFromString(Product product, string source)
        {
            // b,n,r,s
            if (!string.IsNullOrWhiteSpace(source))
            {
                var items = source.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                product.BestSeller = !string.IsNullOrEmpty(items.FirstOrDefault(item => item == "b"));
                product.New = !string.IsNullOrEmpty(items.FirstOrDefault(item => item == "n"));
                product.Recomended = !string.IsNullOrEmpty(items.FirstOrDefault(item => item == "r"));
                product.OnSale = !string.IsNullOrEmpty(items.FirstOrDefault(item => item == "s"));
            }
            else
            {
                product.BestSeller = product.New = product.Recomended = product.OnSale = false;
            }
        }

        private static void _MarkersFromString(Product product, string source, string columSeparator)
        {
            // b,n,r,s
            if (!string.IsNullOrWhiteSpace(source))
            {
                var items = source.Split(new[] { columSeparator }, StringSplitOptions.RemoveEmptyEntries);
                product.BestSeller = !string.IsNullOrEmpty(items.FirstOrDefault(item => item == "b"));
                product.New = !string.IsNullOrEmpty(items.FirstOrDefault(item => item == "n"));
                product.Recomended = !string.IsNullOrEmpty(items.FirstOrDefault(item => item == "r"));
                product.OnSale = !string.IsNullOrEmpty(items.FirstOrDefault(item => item == "s"));
            }
            else
            {
                product.BestSeller = product.New = product.Recomended = product.OnSale = false;
            }
        }

        public static string MarkersToString(Product product, string columSeparator)
        {
            // b,n,r,s
            string res = string.Empty;
            res += product.BestSeller ? "b" + columSeparator : string.Empty;
            res += product.New ? "n" + columSeparator : string.Empty;
            res += product.Recomended ? "r" + columSeparator : string.Empty;
            res += product.OnSale ? "s" + columSeparator : string.Empty;
            if (res.Length > 0)
                res = res.Remove(res.Length - 1, 1);
            return res;
        }

        public static string MarkersToString(bool isBestSeller, bool isNew, bool idRecomended, bool isOnSale, string columSeparator)
        {
            // b,n,r,s
            string res = string.Empty;
            res += isBestSeller ? "b" + columSeparator : string.Empty;
            res += isNew ? "n" + columSeparator : string.Empty;
            res += idRecomended ? "r" + columSeparator : string.Empty;
            res += isOnSale ? "s" + columSeparator : string.Empty;
            if (res.Length > 0)
                res = res.Remove(res.Length - 1, 1);
            return res;
        }

        public static void PreCalcProductParams(int productId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[PreCalcProductParams]", CommandType.StoredProcedure,
                                new SqlParameter("@ProductId", productId),
                                new SqlParameter("@ModerateReviews", SettingsCatalog.ModerateReviews),
                                new SqlParameter("@OnlyAvailable", SettingsCatalog.ShowOnlyAvalible));

            CacheManager.RemoveByPattern(CacheNames.SQLPagingCount);
            CacheManager.RemoveByPattern(CacheNames.SQLPagingItems);
        }

        public static void PreCalcProductComments(int productId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update Catalog.ProductExt Set Comments = (SELECT Count(ReviewId) From CMS.Review Where EntityId = @ProductId And (Checked = 1 or @ModerateReviews = 0)) Where ProductId = @ProductId",
                CommandType.Text,
                180,
                new SqlParameter("@ProductId", productId),
                new SqlParameter("@ModerateReviews", SettingsCatalog.ModerateReviews));
        }

        public static void PreCalcProductRootCategory(int categoryId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update Catalog.ProductExt Set [CategoryId] = " +
                    "(Select Top 1 id From [Settings].[GetParentsCategoryByChild]((SELECT top 1 CategoryID FROM [Catalog].ProductCategories WHERE ProductID = ProductExt.ProductId ORDER BY Main DESC)) Order by sort desc) " +
                "Where CategoryId = @CategoryId",
                CommandType.Text, new SqlParameter("@CategoryId", categoryId));

            CacheManager.RemoveByPattern(CacheNames.SQLPagingCount);
            CacheManager.RemoveByPattern(CacheNames.SQLPagingItems);
        }

        public static void PreCalcProductParamsMass()
        {
            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText = "[Catalog].[PreCalcProductParamsMass]";
                    db.cmd.CommandType = CommandType.StoredProcedure;
                    db.cmd.CommandTimeout = 1000;
                    db.cmd.Parameters.Clear();
                    db.cmd.Parameters.Add(new SqlParameter("@ModerateReviews", SettingsCatalog.ModerateReviews));
                    db.cmd.Parameters.Add(new SqlParameter("@OnlyAvailable", SettingsCatalog.ShowOnlyAvalible));
                    db.cnOpen();
                    db.cmd.ExecuteNonQuery();
                    db.cnClose();
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            CacheManager.RemoveByPattern(CacheNames.SQLPagingCount);
            CacheManager.RemoveByPattern(CacheNames.SQLPagingItems);
        }

        public static void PreCalcProductParamsMassInBackground()
        {
            Task.Factory.StartNew(PreCalcProductParamsMass, TaskCreationOptions.LongRunning);
        }

        public static List<ProductDiscount> GetDiscountList()
        {
            var productDiscounts = new List<ProductDiscount>();

            var discountModules = AttachedModules.GetModules<IDiscount>();
            foreach (var discountModule in discountModules)
            {
                if (discountModule != null)
                {
                    var classInstance = (IDiscount)Activator.CreateInstance(discountModule);
                    productDiscounts.AddRange(classInstance.GetProductDiscountsList());
                }
            }
            return productDiscounts;
        }


        public static List<ProductModel> GetProductsByCategory(int categoryId, int count, ESortOrder sort, bool indepth = false)
        {
            var sortItems = new List<string>();

            if (SettingsCatalog.MoveNotAvaliableToEnd)
            {
                sortItems.Add("(CASE WHEN Price=0 THEN 0 ELSE 1 END) DESC");
                sortItems.Add("AmountSort DESC");
            }

            switch (sort)
            {
                case ESortOrder.AscByName:
                    sortItems.Add("Product.Name ASC");
                    break;

                case ESortOrder.DescByName:
                    sortItems.Add("Product.Name DESC");
                    break;

                case ESortOrder.AscByPrice:
                    sortItems.Add("PriceTemp ASC");
                    break;

                case ESortOrder.DescByPrice:
                    sortItems.Add("PriceTemp DESC");
                    break;

                case ESortOrder.AscByRatio:
                    sortItems.Add("Ratio ASC");
                    break;

                case ESortOrder.DescByRatio:
                    sortItems.Add("Ratio DESC");
                    break;

                case ESortOrder.AscByAddingDate:
                    sortItems.Add("DateAdded ASC");
                    break;

                case ESortOrder.DescByAddingDate:
                    sortItems.Add("DateAdded DESC");
                    break;

                default:
                    //_paging.OrderByDesc("AmountSort".AsSqlField("TempAmountSort"));
                    break;
            }

            //if (!indepth)
                sortItems.Add("[ProductCategories].[SortOrder] ASC");

            return SQLDataAccess.Query<ProductModel>(
                "Select Top(@Count) [Product].[ProductID], CountPhoto, Photo.PhotoId, PhotoName, Photo.Description as PhotoDescription, BriefDescription, Product.ArtNo, " +
                "Product.Name, Recomended as Recomend, Bestseller, New, OnSale as Sales, Product.Discount, Product.DiscountAmount, Offer.OfferID, MaxAvailable AS Amount, MinAmount, MaxAmount, Product.Enabled, " +
                "AllowPreOrder, Ratio, ShoppingCartItemId, Product.UrlPath, Offer.ColorID, DateAdded, NotSamePrices as MultiPrices, " +
                "MinPrice as BasePrice, null as AdditionalPhoto, Brand.BrandName, Brand.UrlPath as BrandUrlPath, CurrencyValue, Product.BarCode, " +
                (SettingsCatalog.ComplexFilter
                    ? "Colors"
                    : "null as Colors") +
                " From [Catalog].[Product] " +
                "Left Join [Catalog].[ProductExt]  ON [Product].[ProductID] = [ProductExt].[ProductID]  " +
                "Left Join [Catalog].[Photo]  ON [Photo].[PhotoId] = [ProductExt].[PhotoId] " +
                "Left Join [Catalog].[Offer] ON [ProductExt].[OfferID] = [Offer].[OfferID] " +
                "Inner Join [Catalog].[Currency] ON [Currency].[CurrencyID] = [Product].[CurrencyID] " +
                "LEFT JOIN [Catalog].[ShoppingCart] ON [Catalog].[ShoppingCart].[OfferID] = [Catalog].[Offer].[OfferID] AND [Catalog].[ShoppingCart].[ShoppingCartType] = @ShoppingCartType AND [ShoppingCart].[CustomerID] = @CustomerId " +
                "Left JOIN [Catalog].[Brand] on Product.BrandId=Brand.BrandID " +
                "Left Join [Catalog].[ProductCategories] On ProductCategories.ProductId = [Product].[ProductID] and ProductCategories.Main=1" +
                "Where  Product.Enabled=1 and CategoryEnabled=1 and " +
                (indepth
                    ? "Exists(select 1 from [Catalog].[ProductCategories] INNER JOIN [Settings].[GetChildCategoryByParent](@CategoryId) AS hCat ON hCat.id = [ProductCategories].[CategoryID] and ProductCategories.ProductId = [Product].[ProductID]) "
                    : "Exists(select 1 from [Catalog].[ProductCategories] where [ProductCategories].[CategoryId] = @CategoryId and ProductCategories.ProductId = [Product].[ProductID])") +
                (SettingsCatalog.ShowOnlyAvalible ? " AND MaxAvailable > 0" : "") +
                (sortItems.Count > 0 ? string.Format(" Order by {0}", string.Join(", ", sortItems)) : string.Empty),
                new
                {
                    Count = count,
                    CustomerId = CustomerContext.CustomerId.ToString(),
                    Type = PhotoType.Product.ToString(),
                    ShoppingCartType = (int)ShoppingCartType.Compare,
                    CategoryId = categoryId
                }).ToList();
        }

        public static List<ProductModel> GetProductsByBrand(int brandId)
        {
            return SQLDataAccess.Query<ProductModel>("SELECT * FROM [Catalog].[Product] WHERE [BrandId] = " + brandId).ToList();
        }

        public static List<int> GetProductIdsByOfferIds(List<int> offerIds)
        {
            return SQLDataAccess.ExecuteReadList(
                "Select Distinct ProductId From [Catalog].[Offer] Where OfferId in (" + String.Join(",", offerIds) + ")",
                CommandType.Text,
                reader => SQLDataHelper.GetInt(reader, "ProductId"));
        }

        public static List<string> GetDeletedProducts(DateTime? from, DateTime? to)
        {
            var query = "SELECT ArtNo FROM [Catalog].[DeletedProducts]";
            var queryParams = new List<SqlParameter>();

            if (from != null && to != null)
            {
                query += " Where [DateTime] >= @From and [DateTime] <= @To";
                queryParams.Add(new SqlParameter("@From", from));
                queryParams.Add(new SqlParameter("@To", to));
            }

            return SQLDataAccess.ExecuteReadList(query, CommandType.Text, reader => SQLDataHelper.GetString(reader, "ArtNo"), queryParams.ToArray());
        }

        #region Gift

        public static bool HasGifts(int productId)
        {
            return
                Convert.ToBoolean(
                    SQLDataAccess.ExecuteScalar(
                        "Select Gifts From [Catalog].[ProductExt] Where ProductId = @ProductId", CommandType.Text,
                        new SqlParameter("@ProductId", productId)));
        }

        #endregion

        public static DateTime? GetModifiedDate(int productId)
        {
            var date = SQLDataAccess.ExecuteScalar(
                "Select Top(1) DateModified From Catalog.Product Where ProductId=@productId",
                CommandType.Text,
                new SqlParameter("@productId", productId));

            return date != null ? Convert.ToDateTime(date) : default(DateTime?);
        }



        public static void DeactivateGoodsMoreThan(int activeProductsCount)
        {
            if (activeProductsCount <= 0)
                return;

            SQLDataAccess.ExecuteNonQuery(
                @"if(Select (Count(ProductId)) From Catalog.Product Where Product.Enabled = 1 ) > @productsNumber
                  BEGIN
                    ;WITH productsToDeactivate AS 
                    ( 
	                    Select 
	                    Top(Select (Count(ProductId) - @productsNumber) From Catalog.Product Where Product.Enabled = 1) Product.ProductId 
	                    From Catalog.Product
	                    Where Product.Enabled = 1 
	                    Order by Product.DateAdded Desc
                    ) 
                    UPDATE Catalog.Product SET Enabled = 0 Where ProductId in (Select ProductId from productsToDeactivate)
                  END",
                CommandType.Text,
                new SqlParameter("@productsNumber", activeProductsCount));
        }
    }
}