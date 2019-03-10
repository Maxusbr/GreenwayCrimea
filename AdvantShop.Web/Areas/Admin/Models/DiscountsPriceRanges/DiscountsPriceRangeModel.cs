using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.DiscountsPriceRanges
{
    public class DiscountsPriceRangeFilterModel : BaseFilterModel
    {
        public float? PriceRange { get; set; }
        public double? PercentDiscount { get; set; }
    }

    public class DiscountsPriceRangeModel : IValidatableObject
    {
        public int OrderPriceDiscountId { get; set; }
        public float PriceRange { get; set; }
        public double PercentDiscount { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PercentDiscount < 0 || PercentDiscount > 100)
                yield return new ValidationResult("Скидка от 0 до 100 %");

            if (PriceRange < 0)
                yield return new ValidationResult("Сумма заказа должна быть больше 0");
        }
    }
}
