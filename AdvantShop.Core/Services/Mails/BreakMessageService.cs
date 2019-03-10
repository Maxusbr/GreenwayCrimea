//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Mails
{
    public class BreakMessageService
    {
        public static BreakMessage GetBreakMessageById(int id)
        {
            var breakMessage = SQLDataAccess.ExecuteReadOne<BreakMessage>("SELECT * FROM [dbo].[BreakMessage] WHERE ID = @ID", CommandType.Text,
                                                                                   GetBreakMessageFromReader, new SqlParameter("@ID", id));
            return breakMessage;
        }

        public static List<BreakMessage> GetBreakMessagesByMessageID(int messageID)
        {
            List<BreakMessage> breakMessages = SQLDataAccess.ExecuteReadList<BreakMessage>("SELECT * FROM [dbo].[BreakMessage] WHERE messageID = @messageID", CommandType.Text,
                                                                             GetBreakMessageFromReader, new SqlParameter("@messageID", messageID));
            return breakMessages;
        }

        public static List<BreakMessage> GetBreakMessages()
        {
            List<BreakMessage> breakMessages = SQLDataAccess.ExecuteReadList<BreakMessage>("SELECT * FROM [dbo].[BreakMessage", CommandType.Text,
                                                                                         GetBreakMessageFromReader);
            return breakMessages;
        }

        private static BreakMessage GetBreakMessageFromReader(SqlDataReader reader)
        {
            return new BreakMessage
            {
                ID = SQLDataHelper.GetInt(reader, "ID"),
                MessageID = SQLDataHelper.GetInt(reader, "MessageID"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                Email = SQLDataHelper.GetString(reader, "Email")
            };
        }

        public static void AddBreakMessages(List<BreakMessage> messages)
        {
            foreach (var breakMessage in messages)
            {
                AddBreakMessage(breakMessage);
            }
        }

        public static void AddBreakMessage(BreakMessage breakMessage)
        {
            breakMessage.ID = SQLDataAccess.ExecuteScalar<int>("INSERT INTO [dbo].[BreakMessage] ([MessageID], [Name], [Email]) VALUES (@MessageID, @Name, @Email); SELECT SCOPE_IDENTITY();",
                                                   CommandType.Text,
                                                   new SqlParameter("@MessageID", breakMessage.MessageID),
                                                   new SqlParameter("@Name", breakMessage.Name),
                                                   new SqlParameter("@Email", breakMessage.Email)
                                                   );
        }

        public static void UpdateBreakMessage(BreakMessage breakMessage)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [dbo].[BreakMessage] SET MessageID = @MessageID, Name = @Name, Email = @Email WHERE ID = @ID",
                                            CommandType.Text,
                                            new SqlParameter("@MessageID", breakMessage.MessageID),
                                            new SqlParameter("@Name", breakMessage.Name),
                                            new SqlParameter("@Email", breakMessage.Email),
                                            new SqlParameter("@ID", breakMessage.ID)
                                            );
        }

        public static void DeleteBreakMessage(int id)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [dbo].[BreakMessage] WHERE ID = @ID", CommandType.Text, new SqlParameter("@ID", id));
        }

        public static int InsertMesageInDB(string title, string text)
        {
            var messageId = SQLDataAccess.ExecuteScalar<int>("INSERT INTO [dbo].[MessageLog] ([AddDate], [Title], [MessageText]) VALUES (GETDATE(), @Title, @MessageText); SELECT SCOPE_IDENTITY();",
                                                             CommandType.Text, new SqlParameter("@Title", title), new SqlParameter("@MessageText", text));
            return messageId;
        }

        public static List<BreakMessage> GetEmailsToAllUsers()
        {
            var message = GetEmailsToRegUsers();
            message.AddRange(GetEmailsToUnregUsers());
            return message;
        }

        public static List<BreakMessage> GetEmailsToRegUsers()
        {
            List<BreakMessage> messsages = SQLDataAccess.ExecuteReadList("SELECT [FirstName], [LastName], [Email] FROM [Customers].[Customer] WHERE [Subscribed4News] =\'True\'",
                                                           CommandType.Text,
                                                           reader => new BreakMessage
                                                                         {
                                                                             MessageID = 0,
                                                                             Email = (string)reader["Email"],
                                                                             Name = reader["FirstName"] + " " + reader["LastName"]
                                                                         });
            return messsages;
        }

        public static List<BreakMessage> GetEmailsToUnregUsers()
        {
            List<BreakMessage> messsages = SQLDataAccess.ExecuteReadList("SELECT [Email] FROM Subscribe WHERE Enable = \'True\'",
                                                           CommandType.Text,
                                                           reader => new BreakMessage
                                                                         {
                                                                             MessageID = 0,
                                                                             Email = (string)reader["Email"],
                                                                             Name = (string)reader["Email"]
                                                                         });
            return messsages;
        }



        public static string GetMessageText(int messageId)
        {
            var text = SQLDataAccess.ExecuteScalar<string>("SELECT MessageText FROM [dbo].[MessageLog] WHERE ID = @messageId",
                                                              CommandType.Text,
                                                              new SqlParameter("@messageId", messageId));
            return text;
        }

        public static string GetMessageTitle(int messageId)
        {
            var title = SQLDataAccess.ExecuteScalar<string>("SELECT Title FROM [dbo].[MessageLog] WHERE ID = @messageId",
                                                            CommandType.Text, new SqlParameter("@messageId", messageId));
            return title;
        }
    }
}