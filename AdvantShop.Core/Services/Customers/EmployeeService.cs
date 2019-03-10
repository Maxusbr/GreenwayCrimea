//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.SQL;

namespace AdvantShop.Customers
{
    public class EmployeeService
    {
        public static int GetEmployeeCount()
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT Count(CustomerId) FROM [Customers].[Customer] WHERE [CustomerRole] = @CustomerRole1 OR [CustomerRole] = @CustomerRole2 AND [Enabled] = 1",
                CommandType.Text,
                new SqlParameter("@CustomerRole1", Role.Administrator),
                new SqlParameter("@CustomerRole2", Role.Moderator)
                );
        }

        public static void DeactivateEmployeeMoreThan(int activeEmployeeCount)
        {

            if (activeEmployeeCount <= 0)
                return;

            SQLDataAccess.ExecuteNonQuery(
               @"IF (Select Count(CustomerID) From [Customers].[Customer] WHERE ([CustomerRole] = @CustomerRole1 OR [CustomerRole] = @CustomerRole2) AND [Enabled] = 1) > @activeEmployeeCount
                 BEGIN
                    ;WITH employeeToDeactivate AS 
                    ( 
	                    SELECT 
	                    TOP(Select (Count(CustomerID) - @activeEmployeeCount) From [Customers].[Customer] WHERE ([CustomerRole] = @CustomerRole1 OR [CustomerRole] = @CustomerRole2) AND [Enabled] = 1) Customer.CustomerID 
	                    FROM Customers.Customer
	                    WHERE( [CustomerRole] = @CustomerRole1 OR [CustomerRole] = @CustomerRole2) AND [Enabled] = 1
                        ORDER BY CustomerRole, [Customer].RegistrationDateTime Desc
                    ) 
                    UPDATE Customers.Customer SET Enabled = 0 Where CustomerID in (Select CustomerID from employeeToDeactivate)
                END",
               CommandType.Text,
                new SqlParameter("@CustomerRole1", Role.Administrator),
                new SqlParameter("@CustomerRole2", Role.Moderator),
                new SqlParameter("@activeEmployeeCount", activeEmployeeCount));
        }
    }
}
