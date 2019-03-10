//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;
using AdvantShop.Mails;

namespace AdvantShop.Module.StoreReviews.Domain
{
    public class StoreReviewRepository
    {
        public static bool InstallStoreReviewsModule()
        {
            if (!ModulesRepository.IsExistsModuleTable("Module", "StoreReview"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                    @"CREATE TABLE Module.StoreReview
                    (  ID int NOT NULL IDENTITY (1, 1),
	                        ParentID int NULL,
	                        ReviewerEmail nvarchar(50) NOT NULL,
                            ReviewerName nvarchar(100) NOT NULL,
	                        Review nvarchar(MAX) NOT NULL,
	                        DateAdded datetime NOT NULL,
                            Moderated bit NOT NULL,
	                        Rate int NULL,
                            ReviewerImage nvarchar(150) NULL
	                        )  ON [PRIMARY]
	                            TEXTIMAGE_ON [PRIMARY]                                        
                        ALTER TABLE Module.StoreReview ADD CONSTRAINT
	                        PK_StoreReview PRIMARY KEY CLUSTERED 
	                        (
	                        ID
	                        ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]                                        
                        ALTER TABLE Module.StoreReview SET (LOCK_ESCALATION = TABLE)
                        SET IDENTITY_INSERT Module.StoreReview ON",
                    CommandType.Text);
            }
            ModuleSettingsProvider.SetSettingValue("PageSize", "20", StoreReviews.ModuleID);
            return ModulesRepository.IsExistsModuleTable("Module", "StoreReview");
        }

        public static bool UpdateStoreReviewsModule()
        {
            if (!ModulesRepository.IsExistsModuleTable("Module", "StoreReview"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                    @"ALTER TABLE [Module].[StoreReview]
                      Add [ReviewerImage] nvarchar(150) NULL",
                    CommandType.Text);
            }
            ModuleSettingsProvider.SetSettingValue("PageSize", "20", StoreReviews.ModuleID);
            return ModulesRepository.IsExistsModuleTable("Module", "StoreReview");
        }

        public static bool UninstallStoreReviewsModule()
        {
            //if (ModulesRepository.IsExistsModuleTable("Module", "StoreReview"))
            //{
            //    ModulesRepository.ModuleExecuteNonQuery(@"DROP TABLE Module.StoreReview", CommandType.Text);
            //}

            //return ModulesRepository.IsExistsModuleTable("Module", "StoreReview");
            return true;
        }

        public static bool IsAliveStoreReviewsModule()
        {
            return ModulesRepository.IsExistsModuleTable("Module", "StoreReview");
        }

        private static StoreReview GetStoreReviewFromReader(SqlDataReader reader)
        {
            return new StoreReview
                {
                    Id = ModulesRepository.ConvertTo<int>(reader, "ID"),
                    ParentId = ModulesRepository.ConvertTo<int>(reader, "ParentID"),
                    Rate = ModulesRepository.ConvertTo<int>(reader, "Rate"),
                    Review = ModulesRepository.ConvertTo<string>(reader, "Review"),
                    ReviewerEmail = ModulesRepository.ConvertTo<string>(reader, "ReviewerEmail"),
                    ReviewerName = ModulesRepository.ConvertTo<string>(reader, "ReviewerName"),
                    DateAdded = ModulesRepository.ConvertTo<DateTime>(reader, "DateAdded"),
                    Moderated = ModulesRepository.ConvertTo<bool>(reader, "Moderated"),
                    ReviewerImage = ModulesRepository.ConvertTo<string>(reader, "ReviewerImage"),
                    ChildsCount = ModulesRepository.ConvertTo<int>(reader, "ChildsCount")
                };
        }

        public static List<StoreReview> GetStoreReviewsByParentId(int parentId)
        {
            return GetStoreReviewsByParentId(parentId, false);
        }

        public static DataTable GetStoreReviews()
        {
            return ModulesRepository.ModuleExecuteTable(
                "SELECT * FROM [Module].[StoreReview] ORDER BY [DateAdded] DESC",
                CommandType.Text
            );
        }

