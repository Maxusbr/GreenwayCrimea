using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Mails;

namespace AdvantShop.Web.Admin.Models.SettingsMail
{
    public class MailFormatModel : MailFormat, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(FormatName) ||
                string.IsNullOrWhiteSpace(FormatSubject) ||
                string.IsNullOrWhiteSpace(FormatText))
            {
                yield return new ValidationResult("Введите обязательные поля");
            }
        }
    }
}
