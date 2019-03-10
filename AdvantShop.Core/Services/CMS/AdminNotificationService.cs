using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using Newtonsoft.Json;

namespace AdvantShop.CMS
{
    public class AdminNotificationService
    {
        private const int NotificationsCount = 4;

        private static AdminNotification GetAdminNotificationFromReader(SqlDataReader reader)
        {
            return new AdminNotification
            {
                Id = SQLDataHelper.GetInt(reader, "Id"),
                DateCreated = SQLDataHelper.GetDateTime(reader, "DateCreated"),
                Tag = SQLDataHelper.GetString(reader, "Tag"),
                Type = SQLDataHelper.GetString(reader, "Type").TryParseEnum<AdminNotificationType>(),
                Title = SQLDataHelper.GetString(reader, "Title"),
                Body = SQLDataHelper.GetString(reader, "Body"),
                IconPath = SQLDataHelper.GetString(reader, "IconPath"),
                Data = JsonConvert.DeserializeObject(SQLDataHelper.GetString(reader, "Data"))
            };
        }

        public static List<AdminNotification> GetMissedAdminNotifications(Guid customerId)
        {
            return SQLDataAccess.ExecuteReadList<AdminNotification>(
                "SELECT TOP(@Count) AdminNotification.* FROM CMS.AdminNotification " +
                "INNER JOIN Customers.AdminNotifications ON AdminNotifications.AdminNotificationId = AdminNotification.Id " +
                "WHERE CustomerId = @CustomerId ORDER BY DateCreated DESC", CommandType.Text, GetAdminNotificationFromReader,
                new SqlParameter("@Count", NotificationsCount),
                new SqlParameter("@CustomerId", customerId));
        }

        public static int AddAdminNotification(AdminNotification notification, Guid customerId)
        {
            var notificationId = SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO CMS.AdminNotification (DateCreated, Tag, Type, Title, Body, IconPath, Data) " +
                "VALUES (@DateCreated, @Tag, @Type, @Title, @Body, @IconPath, @Data); SELECT SCOPE_IDENTITY();",
                CommandType.Text,
                new SqlParameter("@DateCreated", notification.DateCreated),
                new SqlParameter("@Tag", notification.Tag ?? (object)DBNull.Value),
                new SqlParameter("@Type", notification.Type.ToString()),
                new SqlParameter("@Title", notification.Title),
                new SqlParameter("@Body", notification.Body ?? (object)DBNull.Value),
                new SqlParameter("@IconPath", notification.IconPath ?? (object)DBNull.Value),
                new SqlParameter("@Data", notification.Data != null ? JsonConvert.SerializeObject(notification.Data) : (object)DBNull.Value));

            SQLDataAccess.ExecuteNonQuery(
                "INSERT INTO Customers.AdminNotifications (AdminNotificationId, CustomerId) VALUES (@AdminNotificationId, @CustomerId)",
                CommandType.Text, new SqlParameter("@AdminNotificationId", notificationId), new SqlParameter("@CustomerId", customerId));
            return notificationId;
        }

        public static void DeleteCustomerAdminNotifications(Guid customerId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM Customers.AdminNotifications WHERE CustomerId = @CustomerId", CommandType.Text,
                new SqlParameter("@CustomerId", customerId));
            CleanUpAdminNotifications();
        }

        private static void CleanUpAdminNotifications()
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM CMS.AdminNotification WHERE (SELECT COUNT(*) FROM Customers.AdminNotifications WHERE AdminNotificationId = AdminNotification.Id) = 0",
                CommandType.Text);
        }

        public static void ClearExpiredAdminNotifications()
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM CMS.AdminNotification WHERE DateCreated < @Date",
                CommandType.Text, new SqlParameter("@Date", DateTime.Now.AddMonths(-1)));
        }
    }
}