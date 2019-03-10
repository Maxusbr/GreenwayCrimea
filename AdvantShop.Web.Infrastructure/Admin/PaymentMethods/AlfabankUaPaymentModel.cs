using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class AlfabankUaPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string PartnerId
        {
            get { return Parameters.ElementOrDefault(AlfabankUaTemplate.PartnerId); }
            set { Parameters.TryAddValue(AlfabankUaTemplate.PartnerId, value.DefaultOrEmpty()); }
        }
        
        public string MinimumPrice
        {
            get { return Parameters.ElementOrDefault(AlfabankUaTemplate.MinimumPrice); }
            set { Parameters.TryAddValue(AlfabankUaTemplate.MinimumPrice, value.TryParseFloat().ToString()); }
        }

        public string FirstPayment
        {
            get { return Parameters.ElementOrDefault(AlfabankUaTemplate.FirstPayment); }
            set { Parameters.TryAddValue(AlfabankUaTemplate.FirstPayment, value.TryParseFloat().ToString()); }
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
