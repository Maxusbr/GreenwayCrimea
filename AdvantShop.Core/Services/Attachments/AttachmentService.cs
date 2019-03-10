using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.FilePath;
using System.IO;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Helpers;

namespace AdvantShop.Core.Services.Attachments
{
    public class AttachmentService
    {
        #region Files

        public static readonly Dictionary<AttachmentType, FolderType> FolderTypes = new Dictionary<AttachmentType, FolderType>
        {
            {AttachmentType.Task, FolderType.TaskAttachment},
            {AttachmentType.Lead, FolderType.LeadAttachment},
        };

        private static readonly Dictionary<AttachmentType, EAdvantShopFileTypes> FileTypes = new Dictionary<AttachmentType, EAdvantShopFileTypes>
        {
            {AttachmentType.Task, EAdvantShopFileTypes.TaskAttachment},
            {AttachmentType.Lead, EAdvantShopFileTypes.LeadAttachment},
        };

        public static string GetPathAbsolut(int objId, AttachmentType type, string fileName)
        {
            if (objId == 0)
                throw new Exception("no object to attach file");

            var directoryPath = string.Format("{0}{1}/", FoldersHelper.GetPathAbsolut(FolderTypes[type]), objId);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            if (string.IsNullOrWhiteSpace(fileName))
                return directoryPath;
            return string.Format("{0}{1}", directoryPath, fileName);
        }

        public static string GetPath(int objId, AttachmentType type, string fileName, bool isForAdministration)
        {
            return string.Format("{0}{1}/{2}", FoldersHelper.GetPath(FolderTypes[type], null, isForAdministration), objId, fileName);
        }

        public static bool CheckFileExtension(string fileName, AttachmentType type)
        {
            return FileHelpers.CheckFileExtension(fileName, FileTypes[type]);
        }

        public static bool DeleteAttachments<T>(int objId) where T : Attachment, new()
        {
            var attachments = GetAttachments<T>(objId);

            foreach (var attachment in attachments)
            {
                FileHelpers.DeleteFile(attachment.PathAbsolut);
                FilesStorageService.DecrementAttachmentsSize(attachment.FileSize);
            }
            DeleteFromDBByOwner<T>(objId);
            return true;
        }

        public static bool DeleteAttachment<T>(int id) where T : Attachment, new()
        {
            var attachment = GetAttachment<T>(id);
            if (attachment == null)
                return false;
            
            FileHelpers.DeleteFile(attachment.PathAbsolut);
            DeleteFromDB(id);
            FilesStorageService.DecrementAttachmentsSize(attachment.FileSize);

            return true;
        }

        #endregion

        #region DataBase

        private static T GetAttachmentFromReader<T>(SqlDataReader reader) where T : Attachment, new()
        {
            return new T()
            {
                Id = SQLDataHelper.GetInt(reader, "Id"),
                ObjId = SQLDataHelper.GetInt(reader, "ObjId"),
                FileName = SQLDataHelper.GetString(reader, "FileName"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                FileSize = SQLDataHelper.GetInt(reader, "FileSize"),
                DateCreated = SQLDataHelper.GetDateTime(reader, "DateCreated"),
                DateModified = SQLDataHelper.GetDateTime(reader, "DateModified"),
            };
        }

        public static T GetAttachment<T>(int attachmentId) where T : Attachment, new()
        {
            return SQLDataAccess.ExecuteReadOne<T>(
                "SELECT * FROM CMS.Attachment WHERE Id = @Id", CommandType.Text,
                GetAttachmentFromReader<T>, new SqlParameter("@Id", attachmentId));
        }

        public static List<T> GetAttachments<T>(int objId) where T : Attachment, new()
        {
            var type = new T().Type;
            return SQLDataAccess.ExecuteReadList<T>(
                "SELECT * FROM CMS.Attachment WHERE ObjId = @ObjId AND Type = @Type ORDER BY DateCreated", CommandType.Text,
                GetAttachmentFromReader<T>,
                new SqlParameter("@ObjId", objId),
                new SqlParameter("@Type", type.ToString()));
        }

        public static long GetAttachmentsSize<T>() where T : Attachment, new()
        {
            var type = new T().Type;
            return
                Convert.ToInt64(
                    SQLDataAccess.ExecuteScalar<int>("SELECT IsNull(Sum(FileSize),0) FROM CMS.Attachment WHERE Type = @Type",
                        CommandType.Text, new SqlParameter("@Type", type.ToString())));
        }

        public static long GetAllAttachmentsSize()
        {
            return
                Convert.ToInt64(SQLDataAccess.ExecuteScalar<int>("SELECT IsNull(Sum(FileSize),0) FROM CMS.Attachment", CommandType.Text));
        }

        public static int AddAttachment(Attachment attachment)
        {
            FilesStorageService.IncrementAttachmentsSize(attachment.FileSize);

            return SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO CMS.Attachment (ObjId, Type, FileName, SortOrder, FileSize, DateCreated, DateModified) " +
                "VALUES (@ObjId, @Type, @FileName, @SortOrder, @FileSize, GETDATE(), GETDATE()); SELECT SCOPE_IDENTITY();",
                CommandType.Text,
                new SqlParameter("@ObjId", attachment.ObjId),
                new SqlParameter("@Type", attachment.Type.ToString()),
                new SqlParameter("@FileName", attachment.FileName),
                new SqlParameter("@SortOrder", attachment.SortOrder),
                new SqlParameter("@FileSize", attachment.FileSize)
                );
        }

        public static void UpdateAttachment(Attachment attachment)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE CMS.Attachment SET ObjId = @ObjId, FileName = @FileName, SortOrder = @SortOrder, FileSize = @FileSize, DateModified = GETDATE() WHERE Id = @Id", 
                CommandType.Text,
                new SqlParameter("@Id", attachment.Id),
                new SqlParameter("@ObjId", attachment.ObjId),
                new SqlParameter("@FileName", attachment.FileName),
                new SqlParameter("@SortOrder", attachment.SortOrder),
                new SqlParameter("@FileSize", attachment.FileSize)
                );
        }

        private static void DeleteFromDBByOwner<T>(int objId) where T : Attachment, new()
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM CMS.Attachment WHERE ObjId = @ObjId AND Type = @Type", CommandType.Text,
                new SqlParameter("@ObjId", objId),
                new SqlParameter("@Type", new T().Type.ToString()));
        }

        private static void DeleteFromDB(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM CMS.Attachment WHERE Id = @Id", CommandType.Text,
                new SqlParameter("@Id", id));
        }

        #endregion
    }
}