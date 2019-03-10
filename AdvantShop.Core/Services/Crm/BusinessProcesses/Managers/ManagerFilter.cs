using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public class ManagerFilter
    {
        public ManagerFilter()
        {
            Comparers = new List<ManagerFieldComparer>();
        }

        public List<ManagerFieldComparer> Comparers { get; set; }

        public Customer GetCustomer(IBizObject bizObject)
        {
            var customers = CustomerService.GetCustomersByRoles(Role.Administrator, Role.Moderator).Where(x => x.Enabled);
            Customer customer = null;
            foreach (var comparer in Comparers)
            {
                switch (comparer.FilterType)
                {
                    case EManagerFilterType.Any:
                        var rnd = new Random();
                        customer = customers.Where(x => !comparer.ManagerRoleId.HasValue || ManagerRoleService.GetManagerRoles(x.Id).Any(role => role.Id == comparer.ManagerRoleId.Value))
                            .Where(x => comparer.City.IsNullOrEmpty() || string.Equals(x.City, comparer.City, StringComparison.InvariantCultureIgnoreCase))
                            .OrderBy(x => rnd.Next()).FirstOrDefault();
                        break;
                    case EManagerFilterType.MostFree:
                        customer = ManagerService.GetMostFreeCustomer(comparer.ManagerRoleId, comparer.City);
                        break;
                    case EManagerFilterType.Specific:
                        if (comparer.CustomerId.HasValue)
                            customer = customers.FirstOrDefault(x => x.Id == comparer.CustomerId.Value);
                        break;
                    case EManagerFilterType.FromBizObject:
                        var manager = bizObject != null && bizObject.ManagerId.HasValue ? ManagerService.GetManager(bizObject.ManagerId.Value) : null;
                        customer = manager != null ? customers.FirstOrDefault(x => x.Id == manager.CustomerId) : null;
                        break;
                }
                if (customer != null)
                    break;
            }
            return customer;
        }
    }
}
