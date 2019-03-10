using AdvantShop.Module.SimaLand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Threading;
using AdvantShop.Diagnostics;
using AdvantShop.Core;
using AdvantShop.Catalog;
using System.Diagnostics;
using System.IO;
using AdvantShop.Module.SimaLand.ViewModel;
using AdvantShop.Core.Modules;
using System.Data;
using AdvantShop.Helpers;
using System.Drawing;
using AdvantShop.FilePath;
using System.Data.SqlClient;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Module.SimaLand.Service
{
    public class SimalandCategoryService
    {

        public static int[] GetAdvCategoryIdsBySlCatIds(int[] catIds)
        {
            if (catIds.Length > 0)
            {
                var query = @"SELECT CategoryId FROM Module." + ModuleTables.SimalandCategory + " WHERE id in (" + catIds.AggregateString(",") + ") and CategoryId is not null";
                //try
                //{
                    var result = ModulesRepository.Query<int>(query, CommandType.Text);
                    return result.Count() > 0 ? result.ToArray() : new int[0];
                //}
                //catch (Exception ex)
                //{

                //    throw new Exception("query");
                //}
            }
            return new int[0];
        }

        public static int GetAdvCategoryBySlCatId(int slCatId)
        {
            var query = @"SELECT CategoryId FROM Module." + ModuleTables.SimalandCategory + " WHERE id=" + slCatId;
            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text);
        }

        /// <summary>
        /// Получаем id всех привязанных категорий Sima-land
        /// </summary>
        /// <returns></returns>
        public static List<int> GetLinkCategory()
        {
            var query = @"SELECT id FROM Module." + ModuleTables.SimalandCategory + " WHERE CategoryId is not null and hidden=0 and is_leaf=1";
            return ModulesRepository.Query<int>(query, CommandType.Text).ToList();
        }

        /// <summary>
        /// Ids всех категорий sima-land
        /// </summary>
        /// <returns></returns>
        public static List<int> SimalandCategoryIds()
        {
            var query = @"SELECT id FROM Module." + ModuleTables.SimalandCategory;
            return ModulesRepository.Query<int>(query, CommandType.Text).ToList();
        }

        public static List<int> SimalandCategoryHiddenIds()
        {
            var query = @"SELECT id FROM Module." + ModuleTables.SimalandCategory + @" WHERE hidden=1";
            return ModulesRepository.Query<int>(query, CommandType.Text).ToList();
        }

        public static void ClearNullCategoryId()
        {
            var query = @"update Module." + ModuleTables.SimalandCategory + @" set CategoryId = null where CategoryId not in (select CategoryID
  FROM Catalog.Category)";
            ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text);
        }

        public static void AddOneSlCategoryToAdv(int categoryId, int level = 1, int parent = 0)
        {
            var category = GetCategoryById(categoryId);
            if (category != null)
            {
                var advCategory = new Category();
                advCategory.Name = category.name;
                advCategory.ParentCategoryId = parent;
                advCategory.SortOrder = 0;
                advCategory.Enabled = true;
                advCategory.TotalProductsCount = 0;
                advCategory.DisplayChildProducts = false;
                advCategory.UrlPath = GetUrl(category);
                advCategory.DisplayBrandsInMenu = false;
                advCategory.DisplaySubCategoriesInMenu = false;
                advCategory.Sorting = ESortOrder.NoSorting;
                advCategory.DisplayStyle = ECategoryDisplayStyle.Tile;
                advCategory.Hidden = false;
                var addParent = CategoryService.AddCategory(advCategory, false);
                var photoUrl = string.Format("https://cdn.sima-land.ru/categories/{0}.jpg", category.id);
                PhotoFromString(photoUrl, addParent, PhotoType.CategorySmall, CategoryImageType.Small, 120, 120);
                if (GetCategoriesByLevelPath(category.id, category.level + 1) != null)
                {
                    AddSlCategoryToAdv(category.id, level + 1, addParent);
                }
                SetCustom(categoryId);
            }
        }

        public static void SetCustom(int categoryId)
        {
            var query = @"UPDATE Module." + ModuleTables.SimalandCategory + " SET custom=1 WHERE id=" + categoryId;
            ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text);
        }

        public static bool AddSlCategoryToAdv(int categoryId, int level = 1, int parent = 0)
        {
            try
            {
                var categories = GetCategoriesByLevelPath(categoryId, level);
                foreach (var category in categories)
                {
                    if (category.CategoryId == null)
                    {
                        var advCategory = new Category();
                        advCategory.Name = category.name;
                        advCategory.ParentCategoryId = parent;
                        advCategory.SortOrder = 0;
                        advCategory.Enabled = true;
                        advCategory.TotalProductsCount = 0;
                        advCategory.DisplayChildProducts = false;
                        advCategory.UrlPath = GetUrl(category);
                        advCategory.DisplayBrandsInMenu = false;
                        advCategory.DisplaySubCategoriesInMenu = false;
                        advCategory.Sorting = ESortOrder.NoSorting;
                        advCategory.DisplayStyle = ECategoryDisplayStyle.Tile;
                        advCategory.Hidden = false;
                        var addParent = CategoryService.AddCategory(advCategory, false);
                        var photoUrl = string.Format("https://cdn.sky.sima-land.ru/category/{0}.jpg", category.id);
                        var photoRes = PhotoFromString(photoUrl, addParent, PhotoType.CategorySmall, CategoryImageType.Small, 120, 120);
                        if(!photoRes){
                            photoUrl = string.Format("https://cdn.sima-land.ru/categories/{0}.jpg", category.id);
                            PhotoFromString(photoUrl, addParent, PhotoType.CategorySmall, CategoryImageType.Small, 120, 120);
                        }

                        SetAdvCategoryForSlCategory(addParent, category.id);
                        if (GetCategoriesByLevelPath(category.id, category.level + 1) != null)
                        {
                            AddSlCategoryToAdv(category.id, level + 1, addParent);
                        }
                    }
                    else
                    {
                        if (GetCategoriesByLevelPath(category.id, category.level + 1) != null)
                        {
                            AddSlCategoryToAdv(category.id, level + 1, (int)category.CategoryId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Diagnostics.Debug.Log.Error(ex);
                return false;
            }

            return true;
        }

        public static void AddingSlCategoriesToAdv()
        {
            PSLModuleSettings.AddingCategories = true;
            try
            {
                AddSlCategoryToAdv(-1);
                PSLModuleSettings.AddingCategories = false;
            }
            catch (Exception ex)
            {
                PSLModuleSettings.AddingCategories = false;
                Diagnostics.Debug.Log.Error(ex);
            }
        }

        private static bool PhotoFromString(string photo, int objId, PhotoType photoType, CategoryImageType imageType, int width, int height)
        {
            // if remote picture we must download it
            if (photo.Contains("https://"))
            {
                var uri = new Uri(photo);
                var photoname = uri.PathAndQuery.Trim('/').Replace("/", "-");
                photoname = Path.GetInvalidFileNameChars().Aggregate(photoname, (current, c) => current.Replace(c.ToString(), ""));

                FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));

                if (!FileHelpers.DownloadRemoteImageFile(photo, FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoname)))
                {
                    //if error in download proccess
                    return false;
                }

                photo = photoname;
            }

            photo = string.IsNullOrEmpty(photo) ? photo : photo.Trim();

            // temp picture folder
            var fullfilename = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photo);

            if (!File.Exists(fullfilename))
                return false;

            PhotoService.DeletePhotos(objId, photoType);

            var tempName = PhotoService.AddPhoto(new Photo(0, objId, photoType) { OriginName = photo });
            if (!string.IsNullOrWhiteSpace(tempName))
            {
                using (Image image = Image.FromFile(fullfilename))
                {
                    FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageCategoryPathAbsolut(imageType, tempName),
                        width, height, image);
                }
            }
            return true;
        }

        private static string GetUrl(SimalandCategoryView category)
        {
            var url = category.full_slug.Split('/')[category.level - 1];
            return IsExistUrlCategory(url) ? url + category.id : url;
        }

        private static bool IsExistUrlCategory(string url)
        {
            var query = @"SELECT COUNT(*) FROM Catalog.Category WHERE [UrlPath]='" + url + "'";

            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text) > 0 ? true : false;
        }

        public static void SetAdvCategoryForSlCategory(int advId, int slId)
        {
            var query = @"UPDATE Module." + ModuleTables.SimalandCategory + " SET CategoryId=" + advId + " WHERE id=" + slId;
            ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text);
        }

        internal static int GetTotalCountParseCategories()
        {
            ClearNullCategoryId();
            var query = @"SELECT COUNT(*) FROM Module." + ModuleTables.SimalandCategory + " WHERE CategoryId is not null and is_leaf=1";
            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text);
        }

        public static void DeleteAdvCategoryFromSlCategory(int slcategoryId)
        {
            var query = @"UPDATE Module." + ModuleTables.SimalandCategory + " SET CategoryId=null WHERE id=" + slcategoryId;
            ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text);
        }

        public static List<SimalandCategory> GetSlCategoryForParse()
        {
            var categoryIds = new List<int>();
            var mainCategories = GetCategoriesByLevel(1);
            foreach (var category in mainCategories)
            {
                var countLinks = GetCountRelatedCategoriesByCatSlId(category.id);
                if (countLinks > 0)
                {
                    categoryIds.Add(category.id);
                }
            }
            var query = @"SELECT * FROM Module." + ModuleTables.SimalandCategory + " WHERE id IN (" + categoryIds.AggregateString(",") + ") ORDER BY name";
            return ModulesRepository.Query<SimalandCategory>(query, CommandType.Text).ToList();
        }

        public static List<SimalandCategoryView> GetCategoriesByLevel(int level)
        {
            var query = @"SELECT * FROM Module." + ModuleTables.SimalandCategory + " WHERE level=" + level + " AND hidden=0";
            return ModulesRepository.Query<SimalandCategoryView>(query, CommandType.Text).ToList();
        }

        public static SimalandCategoryView GetCategoryById(int categoryId)
        {
            var query = @"SELECT * FROM Module." + ModuleTables.SimalandCategory + " WHERE id=" + categoryId;
            return ModulesRepository.Query<SimalandCategoryView>(query, CommandType.Text).FirstOrDefault();
        }

        public static List<SimalandCategoryView> GetCategoriesByLevelPath(int categoryId, int level)
        {
            if (categoryId == -1)
            {
                return GetCategoriesByLevel(1);
            }
            var query = @"SELECT * FROM Module." + ModuleTables.SimalandCategory + " WHERE level=" + level + " and path like (SELECT path FROM Module." + ModuleTables.SimalandCategory + " WHERE id=" + categoryId + ")+'.%' and hidden=0 ";
            return ModulesRepository.Query<SimalandCategoryView>(query, CommandType.Text).ToList();
        }

        public static int GetParentCategory(int categoryId)
        {
            var query = @"SELECT path FROM Module." + ModuleTables.SimalandCategory + " WHERE id = " + categoryId;
            var path = ModulesRepository.ModuleExecuteScalar<string>(query, CommandType.Text);

            string[] ids = path.Split('.');

            if (ids.Length > 1)
            {
                return Convert.ToInt32(ids[ids.Length - 2]);
            }
            return 0;
        }

        public static int GetCountRelatedCategoriesByCatSlId(int slCategoryId)
        {
            return ModulesRepository.ModuleExecuteScalar<int>(@"SELECT COUNT(*)
                                                                  FROM [Module]." + ModuleTables.SimalandCategory + @"
                                                                  where path like '" + slCategoryId + @".%' and is_leaf = 1 
                                                                  and hidden = 0 and CategoryId is not null", CommandType.Text);
        }

        #region trening

        public static List<SimalandCategory> GetCategoryByLevel(int level)
        {
            List<SimalandCategory> result = new List<SimalandCategory>();
            var q = GetQueryLink("is_not_empty=1&level=" + level);
            var response = ApiService.Request(q);
            var rc = DeserializeCategory(response);
            result.AddRange(rc.items);
            if (rc._meta.pageCount > 1)
            {
                var next_page = rc._links.next.href;
                var limit = 2; Stopwatch sw = new Stopwatch();
                sw.Start();
                for (int i = 2; i < rc._meta.pageCount; i++)
                {
                    response = ApiService.Request(next_page);
                    rc = DeserializeCategory(response);
                    if (rc != null)
                    {
                        result.AddRange(rc.items);
                        next_page = i < rc._meta.pageCount ? rc._links.next.href : "";
                    }

                    limit++;
                    if (limit > 25)
                    {
                        sw.Stop();
                        if (sw.Elapsed.TotalMilliseconds < 1000)
                        {
                            double r = 1000 - sw.Elapsed.TotalMilliseconds;
                            Thread.Sleep(Convert.ToInt32(r));
                        }
                        sw.Reset();
                        sw.Start();
                        limit = 0;
                    }
                }
                sw.Stop();
            }
            return result;
        }

        private static string GetQueryLink(string query)
        {
            return ApiService.ApiSimaLand + "category?" + query;
        }

        private static int ParseCategories()
        {
            try
            {
                var slCategory = new List<SimalandCategory>();
                var query = ApiService.ApiSimaLand + "category?is_not_empty=1&per_page=100"; //only active category
                var response = ApiService.Request(query);
                var categories = DeserializeCategory(response);
                if (categories != null)
                {
                    slCategory.AddRange(categories.items);
                }
                var next_page = categories._links.next.href;
                var sw = Stopwatch.StartNew();
                for (int i = 2; i <= categories._meta.pageCount; i++)
                {
                    response = ApiService.Request(next_page);
                    categories = DeserializeCategory(response);
                    if (categories != null)
                    {
                        slCategory.AddRange(categories.items);
                        SimalandImportStatistic.PrePareCategoriesInSimaLand = slCategory.Count; ////////////statistic
                        next_page = i < categories._meta.pageCount ? categories._links.next.href : "";
                    }
                }
                sw.Stop();

                var exists = ModuleService.ExistRows();
                if (exists)
                {

                    foreach (var cat in slCategory)
                    {
                        InsertUpdateCategory(cat);
                        SimalandImportStatistic.TotalCountCategoriesProcess++;
                    }

                    var slIds = slCategory.Select(s => s.id);
                    var advIds = SimalandCategoryIds();
                    var rexcept = advIds.Except(slIds).ToList();
                    SetCategoryAsHidden(rexcept);

                    if (!PSLModuleSettings.AlwaysAvailable)
                    {
                        UpdateAmountOffer();
                    }
                }
                else
                {
                    foreach (var cat in slCategory)
                    {
                        InsertUpdateCategory(cat);
                        SimalandImportStatistic.TotalCountCategoriesProcess++;
                    }
                }
            }
            catch (Exception ex)
            {
                Diagnostics.Debug.Log.Error(ex);
            }

            return 0;
        }

        public static List<int> GetAdvCategoryIdWithHidden()
        {
            var query = "SELECT DISTINCT CategoryId FROM Module." + ModuleTables.SimalandCategory + " where CategoryId is not null and hidden=1";
            return ModulesRepository.Query<int>(query, CommandType.Text).ToList();
        }

        #region Add, hidden SimaLandCategory

        private static void InsertUpdateCategory(SimalandCategory slCategory)
        {
            try
            {
                var pars = new SqlParameter[]
                {
                new SqlParameter("@id", slCategory.id),
                new SqlParameter("@name", slCategory.name),
                new SqlParameter("@level", slCategory.level),
                new SqlParameter("@is_adult", slCategory.is_adult),
                new SqlParameter("@is_leaf", slCategory.is_leaf),
                new SqlParameter("@path", slCategory.path),
                new SqlParameter("@custom", slCategory.custom),
                new SqlParameter("@full_slug", slCategory.full_slug)
                };
                var query = @"declare @exist int = (SELECT COUNT(*) FROM Module.SimalandCategory WHERE id = @id)
                            if (@exist = 0)
                            begin
                            INSERT INTO Module." + ModuleTables.SimalandCategory + @" VALUES (@id,@name,@level,@is_adult,@is_leaf,@path,@custom,@full_slug,null,0)
                                end                            
                                else
                            begin
                            UPDATE Module." + ModuleTables.SimalandCategory + @" SET name=@name, level=@level, is_adult=@is_adult,is_leaf=@is_leaf,path=@path,full_slug=@full_slug, hidden=0 WHERE id = @id
                            UPDATE Catalog.Category SET Hidden=0 where CategoryId=(SELECT CategoryId FROM Module." + ModuleTables.SimalandCategory + @" where id=@id)
                            end";
                ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text, pars);
            }
            catch (Exception ex)
            {
                LogService.ErrLog("InsertUpdateCategory " + slCategory.id + " err.Message: " + ex.Message);
                Diagnostics.Debug.Log.Error(ex);
            }
        }

        private static void SetCategoryAsHidden(List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return;
            }

            try
            {
                var stringIds = ids.AggregateString(",");
                var query = @"UPDATE Module." + ModuleTables.SimalandCategory + @" SET hidden=1 WHERE id in (" + stringIds + @")";
                    //update catalog.category set Hidden = 1 where CategoryId in (select CategoryID from Module." + ModuleTables.SimalandCategory + @" where hidden = 1)";
                ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text);
                var catIds = GetAdvCategoryIdWithHidden();
                if (!catIds.Any())
                {
                    return;
                }
                var idsToHidden = catIds.AggregateString(",");
                query = @"update catalog.category set Hidden = 1 where CategoryId in (" + idsToHidden + @")";

                ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text);

            }
            catch (Exception ex)
            {
                Diagnostics.Debug.Log.Error(ex);
            }
        }

        public static void AdvCategoryWithNoProductAsHidden()
        {
            var query = @"UPDATE [Catalog].[Category] SET Hidden = 1 WHERE Total_Products_Count = 0";
            ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text);
        }

        private static void UpdateAmountOffer()
        {
            try
            {
                var query = @"update Catalog.Offer 
                            SET Amount=0 
                            where 
                            ProductID in (SELECT Product.ProductID FROM Catalog.Product 
                            INNER JOIN Catalog.ProductCategories ON ProductCategories.ProductID=Product.ProductId
                            INNER JOIN catalog.Category ON Category.CategoryID=ProductCategories.CategoryID
                            WHERE Category.Hidden=1 and Product.ModifiedBy='SimaLand' and ProductCategories.Main=1)";

                ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text);
            }
            catch (Exception ex)
            {
                Diagnostics.Debug.Log.Error(ex);
            }
        }

        #endregion

        public static void pc()
        {
            try
            {
                if (SimalandImportStatistic.Process == true && SimalandImportStatistic.Type == SimalandImportStatistic.ProcessType.ParseCategories)
                {
                    return;
                }
                var totaolsw = Stopwatch.StartNew();
                SimalandImportStatistic.Reset(SimalandImportStatistic.ProcessType.ParseCategories);
                SimalandImportStatistic.Process = true;
                LogService.HistoryLog("Парсинг категорий успешно запущен.");
                var err = ParseCategories();
                SimalandImportStatistic.Process = false;
                PSLModuleSettings.LastUpdateCategory = DateTime.Now.ToShortDateString();
                totaolsw.Stop();
                PSLModuleSettings.ElapsedTimeParseCategory = totaolsw.Elapsed.ToString();
                SimalandImportStatistic.SaveCategoryParsingResult("Парсинг категорий успешно завершен. " + Environment.NewLine + "Парсинг категорий занял: " + totaolsw.Elapsed.ToString());
            }
            catch (Exception ex)
            {
                LogService.HistoryLog("Парсинг категорий прерван из-за ошибки: " + ex.Message);
                SimalandImportStatistic.Process = false;
                AdvantShop.Diagnostics.Debug.Log.Error(ex);
            }
        }

        private static ResponseCategory DeserializeCategory(string obj)
        {
            ResponseCategory result = null;
            try
            {
                if (!string.IsNullOrEmpty(obj))
                {
                    result = JsonConvert.DeserializeObject<ResponseCategory>(obj);
                }
            }
            catch (Exception ex)
            {
                AdvantShop.Diagnostics.Debug.Log.Error(ex);
            }
            return result;
        }

        public static SimalandProduct GetProductById(int sid)
        {
            var result = new SimalandProduct();
            using (var wb = new WebClient())
            {
                var query = ApiService.ApiSimaLand + "item?sid=" + sid;
                wb.Headers.Add(HttpRequestHeader.Authorization, "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE0ODcxNjMzNzksImlhdCI6MTQ4NjU1ODU3OSwianRpIjo0MzQ1MDgsIm5iZiI6MTQ4NjU1ODU3OX0.s6vIqsSf2DykzKMb-ZeRwbKqrvW95AoKLxbuScw12nI");
                wb.Headers.Add(HttpRequestHeader.Accept, "application/json");
                wb.Encoding = Encoding.UTF8;
                var response = wb.DownloadString(query);
                result = JsonConvert.DeserializeObject<Products>(response).items.FirstOrDefault();
            }
            //SimalandImport.UpdateInsertProductWorker(result);
            return result;
        }

        public static int GetCountSlCategoryNoHidden()
        {
            var query = @"SELECT COUNT(*) FROM Module." + ModuleTables.SimalandCategory + " WHERE hidden = 0";
            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text);
        }

        public static int GetCountSlCategoryLinks()
        {
            var query = @"SELECT COUNT(*) FROM Module." + ModuleTables.SimalandCategory + " WHERE CategoryId is not null";
            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text);
        }

        #region

        public class Products
        {
            public List<SimalandProduct> items { get; set; }
            public Links _links { get; set; }
            public Meta _meta { get; set; }
        }

        #endregion

        #endregion
    }
}
