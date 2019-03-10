using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class PayAnyWayPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string MerchantId
        {
            get { return Parameters.ElementOrDefault(PayAnyWayTemplate.MerchantId); }
            set { Parameters.TryAddValue(PayAnyWayTemplate.MerchantId, value.DefaultOrEmpty()); }
        }

        public string Signature
        {
            get { return Parameters.ElementOrDefault(PayAnyWayTemplate.Signature); }
            set { Parameters.TryAddValue(PayAnyWayTemplate.Signature, value.DefaultOrEmpty()); }
        }

        public string CurrencyLabel
        {
            get { return Parameters.ElementOrDefault(PayAnyWayTemplate.CurrencyLabel, "RUB"); }
            set { Parameters.TryAddValue(PayAnyWayTemplate.CurrencyLabel, value.DefaultOrEmpty()); }
        }

        public string CurrencyValue
        {
            get { return Parameters.ElementOrDefault(PayAnyWayTemplate.CurrencyValue, "1"); }
            set { Parameters.TryAddValue(PayAnyWayTemplate.CurrencyValue, value.TryParseFloat().ToString()); }
        }

        public bool TestMode
        {
            get { return Parameters.ElementOrDefault(PayAnyWayTemplate.TestMode).TryParseBool(); }
            set { Parameters.TryAddValue(PayAnyWayTemplate.TestMode, value.ToString()); }
        }

        public string UnitId
        {
            get { return Parameters.ElementOrDefault(PayAnyWayTemplate.UnitId); }
            set { Parameters.TryAddValue(PayAnyWayTemplate.UnitId, value.DefaultOrEmpty()); }
        }

        public string LimitIds
        {
            get { return Parameters.ElementOrDefault(PayAnyWayTemplate.LimitIds); }
            set { Parameters.TryAddValue(PayAnyWayTemplate.LimitIds, value.DefaultOrEmpty()); }
        }

        public bool UseKassa
        {
            get { return Parameters.ElementOrDefault(PayAnyWayTemplate.UseKassa).TryParseBool(); }
            set { Parameters.TryAddValue(PayAnyWayTemplate.UseKassa, value.ToString()); }
        }

        public override Tuple<string, string> Instruction
        {
            get { return new Tuple<string, string>("http://www.advantshop.net/help/pages/connect-payanyway", "Инструкция. Подключение платежного модуля \"PayAnyWay (Moneta.ru)\""); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MerchantId) ||
                string.IsNullOrWhiteSpace(Signature) ||
                string.IsNullOrWhiteSpace(CurrencyLabel))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
