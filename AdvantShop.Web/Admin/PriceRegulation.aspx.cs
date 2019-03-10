//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using Resources;
using AdvantShop.Saas;

namespace Admin
{
    public partial class PriceRegulation : AdvantShopAdminPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((SaasDataService.IsSaasEnabled) && (!SaasDataService.CurrentSaasData.HavePriceRegulating))
            {
                mainDiv.Visible = false;
                notInTariff.Visible = true;
            }

            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName,Resource.Admin_PriceRegulation_Header)); 
        }
    }
}