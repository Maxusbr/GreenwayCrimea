//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Orders;
using AdvantShop.Saas;
using Resources;

namespace Admin
{
    public partial class Statistics : AdvantShopAdminPage
    {
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private int? _statusId;

        protected bool ShowCrm = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            Filter();

            ShowCrm = !(SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCrm);
            
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Statistics_Header));
        }
        
        private void Filter()
        {
            if (txtDateFrom.Text.IsNullOrEmpty() || txtDateTo.Text.IsNullOrEmpty())
            {
                _dateFrom = DateTime.Now.AddMonths(-1);
                _dateTo = DateTime.Now;

                txtDateFrom.Text = _dateFrom.ToString("dd.MM.yyyy");
                txtDateTo.Text = _dateTo.ToString("dd.MM.yyyy");
            }
            else
            {
                _dateFrom = txtDateFrom.Text.TryParseDateTime();
                _dateTo = txtDateTo.Text.TryParseDateTime();
            }

            _statusId = ddlStatuses.SelectedIndex > 0 ? ddlStatuses.SelectedValue.TryParseInt() : default(int?);

            ddlStatuses.DataSource = OrderStatusService.GetOrderStatuses();
            ddlStatuses.DataBind();
            ddlStatuses.Items.Insert(0, new ListItem(Resource.Admin_Catalog_Any, ""));

            if (_statusId != null && ddlStatuses.Items.FindByValue(((int)_statusId).ToString()) != null)
            {
                ddlStatuses.SelectedValue = ((int)_statusId).ToString();
            }
        }
    }
}