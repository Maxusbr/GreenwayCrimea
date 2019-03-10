//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Linq;
using AdvantShop.CMS;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

namespace AdvantShop.DownloadableContent
{
    public class DownloadableContentService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dcObject"></param>
        /// <returns></returns>
        public static int Add(DownloadableContentObject dcObject)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO [dbo].[DownloadableContent] ([StringId], [IsInstall], [DateAdded], [DateModified], [Active], [Version], [DcType]) VALUES (@StringId, @IsInstall, GETDATE(), GETDATE(), @Active, @Version, @DcType); Select Scope_Identity();",
                CommandType.Text,
                new SqlParameter("@StringId", dcObject.StringId),
                new SqlParameter("@IsInstall", dcObject.IsInstall),
                new SqlParameter("@Active", dcObject.Active),
                new SqlParameter("@Version", dcObject.Version),
                new SqlParameter("@DcType", dcObject.DcType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public static void Delete(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM [dbo].[DownloadableContent] WHERE [Id] = @Id",
                CommandType.Text,
                new SqlParameter("@Id", id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringId"></param>
        public static void Delete(string stringId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM [dbo].[DownloadableContent] WHERE [StringId] = @StringId",
                CommandType.Text,
                new SqlParameter("@StringId", stringId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dcObject"></param>
        public static void Update(DownloadableContentObject dcObject)
        {
            SQLDataAccess.ExecuteNonQuery(
               "UPDATE [dbo].[DownloadableContent] SET [StringId] = @StringId, [IsInstall] = @IsInstall, [DateModified] = GETDATE(), [Active] = @Active, [Version] = @Version, [DcType] = @DcType WHERE [Id] = @Id",
               CommandType.Text,
               new SqlParameter("@Id", dcObject.Id),
               new SqlParameter("@StringId", dcObject.StringId),
               new SqlParameter("@IsInstall", dcObject.IsInstall),
               new SqlParameter("@Active", dcObject.Active),
               new SqlParameter("@Version", dcObject.Version),
               new SqlParameter("@DcType", dcObject.DcType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dcObject"></param>
        public static void Install(DownloadableContentObject dcObject)
        {
            SQLDataAccess.ExecuteNonQuery(
                "IF (SELECT COUNT([Id]) FROM [dbo].[DownloadableContent] WHERE [StringId] = @StringId) > 0 " +
                "BEGIN " +
                    "UPDATE [dbo].[DownloadableContent] SET [IsInstall] = 1, [DateModified] = GETDATE(), [Version]=@Version, Active=@Active WHERE [StringId] = @StringId " +
                "END " +
                "ELSE " +
                "BEGIN " +
                    "INSERT INTO [dbo].[DownloadableContent] ([StringId], [IsInstall], [DateAdded], [DateModified], [Active], [Version], [DcType]) " +
                    "VALUES (@StringId, @IsInstall, GETDATE(), GETDATE(), @Active, @Version, @DcType) " +
                "END",
                CommandType.Text,
                new SqlParameter("@Id", dcObject.Id),
                new SqlParameter("@StringId", dcObject.StringId),
                new SqlParameter("@IsInstall", dcObject.IsInstall),
                new SqlParameter("@Active", dcObject.Active),
                new SqlParameter("@Version", dcObject.Version),
                new SqlParameter("@DcType", dcObject.DcType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public static void Uninstall(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [dbo].[DownloadableContent] SET [IsInstall] = 0, [DateModified] = GETDATE() WHERE [Id] = @Id",
                CommandType.Text,
                new SqlParameter("@Id", id));
        }

        public static void Uninstall(string id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [dbo].[DownloadableContent] SET [IsInstall] = 0, [Active] = 0, [DateModified] = GETDATE() WHERE [Id] = @Id",
                CommandType.Text,
                new SqlParameter("@Id", id));
        }

        public static void Uninstall(string stringId, string type)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [dbo].[DownloadableContent] SET [IsInstall] = 0, [Active] = 0, [DateModified] = GETDATE() WHERE [StringId] = @StringId and [DcType] = @DcType",
                CommandType.Text,
                new SqlParameter("@StringId", stringId),
                new SqlParameter("@DcType", type));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DownloadableContentObject GetOne(int id)
        {
            return SQLDataAccess.ExecuteReadOne<DownloadableContentObject>(
                "SELECT * FROM [dbo].[DownloadableContent] WHERE [Id] = @Id",
                CommandType.Text,
                GetFromReader,
                new SqlParameter("@Id", id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringId"></param>
        /// <returns></returns>
        public static DownloadableContentObject GetOne(string stringId)
        {
            return SQLDataAccess.ExecuteReadOne<DownloadableContentObject>(
                "SELECT * FROM [dbo].[DownloadableContent] WHERE [StringId] = @StringId",
                CommandType.Text,
                GetFromReader,
                new SqlParameter("@StringId", stringId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<DownloadableContentObject> GetList()
        {
            return SQLDataAccess.ExecuteReadList<DownloadableContentObject>(
                "SELECT * FROM [dbo].[DownloadableContent]",
                CommandType.Text,
                GetFromReader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dcType"></param>
        /// <returns></returns>
        public static List<DownloadableContentObject> GetList(string dcType)
        {
            return SQLDataAccess.ExecuteReadList<DownloadableContentObject>(
                "SELECT * FROM [dbo].[DownloadableContent]",
                CommandType.Text,
                GetFromReader,
                new SqlParameter("@DcType", dcType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsInstall(int id)
        {
            return SQLDataAccess.ExecuteScalar<bool>(
                "SELECT SNULL([IsInstall], 0) FROM [dbo].[DownloadableContent] WHERE [Id] = @Id",
                CommandType.Text,
                new SqlParameter("@Id", id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringId"></param>
        /// <returns></returns>
        public static bool IsInstall(string stringId)
        {
            return SQLDataAccess.ExecuteScalar<bool>(
                "SELECT ISNULL([IsInstall], 0) FROM [dbo].[DownloadableContent] WHERE [StringId] = @StringId",
                CommandType.Text,
                new SqlParameter("@StringId", stringId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsActive(int id)
        {
            return SQLDataAccess.ExecuteScalar<bool>(
                "SELECT ISNULL([Active],0) FROM [dbo].[DownloadableContent] WHERE [Id] = @Id",
                CommandType.Text,
                new SqlParameter("@Id", id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringId"></param>
        /// <returns></returns>
        public static bool IsActive(string stringId)
        {
            return SQLDataAccess.ExecuteScalar<bool>(
                "SELECT ISNULL([Active],0) FROM [dbo].[DownloadableContent] WHERE [StringId] = @StringId",
                CommandType.Text,
                new SqlParameter("@StringId", stringId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static DownloadableContentObject GetFromReader(SqlDataReader reader)
        {
            return new DownloadableContentObject
            {
                Id = SQLDataHelper.GetInt(reader, "Id"),
                StringId = SQLDataHelper.GetString(reader, "StringId"),
                IsInstall = SQLDataHelper.GetBoolean(reader, "IsInstall"),
                DateAdded = SQLDataHelper.GetDateTime(reader, "DateAdded"),
                DateModified = SQLDataHelper.GetDateTime(reader, "DateModified"),
                Active = SQLDataHelper.GetBoolean(reader, "Active"),
                Version = SQLDataHelper.GetString(reader, "Version"),
                DcType = SQLDataHelper.GetString(reader, "DcType")
            };
        }
    }
}
