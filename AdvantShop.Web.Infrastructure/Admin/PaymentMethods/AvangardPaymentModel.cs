using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class AvangardPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string ShopId
        {
            get { return Parameters.ElementOrDefault(AvangardTemplate.ShopId); }
            set { Parameters.TryAddValue(AvangardTemplate.ShopId, value.DefaultOrEmpty()); }
        }
        
        public string ShopPassword
        {
            get { return Parameters.ElementOrDefault(AvangardTemplate.ShopPassword); }
            set { Parameters.TryAddValue(AvangardTemplate.ShopPassword, value.DefaultOrEmpty()); }
        }

        public string AvSign
        {
            get { return Parameters.ElementOrDefault(AvangardTemplate.AvSign); }
            set { Parameters.TryAddValue(AvangardTemplate.AvSign, value.DefaultOrEmpty()); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ShopId) ||
                string.IsNullOrWhiteSpace(ShopPassword) ||
                string.IsNullOrWhiteSpace(AvSign))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
