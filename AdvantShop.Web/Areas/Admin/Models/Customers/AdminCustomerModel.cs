using System;
using AdvantShop.Customers;
using AdvantShop.Localization;

namespace AdvantShop.Web.Admin.Models.Customers
{
    public partial class AdminCustomerModel
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ManagerId { get; set; }
        public string Rating { get; set; }
        public int LastOrderId { get; set; }
        public string LastOrderNumber { get; set; }
        public float OrdersSum { get; set; }
        public int OrdersCount { get; set; }
        public string Location { get; set; }
        public string ManagerName { get; set; }

        public DateTime RegistrationDateTime { get; set; }

        public string RegistrationDateTimeFormatted
        {
            get { return Culture.ConvertDateWithoutSeconds(RegistrationDateTime); }
        }

        public bool CanBeDeleted
        {
            get { return CustomerService.CanDelete(CustomerId); } 
        }

    }
}
