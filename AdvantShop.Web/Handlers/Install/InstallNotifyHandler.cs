using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.ViewModel.Install;

namespace AdvantShop.Handlers.Install
{
    public class InstallNotifyHandler
    {
        public InstallNotifyModel Get()
        {
            var model = new InstallNotifyModel()
            {
                OrderEmail = SettingsMail.EmailForOrders,
                EmailProductDiscuss = SettingsMail.EmailForProductDiscuss,
                EmailRegReport = SettingsMail.EmailForRegReport,
                FeedbackEmail = SettingsMail.EmailForFeedback,

                EmailSmtp = SettingsMail.SMTP,
                EmailLogin = SettingsMail.Login,
                EmailPassword = SettingsMail.Password,
                Email = SettingsMail.From,
                EnableSsl = SettingsMail.SSL,
                EmailPort = SettingsMail.Port.ToString(),
            };

            return model;
        }

        public void Update(InstallNotifyModel model)
        {
            SettingsMail.EmailForOrders = model.OrderEmail;
            SettingsMail.EmailForProductDiscuss = model.EmailProductDiscuss;
            SettingsMail.EmailForRegReport = model.EmailRegReport;
            SettingsMail.EmailForFeedback = model.FeedbackEmail;

            SettingsMail.SMTP = model.EmailSmtp;
            SettingsMail.Login = model.EmailLogin;
            SettingsMail.Password = model.EmailPassword;
            SettingsMail.From = model.Email;
            SettingsMail.Port = model.EmailPort.TryParseInt();
            SettingsMail.SSL = model.EnableSsl;
        }
    }
}