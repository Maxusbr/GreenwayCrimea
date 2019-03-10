//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Core.Controls;
using AdvantShop.Notifications;

namespace Admin
{
    partial class AdminMessages : AdvantShopAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName, "Сообщения AdVantShop.Net"));
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var items = AdminMessagesService.GetAdminMessagesWithoutMessages().Items;
            lvAdminMessages.DataSource = items;
            lvAdminMessages.DataBind();
        }
    }
}