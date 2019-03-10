using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Web.Admin.Models.Cards
{
    public class CardModel : IValidatableObject
    {
        public bool IsEditMode { get; set; }

        public Guid CardId { get; set; }
        public string Name { get; set; }

        public long CardNumber { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal AdditionBonusAmount { get; set; }

        public bool Blocked { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime? DateLastWipeBonus { get; set; }
        public int GradeId { get; set; }

        public List<Grade> Grades { get; set; }
        public bool DisabledChangeGrade { get; set; }

        public bool ManualGrade { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CardId == Guid.Empty)
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Cards.CardModel.Error.CardId"));

            if (GradeId == 0)
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Cards.CardModel.Error.GradeId"));

            if (IsEditMode) yield break;
        }
    }
}
