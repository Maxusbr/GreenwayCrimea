using System;
using AdvantShop.Module.YaBuying.Domain;

namespace AdvantShop.Module.YaBuying
{
    public partial class Admin_YaMarketBuyingHistory : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            lvHistory.DataSource = YaMarketByuingService.GetHistory();
            lvHistory.DataBind();
        }
    }
}