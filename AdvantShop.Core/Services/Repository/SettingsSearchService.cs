//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using System.Collections.Generic;
using System;

namespace AdvantShop.Repository
{
    public class SettingsSearchService
    {

        #region Get /  Add / Update / Delete 

        public static SettingsSearch GetSettingsSearch(int id)
        {
            if (id == 0)
                return null;

            return SQLDataAccess.ExecuteReadOne("SELECT * FROM [Settings].[SettingsSearch] WHERE [Id] = @Id",
                CommandType.Text, SettingsSearchFromReader, new SqlParameter("@Id", id));
        }

        public static List<SettingsSearch> GetSettingsSearchForAutocomplete(string query)
        {
            var translitKeyboard = StringHelper.TranslitToRusKeyboard(query);

            return SQLDataAccess.ExecuteReadList<SettingsSearch>(
                "(SELECT *, 0 as weight FROM [Settings].[SettingsSearch] " +
                "WHERE [Title] LIKE '%' + @q + '%' OR [Title] like '%' + @qtr + '%') " +
                " Union " +
                "(SELECT *, 1 as weight FROM [Settings].[SettingsSearch]" +
                "WHERE ([KeyWords] LIKE '%' + @q + '%' OR [KeyWords] like '%' + @qtr + '%') " +
                " and Id not in (SELECT Id FROM [Settings].[SettingsSearch] WHERE [Title] LIKE '%' + @q + '%' OR [Title] LIKE '%' + @qtr + '%') " +
                ") " +
                "Order by weight, SortOrder",
                CommandType.Text, SettingsSearchFromReader, new SqlParameter("@q", query), new SqlParameter("@qtr", translitKeyboard));
        }


        public static SettingsSearch SettingsSearchFromReader(SqlDataReader reader)
        {
            return new SettingsSearch
            {
                Id = SQLDataHelper.GetInt(reader, "Id"),
                Title = SQLDataHelper.GetString(reader, "Title"),
                Link = SQLDataHelper.GetString(reader, "Link"),
                KeyWords = SQLDataHelper.GetString(reader, "KeyWords"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
            };
        }

        public static void Update(SettingsSearch settingsSearch)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [Settings].[SettingsSearch] set Title=@Title, Link=@Link, KeyWords=@KeyWords, SortOrder=@SortOrder where ID = @id",
                CommandType.Text,
                new SqlParameter("@Id", settingsSearch.Id),
                new SqlParameter("@Title", settingsSearch.Title),
                new SqlParameter("@Link", settingsSearch.Link),
                new SqlParameter("@KeyWords", settingsSearch.KeyWords ?? (object)DBNull.Value),
                new SqlParameter("@SortOrder", settingsSearch.SortOrder));
        }

        public static void Add(SettingsSearch settingsSearch)
        {
            settingsSearch.Id =
                SQLDataAccess.ExecuteScalar<int>(
                    "Insert into [Settings].[SettingsSearch] (Title, Link, KeyWords, SortOrder) Values (@Title, @Link, @KeyWords, @SortOrder);SELECT scope_identity();",
                    CommandType.Text,
                    new SqlParameter("@Title", settingsSearch.Title),
                    new SqlParameter("@Link", settingsSearch.Link),
                    new SqlParameter("@KeyWords", settingsSearch.KeyWords ?? (object)DBNull.Value),
                    new SqlParameter("@SortOrder", settingsSearch.SortOrder));

        }

        public static void Delete(int id)
        {
            SQLDataAccess.ExecuteNonQuery("Delete from Settings.SettingsSearch Where ID=@ID", CommandType.Text, new SqlParameter("@ID", id));
        }

        #endregion


    }
}