using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Shipping.PointDelivery;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class PointDeliveryShippingAdminModel : ShippingMethodAdminModel, IValidatableObject
    {
        /// <summary>
        /// items separated by ";". Example: address1;addr2;addr3;...
        /// </summary>
        public string Points
        {
            get { return Params.ElementOrDefault(PointDeliveryTemplate.Points); }
            set { Params.TryAddValue(PointDeliveryTemplate.Points, value.DefaultOrEmpty()); }
        }

        public string ShippingPrice
        {
            get { return Params.ElementOrDefault(PointDeliveryTemplate.ShippingPrice, "0"); }
            set { Params.TryAddValue(PointDeliveryTemplate.ShippingPrice, value.TryParseFloat().ToString()); }
        }

        public string DeliveryTime
        {
            get { return Params.ElementOrDefault(PointDeliveryTemplate.DeliveryTime); }
            set { Params.TryAddValue(PointDeliveryTemplate.DeliveryTime, value.DefaultOrEmpty()); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ShippingPrice))
                yield return new ValidationResult("Укажите стоимость доставки");
            
            if (!ShippingPrice.IsDecimal())
                yield return new ValidationResult("Стоимость доставки дожна быть числом");
        }
    }
}
