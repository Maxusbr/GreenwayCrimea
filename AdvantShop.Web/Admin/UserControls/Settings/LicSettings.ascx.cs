using System;
using AdvantShop.Configuration;
using AdvantShop.Saas;

namespace Admin.UserControls.Settings
{
    public partial class LicSettings : System.Web.UI.UserControl
    {
        public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidLic;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtLicKey.Enabled = btnCheckLic.Enabled = !SaasDataService.IsSaasEnabled;
            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            txtLicKey.Text = SettingsLic.LicKey;
            lblState.Text = SettingsLic.ActiveLic ? Resources.Resource.Admin_UserControls_Settings_LicSettings_Active : Resources.Resource.Admin_UserControls_Settings_LicSettings_Deactive;
        }

        public bool SaveData()
        {
            SettingsLic.Activate(txtLicKey.Text);
            LoadData();
            return true;
        }

        protected void btnCheckLic_Click(object sender, EventArgs e)
        {
            SettingsLic.Activate(txtLicKey.Text);
            LoadData();
        }
    }
}