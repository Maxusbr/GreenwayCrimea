using System;
using System.Drawing;
using System.Web.UI;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.Convead
{
    public partial class Admin_ConveadSettings : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            txtAppKey.Text = ModuleSettingsProvider.GetSettingValue<string>("APP_KEY",
                                                                               Convead.ModuleID);
        }

        protected void Save()
        {
            ModuleSettingsProvider.SetSettingValue("APP_KEY", txtAppKey.Text,
                                                   Convead.ModuleID);

            lblMessage.Text = (string) GetLocalResourceObject("Convead_ChangesSaved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}