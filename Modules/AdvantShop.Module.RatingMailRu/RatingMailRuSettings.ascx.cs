using System;
using System.Drawing;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.RatingMailRu
{
    public partial class Admin_RatingMailRuSettings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            txtCounter.Text = ModuleSettingsProvider.GetSettingValue<string>("COUNTER", AdvantShop.Modules.RatingMailRu.ModuleID);
            txtPriceListID.Text = ModuleSettingsProvider.GetSettingValue<string>("PriceListID", AdvantShop.Modules.RatingMailRu.ModuleID);
        }

        protected void Save()
        {
            ModuleSettingsProvider.SetSettingValue("COUNTER", txtCounter.Text, AdvantShop.Modules.RatingMailRu.ModuleID);
            ModuleSettingsProvider.SetSettingValue("PriceListID", txtPriceListID.Text, AdvantShop.Modules.RatingMailRu.ModuleID);

            lblMessage.Text = (string)GetLocalResourceObject("RatingMailRu_ChangesSaved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}