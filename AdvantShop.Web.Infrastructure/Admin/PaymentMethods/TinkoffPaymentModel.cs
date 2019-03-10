using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class TinkoffPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string TerminalKey
        {
            get { return Parameters.ElementOrDefault(TinkoffTemplate.TerminalKey); }
            set { Parameters.TryAddValue(TinkoffTemplate.TerminalKey, value.DefaultOrEmpty()); }
        }

        public string SecretKey
        {
            get { return Parameters.ElementOrDefault(TinkoffTemplate.SecretKey); }
            set { Parameters.TryAddValue(TinkoffTemplate.SecretKey, value.DefaultOrEmpty()); }
        }
        
        public bool SendReceiptData
        {
            get { return Parameters.ElementOrDefault(TinkoffTemplate.SendReceiptData).TryParseBool(); }
            set { Parameters.TryAddValue(TinkoffTemplate.SendReceiptData, value.ToString()); }
        }

        public string Taxation
        {
            get { return Parameters.ElementOrDefault(TinkoffTemplate.Taxation); }
            set { Parameters.TryAddValue(TinkoffTemplate.Taxation, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> Taxations
        {
            get
            {
                var types = new List<SelectListItem>()
                {
                    new SelectListItem() {Text = "Общая", Value = "osn"},
                    new SelectListItem() {Text = "Упрощенная (доходы)", Value = "usn_income"},
                    new SelectListItem() {Text = "Упрощенная (доходы минус расходы)", Value = "usn_income_outcome"},
                    new SelectListItem() {Text = "Единый налог на вмененный доход", Value = "envd"},
                    new SelectListItem() {Text = "Единый сельскохозяйственный налог", Value = "esn"},
                    new SelectListItem() {Text = "Патентная", Value = "patent"},
                };

                var type = types.Find(x => x.Value == Taxation);
                if (type != null)
                    type.Selected = true;

                return types;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(TerminalKey) || string.IsNullOrWhiteSpace(SecretKey))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
