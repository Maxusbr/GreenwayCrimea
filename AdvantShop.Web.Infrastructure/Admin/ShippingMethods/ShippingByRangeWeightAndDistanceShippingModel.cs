using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Shipping;
using AdvantShop.Shipping.ShippingByRangeWeightAndDistance;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class ShippingByRangeWeightAndDistanceShippingAdminModel : ShippingMethodAdminModel, IValidatableObject
    {
        /// <summary>
        /// json string, example: [{"Amount":1.0,"PerUnit":false,"Price":100.0},{"Amount":15.2,"PerUnit":true,"Price":123.45}]
        /// </summary>
        public string WeightLimit
        {
            get { return Params.ElementOrDefault(ShippingByRangeWeightAndDistanceTemplate.WeightLimit); }
            set { Params.TryAddValue(ShippingByRangeWeightAndDistanceTemplate.WeightLimit, value.DefaultOrEmpty()); }
        }

        /// <summary>
        /// json string, example: [{"Amount":10.0,"PerUnit":false,"Price":100.0},{"Amount":20.2,"PerUnit":true,"Price":123.45}]
        /// </summary>
        public string DistanceLimit
        {
            get { return Params.ElementOrDefault(ShippingByRangeWeightAndDistanceTemplate.DistanceLimit); }
            set { Params.TryAddValue(ShippingByRangeWeightAndDistanceTemplate.DistanceLimit, value.DefaultOrEmpty()); }
        }

        public bool UseDistance
        {
            get { return Params.ElementOrDefault(ShippingByRangeWeightAndDistanceTemplate.UseDistance).TryParseBool(); }
            set { Params.TryAddValue(ShippingByRangeWeightAndDistanceTemplate.UseDistance, value.ToString()); }
        }

        public string MaxDistance
        {
            get { return Params.ElementOrDefault(ShippingByRangeWeightAndDistanceTemplate.MaxDistance, "50"); }
            set { Params.TryAddValue(ShippingByRangeWeightAndDistanceTemplate.MaxDistance, value.TryParseFloat().ToString()); }
        }

        public string DeliveryTime
        {
            get { return Params.ElementOrDefault(ShippingByRangeWeightAndDistanceTemplate.DeliveryTime); }
            set { Params.TryAddValue(ShippingByRangeWeightAndDistanceTemplate.DeliveryTime, value.DefaultOrEmpty()); }
        }

        public string DefaultWeight
        {
            get { return Params.ElementOrDefault(DefaultWeightParams.DefaultWeight, "1"); }
            set { Params.TryAddValue(DefaultWeightParams.DefaultWeight, value.TryParseFloat().ToString()); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //if (string.IsNullOrEmpty(DistanceLimit))
            //    yield return new ValidationResult("Укажите диапазон");

            return new List<ValidationResult>();
        }
    }
}
