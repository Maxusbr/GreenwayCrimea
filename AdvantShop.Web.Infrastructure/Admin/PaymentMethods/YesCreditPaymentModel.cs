using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class YesCreditPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string MerchantId
        {
            get { return Parameters.ElementOrDefault(YesCreditTemplate.MerchantId); }
            set { Parameters.TryAddValue(YesCreditTemplate.MerchantId, value.DefaultOrEmpty()); }
        }
        
        public string MinimumPrice
        {
            get { return Parameters.ElementOrDefault(YesCreditTemplate.MinimumPrice); }
            set { Parameters.TryAddValue(YesCreditTemplate.MinimumPrice, value.TryParseFloat().ToString()); }
        }

        public string FirstPayment
        {
            get { return Parameters.ElementOrDefault(YesCreditTemplate.FirstPayment); }
            set { Parameters.TryAddValue(YesCreditTemplate.FirstPayment, value.TryParseFloat().ToString()); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MerchantId))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
