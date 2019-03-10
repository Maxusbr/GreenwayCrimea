using System;
using System.IO;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Configuration.Settings;

namespace Admin.UserControls.Settings
{
    public partial class MobileSettings : System.Web.UI.UserControl
    {
        public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidProfit;
        private string _filename;

        protected void Page_Load(object sender, EventArgs e)
        {
            _filename = Server.MapPath("~/areas/mobile/robots.txt");

            if (!File.Exists(_filename))
            {
                File.Create(_filename);
            }
            
            if (!IsPostBack)
                LoadData();           
        }
        
        private void LoadData()
        {
            cbEnabled.Checked = SettingsMobile.IsMobileTemplateActive;
            txtMainPageProductCountMobile.Text = SettingsMobile.MainPageProductsCount.ToString();
            chkShowCity.Checked = SettingsMobile.DisplayCity;
            chkShowSlider.Checked = SettingsMobile.DisplaySlider;
            chkDisplayHeaderTitle.Checked = SettingsMobile.DisplayHeaderTitle;
            txtHeaderCustomTitle.Text = SettingsMobile.HeaderCustomTitle;
            chkIsFullCheckout.Checked = SettingsMobile.IsFullCheckout;
            chkRedirectToSubdomain.Checked = SettingsMobile.RedirectToSubdomain;

            using (var sr = new StreamReader(_filename))
            {
                txtRobots.Text = sr.ReadToEnd();
            }
        }

        public bool SaveData()
        {
            SettingsMobile.IsMobileTemplateActive = cbEnabled.Checked;
            SettingsMobile.MainPageProductsCount = txtMainPageProductCountMobile.Text.TryParseInt();
            SettingsMobile.DisplayCity = chkShowCity.Checked;
            SettingsMobile.DisplaySlider = chkShowSlider.Checked;
            SettingsMobile.DisplayHeaderTitle = chkDisplayHeaderTitle.Checked;
            SettingsMobile.HeaderCustomTitle = txtHeaderCustomTitle.Text;
            SettingsMobile.IsFullCheckout = chkIsFullCheckout.Checked;
            SettingsMobile.RedirectToSubdomain = chkRedirectToSubdomain.Checked;

            using (var wr = new StreamWriter(_filename))
            {
                wr.Write(txtRobots.Text);
            }

            return true;
        }
    }
}