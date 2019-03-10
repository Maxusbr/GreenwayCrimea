using System;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.AdwordsRemarketing
{
    public partial class Admin_AdwordsRemarketingSettings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            txtConversionId.Text = ModuleSettingsProvider.GetSettingValue<string>("СonversionId", Modules.AdwordsRemarketing.ModuleID);
            chkUseDynx.Checked = ModuleSettingsProvider.GetSettingValue<bool>("UseDynx", Modules.AdwordsRemarketing.ModuleID);
        }

        protected void Save()
        {
            ModuleSettingsProvider.SetSettingValue("СonversionId", txtConversionId.Text, Modules.AdwordsRemarketing.ModuleID);
            ModuleSettingsProvider.SetSettingValue("UseDynx", chkUseDynx.Checked, Modules.AdwordsRemarketing.ModuleID);

            lblMessage.Text = "Изменения сохранены";
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}