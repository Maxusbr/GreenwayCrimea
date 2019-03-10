using AdvantShop.Core;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Web.Admin.Models.SmsTemplates;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.SmsTemplates
{
    public class AddEditSmsTemplateHandler : AbstractCommandHandler<bool>
    {
        private readonly SmsTemplateModel _model;
        private SmsTemplate _smsTemplate;

        public AddEditSmsTemplateHandler(SmsTemplateModel model)
        {
            _model = model;
        }

        protected override void Load()
        {
            _smsTemplate = SmsTemplateService.Get(_model.SmsTypeId);
        }
        protected override void Validate()
        {
            if (_model.IsNew)
            {
                if (_smsTemplate != null)
                    throw new BlException(T("Admin.SmsTemplates.Error.TemplateExist"));
            }
        }

        protected override bool Handle()
        {
            if (_model.IsNew)
            {
                var obj = new SmsTemplate();
                obj.SmsTypeId = _model.SmsTypeId;
                obj.SmsBody = _model.SmsBody;
                SmsTemplateService.Add(obj);
            }
            else
            {
                _smsTemplate.SmsBody = _model.SmsBody;
                SmsTemplateService.Update(_smsTemplate);
            }
            return true;
        }
    }
}
