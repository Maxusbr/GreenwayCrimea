using System;
using System.Globalization;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Helpers;

namespace Admin.UserControls.Settings
{
    public partial class NewsSettings : System.Web.UI.UserControl
    {
        public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidNews;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            txtNewsPerPage.Text = SettingsNews.NewsPerPage.ToString(CultureInfo.InvariantCulture);
            txtNewsMainPageCount.Text = SettingsNews.NewsMainPageCount.ToString(CultureInfo.InvariantCulture);

            txtNewsHeadTitle.Text = SettingsNews.NewsMetaTitle;
            txtNewsH1.Text = SettingsNews.NewsMetaH1;
            txtNewsMetaDescription.Text = SettingsNews.NewsMetaDescription;
            txtNewsMetaKeywords.Text = SettingsNews.NewsMetaKeywords;
            txtMainPageText.Text = SettingsNews.MainPageText;
            chkViewRssNews.Checked = SettingsNews.RssViewNews;

        }
        public bool SaveData()
        {
            if (!ValidateData())
                return false;

            SettingsNews.NewsPerPage = SQLDataHelper.GetInt(txtNewsPerPage.Text);
            SettingsNews.NewsMainPageCount = SQLDataHelper.GetInt(txtNewsMainPageCount.Text);

            SettingsNews.NewsMetaTitle = txtNewsHeadTitle.Text;
            SettingsNews.NewsMetaH1 =  txtNewsH1.Text;
            SettingsNews.NewsMetaDescription = txtNewsMetaDescription.Text;
            SettingsNews.NewsMetaKeywords = txtNewsMetaKeywords.Text;
            SettingsNews.MainPageText = txtMainPageText.Text;
            SettingsNews.RssViewNews = chkViewRssNews.Checked;
            
            CacheManager.RemoveByPattern(CacheNames.News);

            LoadData();
            return true;
        }

        private bool ValidateData()
        {
            int intVal = 0;
            if (!int.TryParse(txtNewsPerPage.Text, out intVal))
            {
                ErrMessage = "";
                return false;
            }
            if (!int.TryParse(txtNewsMainPageCount.Text, out intVal))
            {
                ErrMessage = "";
                return false;
            }
            return true;
        }
    }
}