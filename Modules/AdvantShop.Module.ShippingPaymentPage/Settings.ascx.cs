using System;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.ShippingPaymentPage
{
    public partial class Settings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            txtDefaultWeight.Text = ModuleSettingsProvider.GetSettingValue<string>("DefaultWeight", ShippingPaymentPage.ModuleID);
            txtDefaultWidth.Text = ModuleSettingsProvider.GetSettingValue<string>("DefaultWidth", ShippingPaymentPage.ModuleID);
            txtDefaultHeight.Text = ModuleSettingsProvider.GetSettingValue<string>("DefaultHeight", ShippingPaymentPage.ModuleID);
            txtDefaultLength.Text = ModuleSettingsProvider.GetSettingValue<string>("DefaultLength", ShippingPaymentPage.ModuleID);
            txtDefaultPrice.Text = ModuleSettingsProvider.GetSettingValue<string>("DefaultPrice", ShippingPaymentPage.ModuleID);
            txtDefaultShippingPrice.Text = ModuleSettingsProvider.GetSettingValue<string>("DefaultShippingPrice", ShippingPaymentPage.ModuleID);
            txtShippingTextBlock.Text = ModuleSettingsProvider.GetSettingValue<string>("ShippingTextBlock", ShippingPaymentPage.ModuleID);
            txtShippingTextBlockBottom.Text = ModuleSettingsProvider.GetSettingValue<string>("ShippingTextBlockBottom", ShippingPaymentPage.ModuleID);
            txtTitle.Text = ModuleSettingsProvider.GetSettingValue<string>("Title", ShippingPaymentPage.ModuleID);
            txtMetaDescription.Text = ModuleSettingsProvider.GetSettingValue<string>("MetaDescription", ShippingPaymentPage.ModuleID);
            txtMetaKeywords.Text = ModuleSettingsProvider.GetSettingValue<string>("MetaKeywords", ShippingPaymentPage.ModuleID);
            lnkGoToModule.NavigateUrl = "~/shipping-payment";
        }

        protected void Save()
        {
            ModuleSettingsProvider.SetSettingValue("DefaultWeight", txtDefaultWeight.Text.TryParseFloat(), ShippingPaymentPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("DefaultWidth", txtDefaultWidth.Text.TryParseFloat(), ShippingPaymentPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("DefaultHeight", txtDefaultHeight.Text.TryParseFloat(), ShippingPaymentPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("DefaultLength", txtDefaultLength.Text.TryParseFloat(), ShippingPaymentPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("DefaultPrice", txtDefaultPrice.Text.TryParseFloat(), ShippingPaymentPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("DefaultShippingPrice", txtDefaultShippingPrice.Text.TryParseFloat(), ShippingPaymentPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("ShippingTextBlock", txtShippingTextBlock.Text, ShippingPaymentPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("ShippingTextBlockBottom", txtShippingTextBlockBottom.Text, ShippingPaymentPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("Title", txtTitle.Text, ShippingPaymentPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("MetaDescription", txtMetaDescription.Text, ShippingPaymentPage.ModuleID);
            ModuleSettingsProvider.SetSettingValue("MetaKeywords", txtMetaKeywords.Text, ShippingPaymentPage.ModuleID);

            lblMessage.Text = (string)GetLocalResourceObject("ChangesSaved");
            lblMessage.ForeColor = System.Drawing.Color.Blue;
            lblMessage.Visible = true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}