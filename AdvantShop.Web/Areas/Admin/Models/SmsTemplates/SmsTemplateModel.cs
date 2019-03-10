using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Bonuses.Sms;
using AdvantShop.Core.Services.Bonuses.Sms.Template;

namespace AdvantShop.Web.Admin.Models.SmsTemplates
{
    public class SmsTemplateModel : IValidatableObject
    {
        public ESmsType SmsTypeId { get; set; }
        public string SmsBody { get; set; }
        public bool IsNew { get; set; }


        public List<SelectListItem> SmsTypes { get; set; }
        public List<string> AvalibleVarible { get; set; }

        public SmsTemplateModel()
        {
        }

        public SmsTemplateModel(ESmsType type)
        {
            SmsTypeId = type;
            SmsTypes = EnumExtensions.ToDictionary<ESmsType>().Where(x => x.Key != ESmsType.None).Select(x => new SelectListItem { Text = x.Key.Localize(), Value = x.Value }).ToList();
            AvalibleVarible = BaseSmsTemplate.AvalibleVarible(SmsTypeId);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SmsService.Valid(SmsBody, SmsTypeId))
                yield return new ValidationResult("Смс-сообщение содержит не корректные переменные");
        }
    }
}
