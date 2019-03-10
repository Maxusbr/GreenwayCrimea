using AdvantShop.Customers;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public static class BizProcessRuleExtensions
    {
        public static void RemoveInvalidComparers<T>(this BizProcessRule rule) where T : BizProcessRule, new()
        {
            Customer customer;
            rule.ManagerFilter.Comparers.RemoveAll(x => x.FilterType == EManagerFilterType.Specific && x.CustomerId.HasValue && (customer = CustomerService.GetCustomer(x.CustomerId.Value)) == null);
            foreach (var comparer in rule.ManagerFilter.Comparers)
            {
                ManagerRole managerRole;
                if (comparer.ManagerRoleId.HasValue && (managerRole = ManagerRoleService.GetManagerRole(comparer.ManagerRoleId.Value)) == null)
                    comparer.ManagerRoleId = null;
            }
            switch (rule.ObjectType)
            {
                case EBizProcessObjectType.Order:
                    ((OrderFilter)rule.Filter).Comparers.RemoveAll(x => !x.IsValid());
                    break;
                case EBizProcessObjectType.Lead:
                    ((LeadFilter)rule.Filter).Comparers.RemoveAll(x => !x.IsValid());
                    break;
                case EBizProcessObjectType.Call:
                    ((CallFilter)rule.Filter).Comparers.RemoveAll(x => !x.IsValid());
                    break;
            }
        }
    }
}
