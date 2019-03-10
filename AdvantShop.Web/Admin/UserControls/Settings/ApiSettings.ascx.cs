using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Localization;

namespace Admin.UserControls.Settings
{
    public partial class ApiSettings : System.Web.UI.UserControl
    {
        protected bool IsRu = Culture.Language == Culture.SupportLanguage.Russian ||
                              Culture.Language == Culture.SupportLanguage.Ukrainian;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            var apikey = SettingsApi.ApiKey;
            var siteUrl = SettingsMain.SiteUrl.TrimEnd('/');

            txtApiKey.Text = apikey;

            if (IsRu)
            {
                lblImportProductsUrl.Text = siteUrl + "/api/1c/importproducts?apikey=" + apikey;
                lblImportPhotosUrl.Text = siteUrl + "/api/1c/importphotos?apikey=" + apikey;
                lblExportProductsUrl.Text = siteUrl + "/api/1c/exportproducts?apikey=" + apikey;
                lblExportOrdersUrl.Text = siteUrl + "/api/1c/exportorders?apikey=" + apikey;
                lblChangeOrderStatusUrl.Text = siteUrl + "/api/1c/changeorderstatus?apikey=" + apikey;
                lblDeletedOrdersUrl.Text = siteUrl + "/api/1c/deletedorders?apikey=" + apikey;
                lblDeletedProducts.Text = siteUrl + "/api/1c/deletedproducts?apikey=" + apikey;

                chk1CEnabled.Checked = Settings1C.Enabled;
                chk1CDisableProductsDecremention.Checked = Settings1C.DisableProductsDecremention;
                ddlExportOrdersType.SelectedValue = Settings1C.OnlyUseIn1COrders ? "0" : "1";

                chk1CUpdateStatuses.Checked = Settings1C.UpdateStatuses;
                ddl1CUpdateProducts.SelectedValue = Settings1C.UpdateProducts ? "0" : "1";
                ddl1CSendProducts.SelectedValue = Settings1C.SendAllProducts ? "0" : "1";

            }
        }

        public bool SaveData()
        {
            SettingsApi.ApiKey = txtApiKey.Text.Trim();

            if (IsRu)
            {
                Settings1C.Enabled = chk1CEnabled.Checked;
                Settings1C.DisableProductsDecremention = chk1CDisableProductsDecremention.Checked;

                Settings1C.OnlyUseIn1COrders = ddlExportOrdersType.SelectedValue == "0";

                Settings1C.UpdateStatuses = chk1CUpdateStatuses.Checked;
                Settings1C.UpdateProducts = ddl1CUpdateProducts.SelectedValue == "0";
                Settings1C.SendAllProducts = ddl1CSendProducts.SelectedValue == "0";
            }

            LoadData();
            return true;
        }


        protected void lbGenerateApiKey_Click(object sender, EventArgs e)
        {
            txtApiKey.Text = Guid.NewGuid().ToString().Sha256();

            SaveData();
        }
    }
}