using System;
using System.IO;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Configuration.Settings;

namespace AdvantShop.Module.MobileVersion
{
    public partial class MobileVersionSettings : System.Web.UI.UserControl
    {
        private string _filename;

        protected void Page_Load(object sender, EventArgs e)
        {
            _filename = Server.MapPath("~/areas/mobile/robots.txt");

            if (!File.Exists(_filename))
            {
                File.Create(_filename);
            }

            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            txtMainPageProductCountMobile.Text = SettingsMobile.MainPageProductsCount.ToString();
            txtCatalogProductCountMobile.Text = SettingsMobile.ProductsPerPage.ToString();
            chkShowCity.Checked = SettingsMobile.DisplayCity;
            chkShowSlider.Checked = SettingsMobile.DisplaySlider;
            chkDisplayHeaderTitle.Checked = SettingsMobile.DisplayHeaderTitle;
            txtHeaderCustomTitle.Text = SettingsMobile.HeaderCustomTitle;
            chkIsFullCheckout.Checked = SettingsMobile.IsFullCheckout;

            using (var sr = new StreamReader(_filename))
            {
                txtRobots.Text = sr.ReadToEnd();
            }

        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SettingsMobile.MainPageProductsCount = txtMainPageProductCountMobile.Text.TryParseInt();
            SettingsMobile.ProductsPerPage = txtCatalogProductCountMobile.Text.TryParseInt();
            SettingsMobile.DisplayCity = chkShowCity.Checked;
            SettingsMobile.DisplaySlider = chkShowSlider.Checked;
            SettingsMobile.DisplayHeaderTitle = chkDisplayHeaderTitle.Checked;
            SettingsMobile.HeaderCustomTitle = txtHeaderCustomTitle.Text;
            SettingsMobile.IsFullCheckout = chkIsFullCheckout.Checked;

            using (var wr = new StreamWriter(_filename))
            {
                wr.Write(txtRobots.Text);
            }

            lblMessage.Text = (string)GetLocalResourceObject("ChangesSaved");
            lblMessage.ForeColor = System.Drawing.Color.Blue;
            lblMessage.Visible = true;
        }
    }
}