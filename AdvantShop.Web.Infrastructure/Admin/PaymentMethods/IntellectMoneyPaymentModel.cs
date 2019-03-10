using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class IntellectMoneyPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string MerchantId
        {
            get { return Parameters.ElementOrDefault(IntellectMoneyTemplate.MerchantId); }
            set { Parameters.TryAddValue(IntellectMoneyTemplate.MerchantId, value.DefaultOrEmpty()); }
        }

        public string SecretKey
        {
            get { return Parameters.ElementOrDefault(IntellectMoneyTemplate.SecretKey); }
            set { Parameters.TryAddValue(IntellectMoneyTemplate.SecretKey, value.DefaultOrEmpty()); }
        }

        public override Tuple<string, string> Instruction
        {
            get
            {
                return new Tuple<string, string>("http://www.advantshop.net/help/pages/connect-intellectmoney", "Инструкция. Подключение к системе IntellectMoney");
            }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MerchantId) ||
                string.IsNullOrWhiteSpace(SecretKey))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
