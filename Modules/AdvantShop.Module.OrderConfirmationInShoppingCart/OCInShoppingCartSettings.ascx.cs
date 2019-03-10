using System;
using System.Drawing;
using System.Web.UI;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.OrderConfirmationInShoppingCart
{
    public partial class OCInShoppingCartSettings : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            LoadSettings();
        }

        private void Save()
        {
            if (!Validate())
                return;

            ModuleSettingsProvider.SetSettingValue("FirstText", ckFirstText.Text, OrderConfirmationInShoppingCart.ModuleID);
            ModuleSettingsProvider.SetSettingValue("FinalText", ckFinalText.Text, OrderConfirmationInShoppingCart.ModuleID);

            lblMessage.Text = (String) GetLocalResourceObject("SaveChanged");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        private void LoadSettings()
        {
            var firstText = ModuleSettingsProvider.GetSettingValue<string>("FirstText", OrderConfirmationInShoppingCart.ModuleID);
            if (firstText == null)
            {
                firstText = "Оформление заказа";
                ModuleSettingsProvider.SetSettingValue("FirstText", firstText, OrderConfirmationInShoppingCart.ModuleID);
            }
            ckFirstText.Text = firstText;
            ckFinalText.Text = ModuleSettingsProvider.GetSettingValue<string>("FinalText", OrderConfirmationInShoppingCart.ModuleID);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private bool Validate()
        {
            return true;
        }
    }
}