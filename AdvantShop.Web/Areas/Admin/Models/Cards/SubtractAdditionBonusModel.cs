using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Web.Admin.Models.Cards
{
    public class SubtractAdditionBonusModel : IValidatableObject
    {
        public int BonusId { get; set; }
        [Range(typeof(decimal), "0", "100000")]
        public decimal Amount { get; set; }
        public string Basis { get; set; }
        public Guid CardId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
