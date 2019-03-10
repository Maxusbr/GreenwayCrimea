//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using Resources;

namespace Admin
{
    public partial class CheckoutFields : AdvantShopAdminPage
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ContactFields_Header));

            // customer
            txtFirstName.Text = SettingsCheckout.CustomerFirstNameField;

            chkIsShowLastName.Checked = SettingsCheckout.IsShowLastName;
            chkIsReqLastName.Checked = SettingsCheckout.IsRequiredLastName;

            chkIsShowPatronymic.Checked = SettingsCheckout.IsShowPatronymic;
            chkIsReqPatronymic.Checked = SettingsCheckout.IsRequiredPatronymic;

            txtPhone.Text = SettingsCheckout.CustomerPhoneField;
            chkIsShowPhone.Checked = SettingsCheckout.IsShowPhone;
            chkIsReqPhone.Checked = SettingsCheckout.IsRequiredPhone;

            // checkout
            chkIsShowCountry.Checked = SettingsCheckout.IsShowCountry;
            chkIsReqCountry.Checked = SettingsCheckout.IsRequiredCountry;            

            chkIsShowState.Checked = SettingsCheckout.IsShowState;
            chkIsReqState.Checked = SettingsCheckout.IsRequiredState;

            chkIsShowCity.Checked = SettingsCheckout.IsShowCity;
            chkIsReqCity.Checked = SettingsCheckout.IsRequiredCity;

            chkIsShowZip.Checked = SettingsCheckout.IsShowZip;
            chkIsReqZip.Checked = SettingsCheckout.IsRequiredZip;

            chkIsShowAddress.Checked = SettingsCheckout.IsShowAddress;
            chkIsReqAddress.Checked = SettingsCheckout.IsRequiredAddress;
            
            chkIsShowUserComment.Checked = SettingsCheckout.IsShowUserComment;

            txtCustomShippingField1.Text = SettingsCheckout.CustomShippingField1;
            chkIsShowCustomShippingField1.Checked = SettingsCheckout.IsShowCustomShippingField1;
            chkIsReqCustomShippingField1.Checked = SettingsCheckout.IsReqCustomShippingField1;

            txtCustomShippingField2.Text = SettingsCheckout.CustomShippingField2;
            chkIsShowCustomShippingField2.Checked = SettingsCheckout.IsShowCustomShippingField2;
            chkIsReqCustomShippingField2.Checked = SettingsCheckout.IsReqCustomShippingField2;

            txtCustomShippingField3.Text = SettingsCheckout.CustomShippingField3;
            chkIsShowCustomShippingField3.Checked = SettingsCheckout.IsShowCustomShippingField3;
            chkIsReqCustomShippingField3.Checked = SettingsCheckout.IsReqCustomShippingField3;

            // buy one click
            txtBuyInOneClickName.Text = SettingsCheckout.BuyInOneClickName;
            chkIsShowBuyInOneClickName.Checked = SettingsCheckout.IsShowBuyInOneClickName;
            chkIsRequiredBuyInOneClickName.Checked = SettingsCheckout.IsRequiredBuyInOneClickName;

            txtBuyInOneClickEmail.Text = SettingsCheckout.BuyInOneClickEmail;
            chkIsShowBuyInOneClickEmail.Checked = SettingsCheckout.IsShowBuyInOneClickEmail;
            chkIsRequiredBuyInOneClickEmail.Checked = SettingsCheckout.IsRequiredBuyInOneClickEmail;


            txtBuyInOneClickPhone.Text = SettingsCheckout.BuyInOneClickPhone;
            chkIsShowBuyInOneClickPhone.Checked = SettingsCheckout.IsShowBuyInOneClickPhone;
            chkIsRequiredBuyInOneClickPhone.Checked = SettingsCheckout.IsRequiredBuyInOneClickPhone;

            txtBuyInOneClickComment.Text = SettingsCheckout.BuyInOneClickComment;
            chkIsShowBuyInOneClickComment.Checked = SettingsCheckout.IsShowBuyInOneClickComment;
            chkIsRequiredBuyInOneClickComment.Checked = SettingsCheckout.IsRequiredBuyInOneClickComment;

            chkIsShowFullAddress.Checked = SettingsCheckout.IsShowFullAddress;
        }
        
        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            // customer
            SettingsCheckout.CustomerFirstNameField = txtFirstName.Text;

            SettingsCheckout.IsShowLastName = chkIsShowLastName.Checked;
            SettingsCheckout.IsRequiredLastName = chkIsReqLastName.Checked;

            SettingsCheckout.IsShowPatronymic = chkIsShowPatronymic.Checked;
            SettingsCheckout.IsRequiredPatronymic = chkIsReqPatronymic.Checked;

            SettingsCheckout.CustomerPhoneField = txtPhone.Text;
            SettingsCheckout.IsShowPhone = chkIsShowPhone.Checked;
            SettingsCheckout.IsRequiredPhone = chkIsReqPhone.Checked;

            // checkout
            SettingsCheckout.IsShowCountry = chkIsShowCountry.Checked;
            SettingsCheckout.IsRequiredCountry = chkIsReqCountry.Checked;

            SettingsCheckout.IsShowState = chkIsShowState.Checked;
            SettingsCheckout.IsRequiredState = chkIsReqState.Checked;

            SettingsCheckout.IsShowCity = chkIsShowCity.Checked;
            SettingsCheckout.IsRequiredCity = chkIsReqCity.Checked;

            SettingsCheckout.IsShowZip = chkIsShowZip.Checked;
            SettingsCheckout.IsRequiredZip = chkIsReqZip.Checked;

            SettingsCheckout.IsShowAddress = chkIsShowAddress.Checked;
            SettingsCheckout.IsRequiredAddress = chkIsReqAddress.Checked;
            
            SettingsCheckout.IsShowUserComment = chkIsShowUserComment.Checked;

            SettingsCheckout.CustomShippingField1 = txtCustomShippingField1.Text;
            SettingsCheckout.IsShowCustomShippingField1 = chkIsShowCustomShippingField1.Checked;
            SettingsCheckout.IsReqCustomShippingField1 = chkIsReqCustomShippingField1.Checked;

            SettingsCheckout.CustomShippingField2 = txtCustomShippingField2.Text;
            SettingsCheckout.IsShowCustomShippingField2 = chkIsShowCustomShippingField2.Checked;
            SettingsCheckout.IsReqCustomShippingField2 = chkIsReqCustomShippingField2.Checked;

            SettingsCheckout.CustomShippingField3 = txtCustomShippingField3.Text;
            SettingsCheckout.IsShowCustomShippingField3 = chkIsShowCustomShippingField3.Checked;
            SettingsCheckout.IsReqCustomShippingField3 = chkIsReqCustomShippingField3.Checked;

            // buy one click
            SettingsCheckout.BuyInOneClickName = txtBuyInOneClickName.Text;
            SettingsCheckout.IsShowBuyInOneClickName = chkIsShowBuyInOneClickName.Checked;
            SettingsCheckout.IsRequiredBuyInOneClickName = chkIsRequiredBuyInOneClickName.Checked;

            SettingsCheckout.BuyInOneClickEmail = txtBuyInOneClickEmail.Text;
            SettingsCheckout.IsShowBuyInOneClickEmail = chkIsShowBuyInOneClickEmail.Checked;
            SettingsCheckout.IsRequiredBuyInOneClickEmail = chkIsRequiredBuyInOneClickEmail.Checked;


            SettingsCheckout.BuyInOneClickPhone = txtBuyInOneClickPhone.Text;
            SettingsCheckout.IsShowBuyInOneClickPhone = chkIsShowBuyInOneClickPhone.Checked;
            SettingsCheckout.IsRequiredBuyInOneClickPhone = chkIsRequiredBuyInOneClickPhone.Checked;

            SettingsCheckout.BuyInOneClickComment = txtBuyInOneClickComment.Text;
            SettingsCheckout.IsShowBuyInOneClickComment = chkIsShowBuyInOneClickComment.Checked;
            SettingsCheckout.IsRequiredBuyInOneClickComment = chkIsRequiredBuyInOneClickComment.Checked;


            SettingsCheckout.IsShowFullAddress = chkIsShowFullAddress.Checked;
        }
    }
}