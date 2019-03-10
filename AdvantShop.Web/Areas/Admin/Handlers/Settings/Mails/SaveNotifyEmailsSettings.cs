using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Mails
{
    public class SaveNotifyEmailsSettings
    {
        private MailSettingsModel _model;

        public SaveNotifyEmailsSettings(MailSettingsModel model)
        {
            _model = model;
        }

        public void Execute()
        {
            SettingsMail.EmailForOrders = _model.EmailForOrders.DefaultOrEmpty();
            SettingsMail.EmailForProductDiscuss = _model.EmailForProductDiscuss.DefaultOrEmpty();
            SettingsMail.EmailForRegReport = _model.EmailForRegReport.DefaultOrEmpty();
            SettingsMail.EmailForFeedback = _model.EmailForFeedback.DefaultOrEmpty();
            SettingsMail.EmailForLeads = _model.EmailForLeads.DefaultOrEmpty();

            SettingsMail.SMTP = _model.SMTP.DefaultOrEmpty();
            SettingsMail.Port = _model.Port;
            SettingsMail.From = _model.From.DefaultOrEmpty();
            SettingsMail.Login = _model.Login.DefaultOrEmpty();

            if (_model.Password != _model.PasswordCompare)
                SettingsMail.Password = _model.Password.DefaultOrEmpty();

            SettingsMail.SSL = _model.SSL;
            SettingsMail.SenderName = _model.SenderName.DefaultOrEmpty();

            SettingsMail.ImapHost = _model.ImapHost.DefaultOrEmpty();
            SettingsMail.ImapPort = _model.ImapPort;
        }

    }
}
