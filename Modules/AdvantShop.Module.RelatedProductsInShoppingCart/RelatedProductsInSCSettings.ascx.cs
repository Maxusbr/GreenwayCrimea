//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Drawing;
using System.Web.UI;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.RelatedProductsInShoppingCart
{
    public partial class Admin_RelatedProductsInSCSettings : UserControl
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

        protected void Save()
        {
            if (!Validate())
            {
                return;
            }

            //ModuleSettingsProvider.SetSettingValue("BuyButtonText", txtBuyButtonText.Text, RelatedProductsInShoppingCart.ModuleID);
            //ModuleSettingsProvider.SetSettingValue("MoreButtonText", txtMoreButtonText.Text, RelatedProductsInShoppingCart.ModuleID);
            //ModuleSettingsProvider.SetSettingValue("PreOrderButtonText", txtPreOrderButtonText.Text, RelatedProductsInShoppingCart.ModuleID);
            //ModuleSettingsProvider.SetSettingValue("ImageMaxHeight", txtImageMaxHeight.Text, RelatedProductsInShoppingCart.ModuleID);
            //ModuleSettingsProvider.SetSettingValue("ImageMaxWidth", txtImageMaxWidth.Text, RelatedProductsInShoppingCart.ModuleID);
            //ModuleSettingsProvider.SetSettingValue("DisplayBuyButton", ckbDisplayBuyButton.Checked, RelatedProductsInShoppingCart.ModuleID);
            //ModuleSettingsProvider.SetSettingValue("DisplayMoreButton", ckbDisplayMoreButton.Checked, RelatedProductsInShoppingCart.ModuleID);
            //ModuleSettingsProvider.SetSettingValue("DisplayPreOrderButton", ckbDisplayPreOrderButton.Checked, RelatedProductsInShoppingCart.ModuleID);
            ModuleSettingsProvider.SetSettingValue("RelatedType", ddlRelatedType.SelectedValue,
                                                   RelatedProductsInShoppingCart.ModuleID);
            ModuleSettingsProvider.SetSettingValue("TopHtml", txtTopHtml.Text, RelatedProductsInShoppingCart.ModuleID);
            ModuleSettingsProvider.SetSettingValue("BottomHtml", txtBottomHtml.Text,
                                                   RelatedProductsInShoppingCart.ModuleID);

            lblMessage.Text = (String) GetLocalResourceObject("RelatedProductsInSCSettings_SaveChanged");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void LoadSettings()
        {
            //txtBuyButtonText.Text = ModuleSettingsProvider.GetSettingValue<string>("BuyButtonText", RelatedProductsInShoppingCart.ModuleID);
            //txtMoreButtonText.Text = ModuleSettingsProvider.GetSettingValue<string>("MoreButtonText", RelatedProductsInShoppingCart.ModuleID);
            //txtPreOrderButtonText.Text = ModuleSettingsProvider.GetSettingValue<string>("PreOrderButtonText", RelatedProductsInShoppingCart.ModuleID);
            //txtImageMaxHeight.Text = ModuleSettingsProvider.GetSettingValue<string>("ImageMaxHeight", RelatedProductsInShoppingCart.ModuleID);
            //txtImageMaxWidth.Text = ModuleSettingsProvider.GetSettingValue<string>("ImageMaxWidth", RelatedProductsInShoppingCart.ModuleID);
            //ckbDisplayBuyButton.Checked = ModuleSettingsProvider.GetSettingValue<bool>("DisplayBuyButton", RelatedProductsInShoppingCart.ModuleID);
            //ckbDisplayMoreButton.Checked = ModuleSettingsProvider.GetSettingValue<bool>("DisplayMoreButton", RelatedProductsInShoppingCart.ModuleID);
            //ckbDisplayPreOrderButton.Checked = ModuleSettingsProvider.GetSettingValue<bool>("DisplayPreOrderButton", RelatedProductsInShoppingCart.ModuleID);
            ddlRelatedType.SelectedValue = ModuleSettingsProvider.GetSettingValue<string>("RelatedType",
                                                                                          RelatedProductsInShoppingCart
                                                                                              .ModuleID);
            txtTopHtml.Text = ModuleSettingsProvider.GetSettingValue<string>("TopHtml",
                                                                             RelatedProductsInShoppingCart.ModuleID);
            txtBottomHtml.Text = ModuleSettingsProvider.GetSettingValue<string>("BottomHtml",
                                                                                RelatedProductsInShoppingCart.ModuleID);
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