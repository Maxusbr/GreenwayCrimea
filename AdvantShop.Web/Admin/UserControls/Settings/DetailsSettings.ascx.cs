using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using Resources;

namespace Admin.UserControls.Settings
{
    public partial class DetailsSettings : System.Web.UI.UserControl
    {
        public string ErrMessage = Resource.Admin_CommonSettings_InvalidCatalog;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            chkDisplayWeight.Checked = SettingsCatalog.DisplayWeight;
            chkDisplayDimensions.Checked = SettingsCatalog.DisplayDimensions;
            cbShowStockAvailability.Checked = SettingsCatalog.ShowStockAvailability;

            chkCompressBigImage.Checked = SettingsCatalog.CompressBigImage;

            ckbModerateReviews.Checked = SettingsCatalog.ModerateReviews;
            chkAllowReviews.Checked = SettingsCatalog.AllowReviews;
            chkDisplayReviewsImage.Checked = SettingsCatalog.DisplayReviewsImage;
            chkAllowReviewsImageUploading.Checked = SettingsCatalog.AllowReviewsImageUploading;
            txtReviewImageWidth.Text = SettingsPictureSize.ReviewImageWidth.ToString();
            txtReviewImageHeight.Text = SettingsPictureSize.ReviewImageHeight.ToString();

            chkEnableZoom.Checked = SettingsDesign.EnableZoom;

            ddlShowShippingsMethodsInDetails.SelectedValue =
                ((int) SettingsDesign.ShowShippingsMethodsInDetails).ToString();
            txtShippingsMethodsInDetailsCount.Text = SettingsDesign.ShippingsMethodsInDetailsCount.ToString();

            txtBlockOne.Text = SettingsCatalog.RelatedProductName;
            txtBlockTwo.Text = SettingsCatalog.AlternativeProductName;
            ddlRelatedProductSourceType.SelectedValue = ((int) SettingsDesign.RelatedProductSourceType).ToString();

            txtRelatedProductsMaxCount.Text = SettingsCatalog.RelatedProductsMaxCount.ToString();
        }


        public bool SaveData()
        {
            if (!ValidateData())
                return false;

            SettingsCatalog.DisplayWeight = chkDisplayWeight.Checked;
            SettingsCatalog.DisplayDimensions = chkDisplayDimensions.Checked;
            SettingsCatalog.ShowStockAvailability = cbShowStockAvailability.Checked;

            SettingsCatalog.CompressBigImage = chkCompressBigImage.Checked;
            
            SettingsCatalog.ModerateReviews = ckbModerateReviews.Checked;
            SettingsCatalog.AllowReviews = chkAllowReviews.Checked;
            SettingsCatalog.DisplayReviewsImage = chkDisplayReviewsImage.Checked;
            SettingsCatalog.AllowReviewsImageUploading = chkAllowReviewsImageUploading.Checked;
            SettingsPictureSize.ReviewImageWidth = txtReviewImageWidth.Text.TryParseInt();
            SettingsPictureSize.ReviewImageHeight = txtReviewImageHeight.Text.TryParseInt();

            SettingsDesign.EnableZoom = chkEnableZoom.Checked;

            SettingsDesign.ShowShippingsMethodsInDetails =
                (SettingsDesign.eShowShippingsInDetails)
                    SQLDataHelper.GetInt(ddlShowShippingsMethodsInDetails.SelectedValue);
            SettingsDesign.ShippingsMethodsInDetailsCount = txtShippingsMethodsInDetailsCount.Text.TryParseInt();

            SettingsCatalog.RelatedProductName = txtBlockOne.Text;
            SettingsCatalog.AlternativeProductName = txtBlockTwo.Text;
            SettingsDesign.RelatedProductSourceType =
                (SettingsDesign.eRelatedProductSourceType)
                    SQLDataHelper.GetInt(ddlRelatedProductSourceType.SelectedValue);

            var relatedProductsMaxCount = txtRelatedProductsMaxCount.Text.TryParseInt();
            SettingsCatalog.RelatedProductsMaxCount = relatedProductsMaxCount < 0 || relatedProductsMaxCount > 50
                ? 12
                : relatedProductsMaxCount;

            return true;
        }

        private bool ValidateData()
        {
            return true;
        }
    }
}