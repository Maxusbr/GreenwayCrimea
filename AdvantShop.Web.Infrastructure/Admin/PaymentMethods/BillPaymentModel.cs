using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class BillPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string CompanyName
        {
            get { return Parameters.ElementOrDefault(BillTemplate.CompanyName); }
            set { Parameters.TryAddValue(BillTemplate.CompanyName, value.DefaultOrEmpty()); }
        }

        public string Accountant
        {
            get { return Parameters.ElementOrDefault(BillTemplate.Accountant); }
            set { Parameters.TryAddValue(BillTemplate.Accountant, value.DefaultOrEmpty()); }
        }

        public string PosAccountant
        {
            get { return Parameters.ElementOrDefault(BillTemplate.PosAccountant); }
            set { Parameters.TryAddValue(BillTemplate.PosAccountant, value.DefaultOrEmpty()); }
        }

        public string TransAccount
        {
            get { return Parameters.ElementOrDefault(BillTemplate.TransAccount); }
            set { Parameters.TryAddValue(BillTemplate.TransAccount, value.DefaultOrEmpty()); }
        }
        public string CorAccount
        {
            get { return Parameters.ElementOrDefault(BillTemplate.CorAccount); }
            set { Parameters.TryAddValue(BillTemplate.CorAccount, value.DefaultOrEmpty()); }
        }

        public string Address
        {
            get { return Parameters.ElementOrDefault(BillTemplate.Address); }
            set { Parameters.TryAddValue(BillTemplate.Address, value.DefaultOrEmpty()); }
        }

        public string Telephone
        {
            get { return Parameters.ElementOrDefault(BillTemplate.Telephone); }
            set { Parameters.TryAddValue(BillTemplate.Telephone, value.DefaultOrEmpty()); }
        }
        public string INN
        {
            get { return Parameters.ElementOrDefault(BillTemplate.INN); }
            set { Parameters.TryAddValue(BillTemplate.INN, value.DefaultOrEmpty()); }
        }

        public string KPP
        {
            get { return Parameters.ElementOrDefault(BillTemplate.KPP); }
            set { Parameters.TryAddValue(BillTemplate.KPP, value.DefaultOrEmpty()); }
        }

        public string BIK
        {
            get { return Parameters.ElementOrDefault(BillTemplate.BIK); }
            set { Parameters.TryAddValue(BillTemplate.BIK, value.DefaultOrEmpty()); }
        }

        public string BankName
        {
            get { return Parameters.ElementOrDefault(BillTemplate.BankName); }
            set { Parameters.TryAddValue(BillTemplate.BankName, value.DefaultOrEmpty()); }
        }

        public string Director
        {
            get { return Parameters.ElementOrDefault(BillTemplate.Director); }
            set { Parameters.TryAddValue(BillTemplate.Director, value.DefaultOrEmpty()); }
        }

        public string PosDirector
        {
            get { return Parameters.ElementOrDefault(BillTemplate.PosDirector); }
            set { Parameters.TryAddValue(BillTemplate.PosDirector, value.DefaultOrEmpty()); }
        }

        public string Manager
        {
            get { return Parameters.ElementOrDefault(BillTemplate.Manager); }
            set { Parameters.TryAddValue(BillTemplate.Manager, value.DefaultOrEmpty()); }
        }

        public string PosManager
        {
            get { return Parameters.ElementOrDefault(BillTemplate.PosManager); }
            set { Parameters.TryAddValue(BillTemplate.PosManager, value.DefaultOrEmpty()); }
        }

        public string StampImageName
        {
            get { return Parameters.ElementOrDefault(BillTemplate.StampImageName); }
            set { Parameters.TryAddValue(BillTemplate.StampImageName, value.DefaultOrEmpty()); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(CompanyName) ||
                string.IsNullOrWhiteSpace(Accountant) ||
                string.IsNullOrWhiteSpace(CorAccount) ||
                string.IsNullOrWhiteSpace(Address) ||
                string.IsNullOrWhiteSpace(Telephone) ||
                string.IsNullOrWhiteSpace(INN) ||
                //string.IsNullOrWhiteSpace(KPP) ||
                string.IsNullOrWhiteSpace(BIK) ||
                string.IsNullOrWhiteSpace(BankName) ||
                string.IsNullOrWhiteSpace(Director) ||
                string.IsNullOrWhiteSpace(Manager))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
