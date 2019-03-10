using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AdvantShop.Web.Admin.Models.Grastin
{
    public class SendRequestForAct : IValidatableObject
    {
        public int OrderId { get; set; }
        public string ContractId { get; set; }
        public List<SelectListItem> Contracts { get; set; }
        public int Seats { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ContractId))
                yield return new ValidationResult("Укажите договор");
        }
    }
}
