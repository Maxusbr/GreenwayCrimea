using System;
using System.Drawing;
using System.Web.UI;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.PopupOnMainPage
{
    public partial class Admin_PopupOnMainPageSetting : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            txtPopupOnMainPageHtml.Text = ModuleSettingsProvider.GetSettingValue<string>("PopupOnMainPageHtml",
                                                                                         PopupOnMainPage.ModuleID);
            txtPopupOnMainPageTitle.Text = ModuleSettingsProvider.GetSettingValue<string>("PopupOnMainPageTitle",
                                                                                          PopupOnMainPage.ModuleID);
            ddlPopupOnMainPageTimeSpan.SelectedValue =
                ModuleSettingsProvider.GetSettingValue<int>("PopupOnMainPageTimeSpan", PopupOnMainPage.ModuleID)
                                      .ToString();

            ckbShowOnMain.Checked = ModuleSettingsProvider.GetSettingValue<bool>("ShowOnMain", PopupOnMainPage.ModuleID); ;
            ckbShowInDetails.Checked = ModuleSettingsProvider.GetSettingValue<bool>("ShowInDetails", PopupOnMainPage.ModuleID);
            ckbShowInMobile.Checked = ModuleSettingsProvider.GetSettingValue<bool>("ShowInMobile", PopupOnMainPage.ModuleID);
            ckbShowInOtherPages.Checked = ModuleSettingsProvider.GetSettingValue<bool>("ShowInOtherPages", PopupOnMainPage.ModuleID);
			ckbBlocksBackground.Checked = ModuleSettingsProvider.GetSettingValue<bool>("BlocksBackground", PopupOnMainPage.ModuleID);

            ddlDelayShowPopup.SelectedValue =
                ModuleSettingsProvider.GetSettingValue<int>("DelayShowPopup", PopupOnMainPage.ModuleID)
                    .ToString();
        }

        protected void Save()
        {
            ModuleSettingsProvider.SetSettingValue("PopupOnMainPageHtml", txtPopupOnMainPageHtml.Text,
                                                   PopupOnMainPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("PopupOnMainPageTitle", txtPopupOnMainPageTitle.Text,
                                                   PopupOnMainPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("PopupOnMainPageTimeSpan", ddlPopupOnMainPageTimeSpan.SelectedValue,
                                                   PopupOnMainPage.ModuleID);

            ModuleSettingsProvider.SetSettingValue("ShowOnMain", ckbShowOnMain.Checked, PopupOnMainPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("ShowInDetails", ckbShowInDetails.Checked, PopupOnMainPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("ShowInMobile", ckbShowInMobile.Checked, PopupOnMainPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("ShowInOtherPages", ckbShowInOtherPages.Checked, PopupOnMainPage.ModuleID);
			ModuleSettingsProvider.SetSettingValue("BlocksBackground", ckbBlocksBackground.Checked, PopupOnMainPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("DelayShowPopup", ddlDelayShowPopup.SelectedValue,
                                                   PopupOnMainPage.ModuleID);

            lblMessage.Text = (String)GetLocalResourceObject("PopupOnMainPage_Message");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}