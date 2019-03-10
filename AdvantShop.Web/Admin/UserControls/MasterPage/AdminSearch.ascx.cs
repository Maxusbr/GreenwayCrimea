//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;

namespace Admin.UserControls.MasterPage
{
    public partial class AdminSearch : System.Web.UI.UserControl
    {
        protected string searchRequest = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["search"].IsNotEmpty())
            {
                searchRequest = HttpUtility.HtmlEncode(Request["search"]);
            }

        }
    }
}