        public static List<StoreReview> GetStoreReviewsByParentId(int parentId, bool isModerated, int level = 0)
        {
            return ModulesRepository.ModuleExecuteReadList<StoreReview>(
                "SELECT *, (SELECT Count(ID) FROM [Module].[StoreReview] WHERE [ParentID] = ParentReview.ID) as ChildsCount FROM [Module].[StoreReview] as ParentReview WHERE "
                + (parentId == 0 ? "[ParentID] is NULL" : "[ParentID] = " + parentId)
                + (isModerated ? " AND [Moderated] = 1" : string.Empty) + " ORDER BY [DateAdded]"
                + (parentId == 0 ? "DESC" : "ASC"),
                CommandType.Text,
                (reader) =>
                {
                    var review = GetStoreReviewFromReader(reader);
                    review.Level = level;
                    review.ChildrenReviews = ModulesRepository.ConvertTo<int>(reader, "ChildsCount") > 0
                        ? GetStoreReviewsByParentId(ModulesRepository.ConvertTo<int>(reader, "ID"), isModerated, ++level)
                        : new List<StoreReview>();
                    return review;
                }
            );
        }

        public static StoreReview GetStoreReview(int id)
        {
            return ModulesRepository.ModuleExecuteReadOne<StoreReview>(
                "SELECT *, (SELECT Count(ID) FROM [Module].[StoreReview] WHERE [ParentID] = ParentReview.ID) as ChildsCount FROM [Module].[StoreReview] as ParentReview WHERE [ID] = @ID",
                CommandType.Text,
                (reader) =>
                {
                    var review = GetStoreReviewFromReader(reader);
                    review.ChildrenReviews = ModulesRepository.ConvertTo<int>(reader, "ChildsCount") > 0
                        ? GetStoreReviewsByParentId(ModulesRepository.ConvertTo<int>(reader, "ID"))
                        : new List<StoreReview>();
                    return review;
                },
                new SqlParameter("@ID", id));
        }

        public static void DeleteStoreReviewsById(int id)
        {
            var review = GetStoreReview(id);
            if (review == null)
                return;
            if (!string.IsNullOrEmpty(review.ReviewerImage))
            {
                var imageFullPath = HttpContext.Current.Server.MapPath(StoreReviews.ImagePath + review.ReviewerImage);
                if (File.Exists(imageFullPath))
                    File.Delete(imageFullPath);
            }

            ModulesRepository.ModuleExecuteNonQuery(
                "DELETE FROM [Module].[StoreReview] WHERE [ID] = @ID",
                CommandType.Text,
                new SqlParameter("@ID", id));

            foreach (var childReview in review.ChildrenReviews)
            {
                DeleteStoreReviewsById(childReview.Id);
            }
        }

        public static void AddStoreReview(StoreReview storeReview)
        {
            storeReview.Id = ModulesRepository.ModuleExecuteScalar<int>(
                "INSERT INTO [Module].[StoreReview] ([ParentID],[Rate],[Review],[ReviewerEmail],[ReviewerName],[DateAdded],[Moderated],[ReviewerImage]) VALUES (@ParentID,@Rate,@Review,@ReviewerEmail,@ReviewerName,GETDATE(),@Moderated,@ReviewerImage); SELECT SCOPE_IDENTITY()",
                CommandType.Text,
                new SqlParameter("@ParentID", storeReview.ParentId == 0 ? DBNull.Value : (object)storeReview.ParentId),
                new SqlParameter("@Rate", storeReview.Rate),
                new SqlParameter("@Review", storeReview.Review),
                new SqlParameter("@ReviewerEmail", storeReview.ReviewerEmail),
                new SqlParameter("@ReviewerName", storeReview.ReviewerName),
                new SqlParameter("@Moderated", storeReview.Moderated),
                new SqlParameter("@ReviewerImage", storeReview.ReviewerImage ?? (object)DBNull.Value));

            if (ModuleSettingsProvider.GetSettingValue<bool>("EnableSendMails", StoreReviews.ModuleID))
            {
                var message = ModuleSettingsProvider.GetSettingValue<string>("Format", StoreReviews.ModuleID);
                message = message.Replace("#NAME#", storeReview.ReviewerName);
                message = message.Replace("#EMAIL#", storeReview.ReviewerEmail);
                message = message.Replace("#REVIEW#", storeReview.Review);

                SendMail.SendMailNow(Guid.Empty,
                    ModuleSettingsProvider.GetSettingValue<string>("Email", StoreReviews.ModuleID),
                    ModuleSettingsProvider.GetSettingValue<string>("Subject", StoreReviews.ModuleID),
                    message,
                    true);
            }
        }

