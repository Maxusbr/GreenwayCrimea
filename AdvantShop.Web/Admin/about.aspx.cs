//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Core.Controls;

namespace Admin
{
    public partial class About : AdvantShopAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName,
                                  Resources.Resource.Admin_MasterPageAdmin_About));
        }
    }
}
