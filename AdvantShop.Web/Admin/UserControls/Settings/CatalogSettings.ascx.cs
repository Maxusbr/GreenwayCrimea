using System;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using AdvantShop.Repository.Currencies;
using Resources;
using AdvantShop.Core.Services.FullSearch.Core;
using System.Web.UI.WebControls;
using AdvantShop.FullSearch;

namespace Admin.UserControls.Settings
{
    public partial class CatalogSettings : System.Web.UI.UserControl
    {
        public string ErrMessage = Resource.Admin_CommonSettings_InvalidCatalog;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            ddlDefaultCurrency.DataSource = SqlDataSource2;
            ddlDefaultCurrency.DataTextField = "Name";
            ddlDefaultCurrency.DataValueField = "CurrencyIso3";
            ddlDefaultCurrency.DataBind();

            ddlDefaultCurrency.SelectedValue = SettingsCatalog.DefaultCurrencyIso3;
            cbAllowToChangeCurrency.Checked = SettingsCatalog.AllowToChangeCurrency;

            txtProdPerPage.Text = SettingsCatalog.ProductsPerPage.ToString();
            cbEnableProductRating.Checked = SettingsCatalog.EnableProductRating;
            cbEnablePhotoPreviews.Checked = SettingsCatalog.EnablePhotoPreviews;
            cbShowCountPhoto.Checked = SettingsCatalog.ShowCountPhoto;
            cbEnableCompareProducts.Checked = SettingsCatalog.EnableCompareProducts;
            cbEnableCatalogViewChange.Checked = SettingsCatalog.EnabledCatalogViewChange;
            cbEnableSearchViewChange.Checked = SettingsCatalog.EnabledSearchViewChange;
            cbShowOnlyAvalible.Checked = SettingsCatalog.ShowOnlyAvalible;
            cbMoveNotAvaliableToEnd.Checked = SettingsCatalog.MoveNotAvaliableToEnd;


            cbExluderingFilters.Checked = SettingsCatalog.ExcludingFilters;

            chkShowPriceFilter.Checked = SettingsCatalog.ShowPriceFilter;
            chkShowProducerFilter.Checked = SettingsCatalog.ShowProducerFilter;
            chkShowSizeFilter.Checked = SettingsCatalog.ShowSizeFilter;
            chkShowColorFilter.Checked = SettingsCatalog.ShowColorFilter;
            cbComplexFilter.Checked = SettingsCatalog.ComplexFilter;
            txtSizesHeader.Text = SettingsCatalog.SizesHeader;
            txtColorsHeader.Text = SettingsCatalog.ColorsHeader;

            chkShowQuickView.Checked = SettingsCatalog.ShowQuickView;
            cbShowProductArtNo.Checked = SettingsCatalog.ShowProductArtNo;

            txtColorPictureWidthCatalog.Text = SettingsPictureSize.ColorIconWidthCatalog.ToString();
            txtColorPictureHeightCatalog.Text = SettingsPictureSize.ColorIconHeightCatalog.ToString();
            txtColorPictureWidthDetails.Text = SettingsPictureSize.ColorIconWidthDetails.ToString();
            txtColorPictureHeightDetails.Text = SettingsPictureSize.ColorIconHeightDetails.ToString();
            ddlCatalogView.SelectedValue = ((int)SettingsCatalog.DefaultCatalogView).ToString();
            ddlSearchView.SelectedValue = ((int)SettingsCatalog.DefaultSearchView).ToString();

            txtBuyButtonText.Text = SettingsCatalog.BuyButtonText;
            txtPreOrderButtonText.Text = SettingsCatalog.PreOrderButtonText;

            cbDisplayBuyButton.Checked = SettingsCatalog.DisplayBuyButton;
            cbDisplayPreOrderButton.Checked = SettingsCatalog.DisplayPreOrderButton;

            ckbShowCategoriesInBottomMenu.Checked = SettingsCatalog.DisplayCategoriesInBottomMenu;

            cbShowProductsCount.Checked = SettingsCatalog.ShowProductsCount;

            txtBrandsPerPage.Text = SettingsCatalog.BrandsPerPage.ToString();


            txtSearchExample.Text = SettingsCatalog.SearchExample;

            ckbShowCategoryTree.Checked = SettingsCatalog.ShowCategoryTreeInBrand;
            ckbShowProductsInBrand.Checked = SettingsCatalog.ShowProductsInBrand;

            ddlDeep.Items.Clear();
             foreach (ESearchDeep item in Enum.GetValues(typeof(ESearchDeep)))
            {
                ddlDeep.Items.Add(new ListItem(item.Localize(), item.StrName().ToLower()));
            }
            ddlDeep.DataBind();
            ddlDeep.SelectedValue = SettingsCatalog.SearchDeep.ToString().ToLower();
            txtSearchMaxItems.Text = SettingsCatalog.SearchMaxItems.ToString();

            txBBestDescription.Text = SettingsCatalog.BestDescription;
            txBNewDescription.Text = SettingsCatalog.NewDescription;
            txBDiscountDescription.Text = SettingsCatalog.DiscountDescription;
        }

        public bool SaveData()
        {
            if (!ValidateData())
                return false;

            var iso3 = SettingsCatalog.DefaultCurrencyIso3;
            SettingsCatalog.DefaultCurrencyIso3 = ddlDefaultCurrency.SelectedValue;

            if (SettingsCatalog.DefaultCurrencyIso3 != iso3)
                CurrencyService.CurrentCurrency = CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3);

            SettingsCatalog.AllowToChangeCurrency = cbAllowToChangeCurrency.Checked;

