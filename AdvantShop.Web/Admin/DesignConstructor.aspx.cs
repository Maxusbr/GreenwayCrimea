//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using Resources;

namespace Admin
{
    public partial class DesignConstructor : AdvantShopAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_DesignSettings_Header));
        }
    }
}