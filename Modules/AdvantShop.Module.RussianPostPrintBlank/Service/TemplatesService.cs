using AdvantShop.Core.Modules;
using AdvantShop.Module.RussianPostPrintBlank.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
//using AdvantShop.Module.RussianPostPrintBlank.Models;

namespace AdvantShop.Module.RussianPostPrintBlank.Service
{
    public class TemplatesService
    {
        public static bool CreateTemplatesTable()
        {
            if (!ModulesRepository.IsExistsModuleTable("Module", "RussianPostPrintBlank"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                    @"CREATE TABLE [Module].[RussianPostPrintBlank]
                    (
	                    [TemplateID] int NOT NULL IDENTITY (1, 1),
                        [Name] [nvarchar](100) NOT NULL,
	                    [Type] [nvarchar](50) NOT NULL,
	                    [Content] [nvarchar](MAX) NOT NULL
                    )  ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]    
                                    
                    ALTER TABLE [Module].[RussianPostPrintBlank] ADD CONSTRAINT
	                PK_RussianPostPrintBlank PRIMARY KEY CLUSTERED 
	                (
	                    [TemplateID]
	                ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]                                        
                    ALTER TABLE [Module].[RussianPostPrintBlank] SET (LOCK_ESCALATION = TABLE)
                    SET IDENTITY_INSERT [Module].[RussianPostPrintBlank] ON", CommandType.Text);

                return true;
            }

            return false;
        }

        public static bool DeleteTemplatesTable()
        {
            if (ModulesRepository.IsExistsModuleTable("Module", "RussianPostPrintBlank"))
            {
                ModulesRepository.ModuleExecuteNonQuery("DROP TABLE [Module].[RussianPostPrintBlank]", CommandType.Text);
            }

            return true;
        }

        private static Template GetTemplateFromReader(SqlDataReader reader)
        {
            return new Template
            {
                TemplateID = ModulesRepository.ConvertTo<int>(reader, "TemplateID"),
                Name = ModulesRepository.ConvertTo<string>(reader, "Name"),
                Type = (FormType)Enum.Parse(typeof(FormType), ModulesRepository.ConvertTo<string>(reader, "Type")),
                Content = ModulesRepository.ConvertTo<string>(reader, "Content")
            };
        }

        public static List<Template> GetTemplates()
        {
            return ModulesRepository.ModuleExecuteReadList("SELECT * FROM [Module].[RussianPostPrintBlank]",
                    CommandType.Text, GetTemplateFromReader);
        }
        
        public static Template GetTemplate(int templateId)
        {
            return ModulesRepository.ModuleExecuteReadOne("SELECT * FROM [Module].[RussianPostPrintBlank] WHERE [TemplateID] = @TemplateID",
                    CommandType.Text, GetTemplateFromReader, new SqlParameter("@TemplateID", templateId));
        }

        public static int AddTemplate(Template template)
        {
            return ModulesRepository.ModuleExecuteScalar<int>(
                "INSERT INTO [Module].[RussianPostPrintBlank] ([Name], [Type], [Content]) VALUES (@Name, @Type, @Content)",
                CommandType.Text,
                new SqlParameter("@Name", template.Name ?? string.Empty),
                new SqlParameter("@Type", template.Type.ToString()),
                new SqlParameter("@Content", template.Content ?? string.Empty));
        }

        public static void UpdateTemplate(Template template)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "UPDATE [Module].[RussianPostPrintBlank] SET [Name] = @Name, [Type] = @Type, [Content] = @Content WHERE [TemplateID] = @TemplateID",
                CommandType.Text,
                new SqlParameter("@TemplateID", template.TemplateID),
                new SqlParameter("@Name", template.Name ?? string.Empty),
                new SqlParameter("@Type", template.Type.ToString()),
                new SqlParameter("@Content", template.Content ?? string.Empty));
        }

        public static void DeleteTemplate(int templateId)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "DELETE FROM [Module].[RussianPostPrintBlank] WHERE [TemplateID] = @TemplateID",
                CommandType.Text,
                new SqlParameter("@TemplateID", templateId));
        }
    }
}
