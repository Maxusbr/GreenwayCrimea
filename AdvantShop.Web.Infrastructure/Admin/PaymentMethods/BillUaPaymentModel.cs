using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class BillUaPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string CompanyName
        {
            get { return Parameters.ElementOrDefault(BillUaTemplate.CompanyName); }
            set { Parameters.TryAddValue(BillUaTemplate.CompanyName, value.DefaultOrEmpty()); }
        }

        public string CompanyCode
        {
            get { return Parameters.ElementOrDefault(BillUaTemplate.CompanyCode); }
            set { Parameters.TryAddValue(BillUaTemplate.CompanyCode, value.DefaultOrEmpty()); }
        }

        public string BankName
        {
            get { return Parameters.ElementOrDefault(BillUaTemplate.BankName); }
            set { Parameters.TryAddValue(BillUaTemplate.BankName, value.DefaultOrEmpty()); }
        }

        public string BankCode
        {
            get { return Parameters.ElementOrDefault(BillUaTemplate.BankCode); }
            set { Parameters.TryAddValue(BillUaTemplate.BankCode, value.DefaultOrEmpty()); }
        }

        public string Credit
        {
            get { return Parameters.ElementOrDefault(BillUaTemplate.Credit); }
            set { Parameters.TryAddValue(BillUaTemplate.Credit, value.DefaultOrEmpty()); }
        }

        public string CompanyEssentials
        {
            get { return Parameters.ElementOrDefault(BillUaTemplate.CompanyEssentials); }
            set { Parameters.TryAddValue(BillUaTemplate.CompanyEssentials, value.DefaultOrEmpty()); }
        }
        

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(CompanyName) ||
                string.IsNullOrWhiteSpace(CompanyCode) ||
                string.IsNullOrWhiteSpace(BankName) ||
                string.IsNullOrWhiteSpace(BankCode) ||
                string.IsNullOrWhiteSpace(BankName) ||
                string.IsNullOrWhiteSpace(Credit) ||
                string.IsNullOrWhiteSpace(CompanyEssentials))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
