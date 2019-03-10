using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdvantShop.Web.Admin.Models.Cards
{
    public class AddAdditionalBonusModel : IValidatableObject
    {
        public Guid CardId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Amount< 0 )
                yield return new ValidationResult("Не может быть отрицательным");

            if (StartDate != null && EndDate != null && (StartDate > EndDate))
            {
                yield return new ValidationResult("Дата начала больше даты окончания");
            }
            if (EndDate != null && EndDate < DateTime.Today)
            {
                yield return new ValidationResult("Срок действия бонуса истек");
            }
        }
    }
}
