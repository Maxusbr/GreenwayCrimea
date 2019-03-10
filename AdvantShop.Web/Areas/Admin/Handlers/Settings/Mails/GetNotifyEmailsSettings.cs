using System;
using AdvantShop.Configuration;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Mails
{
    public class GetNotifyEmailsSettings
    {
        public MailSettingsModel Execute()
        {
            var model = new MailSettingsModel()
            {
                EmailForOrders = SettingsMail.EmailForOrders,
                EmailForProductDiscuss = SettingsMail.EmailForProductDiscuss,
                EmailForRegReport = SettingsMail.EmailForRegReport,
                EmailForFeedback = SettingsMail.EmailForFeedback,
                EmailForLeads = SettingsMail.EmailForLeads,

                SMTP = SettingsMail.SMTP,
                Port = SettingsMail.Port,
                From = SettingsMail.From,
                Login = SettingsMail.Login,
                Password = SettingsMail.Password,
                PasswordCompare = DateTime.Now.ToString("ddhhmmss"),
                SSL = SettingsMail.SSL,
                SenderName = SettingsMail.SenderName,

                ImapHost = SettingsMail.ImapHost ?? (SettingsMail.SMTP != null && SettingsMail.SMTP.StartsWith("smtp.") ? SettingsMail.SMTP.Replace("smtp.", "imap.") : ""),
                ImapPort = SettingsMail.ImapPort != 0 ? SettingsMail.ImapPort : 993,
            };

            return model;
        }
    }
}