        public static void UpdateStoreReview(StoreReview storeReview)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "UPDATE [Module].[StoreReview] SET [ParentID]=@ParentID,[Rate]=@Rate,[Review]=@Review,[ReviewerEmail]=@ReviewerEmail,[ReviewerName]=@ReviewerName, [Moderated]=@Moderated, [DateAdded]=@DateAdded, [ReviewerImage]=@ReviewerImage WHERE [ID]=@ID",
                CommandType.Text,
                new SqlParameter("@ID", storeReview.Id),
                new SqlParameter("@ParentID", storeReview.ParentId == 0 ? DBNull.Value : (object)storeReview.ParentId),
                new SqlParameter("@Rate", storeReview.Rate),
                new SqlParameter("@Review", storeReview.Review),
                new SqlParameter("@ReviewerEmail", storeReview.ReviewerEmail),
                new SqlParameter("@ReviewerName", storeReview.ReviewerName),
                new SqlParameter("@Moderated", storeReview.Moderated),
                new SqlParameter("@DateAdded", storeReview.DateAdded),
                new SqlParameter("@ReviewerImage", storeReview.ReviewerImage ?? (object)DBNull.Value));
        }
        
        public static void SaveAndResizeImage(Image image, string resultPath)
        {
            double resultWidth = image.Width;  // 0;
            double resultHeight = image.Height; // 0;

            var maxWidth = ModuleSettingsProvider.GetSettingValue<double>("MaxImageWidth", StoreReviews.ModuleID);
            var maxHeight = ModuleSettingsProvider.GetSettingValue<double>("MaxImageHeight", StoreReviews.ModuleID);

            if ((maxHeight != 0) && (image.Height > maxHeight))
            {
                resultHeight = maxHeight;
                resultWidth = (image.Width * resultHeight) / image.Height;
            }

            if ((maxWidth != 0) && (resultWidth > maxWidth))
            {
                resultHeight = (resultHeight * maxWidth) / resultWidth; // (resultHeight * resultWidth) / resultHeight;
                resultWidth = maxWidth;
            }

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(StoreReviews.ImagePath)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(StoreReviews.ImagePath));
            }

            try
            {
                using (var result = new Bitmap((int)resultWidth, (int)resultHeight))
                {
                    result.MakeTransparent();
                    using (var graphics = Graphics.FromImage(result))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.DrawImage(image, 0, 0, (int)resultWidth, (int)resultHeight);

                        graphics.Flush();
                        var ext = Path.GetExtension(resultPath);
                        var encoder = GetEncoder(ext);
                        using (var myEncoderParameters = new EncoderParameters(3))
                        {
                            myEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                            myEncoderParameters.Param[1] = new EncoderParameter(Encoder.ScanMethod, (int)EncoderValue.ScanMethodInterlaced);
                            myEncoderParameters.Param[2] = new EncoderParameter(Encoder.RenderMethod, (int)EncoderValue.RenderProgressive);

                            using (var stream = new FileStream(resultPath, FileMode.CreateNew))
                            {
                                result.Save(stream, encoder, myEncoderParameters);
                                stream.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
        
        private static ImageCodecInfo GetEncoder(string fileExt)
        {
            fileExt = fileExt.TrimStart(".".ToCharArray()).ToLower().Trim();
            string res;
            switch (fileExt)
            {
                case "jpg":
                case "jpeg":
                    res = "image/jpeg";
                    break;
                case "png":
                    res = "image/png";
                    break;
                case "gif":
                    //if need transparency
                    //res = "image/png";
                    res = "image/gif";
                    break;
                default:
                    res = "image/jpeg";
                    break;
            }

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.FirstOrDefault(codec => codec.MimeType == res);
        }

        public static void DeleteReviewerImage(int reviewId)
        {
            var review = GetStoreReview(reviewId);

            var imageFullPath = HttpContext.Current.Server.MapPath(StoreReviews.ImagePath + review.ReviewerImage);
            if (File.Exists(imageFullPath))
                File.Delete(imageFullPath);

            ModulesRepository.ModuleExecuteNonQuery(
                "UPDATE [Module].[StoreReview] Set [ReviewerImage] = NULL WHERE [ID] = @ID",
                CommandType.Text,
                new SqlParameter("@ID", reviewId));
        }

        public static bool CheckImageExtension(string fileName)
        {
            return new List<string> {".jpg", ".gif", ".png", ".bmp", ".jpeg"}.Contains(Path.GetExtension(fileName));
        }
    }
}
