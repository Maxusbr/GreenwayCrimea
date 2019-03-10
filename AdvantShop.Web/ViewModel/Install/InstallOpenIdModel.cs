using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace AdvantShop.ViewModel.Install
{
    public class InstallOpenIdModel : IValidatableObject
    {
        public string BackUrl { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public string PassRepeat { get; set; }
        public bool ShowYandex { get; set; }
        public bool Yandex { get; set; }
        public bool ShowMailRu { get; set; }
        public bool MailRu { get; set; }
        public bool ShowGoogle { get; set; }
        public bool Google { get; set; }
        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }
        public bool ShowFacebook { get; set; }
        public bool Facebook { get; set; }
        public string FacebookClientId { get; set; }
        public string FacebookApplicationSecret { get; set; }
        public bool ShowVk { get; set; }
        public bool Vk { get; set; }
        public string VkAppId { get; set; }
        public string VkSecret { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                yield return
                    new ValidationResult(Resource.Install_UserContols_OpenidParagrafView_NeedLogin,
                        new[] { "Login" });
            }

            if (string.IsNullOrWhiteSpace(Pass) || string.IsNullOrWhiteSpace(PassRepeat) || Pass != PassRepeat)
            {
                yield return
                    new ValidationResult(Resource.Install_UserContols_OpenidParagrafView_NeedLogin,
                        new[] { "Login" });
            }

            if (Facebook)
            {
                if (string.IsNullOrWhiteSpace(FacebookApplicationSecret) || string.IsNullOrWhiteSpace(FacebookClientId))
                    yield return
                        new ValidationResult("Поля \"Client Id\" и \"Application Secret\" обязательны для заполнения",
                            new[] {"Facebook"});
            }

            if (Vk)
            {
                if (string.IsNullOrWhiteSpace(VkAppId) || string.IsNullOrWhiteSpace(VkSecret))
                    yield return
                        new ValidationResult("Поля \"App Id\" и \"Secret\" обязательны для заполнения",
                            new[] {"Vk"});
            }
        }
    }
}