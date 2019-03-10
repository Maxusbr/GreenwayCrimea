using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class ZPaymentPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string Purse
        {
            get { return Parameters.ElementOrDefault(ZPaymentTemplate.Purse); }
            set { Parameters.TryAddValue(ZPaymentTemplate.Purse, value.DefaultOrEmpty()); }
        }
        
        public string SecretKey
        {
            get { return Parameters.ElementOrDefault(ZPaymentTemplate.SecretKey); }
            set { Parameters.TryAddValue(ZPaymentTemplate.SecretKey, value.DefaultOrEmpty()); }
        }

        public string Password
        {
            get { return Parameters.ElementOrDefault(ZPaymentTemplate.Password); }
            set { Parameters.TryAddValue(ZPaymentTemplate.Password, value.DefaultOrEmpty()); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Purse) ||
                string.IsNullOrWhiteSpace(SecretKey) ||
                string.IsNullOrWhiteSpace(Password))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
