using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.ViewModels.Common;

namespace AdvantShop.Web.Admin.Handlers.Common
{
    public class GetLastStastics
    {
        public LastStatisticsViewModel Execute()
        {
            var model = new LastStatisticsViewModel();

            var currentCustomer = CustomerContext.CurrentCustomer;
            var currentManager = ManagerService.GetManager(currentCustomer.Id);

            model.RemainingLessons = StatisticService.GetRemainingLessons();

            if (currentCustomer.HasRoleAction(RoleAction.Tasks) && currentManager != null)
                model.LastTasksCount = TaskService.GetOpenTasksCount(currentManager.ManagerId);

            // статистика для админа без учета менеджера
            var managerId = currentCustomer.IsAdmin || currentManager == null ? (int?)null : currentManager.ManagerId;

            if (currentCustomer.HasRoleAction(RoleAction.Orders))
                model.LastOrdersCount = StatisticService.GetLastOrdersCount(managerId);
            if (currentCustomer.HasRoleAction(RoleAction.Crm))
                model.LastLeadsCount = LeadService.GetLeadsCount(managerId);
            if (currentCustomer.HasRoleAction(RoleAction.Catalog))
                model.LastReviews = StatisticService.GetLastReviewsCount();

            return model;
        }
    }
}
