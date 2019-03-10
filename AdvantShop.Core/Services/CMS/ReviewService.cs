//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Helpers;

namespace AdvantShop.CMS
{
    public class ReviewService
    {
        public static Review GetReview(int reviewId)
        {
            return SQLDataAccess.ExecuteReadOne<Review>(
                    "SELECT ParentReview.*, Photo.PhotoName, (SELECT Count(*) FROM [CMS].[Review] WHERE [ParentId] = ParentReview.ReviewId) as ChildrenCount " +
                    "FROM [CMS].[Review] as ParentReview LEFT JOIN Catalog.Photo ON ParentReview.ReviewId = Photo.ObjId AND Main = 1 AND Photo.Type = @PhotoType " +
                    "WHERE ReviewId = @ReviewId",
                    CommandType.Text, GetFromReader,
                    new SqlParameter("@ReviewId", reviewId),
                    new SqlParameter("@PhotoType", PhotoType.Review.ToString()));
        }

        public static IEnumerable<Review> GetReviews(int entityId, EntityType entityType)
        {
            return SQLDataAccess.ExecuteReadIEnumerable<Review>(
                    "SELECT ParentReview.*, Photo.PhotoName, (SELECT Count(*) FROM [CMS].[Review] WHERE [ParentId] = ParentReview.ReviewId) as ChildrenCount " +
                    "FROM [CMS].[Review] as ParentReview LEFT JOIN Catalog.Photo ON ParentReview.ReviewId = Photo.ObjId AND Main = 1 AND Photo.Type = @PhotoType " +
                    "WHERE [EntityId] = @EntityId AND ParentReview.[Type] = @entityType order by AddDate desc",
                    CommandType.Text, GetFromReader,
                    new SqlParameter("@EntityId", entityId),
                    new SqlParameter("@entityType", (int)entityType),
                    new SqlParameter("@PhotoType", PhotoType.Review.ToString()));
        }

        public static IEnumerable<Review> GetCheckedReviews(int entityId, EntityType entityType)
        {
            return SQLDataAccess.ExecuteReadIEnumerable<Review>(
                    "SELECT ParentReview.*, Photo.PhotoName, (SELECT Count(*) FROM [CMS].[Review] WHERE [ParentId] = ParentReview.ReviewId) as ChildrenCount " +
                    "FROM [CMS].[Review] as ParentReview LEFT JOIN Catalog.Photo ON ParentReview.ReviewId = Photo.ObjId AND Main = 1 AND Photo.Type = @PhotoType " +
                    "WHERE [EntityId] = @EntityId AND ParentReview.[Type] = @entityType AND [Checked] = 1 order by AddDate desc",
                    CommandType.Text, GetFromReader,
                    new SqlParameter("@EntityId", entityId),
                    new SqlParameter("@entityType", (int)entityType),
                    new SqlParameter("@PhotoType", PhotoType.Review.ToString()));
        }

