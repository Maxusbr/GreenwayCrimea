using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Customers;

namespace AdvantShop.ViewModel.User
{
    public class RegistrationViewModel
    {
        public bool IsBonusSystemActive { get; set; }
        
        public string BonusesForNewCard { get; set; }

        public bool WantBonusCard { get; set; }

        public bool IsDemo { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Patronymic { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }

        public bool NewsSubscription { get; set; }

        public bool Agree { get; set; }

        public string CaptchaCode { get; set; }

        public string CaptchaSource { get; set; }

        public CustomerGroupType CustomerGroup { get; set; }

        private List<CustomerFieldWithValue> _customerFields;
        public List<CustomerFieldWithValue> CustomerFields
        {
            get
            {
                if (_customerFields != null)
                    return _customerFields;
                _customerFields = CustomerFieldService.GetCustomerFieldsWithValue(Guid.Empty).Where(x => x.ShowInClient).ToList();
                return _customerFields;
            }
            set { _customerFields = value; }
        }
    }
}