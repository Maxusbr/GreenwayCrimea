using System.Collections.Generic;
using AdvantShop.Customers;

namespace AdvantShop.ViewModel.MyAccount
{
    public class CommonInfoViewModel
    {
        public string Email { get; set; }
        public string RegistrationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }
        public string CustomerType { get; set; }
        public string CustomerGroup { get; set; }
        public bool SubscribedForNews { get; set; }
        public bool ShowCustomerRole { get; set; }
        public bool ShowCustomerGroup { get; set; }
        public bool ShowSubscription { get; set; }

        public List<CustomerFieldWithValue> CustomerFields { get; set; }
    }
}