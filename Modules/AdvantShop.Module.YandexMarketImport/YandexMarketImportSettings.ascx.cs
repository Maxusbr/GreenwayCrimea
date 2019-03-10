//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web;
using System.Web.UI.WebControls;
using AdvantShop.Core.Modules;
using System.Collections.Generic;

namespace AdvantShop.Module.YandexMarketImport
{
    public partial class YandexMarketImportSettings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            txtDefaultCurrencyIso.Text = YandexMarketImport.DefaultCurrencyIso.ToString();
            ckbProcess301Redirect.Checked = YandexMarketImport.Process301Redirect;

            ckbAmountNulling.Checked = YandexMarketImport.AmountNulling;
            ckbDeactivateProducts.Checked = YandexMarketImport.DeactivateProducts;

            ckbDeleteOldPrices.Checked = YandexMarketImport.DeleteOldPrices;
            rblAmountNulling.SelectedValue = YandexMarketImport.AmountMappingType;
            txtAmountFieldName.Text = YandexMarketImport.AmountMappingTypeField;

            rblArtNoType.SelectedValue = YandexMarketImport.ArtnoMappingType;
            txtArtnoFieldName.Text = YandexMarketImport.ArtnoMappingTypeField;

            rblArtNoProductType.SelectedValue = YandexMarketImport.ArtnoProductMappingType;
            txtArtnoProductFieldName.Text = YandexMarketImport.ArtnoProductMappingTypeField;

            ckbAutoUpdateActive.Checked = YandexMarketImport.AutoUpdateActive;

            rblUpdateType.SelectedValue = YandexMarketImport.UpdateType;

            ddlTimePeriod.SelectedValue = YandexMarketImport.TimePeriod;

            txtTimePeriodValue.Text = YandexMarketImport.TimePeriodValue;

            txtFileUrlPath.Text = YandexMarketImport.FileUrlPath;

            ckbAllowPreorder.Checked = YandexMarketImport.AllowPreorder;
            txtExtraCharge.Text = YandexMarketImport.ExtraCharge.ToString();

            ddlNameProduct.Items.Clear();
            ddlNameProduct.Items.Add(
                new ListItem()
                {
                    Text = "Model",
                    Value = "Model",
                    Selected = YandexMarketImport.TagForNameProduct != null ? YandexMarketImport.TagForNameProduct == "Model" : true
                });
            ddlNameProduct.Items.Add(
                new ListItem()
                {
                    Text = "Name",
                    Value = "Name",
                    Selected = YandexMarketImport.TagForNameProduct == "Name"
                });

            var fileFath = HttpContext.Current.Server.MapPath("~/modules/YandexMarketImport/temp/statisticLog.txt");
            if (System.IO.File.Exists(fileFath))
            {
                trStatisticLog.Visible = true;
            }

        }

        protected void Save()
        {
			var timePeriodValue = 0;
			YandexMarketImport.Process301Redirect = ckbProcess301Redirect.Checked;
			YandexMarketImport.DeactivateProducts = ckbDeactivateProducts.Checked;
			YandexMarketImport.AmountMappingType = rblAmountNulling.SelectedValue;
			YandexMarketImport.DeleteOldPrices = ckbDeleteOldPrices.Checked;
			YandexMarketImport.DefaultCurrencyIso = Convert.ToSingle(txtDefaultCurrencyIso.Text);
			YandexMarketImport.ArtnoMappingType = rblArtNoType.SelectedValue;
			YandexMarketImport.ArtnoMappingTypeField = txtArtnoFieldName.Text;
			YandexMarketImport.ArtnoProductMappingType = rblArtNoProductType.SelectedValue;
			YandexMarketImport.ArtnoProductMappingTypeField = txtArtnoProductFieldName.Text;
			YandexMarketImport.AmountNulling = ckbAmountNulling.Checked;
			YandexMarketImport.AmountMappingTypeField = txtAmountFieldName.Text;
			YandexMarketImport.UpdateType = rblUpdateType.SelectedValue;
            YandexMarketImport.AllowPreorder = ckbAllowPreorder.Checked;
            YandexMarketImport.ExtraCharge = Convert.ToInt32(txtExtraCharge.Text);
            YandexMarketImport.TagForNameProduct = ddlNameProduct.SelectedValue;
            if (ckbAutoUpdateActive.Checked == false && !int.TryParse(txtTimePeriodValue.Text, out timePeriodValue))
			{
				lblMessage.Text = (string)GetLocalResourceObject("YandexMarketImportSettings_WrongData");
				lblMessage.ForeColor = System.Drawing.Color.Red;
				lblMessage.Visible = true;
				return;
			}
			YandexMarketImport.AutoUpdateActive = ckbAutoUpdateActive.Checked;
			YandexMarketImport.TimePeriod = ddlTimePeriod.SelectedValue;
			YandexMarketImport.TimePeriodValue = txtTimePeriodValue.Text;
			YandexMarketImport.FileUrlPath = txtFileUrlPath.Text;
            
            lblMessage.Text = (string)GetLocalResourceObject("YandexMarketImportSettings_ChangesSaved");
			lblMessage.ForeColor = System.Drawing.Color.Blue;
			lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}