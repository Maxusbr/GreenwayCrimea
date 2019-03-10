using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class AssistPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string Login
        {
            get { return Parameters.ElementOrDefault(AssistTemplate.Login); }
            set { Parameters.TryAddValue(AssistTemplate.Login, value.DefaultOrEmpty()); }
        }

        public string Password
        {
            get { return Parameters.ElementOrDefault(AssistTemplate.Password); }
            set { Parameters.TryAddValue(AssistTemplate.Password, value.DefaultOrEmpty()); }
        }

        public string MerchantId
        {
            get { return Parameters.ElementOrDefault(AssistTemplate.MerchantID); }
            set { Parameters.TryAddValue(AssistTemplate.MerchantID, value.DefaultOrEmpty()); }
        }

        public string UrlWorkingMode
        {
            get { return Parameters.ElementOrDefault(AssistTemplate.UrlWorkingMode); }
            set { Parameters.TryAddValue(AssistTemplate.UrlWorkingMode, value.DefaultOrEmpty()); }
        }

        public bool Sandbox
        {
            get { return Parameters.ElementOrDefault(AssistTemplate.Sandbox).TryParseBool(); }
            set { Parameters.TryAddValue(AssistTemplate.Sandbox, value.ToString()); }
        }

        public bool Delay
        {
            get { return Parameters.ElementOrDefault(AssistTemplate.Delay).TryParseBool(); }
            set { Parameters.TryAddValue(AssistTemplate.Delay, value.ToString()); }
        }

        public string CurrencyCode
        {
            get { return Parameters.ElementOrDefault(AssistTemplate.CurrencyCode); }
            set { Parameters.TryAddValue(AssistTemplate.CurrencyCode, value.DefaultOrEmpty()); }
        }

        public string CurrencyValue
        {
            get { return Parameters.ElementOrDefault(AssistTemplate.CurrencyValue); }
            set { Parameters.TryAddValue(AssistTemplate.CurrencyValue, value.DefaultOrEmpty()); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Login) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(MerchantId) ||
                string.IsNullOrWhiteSpace(UrlWorkingMode) ||
                string.IsNullOrWhiteSpace(CurrencyCode) ||
                string.IsNullOrWhiteSpace(CurrencyValue))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
