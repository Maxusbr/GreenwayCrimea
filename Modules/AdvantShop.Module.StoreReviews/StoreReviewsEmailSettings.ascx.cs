using System;
using System.Drawing;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.StoreReviews
{
    public partial class Admin_StoreReviewsEmailSettings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ckbEnableSendMails.Checked = ModuleSettingsProvider.GetSettingValue<bool>("EnableSendMails", StoreReviews.ModuleID);

            txtFormat.Text = ModuleSettingsProvider.GetSettingValue<string>("Format", StoreReviews.ModuleID);
            txtSubject.Text = ModuleSettingsProvider.GetSettingValue<string>("Subject", StoreReviews.ModuleID);
            txtEmail.Text = ModuleSettingsProvider.GetSettingValue<string>("Email", StoreReviews.ModuleID);
        }

        protected void Save()
        {
            ModuleSettingsProvider.SetSettingValue("EnableSendMails", ckbEnableSendMails.Checked, StoreReviews.ModuleID);

            ModuleSettingsProvider.SetSettingValue("Format", txtFormat.Text, StoreReviews.ModuleID);
            ModuleSettingsProvider.SetSettingValue("Subject", txtSubject.Text, StoreReviews.ModuleID);
            ModuleSettingsProvider.SetSettingValue("Email", txtEmail.Text, StoreReviews.ModuleID);

            lblMessage.Text = (string)GetLocalResourceObject("StoreReviewsMails_ChangesSaved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}