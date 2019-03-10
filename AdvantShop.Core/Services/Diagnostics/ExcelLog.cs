//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Diagnostics
{
    public class ExcelLog
    {
        #region ImportLogMessageType enum

        public enum ImportLogMessageType
        {
            ProductAdded,
            ProductUpdated,
            InvalidData,
            SuccessImport,
            ImportedWithErrors
        }

        #endregion

        public static void DeleteLog()
        {
            SQLDataAccess.ExecuteNonQuery("DELETE [Catalog].[ImportLog]", CommandType.Text);
        }

        public static bool LogInvalidData(string value, int column, long row)
        {
            SQLDataAccess.ExecuteNonQuery("INSERT INTO [Catalog].[ImportLog] (message, mtype) VALUES (@message, @mtype)",
                                            CommandType.Text,
                                            new SqlParameter("@mtype", SQLDataHelper.GetInt(ImportLogMessageType.InvalidData)),
                                            new SqlParameter("@message", value == null ? string.Format("Value at cell [{0}; {1}] ([column, row]) cannot be empty", column + 1, row + 1)
                                                                                       : string.Format("Invalid value ({0}) at cell [{1}; {2}] ([column, row])", value, column + 1, row + 1))
                                            );
            return true;
        }

        public static void Log(string message, ImportLogMessageType type)
        {
            SQLDataAccess.ExecuteNonQuery("INSERT INTO [Catalog].[ImportLog] (message, mtype) VALUES (@message, @mtype)",
                                            CommandType.Text,
                                            new SqlParameter("@mtype", (int)type),
                                            new SqlParameter("@message", message));
        }
    }
}