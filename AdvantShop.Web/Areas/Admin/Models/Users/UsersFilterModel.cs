using System;
using AdvantShop.Customers;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Users
{
    public class UsersFilterModel : BaseFilterModel<Guid>
    {
        public Role Role { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int? DepartmentId { get; set; }
        public bool? Enabled { get; set; }
        public bool? HasPhoto { get; set; }
    }
}
