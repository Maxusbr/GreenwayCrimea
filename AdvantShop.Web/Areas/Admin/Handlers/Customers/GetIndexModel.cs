using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Models.Customers;
using AdvantShop.Web.Admin.ViewModels.Customers;

namespace AdvantShop.Web.Admin.Handlers.Customers
{
    public class GetIndexModel
    {
        private readonly CustomersFilterModel _filter;

        public GetIndexModel(CustomersFilterModel filter)
        {
            _filter = filter;
        }

        public CustomersListViewModel Execute()
        {
            var model = new CustomersListViewModel()
            {
                Title = LocalizationService.GetResource("Admin.Customers.Index.Buyers"),
                EnableManagers = !SaasDataService.IsSaasEnabled ||
                    (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveCrm)
            };

            if (_filter.Group != 0)
            {
                var group = CustomerGroupService.GetCustomerGroup(_filter.Group);
                if (group != null)
                {
                    model.GroupId = group.CustomerGroupId;
                    model.Title += ", " + LocalizationService.GetResource("Admin.Customers.Index.CustomerGroup") + " " +
                                   group.GroupName;
                }
            }

            return model;
        }
    }
}
