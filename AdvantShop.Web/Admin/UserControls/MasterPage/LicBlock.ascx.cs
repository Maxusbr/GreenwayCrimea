using System;
using AdvantShop.Configuration;

namespace AdvantShop.Admin.UserControls.MasterPage
{
    public partial class LicBlock : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lCode.Text = SettingsLic.ClientCode;
        }
    }
}