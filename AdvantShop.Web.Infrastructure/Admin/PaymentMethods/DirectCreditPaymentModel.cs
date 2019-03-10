using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class DirectCreditPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string PartnerId
        {
            get { return Parameters.ElementOrDefault(DirectCreditTemplate.PartnerId); }
            set { Parameters.TryAddValue(DirectCreditTemplate.PartnerId, value.DefaultOrEmpty()); }
        }
        
        public string MinimumPrice
        {
            get { return Parameters.ElementOrDefault(DirectCreditTemplate.MinimumPrice); }
            set { Parameters.TryAddValue(DirectCreditTemplate.MinimumPrice, value.TryParseFloat().ToString()); }
        }

        public string FirstPayment
        {
            get { return Parameters.ElementOrDefault(DirectCreditTemplate.FirstPayment); }
            set { Parameters.TryAddValue(DirectCreditTemplate.FirstPayment, value.TryParseFloat().ToString()); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(PartnerId))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
