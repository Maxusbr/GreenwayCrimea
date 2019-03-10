using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Helpers;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class SendTestMessageModel : IValidatableObject
    {
        public string SMTP { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PasswordCompare { get; set; }
        public string From { get; set; }
        public string SenderName { get; set; }
        public bool SSL { get; set; }
        public int Port { get; set; }

        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(SMTP) ||
                string.IsNullOrWhiteSpace(Login) ||
                Port == 0 ||
                string.IsNullOrWhiteSpace(Subject) ||
                string.IsNullOrWhiteSpace(Body))
            {
                yield return new ValidationResult("Заполните все настройки почты");
            }

            if (string.IsNullOrWhiteSpace(To) || !ValidationHelper.IsValidEmail(To))
                yield return new ValidationResult("Не валидный e-mail получателя");
        }
    }
}


