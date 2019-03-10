using AdvantShop.Catalog;
using AdvantShop.Core.Modules;
using AdvantShop.Module.CategoriesOnMainPage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.IO;
using AdvantShop.Core.Caching;

namespace AdvantShop.Module.CategoriesOnMainPage.Service
{
    public class CategoryImageService
    {
        public static void AddImage(int categoryId, string urlImage)
        {
            if (ExistPicture(categoryId))
            {
                RemovePicture(categoryId, false);
            }

            var category = COMPService.GetCategory(categoryId);
            if(category == null)
            {
                category = new CategoryOnMainPage();
                category.CategoryId = categoryId;
                COMPService.AddCategory(category);
            }

            //var query = @"INSERT INTO [Module]." + CategoryContent.ModuleStringId + " VALUES (" + categoryId + ",'" + urlImage + "')";
            var query = @"UPDATE [Module]." + CategoriesOnMainPage.ModuleStringId + " SET [ImageUrl] = '" + urlImage + "' WHERE [CategoryId] = " + categoryId;
            COMPService.Query(query);
        }

        public static bool ExistPicture(string pictureName)
        {
            var query = @"SELECT COUNT(*) FROM [Module]." + CategoriesOnMainPage.ModuleStringId + " WHERE [ImageUrl] = '" + pictureName + "'";

            var count = ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text);

            return count > 0;
        }

        public static bool ExistPicture(int categoryId)
        {
            var query = @"SELECT COUNT(*) FROM [Module]." + CategoriesOnMainPage.ModuleStringId + " WHERE [CategoryId] = '" + categoryId + "' AND [ImageUrl] IS NOT NULL";

            var count = ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text);

            return count > 0;
        }

        public static void RemovePicture(int categoryId, bool withOriginal)
        {
            var category = COMPService.GetCategory(categoryId);
            var query = @"SELECT [ImageUrl] FROM [Module]." + CategoriesOnMainPage.ModuleStringId + " WHERE [CategoryId] = " + categoryId;
            var fileName = category != null ? category.ImageUrl : ModulesRepository.ModuleExecuteScalar<string>(query, CommandType.Text);

            query = @"UPDATE [Module]." + CategoriesOnMainPage.ModuleStringId + " SET [ImageUrl] = NULL WHERE [CategoryId] = " + categoryId;
            COMPService.Query(query);

            var cacheKey = CacheNames.GetCategoryCacheObjectName(categoryId);
            CacheManager.Remove(cacheKey);

            if (category != null)
            {
                category.ImageUrl = null;
                COMPService.UpdateCategory(category);
            }

            var filePath = COMPService.GetPath("modules/CategoriesOnMainPage/Pictures/" + fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if(withOriginal)
            {
                filePath = COMPService.GetPath("modules/CategoriesOnMainPage/Pictures/Original/" + fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}
