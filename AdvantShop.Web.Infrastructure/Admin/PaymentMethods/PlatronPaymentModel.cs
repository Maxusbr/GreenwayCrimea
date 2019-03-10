using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class PlatronPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string MerchantId
        {
            get { return Parameters.ElementOrDefault(PlatronTemplate.MerchantId); }
            set { Parameters.TryAddValue(PlatronTemplate.MerchantId, value.DefaultOrEmpty()); }
        }

        public string PaymentSystem
        {
            get { return Parameters.ElementOrDefault(PlatronTemplate.PaymentSystem); }
            set { Parameters.TryAddValue(PlatronTemplate.PaymentSystem, value.DefaultOrEmpty()); }
        }

        public string SecretKey
        {
            get { return Parameters.ElementOrDefault(PlatronTemplate.SecretKey); }
            set { Parameters.TryAddValue(PlatronTemplate.SecretKey, value.DefaultOrEmpty()); }
        }

        public string Currency
        {
            get { return Parameters.ElementOrDefault(PlatronTemplate.Currency); }
            set { Parameters.TryAddValue(PlatronTemplate.Currency, value.DefaultOrEmpty()); }
        }

        public string CurrencyValue
        {
            get { return Parameters.ElementOrDefault(PlatronTemplate.CurrencyValue); }
            set { Parameters.TryAddValue(PlatronTemplate.CurrencyValue, value.TryParseFloat().ToString()); }
        }

        public bool SendReceiptData
        {
            get { return Parameters.ElementOrDefault(PlatronTemplate.SendReceiptData).TryParseBool(); }
            set { Parameters.TryAddValue(PlatronTemplate.SendReceiptData, value.ToString()); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MerchantId) ||
                string.IsNullOrWhiteSpace(PaymentSystem) ||
                string.IsNullOrWhiteSpace(SecretKey) ||
                string.IsNullOrWhiteSpace(Currency))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
