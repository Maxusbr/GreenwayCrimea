using System.Web.Mvc;
using AdvantShop.Areas.AdminMobile.Models.Sidebar;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Customers;
using AdvantShop.Orders;

namespace AdvantShop.Areas.AdminMobile.Controllers
{
    public class SidebarController : BaseAdminMobileController
    {
        public ActionResult Sidebar()
        {
            var customer = CustomerContext.CurrentCustomer;
            var manager = ManagerService.GetManager(CustomerContext.CurrentCustomer.Id);

            var model = new SidebarModel
            {
                Customer = customer,
                ShowOrders =  HasRole(RoleAction.Orders),
                OrdersCount = customer.IsAdmin
                    ? OrderStatusService.GetOrderCountByStatusId(OrderStatusService.DefaultOrderStatus)
                    : OrderStatusService.GetOrderCountByStatusIdAndManagerId(OrderStatusService.DefaultOrderStatus, manager.ManagerId)
                ,
                ShowTasks = SettingsCheckout.EnableManagersModule && customer.IsManager && HasRole(RoleAction.Crm),
                TasksCount = customer.IsAdmin
                    ? ManagerTaskService.GetManagerTasksCount(ManagerTaskStatus.Opened)
                    : ManagerTaskService.GetManagerTasksCountByManagerId(ManagerTaskStatus.Opened, manager.ManagerId)
                ,
                ShowLeads = HasRole(RoleAction.Crm),
                LeadsCount = customer.IsAdmin
                    ? LeadService.GetLeadsCount(default(int?))
                    : LeadService.GetLeadsCount(manager.ManagerId)
            };

            return PartialView(model);
        }
    }
}