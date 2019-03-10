using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Services.Localization
{
    public static class LanguageService
    {
        public static List<Language> GetList()
        {
            return SQLDataAccess.Query<Language>("Select * From [Settings].[Language]").ToList();
        }

        public static Language GetLanguage(string cultureName)
        {
            return SQLDataAccess.ExecuteReadOne("Select * From [Settings].[Language] Where LanguageCode=@LanguageCode",
                CommandType.Text, GetFromReader, new SqlParameter("@LanguageCode", cultureName));
        }

        public static Language GetLanguage(int languageId)
        {
            return SQLDataAccess.ExecuteReadOne("Select * From [Settings].[Language] Where LanguageId=@languageId",
                CommandType.Text, GetFromReader, new SqlParameter("@languageId", languageId));
        }

        private static Language GetFromReader(SqlDataReader reader)
        {
            return new Language()
            {
                LanguageId = SQLDataHelper.GetInt(reader, "LanguageId"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                LanguageCode = SQLDataHelper.GetString(reader, "LanguageCode")
            };
        }

        public static int Add(Language language)
        {
            return
                Convert.ToInt32(
                    SQLDataAccess.ExecuteScalar(
                        "Insert Into [Settings].[Language] ([Name],[LanguageCode]) Values (@Name, @LanguageCode); Select scope_identity();",
                        CommandType.Text,
                        new SqlParameter("@Name", language.Name),
                        new SqlParameter("@LanguageCode", language.LanguageCode)));
        }

        public static void Update(Language language)
        {
            SQLDataAccess.ExecuteNonQuery("Update [Settings].[Language] Set [Name] = @Name, [LanguageCode] = @LanguageCode Where LanguageId=@LanguageId",
                CommandType.Text,
                new SqlParameter("@LanguageId", language.LanguageId),
                new SqlParameter("@Name", language.Name),
                new SqlParameter("@LanguageCode", language.LanguageCode));
        }

        public static void Delete(int languageId)
        {
            SQLDataAccess.ExecuteNonQuery("Delete From [Settings].[Language] Where LanguageId=@LanguageId",
                CommandType.Text,
                new SqlParameter("@LanguageId", languageId));
        }
    }
}
