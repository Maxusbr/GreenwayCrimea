using System;
using System.Drawing;
using System.Web.UI;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.ShoppingCartPopup
{
    public partial class Admin_ShoppingCartPopupModule : UserControl
    {
        private const string _moduleName = "ShoppingCartPopup";

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var showMode = ModuleSettingsProvider.GetSettingValue<string>("showmode", _moduleName);
            if (ddlShowMode.Items.FindByValue(showMode) != null)
            {
                ddlShowMode.SelectedValue = showMode;
            }
            else
            {
                ddlShowMode.SelectedIndex = 0;
            }

            chkGoToCheckout.Checked = ModuleSettingsProvider.GetSettingValue<bool>("goToCheckout", _moduleName);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ModuleSettingsProvider.SetSettingValue("showmode", ddlShowMode.SelectedValue, _moduleName);
            ModuleSettingsProvider.SetSettingValue("goToCheckout", chkGoToCheckout.Checked.ToString().ToLower(), _moduleName);

            lblMessage.Text = (String) GetLocalResourceObject("ShoppingCartPopup_Message");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }
    }
}