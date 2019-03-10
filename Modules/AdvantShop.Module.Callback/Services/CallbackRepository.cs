//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.Modules;
using AdvantShop.Mails;

namespace AdvantShop.Module.Callback.Services
{
    public class CallbackRepository
    {
        public static bool InstallCallbackModule()
        {
            ModuleSettingsProvider.SetSettingValue("email4notify", "", Callback.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("emailFormat",
                                                   "<h4>Поступил новый заказ обратного звонка</h4><br/> Имя: #NAME# <br/> Телефон: #PHONE# <br/> Комментарий: #COMMENT#",
                                                   Callback.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("emailSubject", "Заказ обратного звонка", Callback.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("windowTitle", "Обратный звонок", Callback.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("windowText",
                                                   "Укажите свое имя и номер телефона, и мы Вам обязательно перезвоним.",
                                                   Callback.ModuleStringId);

            if (!ModulesRepository.IsExistsModuleTable("Module", Callback.ModuleStringId))
            {
                ModulesRepository.ModuleExecuteNonQuery("CREATE TABLE Module." + Callback.ModuleStringId +
                                                        @"(
	                                        ID int NOT NULL IDENTITY (1, 1),
                                            Name nvarchar(100) NOT NULL,
	                                        Phone nvarchar(50) NOT NULL,
                                            DateAdded nvarchar(50) not null,
	                                        Comment nvarchar(MAX) NOT NULL,
	                                        AdminComment nvarchar(MAX) NOT NULL,
                                            Processed bit NOT NULL,
	                                        )  ON [PRIMARY]
	                                         TEXTIMAGE_ON [PRIMARY]                                        
                                           ALTER TABLE Module." + Callback.ModuleStringId + @" ADD CONSTRAINT
	                                        PK_Callback PRIMARY KEY CLUSTERED 
	                                        (
	                                        ID
	                                        ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]                                        
                                        ALTER TABLE Module." + Callback.ModuleStringId + @" SET (LOCK_ESCALATION = TABLE)
                                        SET IDENTITY_INSERT Module." + Callback.ModuleStringId + " ON", CommandType.Text);

                return ModulesRepository.IsExistsModuleTable("Module", Callback.ModuleStringId);
            }

            return true;
        }

        public static bool UninstallCallbackModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("email4notify", Callback.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("emailFormat", Callback.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("emailSubject", Callback.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("windowTitle", Callback.ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("windowText", Callback.ModuleStringId);

            if (ModulesRepository.IsExistsModuleTable("Module", Callback.ModuleStringId))
            {
                ModulesRepository.ModuleExecuteNonQuery("DROP TABLE Module." + Callback.ModuleStringId, CommandType.Text);
                return !ModulesRepository.IsExistsModuleTable("Module", Callback.ModuleStringId);
            }
            return true;
        }
        public static bool UpdateCallbackModule()
        {
            if (ModulesRepository.IsExistsModuleTable("Module", Callback.ModuleStringId))
            {
                ModulesRepository.ModuleExecuteNonQuery("Alter Table [Module].[" + Callback.ModuleStringId + "] ALTER COLUMN DateAdded datetime NULL", CommandType.Text);
            }
            return true;
        }

        public static bool IsAliveCallbackModule()
        {
            return ModulesRepository.IsExistsModuleTable("Module", Callback.ModuleStringId);
        }

        private static CallbackCustomer GetCallbackCustomerFromReader(SqlDataReader reader)
        {
            return new CallbackCustomer
                {
                    ID = ModulesRepository.ConvertTo<int>(reader, "ID"),
                    Name = ModulesRepository.ConvertTo<string>(reader, "Name"),
                    Phone = ModulesRepository.ConvertTo<string>(reader, "Phone"),
                    DateAdded = ModulesRepository.ConvertTo<DateTime>(reader, "DateAdded"),
                    Comment = ModulesRepository.ConvertTo<string>(reader, "Comment"),
                    AdminComment = ModulesRepository.ConvertTo<string>(reader, "AdminComment"),
                    Processed = ModulesRepository.ConvertTo<bool>(reader, "Processed"),
                };
        }

        public static CallbackCustomer GetCallbackCustomer(int id)
        {
            return ModulesRepository.ModuleExecuteReadOne("SELECT * FROM [Module].[" + Callback.ModuleStringId + "] Where ID=@ID",
                                                          CommandType.Text, GetCallbackCustomerFromReader,
                                                          new SqlParameter("@ID", id));
        }

        public static List<CallbackCustomer> GetCallbackCustomers()
        {
            return
                ModulesRepository.ModuleExecuteReadList(
                    "SELECT * FROM [Module].[" + Callback.ModuleStringId + "] ORDER BY [DateAdded] DESC",
                    CommandType.Text, GetCallbackCustomerFromReader);
        }

        public static void DeleteCallbackById(int id)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "DELETE FROM [Module].[" + Callback.ModuleStringId + "] WHERE [ID] = @ID",
                CommandType.Text,
                new SqlParameter("@ID", id));
        }

        public static void AddCallbackCustomer(CallbackCustomer callbackCustomer)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "INSERT INTO [Module].[" + Callback.ModuleStringId +
                "] ([Name], [Phone], [DateAdded], [Comment], [AdminComment], [Processed]) VALUES (@Name, @Phone, GETDATE(), @Comment, @AdminComment, @Processed)",
                CommandType.Text,
                new SqlParameter("@Name", callbackCustomer.Name),
                new SqlParameter("@Phone", callbackCustomer.Phone),
                new SqlParameter("@Comment", callbackCustomer.Comment ?? ""),
                new SqlParameter("@AdminComment", callbackCustomer.AdminComment ?? ""),
                new SqlParameter("@Processed", callbackCustomer.Processed));
        }

        public static void UpdateCallbackCustomer(CallbackCustomer callbackCustomer)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "update [Module].[" + Callback.ModuleStringId +
                "] set [Name]=@Name, [Phone]=@Phone, [Comment]=@Comment, [AdminComment]=@AdminComment, [Processed]=@Processed where id=@id",
                CommandType.Text,
                new SqlParameter("@id", callbackCustomer.ID),
                new SqlParameter("@Name", callbackCustomer.Name),
                new SqlParameter("@Phone", callbackCustomer.Phone),
                new SqlParameter("@Comment", callbackCustomer.Comment ?? ""),
                new SqlParameter("@AdminComment", callbackCustomer.AdminComment ?? ""),
                new SqlParameter("@Processed", callbackCustomer.Processed));
        }

        public static void SetCallbackProcessed(int id, bool state)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "UPDATE [Module].[" + Callback.ModuleStringId + "] SET [Processed]=@Processed WHERE [ID]=@ID",
                CommandType.Text,
                new SqlParameter("@ID", id),
                new SqlParameter("@Processed", state));
        }

        public static void SendEmail(CallbackCustomer callbackCustomer)
        {
            var email = ModuleSettingsProvider.GetSettingValue<string>("email4notify", Callback.ModuleStringId);
            var subject = ModuleSettingsProvider.GetSettingValue<string>("emailSubject", Callback.ModuleStringId);
            var format = ModuleSettingsProvider.GetSettingValue<string>("emailFormat", Callback.ModuleStringId);

            format =
                format.Replace("#NAME#", callbackCustomer.Name)
                      .Replace("#PHONE#", callbackCustomer.Phone)
                      .Replace("#COMMENT#", callbackCustomer.Comment);

            SendMail.SendMailNow(Guid.Empty, email, subject, format, true);
        }
    }
}