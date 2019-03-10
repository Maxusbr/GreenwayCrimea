using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvantShop.Core.Caching;
using AdvantShop.Core.SQL;
using AdvantShop.Orders;

namespace AdvantShop.Core.Services.Orders
{
    public static class OrderPriceDiscountService
    {
        public static OrderPriceDiscount Get(int id)
        {
            return
                SQLDataAccess.Query<OrderPriceDiscount>(
                    "Select * from [Order].[OrderPriceDiscount] Where OrderPriceDiscountID = @id", new {id = id})
                    .FirstOrDefault();
        }

        public static int Add(OrderPriceDiscount orderPriceDiscount)
        {
            orderPriceDiscount.OrderPriceDiscountId =
                SQLDataAccess.ExecuteScalar<int>(
                    "INSERT INTO [Order].[OrderPriceDiscount] (PriceRange, PercentDiscount) VALUES (@PriceRange, @PercentDiscount); Select scope_identity();",
                    CommandType.Text,
                    new SqlParameter("@PriceRange", orderPriceDiscount.PriceRange),
                    new SqlParameter("@PercentDiscount", orderPriceDiscount.PercentDiscount));
            
            CacheManager.Remove(CacheNames.GetOrderPriceDiscountCacheObjectName());

            return orderPriceDiscount.OrderPriceDiscountId;
        }

        public static void Update(OrderPriceDiscount orderPriceDiscount)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Order].[OrderPriceDiscount] SET PriceRange = @PriceRange, PercentDiscount = @PercentDiscount WHERE (OrderPriceDiscountID = @OrderPriceDiscountID)",
                CommandType.Text,
                new SqlParameter("@PriceRange", orderPriceDiscount.PriceRange),
                new SqlParameter("@PercentDiscount", orderPriceDiscount.PercentDiscount),
                new SqlParameter("@OrderPriceDiscountID", orderPriceDiscount.OrderPriceDiscountId));

            CacheManager.Remove(CacheNames.GetOrderPriceDiscountCacheObjectName());
        }

        public static void Delete(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Delete From [Order].[OrderPriceDiscount] WHERE OrderPriceDiscountID = @OrderPriceDiscountID",
                CommandType.Text,
                new SqlParameter("@OrderPriceDiscountID", id));

            CacheManager.Remove(CacheNames.GetOrderPriceDiscountCacheObjectName());
        }

    }
}
