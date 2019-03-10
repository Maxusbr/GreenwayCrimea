using System;
using System.Drawing;
using System.Text;
using System.Web.UI;
using AdvantShop.Core.Modules;
using AdvantShop.Repository.Currencies;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace AdvantShop.Module.MoySklad
{
    public partial class Modules_MoySklad_MoySkladModule : UserControl
    {
        private static readonly string _moduleName = MoySklad.GetModuleStringId();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSettings();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        protected void LoadSettings()
        {
            txtSnowfallMaxSendOrders.Text =
                ModuleSettingsProvider.GetSettingValue<int>("MoySkladCSSO", _moduleName).ToString();

            var moySklad = new MoySklad();

            var typeSyncProp = (byte) moySklad.SyncProperty;
            if (ddlSyncProp.Items.FindByValue(typeSyncProp.ToString()) != null)
                ddlSyncProp.SelectedValue = typeSyncProp.ToString();

            var typeSyncDesc = (byte) moySklad.SyncDescription;
            if (ddlSyncDesc.Items.FindByValue(typeSyncDesc.ToString()) != null)
                ddlSyncDesc.SelectedValue = typeSyncDesc.ToString();

            var typeUpdateEnableProduct = (byte) moySklad.UpdateEnableProduct;
            if (ddlUpdateEnableProduct.Items.FindByValue(typeUpdateEnableProduct.ToString()) != null)
                ddlUpdateEnableProduct.SelectedValue = typeUpdateEnableProduct.ToString();

            cbIsSetNoEnableProductNotSuncMoysklad.Checked = moySklad.IsSetNoEnableProductNotSuncMoysklad;
            cbIsDeleteOfferNotSuncMoysklad.Checked = moySklad.IsDeleteOfferNotSuncMoysklad;
            cbDeleteOffersWithZeroAmount.Checked = moySklad.DeleteOffersWithZeroAmount;
            cbAvailablePreOrder.Checked = moySklad.AvailablePreOrder;
            cbDontChangeArtnoToProductArtno.Checked = moySklad.DontChangeOfferArtnoToProductArtno;
            txtOrderPrefix.Text = moySklad.OrderPrefix;
            cbNotUpdateAmount.Checked = moySklad.NotUpdateAmount;
            cbNotUpdatePrice.Checked = moySklad.NotUpdatePrice;

            cbIsNewCategoryEnabled.Checked = moySklad.IsNewCategoryEnabled;
            cbUpdateOnlyProducts.Checked = moySklad.UpdateOnlyProducts;
            cbUseZip.Checked = moySklad.UseZip;

            var typeSyncOrders = (byte) moySklad.SyncOrders;
            if (ddlSyncOrders.Items.FindByValue(typeSyncOrders.ToString()) != null)
                ddlSyncOrders.SelectedValue = typeSyncOrders.ToString();

            txtNamePropWeight.Text = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNamePropWeight",
                                                                                    _moduleName);
            txtNamePropSize.Text = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNamePropSize", _moduleName);
            txtNamePropBrand.Text = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNamePropBrand", _moduleName);
            txtNamePropDiscount.Text = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNamePropDiscount",
                                                                                      _moduleName);
            txtNamePropGtin.Text = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNamePropGtin", _moduleName);
            txtBarCode.Text = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNameBarCode", _moduleName);

            txtPropNoLoad.Text = string.Join("\r\n",
                                             ModuleSettingsProvider.GetSettingValue<string>("MoySkladPropNoLoad",
                                                                                            _moduleName)
                                                                   .Split(new[] {"[;]"},
                                                                          StringSplitOptions.RemoveEmptyEntries));
            txtNameCharactColor.Text = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNameCharactColor",
                                                                                      _moduleName);
            txtNameCharactSize.Text = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNameCharactSize",
                                                                                     _moduleName);

            txtRetailPriceName.Text = ModuleSettingsProvider.GetSettingValue<string>("MoySkladRetailPriceName", _moduleName);

            txtApiLogin.Text = ModuleSettingsProvider.GetSettingValue<string>("MoySkladApiLogin", _moduleName);

            txtApiPassword.Text = ModuleSettingsProvider.GetSettingValue<string>("MoySkladApiPassword", _moduleName);

            cbUpdateCustomersAndContacts.Checked = moySklad.UpdateCustomersAndContacts;
            cbUpdateOrdersStatuses.Checked = moySklad.UpdateOrdersStatuses;
			var currency = CurrencyService.GetAllCurrencies();
            var listitem = new List<ListItem>() { new ListItem ("Брать из розничной цены товара в Мой склад", "-1") };
			foreach (var cur in currency)
			{
				listitem.Add(new ListItem { Text = cur.Name, Value = cur.CurrencyId.ToString(), Selected = cur.CurrencyId == ModuleSettingsProvider.GetSettingValue<int>("MoySkladImportCurrencyInProdyuct", _moduleName) ? true : false });
			}
			ddlImportCurrency.Items.Clear();
			ddlImportCurrency.Items.AddRange(listitem.ToArray());

            inpApiPasswordCompare.Value = DateTime.Now.ToString("ddhmmss");
        }


        protected void Save()
        {
            var errMessage = new StringBuilder();
            const string tpl = "<li>{0}</li>";
            bool isExistErr = false;

            errMessage.Append("<ul style=\"padding:0; margin:0;\">");

            int maxOrders;
            if (!int.TryParse(txtSnowfallMaxSendOrders.Text, out maxOrders) && maxOrders <= 0)
            {
                isExistErr = true;
                errMessage.AppendFormat(tpl, GetLocalResourceObject("MoySkladValid_MaxSendOrders"));
            }

            if ((cbUpdateCustomersAndContacts.Checked || cbUpdateOrdersStatuses.Checked) && (string.IsNullOrEmpty(txtApiLogin.Text) || string.IsNullOrEmpty(txtApiPassword.Text)))
            {
                isExistErr = true;
                errMessage.AppendFormat(tpl, "Необходимо указать доступы подключения к API МойСклад");
            }

            errMessage.Append("</ul>");

            if (isExistErr)
            {
                lblErr.Text = errMessage.ToString();
                lblErr.ForeColor = Color.Red;
                lblErr.Visible = true;
                return;
            }

            ModuleSettingsProvider.SetSettingValue("MoySkladCSSO", maxOrders, _moduleName);

            var moySklad = new MoySklad();

            moySklad.SyncProperty = (MoySklad.EnSyncProperty) byte.Parse(ddlSyncProp.SelectedValue);
            moySklad.SyncDescription = (MoySklad.EnSyncDescription) byte.Parse(ddlSyncDesc.SelectedValue);
            moySklad.SyncOrders = (MoySklad.EnSyncOrders) byte.Parse(ddlSyncOrders.SelectedValue);

            moySklad.UpdateEnableProduct =
                (MoySklad.EnUpdateEnableProduct) byte.Parse(ddlUpdateEnableProduct.SelectedValue);

            moySklad.IsSetNoEnableProductNotSuncMoysklad = cbIsSetNoEnableProductNotSuncMoysklad.Checked;
            moySklad.IsDeleteOfferNotSuncMoysklad = cbIsDeleteOfferNotSuncMoysklad.Checked;
            moySklad.DeleteOffersWithZeroAmount = cbDeleteOffersWithZeroAmount.Checked;
            moySklad.AvailablePreOrder = cbAvailablePreOrder.Checked;
            moySklad.DontChangeOfferArtnoToProductArtno = cbDontChangeArtnoToProductArtno.Checked;
            moySklad.OrderPrefix = txtOrderPrefix.Text.Trim();
            moySklad.NotUpdateAmount = cbNotUpdateAmount.Checked;
            moySklad.NotUpdatePrice = cbNotUpdatePrice.Checked;

            moySklad.IsNewCategoryEnabled = cbIsNewCategoryEnabled.Checked;
            moySklad.UpdateOnlyProducts = cbUpdateOnlyProducts.Checked;
            moySklad.UseZip = cbUseZip.Checked;

            ModuleSettingsProvider.SetSettingValue("MoySkladNamePropWeight", txtNamePropWeight.Text.Trim(), _moduleName);
            ModuleSettingsProvider.SetSettingValue("MoySkladNamePropSize", txtNamePropSize.Text.Trim(), _moduleName);
            ModuleSettingsProvider.SetSettingValue("MoySkladNamePropBrand", txtNamePropBrand.Text.Trim(), _moduleName);
            ModuleSettingsProvider.SetSettingValue("MoySkladNamePropDiscount", txtNamePropDiscount.Text.Trim(),
                                                   _moduleName);
            ModuleSettingsProvider.SetSettingValue("MoySkladNamePropGtin", txtNamePropGtin.Text.Trim(), _moduleName);
            ModuleSettingsProvider.SetSettingValue("MoySkladNameBarCode", txtBarCode.Text.Trim(), _moduleName);

            ModuleSettingsProvider.SetSettingValue("MoySkladPropNoLoad",
                                                   string.Join("[;]",
                                                               txtPropNoLoad.Text.Trim()
                                                                            .Split(new[] {"\r\n"},
                                                                                   StringSplitOptions.RemoveEmptyEntries)),
                                                   _moduleName);
            ModuleSettingsProvider.SetSettingValue("MoySkladNameCharactColor", txtNameCharactColor.Text.Trim(), _moduleName);
            ModuleSettingsProvider.SetSettingValue("MoySkladNameCharactSize", txtNameCharactSize.Text.Trim(), _moduleName);

            ModuleSettingsProvider.SetSettingValue("MoySkladRetailPriceName", txtRetailPriceName.Text.Trim(), _moduleName);

            ModuleSettingsProvider.SetSettingValue("MoySkladApiLogin", txtApiLogin.Text, _moduleName);

            if (txtApiPassword.Text != inpApiPasswordCompare.Value)
                ModuleSettingsProvider.SetSettingValue("MoySkladApiPassword", txtApiPassword.Text, _moduleName);

			ModuleSettingsProvider.SetSettingValue("MoySkladImportCurrencyInProdyuct", Convert.ToInt32(ddlImportCurrency.SelectedItem.Value), _moduleName);
            moySklad.UpdateCustomersAndContacts = cbUpdateCustomersAndContacts.Checked;
            moySklad.UpdateOrdersStatuses = cbUpdateOrdersStatuses.Checked;

            lblMessage.Text = (String) GetLocalResourceObject("MoySklad_Message");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;

            LoadSettings();
        }
    }
}