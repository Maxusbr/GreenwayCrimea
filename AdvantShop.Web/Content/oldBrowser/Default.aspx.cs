using System;
using System.Web.UI;

namespace ClientPages
{
    public partial class ie6_Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AdvantShop.Localization.Culture.InitializeCulture();
        }
    }
}