using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Catalog
{
    public static class ProductListService
    {
        #region CRUD methods

        public static int Add(ProductList productList)
        {
            ClearCache();

            return SQLDataAccess.ExecuteScalar<int>(
                "Insert Into [Catalog].[ProductList] ([Name],[SortOrder],[Enabled],[Description]) Values (@Name,@SortOrder,@Enabled,@Description); " +
                "Select scope_identity();",
                CommandType.Text,
                new SqlParameter("@Name", productList.Name),
                new SqlParameter("@SortOrder", productList.SortOrder),
                new SqlParameter("@Enabled", productList.Enabled),
                new SqlParameter("@Description", productList.Description ?? ""));
        }

        public static void Update(ProductList productList)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [Catalog].[ProductList] Set Name=@Name, SortOrder=@SortOrder, Enabled=@Enabled, Description=@Description Where Id=@Id",
                CommandType.Text,
                new SqlParameter("@Id", productList.Id),
                new SqlParameter("@Name", productList.Name),
                new SqlParameter("@SortOrder", productList.SortOrder),
                new SqlParameter("@Enabled", productList.Enabled),
                new SqlParameter("@Description", productList.Description ?? ""));

            ClearCache();
        }

        public static void Delete(int productListId)
        {
            SQLDataAccess.ExecuteNonQuery("Delete FROM [Catalog].[ProductList] WHERE Id=@Id", CommandType.Text,
                new SqlParameter("@Id", productListId));

            ClearCache();
        }

        private static ProductList GetProductListFromReader(SqlDataReader reader)
        {
            return new ProductList
            {
                Id = SQLDataHelper.GetInt(reader, "Id"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                Enabled = SQLDataHelper.GetBoolean(reader, "Enabled"),
                Description = SQLDataHelper.GetString(reader, "Description"),
            };
        }

        public static ProductList Get(int productListId)
        {
            return SQLDataAccess.ExecuteReadOne("SELECT * FROM [Catalog].[ProductList] WHERE Id = @Id", CommandType.Text,
                GetProductListFromReader, new SqlParameter("@Id", productListId));
        }

        public static List<ProductList> GetList()
        {
            return SQLDataAccess.ExecuteReadList("SELECT * FROM [Catalog].[ProductList]", CommandType.Text,
                GetProductListFromReader);
        }

        public static int GetCount()
        {
            return SQLDataAccess.ExecuteScalar<int>("SELECT Count(*) FROM [Catalog].[ProductList]", CommandType.Text);
        }

        public static List<ProductList> GetMainPageList()
        {
            return CacheManager.Get(CacheNames.ProductList + "MainPage",
                () =>
                    SQLDataAccess.ExecuteReadList(
                        "SELECT * FROM [Catalog].[ProductList] " +
                        "WHERE Enabled = 1 and Exists(Select 1 From [Catalog].[Product_ProductList] Where [Product_ProductList].[ListId] = [ProductList].[Id]) " +
                        "Order By SortOrder", CommandType.Text, GetProductListFromReader));
        }

        #endregion

        #region Product List mapping 

        public static int AddProduct(int listId, int productId, int sort)
        {
            ClearCache();

            return SQLDataAccess.ExecuteScalar<int>(
                "Insert Into [Catalog].[Product_ProductList] ([ListId],[ProductId],[SortOrder]) Values (@ListId,@ProductId,@SortOrder) ",
                CommandType.Text,
                new SqlParameter("@ListId", listId),
                new SqlParameter("@ProductId", productId),
                new SqlParameter("@SortOrder", sort));
        }

        public static void UpdateProduct(int listId, int productId, int sort)
        {
            SQLDataAccess.ExecuteScalar<int>(
                "Update [Catalog].[Product_ProductList] Set SortOrder=@SortOrder Where ListId=@ListId and ProductId=@ProductId",
                CommandType.Text,
                new SqlParameter("@ListId", listId),
                new SqlParameter("@ProductId", productId),
                new SqlParameter("@SortOrder", sort));

            ClearCache();
        }

        public static void DeleteProduct(int listId, int productId)
        {
            SQLDataAccess.ExecuteNonQuery("Delete FROM [Catalog].[Product_ProductList] WHERE ListId=@ListId and ProductId=@ProductId", CommandType.Text,
                new SqlParameter("@ListId", listId),
                new SqlParameter("@ProductId", productId));

            
        }

        public static List<int> GetProductIds(int listId)
        {
            return SQLDataAccess.ExecuteReadList("SELECT ProductId FROM [Catalog].[Product_ProductList] WHERE ListId = @ListId", CommandType.Text,
                reader => SQLDataHelper.GetInt(reader, "ProductId"), new SqlParameter("@ListId", listId));
        }

        public static List<ProductModel> GetProducts(int listId, int count)
        {
            return
                CacheManager.Get(CacheNames.ProductListCacheName(listId, count),
                    () =>
                        SQLDataAccess.Query<ProductModel>(
                            "Select Top(@Count) [Product].[ProductID], Product.BriefDescription, Product.ArtNo, Product.Name, Recomended as Recomend, Bestseller, New, OnSale as Sales, Discount, DiscountAmount, " +
                            "Product.Enabled, Product.UrlPath, AllowPreOrder, Ratio, Offer.OfferID, MaxAvailable AS Amount, MinAmount, MaxAmount, MinPrice as BasePrice, " +
                            "CountPhoto, Photo.PhotoId, PhotoName, Photo.Description as PhotoDescription, Offer.ColorID, DateAdded, NotSamePrices as MultiPrices, " +
                            "null as AdditionalPhoto, " +
                            (SettingsCatalog.ComplexFilter ? "Colors," : "null as Colors,") +
                            " Comments, Category.Name as CategoryName, Category.UrlPath as CategoryUrl, CurrencyValue, Gifts " +

                            "From [Catalog].[Product] " +
                            "LEFT JOIN [Catalog].[Product_ProductList] ON [Product].[ProductID] = [Product_ProductList].[ProductId] " +
                            "LEFT JOIN [Catalog].[ProductExt]  ON [Product].[ProductID] = [ProductExt].[ProductID]  " +
                            "LEFT JOIN [Catalog].[Offer] ON [ProductExt].[OfferID] = [Offer].[OfferID] " +
                            "LEFT JOIN [Catalog].[Photo] ON [Photo].[PhotoId] = [ProductExt].[PhotoId] And Type=@Type " +
                            "Left JOIN [Catalog].[Category] On [Category].[CategoryId] = [ProductExt].[CategoryId] " +
                            "Inner Join [Catalog].[Currency] On [Currency].[CurrencyID] = [Product].[CurrencyID] " +

                            "Where ListId=@ListId and Product.Enabled=1 and CategoryEnabled=1" +
                            (SettingsCatalog.ShowOnlyAvalible ? " AND MaxAvailable>0" : string.Empty) +
                            " Order by [Product_ProductList].SortOrder, Product.ProductId",

                            new
                            {
                                ListId = listId,
                                Count = count,
                                Type = PhotoType.Product.ToString(),
                            }).ToList());
        }

        private static void ClearCache()
        {
            CacheManager.RemoveByPattern(CacheNames.ProductList);

            CacheManager.RemoveByPattern(CacheNames.SQLPagingItems);
            CacheManager.RemoveByPattern(CacheNames.SQLPagingCount);
        }

        #endregion
    }
}