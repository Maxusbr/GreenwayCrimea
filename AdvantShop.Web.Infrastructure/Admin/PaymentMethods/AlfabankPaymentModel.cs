using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class AlfabankPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string UserName
        {
            get { return Parameters.ElementOrDefault(AlfabankTemplate.UserName); }
            set { Parameters.TryAddValue(AlfabankTemplate.UserName, value.DefaultOrEmpty()); }
        }

        public string Password
        {
            get { return Parameters.ElementOrDefault(AlfabankTemplate.Password); }
            set { Parameters.TryAddValue(AlfabankTemplate.Password, value.DefaultOrEmpty()); }
        }

        public string MerchantLogin
        {
            get { return Parameters.ElementOrDefault(AlfabankTemplate.MerchantLogin); }
            set { Parameters.TryAddValue(AlfabankTemplate.MerchantLogin, value.DefaultOrEmpty()); }
        }

        public bool UseTestMode
        {
            get { return Parameters.ElementOrDefault(AlfabankTemplate.UseTestMode).TryParseBool(true) ?? true; }
            set { Parameters.TryAddValue(AlfabankTemplate.UseTestMode, value.ToString().DefaultOrEmpty()); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