            SettingsCatalog.ProductsPerPage = SQLDataHelper.GetInt(txtProdPerPage.Text);
            SettingsCatalog.EnableProductRating = cbEnableProductRating.Checked;
            SettingsCatalog.EnablePhotoPreviews = cbEnablePhotoPreviews.Checked;
            SettingsCatalog.ShowCountPhoto = cbShowCountPhoto.Checked;
            SettingsCatalog.EnableCompareProducts = cbEnableCompareProducts.Checked;
            SettingsCatalog.EnabledCatalogViewChange = cbEnableCatalogViewChange.Checked;
            SettingsCatalog.EnabledSearchViewChange = cbEnableSearchViewChange.Checked;
            SettingsCatalog.DefaultCatalogView = (ProductViewMode)SQLDataHelper.GetInt(ddlCatalogView.SelectedValue);
            SettingsCatalog.DefaultSearchView = (ProductViewMode)SQLDataHelper.GetInt(ddlSearchView.SelectedValue);
            SettingsCatalog.DisplayCategoriesInBottomMenu = ckbShowCategoriesInBottomMenu.Checked;
            var tempShowOnlyAvalible = SettingsCatalog.ShowOnlyAvalible;
            SettingsCatalog.ShowOnlyAvalible = cbShowOnlyAvalible.Checked;

            if (tempShowOnlyAvalible != SettingsCatalog.ShowOnlyAvalible)
            {
                CategoryService.RecalculateProductsCountManual();
            }

            SettingsCatalog.MoveNotAvaliableToEnd = cbMoveNotAvaliableToEnd.Checked;

            SettingsCatalog.ExcludingFilters = cbExluderingFilters.Checked;
            SettingsCatalog.ShowQuickView = chkShowQuickView.Checked;

            SettingsCatalog.ShowPriceFilter = chkShowPriceFilter.Checked;
            SettingsCatalog.ShowProducerFilter = chkShowProducerFilter.Checked;
            SettingsCatalog.ShowSizeFilter = chkShowSizeFilter.Checked;
            SettingsCatalog.ShowColorFilter = chkShowColorFilter.Checked;
            SettingsCatalog.ComplexFilter = cbComplexFilter.Checked;
            SettingsCatalog.SizesHeader = txtSizesHeader.Text;
            SettingsCatalog.ColorsHeader = txtColorsHeader.Text;
            SettingsCatalog.ShowSizeFilter = chkShowSizeFilter.Checked;
            SettingsCatalog.ShowColorFilter = chkShowColorFilter.Checked;
            SettingsCatalog.ShowProductArtNo = cbShowProductArtNo.Checked;

            SettingsPictureSize.ColorIconWidthCatalog = txtColorPictureWidthCatalog.Text.TryParseInt();
            SettingsPictureSize.ColorIconHeightCatalog = txtColorPictureHeightCatalog.Text.TryParseInt();
            SettingsPictureSize.ColorIconWidthDetails = txtColorPictureWidthDetails.Text.TryParseInt();
            SettingsPictureSize.ColorIconHeightDetails = txtColorPictureHeightDetails.Text.TryParseInt();

            SettingsCatalog.BuyButtonText = txtBuyButtonText.Text;
            SettingsCatalog.PreOrderButtonText = txtPreOrderButtonText.Text;

            SettingsCatalog.DisplayBuyButton = cbDisplayBuyButton.Checked;
            SettingsCatalog.DisplayPreOrderButton = cbDisplayPreOrderButton.Checked;
            
            SettingsCatalog.ShowProductsCount = cbShowProductsCount.Checked;

            SettingsCatalog.ShowCategoryTreeInBrand = ckbShowCategoryTree.Checked;
            SettingsCatalog.ShowProductsInBrand = ckbShowProductsInBrand.Checked;

            SettingsCatalog.SearchExample = txtSearchExample.Text;

            SettingsCatalog.BrandsPerPage = txtBrandsPerPage.Text.TryParseInt();

            SettingsCatalog.SearchDeep = ddlDeep.SelectedValue.TryParseEnum<ESearchDeep>();

            SettingsCatalog.SearchMaxItems = txtSearchMaxItems.Text.TryParseInt();

            SettingsCatalog.BestDescription = txBBestDescription.Text;
            SettingsCatalog.NewDescription = txBNewDescription.Text;
            SettingsCatalog.DiscountDescription = txBDiscountDescription.Text;

            LoadData();
            return true;
        }

        private bool ValidateData()
        {
            if (string.IsNullOrEmpty(ddlDefaultCurrency.SelectedValue))
            {
                ErrMessage = "";
                return false;
            }

            if (string.IsNullOrEmpty(txtProdPerPage.Text))
            {
                ErrMessage = "";
                return false;
            }

            int ti;
            if (!int.TryParse(txtProdPerPage.Text, out ti))
            {
                ErrMessage = Resource.Admin_CommonSettings_NoNumberPerPage;
                return false;
            }
            int maxItem;
            if (int.TryParse(txtSearchMaxItems.Text, out maxItem) && maxItem > 1000)
            {
                ErrMessage = Resource.Admin_CommonSettings_Search_MaxItems;
                return false;
            }
            return true;
        }

        protected void SqlDataSource2_Init(object sender, EventArgs e)
        {
            SqlDataSource2.ConnectionString = Connection.GetConnectionString();
        }

        protected void btnDoindex_Click(object sender, EventArgs e)
        {
            LuceneSearch.CreateAllIndexInBackground();
            lbDone.Visible = true;
        }


    }
}
