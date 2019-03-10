//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.ExportImport
{
    public class ExportHtmlMap
    {
        private readonly string _filenameAndPath;
        private readonly Encoding _encoding;
        private string _prefUrl;
        private StreamWriter _sw;

        private class ExportHtmlCategory
        {
            public int CategoryId { get; set; }
            public string Name { get; set; }
            public string UrlPath { get; set; }
        }

        public ExportHtmlMap(string filenameAndPath, Encoding encoding)
        {
            _filenameAndPath = filenameAndPath;
            _prefUrl = SettingsMain.SiteUrl + "/";
            _encoding = encoding;
        }

        public bool Create()
        {
            try
            {
                var path = Path.GetDirectoryName(_filenameAndPath);
                if (path == null) return false;

                FileHelpers.CreateDirectory(path);
                FileHelpers.DeleteFile(_filenameAndPath);

                _prefUrl = _prefUrl.Contains("http://") || _prefUrl.Contains("https://") ? _prefUrl : "http://" + _prefUrl;

                using (var fs = new FileStream(_filenameAndPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    //using (_sw = new StreamWriter(_filenameAndPath, false, _encoding))
                    using (_sw = new StreamWriter(fs, _encoding))
                    {
                        _sw.WriteLine("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>" + _prefUrl + " - " + LocalizationService.GetResource("Core.ExportImport.ExportHtmlMap.SiteMapGenerateHeader") + "</title></head><body><div>");

                        _sw.WriteLine("<b><a href='{0}'>{0}</a></b><br/><br/>", SettingsMain.SiteUrl);
                        CreateAux();
                        GetCategories();
                        CreateNews();
                        CreateBrands();
                        if (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveTags))
                        {
                            CreateCategoryTags();
                        }

                        CreateModules();

                        _sw.WriteLine("</div></body></html>");

                        _sw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("ExportHtmlMap", ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// write aux to file directly
        /// </summary>
        private void CreateAux()
        {
            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText = "SELECT [StaticPageID], [PageName],[UrlPath] FROM [CMS].[StaticPage] WHERE [IndexAtSiteMap] = 1 and enabled=1 ORDER BY [SortOrder]";
                db.cnOpen();
                using (var read = db.cmd.ExecuteReader())
                {
                    bool tempHaveItem = read.HasRows;

                    if (tempHaveItem)
                        _sw.WriteLine("<b>" + LocalizationService.GetResource("Core.ExportImport.ExportHtmlMap.StaticPages") + " </b> <ul>");

                    while (read.Read())
                        _sw.WriteLine("<li><a href='{0}'>{1}</a></li>", _prefUrl + UrlService.GetLink(ParamType.StaticPage, SQLDataHelper.GetString(read["UrlPath"]), SQLDataHelper.GetInt(read["StaticPageID"])), read["PageName"]);

                    if (tempHaveItem)
                        _sw.WriteLine("</ul>");
                }
            }
        }
        
        private void GetCategories(int categoryId = 0, int level = 0)
        {
            if (level >= 15) // something wrong, mb stackoverflow ex
                return;

            level++;

            var categories =
                SQLDataAccess.Query<ExportHtmlCategory>(
                    "SELECT [CategoryID], [Name], [UrlPath] FROM [Catalog].[Category] WHERE [Enabled] = 1 and HirecalEnabled=1 and ParentCategory=@categoryId and CategoryID<>0 AND [Products_Count] > 0 ORDER BY [SortOrder]",
                    new {categoryId}).ToList();

            if (categoryId == 0 && categories.Count > 0)
                _sw.WriteLine("<b>" + LocalizationService.GetResource("Core.ExportImport.ExportHtmlMap.Catalog") + "</b>");
                
            if (categories.Count > 0)
                _sw.WriteLine("<ul>");

            foreach (var category in categories)
            {
                _sw.WriteLine("<li>");
                _sw.WriteLine("<a href='{0}'>{1}</a>", _prefUrl + UrlService.GetLink(ParamType.Category, category.UrlPath, category.CategoryId), HttpUtility.HtmlEncode(category.Name));

                GetCategories(category.CategoryId, level);
                GetProducts(category.CategoryId);

                _sw.WriteLine("</li>");
            }

            if (categories.Count > 0)
                _sw.WriteLine("</ul>");
        }

        private void GetProducts(int categoryId)
        {
            _sw.WriteLine("<ul>");
            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText =
                    "SELECT Product.[ProductID] as ProductID, Product.[Name] as Name,ProductCategories.CategoryID as ParentCategory,[UrlPath]  FROM [Catalog].[ProductCategories]" +
                    " INNER JOIN [Catalog].[Product] ON [Product].ProductID = ProductCategories.ProductID WHERE CategoryEnabled =1 and [Enabled] = 1 and ProductCategories.Main = 1 " +
                    "and (select Count(CategoryID) from Catalog.ProductCategories where ProductID=[Product].[ProductID])<> 0 and CategoryID=@categoryId ";
                db.cmd.Parameters.AddWithValue("@categoryId", categoryId);
                db.cnOpen();
                using (var read = db.cmd.ExecuteReader())
                    while (read.Read())
                        _sw.WriteLine("<li><a href='{0}'>{1}</a></li>",
                            _prefUrl + UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(read["UrlPath"]), SQLDataHelper.GetInt(read["ProductID"])), HttpUtility.HtmlEncode(read["Name"]));
            }
            _sw.WriteLine("</ul>");
        }

        private void CreateNews()
        {
            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText = "SELECT [NewsID], [Title], [AddingDate],[UrlPath] FROM [Settings].[News] where AddingDate <= GetDate() AND Enabled = 1 ORDER BY AddingDate DESC";
                db.cnOpen();
                using (var read = db.cmd.ExecuteReader())
                {
                    bool tempHaveItem = read.HasRows;

                    if (tempHaveItem)
                        _sw.WriteLine("<b>" + LocalizationService.GetResource("Core.ExportImport.ExportHtmlMap.News") + " </b> <ul>");

                    while (read.Read())
                        _sw.WriteLine("<li><a href='{0}'>{1}</a></li>", _prefUrl + UrlService.GetLink(ParamType.News, SQLDataHelper.GetString(read["UrlPath"]), SQLDataHelper.GetInt(read["NewsID"])), read["AddingDate"] + " :: " + read["Title"]);

                    if (tempHaveItem)
                        _sw.WriteLine("</ul>");
                }
            }
        }

        private void CreateBrands()
        {
            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText = "SELECT [BrandName], [BrandID], [UrlPath] FROM [Catalog].[Brand] Where enabled=1 ORDER BY BrandName";
                db.cnOpen();
                using (var read = db.cmd.ExecuteReader())
                {
                    bool tempHaveItem = read.HasRows;

                    if (tempHaveItem)
                        _sw.WriteLine("<b>" + LocalizationService.GetResource("Core.ExportImport.ExportHtmlMap.Brands") + " </b> <ul>");

                    while (read.Read())
                        _sw.WriteLine("<li><a href='{0}'>{1}</a></li>",
                                      _prefUrl +
                                      UrlService.GetLink(ParamType.Brand, SQLDataHelper.GetString(read["UrlPath"]),
                                                         SQLDataHelper.GetInt(read["BrandID"])), read["BrandName"]);

                    if (tempHaveItem)
                        _sw.WriteLine("</ul>");
                }
            }
        }

        private void CreateCategoryTags()
        {
            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText = @"SELECT Tag.Name as TagName, Tag.UrlPath as TagUrlPath, Category.CategoryId, Category.Name as CategoryName, Category.UrlPath as CategoryUrlPath
                  FROM [Catalog].[Tag] inner join [Catalog].[TagMap] on [TagMap].[TagId] = [Tag].[Id]
                  inner join Catalog.Category on Category.CategoryId = TagMap.ObjId
                  where TagMap.Type = @TagType AND Tag.Enabled = 1 AND Category.Enabled = 1 AND Category.HirecalEnabled = 1";
                db.cmd.Parameters.Clear();
                db.cmd.Parameters.AddWithValue("@TagType", ETagType.Category.ToString());
                db.cnOpen();
                using (var reader = db.cmd.ExecuteReader())
                {
                    bool tempHaveItem = reader.HasRows;

                    if (tempHaveItem)
                        _sw.WriteLine("<b>" + LocalizationService.GetResource("Core.ExportImport.ExportHtmlMap.Tags") + " </b> <ul>");
                    while (reader.Read())
                        _sw.WriteLine("<li><a href='{0}/tag/{1}'>{2}, {3}</a></li>",
                            _prefUrl +
                            UrlService.GetLink(ParamType.Category, SQLDataHelper.GetString(reader["CategoryUrlPath"]), SQLDataHelper.GetInt(reader["CategoryId"])),
                            SQLDataHelper.GetString(reader["TagUrlPath"]),
                            reader["CategoryName"], reader["TagName"]);

                    if (tempHaveItem)
                        _sw.WriteLine("</ul>");
                }
            }
        }


        private void CreateModules()
        {
            var modules = AttachedModules.GetModules<ISiteMap>();
            foreach (var cls in modules)
            {
                var classInstance = (ISiteMap)Activator.CreateInstance(cls, null);
                _sw.WriteLine("<b>" + classInstance.ModuleName + " </b> <ul>");

                foreach (var item in classInstance.GetData())
                {
                    _sw.WriteLine("<li><a href='{0}'>{1}</a></li>", item.Loc, item.Title);
                }
                _sw.WriteLine("</ul>");
            }
        }
    }
}