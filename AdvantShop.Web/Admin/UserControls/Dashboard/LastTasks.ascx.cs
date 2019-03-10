using System;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Saas;

namespace Admin.UserControls.Dashboard
{
    public partial class LastTasks : System.Web.UI.UserControl
    {
        protected string ManagerId;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if ((SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCrm) ||
                !SettingsCheckout.EnableManagersModule || 
                !CustomerContext.CurrentCustomer.IsManager)
            {
                Visible = false;
                return;
            }

            var manager = ManagerService.GetManager(CustomerContext.CurrentCustomer.Id);

            lvLastTasks.DataSource = ManagerTaskService.GetManagerTasks(manager.ManagerId, ManagerTaskStatus.Opened, 7);
            lvLastTasks.DataBind();

            ManagerId = manager.CustomerId.ToString();
        }

        protected void lvLastTasks_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "DeleteTask")
            {
                ManagerTaskService.DeleteManagerTask(e.CommandArgument.ToString().TryParseInt());
            }
        }
    }
}