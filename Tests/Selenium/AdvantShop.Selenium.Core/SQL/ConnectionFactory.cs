using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace AdvantShop.Selenium.Core.SQL
{
    public static class ConnectionFactory
    {
        public static IDbConnection CreateConnection(string connectionString = null) 
        {

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