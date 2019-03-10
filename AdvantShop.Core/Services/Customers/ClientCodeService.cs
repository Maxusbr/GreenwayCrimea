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
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Saas;

namespace AdvantShop.Customers
{
    public class ClientCodeService
    {
        private static bool IsCodeExist(int code)
        {
            return
                SQLDataAccess.ExecuteScalar<int>(
                    "Select Count(*) From [Customers].[ClientCode] Where Code=@Code", CommandType.Text,
                    new SqlParameter("@Code", code)) > 0;
        }

        private static Guid GetUserId(int code)
        {
            return 
                SQLDataHelper.GetGuid(
                    SQLDataAccess.ExecuteScalar("Select UserId From [Customers].[ClientCode] Where Code=@Code",
                        CommandType.Text, new SqlParameter("@Code", code)));
        }

        private static void AddCode(Guid userId, int code)
        {
            SQLDataAccess.ExecuteNonQuery(
                    "Insert Into [Customers].[ClientCode] (UserId,Code,CreatedDate) Values (@UserId, @Code, GETDATE())", CommandType.Text,
                    new SqlParameter("@UserId", userId), new SqlParameter("@Code", code));
        }

        private static int GetCode(Guid userId)
        {
            return
                SQLDataAccess.ExecuteScalar<int>("Select Code From [Customers].[ClientCode] Where UserId=@UserId", CommandType.Text, 
                    new SqlParameter("@UserId", userId));
        }

        private static int GetMaxCode()
        {
            return SQLDataAccess.ExecuteScalar<int>("Select Max(Code) From [Customers].[ClientCode]", CommandType.Text);
        }

        public static void DeleteExpired(DateTime date)
        {
            SQLDataAccess.ExecuteNonQuery("Delete From [Customers].[ClientCode] Where CreatedDate < @Date", CommandType.Text, new SqlParameter("@Date", date));
        }
        
        public static int GenerateCode(Guid userId, int range = 1000000)
        {
            var rand = new Random();
            var code = rand.Next(0, range);

            for (var i = 0; i < 3; i++)
            {
                if (!IsCodeExist(code))
                {
                    AddCode(userId, code);

                    return code;
                }

                code = rand.Next(0, range);
            }

            if (range == 100000000)
            {
                Debug.Log.Error(string.Format("Can't generate code for userId: {0}, code: {1}, range: {2}", userId, code, range));

                code = GetMaxCode() + 1;
                AddCode(userId, code);

                return code;
            }

            return GenerateCode(userId, range*10);
        }

        public static int GetClientCode(Guid userId)
        {
            if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCustomerLog)
                return 0;

            var code = GetCode(userId);
            return code == 0 ? GenerateCode(userId) : code;
        }

        public static List<string> SearchCustomers(string q)
        {
            if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCustomerLog)
                return null;

            var code = q.Replace("-", "").Replace(" ", "").TryParseInt();
            if (code == 0)
                return null;

            return
                SQLDataAccess.ExecuteReadList<string>(
                    "SELECT * FROM [Customers].[Customer] " +
                    "Right Join [Customers].[ClientCode] On [ClientCode].UserId = [Customer].CustomerId " +
                    "WHERE [Code] like '%' + @q + '%'",
                    CommandType.Text,
                    reader =>
                    {
                        var codeDb = SQLDataHelper.GetInt(reader, "Code");
                        var codeStr = code.ToString("##,##0").Replace(",", "-").Replace("\u00A0", "-");

                        return
                            string.Format(
                                "<a href=\"ViewCustomer.aspx?CustomerID={0}&code={1}\">{2} {3} ID: {4}</a>\n",
                                SQLDataHelper.GetGuid(reader, "UserId"),
                                codeDb,
                                SQLDataHelper.GetString(reader, "FirstName"),
                                SQLDataHelper.GetString(reader, "LastName"),
                                codeStr);
                    },
                    new SqlParameter("@q", code.ToString()));
        }


        public static List<Customer> SearchCustomers2(string q)
        {
            if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCustomerLog)
                return new List<Customer>();

            var code = q.Replace("-", "").Replace(" ", "").TryParseInt();
            if (code == 0)
                return new List<Customer>();

            return
                SQLDataAccess.ExecuteReadList(
                    "SELECT * FROM [Customers].[Customer] " +
                    "Right Join [Customers].[ClientCode] On [ClientCode].UserId = [Customer].CustomerId " +
                    "WHERE [Code] like '%' + @q + '%'",
                    CommandType.Text,
                    reader =>
                    {
                        var codeDb = SQLDataHelper.GetInt(reader, "Code");
                        var codeStr = codeDb.ToString("##,##0").Replace(",", "-").Replace("\u00A0", "-"); // code

                        var customer = CustomerService.GetFromSqlDataReader(reader);
                        if (customer.Id == Guid.Empty)
                            customer.Id = SQLDataHelper.GetGuid(reader, "UserId");
                        customer.Code = codeStr;
                        return customer;
                    },
                    new SqlParameter("@q", code.ToString()));
        }

        public static Customer GetCustomerByCode(string code, Guid userId)
        {
            if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCustomerLog)
                return null;

            var codeInt = code.Replace("-", "").TryParseInt();
            if (codeInt == 0)
                return null;

            if (!IsCodeExist(codeInt))
                return null;

            var customer =
                SQLDataAccess.ExecuteReadOne(
                    "SELECT * FROM [Customers].[Customer] Inner Join [Customers].[ClientCode] On [ClientCode].UserId = [Customer].CustomerId WHERE [Code] = @Code",
                    CommandType.Text,
                    CustomerService.GetFromSqlDataReader,
                    new SqlParameter("@Code", codeInt.ToString()));

            return customer ?? new Customer(CustomerGroupService.DefaultCustomerGroup) {Id = userId != Guid.Empty ? userId : GetUserId(codeInt)};
        }
    }
}
