//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//-------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;

namespace AdvantShop.Module.Blog.Domain
{
    public class BlogService
    {
        #region Blog records
        public static BlogItem GetBlogItemFromReader(SqlDataReader reader)
        {
            return new BlogItem
            {
                ItemId = ModulesRepository.ConvertTo<int>(reader, "ItemId"),
                ItemCategoryId = ModulesRepository.ConvertTo<int?>(reader, "ItemCategoryId"),
                Title = ModulesRepository.ConvertTo<string>(reader, "Title"),
                UrlPath = ModulesRepository.ConvertTo<string>(reader, "UrlPath"),
                AddingDate = ModulesRepository.ConvertTo<DateTime>(reader, "AddingDate"),
                Picture = ModulesRepository.ConvertTo<string>(reader, "Picture"),
                ShowOnMainPage = ModulesRepository.ConvertTo<bool>(reader, "ShowOnMainPage"),
                Enabled = ModulesRepository.ConvertTo<bool>(reader, "Enabled"),
                TextAnnotation = ModulesRepository.ConvertTo<string>(reader, "TextAnnotation"),
                TextToPublication = ModulesRepository.ConvertTo<string>(reader, "TextToPublication"),
                TextToEmail = ModulesRepository.ConvertTo<string>(reader, "TextToEmail"),
                MetaTitle = ModulesRepository.ConvertTo<string>(reader, "MetaTitle"),
                MetaKeywords = ModulesRepository.ConvertTo<string>(reader, "MetaKeywords"),
                MetaDescription = ModulesRepository.ConvertTo<string>(reader, "MetaDescription")
            };
        }

        public static BlogItem GetBlogItem(int itemId)
        {
            return
                ModulesRepository.ModuleExecuteReadOne<BlogItem>(
                    "SELECT * FROM [Module].[BlogItem] WHERE [ItemId] = @ItemId", CommandType.Text,
                    GetBlogItemFromReader, new SqlParameter("@ItemId", itemId));
        }

        public static BlogItem GetBlogItem(string url)
        {
            return
                ModulesRepository.ModuleExecuteReadOne<BlogItem>(
                    "SELECT * FROM [Module].[BlogItem] WHERE [UrlPath] = @UrlPath", CommandType.Text,
                    GetBlogItemFromReader, new SqlParameter("@UrlPath", url));
        }

        public static List<BlogItem> GetListBlogItem(bool enabled)
        {
            return ModulesRepository.ModuleExecuteReadList<BlogItem>(
                "SELECT * FROM [Module].[BlogItem]" + (enabled ? " WHERE [Enabled]=1" : string.Empty) + " ORDER BY [AddingDate] Desc",
                CommandType.Text,
                GetBlogItemFromReader);
        }

        public static List<BlogItem> GetListBlogItemByCategory(int itemCategoryId, bool enabled)
        {
            return ModulesRepository.ModuleExecuteReadList<BlogItem>(
                "SELECT * FROM [Module].[BlogItem] WHERE [ItemCategoryId]=@ItemCategoryId" + (enabled ? " and [Enabled]=1" : string.Empty) + " ORDER BY [AddingDate] Desc",
                CommandType.Text,
                GetBlogItemFromReader,
                new SqlParameter("@ItemCategoryId", itemCategoryId));
        }

