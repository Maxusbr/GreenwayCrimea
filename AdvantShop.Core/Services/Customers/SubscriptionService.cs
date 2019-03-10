//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Mails;
using AdvantShop.Core.SQL;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace AdvantShop.Customers
{
    public class SubscriptionService
    {
        public static void AddSubscription(Subscription subcription)
        {
            SQLDataAccess.ExecuteNonQuery("INSERT INTO [Customers].[Subscription] (Email,Subscribe,SubscribeDate,UnsubscribeDate,UnsubscribeReason) VALUES (@Email,@Subscribe,GETDATE(),NULL,@UnsubscribeReason)",
              CommandType.Text,
              new SqlParameter("@Email", subcription.Email),
              new SqlParameter("@Subscribe", subcription.Subscribe),
              new SqlParameter("@UnsubscribeReason", subcription.UnsubscribeReason));
        }

        public static void UpdateSubscription(Subscription subcription)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Customers].[Subscription] SET Email=@Email, Subscribe=@Subscribe, UnsubscribeReason=@UnsubscribeReason WHERE [Id]=@Id",
              CommandType.Text,
              new SqlParameter("@Email", subcription.Email),
              new SqlParameter("@Subscribe", subcription.Subscribe),
              new SqlParameter("@UnsubscribeReason", subcription.UnsubscribeReason),
              new SqlParameter("@Id", subcription.Id));

        }

        public static bool IsExistsSubscription(string email)
        {
            return SQLDataAccess.ExecuteScalar<bool>("IF(SELECT Count(Id) FROM [Customers].[Subscription] WHERE [Email] = @Email) > 0 BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END",
                CommandType.Text,
                new SqlParameter("@Email", email));
        }

        public static bool IsSubscribe(string email)
        {
            return SQLDataAccess.ExecuteScalar<bool>("IF(SELECT COUNT(Id) FROM [Customers].[Subscription] WHERE [Email] = @Email AND [Subscribe] = 1) > 0 BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END",
                CommandType.Text,
                new SqlParameter("@Email", email));
        }

        public static void Subscribe(string email)
        {
            if (email.IsNullOrEmpty())
                return;

            SQLDataAccess.ExecuteNonQuery(
                "IF (SELECT COUNT(Id) FROM [Customers].[Subscription] WHERE [Email] = @Email) > 0" +
                "BEGIN UPDATE [Customers].[Subscription] SET [Subscribe] = 1, [SubscribeDate] = GETDATE() WHERE [Email] = @Email END " +
                "ELSE " +
                "BEGIN INSERT INTO [Customers].[Subscription] ([Email],[Subscribe],[SubscribeDate],[UnsubscribeDate],[UnsubscribeReason]) VALUES (@Email,1,GETDATE(),NULL,NULL) END",
                CommandType.Text,
                new SqlParameter("@Email", email));

            var customer = CustomerService.GetCustomerByEmail(email);
            var subscription = new Subscription { Email = email, CustomerType = EMailRecipientType.Subscriber };
            if (customer != null)
            {
                subscription.FirstName = customer.FirstName;
                subscription.LastName = customer.LastName;
                subscription.Phone = customer.StandardPhone.ToString();
                subscription.CustomerType = EMailRecipientType.Subscriber;
            }

            ModulesExecuter.Subscribe(subscription);
        }

        public static void Subscribe(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Customers].[Subscription] SET [Subscribe] = 1, [SubscribeDate] = GETDATE(), [UnsubscribeDate] = NULL WHERE [Id] = @Id",
                CommandType.Text,
                new SqlParameter("@Id", id));
            //todo: лишние запросы получения кастомера и подписки <Sckeef>
            var subscription = GetSubscription(id);
            var customer = CustomerService.GetCustomerByEmail(subscription.Email);
            subscription = new Subscription
            {
                Email = subscription.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.StandardPhone.ToString(),
                CustomerType = EMailRecipientType.Subscriber
            };

            ModulesExecuter.Subscribe(subscription);
        }

        public static void Unsubscribe(string email, string unsubscribeReason)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Customers].[Subscription] SET [Subscribe] = 0, [UnsubscribeDate] = GETDATE(), [UnsubscribeReason] = @UnsubscribeReason WHERE [Email] = @Email",
                CommandType.Text,
                new SqlParameter("@Email", email),
                new SqlParameter("@UnsubscribeReason", string.IsNullOrEmpty(unsubscribeReason) ? (object)DBNull.Value : unsubscribeReason));

            ModulesExecuter.UnSubscribe(email);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        public static void Unsubscribe(string email)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Customers].[Subscription] SET [Subscribe] = 0, [UnsubscribeDate] = GETDATE() WHERE [Email] = @Email",
                CommandType.Text,
                new SqlParameter("@Email", email));

            ModulesExecuter.UnSubscribe(email);
        }

        public static void Unsubscribe(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Customers].[Subscription] SET [Subscribe] = 0, [UnsubscribeDate] = GETDATE() WHERE [Id] = @Id",
                CommandType.Text,
                new SqlParameter("@Id", id));

            var subscription = GetSubscription(id);

            ModulesExecuter.UnSubscribe(subscription.Email);
        }

        public static List<Subscription> GetSubscriptions()
        {
            return SQLDataAccess.ExecuteReadList<Subscription>(
                "SELECT * FROM [Customers].[Subscription]",
                CommandType.Text,
                GetSubscriptionFromReader);
        }

        public static Subscription GetSubscription(int id)
        {
            return SQLDataAccess.ExecuteReadOne<Subscription>(
                "SELECT * FROM [Customers].[Subscription] WHERE Id = @Id",
                CommandType.Text,
                GetSubscriptionFromReader,
                new SqlParameter("@Id", id));
        }

        public static Subscription GetSubscription(string email)
        {
            return SQLDataAccess.ExecuteReadOne<Subscription>(
                "SELECT * FROM [Customers].[Subscription] WHERE [Email] = @Email",
                CommandType.Text,
                GetSubscriptionFromReader,
                new SqlParameter("@Email", email));
        }

        public static List<ISubscriber> GetSubscribedEmails()
        {
            return SQLDataAccess.ExecuteReadList<ISubscriber>(
              "SELECT [Subscription].[Email], [Customer].[FirstName], [Customer].[LastName], [Customer].[Phone] FROM [Customers].[Subscription] LEFT JOIN [Customers].[Customer] ON [Subscription].[Email] = [Customer].[Email] WHERE [Subscribe] = 1",
              CommandType.Text,
              (reader) =>
              {
                  return new Subscription
                  {
                      Email = SQLDataHelper.GetString(reader, "Email"),
                      FirstName = SQLDataHelper.GetString(reader, "FirstName"),
                      LastName = SQLDataHelper.GetString(reader, "LastName"),
                      Phone = SQLDataHelper.GetString(reader, "Phone"),
                  };
              });
        }

        public static void DeleteSubscription(int id)
        {
            var email = SQLDataAccess.ExecuteScalar<string>(
                "Select email from  [Customers].[Subscription] WHERE [Id] = @Id; DELETE FROM [Customers].[Subscription] WHERE [Id] = @Id",
                CommandType.Text,
                new SqlParameter("@Id", id));

            ModulesExecuter.UnSubscribe(email);
        }

        public static void DeleteSubscription(string email)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM [Customers].[Subscription] WHERE [Email] = @Email",
                CommandType.Text,
                new SqlParameter("@Email", email));

            ModulesExecuter.UnSubscribe(email);
        }

        public static Subscription GetSubscriptionFromReader(SqlDataReader reader)
        {
            return new Subscription
            {
                Id = SQLDataHelper.GetInt(reader, "Id"),
                Email = SQLDataHelper.GetString(reader, "Email"),
                Subscribe = SQLDataHelper.GetBoolean(reader, "Subscribe"),
                SubscribeDate = SQLDataHelper.GetDateTime(reader, "SubscribeDate"),
                UnsubscribeDate = SQLDataHelper.GetDateTime(reader, "UnsubscribeDate"),
                UnsubscribeReason = SQLDataHelper.GetString(reader, "UnsubscribeReason")
            };
        }
    }
}