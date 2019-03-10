using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.CMS
{
    public class AdminCommentService
    {
        private static AdminComment GetAdminCommentFromReader(SqlDataReader reader)
        {
            var comment = new AdminComment
            {
                Id = SQLDataHelper.GetInt(reader, "Id"),
                ParentId = SQLDataHelper.GetNullableInt(reader, "ParentId"),
                ObjId = SQLDataHelper.GetInt(reader, "ObjId"),
                Type = SQLDataHelper.GetString(reader, "Type").TryParseEnum<AdminCommentType>(),
                CustomerId = SQLDataHelper.GetNullableGuid(reader, "CustomerId"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                Email = SQLDataHelper.GetString(reader, "Email"),
                Text = SQLDataHelper.GetString(reader, "Text"),
                DateCreated = SQLDataHelper.GetDateTime(reader, "DateCreated"),
                DateModified = SQLDataHelper.GetDateTime(reader, "DateModified"),
                Deleted = SQLDataHelper.GetBoolean(reader, "Deleted"),
                Avatar = SQLDataHelper.GetString(reader, "Avatar")
            };
            return comment;
        }

        public static AdminComment GetAdminComment(int id)
        {
            return SQLDataAccess.ExecuteReadOne<AdminComment>(
                    "SELECT AdminComment.*, Customer.Avatar FROM CMS.AdminComment " +
                    "LEFT JOIN Customers.Customer ON Customer.CustomerId = AdminComment.CustomerId WHERE Id = @Id",
                    CommandType.Text, GetAdminCommentFromReader,
                    new SqlParameter("@Id", id));
        }

        public static List<AdminComment> GetAdminComments(int objId, AdminCommentType type)
        {
            return SQLDataAccess.ExecuteReadList<AdminComment>(
                    "SELECT AdminComment.*, Customer.Avatar FROM CMS.AdminComment LEFT JOIN Customers.Customer ON Customer.CustomerId = AdminComment.CustomerId " +
                    "WHERE ObjId = @ObjId AND Type = @Type ORDER BY DateCreated",
                    CommandType.Text, GetAdminCommentFromReader,
                    new SqlParameter("@ObjId", objId),
                    new SqlParameter("@Type", type.ToString()));
        }

        public static int AddAdminComment(AdminComment comment)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO CMS.AdminComment " +
                "(ParentId, ObjId, Type, CustomerId, Name, Email, Text, DateCreated, DateModified) " +
                "VALUES (@ParentId, @ObjId, @Type, @CustomerId, @Name, @Email, @Text, GETDATE(), GETDATE()); SELECT SCOPE_IDENTITY(); ",
                CommandType.Text,
                new SqlParameter("@ParentId", comment.ParentId.HasValue ? comment.ParentId.Value : (object)DBNull.Value),
                new SqlParameter("@ObjId", comment.ObjId),
                new SqlParameter("@Type", comment.Type.ToString()),
                new SqlParameter("@CustomerId", comment.CustomerId),
                new SqlParameter("@Name", comment.Name),
                new SqlParameter("@Email", comment.Email ?? string.Empty),
                new SqlParameter("@Text", comment.Text));
        }

        public static void UpdateAdminComment(AdminComment comment)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE CMS.AdminComment SET ParentId = @ParentId, ObjId = @ObjId, Type = @Type, CustomerId = @CustomerId, Name = @Name, Email = @Email, " +
                "Text = @Text, DateModified = GETDATE() WHERE Id = @Id",
                CommandType.Text,
                new SqlParameter("@Id", comment.Id),
                new SqlParameter("@ParentId", comment.ParentId.HasValue ? comment.ParentId.Value : (object)DBNull.Value),
                new SqlParameter("@ObjId", comment.ObjId),
                new SqlParameter("@Type", comment.Type.ToString()),
                new SqlParameter("@CustomerId", comment.CustomerId),
                new SqlParameter("@Name", comment.Name),
                new SqlParameter("@Email", comment.Email ?? string.Empty),
                new SqlParameter("@Text", comment.Text));
        }

        public static void DeleteAdminComments(int objId, AdminCommentType type)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM CMS.AdminComment WHERE ObjId = @ObjId AND Type = @Type", CommandType.Text, 
                new SqlParameter("@ObjId", objId), 
                new SqlParameter("@Type", type.ToString()));
        }

        public static void DeleteAdminComment(int id)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE CMS.AdminComment SET Deleted = 1 WHERE Id = @Id", CommandType.Text, 
                new SqlParameter("@Id", id));
        }
    }
}