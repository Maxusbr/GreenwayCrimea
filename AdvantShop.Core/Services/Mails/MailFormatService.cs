//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.SQL;

namespace AdvantShop.Mails
{
    public class MailFormatService
    {
        #region Get/Update/Delete MailFormat

        public static MailFormat Get(int mailFormatId)
        {
            return
                SQLDataAccess.Query<MailFormat>("SELECT * FROM [Settings].[MailFormat] WHERE MailFormatID = @id",
                    new {id = mailFormatId}).FirstOrDefault();
        }

        public static MailFormat GetByType(string mailType)
        {
            return
                SQLDataAccess.Query<MailFormat>(
                    "SELECT Top(1)* FROM [Settings].[MailFormat] " +
                    "Inner Join  [Settings].[MailFormatType] On [MailFormatType].[MailFormatTypeId] = [MailFormat].[MailFormatTypeId] " +
                    "WHERE MailType = @mailType And Enable=1",
                    new { mailType}).FirstOrDefault();
        }

        public static void Update(MailFormat mailFormat)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Settings].[MailFormat] SET FormatName = @FormatName, FormatSubject = @FormatSubject, FormatText = @FormatText, MailFormatTypeId = @MailFormatTypeId, SortOrder = @SortOrder, Enable = @Enable, ModifyDate = GETDATE() WHERE (MailFormatID = @MailFormatID)",
                CommandType.Text,
                new SqlParameter("@MailFormatID", mailFormat.MailFormatId),
                new SqlParameter("@FormatName", mailFormat.FormatName),
                new SqlParameter("@FormatSubject", mailFormat.FormatSubject),
                new SqlParameter("@FormatText", mailFormat.FormatText),
                new SqlParameter("@MailFormatTypeId", mailFormat.MailFormatTypeId),
                new SqlParameter("@SortOrder", mailFormat.SortOrder),
                new SqlParameter("@Enable", mailFormat.Enable));
        }

        public static int Add(MailFormat mailFormat)
        {
            mailFormat.MailFormatId =
                SQLDataAccess.ExecuteScalar<int>(
                    "INSERT INTO [Settings].[MailFormat] (FormatName, FormatSubject, FormatText, MailFormatTypeId, SortOrder, Enable, AddDate, ModifyDate ) VALUES (@FormatName, @FormatSubject, @FormatText, @MailFormatTypeId, @SortOrder, @Enable, GETDATE(), GETDATE()); SELECT SCOPE_IDENTITY()",
                    CommandType.Text,
                    new SqlParameter("@FormatName", mailFormat.FormatName),
                    new SqlParameter("@FormatSubject", mailFormat.FormatSubject),
                    new SqlParameter("@FormatText", mailFormat.FormatText),
                    new SqlParameter("@MailFormatTypeId", mailFormat.MailFormatTypeId),
                    new SqlParameter("@SortOrder", mailFormat.SortOrder),
                    new SqlParameter("@Enable", mailFormat.Enable));
            return mailFormat.MailFormatId;
        }

        public static void Delete(int mailFormatId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Settings].[MailFormat] WHERE MailFormatID = @MailFormatID",
                CommandType.Text, new SqlParameter("@MailFormatID", mailFormatId));
        }

        #endregion

        #region Get/Update/Delete MailFormatType

        public static List<MailFormatType> GetMailFormatTypes()
        {
            return
                SQLDataAccess.Query<MailFormatType>(
                    "SELECT * FROM [Settings].[MailFormatType] Order By [SortOrder]").ToList();
        }

        public static MailFormatType GetMailFormatType(int mailFormatTypeId)
        {
            return
                SQLDataAccess.Query<MailFormatType>(
                    "SELECT * FROM [Settings].[MailFormatType] WHERE MailFormatTypeID = @id",
                    new {id = mailFormatTypeId}).FirstOrDefault();
        }

        private static void UpdateMailFormatType(MailFormatType mailFormatType)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Settings].[MailFormatType] SET MailType = MailType, TypeName = @TypeName, SortOrder = @SortOrder, Comment = @Comment WHERE (MailFormatTypeID = @MailFormatTypeID)",
                CommandType.Text,
                new SqlParameter("@MailFormatTypeID", mailFormatType.MailFormatTypeId),
                new SqlParameter("@MailType", mailFormatType.MailType),
                new SqlParameter("@TypeName", mailFormatType.TypeName),
                new SqlParameter("@SortOrder", mailFormatType.SortOrder),
                new SqlParameter("@Comment", mailFormatType.Comment));
        }

        private static void InsertMailFormatType(MailFormatType mailFormatType)
        {
            SQLDataAccess.ExecuteNonQuery(
                "INSERT INTO [Settings].[MailFormatType] (MailType, TypeName, SortOrder, Comment) VALUES (@MailType, @TypeName, @SortOrder, @Comment); SELECT SCOPE_IDENTITY()",
                CommandType.Text,
                new SqlParameter("@MailType", mailFormatType.MailType),
                new SqlParameter("@TypeName", mailFormatType.TypeName),
                new SqlParameter("@SortOrder", mailFormatType.SortOrder),
                new SqlParameter("@Comment", mailFormatType.Comment));
        }

        private static void DeleteMailFormatType(int mailFormatTypeId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM [Settings].[MailFormatType] WHERE MailFormatTypeID = @MailFormatTypeID", CommandType.Text,
                new SqlParameter("@MailFormatTypeID", mailFormatTypeId));
        }

        #endregion
    }
}