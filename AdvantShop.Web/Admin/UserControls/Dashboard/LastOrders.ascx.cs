using System;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Saas;

namespace Admin.UserControls.Dashboard
{
    public partial class Admin_UserControls_LastOrders : System.Web.UI.UserControl
    {
        protected bool AvalableAssignedOrders;

        protected bool AvalableNotAssignedOrders;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            lvLastOrders.DataSource = OrderService.GetLastOrders(7);
            lvLastOrders.DataBind();

            if (SettingsCheckout.EnableManagersModule 
                && (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveCrm)))
            {
                orderRadioBlock.Visible = true;

                Manager currentManager;
                if (CustomerContext.CurrentCustomer.IsManager && (currentManager = ManagerService.GetManager(CustomerContext.CurrentCustomer.Id)) != null)
                {
                    lvAssignedOrders.DataSource = OrderService.GetLastOrders(7, currentManager.ManagerId);
                    lvAssignedOrders.DataBind();
                    
                    lvNotAssignedOrders.DataSource = OrderService.GetLastOrders(7, null);
                    lvNotAssignedOrders.DataBind();
                    
                    AvalableAssignedOrders = lvAssignedOrders.Items.Count > 0;
                    AvalableNotAssignedOrders = lvNotAssignedOrders.Items.Count > 0;
                }
            }
        }

        protected void lvLastOrders_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "DeleteOrder")
            {
                OrderService.DeleteOrder(int.Parse(e.CommandArgument.ToString()));
            }
        }
    }
}