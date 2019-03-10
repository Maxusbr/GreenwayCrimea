using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AdvantShop.Web.Admin.Models.Grastin
{
    public class SendRequestForIntake : IValidatableObject
    {
        public int OrderId { get; set; }
        public string RegionId { get; set; }
        public List<SelectListItem> Regions { get; set; }
        public string Time { get; set; }
        public List<SelectListItem> Times { get; set; }
        public string Volume { get; set; }
        public List<SelectListItem> Volumes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(RegionId))
                yield return new ValidationResult("Укажите регион забора товара");

            if (string.IsNullOrWhiteSpace(Time))
                yield return new ValidationResult("Укажите время забора");

            if (string.IsNullOrWhiteSpace(Volume))
                yield return new ValidationResult("Укажите объем заказов");
        }
    }
}
