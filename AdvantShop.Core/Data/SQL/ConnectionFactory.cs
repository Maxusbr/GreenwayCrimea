using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace AdvantShop.Core.SQL
{
    public static class ConnectionFactory
    {
        public static IDbConnection CreateConnection(string connectionString = null) // string connectionString = "AdvantConnectionString"
        {
            //var localConnectionString = ConfigurationManager.ConnectionStrings[connectionString];
            //var factory = DbProviderFactories.GetFactory(localConnectionString.ProviderName);
            //var cnn = factory.CreateConnection();
            //if (cnn == null)
            //{
            //    throw new Exception("Can't create connection");
            //}
            //cnn.ConnectionString = localConnectionString.ConnectionString;

            IDbConnection cnn = new SqlConnection(connectionString ?? Connection.GetConnectionString());
            cnn.Open();

            return cnn;
        }

        public static IDbDataAdapter CreateAdaptern(string connectionString = "AdvantConnectionString")
        {
            var localConnectionString = ConfigurationManager.ConnectionStrings[connectionString];
            var factory = DbProviderFactories.GetFactory(localConnectionString.ProviderName);
            return factory.CreateDataAdapter();
        }
    }
}