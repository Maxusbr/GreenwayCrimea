using System;
using AdvantShop.Customers;
using AdvantShop.Saas;

namespace Admin.UserControls.MasterPage
{
    public partial class CurrentSaasData : System.Web.UI.UserControl
    {
        protected SaasData MySaasData;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SaasDataService.IsSaasEnabled && (CustomerContext.CurrentCustomer.IsAdmin || CustomerContext.CurrentCustomer.IsVirtual))
            {
                MySaasData = SaasDataService.CurrentSaasData;
            }
            else
            {
                this.Visible = false;
            }
        }

        protected string RenderClass()
        {
            if (MySaasData.LeftDay > 15)
                return "battery-progress-normal";
            if (MySaasData.LeftDay > 5)
                return "battery-progress-less";
            return "battery-progress-end";
        }

        protected void lbUpdate_OnClick(object sender, EventArgs e)
        {
            MySaasData = SaasDataService.GetSaasData(true);
        }
    }
}