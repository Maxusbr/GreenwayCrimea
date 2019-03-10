using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.SQL;

namespace AdvantShop.Core.Services.Vk
{
    public static class VkService
    {
        private const string VkCacheKeyCustomerId = "_Vk_User_";

        public static VkUser GetUser(long userId)
        {
            return CacheManager.Get(CacheNames.Customer + VkCacheKeyCustomerId + userId, 20,
                () =>
                    SQLDataAccess.Query<VkUser>("Select * From [Customers].[VkUser] Where Id=@id", new {id = userId})
                        .FirstOrDefault());
        }

        public static VkUser GetUser(Guid customerId)
        {
            return CacheManager.Get(CacheNames.Customer + VkCacheKeyCustomerId + "_customerId_" + customerId,
                () =>
                    SQLDataAccess.Query<VkUser>("Select * From [Customers].[VkUser] Where CustomerId=@id", new { id = customerId })
                        .FirstOrDefault());
        }


        public static void AddUser(VkUser user)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Insert Into [Customers].[VkUser] (Id, CustomerId, FirstName, LastName, BirthDate, Photo100, MobilePhone, HomePhone, Sex, ScreenName) " +
                "Values (@Id, @CustomerId, @FirstName, @LastName, @BirthDate, @Photo100, @MobilePhone, @HomePhone, @Sex, @ScreenName)",
                CommandType.Text,
                new SqlParameter("@Id", user.Id),
                new SqlParameter("@CustomerId", user.CustomerId),
                new SqlParameter("@FirstName", user.FirstName ?? (object)DBNull.Value),
                new SqlParameter("@LastName", user.LastName ?? (object)DBNull.Value),
                new SqlParameter("@BirthDate", user.BirthDate ?? (object)DBNull.Value),
                new SqlParameter("@Photo100", user.Photo100 ?? (object)DBNull.Value),
                new SqlParameter("@MobilePhone", user.MobilePhone ?? (object)DBNull.Value),
                new SqlParameter("@HomePhone", user.HomePhone ?? (object)DBNull.Value),
                new SqlParameter("@Sex", user.Sex ?? (object)DBNull.Value),
                new SqlParameter("@ScreenName", user.ScreenName ?? (object)DBNull.Value)
            );
        }
        

        /// <summary>
        /// Сообщения для пользователя из контакта
        /// </summary>
        public static List<VkUserMessage> GetCustomerMessages(Guid customerId)
        {
            var user = GetUser(customerId);
            if (user == null)
                return null;

            var userMessages =
                SQLDataAccess.Query<VkUserMessage>(
                    "Select * From Customers.VkMessage m " +
                    "Left Join Customers.VkUser u On u.Id = m.UserId " +
                    "Where m.UserId=@userId OR m.FromId=@userId " +
                    "Order by Date desc",
                    new { userId = user.Id })
                    .ToList();

            var group = SettingsVk.Group;

            foreach (var message in userMessages)
            {
                // В исходящих сообщениях FromId - автор сообщения
                if (message.FromId != null && message.FromId.Value == -group.Id)
                {
                    message.FirstName = group.Name;
                    message.LastName = null;
                    message.Photo100 = group.Photo100;
                    message.ScreenName = group.ScreenName;
                }
            }

            return userMessages;
        }

        public static void AddMessages(List<VkMessage> messages)
        {
            if (messages == null || messages.Count == 0)
                return;
            
            foreach (var message in messages.Where(x => x.MessageId != null && x.UserId != null))
            {
                AddMessage(message);
            }
        }

        public static void AddMessage(VkMessage message)
        {
            SQLDataAccess.ExecuteNonQuery(
                "If not Exists(Select 1 From [Customers].[VkMessage] Where MessageId=@MessageId and UserId=@UserId) " +
                "begin " +
                    "Insert Into [Customers].[VkMessage] (MessageId, UserId, Date, Body, ChatId, FromId, Type) Values (@MessageId, @UserId, @Date, @Body, @ChatId, @FromId, @Type) " +
                "end",
                CommandType.Text,
                new SqlParameter("@MessageId", message.MessageId ?? 0),
                new SqlParameter("@UserId", message.UserId ?? 0),
                new SqlParameter("@Date", message.Date ?? DateTime.Now),
                new SqlParameter("@Body", message.Body ?? ""),
                new SqlParameter("@ChatId", message.ChatId ?? (object)DBNull.Value),
                new SqlParameter("@FromId", message.FromId ?? (object)DBNull.Value),
                new SqlParameter("@Type", message.Type.ToString())
            );
        }
    }
}
