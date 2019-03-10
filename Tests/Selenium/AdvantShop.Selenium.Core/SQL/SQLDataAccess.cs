//--------------------------------------------------
// Project: AdvantShop.NET (AVERA)
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace AdvantShop.Selenium.Core.SQL
{
    public class SQLDataAccess2
    {
        public static IEnumerable<T> ExecuteReadIEnumerable<T>(string sql, object obj = null, CommandType? commandType = null, string connectionString = null)
        {
            using (var connection = ConnectionFactory.CreateConnection(connectionString))
            {
                return connection.Query<T>(sql, obj, commandType: commandType);
            }
        }

        public static T Query<T>(string sql, object obj = null, CommandType? commandType = null, string connectionString = null)
        {
            using (var connection = ConnectionFactory.CreateConnection(connectionString))
            {
                return connection.Query<T>(sql, obj, commandType: commandType).FirstOrDefault();
            }
        }

        public static T ExecuteScalar<T>(string sql, object obj = null, CommandType? commandType = null, string connectionString = null)
        {
            using (var connection = ConnectionFactory.CreateConnection(connectionString))
            {
                return connection.ExecuteScalar<T>(sql, obj, commandType: commandType);
            }
        }

        public static void ExecuteNonQuery(string sql, object obj = null, CommandType? commandType = null, string connectionString = null)
        {
            using (var connection = ConnectionFactory.CreateConnection(connectionString))
            {
                connection.Execute(sql, obj, commandType: commandType);
            }
        }

    }
}