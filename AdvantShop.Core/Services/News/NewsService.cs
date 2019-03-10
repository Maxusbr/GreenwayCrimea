//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

using AdvantShop.SEO;
using System;

namespace AdvantShop.News
{
    public class NewsService
    {
        #region NewsCategory

        public static int InsertNewsCategory(NewsCategory newsCategory)
        {
            var id =
                SQLDataAccess.ExecuteScalar<int>(
                    "Insert into [Settings].[NewsCategory] ([Name],[SortOrder],[UrlPath]) values (@Name,@SortOrder,@UrlPath); Select SCOPE_IDENTITY ();",
                    CommandType.Text,
                    new SqlParameter("@Name", newsCategory.Name),
                    new SqlParameter("@UrlPath", newsCategory.UrlPath),
                    new SqlParameter("@SortOrder", newsCategory.SortOrder));

            if (newsCategory.Meta != null)
            {
                if (!newsCategory.Meta.Title.IsNullOrEmpty() || !newsCategory.Meta.MetaKeywords.IsNullOrEmpty() || !newsCategory.Meta.MetaDescription.IsNullOrEmpty() || !newsCategory.Meta.H1.IsNullOrEmpty())
                {
                    newsCategory.Meta.ObjId = id;
                    MetaInfoService.SetMeta(newsCategory.Meta);
                }
            }
            return id;
        }

        public static void UpdateNewsCategory(NewsCategory newsCategory)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [Settings].[NewsCategory] set [Name]=@name,[UrlPath] = @UrlPath , [SortOrder] = @SortOrder where NewsCategoryID = @NewsCategoryID",
                CommandType.Text,
                new SqlParameter("@NewsCategoryID", newsCategory.NewsCategoryId),
                new SqlParameter("@Name", newsCategory.Name),
                new SqlParameter("@UrlPath", newsCategory.UrlPath),
                new SqlParameter("@SortOrder", newsCategory.SortOrder));