        public static void AddBlogItem(BlogItem blogItem)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "INSERT INTO [Module].[BlogItem]" +
                "([ItemCategoryId],[Title],[UrlPath],[AddingDate],[Picture],[ShowOnMainPage],[TextAnnotation],[TextToPublication],[TextToEmail],[MetaTitle],[MetaKeywords],[MetaDescription],[Enabled]) " +
                "VALUES (@ItemCategoryId,@Title,@UrlPath,GETDATE(),@Picture,@ShowOnMainPage,@TextAnnotation,@TextToPublication,@TextToEmail,@MetaTitle,@MetaKeywords,@MetaDescription,@Enabled)",
                CommandType.Text,
                new SqlParameter("@ItemCategoryId", blogItem.ItemCategoryId ?? (object)DBNull.Value),
                new SqlParameter("@Title", blogItem.Title),
                new SqlParameter("@UrlPath", blogItem.UrlPath),
                new SqlParameter("@Picture", string.IsNullOrEmpty(blogItem.Picture) ? DBNull.Value : (object)blogItem.Picture),
                new SqlParameter("@ShowOnMainPage", blogItem.ShowOnMainPage),
                new SqlParameter("@TextAnnotation", string.IsNullOrEmpty(blogItem.TextAnnotation) ? DBNull.Value : (object)blogItem.TextAnnotation),
                new SqlParameter("@TextToPublication", string.IsNullOrEmpty(blogItem.TextToPublication) ? DBNull.Value : (object)blogItem.TextToPublication),
                new SqlParameter("@TextToEmail", string.IsNullOrEmpty(blogItem.TextToEmail) ? DBNull.Value : (object)blogItem.TextToEmail),
                new SqlParameter("@MetaTitle", blogItem.MetaTitle),
                new SqlParameter("@MetaKeywords", blogItem.MetaKeywords),
                new SqlParameter("@MetaDescription", blogItem.MetaDescription),
                new SqlParameter("@Enabled", blogItem.Enabled));
        }

        public static void UpdateBlogItem(BlogItem blogItem)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "UPDATE [Module].[BlogItem] SET " +
                "[ItemCategoryId]=@ItemCategoryId,[Title]=@Title,[UrlPath]=@UrlPath,[AddingDate]=@AddingDate,[Picture]=@Picture,[ShowOnMainPage]=@ShowOnMainPage,[Enabled]=@Enabled," +
                "[TextAnnotation]=@TextAnnotation,[TextToPublication]=@TextToPublication,[TextToEmail]=@TextToEmail,[MetaTitle]=@MetaTitle,[MetaKeywords]=@MetaKeywords,[MetaDescription]=@MetaDescription " +
                "WHERE [ItemId] = @ItemId",
                CommandType.Text,
                new SqlParameter("@ItemCategoryId", blogItem.ItemCategoryId ?? (object)DBNull.Value),
                new SqlParameter("@Title", blogItem.Title),
                new SqlParameter("@UrlPath", blogItem.UrlPath),
                new SqlParameter("@AddingDate", blogItem.AddingDate),
                new SqlParameter("@Picture", string.IsNullOrEmpty(blogItem.Picture) ? DBNull.Value : (object)blogItem.Picture),
                new SqlParameter("@ShowOnMainPage", blogItem.ShowOnMainPage),
                new SqlParameter("@TextAnnotation", string.IsNullOrEmpty(blogItem.TextAnnotation) ? DBNull.Value : (object)blogItem.TextAnnotation),
                new SqlParameter("@TextToPublication", string.IsNullOrEmpty(blogItem.TextToPublication) ? DBNull.Value : (object)blogItem.TextToPublication),
                new SqlParameter("@TextToEmail", string.IsNullOrEmpty(blogItem.TextToEmail) ? DBNull.Value : (object)blogItem.TextToEmail),
                new SqlParameter("@MetaTitle", blogItem.MetaTitle),
                new SqlParameter("@MetaKeywords", blogItem.MetaKeywords),
                new SqlParameter("@MetaDescription", blogItem.MetaDescription),
                new SqlParameter("@Enabled", blogItem.Enabled),
                new SqlParameter("@ItemId", blogItem.ItemId));
        }

        public static void DeleteBlogItem(int itemId)
        {
            var products = GetProducts(itemId);
            foreach(var product in products)
            {
                DeleteProduct(itemId, product.ProductId);
            }
            ModulesRepository.ModuleExecuteNonQuery(
                "DELETE FROM [Module].[BlogItem] WHERE [ItemId]=@itemId",
                CommandType.Text,
                new SqlParameter("@itemId", itemId));
        }

        #endregion

        #region Blog Categories
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static BlogCategory GetBlogCategoryFromReader(SqlDataReader reader)
        {
            return new BlogCategory
            {
                ItemCategoryId = ModulesRepository.ConvertTo<int>(reader, "ItemCategoryId"),
                Name = ModulesRepository.ConvertTo<string>(reader, "Name"),
                UrlPath = ModulesRepository.ConvertTo<string>(reader, "UrlPath"),
                SortOrder = ModulesRepository.ConvertTo<int>(reader, "SortOrder"),
                CountItems = ModulesRepository.ConvertTo<int>(reader, "CountItems"),
                MetaTitle = ModulesRepository.ConvertTo<string>(reader, "MetaTitle"),
                MetaKeywords = ModulesRepository.ConvertTo<string>(reader, "MetaKeywords"),
                MetaDescription = ModulesRepository.ConvertTo<string>(reader, "MetaDescription")
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemCategoryId"></param>
        /// <returns></returns>
        public static BlogCategory GetBlogCategory(int itemCategoryId)
        {
            return ModulesRepository.ModuleExecuteReadOne<BlogCategory>(
                "SELECT *,(Select Count(ItemID) FROM [Module].[BlogItem] WHERE ItemCategoryID = [Module].[BlogCategory].[ItemCategoryID]) as CountItems FROM [Module].[BlogCategory] WHERE [ItemCategoryId] = @itemCategoryId",
                CommandType.Text,
                GetBlogCategoryFromReader,
                new SqlParameter("@itemCategoryId", itemCategoryId));
        }

        public static BlogCategory GetBlogCategory(string url)
        {
            return ModulesRepository.ModuleExecuteReadOne<BlogCategory>(
                "SELECT *,(Select Count(ItemID) FROM [Module].[BlogItem] WHERE ItemCategoryID = [Module].[BlogCategory].[ItemCategoryID]) as CountItems FROM [Module].[BlogCategory] WHERE [UrlPath] = @UrlPath",
                CommandType.Text,
                GetBlogCategoryFromReader,
                new SqlParameter("@UrlPath", url));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<BlogCategory> GetListBlogCategory()
        {
            return ModulesRepository.ModuleExecuteReadList<BlogCategory>(
                "SELECT *,(Select Count(ItemID) FROM [Module].[BlogItem] WHERE ItemCategoryID = [Module].[BlogCategory].[ItemCategoryID]) as CountItems  FROM [Module].[BlogCategory] ORDER BY [SortOrder]",
                CommandType.Text,
                GetBlogCategoryFromReader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogCategory"></param>
        public static void AddBlogCategory(BlogCategory blogCategory)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "INSERT INTO [Module].[BlogCategory] ([Name],[UrlPath],[SortOrder],MetaTitle,MetaKeywords,MetaDescription) VALUES (@Name,@UrlPath,@SortOrder,@MetaTitle,@MetaKeywords,@MetaDescription)",
                CommandType.Text,
                new SqlParameter("@Name", blogCategory.Name),
                new SqlParameter("@UrlPath", blogCategory.UrlPath),
                new SqlParameter("@SortOrder", blogCategory.SortOrder),
                new SqlParameter("@MetaTitle", blogCategory.MetaTitle ?? string.Empty),
                new SqlParameter("@MetaKeywords", blogCategory.MetaKeywords ?? string.Empty),
                new SqlParameter("@MetaDescription", blogCategory.MetaDescription ?? string.Empty));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogCategory"></param>
        public static void UpdateBlogCategory(BlogCategory blogCategory)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "UPDATE [Module].[BlogCategory] " +
                "SET [Name] = @Name, [UrlPath]=@UrlPath, [SortOrder]=@SortOrder, MetaTitle=@MetaTitle, MetaKeywords=@MetaKeywords, MetaDescription=@MetaDescription " +
                "WHERE [ItemCategoryId]=@ItemCategoryId",
                CommandType.Text,
                new SqlParameter("@Name", blogCategory.Name),
                new SqlParameter("@UrlPath", blogCategory.UrlPath),
                new SqlParameter("@SortOrder", blogCategory.SortOrder),
                new SqlParameter("@ItemCategoryId", blogCategory.ItemCategoryId),
                new SqlParameter("@MetaTitle", blogCategory.MetaTitle ?? string.Empty),
                new SqlParameter("@MetaKeywords", blogCategory.MetaKeywords ?? string.Empty),
                new SqlParameter("@MetaDescription", blogCategory.MetaDescription ?? string.Empty));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemCategoryId"></param>
        public static void DeleteBlogcategory(int itemCategoryId)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "DELETE FROM [Module].[BlogCategory] WHERE [ItemCategoryId] = @itemCategoryId",
                CommandType.Text,
                new SqlParameter("@itemCategoryId", itemCategoryId));
        }

        #endregion

        #region Helpers

        public static bool InstallBlogModule()
        {
            //"Alter table [Module].BlogItem Alter Column [ItemCategoryId] int NULL"
            if (!ModulesRepository.IsExistsModuleTable("Module", "BlogItem"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                    @"CREATE TABLE [Module].[BlogItem](
                        [ItemId] [int] IDENTITY(1,1) NOT NULL,
                        [ItemCategoryId] [int] NULL,
                        [Title] [nvarchar](255) NOT NULL,
	                    [Picture] [nvarchar](150) NULL,
	                    [TextToPublication] [nvarchar](max) NULL,
	                    [TextToEmail] [nvarchar](max) NULL,
	                    [TextAnnotation] [nvarchar](max) NULL,
	                    [ShowOnMainPage] [bit] NOT NULL,
	                    [AddingDate] [datetime] NOT NULL,
	                    [UrlPath] [nvarchar](150) NOT NULL,
	                    [MetaTitle] [nvarchar](255) NOT NULL,
	                    [MetaKeywords] [nvarchar](max) NOT NULL,
	                    [MetaDescription] [nvarchar](max) NOT NULL,
	                    [Enabled] [bit] NOT NULL,
                        CONSTRAINT [PK_BlogItem] PRIMARY KEY CLUSTERED ([ItemId] ASC)
                        WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]) ON [PRIMARY]",
                    CommandType.Text);
            }

            if (!ModulesRepository.IsExistsModuleTable("Module", "BlogProduct"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                    @"CREATE TABLE [Module].[BlogProduct](
                        [BlogId] [int] NOT NULL,
	                    [ProductId] [int] NOT NULL,
                        CONSTRAINT [PK_BlogProduct_BlogId] PRIMARY KEY CLUSTERED ([BlogId] ASC,[ProductId] ASC)
                        WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]) ON [PRIMARY]",
                    CommandType.Text);

                ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE [Module].[BlogProduct]  WITH CHECK ADD  CONSTRAINT [FK_BlogProduct_BlogItem] FOREIGN KEY([BlogId])
                                                        REFERENCES [Module].[BlogItem] ([ItemId])", CommandType.Text);
                ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE [Module].[BlogProduct]  WITH CHECK ADD  CONSTRAINT [FK_BlogProduct_Product] FOREIGN KEY([ProductId])
                                                        REFERENCES [Catalog].[Product] ([ProductId])", CommandType.Text);
            }

            if (!ModulesRepository.IsExistsModuleTable("Module", "BlogCategory"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                    @"CREATE TABLE [Module].[BlogCategory](
	                    [ItemCategoryId] [int] IDENTITY(1,1) NOT NULL,
	                    [Name] [nvarchar](50) NOT NULL,
	                    [SortOrder] [int] NOT NULL,
	                    [UrlPath] [nvarchar](150) NOT NULL,
                        [MetaTitle] [nvarchar](max) NOT NULL,
	                    [MetaKeywords] [nvarchar](max) NOT NULL,
	                    [MetaDescription] [nvarchar](max) NOT NULL,
                        CONSTRAINT [PK_BlogCategory] PRIMARY KEY CLUSTERED ([ItemCategoryId] ASC)
                    WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]) ON [PRIMARY]",
                    CommandType.Text);

                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {

                    case "ru":
                        ModulesRepository.ModuleExecuteNonQuery(
                            @"INSERT INTO [Module].[BlogCategory] ([Name],[SortOrder],[UrlPath], [MetaTitle], [MetaKeywords], [MetaDescription]) VALUES ('Основная','0','main', 'Основная', '', '')",
                            CommandType.Text);
                        break;
                    case "en":
                        ModulesRepository.ModuleExecuteNonQuery(
                            "INSERT INTO [Module].[BlogCategory] ([Name],[SortOrder],[UrlPath], [MetaTitle], [MetaKeywords], [MetaDescription]) VALUES ('Main','0','main', 'Main', '', '')",
                            CommandType.Text);
                        break;
                }
            }

            ModuleSettingsProvider.SetSettingValue("PageUrlPath", "blog", Blog.ModuleID);
            ModuleSettingsProvider.SetSettingValue("MaxImageWidth", 140, Blog.ModuleID);
            ModuleSettingsProvider.SetSettingValue("MaxImageHeight", 140, Blog.ModuleID);
            ModuleSettingsProvider.SetSettingValue("ShowAddDate", true, Blog.ModuleID);

            switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
            {
                case "ru":
                    ModuleSettingsProvider.SetSettingValue("PageTitle", "Блог", Blog.ModuleID);
                    ModuleSettingsProvider.SetSettingValue("MetaTitle", "Блог", Blog.ModuleID);
                    ModuleSettingsProvider.SetSettingValue("MetaKeywords", "Блог", Blog.ModuleID);
                    ModuleSettingsProvider.SetSettingValue("MetaDescription", "Блог", Blog.ModuleID);
                    ModuleSettingsProvider.SetSettingValue("CategoriesListTitle", "Категории записей", Blog.ModuleID);

                    break;
                case "en":
                    ModuleSettingsProvider.SetSettingValue("PageTitle", "Blog", Blog.ModuleID);
                    ModuleSettingsProvider.SetSettingValue("MetaTitle", "Blog", Blog.ModuleID);
                    ModuleSettingsProvider.SetSettingValue("MetaKeywords", "Blog", Blog.ModuleID);
                    ModuleSettingsProvider.SetSettingValue("MetaDescription", "Blog", Blog.ModuleID);
                    ModuleSettingsProvider.SetSettingValue("CategoriesListTitle", "Record categories", Blog.ModuleID);
                    break;
            }

            return ModulesRepository.IsExistsModuleTable("Module", "BlogItem") && ModulesRepository.IsExistsModuleTable("Module", "BlogCategory");
        }

        public static bool UpdateBlogModule()
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'MetaTitle' AND [object_id] = OBJECT_ID(N'Module.BlogCategory')) " +
                "BEGIN " +
                    "Alter table Module.BlogCategory add MetaTitle nvarchar(MAX) " +
                    "Alter table Module.BlogCategory add MetaKeywords nvarchar(MAX) " +
                    "Alter table Module.BlogCategory add MetaDescription nvarchar(MAX) " +
                "END",
                CommandType.Text);

            ModuleSettingsProvider.SetSettingValue("PageUrlPath", "blog", Blog.ModuleID);

            if (!ModulesRepository.IsExistsModuleTable("Module", "BlogProduct"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                    @"CREATE TABLE [Module].[BlogProduct](
                        [BlogId] [int] NOT NULL,
	                    [ProductId] [int] NOT NULL,
                        CONSTRAINT [PK_BlogProduct_BlogId] PRIMARY KEY CLUSTERED ([BlogId] ASC,[ProductId] ASC)
                        WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]) ON [PRIMARY]",
                    CommandType.Text);

                ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE [Module].[BlogProduct]  WITH CHECK ADD  CONSTRAINT [FK_BlogProduct_BlogItem] FOREIGN KEY([BlogId])
                                                        REFERENCES [Module].[BlogItem] ([ItemId]) ON UPDATE NO ACTION  ON DELETE  CASCADE ", CommandType.Text);
                ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE [Module].[BlogProduct]  WITH CHECK ADD  CONSTRAINT [FK_BlogProduct_Product] FOREIGN KEY([ProductId])
                                                        REFERENCES [Catalog].[Product] ([ProductId]) ON UPDATE NO ACTION ON DELETE  CASCADE ", CommandType.Text);
            }

            else 
            {
                // delete reference to create the new one with cascade rules
                if (ModulesRepository.ModuleExecuteScalar<int>(@"SELECT COUNT(*) FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_BlogProduct_BlogItem' AND DELETE_RULE <> 'CASCADE'", CommandType.Text) > 0)
                {
                    ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE Module.BlogProduct DROP CONSTRAINT FK_BlogProduct_BlogItem", CommandType.Text);
                    ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE [Module].[BlogProduct]  WITH CHECK ADD  CONSTRAINT [FK_BlogProduct_BlogItem] FOREIGN KEY([BlogId])
                                                        REFERENCES [Module].[BlogItem] ([ItemId]) ON UPDATE NO ACTION  ON DELETE  CASCADE", CommandType.Text);
                }
                if (ModulesRepository.ModuleExecuteScalar<int>(@"SELECT COUNT(*) FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_BlogProduct_Product' AND DELETE_RULE <> 'CASCADE'", CommandType.Text) > 0)
                {
                    ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE Module.BlogProduct DROP CONSTRAINT FK_BlogProduct_Product", CommandType.Text);
                    ModulesRepository.ModuleExecuteNonQuery(@"ALTER TABLE [Module].[BlogProduct]  WITH CHECK ADD  CONSTRAINT [FK_BlogProduct_Product] FOREIGN KEY([ProductId])
                                                        REFERENCES [Catalog].[Product] ([ProductId]) ON UPDATE NO ACTION ON DELETE  CASCADE", CommandType.Text);
                }
            }

            return true;
        }

        public static void SaveAndResizeImage(Image image, string resultPath)
        {
            double resultWidth = image.Width;  // 0;
            double resultHeight = image.Height; // 0;

            var maxWidth = ModuleSettingsProvider.GetSettingValue<double>("MaxImageWidth", Blog.ModuleID);
            var maxHeight = ModuleSettingsProvider.GetSettingValue<double>("MaxImageHeight", Blog.ModuleID);

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

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/modules/blog/pictures/")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/modules/blog/pictures/"));
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
                Debug.Log.Error(ex);
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

        #endregion

        public static List<Product> GetProducts(int blogId)
        {

            if (!ModulesRepository.IsExistsModuleTable("Module", "BlogProduct"))
            {
                UpdateBlogModule();
            }

            return SQLDataAccess.Query<Product>(
                "Select Product.ProductID, Product.ArtNo, Product.Name, Product.UrlPath " +
                "From [Catalog].[Product] " +
                "Inner Join [Module].[BlogProduct] ON [Product].[ProductID] = [BlogProduct].[ProductId] " +
                "Where [BlogProduct].[BlogId] = @BlogId",
                new
                {
                    BlogId = blogId
                })
                .ToList();
        }

        public static void AddProduct(int blogId, int productId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "IF (SELECT COUNT(*) FROM [Module].[BlogProduct] WHERE BlogId = @BlogId AND ProductId = @ProductId) = 0 " +
                "INSERT INTO [Module].[BlogProduct] (BlogId, ProductId) VALUES (@BlogId, @ProductId)",
                CommandType.Text,
                new SqlParameter("@BlogId", blogId),
                new SqlParameter("@Productid", productId));
        }

        public static void DeleteProduct(int blogId, int productId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM [Module].[BlogProduct] WHERE BlogId = @BlogId AND ProductId = @ProductId",
                CommandType.Text,
                new SqlParameter("@BlogId", blogId),
                new SqlParameter("@Productid", productId));
        }

        public static List<ProductModel> GetBlogProductModels(int itemId)
        {
            if (!ModulesRepository.IsExistsModuleTable("Module", "BlogProduct"))
            {
                UpdateBlogModule();
            }

            return SQLDataAccess.Query<ProductModel>(
              "Select Product.ProductID, Product.ArtNo, Product.Name, Product.BriefDescription, Product.Recomended, Product.Bestseller, Product.New, Product.OnSale as Sale, " +
              "Product.Enabled, Product.UrlPath, Product.AllowPreOrder, Product.Ratio, Product.Discount, Product.MinAmount, Product.MaxAmount, Product.DateAdded, " +
              "Offer.OfferID, Offer.ColorID, MaxAvailable AS Amount, MinPrice as BasePrice, Colors, CurrencyValue, " +
              "CountPhoto, Photo.PhotoId, PhotoName, Photo.Description as PhotoDescription, NotSamePrices as MultiPrices " +
              "From [Catalog].[Product] " +
              "Left Join [Catalog].[ProductExt]  On [Product].[ProductID] = [ProductExt].[ProductID]  " +
              "Inner Join [Module].[BlogProduct] ON [Product].[ProductID] = [BlogProduct].[ProductId] " +
              "Inner Join [Catalog].[Currency] On [Currency].[CurrencyID] = [Product].[CurrencyID] " +
              "Left Join [Catalog].[Photo] On [Photo].[PhotoId] = [ProductExt].[PhotoId] " +
              "Left Join [Catalog].[Offer] On [ProductExt].[OfferID] = [Offer].[OfferID] " +
              "Where [BlogProduct].[BlogId] = @BlogId and Product.Enabled=1 and CategoryEnabled=1 " +
              "order by AmountSort DESC, (CASE WHEN Price=0 THEN 0 ELSE 1 END) DESC",
              new
              {
                  BlogId = itemId,
                  Type = PhotoType.Product.ToString()
              })
              .ToList();
        }
    }
}