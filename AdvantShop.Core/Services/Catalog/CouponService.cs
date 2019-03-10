//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Orders;

namespace AdvantShop.Catalog
{
    public class CouponService
    {

        #region Get, Add, Update, Delete
        public static Coupon GetCoupon(int couponID)
        {
            return SQLDataAccess.ExecuteReadOne<Coupon>("SELECT * FROM [Catalog].[Coupon] WHERE CouponID = @CouponID",
                                                         CommandType.Text, GetFromReader, new SqlParameter("@CouponID", couponID));
        }

        public static Coupon GetCouponByCode(string code)
        {
            return SQLDataAccess.ExecuteReadOne<Coupon>("SELECT * FROM [Catalog].[Coupon] WHERE Code = @Code",
                                             CommandType.Text, GetFromReader, new SqlParameter("@Code", code));
        }

        public static List<Coupon> GetAllCoupons()
        {
            List<Coupon> couponList = SQLDataAccess.ExecuteReadList<Coupon>("SELECT * FROM [Catalog].[Coupon]", CommandType.Text, GetFromReader);
            return couponList;
        }

        private static Coupon GetFromReader(SqlDataReader reader)
        {
            return new Coupon
            {
                CouponID = SQLDataHelper.GetInt(reader, "CouponID"),
                Code = SQLDataHelper.GetString(reader, "Code"),
                Type = (CouponType) SQLDataHelper.GetInt(reader, "Type"),
                Value = SQLDataHelper.GetFloat(reader, "Value"),
                AddingDate = SQLDataHelper.GetDateTime(reader, "AddingDate"),
                ExpirationDate = SQLDataHelper.GetNullableDateTime(reader, "ExpirationDate"),
                PossibleUses = SQLDataHelper.GetInt(reader, "PossibleUses"),
                ActualUses = SQLDataHelper.GetInt(reader, "ActualUses"),
                Enabled = SQLDataHelper.GetBoolean(reader, "Enabled"),
                MinimalOrderPrice = SQLDataHelper.GetFloat(reader, "MinimalOrderPrice"),
                CurrencyIso3 = SQLDataHelper.GetString(reader, "CurrencyIso3"),
            };
        }

        public static void AddCoupon(Coupon coupon)
        {
            coupon.CouponID = SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO [Catalog].[Coupon] " +
                "([Code], [Type], [Value], [AddingDate], [ExpirationDate], [PossibleUses], [ActualUses], [Enabled], [MinimalOrderPrice], CurrencyIso3) " +
                "VALUES (@Code, @Type, @Value, @AddingDate, @ExpirationDate, @PossibleUses, @ActualUses, @Enabled, @MinimalOrderPrice, @CurrencyIso3); SELECT SCOPE_IDENTITY();",
                CommandType.Text,
                new SqlParameter("@Code", coupon.Code),
                new SqlParameter("@Type", coupon.Type),
                new SqlParameter("@Value", coupon.Value),
                new SqlParameter("@AddingDate", coupon.AddingDate),
                new SqlParameter("@ExpirationDate", coupon.ExpirationDate ?? (object) DBNull.Value),
                new SqlParameter("@PossibleUses", coupon.PossibleUses),
                new SqlParameter("@ActualUses", coupon.ActualUses),
                new SqlParameter("@Enabled", coupon.Enabled),
                new SqlParameter("@MinimalOrderPrice", coupon.MinimalOrderPrice),
                new SqlParameter("@CurrencyIso3", coupon.CurrencyIso3)
                );
        }

