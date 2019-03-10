//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Helpers;

namespace ClientPages
{
    public partial class err500 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonHelper.DisableBrowserCache();
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = 500;
            Response.Status = "500 Internal Server Error";
        }
    }
}