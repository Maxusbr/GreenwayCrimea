using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class RsbCreditPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string PartnerId
        {
            get { return Parameters.ElementOrDefault(RsbCreditTemplate.PartnerId); }
            set { Parameters.TryAddValue(RsbCreditTemplate.PartnerId, value.DefaultOrEmpty()); }
        }
        
        public string MinimumPrice
        {
            get { return Parameters.ElementOrDefault(RsbCreditTemplate.MinimumPrice); }
            set { Parameters.TryAddValue(RsbCreditTemplate.MinimumPrice, value.TryParseFloat().ToString()); }
        }

        public string FirstPayment
        {
            get { return Parameters.ElementOrDefault(RsbCreditTemplate.FirstPayment); }
            set { Parameters.TryAddValue(RsbCreditTemplate.FirstPayment, value.TryParseFloat().ToString()); }
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