        public static int GetReviewsCount(int entityId, EntityType entityType)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                    "SELECT count(ReviewID) FROM [CMS].[Review] WHERE [EntityId] = @EntityId AND [Type] = @Type",
                    CommandType.Text,
                    new SqlParameter("@EntityId", entityId),
                    new SqlParameter("@Type", (int) entityType));
        }

        public static int GetCheckedReviewsCount(int entityId, EntityType entityType)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                    "SELECT count(ReviewID) FROM [CMS].[Review] WHERE [EntityId] = @EntityId AND [Type] = @Type AND [Checked] = 1",
                    CommandType.Text,
                    new SqlParameter("@EntityId", entityId),
                    new SqlParameter("@Type", (int) entityType));
        }

        public static List<Review> GetReviewChildren(int reviewId)
        {
            return SQLDataAccess.ExecuteReadList<Review>(
                "SELECT ParentReview.*, Photo.PhotoName, (SELECT Count(*) FROM [CMS].[Review] WHERE [ParentId] = ParentReview.ReviewId) as ChildrenCount " +
                "FROM [CMS].[Review] as ParentReview LEFT JOIN Catalog.Photo ON ParentReview.ReviewId = Photo.ObjId AND Main = 1 AND Photo.Type = @PhotoType " +
                "WHERE [ParentId] = @ParentId",
                CommandType.Text, GetFromReader,
                new SqlParameter("@ParentId", reviewId),
                    new SqlParameter("@PhotoType", PhotoType.Review.ToString()));
        }

        public static List<int> GetReviewChildrenIds(int reviewId)
        {
            return SQLDataAccess.ExecuteReadColumn<int>(
                "SELECT [ReviewId] FROM [CMS].[Review] WHERE [ParentId] = @ParentId",
                CommandType.Text, "ReviewId",
                new SqlParameter("@ParentId", reviewId));
        }

        public static IEnumerable<Review> GetReviewList()
        {
            return SQLDataAccess.ExecuteReadIEnumerable<Review>(
                "SELECT ParentReview.*, Photo.PhotoName, (SELECT Count(*) FROM [CMS].[Review] WHERE [ParentId] = ParentReview.ReviewId) as ChildrenCount " +
                "FROM [CMS].[Review] as ParentReview LEFT JOIN Catalog.Photo ON ParentReview.ReviewId = Photo.ObjId AND Main = 1 AND Photo.Type = @PhotoType  order by AddDate desc",
                CommandType.Text, GetFromReader,
                new SqlParameter("@PhotoType", PhotoType.Review.ToString()));
        }

        private static Review GetFromReader(SqlDataReader reader)
        {
            return new Review
            {
                ReviewId = SQLDataHelper.GetInt(reader, "ReviewId"),
                ParentId = SQLDataHelper.GetNullableInt(reader, "ParentId") ?? 0,
                EntityId = SQLDataHelper.GetInt(reader, "EntityId"),
                CustomerId = SQLDataHelper.GetGuid(reader, "CustomerId"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                Email = SQLDataHelper.GetString(reader, "Email"),
                Text = SQLDataHelper.GetString(reader, "Text"),
                Checked = SQLDataHelper.GetBoolean(reader, "Checked"),
                AddDate = SQLDataHelper.GetDateTime(reader, "AddDate"),
                Ip = SQLDataHelper.GetString(reader, "IP"),
                ChildrenCount = SQLDataHelper.GetInt(reader, "ChildrenCount"),
                PhotoName = SQLDataHelper.GetString(reader, "PhotoName")
            };
        }

        public static void AddReview(Review review)
        {
            review.ReviewId = SQLDataHelper.GetInt(
                SQLDataAccess.ExecuteScalar(
                    "INSERT INTO [CMS].[Review] " +
                    " ([ParentId], [EntityId], [Type], [CustomerId], [Name], [Email], [Text], [Checked], [AddDate], [IP]) " +
                    " VALUES (@ParentId, @EntityId, @Type, @CustomerId, @Name, @Email, @Text, @Checked, @AddDate, @IP); SELECT SCOPE_IDENTITY(); ",
                    CommandType.Text,
                    new SqlParameter("@ParentId", review.ParentId),
                    new SqlParameter("@EntityId", review.EntityId),
                    new SqlParameter("@Type", (int) review.Type),
                    new SqlParameter("@CustomerId", review.CustomerId),
                    new SqlParameter("@Name", review.Name),
                    new SqlParameter("@Email", review.Email),
                    new SqlParameter("@Text", review.Text),
                    new SqlParameter("@Checked", review.Checked),
                    new SqlParameter("@IP", review.Ip),
                    new SqlParameter("@AddDate", review.AddDate))
                );

            if (review.Type == EntityType.Product)
                ProductService.PreCalcProductComments(review.EntityId);
            BizProcessExecuter.ReviewAdded(review);
        }

        public static void UpdateReview(Review review)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [CMS].[Review] SET [ParentId] = @ParentId, [EntityId] = @EntityId, [Type] = @Type, [CustomerId] = @CustomerId, [Name] = @Name, [Email] = @Email, [Text] = @Text , [Checked] = @Checked, AddDate=@AddDate  WHERE reviewId = @reviewId",
                CommandType.Text,
                new SqlParameter("@reviewId", review.ReviewId),
                new SqlParameter("@ParentId", review.ParentId),
                new SqlParameter("@EntityId", review.EntityId),
                new SqlParameter("@Type", review.Type),
                new SqlParameter("@CustomerId", review.CustomerId),
                new SqlParameter("@Name", review.Name),
                new SqlParameter("@Email", review.Email),
                new SqlParameter("@Checked", review.Checked),
                new SqlParameter("@Text", review.Text),
                new SqlParameter("@AddDate", review.AddDate));

            if (review.Type == EntityType.Product)
                ProductService.PreCalcProductComments(review.EntityId);
        }

        public static void CheckReview(int reviewId, bool isChecked)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [CMS].[Review] SET [Checked] = @Checked WHERE reviewId = @reviewId",
                CommandType.Text,
                new SqlParameter("@reviewId", reviewId),
                new SqlParameter("@Checked", isChecked));

            var review = GetReview(reviewId);
            if (review != null && review.Type == EntityType.Product)
            {
                ProductService.PreCalcProductComments(review.EntityId);
            }
        }

        public static void DeleteReview(int reviewId)
        {
            var review = GetReview(reviewId);

            // Список удаляемых коментов
            var deleteIds = new List<int> { reviewId };
            var newIds = new List<int> { reviewId };

            // Пока есть новые коментарии для удаления
            while (newIds.Count > 0)
            {
                var listIds = new List<int>();

                // Берём все дочерние коменты, у всех комметариев с прошлой итерации
                foreach (var newId in newIds)
                {
                    listIds.AddRange(GetReviewChildrenIds(newId));
                }

                // Добавляем в список
                deleteIds.AddRange(listIds);

                newIds.Clear();
                newIds.AddRange(listIds);
            }

            // Удаляем комменты
            foreach (var deleteId in deleteIds)
            {
                PhotoService.DeletePhotos(deleteId, PhotoType.Review);
                DeleteCommentFromDb(deleteId);
            }

            if (review != null && review.Type == EntityType.Product)
            {
                ProductService.PreCalcProductComments(review.EntityId);
            }
        }

        private static void DeleteCommentFromDb(int commentId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [CMS].[Review] WHERE ReviewId = @ReviewId", CommandType.Text, new SqlParameter("@ReviewId", commentId));
        }


        // ********************** Request Methods ********************** //

        public static bool IsExistsEntity(int entityId, EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Product:
                    return ProductService.IsExists(entityId);

                default:
                    throw new NotImplementedException();
            }
        }

        public static ReviewEntity GetReviewEntity(int reviewId)
        {
            var type = (EntityType) SQLDataAccess.ExecuteScalar<int>("Select Type From CMS.Review Where ReviewId=@ReviewId",
                    CommandType.Text, new SqlParameter {ParameterName = "@ReviewId", Value = reviewId});
            return GetReviewEntity(reviewId, type);
        }
        
        public static ReviewEntity GetReviewEntity(int reviewId, EntityType type)
        {
            switch (type)
            {
                case EntityType.Product:
                    return SQLDataAccess.ExecuteReadOne(
                        "Select Product.ProductID, Name, ArtNo, Photo.Description, PhotoName " +
                        "From Catalog.Product left join catalog.Photo on Product.ProductID = Photo.ObjId and Type=@type and main = 1 " +
                        "Where Product.ProductID = (Select EntityID From CMS.Review Where ReviewId=@ReviewId )",
                        CommandType.Text,
                        reader => new ReviewEntity()
                        {
                            Type = EntityType.Product,
                            Name = SQLDataHelper.GetString(reader, "Name"),
                            ReviewEntityId = SQLDataHelper.GetInt(reader, "ProductID"),
                            Photo = SQLDataHelper.GetString(reader, "PhotoName"),
                            PhotoDescription = SQLDataHelper.GetString(reader, "Description")
                        },
                        new SqlParameter("@ReviewId", reviewId),
                        new SqlParameter("@type", PhotoType.Product.ToString()));
                default:
                    throw new NotImplementedException();
            }
        }
        
        public static string GetEntityAdminUrl(int entityId, EntityType type)
        {
            if (!IsExistsEntity(entityId, type))
            {
                return string.Empty;
            }

            switch (type)
            {
                case EntityType.Product:
                    return UrlService.GetAbsoluteLink("/admin/Product.aspx?ProductId=" + entityId);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}