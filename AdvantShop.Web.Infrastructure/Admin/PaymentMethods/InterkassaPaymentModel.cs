using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class InterkassaPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string ShopId
        {
            get { return Parameters.ElementOrDefault(InterkassaTemplate.ShopId); }
            set { Parameters.TryAddValue(InterkassaTemplate.ShopId, value.DefaultOrEmpty()); }
        }
        
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ShopId))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