            if (newsCategory.Meta != null)
            {
                if (newsCategory.Meta.Title.IsNullOrEmpty() && newsCategory.Meta.MetaKeywords.IsNullOrEmpty() && newsCategory.Meta.MetaDescription.IsNullOrEmpty() && newsCategory.Meta.H1.IsNullOrEmpty())
                {
                    if (MetaInfoService.IsMetaExist(newsCategory.NewsCategoryId, MetaType.NewsCategory))
                        MetaInfoService.DeleteMetaInfo(newsCategory.NewsCategoryId, MetaType.NewsCategory);
                }
                else
                    MetaInfoService.SetMeta(newsCategory.Meta);
            }
        }

        public static void DeleteNewsCategory(int newsCategoryId)
        {
            foreach (var id in GetNewsByCategoryID(newsCategoryId).Select(news => news.NewsId))
                DeleteNews(id);

            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Settings].[NewsCategory] WHERE NewsCategoryID=@ID",
                                          CommandType.Text, new SqlParameter("@ID", newsCategoryId));
        }

        public static IEnumerable<NewsCategory> GetNewsCategories(bool onlyEnabledNews = false)
        {
            return SQLDataAccess.ExecuteReadIEnumerable(
                "SELECT *, (Select Count(NewsID) FROM [Settings].[News] WHERE NewsCategoryID = [Settings].[NewsCategory].[NewsCategoryID] " +
                (onlyEnabledNews ? "AND Enabled = 1 " : string.Empty) +
                ") as CountNews FROM [Settings].[NewsCategory] ORDER BY SortOrder",
                CommandType.Text,
                reader => new NewsCategory
                {
                    NewsCategoryId = SQLDataHelper.GetInt(reader, "NewsCategoryID"),
                    Name = SQLDataHelper.GetString(reader, "Name"),
                    SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                    CountNews = SQLDataHelper.GetInt(reader, "CountNews"),
                    UrlPath = SQLDataHelper.GetString(reader, "UrlPath")
                });
        }

        public static NewsCategory GetNewsCategoryById(int id)
        {
            return SQLDataAccess.ExecuteReadOne(
                "SELECT * FROM [Settings].[NewsCategory] where NewsCategoryID=@NewsCategoryID",
                CommandType.Text,
                reader => new NewsCategory
                {
                    NewsCategoryId = SQLDataHelper.GetInt(reader, "NewsCategoryID"),
                    Name = SQLDataHelper.GetString(reader, "Name"),
                    SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                    UrlPath = SQLDataHelper.GetString(reader, "UrlPath")
                },
                new SqlParameter("@NewsCategoryID", id));
        }

        public static NewsCategory GetNewsCategory(string url)
        {
            return SQLDataAccess.ExecuteReadOne(
                "SELECT * FROM [Settings].[NewsCategory] where UrlPath=@UrlPath",
                CommandType.Text,
                reader => new NewsCategory
                {
                    NewsCategoryId = SQLDataHelper.GetInt(reader, "NewsCategoryID"),
                    Name = SQLDataHelper.GetString(reader, "Name"),
                    SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                    UrlPath = SQLDataHelper.GetString(reader, "UrlPath")
                },
                new SqlParameter("@UrlPath", url));
        }

        public static IEnumerable<NewsItem> GetNewsByCategoryID(int categoryID, bool onlyEnabled = false)
        {
            return SQLDataAccess.ExecuteReadIEnumerable(
                "SELECT * FROM [Settings].[News] WHERE NewsCategoryID = @NewsCategoryID " +
                (onlyEnabled ? "AND [Enabled] = 1 " : string.Empty) +
                "ORDER BY [AddingDate], [NewsID] DESC",
                CommandType.Text,
                GetNewsFromReader,
                new SqlParameter("@NewsCategoryID", categoryID));
        }

        #endregion

        #region News

        public static void DeleteNews(int newsId)
        {
            DeleteNewsImage(newsId);
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Settings].[News] WHERE NewsID=@ID", CommandType.Text, new SqlParameter("@ID", newsId));
            ClearCache();
        }

        public static List<NewsItem> GetNews(bool onlyEnabled = false)
        {
            return
                SQLDataAccess.ExecuteReadList(
                    "SELECT * FROM [Settings].[News] " +
                    (onlyEnabled ? "WHERE [Enabled] = 1 " : string.Empty) +
                    "ORDER BY [AddingDate], [NewsID] DESC", CommandType.Text,
                    GetNewsFromReader);
        }
        
        public static void DeleteNewsImage(int newsId)
        {
            PhotoService.DeletePhotos(newsId, PhotoType.News);
            ClearCache();
        }

        public static int GetLastId()
        {
            return SQLDataHelper.GetInt(SQLDataAccess.ExecuteScalar("SELECT max([NewsID])+1 from [Settings].[News]", CommandType.Text));
        }


        // get from cache by name of function if it in cache or from db
        public static List<NewsItem> GetNewsForMainPage()
        {
            return CacheManager.Get(CacheNames.GetNewsForMainPage(), () => { return GetNewsForMainPageFromDb(); });
        }

        private static List<NewsItem> GetNewsForMainPageFromDb()
        {
            return
                SQLDataAccess.ExecuteReadList(
                    "SELECT TOP (@count) * FROM [Settings].[News] WHERE [Enabled] = 1 AND [ShowOnMainPage] = 1 AND AddingDate <= GetDate() ORDER BY [AddingDate] DESC, NewsID DESC",
                    CommandType.Text, GetNewsFromReader,
                    new SqlParameter("@count", SettingsNews.NewsMainPageCount));
        }

        

        public static int InsertNews(NewsItem news)
        {
            var id = SQLDataAccess.ExecuteScalar<int>(
                "Insert Into [Settings].[News] ([NewsCategoryID],[Title],[TextToPublication],[TextToEmail],[TextAnnotation],[ShowOnMainPage],[AddingDate],[UrlPath],[Enabled]) " +
                "Values (@NewsCategoryID, @Title, @TextToPublication, @TextToEmail, @TextAnnotation, @ShowOnMainPage, @AddingDate, @UrlPath, @Enabled); " +
                "Select scope_identity();",
                CommandType.Text,
                new SqlParameter("@NewsCategoryID", news.NewsCategoryId),
                new SqlParameter("@AddingDate", news.AddingDate),
                new SqlParameter("@Title", news.Title),
                new SqlParameter("@TextToPublication", news.TextToPublication ?? ""),
                new SqlParameter("@TextToEmail", news.TextToEmail ?? ""),
                new SqlParameter("@TextAnnotation", news.TextAnnotation ?? ""),
                new SqlParameter("@ShowOnMainPage", news.ShowOnMainPage),
                new SqlParameter("@UrlPath", news.UrlPath),
                new SqlParameter("@Enabled", news.Enabled)
                );

            // ---- Meta
            if (news.Meta != null)
            {
                if (!news.Meta.Title.IsNullOrEmpty() || !news.Meta.MetaKeywords.IsNullOrEmpty() || !news.Meta.MetaDescription.IsNullOrEmpty() || !news.Meta.H1.IsNullOrEmpty())
                {
                    news.Meta.ObjId = id;
                    MetaInfoService.SetMeta(news.Meta);
                }
            }

            ClearCache();

            return id;
        }

        public static bool UpdateNews(NewsItem news)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [Settings].[News] " +
                "Set NewsCategoryID=@NewsCategoryID, Title=@Title, TextToPublication=@TextToPublication, TextToEmail=@TextToEmail, " +
                "TextAnnotation=@TextAnnotation, ShowOnMainPage=@ShowOnMainPage, AddingDate=@AddingDate, UrlPath=@UrlPath, [Enabled]=@Enabled " +
                "Where NewsId=@NewsID",
                CommandType.Text,
                new SqlParameter("@NewsID", news.NewsId),
                new SqlParameter("@NewsCategoryID", news.NewsCategoryId),
                new SqlParameter("@Title", news.Title),
                new SqlParameter("@TextToPublication", news.TextToPublication ?? ""),
                new SqlParameter("@TextToEmail", news.TextToEmail ?? ""),
                new SqlParameter("@TextAnnotation", news.TextAnnotation ?? ""),
                new SqlParameter("@ShowOnMainPage", news.ShowOnMainPage),
                new SqlParameter("@AddingDate", news.AddingDate),
                new SqlParameter("@UrlPath", news.UrlPath),
                new SqlParameter("@Enabled", news.Enabled)
                );

            if (news.Meta != null)
            {
                if (news.Meta.Title.IsNullOrEmpty() && news.Meta.MetaKeywords.IsNullOrEmpty() &&
                    news.Meta.MetaDescription.IsNullOrEmpty() && news.Meta.H1.IsNullOrEmpty())
                {
                    if (MetaInfoService.IsMetaExist(news.NewsId, MetaType.News))
                        MetaInfoService.DeleteMetaInfo(news.NewsId, MetaType.News);
                }
                else
                    MetaInfoService.SetMeta(news.Meta);
            }

            ClearCache();

            return true;
        }

        public static NewsItem GetNewsById(int newsId)
        {
            if (newsId == 0) return null;

            return SQLDataAccess.ExecuteReadOne("SELECT * FROM [Settings].[News] WHERE NewsID=@NewsID",
                CommandType.Text, GetNewsFromReader, new SqlParameter("@NewsID", newsId));
        }

        public static NewsItem GetNews(string url)
        {
            return SQLDataAccess.ExecuteReadOne("SELECT * FROM [Settings].[News] WHERE UrlPath=@UrlPath",
                CommandType.Text, GetNewsFromReader, new SqlParameter("@UrlPath", url));
        }


        private static NewsItem GetNewsFromReader(SqlDataReader reader)
        {
            return new NewsItem
            {
                NewsId = SQLDataHelper.GetInt(reader, "NewsID"),
                NewsCategoryId = SQLDataHelper.GetInt(reader, "NewsCategoryID"),
                Title = SQLDataHelper.GetString(reader, "Title"),
                //Picture = SQLDataHelper.GetString(reader, "Picture"),
                TextToPublication = SQLDataHelper.GetString(reader, "TextToPublication"),
                TextToEmail = SQLDataHelper.GetString(reader, "TextToEmail"),
                TextAnnotation = SQLDataHelper.GetString(reader, "TextAnnotation"),
                ShowOnMainPage = SQLDataHelper.GetBoolean(reader, "ShowOnMainPage"),
                AddingDate = SQLDataHelper.GetDateTime(reader, "AddingDate"),
                UrlPath = SQLDataHelper.GetString(reader, "UrlPath"),
                Enabled = SQLDataHelper.GetBoolean(reader, "Enabled")
            };
        }

        public static void SendNews(string txtTitle, string text)
        {
            foreach (var moduleType in AttachedModules.GetModules<ISendMails>())
            {
                var moduleObject = (ISendMails)Activator.CreateInstance(moduleType, null);
                if (ModulesRepository.IsActiveModule(moduleObject.ModuleStringId))
                {
                    moduleObject.SendMails(txtTitle, text, Core.Services.Mails.EMailRecipientType.Subscriber);
                }
            }
        }

        public static void SetNewsOnMainPage(int newsId, bool showOnMainPage)
        {
            SQLDataAccess.ExecuteNonQuery("Update [Settings].[News] Set ShowOnMainPage = @ShowOnMainPage WHERE NewsID = @NewsID",
                                          CommandType.Text,
                                          new SqlParameter("@NewsID", newsId),
                                          new SqlParameter("@ShowOnMainPage", showOnMainPage));
            ClearCache();
        }

        public static void ChangeNewsEnabled(int newsId, bool enabled)
        {
            SQLDataAccess.ExecuteNonQuery("Update [Settings].[News] Set [Enabled] = @Enabled WHERE NewsID = @NewsID",
                                          CommandType.Text,
                                          new SqlParameter("@NewsID", newsId),
                                          new SqlParameter("@Enabled", enabled));
            ClearCache();
        }

        public static void ChangeCategoryNews(int newsId, int newsCategoryId)
        {
            SQLDataAccess.ExecuteNonQuery("Update [Settings].[News] Set NewsCategoryID = @NewsCategoryID WHERE NewsID = @NewsID",
                                           CommandType.Text,
                                           new SqlParameter("@NewsID", newsId),
                                           new SqlParameter("@NewsCategoryID", newsCategoryId));
            ClearCache();
        }

        public static void ClearCache()
        {
            CacheManager.RemoveByPattern(CacheNames.News);
        }

        #endregion

        #region Products in news

        public static List<ProductModel> GetNewsProductModels(int newsId)
        {
            return SQLDataAccess.Query<ProductModel>(
                "Select Product.ProductID, Product.ArtNo, Product.Name, Product.BriefDescription, Product.Recomended, Product.Bestseller, Product.New, Product.OnSale as Sale, " +
                "Product.Enabled, Product.UrlPath, Product.AllowPreOrder, Product.Ratio, Product.Discount, Product.DiscountAmount, Product.MinAmount, Product.MaxAmount, Product.DateAdded, " +
                "Offer.OfferID, Offer.ColorID, MaxAvailable AS Amount, MinPrice as BasePrice, Colors, CurrencyValue, " +
                "CountPhoto, Photo.PhotoId, PhotoName, Photo.Description as PhotoDescription, NotSamePrices as MultiPrices " +
                "From [Catalog].[Product] " +
                "Left Join [Catalog].[ProductExt]  On [Product].[ProductID] = [ProductExt].[ProductID]  " +
                "Inner Join [CMS].[NewsProduct] ON [Product].[ProductID] = [NewsProduct].[ProductId] " +
                "Inner Join [Catalog].[Currency] On [Currency].[CurrencyID] = [Product].[CurrencyID] " +
                "Left Join [Catalog].[Photo] On [Photo].[PhotoId] = [ProductExt].[PhotoId] " +
                "Left Join [Catalog].[Offer] On [ProductExt].[OfferID] = [Offer].[OfferID] " +
                "Where [NewsProduct].[NewsId] = @NewsId and Product.Enabled=1 and CategoryEnabled=1 " +
                "order by AmountSort DESC, (CASE WHEN Price=0 THEN 0 ELSE 1 END) DESC",
                new
                {
                    NewsId = newsId,
                    Type = PhotoType.Product.ToString()
                })
                .ToList();
        }

        public static List<ProductModel> GetAllNewsProducts(int newsId)
        {
            return SQLDataAccess.Query<ProductModel>(
                "Select Product.ProductID, Product.ArtNo, Product.Name, Product.UrlPath, Photo.PhotoId, PhotoName " +
                "From [Catalog].[Product] " +
                "Inner Join [CMS].[NewsProduct] ON [Product].[ProductID] = [NewsProduct].[ProductId] " +
                "Left Join [Catalog].[ProductExt] On [Product].[ProductID] = [ProductExt].[ProductId] " +
                "Left Join [Catalog].[Photo] ON [Photo].[PhotoId] = [ProductExt].[PhotoId] " +
                "Where [NewsProduct].[NewsId] = @NewsId",
                new
                {
                    NewsId = newsId
                })
                .ToList();
        }

        public static void AddNewsProduct(int newsId, int productId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "IF (SELECT COUNT(*) FROM [CMS].[NewsProduct] WHERE NewsId = @NewsId AND ProductId = @ProductId) = 0 " +
                "INSERT INTO [CMS].[NewsProduct] (NewsId, ProductId) VALUES (@NewsId, @ProductId)",
                CommandType.Text,
                new SqlParameter("@NewsId", newsId),
                new SqlParameter("@Productid", productId));
        }

        public static void DeleteNewsProduct(int newsId, int productId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM [CMS].[NewsProduct] WHERE NewsId = @NewsId AND ProductId = @ProductId", 
                CommandType.Text,
                new SqlParameter("@NewsId", newsId),
                new SqlParameter("@Productid", productId));
        }

        #endregion
    }
}