using System;
using System.Drawing;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Module.RetailRocket.Domain;

namespace AdvantShop.Module.RetailRocket
{
    public partial class Admin_RetailRocketModule : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            txtPartnerId.Text = RRSettings.PartnerId;
            txtLimit.Text = (RRSettings.Limit > 0 ? RRSettings.Limit : 8).ToString("D");

            chkUseApi.Checked = RRSettings.UseApi;
            txtRelatedRecoms.Text = RRSettings.RelatedProductRecoms;
            txtAlterRecoms.Text = RRSettings.AlternativeProductRecoms;
            txtShoppingCartRecoms.Text = RRSettings.ShoppingCartRecoms;

            locRelatedProduct.Text = string.Format((string)GetLocalResourceObject("RR_Label_Related"),
                                                    SettingsCatalog.RelatedProductName);

            locAlternativeProduct.Text = string.Format((string)GetLocalResourceObject("RR_Label_Related"),
                                                    SettingsCatalog.AlternativeProductName);

            chkSendMail.Checked = RRSettings.SendMail;

            ddlMainPageProductsTop.SelectedValue = ((int)RRSettings.MainPageBeforeType).ToString();
            txtMainPageTopTitle.Text = RRSettings.MainPageBeforeTitle;

            ddlMainPageProductsBottom.SelectedValue = ((int)RRSettings.MainPageAfterType).ToString();
            txtMainPageBottomTitle.Text = RRSettings.MainPageAfterTitle;

            ddlCategoryTop.SelectedValue = ((int) RRSettings.CategoryTopType).ToString();
            txtCategoryTopTitle.Text = RRSettings.CategoryTopTitle;

            ddlCategoryBottom.SelectedValue = ((int)RRSettings.CategoryBottomType).ToString();
            txtCategoryBottomTitle.Text = RRSettings.CategoryBottomTitle;
        }

        protected void Save()
        {
            RRSettings.PartnerId = txtPartnerId.Text.Trim();
            RRSettings.Limit = txtLimit.Text.TryParseInt(8);

            RRSettings.UseApi = chkUseApi.Checked;
            RRSettings.RelatedProductRecoms = txtRelatedRecoms.Text;
            RRSettings.AlternativeProductRecoms = txtAlterRecoms.Text;
            RRSettings.ShoppingCartRecoms = txtShoppingCartRecoms.Text;

            RRSettings.SendMail = chkSendMail.Checked;


            RRSettings.MainPageAfterType = (EMainPageProductsType)ddlMainPageProductsBottom.SelectedValue.TryParseInt();
            RRSettings.MainPageAfterTitle = txtMainPageBottomTitle.Text;

            RRSettings.MainPageBeforeType = (EMainPageProductsType)ddlMainPageProductsTop.SelectedValue.TryParseInt();
            RRSettings.MainPageBeforeTitle = txtMainPageTopTitle.Text;

            RRSettings.CategoryTopType = (ECategoryProductsType)ddlCategoryTop.SelectedValue.TryParseInt();
            RRSettings.CategoryTopTitle = txtCategoryTopTitle.Text;

            RRSettings.CategoryBottomType = (ECategoryProductsType)ddlCategoryBottom.SelectedValue.TryParseInt();
            RRSettings.CategoryBottomTitle = txtCategoryBottomTitle.Text;

            
            lblMessage.Text = (string)GetLocalResourceObject("RR_ChangesSaved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}