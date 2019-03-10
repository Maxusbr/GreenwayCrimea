//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using AdvantShop.Core;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Statistic
{

    public class InternalServices
    {

        public static void LogApplicationRestart(bool checkPing, bool checkDbVersion)
        {

            if (checkPing)
            {
                if (!DataBaseService.PingDateBase())
                    return; // Fail connection to DB
            }

            if (checkDbVersion)
            {
                if (!DataBaseService.CheckDBVersion())
                    return; // Wrong version of DB
            }

            SQLDataAccess.ExecuteNonQuery("INSERT INTO [Internal].[AppRestartLog] ([RestartDate], [ServerName]) VALUES (@RestartDate, @ServerName);",
                                            CommandType.Text,
                                            new SqlParameter("@RestartDate", DateTime.Now),
                                            new SqlParameter("@ServerName", Dns.GetHostName()));
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Internal].[AppRestartLog] WHERE ID < (SELECT MAX(ID) - 300 FROM [Internal].[AppRestartLog]);", CommandType.Text);
        }

        public static ICollection<ApplicationLogEntry> GetAppRestartLogData()
        {
            List<ApplicationLogEntry> result = SQLDataAccess.ExecuteReadList<ApplicationLogEntry>(
                "SELECT TOP (100) * FROM [Internal].[AppRestartLog] ORDER BY [RestartDate] DESC;",
                CommandType.Text, reader => new ApplicationLogEntry
                                                {
                                                    EntryId = SQLDataHelper.GetInt(reader, "ID"),
                                                    EntryDate = SQLDataHelper.GetDateTime(reader, "RestartDate"),
                                                    ServerName = SQLDataHelper.GetString(reader, "ServerName")
                                                });
            return result;
        }

        public static void DeleteAppRestartLogData()
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Internal].[AppRestartLog];", CommandType.Text);
        }

        public static void DeleteExpiredAppRestartLogData()
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Internal].[AppRestartLog] where GetDate() > DATEADD(week, 1, RestartDate);", CommandType.Text);
        }

    }
}
