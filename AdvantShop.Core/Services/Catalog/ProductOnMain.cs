//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL;

namespace AdvantShop.Catalog
{
    public enum EProductOnMain
    {
        None = 0,
        Best = 1,
        New = 2,
        Sale = 3,
        List = 4
    }

    public static class ProductOnMain
    {
        public static List<int> GetProductIdByType(EProductOnMain type)
        {
            string sqlCmd;
            switch (type)
            {
                case EProductOnMain.Best:
                    sqlCmd = "select ProductId from Catalog.Product where Bestseller=1";
                    break;
                case EProductOnMain.New:
                    sqlCmd = "select ProductId from Catalog.Product where New=1";
                    break;
                case EProductOnMain.Sale:
                    sqlCmd = "select ProductId from Catalog.Product where (Discount > 0 or DiscountAmount > 0)";
                    break;
                default:
                    throw new NotImplementedException();
            }
            return SQLDataAccess.ExecuteReadColumn<int>(sqlCmd, CommandType.Text, "ProductId", new SqlParameter("@type", (int)type));
        }
        
        public static List<ProductModel> GetProductsByType(EProductOnMain type, int count)
        {
            return CacheManager.Get(CacheNames.MainPageProductsCacheName(type.ToString(), count), () =>
            {
                var sqlCmd =
                    "Select Top(@Count) [Product].[ProductID], Product.BriefDescription, Product.ArtNo, Product.Name, Recomended as Recomend, Bestseller, New, OnSale as Sales, Discount, DiscountAmount, " +
                    "Product.Enabled, Product.UrlPath, AllowPreOrder, Ratio, Offer.OfferID, MaxAvailable AS Amount, MinAmount, MaxAmount, MinPrice as BasePrice," +
                    "CountPhoto, Photo.PhotoId, PhotoName, Photo.Description as PhotoDescription, Offer.ColorID, DateAdded, NotSamePrices as MultiPrices," +
                    "null as AdditionalPhoto, " +
                    (SettingsCatalog.ComplexFilter ? "Colors," : "null as Colors,") +
                    " Category.Name as CategoryName, Category.UrlPath as CategoryUrl, " +
                    "CurrencyValue, Gifts " +

                    "From [Catalog].[Product] " +
                    "Left Join [Catalog].[ProductExt]  On [Product].[ProductID] = [ProductExt].[ProductID]  " +
                    "Inner Join Catalog.Currency On Currency.CurrencyID = Product.CurrencyID " +
                    "Left Join [Catalog].[Photo] On [Photo].[PhotoId] = [ProductExt].[PhotoId] And Type=@Type " +
                    "Left Join [Catalog].[Offer] On [ProductExt].[OfferID] = [Offer].[OfferID] " +

                    "Left JOIN [Catalog].[Category] On [Category].[CategoryId] = [ProductExt].[CategoryId] " +
                    "Where {0} and Product.Enabled=1 and CategoryEnabled=1 " +
                    (SettingsCatalog.ShowOnlyAvalible ? " AND MaxAvailable>0" : string.Empty) + " Order by {1}";

                switch (type)
                {
                    case EProductOnMain.Best:
                        sqlCmd = string.Format(sqlCmd, "Bestseller=1", "SortBestseller, DateAdded desc");
                        break;
                    case EProductOnMain.New:
                        sqlCmd = string.Format(sqlCmd, "New=1", "SortNew, DateAdded desc");
                        break;
                    case EProductOnMain.Sale:
                        sqlCmd = string.Format(sqlCmd, "(Discount>0 or DiscountAmount>0)", "SortDiscount, Discount desc, DiscountAmount desc");
                        break;
                    default:
                        throw new NotImplementedException();
                }

                return
                    SQLDataAccess.Query<ProductModel>(sqlCmd,
                        new
                        {
                            Count = count,
                            Type = PhotoType.Product.ToString(),
                        }).ToList();
            });
        }

