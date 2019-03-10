//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Configuration;

namespace AdvantShop
{
    public class Connection
    {
        private static string _connectionString;

        public static string GetConnectionString()
        {
            return _connectionString ??
                   (_connectionString = ConfigurationManager.ConnectionStrings["AdvantConnectionString"].ConnectionString);
        }
    }
}