        public static void UpdateCoupon(Coupon coupon)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Catalog].[Coupon] SET [Code] = @Code, [Type] = @Type, [Value] = @Value, [AddingDate]=@AddingDate, [ExpirationDate] = @ExpirationDate, [PossibleUses] = @PossibleUses, [ActualUses] = @ActualUses, [Enabled] = @Enabled, [MinimalOrderPrice] = @MinimalOrderPrice, CurrencyIso3 = @CurrencyIso3 " +
                "WHERE CouponID = @CouponID", CommandType.Text,
                new SqlParameter("@CouponID", coupon.CouponID),
                new SqlParameter("@Code", coupon.Code),
                new SqlParameter("@Type", coupon.Type),
                new SqlParameter("@Value", coupon.Value),
                new SqlParameter("@AddingDate", coupon.AddingDate),
                new SqlParameter("@ExpirationDate", coupon.ExpirationDate ?? (object) DBNull.Value),
                new SqlParameter("@PossibleUses", coupon.PossibleUses),
                new SqlParameter("@ActualUses", coupon.ActualUses),
                new SqlParameter("@Enabled", coupon.Enabled),
                new SqlParameter("@MinimalOrderPrice", coupon.MinimalOrderPrice),
                new SqlParameter("@CurrencyIso3", coupon.CurrencyIso3)
                );
        }

        public static void DeleteCoupon(int couponId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Catalog].[Coupon] WHERE CouponID = @CouponID", CommandType.Text, new SqlParameter("@CouponID", couponId));
        }

        #endregion

        #region Product links

        public static void AddProductToCoupon(int couponID, int productID)
        {
            SQLDataAccess.ExecuteNonQuery("insert into [Catalog].[CouponProducts] (couponID,  productID) values (@couponID, @productID)",
                                            CommandType.Text,
                                            new SqlParameter("@CouponID", couponID),
                                            new SqlParameter("@productID", productID));
        }

        public static List<int> GetProductsIDsByCoupon(int couponID)
        {
            List<int> list = SQLDataAccess.ExecuteReadList<int>("Select ProductID from [Catalog].[CouponProducts] where couponID=@couponID",
                                                           CommandType.Text,
                                                           reader => SQLDataHelper.GetInt(reader, "ProductID"),
                                                           new SqlParameter("@CouponID", couponID));
            return list;
        }

        public static void DeleteProductFromCoupon(int couponID, int productID)
        {
            SQLDataAccess.ExecuteNonQuery("Delete from [Catalog].[CouponProducts] Where couponID=@couponID and productID=@productID",
                                            CommandType.Text,
                                            new SqlParameter("@CouponID", couponID),
                                            new SqlParameter("@productID", productID));
        }

        public static void DeleteAllProductsFromCoupon(int couponID)
        {
            SQLDataAccess.ExecuteNonQuery("Delete from [Catalog].[CouponProducts] Where couponID=@couponID",
                                            CommandType.Text,
                                            new SqlParameter("@CouponID", couponID));
        }

        #endregion

        #region Categories link

        public static void AddCategoryToCoupon(int couponID, int categoryId)
        {
            SQLDataAccess.ExecuteNonQuery("insert into [Catalog].[CouponCategories] (couponID,  categoryId) values (@couponID, @categoryId)",
                                            CommandType.Text,
                                            new SqlParameter("@CouponID", couponID),
                                            new SqlParameter("@categoryId", categoryId));
        }

        public static List<int> GetCategoriesIDsByCoupon(int couponID)
        {
            List<int> list = SQLDataAccess.ExecuteReadList<int>("Select CategoryID from [Catalog].[CouponCategories] where couponID=@couponID",
                                                           CommandType.Text,
                                                           reader => SQLDataHelper.GetInt(reader, "CategoryID"),
                                                           new SqlParameter("@CouponID", couponID));
            return list;
        }

        public static void DeletecategoriesFromCoupon(int couponID, int categoryID)
        {
            SQLDataAccess.ExecuteNonQuery("Delete from [Catalog].[CouponCategories] Where couponID=@couponID and categoryID=@categoryID",
                                            CommandType.Text,
                                            new SqlParameter("@CouponID", couponID),
                                            new SqlParameter("@categoryID", categoryID));
        }

        public static void DeleteAllCategoriesFromCoupon(int couponID)
        {
            SQLDataAccess.ExecuteNonQuery("Delete from [Catalog].[CouponCategories] Where couponID=@couponID",
                                            CommandType.Text,
                                            new SqlParameter("@CouponID", couponID));
        }

        #endregion
        
        #region CustomerCoupon

        public static Coupon GetCustomerCoupon()
        {
            return GetCustomerCoupon(CustomerContext.CustomerId);
        }

        public static Coupon GetCustomerCoupon(Guid customerId)
        {
            var coupon = SQLDataAccess.ExecuteReadOne(
                "Select * From Catalog.Coupon Where CouponID = (Select CouponID From Customers.CustomerCoupon Where CustomerID = @CustomerID)",
                CommandType.Text, 
                GetFromReader, 
                new SqlParameter("@CustomerID", customerId));

            if (coupon == null)
                return null;

            if ((coupon.ExpirationDate == null || coupon.ExpirationDate > DateTime.Now) && (coupon.PossibleUses == 0 || coupon.PossibleUses > coupon.ActualUses))
                return coupon;

            DeleteCustomerCoupon(coupon.CouponID, customerId);
            return null;
        }

        public static void DeleteCustomerCoupon(int couponId)
        {
            DeleteCustomerCoupon(couponId, CustomerContext.CustomerId);
        }

        public static void DeleteCustomerCoupon(int couponId, Guid customerId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Delete From Customers.CustomerCoupon Where CouponID = @CouponID and CustomerID = @CustomerID",
                CommandType.Text, 
                new SqlParameter("@CustomerID", customerId), 
                new SqlParameter("@CouponID", couponId));
        }

        public static void AddCustomerCoupon(int couponId)
        {
            if (!IsCustomerHaveThisCupon(couponId, CustomerContext.CustomerId))
                AddCustomerCoupon(couponId, CustomerContext.CustomerId);
        }

        public static void AddCustomerCoupon(int couponId, Guid customerId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "INSERT INTO Customers.CustomerCoupon (CustomerID, CouponID) VALUES (@CustomerID, @CouponID)",
                 CommandType.Text,
                 new SqlParameter("@CustomerID", customerId),
                 new SqlParameter("@CouponID", couponId));
        }

        private static bool IsCustomerHaveThisCupon(int couponId, Guid customerId)
        {
            return SQLDataAccess.ExecuteScalar<int>
                ("Select Count(*) from Customers.CustomerCoupon where CustomerID=@CustomerID and CouponID=@CouponID",
                 CommandType.Text,
                 new SqlParameter("@CustomerID", customerId),
                 new SqlParameter("@CouponID", couponId)
                ) > 0;
        }

        #endregion

        public static string GenerateCouponCode()
        {
            var code = "";
            while (string.IsNullOrEmpty(code) || IsExistCouponCode(code) || GiftCertificateService.IsExistCertificateCode(code))
            {
                code = @"CP-" + Strings.GetRandomString(8);
            }
            return code;
        }

        public static bool IsExistCouponCode(string code)
        {
            return
                SQLDataHelper.GetInt(
                    SQLDataAccess.ExecuteScalar("Select COUNT(CouponID) From Catalog.Coupon Where Code = @Code",
                        CommandType.Text,
                        new SqlParameter("@Code", code))) > 0;
        }

        public static bool IsCouponAppliedToProduct(int couponId, int productId)
        {
            return
                SQLDataHelper.GetInt(
                    SQLDataAccess.ExecuteScalar("Catalog.sp_IsCouponAppliedToProduct", 
                        CommandType.StoredProcedure,
                        new SqlParameter("@CouponID", couponId),
                        new SqlParameter("@productId", productId))) > 0;
        }

        public static void SetCouponActivity(int couponId, bool active)
        {
            SQLDataAccess.ExecuteNonQuery("Update Catalog.Coupon Set Enabled = @Enabled Where CouponID = @CouponID",
                                         CommandType.Text,
                                          new SqlParameter("@CouponID", couponId),
                                          new SqlParameter("@Enabled", active));
        }
    }
}