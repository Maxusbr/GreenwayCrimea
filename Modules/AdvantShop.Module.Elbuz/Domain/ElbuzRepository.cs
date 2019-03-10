//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.Elbuz.Domain
{
    public class ElbuzRepository
    {
        public static bool InstallElbuzModule()
        {
            if (!ModulesRepository.IsExistsModuleTable("Module", "ElbuzExceptions"))
            {
                ModulesRepository.ModuleExecuteNonQuery(@"CREATE TABLE [Module].[ElbuzExceptions](	[Exception] [nvarchar](250) NOT NULL) ON [PRIMARY]", CommandType.Text);
            }
            return ModulesRepository.IsExistsModuleTable("Module", "ElbuzExceptions");
        }

        public static List<string> GetElbuzExceptions()
        {
            return ModulesRepository.ModuleExecuteReadColumn<string>("SELECT * FROM [Module].[ElbuzExceptions]",
                                                                     CommandType.Text, "Exception");
        }

        public static void AddElbuzException(string exception)
        {
            ModulesRepository.ModuleExecuteNonQuery("IF (SELECT COUNT(Exception) FROM [Module].[ElbuzExceptions] WHERE Exception = @Exception) = 0 BEGIN INSERT INTO [Module].[ElbuzExceptions] (Exception) VALUES (@Exception) END",
                 CommandType.Text,
                 new SqlParameter("@Exception", exception));
        }

        public static void RemoveElbuzException(string exception)
        {
            ModulesRepository.ModuleExecuteNonQuery("DELETE FROM [Module].[ElbuzExceptions] Where Exception = @Exception",
                CommandType.Text,
                new SqlParameter("@Exception", exception));
        }

        //todo: очень жестоко, не надо так делать
        public static void DisableAllCategories()
        {
            ModulesRepository.ModuleExecuteNonQuery("UPDATE [Catalog].[Category] SET Enabled = 0", CommandType.Text);
        }
    }
}