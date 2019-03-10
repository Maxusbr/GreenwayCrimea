using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class PaymasterPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string MerchantId
        {
            get { return Parameters.ElementOrDefault(PaymasterTemplate.MerchantId); }
            set { Parameters.TryAddValue(PaymasterTemplate.MerchantId, value.DefaultOrEmpty()); }
        }

        public string SecretWord
        {
            get { return Parameters.ElementOrDefault(PaymasterTemplate.SecretWord); }
            set { Parameters.TryAddValue(PaymasterTemplate.SecretWord, value.DefaultOrEmpty()); }
        }



        public override Tuple<string, string> Instruction
        {
            get { return new Tuple<string, string>("http://www.advantshop.net/help/pages/connect-paymaster", "Инструкция. Подключение к сервису PayMaster."); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MerchantId) ||
                string.IsNullOrWhiteSpace(SecretWord))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
