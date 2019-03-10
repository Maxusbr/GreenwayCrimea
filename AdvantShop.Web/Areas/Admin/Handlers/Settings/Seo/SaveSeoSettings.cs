using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Seo
{
    public class SaveSeoSettings
    {
        private SEOSettingsModel _model;

        public SaveSeoSettings(SEOSettingsModel model)
        {
            _model = model;
        }

        public void Execute()
        {
            SettingsSEO.BrandItemH1 = _model.BrandItemDefaultH1;
            SettingsSEO.BrandItemMetaTitle = _model.BrandItemDefaultTitle;
            SettingsSEO.BrandItemMetaKeywords = _model.BrandItemDefaultMetaKeywords;
            SettingsSEO.BrandItemMetaDescription = _model.BrandItemDefaultMetaDescription;

            SettingsSEO.BrandMetaTitle = _model.BrandsDefaultTitle;
            SettingsSEO.BrandMetaKeywords = _model.BrandsDefaultMetaKeywords;
            SettingsSEO.BrandMetaDescription = _model.BrandsDefaultMetaDescription;

            SettingsSEO.CategoryMetaH1 = _model.CategoriesDefaultH1;
            SettingsSEO.CategoryMetaTitle = _model.CategoriesDefaultTitle;
            SettingsSEO.CategoryMetaKeywords = _model.CategoriesDefaultMetaKeywords;
            SettingsSEO.CategoryMetaDescription = _model.CategoriesDefaultMetaDescription;

            SettingsSEO.TagsMetaH1 = _model.TagsDefaultH1;
            SettingsSEO.TagsMetaTitle = _model.TagsDefaultTitle;
            SettingsSEO.TagsMetaKeywords = _model.TagsDefaultMetaKeywords;
            SettingsSEO.TagsMetaDescription = _model.TagsDefaultMetaDescription;

            SettingsSEO.DefaultMetaTitle = _model.DefaultTitle;
            SettingsSEO.DefaultMetaKeywords = _model.DefaultMetaKeywords;
            SettingsSEO.DefaultMetaDescription = _model.DefaultMetaDescription;

            SettingsSEO.NewsMetaH1 = _model.NewsDefaultH1;
            SettingsSEO.NewsMetaTitle = _model.NewsDefaultTitle;
            SettingsSEO.NewsMetaKeywords = _model.NewsDefaultMetaKeywords;
            SettingsSEO.NewsMetaDescription = _model.NewsDefaultMetaDescription;

            SettingsSEO.ProductMetaH1 = _model.ProductsDefaultH1;
            SettingsSEO.ProductMetaTitle = _model.ProductsDefaultTitle;
            SettingsSEO.ProductMetaKeywords = _model.ProductsDefaultMetaKeywords;
            SettingsSEO.ProductMetaDescription = _model.ProductsDefaultMetaDescription;
            SettingsSEO.ProductAdditionalDescription = _model.ProductsDefaultAdditionalDescription;

            SettingsSEO.StaticPageMetaH1 = _model.StaticPageDefaultH1;
            SettingsSEO.StaticPageMetaTitle = _model.StaticPageDefaultTitle;
            SettingsSEO.StaticPageMetaKeywords = _model.StaticPageDefaultMetaKeywords;
            SettingsSEO.StaticPageMetaDescription = _model.StaticPageDefaultMetaDescription;

            SettingsNews.NewsMetaH1 = _model.CategoryNewsDefaultH1;
            SettingsNews.NewsMetaTitle = _model.CategoryNewsDefaultTitle;
            SettingsNews.NewsMetaKeywords = _model.CategoryNewsDefaultMetaKeywords;
            SettingsNews.NewsMetaDescription = _model.CategoryNewsDefaultMetaDescription;

            SettingsSEO.CustomMetaString = _model.CustomMetaString;

            SettingsSEO.OpenGraphEnabled = _model.OpenGraphEnabled;
            SettingsSEO.OpenGraphFbAdmins = _model.OpenGraphFbAdmins;
            SettingsMain.EnableCyrillicUrl = _model.EnableCyrillicUrl;

            SettingsSEO.GoogleAnalyticsNumber = _model.GoogleAnalyticsNumber;
            SettingsSEO.GoogleAnalyticsEnableDemogrReports = _model.GoogleAnalyticsEnableDemogrReports;
            SettingsSEO.GoogleAnalyticsEnabled = _model.GoogleAnalyticsEnabled;

            SettingsSEO.GoogleAnalyticsApiEnabled = _model.GoogleAnalyticsApiEnabled;
            SettingsSEO.GoogleAnalyticsAccountID = _model.GoogleAnalyticsAccountID;
            SettingsOAuth.GoogleClientId = _model.GoogleClientId;
            SettingsOAuth.GoogleClientSecret = _model.GoogleClientSecret;

            SettingsSEO.UseGTM = _model.UseGTM;
            SettingsSEO.GTMContainerID = _model.GTMContainerId;

            SettingsSEO.Enabled301Redirects = _model.EnableRedirect301;


            using (var streamWriter = new global::System.IO.StreamWriter(HttpContext.Current.Server.MapPath("~/robots.txt")))
            {
                streamWriter.Write(_model.RobotsText);
            }

            CacheManager.RemoveByPattern(CacheNames.GetCategoryCacheObjectPrefix());

        }
    }
}