        public static List<ProductModel> GetProductsByTypeMobile(EProductOnMain type, int count)
        {
            return CacheManager.Get(CacheNames.MainPageProductsCacheName(type.ToString() + "_Mobile", count), () =>
            {
                var sqlCmd =
                    "Select Top(@Count) [Product].[ProductID], Product.ArtNo, Offer.OfferID, Product.Name, Product.UrlPath, MinPrice as BasePrice, Recomended, Bestseller, New, OnSale as Sale, Discount, DiscountAmount, " +
                    "PhotoName, Photo.Description as PhotoDescription, CurrencyValue, Gifts, NotSamePrices as MultiPrices " +
                    "From [Catalog].[Product] " +
                    "Left Join [Catalog].[ProductExt]  On [Product].[ProductID] = [ProductExt].[ProductID]  " +
                    "Inner Join Catalog.Currency On Currency.CurrencyID = Product.CurrencyID " +
                    "Left Join [Catalog].[Photo] On [Photo].[PhotoId] = [ProductExt].[PhotoId] And Type=@Type " +
                    "Left Join [Catalog].[Offer] On [ProductExt].[OfferID] = [Offer].[OfferID] " +
                    "Where {0} and Product.Enabled=1 and CategoryEnabled=1 " +
                    (SettingsCatalog.ShowOnlyAvalible ? " AND MaxAvailable>0" : string.Empty) + " Order by {1}";

                switch (type)
                {
                    case EProductOnMain.Best:
                        sqlCmd = string.Format(sqlCmd, "Bestseller=1", "SortBestseller, DateAdded desc");
                        break;
                    case EProductOnMain.New:
                        sqlCmd = string.Format(sqlCmd, "New=1", "SortNew, DateAdded desc");
                        break;
                    case EProductOnMain.Sale:
                        sqlCmd = string.Format(sqlCmd, "(Discount>0 or DiscountAmount> 0)", "SortDiscount, Discount desc, DiscountAmount desc");
                        break;
                    default:
                        throw new NotImplementedException();
                }

                return
                    SQLDataAccess.Query<ProductModel>(sqlCmd,
                        new
                        {
                            Count = count,
                            Type = PhotoType.Product.ToString(),
                        }).ToList();
            });
        }

        public static DataTable GetAdminProductsByType(EProductOnMain type, int count)
        {
            string sqlCmd;
            switch (type)
            {
                case EProductOnMain.Best:
                    sqlCmd = "select Top(@count) Product.ProductId, Name from Catalog.Product where Bestseller=1 order by SortBestseller";
                    break;
                case EProductOnMain.New:
                    sqlCmd = "select Top(@count) Product.ProductId, Name from Catalog.Product where New=1 order by SortNew";
                    break;
                case EProductOnMain.Sale:
                    sqlCmd = "select Top(@count) Product.ProductId, Name from Catalog.Product where (Discount > 0 or DiscountAmount > 0) order by SortDiscount";
                    break;
                default:
                    throw new NotImplementedException();
            }
            return SQLDataAccess.ExecuteTable(sqlCmd, CommandType.Text, new SqlParameter { ParameterName = "@count", Value = count });
        }

        public static int GetProductCountByType(EProductOnMain type, bool enabled = true)
        {
            return CacheManager.Get(CacheNames.MainPageProductsCountCacheName(type.ToString(), enabled), () =>
            {
                var sql = "select Count(ProductId) from Catalog.Product where " +
                          (enabled ? "Enabled=1 and CategoryEnabled=1 and " : "");

                switch (type)
                {
                    case EProductOnMain.Best:
                        sql += "bestseller=1";
                        break;
                    case EProductOnMain.New:
                        sql += "new=1";
                        break;
                    case EProductOnMain.Sale:
                        sql += "(Discount > 0 or DiscountAmount > 0)";
                        break;
                    default:
                        throw new NotImplementedException();
                }
                return SQLDataAccess.ExecuteScalar<int>(sql, CommandType.Text);
            });
        }

