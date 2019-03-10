using System;
using AdvantShop.Configuration;

namespace Admin.UserControls.Settings
{
    public partial class SEOSettings : System.Web.UI.UserControl
    {
        public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidSEO;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            txtProductsHeadTitle.Text = SettingsSEO.ProductMetaTitle;
            txtProductsMetaKeywords.Text = SettingsSEO.ProductMetaKeywords;
            txtProductsMetaDescription.Text = SettingsSEO.ProductMetaDescription;
            txtProductsH1.Text = SettingsSEO.ProductMetaH1;
            txtProductsAdditionalDescription.Text = SettingsSEO.ProductAdditionalDescription;

            txtCategoriesHeadTitle.Text = SettingsSEO.CategoryMetaTitle;
            txtCategoriesMetaKeywords.Text = SettingsSEO.CategoryMetaKeywords;
            txtCategoriesMetaDescription.Text = SettingsSEO.CategoryMetaDescription;
            txtCategoriesMetaH1.Text = SettingsSEO.CategoryMetaH1;

            txtNewsHeadTitle.Text = SettingsSEO.NewsMetaTitle;
            txtNewsMetaKeywords.Text = SettingsSEO.NewsMetaKeywords;
            txtNewsMetaDescription.Text = SettingsSEO.NewsMetaDescription;
            txtNewsH1.Text = SettingsSEO.NewsMetaH1;

            txtStaticPageHeadTitle.Text = SettingsSEO.StaticPageMetaTitle;
            txtStaticPageMetaKeywords.Text = SettingsSEO.StaticPageMetaKeywords;
            txtStaticPageMetaDescription.Text = SettingsSEO.StaticPageMetaDescription;
            txtStaticPageH1.Text = SettingsSEO.StaticPageMetaH1;

            txtTagsTitle.Text = SettingsSEO.TagsMetaTitle;
            txtTagsMKeywords.Text = SettingsSEO.TagsMetaKeywords;
            txtTagsMDescription.Text = SettingsSEO.TagsMetaDescription;
            txtTagsH1.Text = SettingsSEO.TagsMetaH1;

            txtBrandItemMetaTitle.Text = SettingsSEO.BrandItemMetaTitle;
            txtBrandItemMetaKeywords.Text = SettingsSEO.BrandItemMetaKeywords;
            txtBrandItemMetaDescription.Text = SettingsSEO.BrandItemMetaDescription;
            txtBrandItemH1.Text = SettingsSEO.BrandItemH1;


            txtTitle.Text = SettingsSEO.DefaultMetaTitle;
            txtMetaKeys.Text = SettingsSEO.DefaultMetaKeywords;
            txtMetaDescription.Text = SettingsSEO.DefaultMetaDescription;

            txtBrandTitle.Text = SettingsSEO.BrandMetaTitle;
            txtBrandMetaKeywords.Text = SettingsSEO.BrandMetaKeywords;
            txtBrandMetaDescription.Text = SettingsSEO.BrandMetaDescription;

            txtCustomMetaString.Text = SettingsSEO.CustomMetaString;

            chkOpenGraphEnabled.Checked = SettingsSEO.OpenGraphEnabled;
            txtOpenGraphFbAdmins.Text = SettingsSEO.OpenGraphFbAdmins;

            chkEnableCyrillicUrl.Checked = SettingsMain.EnableCyrillicUrl;
        }

        public bool SaveData()
        {
            SettingsSEO.ProductMetaTitle = txtProductsHeadTitle.Text;
            SettingsSEO.ProductMetaKeywords = txtProductsMetaKeywords.Text;
            SettingsSEO.ProductMetaDescription = txtProductsMetaDescription.Text;
            SettingsSEO.ProductMetaH1 = txtProductsH1.Text;
            SettingsSEO.ProductAdditionalDescription = txtProductsAdditionalDescription.Text;

            SettingsSEO.CategoryMetaTitle = txtCategoriesHeadTitle.Text;
            SettingsSEO.CategoryMetaKeywords = txtCategoriesMetaKeywords.Text;
            SettingsSEO.CategoryMetaDescription = txtCategoriesMetaDescription.Text;
            SettingsSEO.CategoryMetaH1 = txtCategoriesMetaH1.Text;

            SettingsSEO.NewsMetaTitle = txtNewsHeadTitle.Text;
            SettingsSEO.NewsMetaKeywords = txtNewsMetaKeywords.Text;
            SettingsSEO.NewsMetaDescription = txtNewsMetaDescription.Text;
            SettingsSEO.NewsMetaH1 = txtNewsH1.Text;

            SettingsSEO.StaticPageMetaTitle = txtStaticPageHeadTitle.Text;
            SettingsSEO.StaticPageMetaKeywords = txtStaticPageMetaKeywords.Text;
            SettingsSEO.StaticPageMetaDescription = txtStaticPageMetaDescription.Text;
            SettingsSEO.StaticPageMetaH1 = txtStaticPageH1.Text;

            SettingsSEO.TagsMetaTitle = txtTagsTitle.Text;
            SettingsSEO.TagsMetaKeywords = txtTagsMKeywords.Text;
            SettingsSEO.TagsMetaDescription = txtTagsMDescription.Text;
            SettingsSEO.TagsMetaH1 = txtTagsH1.Text;

            SettingsSEO.BrandItemMetaTitle = txtBrandItemMetaTitle.Text;
            SettingsSEO.BrandItemMetaKeywords = txtBrandItemMetaKeywords.Text;
            SettingsSEO.BrandItemMetaDescription = txtBrandItemMetaDescription.Text;
            SettingsSEO.BrandItemH1 = txtBrandItemH1.Text;
            
            SettingsSEO.DefaultMetaTitle = txtTitle.Text;
            SettingsSEO.DefaultMetaKeywords = txtMetaKeys.Text;
            SettingsSEO.DefaultMetaDescription = txtMetaDescription.Text;
        
            SettingsSEO.BrandMetaTitle = txtBrandTitle.Text;
            SettingsSEO.BrandMetaKeywords = txtBrandMetaKeywords.Text;
            SettingsSEO.BrandMetaDescription = txtBrandMetaDescription.Text;
            
            SettingsSEO.CustomMetaString = txtCustomMetaString.Text;

            SettingsSEO.OpenGraphEnabled = chkOpenGraphEnabled.Checked;
            SettingsSEO.OpenGraphFbAdmins = txtOpenGraphFbAdmins.Text;

            SettingsMain.EnableCyrillicUrl = chkEnableCyrillicUrl.Checked;

            LoadData();

            return true;
        }
    }
}