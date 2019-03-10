using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class Interkassa2PaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string ShopId
        {
            get { return Parameters.ElementOrDefault(Interkassa2Template.ShopId); }
            set { Parameters.TryAddValue(Interkassa2Template.ShopId, value.DefaultOrEmpty()); }
        }

        public bool IsCheckSign
        {
            get { return Parameters.ElementOrDefault(Interkassa2Template.IsCheckSign).TryParseBool(); }
            set { Parameters.TryAddValue(Interkassa2Template.IsCheckSign, value.ToString()); }
        }

        public string SecretKey
        {
            get { return Parameters.ElementOrDefault(Interkassa2Template.SecretKey); }
            set { Parameters.TryAddValue(Interkassa2Template.SecretKey, value.DefaultOrEmpty()); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ShopId) ||
                string.IsNullOrWhiteSpace(SecretKey))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
