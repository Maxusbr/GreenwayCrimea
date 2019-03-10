using System;
using System.Drawing;
using System.Web.UI;

namespace AdvantShop.Module.SubscribePopup
{
    public partial class Admin_SubscribePopupSetting : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            txtSubscribePopupTopHtml.Text = SubscribePopup.SettingPopupTopHtml;
            txtSubscribePopupBottomHtml.Text = SubscribePopup.SettingPopupBottomHtml;
            txtSubscribePopupFinalHtml.Text = SubscribePopup.SettingPopupFinalHtml;
            txtPopupTitle.Text = SubscribePopup.SettingPopupTitle;

            ddlSubscribePopupTimeSpan.SelectedValue = SubscribePopup.SettingTimeSpan.ToString();

            ckbShowOnMain.Checked = SubscribePopup.SettingShowOnMain;
            ckbShowInDetails.Checked = SubscribePopup.SettingShowInDetails;
            ckbShowInOtherPages.Checked = SubscribePopup.SettingShowInOtherPages;
            ckbNotifyAdmin.Checked = SubscribePopup.SettingNotifyAdmin;

            ddlDelayShowPopup.SelectedValue = SubscribePopup.SettingDelayShowPopup.ToString();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SubscribePopup.SettingPopupTopHtml = txtSubscribePopupTopHtml.Text;
            SubscribePopup.SettingPopupBottomHtml = txtSubscribePopupBottomHtml.Text;
            SubscribePopup.SettingPopupFinalHtml = txtSubscribePopupFinalHtml.Text;
            SubscribePopup.SettingPopupTitle = txtPopupTitle.Text;

            SubscribePopup.SettingTimeSpan = Convert.ToInt32(ddlSubscribePopupTimeSpan.SelectedValue);
            SubscribePopup.SettingShowOnMain = ckbShowOnMain.Checked;
            SubscribePopup.SettingShowInDetails = ckbShowInDetails.Checked;
            SubscribePopup.SettingShowInOtherPages = ckbShowInOtherPages.Checked;
            SubscribePopup.SettingNotifyAdmin = ckbNotifyAdmin.Checked;
            SubscribePopup.SettingDelayShowPopup = Convert.ToInt32(ddlDelayShowPopup.SelectedValue);

            lblMessage.Text = (String)GetLocalResourceObject("SubscribePopup_Message");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }
    }
}