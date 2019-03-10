//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using System;
using System.Web;
using System.Collections.Generic;

using AdvantShop.FilePath;
using AdvantShop.Configuration;


namespace AdvantShop.ExportImport
{
    public abstract class BaseExportFeed
    {
        protected BaseExportFeed()
        {

        }

        protected BaseExportFeed(IEnumerable<ExportFeedCategories> categories, IEnumerable<ExportFeedProductModel> products, ExportFeedSettings options, int categoriesCount, int productsCount)
        {

        }

        public abstract string Export(int exportFeedId);

        public abstract string Export(IEnumerable<ExportFeedCategories> categories, IEnumerable<ExportFeedProductModel> products, ExportFeedSettings options, int categoriesCount, int productsCount);


        [Obsolete("This method is obsolete. Use Export method", false)]
        public abstract void Build();

        public abstract void SetDefaultSettings(int exportFeedId);

        public abstract List<string> GetAvailableVariables();

        public abstract List<string> GetAvailableFileExtentions();

        public abstract List<ExportFeedCategories> GetCategories(int exportFeedId);

        public abstract List<ExportFeedProductModel> GetProducts(int exportFeedId);

        public abstract int GetProductsCount(int exportFeedId);

        public abstract int GetCategoriesCount(int exportFeedId);


        public virtual string GetDownloadableExportFeedFileLink(int exportFeedId)
        {
            var settings = ExportFeedSettingsProvider.GetSettings(exportFeedId);            
            return SettingsMain.SiteUrl + "/" + settings.FileFullName + "?rnd=" + (new Random()).Next();
        }

        protected string GetImageProductPath(string photoPath)
        {
            if (string.IsNullOrEmpty(photoPath))
                photoPath = "";

            photoPath = photoPath.Trim();

            if (photoPath.ToLower().Contains("http://"))
                return photoPath;

            return SettingsMain.SiteUrl.TrimEnd('/') + "/" + FoldersHelper.GetImageProductPathRelative(ProductImageType.Big, photoPath, false);
        }

        protected string GetAdditionalUrlTags(ExportFeedProductModel row, string urlTags)
        {
            if (string.IsNullOrEmpty(urlTags))
            {
                return string.Empty;
            }

            urlTags = urlTags.Replace("#STORE_NAME#", HttpUtility.UrlEncode(SettingsMain.ShopName));
            urlTags = urlTags.Replace("#STORE_URL#", HttpUtility.UrlEncode(SettingsMain.SiteUrl));
            urlTags = urlTags.Replace("#PRODUCT_NAME#", HttpUtility.UrlEncode(row.Name));
            urlTags = urlTags.Replace("#PRODUCT_ID#", row.ProductId.ToString());
            urlTags = urlTags.Replace("#PRODUCT_ARTNO#", HttpUtility.UrlEncode(row.ArtNo));
            return urlTags.Trim('?').Trim('/');
        }


    }
}