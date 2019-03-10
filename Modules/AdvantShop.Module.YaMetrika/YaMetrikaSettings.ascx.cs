using System;
using System.Drawing;
using System.Web.UI;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.YaMetrika
{
    public partial class Admin_YaMetrikaSettings : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            txtCounterId.Text = ModuleSettingsProvider.GetSettingValue<string>("COUNTER_ID", YaMetrika.ModuleID);
            txtCounter.Text = ModuleSettingsProvider.GetSettingValue<string>("COUNTER", YaMetrika.ModuleID);

            chkOldApiEnabled.Checked = ModuleSettingsProvider.GetSettingValue<bool>("OldApiEnabled", YaMetrika.ModuleID);
            chkEcommerceApi.Checked = ModuleSettingsProvider.GetSettingValue<bool>("EcomerceApiEnabled", YaMetrika.ModuleID);

            chkCollectIp.Checked = ModuleSettingsProvider.GetSettingValue<bool>("CollectIp", YaMetrika.ModuleID);
        }

        protected void Save()
        {
            ModuleSettingsProvider.SetSettingValue("COUNTER_ID", txtCounterId.Text,YaMetrika.ModuleID);
            ModuleSettingsProvider.SetSettingValue("COUNTER", txtCounter.Text, YaMetrika.ModuleID);

            ModuleSettingsProvider.SetSettingValue("OldApiEnabled", chkOldApiEnabled.Checked, YaMetrika.ModuleID);
            ModuleSettingsProvider.SetSettingValue("EcomerceApiEnabled", chkEcommerceApi.Checked, YaMetrika.ModuleID);

            ModuleSettingsProvider.SetSettingValue("CollectIp", chkCollectIp.Checked, YaMetrika.ModuleID);

            lblMessage.Text = (string) GetLocalResourceObject("YaMetrika_ChangesSaved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}