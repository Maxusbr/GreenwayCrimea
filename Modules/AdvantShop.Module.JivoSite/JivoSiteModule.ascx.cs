using System;
using System.Web.UI;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Module.JivoSite.Domain;
using AdvantShop.Configuration;

namespace AdvantShop.Module.JivoSite
{
    public partial class Admin_JivoSiteModule : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblWebHook.Text = SettingsMain.SiteUrl.Trim('/') + "/jivosite/webhook";
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var vidgetid = ModuleSettingsProvider.GetSettingValue<string>("WidgetId", JivoSite.ModuleId);
                if (vidgetid.IsNotEmpty())
                {
                    txtWidgetID.Text = vidgetid;
                    rbl.SelectedValue = "account";
                    tblSelect.Visible = false;
                }
                else
                {
                    rbl.SelectedValue = "reg";
                    tblSelect.Visible = true;
                }
            }

            if (rbl.SelectedValue == "reg")
            {
                tblReg.Visible = true;
                tblAccount.Visible = false;
            }
            else
            {
                tblReg.Visible = false;
                tblAccount.Visible = true;
            }

        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            var oldWidget = ModuleSettingsProvider.GetSettingValue<string>("WidgetId", JivoSite.ModuleId);
            if (oldWidget != txtWidgetID.Text)
            {
                ModuleSettingsProvider.SetSettingValue("AuthToken", string.Empty, JivoSite.ModuleId);
            }

            ModuleSettingsProvider.SetSettingValue("WidgetId", txtWidgetID.Text, JivoSite.ModuleId);
            tblSelect.Visible = false;
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            if (!Helpers.ValidationHelper.IsValidEmail(txtEmail.Text))
            {
                lblErr.Text = "Неправильный email";
                lblErr.Visible = true;
                return;
            }

            if (txtPassword.Text.IsNullOrEmpty())
            {
                lblErr.Text = "Укажите пароль";
                lblErr.Visible = true;
                return;
            }

            if (txtName.Text.IsNullOrEmpty())
            {
                lblErr.Text = "Укажите имя";
                lblErr.Visible = true;
                return;
            }

            string error;
            if (JivoService.InstallNewWidget(txtEmail.Text, txtName.Text, txtPassword.Text, out error))
            {
                txtWidgetID.Text = ModuleSettingsProvider.GetSettingValue<string>("WidgetId", JivoSite.ModuleId);
                lblMessage.Text = "Saved";
                lblMessage.ForeColor = System.Drawing.Color.Blue;
                lblMessage.Visible = true;
                rbl.SelectedValue = "account";
                tblSelect.Visible = false;
            }
            else
            {
                lblMessage.Text = error;
                lblMessage.Visible = true;
            }
        }


        protected void rbl_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        protected void lbDeleteAcc_Click(object sender, EventArgs e)
        {
            ModuleSettingsProvider.SetSettingValue("AuthToken", string.Empty, JivoSite.ModuleId);
            ModuleSettingsProvider.SetSettingValue("WidgetId", string.Empty, JivoSite.ModuleId);
            rbl.SelectedValue = "reg";
        }
    }
}