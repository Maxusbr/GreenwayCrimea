using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class MailRuPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string Key
        {
            get { return Parameters.ElementOrDefault(MailRuTemplate.Key); }
            set { Parameters.TryAddValue(MailRuTemplate.Key, value.DefaultOrEmpty()); }
        }

        public string ShopId
        {
            get { return Parameters.ElementOrDefault(MailRuTemplate.ShopID); }
            set { Parameters.TryAddValue(MailRuTemplate.ShopID, value.DefaultOrEmpty()); }
        }

        public string CurrencyCode
        {
            get { return Parameters.ElementOrDefault(MailRuTemplate.CurrencyCode); }
            set { Parameters.TryAddValue(MailRuTemplate.CurrencyCode, value.DefaultOrEmpty()); }
        }

        public bool KeepUnique
        {
            get { return Parameters.ElementOrDefault(MailRuTemplate.KeepUnique).TryParseBool(); }
            set { Parameters.TryAddValue(MailRuTemplate.KeepUnique, value.ToString()); }
        }

        public string CryptoHex
        {
            get { return Parameters.ElementOrDefault(MailRuTemplate.CryptoHex); }
            set { Parameters.TryAddValue(MailRuTemplate.CryptoHex, value.DefaultOrEmpty()); }
        }
        

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ShopId) ||
                string.IsNullOrWhiteSpace(Key) ||
                string.IsNullOrWhiteSpace(CurrencyCode) ||
                string.IsNullOrWhiteSpace(CryptoHex))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
