using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Shipping.NovaPoshta;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class NovaPoshtaShippingAdminModel : ShippingMethodAdminModel, IValidatableObject
    {
        public string ApiKey
        {
            get { return Params.ElementOrDefault(NovaPoshtaTemplate.APIKey); }
            set { Params.TryAddValue(NovaPoshtaTemplate.APIKey, value.DefaultOrEmpty()); }
        }

        public string CityFrom
        {
            get { return Params.ElementOrDefault(NovaPoshtaTemplate.CityFrom); }
            set { Params.TryAddValue(NovaPoshtaTemplate.CityFrom, value.DefaultOrEmpty()); }
        }

        public string DeliveryType
        {
            get { return Params.ElementOrDefault(NovaPoshtaTemplate.DeliveryType); }
            set { Params.TryAddValue(NovaPoshtaTemplate.DeliveryType, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> DeliveryTypes
        {
            get
            {
                var deliveryTypes =
                (from enNovaPoshtaDeliveryType item in Enum.GetValues(typeof(enNovaPoshtaDeliveryType))
                    select new SelectListItem() {Text = item.Localize(), Value = ((int) item).ToString()}).ToList();

                var deliveryType = deliveryTypes.Find(x => x.Value == DeliveryType);
                if (deliveryType != null)
                {
                    deliveryType.Selected = true;
                }
                else
                {
                    deliveryTypes[0].Selected = true;
                    DeliveryType = deliveryTypes[0].Value;
                }

                return deliveryTypes;
            }
        }

        public string Rate
        {
            get { return Params.ElementOrDefault(NovaPoshtaTemplate.Rate, "1"); }
            set { Params.TryAddValue(NovaPoshtaTemplate.Rate, value.TryParseFloat().ToString()); }
        }

        public string DefaultWeight
        {
            get { return Params.ElementOrDefault(NovaPoshtaTemplate.DefaultWeight, "1"); }
            set { Params.TryAddValue(NovaPoshtaTemplate.DefaultWeight, value.TryParseFloat().ToString()); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ApiKey) || string.IsNullOrWhiteSpace(CityFrom))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
