using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class NetPayPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string ApiKey
        {
            get { return Parameters.ElementOrDefault(NetPayTemplate.ApiKey); }
            set { Parameters.TryAddValue(NetPayTemplate.ApiKey, value.DefaultOrEmpty()); }
        }

        public string AuthSign
        {
            get { return Parameters.ElementOrDefault(NetPayTemplate.AuthSign); }
            set { Parameters.TryAddValue(NetPayTemplate.AuthSign, value.DefaultOrEmpty()); }
        }

        public bool TestMode
        {
            get { return Parameters.ElementOrDefault(NetPayTemplate.TestMode).TryParseBool(true) ?? true; }
            set { Parameters.TryAddValue(NetPayTemplate.TestMode, value.ToString()); }
        }

        public override Tuple<string, string> Instruction
        {
            get { return new Tuple<string, string>("http://www.advantshop.net/help/pages/connect-onpay", "Инструкция. Подключение платежного модуля OnPay.ru"); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ApiKey) ||
                string.IsNullOrWhiteSpace(AuthSign))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