        public static bool IsExistsProductByType(EProductOnMain type)
        {
            string sqlCmd = "if exists(select 1 from Catalog.Product where Enabled=1 and CategoryEnabled=1 and {0}) Select 1 else Select 0";
            switch (type)
            {
                case EProductOnMain.Best:
                    sqlCmd = string.Format(sqlCmd, "bestseller=1");
                    break;
                case EProductOnMain.New:
                    sqlCmd = string.Format(sqlCmd, "new=1");
                    break;
                case EProductOnMain.Sale:
                    sqlCmd = string.Format(sqlCmd, "(Discount > 0 or DiscountAmount > 0)");
                    break;
                default:
                    throw new NotImplementedException();
            }
            return Convert.ToBoolean(SQLDataAccess.ExecuteScalar(sqlCmd, CommandType.Text));
        }


        public static void AddProductByType(int productId, EProductOnMain type)
        {
            string sqlCmd;
            switch (type)
            {
                case EProductOnMain.Best:
                    sqlCmd = "Update Catalog.Product set SortBestseller=(Select min(SortBestseller)-10 from Catalog.Product), Bestseller=1 where ProductId=@productId";
                    break;
                case EProductOnMain.New:
                    sqlCmd = "Update Catalog.Product set SortNew=(Select min(SortNew)-10 from Catalog.Product), New=1 where ProductId=@productId";
                    break;
                case EProductOnMain.Sale:
                    sqlCmd = "Update Catalog.Product set SortDiscount=(Select min(SortDiscount)-10 from Catalog.Product) where ProductId=@productId";
                    break;
                default:
                    throw new NotImplementedException();
            }
            SQLDataAccess.ExecuteNonQuery(sqlCmd, CommandType.Text, new SqlParameter("@productId", productId));

            ClearCache();
        }

        public static void DeleteProductByType(int prodcutId, EProductOnMain type)
        {
            string sqlCmd;
            switch (type)
            {
                case EProductOnMain.Best:
                    sqlCmd = "Update Catalog.Product set SortBestseller=0, Bestseller=0 where ProductId=@productId";
                    break;
                case EProductOnMain.New:
                    sqlCmd = "Update Catalog.Product set SortNew=0, New=0 where ProductId=@productId";
                    break;
                case EProductOnMain.Sale:
                    sqlCmd = "Update Catalog.Product set SortDiscount=0, Discount=0, DiscountAmount=0 where ProductId=@productId";
                    break;
                default:
                    throw new NotImplementedException();
            }

            SQLDataAccess.ExecuteNonQuery(sqlCmd, CommandType.Text, new SqlParameter("@productId", prodcutId));

            ClearCache();
        }

        public static void UpdateProductByType(int productId, int sortOrder, EProductOnMain type)
        {
            string sqlCmd;
            switch (type)
            {
                case EProductOnMain.Best:
                    sqlCmd = "Update Catalog.Product set SortBestseller=@sortOrder where ProductId=@productId and Bestseller=1";
                    break;
                case EProductOnMain.New:
                    sqlCmd = "Update Catalog.Product set SortNew=@sortOrder where ProductId=@productId and New=1";
                    break;
                case EProductOnMain.Sale:
                    sqlCmd = "Update Catalog.Product set SortDiscount=@sortOrder where ProductId=@productId";
                    break;
                default:
                    throw new NotImplementedException();
            }

            SQLDataAccess.ExecuteNonQuery(sqlCmd, CommandType.Text, new SqlParameter("@productId", productId), new SqlParameter("@sortOrder", sortOrder));

            ClearCache();
        }

        private static void ClearCache()
        {
            CacheManager.RemoveByPattern(CacheNames.SQLPagingItems);
            CacheManager.RemoveByPattern(CacheNames.SQLPagingCount);
        }
    }
}