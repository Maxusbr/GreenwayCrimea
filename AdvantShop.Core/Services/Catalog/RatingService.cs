//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Helpers;

namespace AdvantShop.Catalog
{
    public class RatingService
    {
        public static float Vote(int productID, int myRating)
        {
            var customerID = CustomerContext.CurrentCustomer.Id;

            if (DoesUserVote(productID, customerID))
            {
                return 0;
            }


            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_AddRatio]", CommandType.StoredProcedure,
                                          new SqlParameter() { ParameterName = "@ProductID", Value = productID },
                                          new SqlParameter() { ParameterName = "@ProductRatio", Value = myRating },
                                          new SqlParameter() { ParameterName = "@CustomerId", Value = customerID });

            int newRating = SQLDataHelper.GetInt(SQLDataAccess.ExecuteScalar("[Catalog].[sp_GetAVGRatioByProductID]", CommandType.StoredProcedure,
                                                             new SqlParameter() { ParameterName = "@ProductID", Value = productID }));


            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateProductRatio]", CommandType.StoredProcedure,
                                          new SqlParameter() { ParameterName = "@ProductID", Value = productID },
                                          new SqlParameter() { ParameterName = "@Ratio", Value = newRating });

            return newRating;
        }

        public static bool DoesUserVote(int productID, Guid customerID)
        {
            return SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_GetCOUNT_Ratio]", CommandType.StoredProcedure,
                                                                    new SqlParameter() { ParameterName = "@CustomerId", Value = customerID },
                                                                    new SqlParameter() { ParameterName = "@ProductID", Value = productID }) > 0;
        }
    }
}
