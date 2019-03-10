using System;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public enum EManagerFilterType
    {
        [Localize("Core.Crm.EManagerFilterType.Any")]
        Any = 0,
        [Localize("Core.Crm.EManagerFilterType.MostFree")]
        MostFree = 1,
        [Localize("Core.Crm.EManagerFilterType.Specific")]
        Specific = 2,

        FromBizObject = 3
    }

    public class ManagerFieldComparer
    {
        public EManagerFilterType FilterType { get; set; }

        public Guid? CustomerId { get; set; }

        public int? ManagerRoleId { get; set; }

        public string City { get; set; }

        public string FilterTypeName
        {
            get { return FilterType.Localize(); }
        }

        private string _customerName;
        public string CustomerName
        {
            get
            {
                if (_customerName.IsNotEmpty() || !CustomerId.HasValue)
                    return _customerName;
                var customer = CustomerService.GetCustomer(CustomerId.Value);
                if (customer != null)
                    _customerName = StringHelper.AggregateStrings(" ", customer.FirstName, customer.LastName);
                return _customerName;
            }
        }

        private string _managerRoleName;
        public string ManagerRoleName
        {
            get
            {
                if (_managerRoleName.IsNotEmpty() || !ManagerRoleId.HasValue)
                    return _managerRoleName;
                var managerRole = ManagerRoleService.GetManagerRole(ManagerRoleId.Value);
                _managerRoleName = managerRole != null ? managerRole.Name : null;
                return _managerRoleName;
            }
        }
    }
}
