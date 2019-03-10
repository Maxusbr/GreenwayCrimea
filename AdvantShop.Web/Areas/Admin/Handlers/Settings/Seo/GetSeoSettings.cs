using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Seo
{
    public class GetSeoSettings
    {
        public SEOSettingsModel Execute()
        {
            var model = new SEOSettingsModel
            {
                BrandItemDefaultH1 = SettingsSEO.BrandItemH1,
                BrandItemDefaultTitle = SettingsSEO.BrandItemMetaTitle,
                BrandItemDefaultMetaKeywords = SettingsSEO.BrandItemMetaKeywords,
                BrandItemDefaultMetaDescription = SettingsSEO.BrandItemMetaDescription,

                BrandsDefaultTitle = SettingsSEO.BrandMetaTitle,
                BrandsDefaultMetaKeywords = SettingsSEO.BrandMetaKeywords,
                BrandsDefaultMetaDescription = SettingsSEO.BrandMetaDescription,

                CategoriesDefaultH1 = SettingsSEO.CategoryMetaH1,
                CategoriesDefaultTitle = SettingsSEO.CategoryMetaTitle,
                CategoriesDefaultMetaKeywords = SettingsSEO.CategoryMetaKeywords,
                CategoriesDefaultMetaDescription = SettingsSEO.CategoryMetaDescription,

                TagsDefaultH1 = SettingsSEO.TagsMetaH1,
                TagsDefaultTitle = SettingsSEO.TagsMetaTitle,
                TagsDefaultMetaKeywords = SettingsSEO.TagsMetaKeywords,
                TagsDefaultMetaDescription = SettingsSEO.TagsMetaDescription,

                DefaultTitle = SettingsSEO.DefaultMetaTitle,
                DefaultMetaKeywords = SettingsSEO.DefaultMetaKeywords,
                DefaultMetaDescription = SettingsSEO.DefaultMetaDescription,

                NewsDefaultH1 = SettingsSEO.NewsMetaH1,
                NewsDefaultTitle = SettingsSEO.NewsMetaTitle,
                NewsDefaultMetaKeywords = SettingsSEO.NewsMetaKeywords,
                NewsDefaultMetaDescription = SettingsSEO.NewsMetaDescription,

                ProductsDefaultH1 = SettingsSEO.ProductMetaH1,
                ProductsDefaultTitle = SettingsSEO.ProductMetaTitle,
                ProductsDefaultMetaKeywords = SettingsSEO.ProductMetaKeywords,
                ProductsDefaultMetaDescription = SettingsSEO.ProductMetaDescription,
                ProductsDefaultAdditionalDescription = SettingsSEO.ProductAdditionalDescription,

                StaticPageDefaultH1 = SettingsSEO.StaticPageMetaH1,
                StaticPageDefaultTitle = SettingsSEO.StaticPageMetaTitle,
                StaticPageDefaultMetaKeywords = SettingsSEO.StaticPageMetaKeywords,
                StaticPageDefaultMetaDescription = SettingsSEO.StaticPageMetaDescription,

                CategoryNewsDefaultH1 = SettingsNews.NewsMetaH1,
                CategoryNewsDefaultTitle = SettingsNews.NewsMetaTitle,
                CategoryNewsDefaultMetaKeywords = SettingsNews.NewsMetaKeywords,
                CategoryNewsDefaultMetaDescription = SettingsNews.NewsMetaDescription,

                CustomMetaString = SettingsSEO.CustomMetaString,

                OpenGraphEnabled = SettingsSEO.OpenGraphEnabled,
                OpenGraphFbAdmins = SettingsSEO.OpenGraphFbAdmins,
                EnableCyrillicUrl = SettingsMain.EnableCyrillicUrl,

                GoogleAnalyticsNumber = SettingsSEO.GoogleAnalyticsNumber,
                GoogleAnalyticsEnableDemogrReports = SettingsSEO.GoogleAnalyticsEnableDemogrReports,
                GoogleAnalyticsEnabled = SettingsSEO.GoogleAnalyticsEnabled,

                GoogleAnalyticsApiEnabled = SettingsSEO.GoogleAnalyticsApiEnabled,
                GoogleAnalyticsAccountID = SettingsSEO.GoogleAnalyticsAccountID,
                GoogleClientId = SettingsOAuth.GoogleClientId,
                GoogleClientSecret = SettingsOAuth.GoogleClientSecret,


                UseGTM = SettingsSEO.UseGTM,
                GTMContainerId = SettingsSEO.GTMContainerID,

                EnableRedirect301 = SettingsSEO.Enabled301Redirects
            };


            using (var streamReader = new global::System.IO.StreamReader(HttpContext.Current.Server.MapPath("~/robots.txt")))
            {
                model.RobotsText = streamReader.ReadToEnd();
            }

            return model;
        }
    }
}
