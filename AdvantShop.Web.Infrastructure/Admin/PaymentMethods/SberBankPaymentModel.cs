using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class SberBankPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string CompanyName
        {
            get { return Parameters.ElementOrDefault(SberBankTemplate.CompanyName); }
            set { Parameters.TryAddValue(SberBankTemplate.CompanyName, value.DefaultOrEmpty()); }
        }

        public string CorAccount
        {
            get { return Parameters.ElementOrDefault(SberBankTemplate.CorAccount); }
            set { Parameters.TryAddValue(SberBankTemplate.CorAccount, value.DefaultOrEmpty()); }
        }

        public string Inn
        {
            get { return Parameters.ElementOrDefault(SberBankTemplate.INN); }
            set { Parameters.TryAddValue(SberBankTemplate.INN, value.DefaultOrEmpty()); }
        }

        public string Kpp
        {
            get { return Parameters.ElementOrDefault(SberBankTemplate.KPP); }
            set { Parameters.TryAddValue(SberBankTemplate.KPP, value.DefaultOrEmpty()); }
        }

        public string BankName
        {
            get { return Parameters.ElementOrDefault(SberBankTemplate.BankName); }
            set { Parameters.TryAddValue(SberBankTemplate.BankName, value.DefaultOrEmpty()); }
        }

        public string Bik
        {
            get { return Parameters.ElementOrDefault(SberBankTemplate.BIK); }
            set { Parameters.TryAddValue(SberBankTemplate.BIK, value.DefaultOrEmpty()); }
        }

        public string TransAccount
        {
            get { return Parameters.ElementOrDefault(SberBankTemplate.TransAccount); }
            set { Parameters.TryAddValue(SberBankTemplate.TransAccount, value.DefaultOrEmpty()); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(CompanyName) ||
                string.IsNullOrWhiteSpace(CorAccount) ||
                string.IsNullOrWhiteSpace(Inn) ||
                string.IsNullOrWhiteSpace(BankName) ||
                string.IsNullOrWhiteSpace(Bik) ||
                string.IsNullOrWhiteSpace(TransAccount))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
