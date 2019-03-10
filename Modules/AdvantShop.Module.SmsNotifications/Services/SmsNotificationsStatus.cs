using AdvantShop.Core.Modules;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Module.SmsNotifications.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SmsNotifications.Status
{
    public class StatusItem
    {
        public int Status { get; set; }
        public string Content { get; set; }
        public bool Enabled { get; set; }
    }

    public class SmsNotificationsStatus
    {
        private static StatusItem GetStatusFromReader(SqlDataReader reader)
        {
            return new StatusItem
            {
                Status = SQLDataHelper.GetInt(reader, "Status"),
                Content = SQLDataHelper.GetString(reader, "Content"),
                Enabled = SQLDataHelper.GetBoolean(reader, "Enabled")
            };
        }

        public static DataTable GetDataStatus()
        {
            if (!ModulesRepository.IsExistsModuleTable("Module", "SmsNotificationsStatus"))
            {
                SmsNotificationsService.InstallModule();
            }

            return ModulesRepository.ModuleExecuteTable(
            @"SELECT SmsNotificationsStatus.Status, SmsNotificationsStatus.Content, SmsNotificationsStatus.Enabled FROM [Module].[SmsNotificationsStatus] 
	                LEFT JOIN [Order].OrderStatus ON OrderStatus.OrderStatusID = SmsNotificationsStatus.Status ORDER BY OrderStatus.SortOrder",
            CommandType.Text
            );
        }

        public static StatusItem GetStatus(int stat)
        {
            return SQLDataAccess.ExecuteReadOne<StatusItem>("Select * From [Module].[SmsNotificationsStatus] where Status=@Status", CommandType.Text,
                                                            GetStatusFromReader, new SqlParameter("@Status", stat));
        }

        public static void DeleteStatusById(int id)
        {
            var status = GetStatus(id);
            if (status == null)
                return;

            ModulesRepository.ModuleExecuteNonQuery(
                "DELETE FROM [Module].[SmsNotificationsStatus] WHERE [Status] = @ID",
                CommandType.Text,
                new SqlParameter("@ID", id));
        }

        public static void UpdateStatus(StatusItem status)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "UPDATE [Module].[SmsNotificationsStatus] SET [Content]=@Content,[Enabled]=@Enabled WHERE [Status]=@Status",
                CommandType.Text,
                new SqlParameter("@Status", status.Status),
                new SqlParameter("@Content", status.Content),
                new SqlParameter("@Enabled", status.Enabled));
        }
        public static void AddStatus(StatusItem status)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "INSERT INTO [Module].[SmsNotificationsStatus] ([Status],[Content],[Enabled]) VALUES(@Status,@Content,@Enabled)",
                CommandType.Text,
                new SqlParameter("@Status", status.Status),
                new SqlParameter("@Content", status.Content),
                new SqlParameter("@Enabled", status.Enabled));
        }
    }
}
