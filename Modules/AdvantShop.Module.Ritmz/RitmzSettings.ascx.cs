//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Drawing;
using AdvantShop.Modules;

namespace Advantshop.Modules.UserControls
{
    public partial class Admin_RitmzSettings : System.Web.UI.UserControl
    {
        private const string _moduleName = "Ritmz";

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtLogin.Text = ModuleSettingsProvider.GetSettingValue<string>("RitmzLogin", _moduleName);
                txtPassword.Text = ModuleSettingsProvider.GetSettingValue<string>("RitmzPassword", _moduleName);
                txtSiteUrl.Text = ModuleSettingsProvider.GetSettingValue<string>("RitmzSiteUrl", _moduleName);
                txtSiteName.Text = ModuleSettingsProvider.GetSettingValue<string>("RitmzSiteName", _moduleName);
            }
        }

        protected void Save()
        {
            ModuleSettingsProvider.SetSettingValue("RitmzLogin", txtLogin.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("RitmzPassword", txtPassword.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("RitmzSiteUrl", txtSiteUrl.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("RitmzSiteName", txtSiteName.Text, _moduleName);
            
            lblMessage.Text = (String)GetLocalResourceObject("Ritmz_Message");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}