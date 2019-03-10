using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class RbkMoneyPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string EshopId
        {
            get { return Parameters.ElementOrDefault(RbkmoneyTemplate.EshopId); }
            set { Parameters.TryAddValue(RbkmoneyTemplate.EshopId, value.DefaultOrEmpty()); }
        }
        
        public string Preference
        {
            get { return Parameters.ElementOrDefault(RbkmoneyTemplate.Preference); }
            set { Parameters.TryAddValue(RbkmoneyTemplate.Preference, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> RbkPaymentTypes
        {
            get
            {
                var types = Rbkmoney.PaymentSystems.Select(x => new SelectListItem(){Text = x.Value,Value = x.Key}).ToList();

                var type = types.Find(x => x.Value == Preference);
                if (type != null)
                    type.Selected = true;
                else
                {
                    Preference = types[0].Value;
                    types[0].Selected = true;
                }

                return types;
            }
        }
        

        public override Tuple<string, string> Instruction
        {
            get { return new Tuple<string, string>("http://www.advantshop.net/help/pages/paymentrbkmoney", "Инструкция. Подключение платежного модуля \"RBK Money\""); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(EshopId))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
