//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Mails;
using AdvantShop.Customers;
using AdvantShop.Modules;

namespace AdvantShop.Module.MailChimp
{
    public class MailChimp : ISendMails, IModule
    {
        public static string ModuleID
        {
            get { return "MailChimp"; }
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "MailChimp";

                    case "en":
                        return "MailChimp";

                    default:
                        return "MailChimp";
                }
            }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleID; }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new MailChimpSettingsControl() }; }
        }

        public bool HasSettings
        {
            get { return true; }
        }

        public bool CheckAlive()
        {
            return ModulesRepository.IsInstallModule(ModuleID);
        }

        public bool InstallModule()
        {
            //ModuleSettingsProvider.SetSettingValue("MailChimpId", string.Empty, ModuleID);
            //ModuleSettingsProvider.SetSettingValue("MailChimpFromName", string.Empty, ModuleID);
            //ModuleSettingsProvider.SetSettingValue("MailChimpFromEmail", string.Empty, ModuleID);
            //ModuleSettingsProvider.SetSettingValue("MailChimpRegUsersList", string.Empty, ModuleID);
            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("MailChimpId", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("MailChimpFromName", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("MailChimpFromEmail", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("MailChimpRegUsersList", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("MailChimpOrderCustomer", ModuleID);

            return true;
        }

        public bool UpdateModule()
        {
            //ModuleSettingsProvider.SetSettingValue("MailChimpOrderCustomer", string.Empty, ModuleID);
            return true;
        }

        private class MailChimpSettingsControl : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Настройки";

                        case "en":
                            return "Settings";

                        default:
                            return "Settings";
                    }
                }
            }

            public string File
            {
                get { return "MailChimpModule.ascx"; }
            }

            #endregion
        }

        public bool SendMails(string subject, string message, EMailRecipientType recipientType)
        {
            bool result = false;

            if (recipientType.HasFlag(EMailRecipientType.Subscriber) && !string.IsNullOrEmpty(MailChimpSettings.RegUsersList))
                result |= MailChimpService.SendMail(MailChimpSettings.ApiKey, MailChimpSettings.RegUsersList, subject,
                    MailChimpSettings.FromEmail, MailChimpSettings.FromName, string.Empty, message);

            if (recipientType.HasFlag(EMailRecipientType.OrderCustomer) && !string.IsNullOrEmpty(MailChimpSettings.OrderCustomersList))
                result |= MailChimpService.SendMail(MailChimpSettings.ApiKey, MailChimpSettings.OrderCustomersList, subject,
                    MailChimpSettings.FromEmail, MailChimpSettings.FromName, string.Empty, message);

            return result;
        }


        public void SubscribeEmail(ISubscriber subscriber)
        {
            if (subscriber.CustomerType == EMailRecipientType.Subscriber || subscriber.CustomerType == EMailRecipientType.None)
            {
                if (!string.IsNullOrEmpty(MailChimpSettings.RegUsersList))
                    MailChimpService.SubscribeListMembers(MailChimpSettings.ApiKey, MailChimpSettings.RegUsersList,
                        new List<ISubscriber> { subscriber });

            }
            else if (subscriber.CustomerType == EMailRecipientType.OrderCustomer)
            {
                if (!string.IsNullOrEmpty(MailChimpSettings.OrderCustomersList))
                    MailChimpService.SubscribeListMembers(MailChimpSettings.ApiKey, MailChimpSettings.OrderCustomersList,
                        new List<ISubscriber> { subscriber });
            }

        }

        public void UnsubscribeEmail(string email)
        {
            if (!string.IsNullOrEmpty(MailChimpSettings.RegUsersList))
                MailChimpService.UnsubscribeListMembers(MailChimpSettings.ApiKey, MailChimpSettings.RegUsersList,
                    new MailChimpListMembers
                    {
                        Total = 1,
                        Data = new List<MailChimpListMember> { new MailChimpListMember { email = email } }
                    });
        }
    }
}