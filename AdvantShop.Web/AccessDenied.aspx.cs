//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Controls;
using AdvantShop.Helpers;

namespace ClientPages
{
    public partial class AccessDenied : AdvantShopPage
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            CommonHelper.DisableBrowserCache();
        }
    }
}
