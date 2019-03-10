using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class CheckPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string CompanyName
        {
            get { return Parameters.ElementOrDefault(CheckTemplate.CompanyName); }
            set { Parameters.TryAddValue(CheckTemplate.CompanyName, value); }
        }
        public string Phone
        {
            get { return Parameters.ElementOrDefault(CheckTemplate.Phone); }
            set { Parameters.TryAddValue(CheckTemplate.Phone, value); }
        }
        public string Country
        {
            get { return Parameters.ElementOrDefault(CheckTemplate.Country); }
            set { Parameters.TryAddValue(CheckTemplate.Country, value); }
        }
        public string State
        {
            get { return Parameters.ElementOrDefault(CheckTemplate.State); }
            set { Parameters.TryAddValue(CheckTemplate.State, value); }
        }
        public string City
        {
            get { return Parameters.ElementOrDefault(CheckTemplate.City); }
            set { Parameters.TryAddValue(CheckTemplate.City, value); }
        }
        public string Address
        {
            get { return Parameters.ElementOrDefault(CheckTemplate.Address); }
            set { Parameters.TryAddValue(CheckTemplate.Address, value); }
        }
        public string Fax
        {
            get { return Parameters.ElementOrDefault(CheckTemplate.Fax); }
            set { Parameters.TryAddValue(CheckTemplate.Fax, value); }
        }
        public string IntPhone
        {
            get { return Parameters.ElementOrDefault(CheckTemplate.IntPhone); }
            set { Parameters.TryAddValue(CheckTemplate.IntPhone, value); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(CompanyName)||
                string.IsNullOrWhiteSpace(Phone) ||
                string.IsNullOrWhiteSpace(Country) ||
                string.IsNullOrWhiteSpace(State) ||
                string.IsNullOrWhiteSpace(City) ||
                string.IsNullOrWhiteSpace(Address) ||
                string.IsNullOrWhiteSpace(Fax) ||
                string.IsNullOrWhiteSpace(IntPhone))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
