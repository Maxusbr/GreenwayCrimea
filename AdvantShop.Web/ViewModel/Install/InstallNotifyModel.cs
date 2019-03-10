using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace AdvantShop.ViewModel.Install
{
    public class InstallNotifyModel : IValidatableObject
    {
        public string EmailRegReport { get; set; }
        public string OrderEmail { get; set; }
        public string EmailProductDiscuss { get; set; }
        public string FeedbackEmail { get; set; }
        public string EmailSmtp { get; set; }
        public string EmailLogin { get; set; }
        public string EmailPassword { get; set; }
        public string EmailPort { get; set; }
        public string Email { get; set; }
        public bool EnableSsl { get; set; }

        public string BackUrl { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(EmailRegReport) || !EmailRegReport.Contains("@") ||
                string.IsNullOrWhiteSpace(OrderEmail) || !OrderEmail.Contains("@") ||
                string.IsNullOrWhiteSpace(EmailProductDiscuss) || !EmailProductDiscuss.Contains("@") ||
                string.IsNullOrWhiteSpace(FeedbackEmail) || !FeedbackEmail.Contains("@") ||
                string.IsNullOrWhiteSpace(Email) || !Email.Contains("@") ||
                string.IsNullOrWhiteSpace(EmailSmtp) ||
                string.IsNullOrWhiteSpace(EmailLogin) ||
                string.IsNullOrWhiteSpace(EmailPassword) ||
                string.IsNullOrWhiteSpace(EmailPort))
            {
                yield return new ValidationResult("Все поля обязательны для заполнения", new[] {"Notify"});
            }

        }
    }
}