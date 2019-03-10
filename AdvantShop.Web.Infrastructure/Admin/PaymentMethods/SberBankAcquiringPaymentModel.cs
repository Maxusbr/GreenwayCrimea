using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class SberBankAcquiringPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string UserName
        {
            get { return Parameters.ElementOrDefault(SberBankAcquiringTemplate.UserName); }
            set { Parameters.TryAddValue(SberBankAcquiringTemplate.UserName, value.DefaultOrEmpty()); }
        }

        public string Password
        {
            get { return Parameters.ElementOrDefault(SberBankAcquiringTemplate.Password); }
            set { Parameters.TryAddValue(SberBankAcquiringTemplate.Password, value.DefaultOrEmpty()); }
        }

        public bool TestMode
        {
            get { return Parameters.ElementOrDefault(SberBankAcquiringTemplate.TestMode).TryParseBool(); }
            set { Parameters.TryAddValue(SberBankAcquiringTemplate.TestMode, value.ToString()); }
        }

        public string MerchantLogin
        {
            get { return Parameters.ElementOrDefault(SberBankAcquiringTemplate.MerchantLogin); }
            set { Parameters.TryAddValue(SberBankAcquiringTemplate.MerchantLogin, value.DefaultOrEmpty()); }
        }
        
        public bool SendReceiptData
        {
            get { return Parameters.ElementOrDefault(SberBankAcquiringTemplate.SendReceiptData).TryParseBool(); }
            set { Parameters.TryAddValue(SberBankAcquiringTemplate.SendReceiptData, value.ToString()); }
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
