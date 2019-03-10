using System;
using AdvantShop.Saas;

namespace AdvantShop.Admin.UserControls.MasterPage
{
    public partial class SaasBlock : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SaasDataService.IsSaasEnabled)
            {
                this.Visible = false;
                return;
            }

            lDate.Text = DateTime.Now.AddDays(SaasDataService.CurrentSaasData.LeftDay).ToShortDateString();
           
        }
    }
}