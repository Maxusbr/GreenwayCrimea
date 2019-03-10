using System;
using System.Linq;
using System.Web.UI;
using AdvantShop.Customers;

namespace Admin.UserControls.Dashboard
{
    public partial class Navigation : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var _customer = CustomerContext.CurrentCustomer;

            if (_customer.CustomerRole == Role.Moderator)
            {
                var actions = RoleActionService.GetCustomerRoleActionsByCustomerId(_customer.Id);

                dashCatalog.Visible = actions.Any(item => item.Role == RoleAction.Catalog);
                dashOrder.Visible = actions.Any(item => item.Role == RoleAction.Orders);
                dashNews.Visible = actions.Any(item => item.Role == RoleAction.Cms);
                dashImportCsv.Visible = actions.Any(item => item.Role == RoleAction.Catalog);
                dashModules.Visible = actions.Any(item => item.Role == RoleAction.Modules);
                dashDesign.Visible = actions.Any(item => item.Role == RoleAction.Design);

                this.Visible = dashCatalog.Visible || dashOrder.Visible || dashNews.Visible || dashImportCsv.Visible || dashModules.Visible || dashDesign.Visible;
            }
        }
    }
}