using System;
using System.Collections.Generic;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Models.Common;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Customers
{
    public class CustomersFilterModel : BaseFilterModel<Guid>
    {
        public Role? Role { get; set; }
        public int Group { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ManagerName { get; set; }
        public string LastOrderNumber { get; set; }
        public float OrdersCountFrom { get; set; }
        public float OrdersCountTo { get; set; }
        public string Location { get; set; }
        public float OrderSumFrom { get; set; }
        public float OrderSumTo { get; set; }
        public string RegistrationDateTimeFrom { get; set; }
        public string RegistrationDateTimeTo { get; set; }
        public int LastOrderFrom { get; set; }
        public int LastOrderTo { get; set; }

        public Dictionary<string, CustomerFiledFilterModel> CustomerFields { get; set; }
    }
}
