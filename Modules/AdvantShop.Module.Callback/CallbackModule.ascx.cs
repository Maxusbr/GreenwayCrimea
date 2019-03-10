using System;
using System.Drawing;
using System.Web.UI;
using AdvantShop.Core.Modules;
using AdvantShop.Saas;


namespace AdvantShop.Module.Callback
{
    public partial class Admin_CallbackModule : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCrm)
            {
                tdCreateLead.Visible = false;
            }

            if (!IsPostBack)
            {

                txtEmail.Text = ModuleSettingsProvider.GetSettingValue<string>("email4notify", Callback.ModuleStringId);
                txtMailSubject.Text = ModuleSettingsProvider.GetSettingValue<string>("emailSubject", Callback.ModuleStringId);
                txtMailFormat.Text = ModuleSettingsProvider.GetSettingValue<string>("emailFormat", Callback.ModuleStringId);
                txtWindowTitle.Text = ModuleSettingsProvider.GetSettingValue<string>("windowTitle", Callback.ModuleStringId);
                txtWindowText.Text = ModuleSettingsProvider.GetSettingValue<string>("windowText", Callback.ModuleStringId);
                ckbCreateLead.Checked = ModuleSettingsProvider.GetSettingValue<bool>("createLead", Callback.ModuleStringId);
                ckbShowCommentField.Checked = ModuleSettingsProvider.GetSettingValue<bool>("showcommentfield", Callback.ModuleStringId);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ModuleSettingsProvider.SetSettingValue("email4notify", txtEmail.Text, Callback.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("emailSubject", txtMailSubject.Text, Callback.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("emailFormat", txtMailFormat.Text, Callback.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("windowTitle", txtWindowTitle.Text, Callback.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("windowText", txtWindowText.Text, Callback.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("createLead", ckbCreateLead.Checked, Callback.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("showcommentfield", ckbShowCommentField.Checked, Callback.ModuleStringId);

            lblMessage.Text = (String)GetLocalResourceObject("Callback_Message");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }
    }
}