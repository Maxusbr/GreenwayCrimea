using System;
using System.Drawing;
using System.Web.UI;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Helpers;
using AdvantShop.Trial;

namespace AdvantShop.Module.BonusSystemModule
{
    public partial class Admin_BonusSystemModule : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            txtApiKey.Text = BonusSystem.ApiKey;
            ddlBonusType.SelectedValue = ((int) BonusSystem.BonusType).ToString();
            lblBonusFirstPercent.Text = BonusSystem.BonusFirstPercent.ToString();
            txtMaxOrderPercent.Text = BonusSystem.MaxOrderPercent.ToString("F2");
            ckbUseOrderId.Checked = BonusSystem.UseOrderId;
            txtBonusesForNewCard.Text = BonusSystem.BonusesForNewCard.ToString("F2");

            txtBonusTextBlock.Text = ModuleSettingsProvider.GetSettingValue<string>("BonusTextBlock", BonusSystemModule.ModuleID);
            txtRightBonusTextBlock.Text = ModuleSettingsProvider.GetSettingValue<string>("BonusRightTextBlock", BonusSystemModule.ModuleID);
            chkShowGrades.Checked = ModuleSettingsProvider.GetSettingValue<bool>("BonusShowGrades", BonusSystemModule.ModuleID);

            hlGetBonusCard.NavigateUrl = UrlService.GetUrl("getbonuscard");
            
            if (TrialService.IsTrialEnabled)
            {
                txtApiKey.Visible = false;
                divTrial.Visible = true;
            }
            else
            {
                txtApiKey.Visible = true;
                divTrial.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            BonusSystem.ApiKey = txtNewKey.Text.IsNotEmpty() ? txtNewKey.Text : txtApiKey.Text;
            BonusSystem.BonusType = (EBonusType) SQLDataHelper.GetInt(ddlBonusType.SelectedValue);
            BonusSystem.MaxOrderPercent = txtMaxOrderPercent.Text.TryParseFloat(100);
            BonusSystem.UseOrderId = ckbUseOrderId.Checked;
            BonusSystem.BonusesForNewCard = txtBonusesForNewCard.Text.TryParseFloat();

            ModuleSettingsProvider.SetSettingValue("BonusTextBlock", txtBonusTextBlock.Text, BonusSystemModule.ModuleID);
            ModuleSettingsProvider.SetSettingValue("BonusRightTextBlock", txtRightBonusTextBlock.Text, BonusSystemModule.ModuleID);
            ModuleSettingsProvider.SetSettingValue("BonusShowGrades", chkShowGrades.Checked, BonusSystemModule.ModuleID);

            if (BonusSystemService.IsActive())
            {
                lblMessage.Text = (String) GetLocalResourceObject("BonusSystem_Message");
                lblMessage.ForeColor = Color.Blue;
                lblMessage.Visible = true;
            }
            else
            {
                lblMessage.Text = (String) GetLocalResourceObject("BonusSystem_Save_Error");
                lblMessage.ForeColor = Color.Red;
                lblMessage.Visible = true;
            }
        }
    }
}