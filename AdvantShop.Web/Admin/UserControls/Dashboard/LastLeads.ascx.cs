using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Saas;

namespace Admin.UserControls.Dashboard
{
    public partial class LastLeads : System.Web.UI.UserControl
    {
        protected string ManagerId;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if ((SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCrm) ||
                //!CustomerContext.CurrentCustomer.IsManager)
            {// hide for all users, for new leads go to new /adminv2
                Visible = false;
                return;
            }

            var manager = ManagerService.GetManager(CustomerContext.CurrentCustomer.Id);

            var leads =
                SQLDataAccess.ExecuteTable(
                    "Select Top(@count) * From (Select * From [Order].[Lead] Where ManagerId = @managerId) as leads " +
                    "Where leads.LeadStatus = @status1 or leads.LeadStatus = @status2 " +
                    "Order BY leads.CreatedDate Desc",
                    CommandType.Text,
                    new SqlParameter("@count", 5),
                    new SqlParameter("@managerId", manager.ManagerId),
                    new SqlParameter("@status1", LeadStatus.New.ToString()),
                    new SqlParameter("@status2", LeadStatus.Processing.ToString())
                    );

            lvLastLeads.DataSource = leads;
            lvLastLeads.DataBind();

            ManagerId = manager.CustomerId.ToString();
        }

        protected void lvLastLeads_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "DeleteLead")
            {
                LeadService.DeleteLead(e.CommandArgument.ToString().TryParseInt());
            }
        }

        protected string GetLeadStatus(string status)
        {
            LeadStatus s;
            if (!Enum.TryParse(status, out s))
                return "";

            return s.Localize();
        }
    }
}