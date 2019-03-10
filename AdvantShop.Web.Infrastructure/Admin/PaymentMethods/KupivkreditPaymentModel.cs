using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class KupivkreditPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string ShopId
        {
            get { return Parameters.ElementOrDefault(KupivkreditTemplate.ShopId); }
            set { Parameters.TryAddValue(KupivkreditTemplate.ShopId, value.DefaultOrEmpty()); }
        }

        public bool UseTest
        {
            get { return Parameters.ElementOrDefault(KupivkreditTemplate.UseTest).TryParseBool(true) ?? true; }
            set { Parameters.TryAddValue(KupivkreditTemplate.UseTest, value.ToString()); }
        }

        public string ShowCaseId
        {
            get { return Parameters.ElementOrDefault(KupivkreditTemplate.ShowCaseId); }
            set { Parameters.TryAddValue(KupivkreditTemplate.ShowCaseId, value.DefaultOrEmpty()); }
        }

        public string PromoCode
        {
            get { return Parameters.ElementOrDefault(KupivkreditTemplate.PromoCode); }
            set { Parameters.TryAddValue(KupivkreditTemplate.PromoCode, value.DefaultOrEmpty()); }
        }

        public string MinimumPrice
        {
            get { return Parameters.ElementOrDefault(KupivkreditTemplate.MinimumPrice) ?? "3000"; }
            set { Parameters.TryAddValue(KupivkreditTemplate.MinimumPrice, value.TryParseFloat().ToString()); }
        }

        public string FirstPayment
        {
            get { return Parameters.ElementOrDefault(KupivkreditTemplate.FirstPayment) ?? "10"; }
            set { Parameters.TryAddValue(KupivkreditTemplate.FirstPayment, value.TryParseFloat().ToString()); }
        }


        public override Tuple<string, string> Instruction
        {
            get { return new Tuple<string, string>("http://www.advantshop.net/help/pages/buy-in-credit", "Инструкция. Подключение платежного модуля \"Купи в кредит\""); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ShopId) ||
                string.IsNullOrWhiteSpace(ShowCaseId))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
