using System;
using System.Drawing;
using System.Web.UI;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.PostAffiliatePro
{
    public partial class Admin_PostAffiliateProSettings : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            txtPostAffiliateProLogin.Text = ModuleSettingsProvider.GetSettingValue<string>("PostAffiliateProLogin",
                                                                                           PostAffiliatePro.ModuleID);
            txtPostAffiliateProAccount.Text = ModuleSettingsProvider.GetSettingValue<string>("PostAffiliateProProfile",
                                                                                             PostAffiliatePro.ModuleID);
        }

        protected void Save()
        {
            ModuleSettingsProvider.SetSettingValue("PostAffiliateProLogin", txtPostAffiliateProLogin.Text,
                                                   PostAffiliatePro.ModuleID);
            ModuleSettingsProvider.SetSettingValue("PostAffiliateProProfile", txtPostAffiliateProAccount.Text,
                                                   PostAffiliatePro.ModuleID);

            lblMessage.Text = (string) GetLocalResourceObject("PostAffiliatePro_ChangesSaved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}