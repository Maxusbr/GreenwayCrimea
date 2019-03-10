using System;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Mails;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Mails
{
    public class SendTestMessageHandler
    {
        public bool Execute(SendTestMessageModel model)
        {
            var password = model.Password != model.PasswordCompare ? model.Password : SettingsMail.Password;

            password =
                password == string.Empty || password == SettingsMail.SIX_STARS
                    ? SettingsMail.InternalDataP
                    : password;

            var login = model.Login == string.Empty || model.Login == SettingsMail.SIX_STARS ? SettingsMail.InternalDataL : SettingsMail.Login;

            string error;
            var result = SendMail.SendMailThreadStringResult(Guid.Empty, model.To, model.Subject, model.Body, false,
                                                                model.SMTP, 
                                                                login, password, 
                                                                model.Port, 
                                                                model.From, model.SSL, model.SenderName,
                                                                out error, null, 1);

            if (result)
            {
                if (model.From != null && !model.From.Contains("advantshop"))
                    TrialService.TrackEvent(TrialEvents.SendTestEmail, string.Empty);
                
                return true;
            }

            throw new BlException(error);
        }
    }
}
