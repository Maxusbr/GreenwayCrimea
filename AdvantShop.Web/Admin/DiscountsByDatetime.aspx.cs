//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using Resources;
using AdvantShop.Catalog;

namespace Admin
{
    public partial class DiscountsByDatetime : AdvantShopAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_DiscountsByDatetime_Header));
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            chkEnabled.Checked = DiscountByTimeService.Enabled;
            txtFromDateTime.Text = DiscountByTimeService.FromDateTime.ToShortTimeString();
            txtToDateTime.Text = DiscountByTimeService.ToDateTime.ToShortTimeString();
            txtDiscountByTime.Text = DiscountByTimeService.DiscountByTime.ToString();
            FCKDiscountPopupText.Text = DiscountByTimeService.PopupText;
            chkShowPopup.Checked = DiscountByTimeService.ShowPopup;
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            DiscountByTimeService.Enabled = chkEnabled.Checked;
            DiscountByTimeService.FromDateTime = txtFromDateTime.Text.TryParseDateTime();
            DiscountByTimeService.ToDateTime = txtToDateTime.Text.TryParseDateTime();
            DiscountByTimeService.DiscountByTime = txtDiscountByTime.Text.TryParseFloat();
            DiscountByTimeService.PopupText = FCKDiscountPopupText.Text;
            DiscountByTimeService.ShowPopup = chkShowPopup.Checked;
        }
    }
}