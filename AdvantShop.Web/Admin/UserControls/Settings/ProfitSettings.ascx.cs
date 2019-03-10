using System;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using Resources;

namespace Admin.UserControls.Settings
{
    public partial class ProfitSettings : System.Web.UI.UserControl
    {
        public string ErrMessage = Resource.Admin_CommonSettings_InvalidProfit;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            txtSalesPlan.Text = PriceFormatService.FormatPrice(OrderStatisticsService.SalesPlan, CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3));
            txtProfitPlan.Text = PriceFormatService.FormatPrice(OrderStatisticsService.ProfitPlan, CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3));

        }

        public bool SaveData()
        {
            var cu = CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3);
            bool isValid = true;

            float sales = -1;
            float profit = -1;

            if (float.TryParse(txtSalesPlan.Text.Replace(cu.Symbol, "").Replace(" ", ""), out sales) && sales >= 0 && float.TryParse(txtProfitPlan.Text.Replace(cu.Symbol, "").Replace(" ", ""), out profit) && profit >= 0)
            {
                OrderStatisticsService.SetProfitPlan(sales, profit);
            }
            else
            {
                ErrMessage += Resource.Admin_CommonSettings_ProfitError;
                isValid = false;
            }
        
            LoadData();
            return isValid;
        }
    }
}