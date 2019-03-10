using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class RobokassaPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string MerchantLogin
        {
            get { return Parameters.ElementOrDefault(RobokassaTemplate.MerchantLogin); }
            set { Parameters.TryAddValue(RobokassaTemplate.MerchantLogin, value.DefaultOrEmpty()); }
        }

        public string Password
        {
            get { return Parameters.ElementOrDefault(RobokassaTemplate.Password); }
            set { Parameters.TryAddValue(RobokassaTemplate.Password, value.DefaultOrEmpty()); }
        }

        public string PasswordNotify
        {
            get { return Parameters.ElementOrDefault(RobokassaTemplate.PasswordNotify); }
            set { Parameters.TryAddValue(RobokassaTemplate.PasswordNotify, value.DefaultOrEmpty()); }
        }

        public bool SendReceiptData
        {
            get { return Parameters.ElementOrDefault(RobokassaTemplate.SendReceiptData).TryParseBool(); }
            set { Parameters.TryAddValue(RobokassaTemplate.SendReceiptData, value.ToString()); }
        }

        public bool IsTest
        {
            get { return Parameters.ElementOrDefault(RobokassaTemplate.IsTest).TryParseBool(); }
            set { Parameters.TryAddValue(RobokassaTemplate.IsTest, value.ToString()); }
        }
                

        public override Tuple<string, string> Instruction
        {
            get { return new Tuple<string, string>("http://www.advantshop.net/help/pages/connect-robokassa", "Инструкция. Подключение к системе Robokassa"); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MerchantLogin) ||
                string.IsNullOrWhiteSpace(PasswordNotify) ||
                string.IsNullOrWhiteSpace(Password))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
