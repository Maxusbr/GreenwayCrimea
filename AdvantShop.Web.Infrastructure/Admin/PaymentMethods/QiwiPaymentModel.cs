using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class QiwiPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string ProviderId
        {
            get { return Parameters.ElementOrDefault(QiwiTemplate.ProviderID); }
            set { Parameters.TryAddValue(QiwiTemplate.ProviderID, value.DefaultOrEmpty()); }
        }

        public string RestId
        {
            get { return Parameters.ElementOrDefault(QiwiTemplate.RestID); }
            set { Parameters.TryAddValue(QiwiTemplate.RestID, value.DefaultOrEmpty()); }
        }

        public string ProviderName
        {
            get { return Parameters.ElementOrDefault(QiwiTemplate.ProviderName); }
            set { Parameters.TryAddValue(QiwiTemplate.ProviderName, value.DefaultOrEmpty()); }
        }
        
        public string Password
        {
            get { return Parameters.ElementOrDefault(QiwiTemplate.Password); }
            set { Parameters.TryAddValue(QiwiTemplate.Password, value.DefaultOrEmpty()); }
        }

        public string PasswordNotify
        {
            get { return Parameters.ElementOrDefault(QiwiTemplate.PasswordNotify); }
            set { Parameters.TryAddValue(QiwiTemplate.PasswordNotify, value.DefaultOrEmpty()); }
        }

        public string CurrencyCode
        {
            get { return Parameters.ElementOrDefault(QiwiTemplate.CurrencyCode, "RUB") ?? "RUB"; }
            set { Parameters.TryAddValue(QiwiTemplate.CurrencyCode, value.DefaultOrEmpty()); }
        }

        public string CurrencyValue
        {
            get { return Parameters.ElementOrDefault(QiwiTemplate.CurrencyValue, "1") ?? "1"; }
            set { Parameters.TryAddValue(QiwiTemplate.CurrencyValue, value.DefaultOrEmpty()); }
        }
        
        public override Tuple<string, string> Instruction
        {
            get { return new Tuple<string, string>("http://www.advantshop.net/help/pages/connect-qiwi", "Инструкция. Подключение платежного модуля Qiwi"); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ProviderId) ||
                string.IsNullOrWhiteSpace(RestId) ||
                string.IsNullOrWhiteSpace(ProviderName) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(PasswordNotify) ||
                string.IsNullOrWhiteSpace(CurrencyCode) ||
                string.IsNullOrWhiteSpace(CurrencyValue))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
