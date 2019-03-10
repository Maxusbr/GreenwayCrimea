using AdvantShop.Catalog;
using AdvantShop.Core.Modules;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Module.CategoriesOnMainPage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AdvantShop.Module.CategoriesOnMainPage.Service
{
    public class COMPService
    {
        public static bool Query(string query, SqlParameter[] parameters = null)
        {
            try
            {
                if (parameters != null)
                {
                    ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text, parameters);
                }
                else
                {
                    ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text);
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return false;
            }
        }

        public static bool Install()
        {
            var install = COMPSettings.SetDefaultSettings();

            var isSuccessQuery = ModulesRepository.IsExistsModuleTable("Module", "CategoriesOnMainPage");
            if (!isSuccessQuery)
            {
                isSuccessQuery = Query(@"CREATE TABLE [Module].[CategoriesOnMainPage] (
	                                    [CategoryId] [int] NOT NULL, 
                                        [Name] [nvarchar](250) NOT NULL,
	                                    [ImageUrl] [nvarchar](250) NULL,
										[URL] [nvarchar](max) NOT NULL,
										[SortOrder] [int] NOT NULL,
                                    ) ON [PRIMARY];
                                    ALTER TABLE [Module].[CategoriesOnMainPage] ADD CONSTRAINT FK_CategoriesOnMainPage_CategoryId FOREIGN KEY ([CategoryId]) REFERENCES [Catalog].[Category] (CategoryID) ON UPDATE NO ACTION ON DELETE CASCADE;");

            }

            return isSuccessQuery && install;
        }

        public static bool UnInstall()
        {
            var uninstall = COMPSettings.RemoveSettings();

            var isDropped = true;
            if (ModulesRepository.IsExistsModuleTable("Module", "CategoriesOnMainPage"))
            {
                isDropped = Query("DROP TABLE [Module].[CategoriesOnMainPage]");
            }

            return isDropped && uninstall;
        }
        
        public static bool Update()
        {
            var isUpdated = true;
            if (ModulesRepository.IsExistsModuleTable("Module", "CategoriesOnMainPage"))
            {
                if (IsExistColumn("CategoryName"))
                {
                    isUpdated &= Query("ALTER TABLE [Module].[CategoriesOnMainPage] DROP COLUMN [CategoryName]");
                }

                if (!IsExistColumn("Name"))
                {
                    isUpdated = Query("ALTER TABLE [Module].[CategoriesOnMainPage] ADD [Name] [nvarchar](250) NOT NULL DEFAULT ''");
                }
            }

            return isUpdated;
        }

        private static bool IsExistColumn(string columnName)
        {
            return ModulesRepository.ModuleExecuteScalar<int>(
                "IF COLUMNPROPERTY(OBJECT_ID('[Module].[CategoriesOnMainPage]'), '" + columnName + "', 'AllowsNull') IS NOT NULL SELECT 1 ELSE SELECT 0", 
                CommandType.Text) > 0;
        }

        //----------------------------------------------------------------------------------

        public static List<Category> GetAllCategories()
        {
            return ModulesRepository.ModuleExecuteReadList<Category>(
                "SELECT [CategoryID], [Name], [ParentCategory], [SortOrder] FROM [Catalog].[Category] Where [CategoryId] <> 0 AND [Enabled] = 1 AND [HirecalEnabled] = 1",
                    CommandType.Text,
                    reader => new Category
                    {
                        CategoryId = SQLDataHelper.GetInt(reader, "CategoryId"),
                        Name = SQLDataHelper.GetString(reader, "Name"),
                        ParentCategoryId = SQLDataHelper.GetInt(reader, "ParentCategory"),
                        SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                    });
        }

        public static string GetCategoryUrlById(int categoryId)
        {
            return ModulesRepository.ModuleExecuteScalar<string>(
                "SELECT [UrlPath] FROM [Catalog].[Category] WHERE [CategoryID] = @CategoryID",
                CommandType.Text,
                new SqlParameter("@CategoryID", categoryId));
        }

        public static List<CategoryOnMainPage> GetCategories()
        {
            return ModulesRepository.ModuleExecuteReadList<CategoryOnMainPage>(
                "SELECT [CategoryId], [Name], [ImageUrl], [URL], [SortOrder] FROM [Module].[CategoriesOnMainPage] ORDER BY [SortOrder]",
                    CommandType.Text,
                    reader => new CategoryOnMainPage
                    {
                        CategoryId = SQLDataHelper.GetInt(reader, "CategoryId"),
                        Name = SQLDataHelper.GetString(reader, "Name"),
                        ImageUrl = SQLDataHelper.GetString(reader, "ImageUrl"),
                        URL = SQLDataHelper.GetString(reader, "URL"),
                        SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                    });
        }

        public static CategoryOnMainPage GetCategory(int categoryId)
        {
            return ModulesRepository.ModuleExecuteReadOne<CategoryOnMainPage>(
                "SELECT [CategoryId], [Name], [ImageUrl], [URL], [SortOrder] FROM [Module].[CategoriesOnMainPage] WHERE [CategoryId] = @CategoryID",
                    CommandType.Text,
                    reader => new CategoryOnMainPage
                    {
                        CategoryId = SQLDataHelper.GetInt(reader, "CategoryId"),
                        Name = SQLDataHelper.GetString(reader, "Name"),
                        ImageUrl = SQLDataHelper.GetString(reader, "ImageUrl"),
                        URL = SQLDataHelper.GetString(reader, "URL"),
                        SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                    },
                    new SqlParameter("@CategoryID", categoryId));
        }

        public static bool AddCategory(CategoryOnMainPage category)
        {
            return ModulesRepository.ModuleExecuteScalar<bool>(
                "IF (SELECT COUNT([CategoryId]) FROM [Module].[CategoriesOnMainPage] WHERE [CategoryID] = @CategoryID) = 0" +
                "BEGIN INSERT INTO [Module].[CategoriesOnMainPage] ([CategoryId], [Name], [ImageUrl], [URL], [SortOrder]) VALUES (@CategoryID, @Name, @ImageUrl, @URL, @SortOrder); SELECT 1 END " +
                "ELSE SELECT 0",
                CommandType.Text,
                new SqlParameter("@CategoryID", category.CategoryId),
                new SqlParameter("@Name", category.Name),
                new SqlParameter("@ImageUrl", category.ImageUrl ?? (object)DBNull.Value),
                new SqlParameter("@URL", category.URL ?? (object)DBNull.Value),
                new SqlParameter("@SortOrder", category.SortOrder));
        }

        public static void UpdateCategory(CategoryOnMainPage category)
        {
            //int CategoryIdnew = COMPService.GetCategoryIdByName(category);
            //if (CategoryIdnew != 0)
            //{
            ModulesRepository.ModuleExecuteNonQuery("UPDATE [Module].[CategoriesOnMainPage] SET " +
                "[Name] = @Name, [ImageUrl] = @ImageUrl, [URL] = @URL, [SortOrder] = @SortOrder WHERE [CategoryID] = @CategoryID",
                CommandType.Text,
                new SqlParameter("@CategoryID", category.CategoryId),
                new SqlParameter("@Name", category.Name),
                new SqlParameter("@ImageUrl", category.ImageUrl ?? (object)DBNull.Value),
                new SqlParameter("@URL", category.URL ?? (object)DBNull.Value),
                new SqlParameter("@SortOrder", category.SortOrder));

            //    ModulesRepository.ModuleExecuteNonQuery("UPDATE [Module].[CategoriesOnMainPage] SET " +
            //    "[CategoryID] = " + CategoryIdnew + "" +
            //    "WHERE [ImageUrl] = @ImageUrl",
            //    CommandType.Text,
            //    new SqlParameter("@CategoryID", CategoryIdnew),
            //    new SqlParameter("@ImageUrl", category.ImageUrl ?? (object)DBNull.Value));
            //}
        }

        public static bool DeleteCategory(int categoryId)
        {
            //if (CategoryImageService.ExistPicture(categoryId)) { CategoryImageService.RemovePicture(categoryId); }

            CategoryImageService.RemovePicture(categoryId, true);

            return ModulesRepository.ModuleExecuteScalar<bool>(
                "DELETE FROM [Module].[CategoriesOnMainPage] WHERE [CategoryId] = @CategoryID", CommandType.Text, new SqlParameter("@CategoryID", categoryId));
        }

        public static string GetCategoryNameByCategoryId(int categoryId)
        {
            return ModulesRepository.ModuleExecuteScalar<string>("SELECT [Name] FROM [Catalog].[Category] WHERE [CategoryID] = @CategoryID",
                    CommandType.Text, new SqlParameter("@CategoryID", categoryId));
        }
        ////получаем имя для Update
        //public static int GetCategoryIdByName(CategoryOnMainPage category)
        //{
        //    return ModulesRepository.ModuleExecuteScalar<int>("SELECT [CategoryID] FROM [Catalog].[Category] WHERE [Name] = @Name",
        //    CommandType.Text, new SqlParameter("@Name", category.NameUpCategory));
        //}

        public static int GetMaxSortOrder()
        {
            return SQLDataHelper.GetInt(ModulesRepository.ModuleExecuteScalar<int>("SELECT MAX([SortOrder]) FROM [Module].[CategoriesOnMainPage]", CommandType.Text)) + 10;
        }

        public static string GetPath(string imagePathBase)
        {
            return System.Web.Hosting.HostingEnvironment.MapPath("~/") + imagePathBase;
        }
    }
}
