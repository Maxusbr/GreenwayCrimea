using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;
using AdvantShop.Shipping;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class PickPointPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string ShippingMethodTemplate
        {
            get { return Parameters.ElementOrDefault(PickPoint.ShippingMethodTemplate); }
            set { Parameters.TryAddValue(PickPoint.ShippingMethodTemplate, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> ShippingMethodTemplates
        {
            get
            {
                var keys = new PickPoint().ShippingKeys;
                var methods = new List<ShippingMethod>();

                foreach (var key in keys)
                {
                    methods.AddRange(ShippingMethodService.GetShippingMethodByType(key));
                }

                var shippingKeys = new List<SelectListItem>() {new SelectListItem() {Text = "Не выбрано", Value = ""}};

                foreach (var method in methods)
                {
                    shippingKeys.Add(new SelectListItem { Text = method.Name, Value = method.ShippingMethodId.ToString() });
                }
                
                var shippingKey = shippingKeys.Find(x => x.Value == ShippingMethodTemplate);
                if (shippingKey != null)
                    shippingKey.Selected = true;
                
                return shippingKeys;
            }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
