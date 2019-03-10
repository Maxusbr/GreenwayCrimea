using System;
using System.Drawing;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.StoreReviews
{
    public partial class Admin_StoreReviewsSettings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            chkShowRatio.Checked = ModuleSettingsProvider.GetSettingValue<bool>("ShowRatio", StoreReviews.ModuleID);
            chkUseCaptcha.Checked = ModuleSettingsProvider.GetSettingValue<bool>("UseCaptcha", StoreReviews.ModuleID);
            ckbActiveModerate.Checked = ModuleSettingsProvider.GetSettingValue<bool>("ActiveModerateStoreReviews", StoreReviews.ModuleID);
            txtPageSize.Text = ModuleSettingsProvider.GetSettingValue<string>("PageSize", StoreReviews.ModuleID);

            txtPageTitle.Text = ModuleSettingsProvider.GetSettingValue<string>("PageTitle", StoreReviews.ModuleID);
            txtMetaDescription.Text = ModuleSettingsProvider.GetSettingValue<string>("MetaDescription", StoreReviews.ModuleID);
            txtMetaKeyWords.Text = ModuleSettingsProvider.GetSettingValue<string>("MetaKeyWords", StoreReviews.ModuleID);

            ckbAllowImageUploading.Checked = ModuleSettingsProvider.GetSettingValue<bool>("AllowImageUploading", StoreReviews.ModuleID);
            txtMaxImageWidth.Text = ModuleSettingsProvider.GetSettingValue<string>("MaxImageWidth", StoreReviews.ModuleID);
            txtMaxImageHeight.Text = ModuleSettingsProvider.GetSettingValue<string>("MaxImageHeight", StoreReviews.ModuleID);
        }

        protected void Save()
        {
            if (!Validate())
            {

                lblMessage.Text = (string)GetLocalResourceObject("StoreReviews_WrongData");
                lblMessage.ForeColor = Color.Red;
                lblMessage.Visible = true;
                return;
            }

            ModuleSettingsProvider.SetSettingValue("ShowRatio", chkShowRatio.Checked, StoreReviews.ModuleID);
            ModuleSettingsProvider.SetSettingValue("UseCaptcha", chkUseCaptcha.Checked, StoreReviews.ModuleID);
            ModuleSettingsProvider.SetSettingValue("ActiveModerateStoreReviews", ckbActiveModerate.Checked, StoreReviews.ModuleID);
            ModuleSettingsProvider.SetSettingValue("PageSize", txtPageSize.Text, StoreReviews.ModuleID);

            ModuleSettingsProvider.SetSettingValue("PageTitle", txtPageTitle.Text, StoreReviews.ModuleID);
            ModuleSettingsProvider.SetSettingValue("MetaDescription", txtMetaDescription.Text, StoreReviews.ModuleID);
            ModuleSettingsProvider.SetSettingValue("MetaKeyWords", txtMetaKeyWords.Text, StoreReviews.ModuleID);

            ModuleSettingsProvider.SetSettingValue("AllowImageUploading", ckbAllowImageUploading.Checked, StoreReviews.ModuleID);
            ModuleSettingsProvider.SetSettingValue("MaxImageWidth", txtMaxImageWidth.Text, StoreReviews.ModuleID);
            ModuleSettingsProvider.SetSettingValue("MaxImageHeight", txtMaxImageHeight.Text, StoreReviews.ModuleID);

            lblMessage.Text = (string)GetLocalResourceObject("StoreReviews_ChangesSaved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        protected bool Validate()
        {
            bool valid = true;

            int height = 0;
            int width = 0;
            int pageSize = 0;

            if (!int.TryParse(txtMaxImageHeight.Text, out height))
            {
                txtMaxImageHeight.BorderColor = Color.Red;
                txtMaxImageHeight.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtMaxImageHeight.BorderColor = Color.Gray;
                txtMaxImageHeight.BorderWidth = 1;
            }

            if (!int.TryParse(txtMaxImageWidth.Text, out width))
            {
                txtMaxImageWidth.BorderColor = Color.Red;
                txtMaxImageWidth.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtMaxImageWidth.BorderColor = Color.Gray;
                txtMaxImageWidth.BorderWidth = 1;
            }

            if (!int.TryParse(txtPageSize.Text, out pageSize))
            {
                txtPageSize.BorderColor = Color.Red;
                txtPageSize.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtPageSize.BorderColor = Color.Gray;
                txtPageSize.BorderWidth = 1;
            }

            return valid;
        }
    }
}