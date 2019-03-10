using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class WebMoneyPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string Purse
        {
            get { return Parameters.ElementOrDefault(WebMoneyTemplate.Purse); }
            set { Parameters.TryAddValue(WebMoneyTemplate.Purse, value.DefaultOrEmpty()); }
        }
        
        public string SecretKey
        {
            get { return Parameters.ElementOrDefault(WebMoneyTemplate.SecretKey); }
            set { Parameters.TryAddValue(WebMoneyTemplate.SecretKey, value.DefaultOrEmpty()); }
        }
        

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Purse) ||
                string.IsNullOrWhiteSpace(SecretKey))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
