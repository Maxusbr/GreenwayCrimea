using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdvantShop.Web.Admin.Models.Cards
{
    public class SubctractAdditionalBonusModel : IValidatableObject
    {
        public Guid CardId { get; set; }
        public int AdditionId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Amount < 0)
                yield return new ValidationResult("Не может быть отрицательным");
        }
    }
}